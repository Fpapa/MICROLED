using System.ComponentModel.DataAnnotations;

namespace ADE02Service.Enums
{
    public enum Eventos
    {
        [Display(Name = "Armazenamento de Lote")]
        ArmazenamentoLote = 1,
        [Display(Name = "Troca de Navio")]
        TrocaNavio,
        [Display(Name = "Avarias/Extravios de Lote")]
        AvariaExtravioLote,
        [Display(Name = "Acesso de Pessoas")]
        AcessoDePessoa,
        [Display(Name = "Acesso de Veículos")]
        AcessoDeVeiculo,
        [Display(Name = "Embarque/Desembarque de Navio")]
        EmbarqueDesembarqueDeNavio,
        [Display(Name = "Credenciamento de Pessoas")]
        CredenciamentoDePessoas,
        [Display(Name = "Credenciamento de Veículos")]
        CredenciamentoDeVeiculos,
        [Display(Name = "Geração de Lote")]
        GeracaoDeLote,
        [Display(Name = "Pesagem de Veículos/Cargas")]
        PesagemDeVeiculoCarga,
        [Display(Name = "Posição do Contêiner")]
        PosicaoDoConteiner
    }
}
