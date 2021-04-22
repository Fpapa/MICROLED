namespace AD02Monitor.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string CPF { get; set; }

        public string Nome { get; set; }

        public string Senha { get; set; }

        public bool Ativo { get; set; }

        public bool Autenticar(string cpf, string senha)
        {
            return this.CPF == cpf && this.Senha == senha;
        }
    }
}