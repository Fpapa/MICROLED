using ADE02Service.Config;
using ADE02Service.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ADE02Service.Data
{
    public class EmpresasRepositorio
    {
        public IEnumerable<Empresa> ObterEmpresas()
        {
            using (SqlConnection con = new SqlConnection(Configuration.StringConexao()))
            {
                return con.Query<Empresa>("SELECT Id, Empresa As Descricao, Tabela, CpfCertificado FROM [Tb_empresas] Order By Id");
            }
        }
    }
}
