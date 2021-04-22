using ADE02Service.Enums;
using System.Collections.Generic;
using System.Linq;

namespace ADE02Service.Config
{
    public class EndPointsAD02
    {
        public static string ObterEndPoint(int evento)
        {
            var endPoints = new Dictionary<Eventos, string>
            {
                {Eventos.ArmazenamentoLote, "/armazenamento-lote"},
                {Eventos.TrocaNavio, "/atribuicao-troca-navio"},
                {Eventos.AvariaExtravioLote, "/avaria-extravio-lote"},
                {Eventos.AcessoDePessoa, "/acesso-pessoas"},
                {Eventos.AcessoDeVeiculo, "/acesso-veiculos"},
                {Eventos.EmbarqueDesembarqueDeNavio, "/embarque-desembarque-navios"},
                {Eventos.CredenciamentoDePessoas, "/credenciamento-pessoas"},
                {Eventos.CredenciamentoDeVeiculos, "/credenciamento-veiculos"},
                {Eventos.GeracaoDeLote, "/geracao-lotes"},
                {Eventos.PesagemDeVeiculoCarga, "/pesagem-veiculos-cargas"},
                {Eventos.PosicaoDoConteiner, "​/posicao-conteiner"},
            };

            return endPoints.FirstOrDefault(c => c.Key == (Eventos)evento).Value ?? string.Empty;
        }
    }
}
