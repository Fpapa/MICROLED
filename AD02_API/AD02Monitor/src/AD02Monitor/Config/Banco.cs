using System.Configuration;

namespace AD02Monitor.Config
{
    public class Banco
    {
        public static string StringConexao()
        {
            return ConfigurationManager.ConnectionStrings["AD02"].ConnectionString;
        }
    }
}