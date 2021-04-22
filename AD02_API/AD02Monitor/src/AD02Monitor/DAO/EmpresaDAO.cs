using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using AD02Monitor.Config;
using AD02Monitor.Models;
using System.Linq;

namespace AD02Monitor.DAO
{
    public class EmpresaDAO
    {      
        public IEnumerable<string> ObterEmpresasMemorando()
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<string>(@"SELECT DISTINCT EMPRESA FROM TB_DUE_NF WHERE EMPRESA IS NOT NULL");
            }
        }

        public IEnumerable<Empresa> ObterEmpresasPorUsuarioCpf(string cpf)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Empresa>(@"
                    SELECT 
	                    A.Id, 
	                    A.Empresa as Descricao 
                    FROM 
	                    Tb_empresas A 
                    INNER JOIN 
	                    Tb_Login_Empresa B ON A.Id = B.EmpresaId 
                    INNER JOIN 
	                    Tb_Login C ON B.UsuarioId = C.Id 
                    WHERE 
	                    C.Cpf = @uCpf", new { uCpf = cpf });
            }
        }

        public Empresa ObterEmpresaPorId(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Empresa>(@"
                    SELECT 
	                    Id,
                        Empresa As Descricao,
                        Tabela,
                        LogoUrl
                    FROM 
	                    Tb_empresas 
                    WHERE
                        Id = @eId", new { eId = id }).FirstOrDefault();
            }
        }
    }
}