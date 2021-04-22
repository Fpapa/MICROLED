using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ADE02Service.Services
{
    public class CertificadoService
    {
        public static IEnumerable<X509Certificate2> ListarCertificadosInstalados(StoreLocation storeLocation)
        {
            var stores = new X509Store(StoreName.My, storeLocation);

            stores.Open(OpenFlags.ReadOnly);

            var certificadosInstalados = stores.Certificates;

            certificadosInstalados.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            certificadosInstalados.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);

            stores.Close();

            foreach (X509Certificate2 certificado in certificadosInstalados)
                yield return certificado;
        }

        public static X509Certificate2 ObterCertificado(string cpf)
        {
            var certificado = ListarCertificadosInstalados(StoreLocation.LocalMachine)
                .FirstOrDefault(a => a.SubjectName.Name.Contains(cpf));

            if (certificado == null)
            {
                certificado = ListarCertificadosInstalados(StoreLocation.CurrentUser)
                    .FirstOrDefault(a => a.SubjectName.Name.Contains(cpf));
            }

            if (certificado == null)
                throw new Exception($"Certificado Digital de CPF {cpf} não encontrado");

            return certificado;
        }
    }
}
