namespace ADE02Service.Models.Responses
{
    public class ErrosValidacaoResponse
    {
        public Errosvalidacao[] errosValidacao { get; set; }
    }

    public class Errosvalidacao
    {
        public int codigo { get; set; }
        public string atributo { get; set; }
        public string detalhes { get; set; }
    }
}
