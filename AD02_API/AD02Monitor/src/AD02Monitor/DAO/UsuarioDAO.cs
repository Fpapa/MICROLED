using System.Data.SqlClient;
using System.Linq;
using Dapper;
using AD02Monitor.Config;
using AD02Monitor.Models;

namespace AD02Monitor
{
    public class UsuarioDAO
    {          
        public Usuario ObterUsuarioPorCpf(string cpf)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Usuario>(@"
                    SELECT Id, Cpf, Senha, Ativo, Nome FROM [dbo].[Tb_Login] WHERE Cpf = @uCpf AND Ativo = 1", new { uCpf = cpf }).FirstOrDefault();
            }
        }
    }
}