using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace GeradorArquivosPAF.Util
{
    class AssinarXML
    {
       
        public static XmlElement AssinarXmlNfe(XmlDocument xmlDocAssinado, string uriReferenciaAssinatura, X509Certificate2 certificado)
        {
            XmlElement xmlAssinatura = null;
            ConfigHelper config = new ConfigHelper();

            // Valida o URI
            #region Valida o URI
          
            try
            {
                // Verifica se a tag a ser assinada existe é única
                int qtdeRefUri = xmlDocAssinado.GetElementsByTagName(uriReferenciaAssinatura).Count;
                if (qtdeRefUri == 0)
                {
                    throw new Exception("A tag de assinatura " + uriReferenciaAssinatura.Trim() + " inexiste.");
                }
                else if (qtdeRefUri > 1)
                {
                    throw new Exception("A tag de assinatura " + uriReferenciaAssinatura.Trim() + " não é unica.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("XML mal formado - " + ex.Message);
            }
            #endregion

            // Assina o XML
            #region Assina o XML
           
            try
            {
                // Create a SignedXml object.
                SignedXml signedXml = new SignedXml(xmlDocAssinado);

                // Add the key to the SignedXml document
                signedXml.SigningKey = certificado.PrivateKey;

            

                // Create a reference to be signed
                Reference reference = new Reference();
                reference.Uri = "";

                // Localiza o URI que deve ser assinado
                XmlAttributeCollection uri = xmlDocAssinado.GetElementsByTagName(uriReferenciaAssinatura).Item(0).Attributes;
                
                /*foreach (XmlAttribute atributo in uri)
                {
                    if (atributo.Name == "Id")
                    {
                        reference.Uri = "#" + atributo.InnerText;
                    }
                }
                */

             
                // Add an enveloped transformation to the reference.
                XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(env);
                

                XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();
                reference.AddTransform(c14);


                // Add the reference to the SignedXml object.
                signedXml.AddReference(reference);

    
                // Create a new KeyInfo object
                KeyInfo keyInfo = new KeyInfo();


                // Load the certificate into a KeyInfoX509Data object
                // and add it to the KeyInfo object.
                keyInfo.AddClause(new KeyInfoX509Data(certificado));

                // Add the KeyInfo object to the SignedXml object.
                signedXml.KeyInfo = keyInfo;

                signedXml.ComputeSignature();

                // Get the XML representation of the signature and save it to an XmlElement object.

                xmlAssinatura = signedXml.GetXml();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao assinar o documento - " + ex.Message);
            }
            #endregion

            return xmlAssinatura;
        }
    
    }
}
