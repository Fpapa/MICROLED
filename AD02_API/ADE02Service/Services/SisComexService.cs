using ADE02Service.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ADE02Service.Services
{
    public class SisComexService
    {
        private static readonly string _perfilSiscomex = ConfigurationManager.AppSettings["PerfilSiscomex"].ToString();

        private static readonly string _urlSiscomexAutenticacao = ConfigurationManager.AppSettings["UrlSiscomexAutenticacao"].ToString();

        private static readonly string _urlSiscomex = ConfigurationManager.AppSettings["UrlSiscomex"].ToString();

        private static readonly HttpClient _httpClient;
        private static readonly WebRequestHandler _handler;

        static SisComexService()
        {
            if (_httpClient == null)
            {
                _handler = new WebRequestHandler();
                _httpClient = new HttpClient(_handler);

                _httpClient.Timeout = new TimeSpan(0, 5, 00);
            }
        }

        private static bool RemoteCertificateValidate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        public static async Task<SiscomexToken> Autenticar(string cpfCertificado)
        {
            var token = new SiscomexToken();

            using (var handler = new WebRequestHandler())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                handler.ClientCertificates.Add(CertificadoService.ObterCertificado(cpfCertificado));
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;

                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("Role-Type", _perfilSiscomex);

                    var request = new HttpRequestMessage(HttpMethod.Post, new Uri(_urlSiscomexAutenticacao + "/portal/api/autenticar"));
                    var response = await client.SendAsync(request);

                    IEnumerable<string> valor;

                    if (response.Headers.TryGetValues("set-token", out valor))
                        token.SetToken = valor.FirstOrDefault();

                    if (response.Headers.TryGetValues("x-csrf-token", out valor))
                        token.CsrfToken = valor.FirstOrDefault();

                    if (response.Headers.TryGetValues("x-csrf-expiration", out valor))
                        token.CsrfExpiration = valor.FirstOrDefault();

                    return token;
                }
            }
        }

        public static async Task<HttpResponseMessage> CriarRequestPost(string url, SiscomexToken token, string json, string cpfCertificado)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            _handler.ClientCertificates.Add(CertificadoService.ObterCertificado(cpfCertificado));
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;

            var headers = new Dictionary<string, string>
            {
                {"Authorization", token.SetToken},
                {"x-csrf-token", token.CsrfToken}
            };

            _httpClient.DefaultRequestHeaders.Clear();

            foreach (var header in headers)
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);

            using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await _httpClient.PostAsync(new Uri(string.Concat(_urlSiscomex, url)), stringContent);
            }
        }
    }
}
