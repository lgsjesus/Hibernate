using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GeradorArquivosPAF.Util
{
    class Serializar
    {
        public static XmlDocument Serializa<T>(T env)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            //Stream fileStream = new FileStream(fileName, FileMode.Create);
            Stream memoryStream = new MemoryStream();
            //Salva na condificação UTF-8
            StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
           // ns.Add("", uri);

            serializer.Serialize(memoryStream, env, ns);

            memoryStream.Position = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memoryStream);

            memoryStream.Close();

            return xmlDoc;

        }

    }
}
