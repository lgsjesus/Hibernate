using GeradorArquivosPAF.Util;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeradorArquivosPAF.Persistencia
{
    class BDFactory
    {
        #region Oracle

        public static IBancoDados CriarBancoOracle(string ipServidor, string nomeDatabase, string usuarioBancoDados, string senhaBancoDados, int portaBancoDados)
        {
            return new BancoDadosOracle(ipServidor, nomeDatabase, usuarioBancoDados, senhaBancoDados, portaBancoDados);
        }

        /// <summary>
        /// Carrega as configurações do app.config ou web.config
        /// </summary>
        /// <returns></returns>
        public static IBancoDados CriarBancoOracle()
        {

            try
            {
                ConfigHelper config = new ConfigHelper();

                int port = int.TryParse(config.DB_Porta, out port)
                    ? port
                    : 1521;

                return new BancoDadosOracle(config.DB_IP_BANCO, config.DB_Nome, config.DB_Usuario, config.DB_Senha, port);
            }
            catch (Exception ex)
            {
   //             LogHelper.GravarLog(ex, "Erro ao carregar as configurações do app.config");
                throw ex;
            }

        }

        #endregion
    }
}
