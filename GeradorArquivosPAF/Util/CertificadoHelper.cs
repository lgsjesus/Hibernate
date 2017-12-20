using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GeradorArquivosPAF.Util
{
    class CertificadoHelper
    {
        public enum TipoConsultaCertificado
        {
            PorNome,
            PorNroSerie,
        }
        public static X509Certificate2 Consultar(StoreName storeName, StoreLocation storeLocation, string valor, TipoConsultaCertificado tpConsultaTipoCertificado)
        {
            System.Security.Cryptography.X509Certificates.X509Certificate2 _X509Cert = new System.Security.Cryptography.X509Certificates.X509Certificate2();
           try
            {
                X509Store store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection collection2 = (X509Certificate2Collection)collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);

                LogHelper.GravarLog("StoreName=" + storeName.ToString() + " location=" + storeLocation.ToString());

                if (valor == "")
                {
                    //Exibe uma lista de certificado existentes na maquina do usuário


                    X509Certificate2Collection scollection =
                        X509Certificate2UI.SelectFromCollection(collection2, "Certificado(s) Digital(is) disponível(is)", "Selecione o Certificado Digital para uso no aplicativo", X509SelectionFlag.SingleSelection);
                    if (scollection.Count > 0)
                    {

                        return scollection[0];
                    }
                }
                else
                {
                    //Consulta com base no tipo de solicitação

                    X509Certificate2Collection scollection = null;
                    switch (tpConsultaTipoCertificado)
                    {
                        case TipoConsultaCertificado.PorNome:

                            scollection = (X509Certificate2Collection)collection2
                                .Find(X509FindType.FindBySubjectDistinguishedName, valor, false);
                            break;
                        case TipoConsultaCertificado.PorNroSerie:

                            LogHelper.GravarLog("Listar os certificados");

                            foreach (var cert in store.Certificates)
                            {
                               
                                string serial = cert.SerialNumber;
                                LogHelper.GravarLog("certificado:" + serial + ".valor passado:" + valor+".");

                                if (string.Compare(serial, valor) == 0)
                                    return cert;
                            }
                            break;
                    }
                   
                    if (scollection != null)
                    {
                        if (scollection.Count > 0)
                            return scollection[0];
                    }
                }
                store.Close();
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("Erro ao consultar o cetificado digital:{0}", ex.Message);
                LogHelper.GravarLog("Erro BUSCA CERTIFICADO " + " \n " + ex.StackTrace);
                throw new Exception(msg);
            }


            return null;
        }

        public static X509Certificate2 Consultar(string valor, StoreName storeName, StoreLocation storeLocation,
            TipoConsultaCertificado tpConsultaTipoCertificado)
        {
            System.Security.Cryptography.X509Certificates.X509Certificate2 _X509Cert = new System.Security.Cryptography.X509Certificates.X509Certificate2();

            try
            {
                X509Store store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection collection2 = (X509Certificate2Collection)collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);
                if (valor == "")
                {
                    //Exibe uma lista de certificado existentes na maquina do usuário
                    X509Certificate2Collection scollection =
                        X509Certificate2UI.SelectFromCollection(collection2, "Certificado(s) Digital(is) disponível(is)", "Selecione o Certificado Digital para uso no aplicativo", X509SelectionFlag.SingleSelection);
                    if (scollection.Count > 0)
                    {
                        return scollection[0];
                    }
                }
                else
                {
                    //Consulta com base no tipo de solicitação
                    X509Certificate2Collection scollection = null;
                    switch (tpConsultaTipoCertificado)
                    {
                        case TipoConsultaCertificado.PorNome:
                            scollection = (X509Certificate2Collection)collection2
                                .Find(X509FindType.FindBySubjectDistinguishedName, valor, false);
                            break;
                        case TipoConsultaCertificado.PorNroSerie:
                            scollection = (X509Certificate2Collection)collection2
                                .Find(X509FindType.FindBySerialNumber, valor, true);
                            break;
                    }
                    if (scollection != null)
                    {
                        if (scollection.Count > 0)
                            return scollection[0];
                    }
                }
                store.Close();
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("Erro ao consultar o cetificado digital:{0}", ex.Message);
                throw new Exception(msg);
            }


            return null;
        }

        /// <summary>
        /// Metodos para testar se o certificado está retornando corretamente
        /// </summary>
        /// <param name="nomeCertificado"></param>
        /// <param name="senhaCertificado"></param>
        /// <returns></returns>
        public static X509Certificate2 RetornaCertificado(string nomeCertificado, string senhaCertificado)
        {

            X509Certificate2 cert = CertificadoHelper
                .Consultar(nomeCertificado, StoreName.My, StoreLocation.LocalMachine, TipoConsultaCertificado.PorNome);

            var privateKey = cert.PrivateKey;
            return cert;
        }
        public static void PrintCertificateInfo(X509Certificate2 certificate)
        {
            LogHelper.GravarLog(string.Format("Name: {0}", certificate.FriendlyName, Environment.NewLine));
            LogHelper.GravarLog(string.Format("Issuer: {0}", certificate.IssuerName.Name, Environment.NewLine));
            LogHelper.GravarLog(string.Format("Subject: {0}", certificate.SubjectName.Name, Environment.NewLine));
            LogHelper.GravarLog(string.Format("Version: {0}", certificate.Version, Environment.NewLine));
            LogHelper.GravarLog(string.Format("Valid from: {0}", certificate.NotBefore, Environment.NewLine));
            LogHelper.GravarLog(string.Format("Valid until: {0}", certificate.NotAfter, Environment.NewLine));
            LogHelper.GravarLog(string.Format("Serial number: {0}", certificate.SerialNumber, Environment.NewLine));
            LogHelper.GravarLog(string.Format("Signature Algorithm: {0}", certificate.SignatureAlgorithm.FriendlyName, Environment.NewLine));
            LogHelper.GravarLog(string.Format("Thumbprint: {0}", certificate.Thumbprint, Environment.NewLine));
        }
    }
}
