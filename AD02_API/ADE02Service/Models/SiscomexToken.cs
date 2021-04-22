using System;

namespace ADE02Service.Models
{
    /// <summary>
    /// Classe para obtenção do Token de acesso
    /// </summary>
    public class SiscomexToken
    {
        public string SetToken { get; set; }

        public string CsrfToken { get; set; }

        public string CsrfExpiration { get; set; }

        public bool Valido() => SetToken?.Length > 0 && CsrfToken?.Length > 0
                && TimeSpan.FromMilliseconds(Convert.ToDouble(CsrfExpiration ?? "0")).TotalMinutes > 0;
    }
}
