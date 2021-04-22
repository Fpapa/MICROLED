namespace ADE02Service.Models
{
    /// <summary>
    /// Informações de empresas habilitadas para envio
    /// </summary>
    public class Empresa
    {
        public long Id { get; set; }

        public string Descricao { get; set; }

        public string Tabela { get; set; }

        public string CpfCertificado { get; set; }
    }
}
