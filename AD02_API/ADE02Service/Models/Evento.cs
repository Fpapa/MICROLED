using System;

namespace ADE02Service.Models
{
    /// <summary>
    /// Informações de eventos 
    /// </summary>
    public class Evento
    {
        public long Id { get; set; }

        public int EventoId { get; set; }

        public string JSON { get; set; }

        public DateTime? DataEnvio { get; set; }

        public DateTime? DataRetorno { get; set; }

        public string JSONRetorno { get; set; }

        public string MensagemRetorno { get; set; }

        public long CodigoRetorno { get; set; }

        public long IdAnterior { get; set; }
    }
}
