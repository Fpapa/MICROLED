using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using AD02Monitor.Config;
using AD02Monitor.Models;
using System.Linq;

namespace AD02Monitor.DAO
{
    public class ConsultaDAO
    {
        private readonly string _baseCliente;

        public ConsultaDAO(string baseCliente)
        {
            _baseCliente = baseCliente;
        }

        public IEnumerable<Envio> ObterListaEnvios(string filtro = "")
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
				var limitador = filtro.Length == 0 ? " TOP 1000 " : "";

                return con.Query<Envio>($@"
					SELECT 
						{limitador}
						Id, 
						Evento As EventoId,
							CASE 
								WHEN Evento = 1  THEN 'Armazenamento de Lote' 
								WHEN Evento = 2  THEN 'Troca de Navio' 
								WHEN Evento = 3  THEN 'Avarias/Extravios de Lote' 
								WHEN Evento = 4  THEN 'Acesso de Pessoas' 
								WHEN Evento = 5  THEN 'Acesso de Veículos' 
								WHEN Evento = 6  THEN 'Embarque/Desembarque de Navio' 
								WHEN Evento = 7  THEN 'Credenciamento de Pessoas' 
								WHEN Evento = 8  THEN 'Credenciamento de Veículos'
								WHEN Evento = 9  THEN 'Geração de Lote'
								WHEN Evento = 10 THEN 'Pesagem de Veículos/Cargas'
								WHEN Evento = 11 THEN 'Posição do Contêiner'
							END As Evento,
						Envio_JSON As Json,
						CONVERT(VARCHAR, Envio_Dth, 103) + ' ' + CONVERT(VARCHAR(10), Envio_Dth, 108) As DataEnvio,
						Retorno_Codigo as CodigoRetorno,
						Protocolo,
						Tentativas,
						CASE WHEN Envio_Dth IS NULL AND Retorno_Codigo IS NULL AND ISNULL(Tentativas,0) = 0 THEN 1 ELSE 0 END Pendente,
						CASE WHEN Envio_Dth IS NOT NULL AND Retorno_Codigo IS NULL AND ISNULL(Tentativas,0) > 0 THEN 1 ELSE 0 END EnvioComFalhas,
						CASE WHEN ISNULL(LEN(Protocolo),0) = 0 AND Retorno_Codigo IS NOT NULL AND Retorno_Codigo <> 201 THEN 1 ELSE 0 END ComErro,
						CASE WHEN ISNULL(LEN(Protocolo),0) > 0 AND ISNULL(Retorno_Codigo,0) = 201 THEN 1 ELSE 0 END Enviado
					FROM
						[bd_ad_02].[dbo].{_baseCliente}
					WHERE
						Id_Anterior IS NULL {filtro}
					ORDER BY
						(CASE WHEN [Envio_dth] IS NULL THEN 0 ELSE 1 END), [Envio_dth] DESC");
            }
        }

		public IEnumerable<Envio> ObterListaEnviosRecusados(int idAnterior)
		{
			using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
			{
				return con.Query<Envio>($@"
					SELECT 
						Id, 
						CONVERT(VARCHAR, Retorno_dth, 103) + ' ' + CONVERT(VARCHAR(10), Retorno_dth, 108) As DataRetorno,
						Retorno_MSG MensagemRetorno,
						Retorno_codigo As CodigoRetorno,
						Protocolo
					FROM
						[bd_ad_02].[dbo].{_baseCliente}
					WHERE
						Id_anterior = {idAnterior}
					ORDER BY
						Retorno_dth");
			}
		}

		public Totais ObterTotais()
		{
			using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
			{				
				return con.Query<Totais>($@"
					SELECT 
						(SELECT COUNT(1) FROM [bd_ad_02].[dbo].{_baseCliente} WHERE Envio_Dth IS NULL AND Retorno_Codigo IS NULL AND ISNULL(Tentativas,0) = 0 AND Id_Anterior IS NULL) As Pendentes,
						(SELECT COUNT(1) FROM [bd_ad_02].[dbo].{_baseCliente} WHERE Envio_Dth IS NOT NULL AND Retorno_Codigo IS NULL AND ISNULL(Tentativas,0) > 0) As ComErro,
						(SELECT COUNT(1) FROM [bd_ad_02].[dbo].{_baseCliente} WHERE ISNULL(LEN(Protocolo),0) > 0 AND ISNULL(Retorno_Codigo,0) = 201 AND Id_Anterior IS NULL) As Enviados,
						(SELECT COUNT(1) FROM [bd_ad_02].[dbo].{_baseCliente} WHERE Id_Anterior IS NULL) As Total").FirstOrDefault();
			}
		}
	}
}