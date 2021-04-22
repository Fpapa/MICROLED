using ADE02Service.Config;
using ADE02Service.Models;
using Dapper;
using System.Data.SqlClient;
using System.Linq;

namespace ADE02Service.Data
{
    public class ParametrosRepositorio
	{
        public Parametros ObterParametros()
        {
            using (SqlConnection con = new SqlConnection(Configuration.StringConexao()))
            {
                return con.Query<Parametros>("SELECT NumeroTentativas, IntervaloEmMinutos FROM Tb_Parametros").FirstOrDefault();
            }
        }
	}
}
