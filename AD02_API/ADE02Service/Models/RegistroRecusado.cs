namespace ADE02Service.Models
{
    public class RegistroRecusado
    {
        public RegistroRecusado(long id, string tabela, string jsonRetorno, int codigoRetorno, string mensagemRetorno)
        {
            Id = id;
            Tabela = tabela;
            JsonRetorno = jsonRetorno;
            CodigoRetorno = codigoRetorno;
            MensagemRetorno = mensagemRetorno;
        }

        public long Id { get; set; }

        public string Tabela { get; set; }

        public string JsonRetorno { get; set; }

        public int CodigoRetorno { get; set; }

        public string MensagemRetorno { get; set; }
    }
}
