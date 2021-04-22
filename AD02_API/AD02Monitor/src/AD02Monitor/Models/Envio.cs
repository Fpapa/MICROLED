namespace AD02Monitor.Models
{
    public class Envio
    {
        public int Id { get; set; }

        public int EventoId { get; set; }

        public string Evento { get; set; }

        public string Json { get; set; }

        public string DataEnvio { get; set; }

        public string DataRetorno { get; set; }

        public string CodigoRetorno { get; set; }

        public string MensagemRetorno { get; set; }

        public string Protocolo { get; set; }

        public string Tentativas { get; set; }

        public bool Pendente { get; set; }

        public bool EnvioComFalhas { get; set; }

        public bool Enviado { get; set; }

        public bool ComErro { get; set; }
    }
}