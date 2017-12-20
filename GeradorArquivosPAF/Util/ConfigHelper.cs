using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace GeradorArquivosPAF.Util
{
    class ConfigHelper
    {
        /// <summary>
        /// Construtor da classe também carrega as informações do config.xml
        /// </summary>
        public ConfigHelper()
        {
            Carregar();
        }

        #region Conexão_variáveis

        public string DB_IP_BANCO { get; set; }

        public string DB_Nome { get; set; }

        public string DB_Usuario { get; set; }

        public string DB_Senha { get; set; }

        public string DB_Porta { get; set; }

        public string DB_Unidade_negocio { get; set; }

        public string local_arquivo_config { get; set; }

        public string local_salvar_arquivos { get; set; }

        #endregion

        /// <summary>
        /// Carrega as configurações do arquivo config.xml, é automáticamente chamado no contrutor da classe.
        /// </summary>
        /// 
        public void Carregar()
        {
            try
            {
                string loc = Assembly.GetExecutingAssembly().Location;
                string path = System.IO.Path.GetDirectoryName(loc);
                string arquivo = path + @"\" + "config.xml";
                this.local_arquivo_config = arquivo;

                XmlDocument config = new XmlDocument();

                config.Load(arquivo);

                if (!config.HasChildNodes)
                {
                    throw new Exception(string.Format("Não foi possível carregar as configurações do arquivo: {0}", arquivo));
                }

                #region Conexão
                if (config.GetElementsByTagName("ip-banco").Count > 0)
                    this.DB_IP_BANCO = config.GetElementsByTagName("ip-banco")[0].InnerText;

                if (config.GetElementsByTagName("nome-banco").Count > 0)
                    this.DB_Nome = config.GetElementsByTagName("nome-banco")[0].InnerText;

                if (config.GetElementsByTagName("usuario-banco").Count > 0)
                    this.DB_Usuario = config.GetElementsByTagName("usuario-banco")[0].InnerText;

                if (config.GetElementsByTagName("senha-banco").Count > 0)
                    this.DB_Senha = config.GetElementsByTagName("senha-banco")[0].InnerText;

                if (config.GetElementsByTagName("porta-banco").Count > 0)
                    this.DB_Porta = config.GetElementsByTagName("porta-banco")[0].InnerText;

                if (config.GetElementsByTagName("pasta-salvar").Count > 0)
                    this.local_salvar_arquivos = config.GetElementsByTagName("pasta-salvar")[0].InnerText;
                #endregion              

            }
            catch (Exception exp)
            {
                throw new Exception(string.Format("Erro ao carregar as configurações. Erro: {0}", exp.Message), exp);
            }
        }
    }
}
