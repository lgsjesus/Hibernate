﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;

namespace GeradorArquivosPAF.Persistencia
{
    class BancoDadosOracle : IBancoDados
    {
        public BancoDadosOracle(
             string ipServidor,
             string nomeDatabase,
             string usuarioBancoDados,
             string senhaBancoDados,
             int portaBancoDados)
        {
            StringConexao = String.Format("user id={0};password={1};data source={2}:{3}/{4};Incr Pool Size={5}",
                usuarioBancoDados,
                senhaBancoDados,
                ipServidor,
                portaBancoDados,
                nomeDatabase,
                1);
            ListaParametros = new List<OracleParameter>();
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        private OracleConnection Conexao { get; set; }
        public string StringConexao { get; set; }
        private OracleTransaction Trans { get; set; }
        private List<OracleParameter> ListaParametros { get; set; }

        public bool InTransaction
        {
            get
            {
                if (Conexao == null)
                    return false;

                if (Trans == null)
                    return false;

                if (Conexao.State == ConnectionState.Open && Trans != null)
                    return true;

                return false;
            }
        }

        public bool IsOpen
        {
            get
            {
                if (Conexao == null)
                    return false;

                if (Trans == null)
                    return false;

                if (Conexao.State == ConnectionState.Open)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Abre uma conexão com o banco de dados
        /// </summary>
        /// <returns>Objeto SqlConnection</returns>
        public OracleConnection AbreConexao()
        {
            if (Conexao == null)
            {
                Conexao = new OracleConnection();
            }
            try
            {
                if (Conexao.State != ConnectionState.Open)
                {
                    Conexao.ConnectionString = StringConexao;

                    Conexao.Open();
                }
            }
            catch (Exception ex)
            {
             //   LogHelper.GravarLog(ex, "Erro ao abrir a conexao, joga erro no windows.");
          //      Environment.Exit(1);

                throw ex;
            }
            return Conexao;
        }


        /// <summary>
        /// Fecha a conexão com o banco de dados
        /// </summary>        
        public void FechaConexao()
        {
            if (Conexao != null && Conexao.State == ConnectionState.Open)
            {
                Conexao.Close();
            }
            Conexao = null;
        }

        /// <summary>
        /// Adiciona um parâmetro à lista de parâmetros
        /// </summary>
        /// <param name="nomeParametro">Nome do parâmetro</param>
        /// <param name="tipoParametro">Tipo do parâmetro</param>
        /// <param name="valor">Valor do parâmetro</param>
        public void AddParam(string nomeParametro, DbType tipoParametro, object valor)
        {
            // Remove o parâmetro caso já exista
            OracleParameter paramExiste = ListaParametros.Where(x => x.ParameterName.Equals(nomeParametro)).FirstOrDefault();
            if (paramExiste != null)
                ListaParametros.Remove(paramExiste);


            // Adiciona o parâmetro
            //OracleParameter param = new OracleParameter(nomeParametro, tipoParametro);
            OracleParameter param = new OracleParameter();

            param.ParameterName = nomeParametro;
            param.DbType = tipoParametro;
            param.Value = valor;

            param.Value = (valor == null) ? "" : valor;
            ListaParametros.Add(param);
        }

        /// <summary>
        /// Adiciona um parâmetro à lista de parâmetros
        /// </summary>
        /// <param name="nomeParametro">Nome do parâmetro</param>
        /// <param name="tipoParametro">Tipo do parâmetro</param>
        /// <param name="valor">Valor do parâmetro</param>
        public void AddParam(string nomeParametro, OracleDbType tipoParametro, object valor)
        {
            // Remove o parâmetro caso já exista
            OracleParameter paramExiste = ListaParametros.Where(x => x.ParameterName.Equals(nomeParametro)).FirstOrDefault();
            if (paramExiste != null)
                ListaParametros.Remove(paramExiste);
            // Adiciona o parâmetro
            //OracleParameter param = new OracleParameter(nomeParametro, tipoParametro);
            OracleParameter param = new OracleParameter();

            param = new Oracle.DataAccess.Client.OracleParameter();
            param.ParameterName = nomeParametro;
            param.OracleDbType = tipoParametro;
            param.Direction = ParameterDirection.Input;

            if (tipoParametro == OracleDbType.Blob)
                param.Size = ((byte[])valor).Length;

            param.Value = valor;
            ListaParametros.Add(param);
        }

        /// <summary>
        /// Executa um begin transaction
        /// </summary>
        /// <param name="isolation">Nível de isolamento</param>
        public void BeginTransaction(IsolationLevel isolation)
        {
            AbreConexao();
            if (Trans == null)
            {
                Trans = Conexao.BeginTransaction(isolation);
            }
        }

        /// <summary>
        /// Executa um commit
        /// </summary>
        public void Commit()
        {
            try
            {
                if (Trans != null)
                {
                    Trans.Commit();
                }
                FechaConexao();
            }
            catch (Exception ex)
            {
                //  LogHelper.GravarLog(ex, "Erro ao commitar");
                throw ex;
            }
        }

        /// <summary>
        /// Executa um rollback
        /// </summary>
        public void Rollback()
        {

            try
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                FechaConexao();
            }
            catch (Exception ex)
            {
                // LogHelper.GravarLog(ex, "Erro ao executar o RollBack");
                throw ex;
            }
        }

        /// <summary>
        /// Retorna um valor do banco de dados
        /// </summary>
        /// <param name="queryString">String com o comando select a ser executado</param>                
        /// <param name="parametros">Lista com os parâmetros da query</param> 
        /// <param name="fechaConexao">Indica se a conexão deve ser fechada após a execução do comando</param> 
        /// <returns></returns>
        // public object ExecuteScalar(string queryString, Param parametros, bool fechaConexao)
        public object ExecuteScalar(string queryString, bool fechaConexao)
        {
            object scalar = new object();
            try
            {     
                OracleConnection sqlConn = (OracleConnection)AbreConexao();
                Trans = sqlConn.BeginTransaction();

                OracleCommand oracleCommand = new OracleCommand(queryString, sqlConn);
                oracleCommand.BindByName = true;
                oracleCommand.CommandTimeout = 600;

                for (int i = 0; i < ListaParametros.Count; i++)
                {
                    oracleCommand.Parameters.Add(ListaParametros[i].ParameterName, ListaParametros[i].Value);
                }
                scalar = oracleCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
               // LogHelper.GravarLog(e, "Erro ao executa o comando ExecuteScalar");
               throw e;
            }
            finally
            {
                if (fechaConexao)
                {
                    Commit();
                    FechaConexao();
                }
            }
            return scalar;
        }

        /// <summary>
        /// Executa um comando SQL
        /// </summary>
        /// <param name="queryString">String com o comando select a ser executado</param>                
        /// <param name="parametros">Lista com os parâmetros da query</param> 
        /// <param name="fechaConexao">Indica se a conexão deve ser fechada após a execução do comando</param> 
        /// <returns></returns>
        // public int ExecuteNonQuery(string queryString, Param parametros, bool fechaConexao)
        public int ExecuteNonQuery(string queryString, bool fechaConexao, bool utilizarTransaction)
        {
            int rows;
            try
            {
                OracleConnection sqlConn = AbreConexao();
                Trans = sqlConn.BeginTransaction();
                OracleCommand oracleCommand = sqlConn.CreateCommand();
                oracleCommand.CommandText = queryString;
                // oracleCommand.CommandTimeout = 5000;

                oracleCommand.BindByName = true;

                foreach (OracleParameter param in ListaParametros)
                {
                    oracleCommand.Parameters.Add(param);
                }

                rows = oracleCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
              //  LogHelper.GravarLog(ex, "Erro ao executar o comando ExecuteNonQuery");
                throw ex;
            }
            finally
            {
                if (fechaConexao)
                {
                    Commit();
                    FechaConexao();
                }
            }
            return rows;
        }

        public int ExecuteNonQuery(string queryString, bool fechaConexao)
        {
            return ExecuteNonQuery(queryString, fechaConexao, true);
        }

        /// <summary>
        /// Seleciona dados do banco de dados
        /// </summary>
        /// <param name="queryString">String com o comando select a ser executado</param>
        /// <param name="parametros">Lista com os parâmetros da query</param>
        /// <returns>DataTable com as informações</returns>
        public DataTable Select(string queryString, bool fechaConexao)
        {
            DataTable dt = new DataTable();
            OracleDataAdapter da = new OracleDataAdapter();
            try
            {
                OracleConnection sqlConn = (OracleConnection)AbreConexao();
                da.SelectCommand = sqlConn.CreateCommand();
                da.SelectCommand.CommandText = queryString;
                da.SelectCommand.CommandTimeout = 600;

                //  SelectCommand.Transaction = Trans;
                for (int i = 0; i < ListaParametros.Count; i++)
                {
                    da.SelectCommand.Parameters.Add(ListaParametros[i].ParameterName, ListaParametros[i].Value);
                }
                da.Fill(dt);
            }
            catch (Exception ex)
            {
               // LogHelper.GravarLog(ex, "Erro ao executar o comando Select = " + queryString);
               // LogHelper.GravarLog("Erro : " + ex.InnerException);
                throw ex;
            }
            finally
            {
                if (fechaConexao)
                {
                    FechaConexao();
                }
            }
            return dt;
        }

        public int Update(string query)
        {
            int result = 0;
            try
            {
                OracleConnection conn = new OracleConnection(StringConexao);
                conn.Open();

                if (conn.State != ConnectionState.Open)
                    throw new Exception("Não foi possível abrir a conexão com banco durante o UPDADE.");

                OracleCommand cmdUpdate = conn.CreateCommand();
                cmdUpdate.CommandText = query;
                cmdUpdate.BindByName = true;

                foreach (OracleParameter param in ListaParametros)
                {
                    cmdUpdate.Parameters.Add(param);
                }
                result = cmdUpdate.ExecuteNonQuery();
                conn.Close();
                return result;

            }
            catch (Exception ex)
            {
             //   LogHelper.GravarLog(ex, "Erro ao executar o comando ExecuteNonQuery");
                throw ex;
            }
        }
    }
}
