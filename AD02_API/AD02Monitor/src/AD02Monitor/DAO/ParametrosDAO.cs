using System.Data.SqlClient;
using System.Linq;
using Dapper;
using AD02Monitor.Config;
using AD02Monitor.Models;

namespace AD02Monitor.DAO
{
    public class ParametrosDAO
    {      
        public Parametros ObterParametros()
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Parametros>(@"SELECT MensagemEmailRedefinicaoSenha, ValidarAtributosCafe, EnvioRetificacaoSemServico FROM [dbo].[TB_Parametros]").FirstOrDefault();
            }
        }
    }
}