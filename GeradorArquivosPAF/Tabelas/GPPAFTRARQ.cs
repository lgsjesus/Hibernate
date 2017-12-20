using GeradorArquivosPAF.Persistencia;
using GeradorArquivosPAF.Util;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GeradorArquivosPAF.Tabelas
{
    class GPPAFTRARQ
    {
       
        public DataTable retornaReducao(string unidNeg, string COO, string serie)
        {
            DataTable dt = null;
            string sql = "";
            try
            {

                sql = "select * from GPPAFTRARQ g where g.unid_negocio = '" + unidNeg
                    + "' and g.serie = '" + serie + "' and g.coo = " + COO + " and g.tipo_arquivo = 'R'";
                IBancoDados db = BDFactory.CriarBancoOracle();
                dt = db.Select(sql, true);

            }
            catch (Exception ex)
            {
                LogHelper.GravarLog("Erro leitura da tabela GPPAFTRARQ " + ex);
                LogHelper.GravarLog("Sql executado = " + sql);
            }
            return dt;
        }
        public void insereArquivo(string unidNeg, string COO, string serie, string tipo, string path,DateTime dtRed,string numFabr)
        {
            string sql = "";
            try
            {
                ConfigHelper config = new ConfigHelper();
                string conexao = String.Format("user id={0};password={1};data source={2}:{3}/{4};Incr Pool Size={5}",
                      config.DB_Usuario,
                      config.DB_Senha,
                      config.DB_IP_BANCO,
                      config.DB_Porta,
                      config.DB_Nome,
                      1);
                OracleConnection orl = new OracleConnection(conexao);
                sql = @"insert into GPPAFTRARQ (UNID_NEGOCIO, SERIE, COO, NUM_FABRICA_ECF, DT_MOVIMENTO, TIPO_ARQUIVO, SITUACAO, PATH_FILE)
                        values ('" + unidNeg + "', '" + serie + "', " + COO + ", '" + numFabr 
                        + "', to_date('" + dtRed.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy'), '"+tipo
                        +"', 'P', '" + path + "')";
                OracleCommand oracleCommand = new OracleCommand(sql, orl);
                oracleCommand.BindByName = true;
                oracleCommand.CommandTimeout = 600;

                orl.Open();
                oracleCommand.ExecuteNonQuery();
                orl.Close();
                oracleCommand.Dispose();
                orl.Dispose();
            }
            catch (Exception ex)
            {
                LogHelper.GravarLog("Erro inserir na tabela GPPAFTRARQ " + ex);
                LogHelper.GravarLog("Sql executado = " + sql);
            }
        }
        public int retornaQtdeRedPendente(string unidNeg, string serie,string tipoAr)
        {
            int qtde = 0;
            DataTable dt = null;
            string sql = "";
            try
            {
                sql = "select count(*) as qtde from GPPAFTRARQ g where g.unid_negocio = '" + 
                    unidNeg + "' and g.serie = '" + serie + "' and g.tipo_arquivo = '"+tipoAr+"' and g.situacao = 'P'";
                IBancoDados db = BDFactory.CriarBancoOracle();
                dt = db.Select(sql, true);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    qtde = Int32.Parse(row["qtde"].ToString());
                }
            }
            catch (Exception ex)
            {              
               LogHelper.GravarLog("Erro leitura da tabela GPPAFTRARQ " + ex);
                LogHelper.GravarLog("Sql executado = " + sql);
                throw ex;
            }
            return qtde;
        }
        public DataTable retornaEstoque(string unidNeg, string COO, string serie,DateTime dtMov,string numFabric)
        {
            DataTable dt = null;
            string sql = "";
            try
            {
                sql = "select * from GPPAFTRARQ g where g.unid_negocio = '" + unidNeg
                    + "' and g.serie = '" + serie + "' and g.coo = " + COO +
                    " and g.tipo_arquivo = 'E' and g.dt_movimento = to_date('" + dtMov.ToString("dd/MM/yyyy")
                    +"','DD/MM/YYYY') AND g.num_fabrica_ecf = '"+numFabric + "'";
                IBancoDados db = BDFactory.CriarBancoOracle();
                dt = db.Select(sql, true);

            }
            catch (Exception ex)
            {
                LogHelper.GravarLog("Erro leitura da tabela GPPAFTRARQ " + ex);
                LogHelper.GravarLog("Sql executado = " + sql);
            }
            return dt;
        }
        public int retornaQtdeDiasPendente(string unidNeg, string serie, string tipoAr)
        {
            int qtde = 0;
            DataTable dt = null;
            string sql = "";
            try
            {
                sql = "select g.dt_movimento from GPPAFTRARQ g where g.unid_negocio = '" +
                    unidNeg + "' and g.serie = '" + serie + "' and g.tipo_arquivo = '" + tipoAr + "' and g.situacao = 'P' " +
                    " order by g.dt_movimento ";

                IBancoDados db = BDFactory.CriarBancoOracle();
                dt = db.Select(sql, true);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    qtde = DateTime.Now.Subtract(Convert.ToDateTime(row["dt_movimento"].ToString())).Days;
                }
            }
            catch (Exception ex)
            {
                LogHelper.GravarLog("Erro leitura da tabela GPPAFTRARQ " + ex);
                LogHelper.GravarLog("Sql executado = " + sql);
                throw ex;
            }
            return qtde;
        }
    }
}
