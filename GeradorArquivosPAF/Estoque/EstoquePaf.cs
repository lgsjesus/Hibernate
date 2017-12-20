using GeradorArquivosPAF.Model;
using GeradorArquivosPAF.Persistencia;
using GeradorArquivosPAF.Tabelas;
using GeradorArquivosPAF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace GeradorArquivosPAF
{
    class EstoquePaf
    {
        public void pesquisaEstoque(string unidNeg,DateTime dataEstoque,string path,string serie)
        {           
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                int cont = 1;
     
                DadosPaf df = new DadosPaf();
                IList<EstoqueMensagemDadosEstoqueProduto> listProd = new List<EstoqueMensagemDadosEstoqueProduto>();
                foreach (string linha in lines)
                {
                    if (cont == 1)
                        df.estabCli = linha.Trim();
                    else if (cont == 2)
                        df.iestadualCli = linha.Trim();
                    else if (cont == 3)
                        df.cnpjCli = linha.Trim();
                    else if (cont == 4)
                        df.nomeCli = linha.Trim();
                    else if (cont == 5)
                        df.numCredenCli = linha.Trim();
                    else if (cont == 6)
                        df.nomComercial = linha.Trim();
                    else if (cont == 7)
                        df.versaoPafEcf = linha.Trim();
                    else if (cont == 8)
                        df.cnpjDesenv = linha.Trim();
                    else if (cont == 9)
                        df.nomDesenv = linha.Trim();
                    else if (cont == 10)
                        df.nFabEcf = linha.Trim();
                    else if (cont == 11)
                        df.nTipoEcf = linha.Trim();
                    else if (cont == 12)
                        df.marcaEcf = linha.Trim();
                    else if (cont == 13)
                        df.modeloEcf = linha.Trim();
                    else if (cont == 14)
                        df.versaoSofwEcf = linha.Trim();
                    else if (cont == 15)
                        df.caixa = linha.Trim();
                    else if (cont == 16)
                        df.dataRedz = Convert.ToDateTime(linha.Trim());
                    else if(cont >=17)
                    {
                        char[] delimiterChars = { ';' };
                        string[] words = linha.Trim().Split(delimiterChars);
                        EstoqueMensagemDadosEstoqueProduto prod = new EstoqueMensagemDadosEstoqueProduto();
                        int conta = 1;
                        foreach (string s in words)
                        {
                            if (conta == 1)
                                prod.Descricao = s.Trim();
                            else if (conta == 2)
                            {
                                prod.CodigoProprio = s.Trim();

                              //  prod.Tipo = (EstoqueMensagemDadosEstoqueProdutoCodigoTipo)Enum.Parse(typeof(EstoqueMensagemDadosEstoqueProdutoCodigoTipo), "Proprio");
                            }
                            else if (conta == 3)
                                prod.ValorUnitario = s.Trim();
                            else if (conta == 4)
                                prod.Quantidade = Convert.ToInt64(s.Trim()).ToString();
                            else if (conta == 5)
                                prod.Unidade = s.Trim();
                            else if (conta == 6)
                            {
                                prod.SituacaoTributaria = (EstoqueMensagemDadosEstoqueProdutoSituacaoTributaria)Enum.Parse(typeof(EstoqueMensagemDadosEstoqueProdutoSituacaoTributaria), s.Trim());
                            }
                            else if (conta ==7)
                            {
                               
                                if (!(s.Trim().Equals("") || s.Trim().Equals("0000")))
                                {
                                    prod.Aliquota = s.Trim();
                                }
                                else
                                {
                                    prod.Aliquota = "";
                                }
                            }
                            else if (conta == 8)
                            {
                                prod.IsArredondado = s.Trim().Equals("T");
                            }
                            else if (conta == 10)
                            {
                                
                                prod.SituacaoEstoque = (EstoqueMensagemDadosEstoqueProdutoSituacaoEstoque)Enum.Parse(typeof(EstoqueMensagemDadosEstoqueProdutoSituacaoEstoque), s.Trim());
                            }
                            else if (conta == 11)
                            {
                                prod.CodigoNCMSH = s.Trim();
                            }
                            else if (conta == 12)
                            {
                                prod.CodigoGTIN = s.Trim();
                            }
                            else if (conta == 13)
                            {
                                prod.CodigoCEST = s.Trim();
                            }
                            conta++; 
                        }
                        
                        prod.Ippt = (EstoqueMensagemDadosEstoqueProdutoIppt)Enum.Parse(typeof(EstoqueMensagemDadosEstoqueProdutoIppt), "Proprio");
                        listProd.Add(prod);

                    }
                    cont++;
                }
                
                Estoque est = new Estoque();
                est.Versao = "1.0";
                
                EstoqueMensagemEstabelecimento estE = new EstoqueMensagemEstabelecimento();
              //  estE.Cnpj = df.cnpjCli;
                estE.Ie = df.iestadualCli;
                
             //   estE.NomeEmpresarial = df.nomeCli;

                EstoqueMensagemPafEcf estP = new EstoqueMensagemPafEcf();
                //estP.CnpjDesenvolvedor = df.cnpjDesenv;
                //estP.NomeComercial = df.nomComercial;
                //estP.NomeEmpresarialDesenvolvedor = df.nomDesenv;
                //estP.Codigo= df.numCredenCli;
                //estP.Versao = df.versaoPafEcf;
                estP.NumeroCredenciamento = df.numCredenCli;

                EstoqueMensagem estM = new EstoqueMensagem();
                estM.Estabelecimento = estE;
                estM.PafEcf = estP;

                EstoqueMensagemDadosEstoque estDad = new EstoqueMensagemDadosEstoque();
                DateTime dataIni = dataEstoque.AddMonths(-1);
                //estDad.DataReferenciaInicial = dataIni.ToString("dd/MM/yyyy");
                estDad.DataReferencia = dataEstoque;

                estDad.Produtos = listProd.ToArray();
                estM.DadosEstoque = estDad;

                est.Mensagem = estM;
                XmlDocument xmlNFE = Serializar.Serializa<Estoque>(est);

                ConfigHelper config = new ConfigHelper();
                string pathfile = config.local_salvar_arquivos + unidNeg + @"\" + "estoquePaf" + @"\" + df.nFabEcf + @"\"
               + df.dataRedz.ToString("ddMMyyyy");

                if (!Directory.Exists(pathfile))
                    Directory.CreateDirectory(pathfile);
                pathfile = pathfile + @"\env_estoquePaf.xml";

                CarregaConfiguracoesGP carg = new CarregaConfiguracoesGP();
                DataTable datCert = carg.BuscaConfig(85475, unidNeg);

                if (datCert.Rows.Count > 0)
                {
                    DataRow row = datCert.Rows[0];
                    string serialNumber = row["DADO_ALPHA"].ToString().Trim();

                    if (serialNumber.Contains("SERIALNUMBER="))
                        serialNumber = serialNumber.Replace("SERIALNUMBER=", string.Empty);

                    var certificado = CertificadoHelper.Consultar(StoreName.My, StoreLocation.CurrentUser, serialNumber, CertificadoHelper.TipoConsultaCertificado.PorNroSerie);
                    if (certificado == null)
                    {
                        MessageBox.Show("Certificado " + row["DADO_ALPHA"].ToString().Trim() + " não localizado na maquina, impossivel assinar.", "Erro Certificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        XmlElement xmlAssinatura = AssinarXML.AssinarXmlNfe(xmlNFE, "Estoque", certificado);
                        XmlNode nodeEvento = xmlNFE.GetElementsByTagName("Estoque")[0];
                        if (nodeEvento != null)
                            nodeEvento.AppendChild(xmlAssinatura);
                    }
                }
                else
                {
                    MessageBox.Show("Não foi possivel assinar o arquivo, deve informar o certificado na config 85475.", "Erro Certificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                xmlNFE.Save(pathfile);
                GPPAFTRARQ arq = new GPPAFTRARQ();
                DataTable dt = arq.retornaEstoque(unidNeg, "0",serie,df.dataRedz,df.nFabEcf);

                if(dt.Rows.Count<=0)
                     arq.insereArquivo(unidNeg, "0", serie, "E", pathfile, df.dataRedz, df.nFabEcf);


                estoquesPendentes(unidNeg, serie,false);
                MessageBox.Show("Arquivo gerado com sucesso mas não transmitido! \n" + pathfile, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(pathfile);
            }
            catch(Exception ex)
            {
                LogHelper.GravarLog("Erro Estoque Paf " + ex);
                throw ex;
            }
        }

        internal void estoquesPendentes(string serie5, string unidNeg5,bool mostraMen)
        {
            GPPAFTRARQ arq = new GPPAFTRARQ();
            int qtdePend = arq.retornaQtdeDiasPendente(unidNeg5, serie5, "E");
            string mensag = "HÁ UM ARQUIVO COM INFORMAÇÕES DO ESTOQUE MENSAL DO ESTABELECIMENTO PENDENTE DE TRANSMISSÃO AO FISCO."+
                             "O CONTRIBUINTE PODE TRANSMITIR O ARQUIVO PELO MENU FISCAL"+
                             " POR MEIO DO COMANDO ‘Envio ao FISCO-ESTOQUE’.";
            if (qtdePend > 0)
            {
                if (qtdePend >= 10)
                    mensag = mensag + "\n VERIFIQUE COM O FORNECEDOR DO PROGRAMA A SOLUÇÃO DA PENDÊNCIA.";

                MessageBox.Show(mensag, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else 
            {
                if(mostraMen)
                     MessageBox.Show("Não há arquivos pendentes de transmissão...\n", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
