using ADE02Service.Config;
using ADE02Service.Data;
using ADE02Service.Enums;
using ADE02Service.Extensions;
using ADE02Service.Helpers;
using ADE02Service.Models;
using ADE02Service.Models.Responses;
using ADE02Service.Services;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace ADE02Service
{
    public partial class Worker : ServiceBase
    {
        private readonly EmpresasRepositorio _empresasRepositorio;
        private readonly EventosRepositorio _eventosRepositorio;
        private readonly ParametrosRepositorio _parametrosRepositorio;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly int _numeroTentativas = 3;
        private readonly int _intervaloEmMilisegundos = 5;

        public Worker()
        {
            InitializeComponent();

            _empresasRepositorio = new EmpresasRepositorio();
            _eventosRepositorio = new EventosRepositorio();
            _parametrosRepositorio = new ParametrosRepositorio();

            var parametros = _parametrosRepositorio.ObterParametros();

            if (parametros != null)
            {
                _numeroTentativas = parametros.NumeroTentativas;
                _intervaloEmMilisegundos = parametros.IntervaloEmMinutos * 60000;
            }
        }

        public async Task Processar(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var empresas = ObterEmpresas();

                    foreach (var empresa in empresas)
                    {
                        var fila = ObterFilaDeEventos(empresa, _numeroTentativas);

                        foreach (var itemFila in fila)
                        {
                            SiscomexToken token = await ObterToken(empresa);

                            if (token.Valido() == false)
                            {
                                _logger.Info($"[SISTEMA] - Token inválido. Pulando para o próximo registro...");

                                continue;
                            }

                            var endPoint = EndPointsAD02.ObterEndPoint(itemFila.EventoId);

                            if (string.IsNullOrEmpty(endPoint))
                            {
                                _logger.Info($"[SISTEMA] - End-Point não informado. Pulando para o próximo registro...");

                                continue;
                            }

                            await ProcessarItemFila(endPoint, empresa, itemFila, token);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Info($"[ERRO] - Erro no processamento: {ex.ToString()}");
                }

                await Task.Delay(_intervaloEmMilisegundos, cancellationToken);
            }
        }

        private async Task ProcessarItemFila(string endPoint, Empresa empresa, Evento itemFila, SiscomexToken token)
        {
            var descricaoEvento = ((Eventos)itemFila.EventoId).ToName();

            _logger.Info($"[REQUEST] - Iniciando envio para o ID: {itemFila.Id} / Evento {descricaoEvento}...");

            var response = await SisComexService
                .CriarRequestPost(endPoint, token, itemFila.JSON, empresa.CpfCertificado);

            if (response.IsSuccessStatusCode)
                _logger.Info($"[RESPONSE] - Registro recebido com sucesso - StatusCode: {response.StatusCode} ");
            else
                _logger.Info($"[RESPONSE] - Registro recebido com falhas - StatusCode: {response.StatusCode} ");

            var jsonRetorno = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(jsonRetorno))
                _logger.Info($"[RESPONSE] - JSON de retorno foi recebido em branco");

            if (response.StatusCode == HttpStatusCode.Created)
            {                
                RetryHelper.RetryOnException(2, () =>
                {
                    try
                    {
                        _logger.Info($"[MSSQL] - Atualizando registro {itemFila.Id} na fila...");

                        var retornoSiscomex = DeserializarJson<OkResponse>(jsonRetorno);

                        var linhasAfetadas = _eventosRepositorio.MarcarComoProcessado(
                            new RegistroAceito(itemFila.Id, empresa.Tabela, jsonRetorno, (int)response.StatusCode, retornoSiscomex?.protocolo));

                        _logger.Info($"[MSSQL] - {linhasAfetadas} linha(s) afetada(s)...");
                    }
                    catch (Exception ex)
                    {
                        _logger.Info($"[ERRO] - Falha ao atualizar o registro da fila. Realizando uma nova tentativa. Detalhes: {ex.Message}");

                        throw;
                    }
                });
            }
            else
            {
                RetryHelper.RetryOnException(2, () =>
                {
                    try
                    {
                        var mensagensValidacao = ObterMensagensFalha(response.StatusCode, jsonRetorno);

                        _logger.Info($"[RESPONSE] - Erros: {mensagensValidacao}");

                        _logger.Info($"[MSSQL] - Recolocando registro na fila com ID anterior {itemFila.Id}...");

                        var linhasAfetadas = _eventosRepositorio.RecolocarNaFila(
                            new RegistroRecusado(itemFila.Id, empresa.Tabela, jsonRetorno, (int)response.StatusCode, mensagensValidacao));

                        _logger.Info($"[MSSQL] - {linhasAfetadas} linha(s) afetada(s)...");
                    }
                    catch (Exception ex)
                    {
                        _logger.Info($"[ERRO] - Falha ao recolocar o registro na fila. Realizando uma nova tentativa. Detalhes: {ex.Message}");

                        throw;
                    }
                });
            }            
        }

        private string ObterMensagensFalha(HttpStatusCode statusCode, string jsonRetorno)
        {
            if (statusCode == HttpStatusCode.BadRequest)
            {
                var retorno = DeserializarJson<ErrosValidacaoResponse>(jsonRetorno);

                if (retorno != null)
                {
                    var listaValidacoes = retorno?.errosValidacao?.Select(c => c.detalhes)?.ToList() ?? new List<string> { };

                    return string.Join(";", listaValidacoes);
                }
            }

            if (statusCode == HttpStatusCode.InternalServerError || statusCode == HttpStatusCode.Forbidden)
            {
                var retorno = DeserializarJson<ErroResponse>(jsonRetorno);

                if (retorno != null)
                {
                    if (!string.IsNullOrEmpty(retorno.code) && !string.IsNullOrEmpty(retorno.message))
                        return $"{retorno?.code} - {retorno?.message}";

                    return "Erro inesperado do Siscomex";
                }
            }

            return jsonRetorno;
        }

        private async Task<SiscomexToken> ObterToken(Empresa empresa)
        {
            _logger.Info($"[REQUEST] - Obtendo Token para o Certificado {empresa.CpfCertificado}");

            try
            {
                var token = await SisComexService.Autenticar(empresa.CpfCertificado);

                if (token.Valido())
                {
                    _logger.Info($"[RESPONSE] - Token recebido com sucesso!");

                    return token;
                }
            }
            catch (Exception ex)
            {
                _logger.Info($"[ERRO] - Falha ao obter o Token - Detalhes: {ex.Message}");

                throw;
            }

            return new SiscomexToken();
        }

        private IEnumerable<Evento> ObterFilaDeEventos(Empresa empresa, int numeroTentativas)
        {
            var timeout = 1;

            IEnumerable<Evento> fila = null;

            RetryHelper.RetryOnException(timeout, () =>
            {
                try
                {
                    _logger.Info($"[MSSQL] - Obtendo lista de eventos para empresa {empresa.Descricao}/{empresa.Tabela}");

                    fila = _eventosRepositorio.ObterFilaEventos(empresa.Tabela, numeroTentativas);

                    _logger.Info($"[MSSQL] - {fila.Count()} evento(s) encontrado(s)");
                }
                catch (Exception ex)
                {
                    _logger.Info($"[ERRO] - Falha ao obter a lista de eventos. Detalhes: {ex.Message}");

                    throw;
                }
            });
          
            return fila;
        }

        private IEnumerable<Empresa> ObterEmpresas()
        {
            var timeout = 1;

            IEnumerable<Empresa> empresas = null;

            RetryHelper.RetryOnException(timeout, () =>
            {
                try
                {
                    _logger.Info("[MSSQL] - Obtendo lista de empresas...");

                    empresas = _empresasRepositorio.ObterEmpresas();

                    _logger.Info($"[MSSQL] - {empresas.Count()} empresa(s) encontrada(s)");
                }
                catch (Exception ex)
                {
                    _logger.Info($"[ERRO] - Falha ao obter a lista de empresas. Detalhes: {ex.Message}");

                    throw;
                }
            });

            return empresas;
        }

        private T DeserializarJson<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                _logger.Info($"[ERRO] - Falha ao deserializar o JSON de retorno - Entrada: {json} - Detalhes: {ex.Message}");

                throw;
            }
        }

        protected override void OnStart(string[] args)
        {
            _logger.Info("Serviço iniciado...");

            CancellationToken cancellationToken;
            Task backgroundTask;

            backgroundTask = Task.Factory.StartNew(() =>
                Processar(cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        protected override void OnStop()
        {
            _logger.Info("Serviço parado...");
        }
    }
}
