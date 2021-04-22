namespace ADE02Service.Models
{
    public class RegistroAceito
    {
        public RegistroAceito(long id, string tabela, string jsonRetorno, int codigoRetorno, string protocolo)
        {
            Id = id;
            Tabela = tabela;
            JsonRetorno = jsonRetorno;
            CodigoRetorno = codigoRetorno;
            Protocolo = protocolo;
        }

        public long Id { get; set; }

        public string Tabela { get; set; }

        public string JsonRetorno { get; set; }

        public int CodigoRetorno { get; set; }

        public string Protocolo { get; set; }
    }
}
