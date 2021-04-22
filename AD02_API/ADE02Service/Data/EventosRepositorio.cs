using ADE02Service.Config;
using ADE02Service.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ADE02Service.Data
{
    public class EventosRepositorio
    {
        public IEnumerable<Evento> ObterFilaEventos(string tabela, int numeroTentativas)
        {
            using (SqlConnection con = new SqlConnection(Configuration.StringConexao()))
            {
				var parametros = new DynamicParameters();
				parametros.Add(name: "NumeroTentativas", value: numeroTentativas, direction: ParameterDirection.Input);

				return con.Query<Evento>($@"
					SELECT 
						Id, 
						Evento As EventoId, 
						Envio_JSON As JSON, 
						Envio_DTH As DataEnvio, 
						Retorno_DTH As DataRetorno, 
						Retorno_JSON As JSONRetorno, 
						Retorno_MSG As MensagemRetorno, 
						Retorno_Codigo As CodigoRetorno, 
						Id_Anterior As IdAnterior
					FROM 
						{tabela} 
					WHERE 
						Retorno_DTH IS NULL
					AND
						ISNULL(Id_Anterior, 0) = 0
					AND
						ISNULL(Tentativas, 0) < @NumeroTentativas", parametros);
            }
        }

		public int RecolocarNaFila(RegistroRecusado registroRecusado)
		{
			using (SqlConnection con = new SqlConnection(Configuration.StringConexao()))
			{
				con.Open();

				using (var transaction = con.BeginTransaction())
				{
					var parametros = new DynamicParameters();

					parametros.Add(name: "JsonRetorno", value: registroRecusado.JsonRetorno, direction: ParameterDirection.Input);
					parametros.Add(name: "CodigoRetorno", value: registroRecusado.CodigoRetorno, direction: ParameterDirection.Input);
					parametros.Add(name: "MensagemRetorno", value: registroRecusado.MensagemRetorno, direction: ParameterDirection.Input);
					parametros.Add(name: "Id", value: registroRecusado.Id, direction: ParameterDirection.Input);

					try
					{
						con.Execute($@"UPDATE {registroRecusado.Tabela} SET Envio_dth = GetDate(), Tentativas = ISNULL(Tentativas, 0) + 1 WHERE Id = @Id", parametros, transaction);

						var linhas = con.Execute($@"INSERT INTO {registroRecusado.Tabela} (Evento, Envio_JSON, Envio_dth, Retorno_dth, Retorno_JSON, Retorno_MSG, Retorno_codigo, Id_anterior)
							SELECT Evento, Envio_JSON, Envio_Dth, GetDate(), @JSONRetorno, @MensagemRetorno, @CodigoRetorno, @Id FROM {registroRecusado.Tabela} WHERE Id = @Id", parametros, transaction);
					
						transaction.Commit();

						return linhas;
					}
					catch
					{
						transaction.Rollback();

						throw;
					}					
				}
			}
		}

		public int MarcarComoProcessado(RegistroAceito registroAceito)
		{
			using (SqlConnection con = new SqlConnection(Configuration.StringConexao()))
			{
				var parametros = new DynamicParameters();

				parametros.Add(name: "Id", value: registroAceito.Id, direction: ParameterDirection.Input);
				parametros.Add(name: "Protocolo", value: registroAceito.Protocolo, direction: ParameterDirection.Input);
				parametros.Add(name: "CodigoRetorno", value: registroAceito.CodigoRetorno, direction: ParameterDirection.Input);
				parametros.Add(name: "JsonRetorno", value: registroAceito.JsonRetorno, direction: ParameterDirection.Input);

                try
                {
					return con.Execute($@"UPDATE {registroAceito.Tabela} SET Envio_dth = GetDate(), Retorno_dth = GetDate(), Protocolo = @Protocolo, Retorno_codigo = @CodigoRetorno, Retorno_JSON = @JsonRetorno, Tentativas = ISNULL(Tentativas, 0) + 1 WHERE Id = @Id", parametros);
				}
                catch (Exception ex)
                {
                    throw;
                }
			}
		}
	}
}
