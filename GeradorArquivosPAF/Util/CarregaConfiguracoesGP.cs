using GeradorArquivosPAF.Persistencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GeradorArquivosPAF.Util
{
    class CarregaConfiguracoesGP
    {
        public DataTable BuscaConfig(int config, string unid)
        {
            try
            {
                IBancoDados db = BDFactory.CriarBancoOracle();

                string sql = @"select * from gecfu where sequencia = " + config + " and unidade_de_negocio = '" + unid + "'";

                return db.Select(sql, true);
            }
            catch (Exception ex)
            {
                LogHelper.GravarLog(ex, "Erro CarregaConfiguracoesGP.BuscaConfig");
                throw ex;
            }
        }
    }
}
