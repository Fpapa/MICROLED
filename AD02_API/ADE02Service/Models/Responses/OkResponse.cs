namespace ADE02Service.Models.Responses
{
    public class OkResponse
    {
        public Cabecalhorequisicao cabecalhoRequisicao { get; set; }
        public string dtHrTransmissao { get; set; }
        public string protocolo { get; set; }
    }

    public class Cabecalhorequisicao
    {
        public string tipoOperacao { get; set; }
        public string idEvento { get; set; }
        public string dtHrOcorrencia { get; set; }
        public string dtHrRegistro { get; set; }
        public string cpfOperOcor { get; set; }
        public string cpfOperReg { get; set; }
        public string protocoloEventoRetifCanc { get; set; }
        public bool contingencia { get; set; }
    }
}
