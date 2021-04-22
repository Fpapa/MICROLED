using System.Configuration;

namespace ADE02Service.Config
{
    public static class Configuration
    {
        public static string StringConexao()
           => ConfigurationManager.ConnectionStrings["StringConexaoMSSQL"].ToString();
    }
}
