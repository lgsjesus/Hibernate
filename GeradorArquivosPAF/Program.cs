using GeradorArquivosPAF.Persistencia;
using GeradorArquivosPAF.Util;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace GeradorArquivosPAF
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int operacao = 0;
              
                if (args.Count() > 0)
                {
                    
                    // Creating a Global culture specific to our application.
                    System.Globalization.CultureInfo cultureInfo =
                          new System.Globalization.CultureInfo("pt-BR");
                    // Creating the DateTime Information specific to our application.
                     System.Globalization.DateTimeFormatInfo dateTimeInfo =
                     new System.Globalization.DateTimeFormatInfo();
                    // Defining various date and time formats.
                    dateTimeInfo.DateSeparator = "/";
                    dateTimeInfo.LongDatePattern = "dd/MM/yyyy";
                    dateTimeInfo.ShortDatePattern = "dd/MMM/yy";
                    dateTimeInfo.LongTimePattern = "hh:mm:ss tt";
                    dateTimeInfo.ShortTimePattern = "hh:mm tt";
                    // Setting application wide date time format.
                    cultureInfo.DateTimeFormat = dateTimeInfo;
                    // Assigning our custom Culture to the application.

                    Thread.CurrentThread.CurrentCulture = cultureInfo;

                    operacao = Int32.Parse(args[0].ToString());
                    switch (operacao)
                    {
                        case 1:
                            string path = args[1].ToString();
                            string serie = args[2].ToString();
                            string unidNeg = args[3].ToString();
                            GeraReducaoZ ger = new GeraReducaoZ();

                            ger.gerarArquivos(serie, unidNeg, path);

                            break;
                        case 2:
                            string path2 = args[1].ToString();
                            string serie2 = args[2].ToString();
                            string unidNeg2 = args[3].ToString();
                            GeraReducaoZ ger2 = new GeraReducaoZ();

                            ger2.reducoesPendentes(serie2, unidNeg2, path2);

                            break;
                        case 3:
                            string path3 = args[1].ToString();
                            DateTime dtGer = Convert.ToDateTime(args[2].ToString());
                            string unidNeg3 = args[3].ToString();
                            string serie3 = args[4].ToString();
                            EstoquePaf est = new EstoquePaf();
                            est.pesquisaEstoque(unidNeg3, dtGer, path3,serie3);

                            break;
                        case 4:
                            string serie5 = args[1].ToString();
                            string unidNeg5 = args[2].ToString();
                            EstoquePaf  ger5 = new EstoquePaf();
                            ger5.estoquesPendentes(serie5, unidNeg5,true);

                            break;
                        case 5:
                            string serialNumber = args[1].ToString();

                            var certificado = CertificadoHelper.Consultar(StoreName.My, StoreLocation.LocalMachine, serialNumber, CertificadoHelper.TipoConsultaCertificado.PorNroSerie);
                            if (certificado == null)
                            {
                                LogHelper.GravarLog("Certificado nao encontrado");
                            }
                            else
                            {
                                LogHelper.GravarLog("Certificado encontrado");
                            }
                            break;
                        case 99:
                            testaConexao();

                            break;
                        default:
                            LogHelper.GravarLog("Operacao nao programada...");
                            break;
                    }
                }
                else
                {
                    LogHelper.GravarLog("Nao foi passado nenhum argumento...");
                }
            }
           catch(Exception ex)
            {
                LogHelper.GravarLog("ERRO na operacao:" + ex.Message);
            }
        }
        private static void testaConexao()
        {
            try
            {
                IBancoDados db = BDFactory.CriarBancoOracle();
                db.AbreConexao();

                if (db.IsOpen == true)
                   Console.WriteLine("Conexão realizada com sucesso!");

                else
                    Console.WriteLine("Erro ao abrir conexão com banco!");

                db.FechaConexao();
            }

            catch (Exception ex)
            {
                LogHelper.GravarLog("Erro ao abrir conexão com banco de dados." + ex);
            }
        }
    }
}
