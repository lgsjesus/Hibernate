using GeradorArquivosPAF.Model;
using GeradorArquivosPAF.Persistencia;
using GeradorArquivosPAF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using GeradorArquivosPAF.Tabelas;
using System.Security.Cryptography.X509Certificates;

namespace GeradorArquivosPAF
{
    class GeraReducaoZ
    {
        private string _decimal2casas = "0.00";
        public void gerarArquivos(string serie,string unidNeg,string caminho)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(caminho);
                int cont = 1;
                GPPAFTRARQ gpArq = new GPPAFTRARQ();
                DadosPaf df = new DadosPaf();
                foreach(string linha in lines)
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

                    cont++;
                }


              
                ReducaoZ redZ = new ReducaoZ();
                redZ.Versao = "1.0";

                ReducaoZMensagem redMensagem = new ReducaoZMensagem();

                
                ReducaoZMensagemEstabelecimento redMen = new ReducaoZMensagemEstabelecimento();
                redMen.Ie = df.iestadualCli;
                redMen.NomeEmpresarial = df.nomeCli;
                redMen.Cnpj = df.cnpjCli;

               
                ReducaoZMensagemPafEcf redPaf = new ReducaoZMensagemPafEcf();
                redPaf.CnpjDesenvolvedor = df.cnpjDesenv;
                redPaf.NomeComercial = df.nomComercial;
                redPaf.NomeEmpresarialDesenvolvedor = df.nomDesenv;
                redPaf.NumeroCredenciamento = df.numCredenCli;
                redPaf.Versao = df.versaoPafEcf;
                

                ReducaoZMensagemEcf redEcf = new ReducaoZMensagemEcf();
                redEcf.Marca = df.marcaEcf;
                redEcf.Modelo = df.modeloEcf;
                redEcf.NumeroFabricacao = df.nFabEcf;
                redEcf.Tipo = df.nTipoEcf;
                redEcf.Versao = df.versaoSofwEcf;

                
                ReducaoZMensagemEcfDadosReducaoZ redDados = buscaMensagemEcf(df.dataRedz,serie, unidNeg);

                
                DataTable dt= gpArq.retornaReducao(unidNeg, redDados.COO, serie);

              
                bool arquivoGerado = false;
                if(dt.Rows.Count > 0)
                {
                    DataRow linha = dt.Rows[0];
                    if (System.IO.File.Exists(linha["PATH_FILE"].ToString()))
                    {
                        arquivoGerado = true;
                        string situacao = linha["SITUACAO"].ToString().Equals("T") ? "Transmitido" : "Pendente de Transmissão";
                        MessageBox.Show("Arquivo de redução Z já foi gerado para esta data. \n" + "Situação: " + situacao);
                    }
                    else
                    {
                        arquivoGerado = false;
                    }
                }
                if (dt.Rows.Count <= 0 || !arquivoGerado)
                {
                    IList<ReducaoZMensagemEcfDadosReducaoZTotalizadorParcial> listTotali = retTotalizadores(serie, unidNeg, df.dataRedz, redDados.CRZ, redDados.COO);
                    redDados.TotalizadoresParciais = listTotali.ToArray();
                    redEcf.DadosReducaoZ = redDados;

                    redMensagem.Ecf = redEcf;
                    redMensagem.Estabelecimento = redMen;
                    redMensagem.PafEcf = redPaf;
                    redZ.Mensagem = redMensagem;

                    XmlDocument xmlNFE = Serializar.Serializa<ReducaoZ>(redZ);

                   

                    ConfigHelper config = new ConfigHelper();
                    string pathfile = config.local_salvar_arquivos + unidNeg + @"\" + "reducoesZ" + @"\" + df.nFabEcf + @"\"
                   + df.dataRedz.ToString("ddMMyyyy");

                    if (!Directory.Exists(pathfile))
                        Directory.CreateDirectory(pathfile);
                    pathfile = pathfile + @"\reducaoZ.xml";

                    CarregaConfiguracoesGP carg = new CarregaConfiguracoesGP();
                    DataTable datCert = carg.BuscaConfig(85475, unidNeg);

                    if(datCert.Rows.Count > 0)
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
                            XmlElement xmlAssinatura = AssinarXML.AssinarXmlNfe(xmlNFE, "ReducaoZ", certificado);
                            XmlNode nodeEvento = xmlNFE.GetElementsByTagName("ReducaoZ")[0];
                            if (nodeEvento != null)
                                nodeEvento.AppendChild(xmlAssinatura);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Não foi possivel assinar o arquivo, deve informar o certificado na config 85475.","Erro Certificado" ,MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    xmlNFE.Save(pathfile);

                    if(dt.Rows.Count <=0)
                        gpArq.insereArquivo(unidNeg, redDados.COO, serie, "R", pathfile,df.dataRedz, df.nFabEcf);

                    int qtdePend = gpArq.retornaQtdeRedPendente(unidNeg, serie,"R");
                    string mensag = @" “HÁ (0) ARQUIVOS COM INFORMAÇÕES DA REDUÇÃO Z DO PAF-ECF PENDENTES DE TRANSMISSÃO AO FISCO. 
                                    O CONTRIBUINTE PODE TRANSMITIR OS ARQUIVOS PELO MENU FISCAL POR MEIO DO COMANDO ‘Envio ao FISCO-REDUÇÃO Z’.";
                    if (qtdePend > 0)
                    {
                        mensag =  @"HÁ "+ qtdePend+" ARQUIVOS COM INFORMAÇÕES DA REDUÇÃO Z DO PAF-ECF PENDENTES DE TRANSMISSÃO AO FISCO. "+
                                "O CONTRIBUINTE PODE TRANSMITIR OS ARQUIVOS PELO MENU FISCAL POR MEIO DO COMANDO ‘Envio ao FISCO-REDUÇÃO Z’.";
                        if (qtdePend >= 5 && qtdePend <= 8)
                        {
                            mensag = mensag + "\n" + " VERIFIQUE COM O FORNECEDOR DO PROGRAMA A SOLUÇÃO DA PENDÊNCIA.";
                        }
                        else if (qtdePend == 9)
                        {
                            mensag = mensag + "\n" + @"A PARTIR DA 10ª TRANSMISSÃO PENDENTE, O SEU PROGRAMA SERÁ BLOQUEADO E 
                                            SOMENTE SERÁ LIBERADO APÓS TODAS AS TRANSMISSÕES SEREM REALIZADAS. 
                                           VERIFIQUE URGENTEMENTE COM O FORNECEDOR DO PROGRAMA A SOLUÇÃO DA PENDÊNCIA";
                        }
                        MessageBox.Show("Arquivo gerado com sucesso mas não transmitido! \n" + pathfile, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show(mensag, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Arquivo gerado com sucesso mas não transmitido! \n" + pathfile, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }       
                    System.Diagnostics.Process.Start(pathfile);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arquivo não pode ser gerado, favor verifique o log...", "Erro Gerar ReduçãoZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.GravarLog("ERRO geraçao do arquivo RedZ= \n" + ex.Message);
                LogHelper.GravarLog("ERRO geraçao do arquivo RedZ= \n" + ex);
            }
        }

        internal void reducoesPendentes(string serie2, string unidNeg2, string path2)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path2);
                int cont = 1;
                GPPAFTRARQ gpArq = new GPPAFTRARQ();
                DadosPaf df = new DadosPaf();
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

                    cont++;
                }
                int qtdePend = gpArq.retornaQtdeRedPendente(unidNeg2, serie2,"R");
                string mensag = @" “HÁ (0) ARQUIVOS COM INFORMAÇÕES DA REDUÇÃO Z DO PAF-ECF PENDENTES DE TRANSMISSÃO AO FISCO. 
                                    O CONTRIBUINTE PODE TRANSMITIR OS ARQUIVOS PELO MENU FISCAL POR MEIO DO COMANDO ‘Envio ao FISCO-REDUÇÃO Z’.";
                if (qtdePend > 0)
                {
                    mensag = @"HÁ " + qtdePend + " ARQUIVOS COM INFORMAÇÕES DA REDUÇÃO Z DO PAF-ECF PENDENTES DE TRANSMISSÃO AO FISCO. " +
                            "O CONTRIBUINTE PODE TRANSMITIR OS ARQUIVOS PELO MENU FISCAL POR MEIO DO COMANDO ‘Envio ao FISCO-REDUÇÃO Z’.";
                    if (qtdePend >= 5 && qtdePend <= 8)
                    {
                        mensag = mensag + "\n" + " VERIFIQUE COM O FORNECEDOR DO PROGRAMA A SOLUÇÃO DA PENDÊNCIA.";
                    }
                    else if (qtdePend == 9)
                    {
                        mensag = mensag +  "\n" + @"A PARTIR DA 10ª TRANSMISSÃO PENDENTE, O SEU PROGRAMA SERÁ BLOQUEADO E 
                                            SOMENTE SERÁ LIBERADO APÓS TODAS AS TRANSMISSÕES SEREM REALIZADAS. 
                                           VERIFIQUE URGENTEMENTE COM O FORNECEDOR DO PROGRAMA A SOLUÇÃO DA PENDÊNCIA";
                    }
                   
                    MessageBox.Show(mensag, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Não há arquivos pendentes de transmissão...\n", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show("ERRO Consulta arquivos pendentes, verifique log...", "Erro Gerar ReduçãoZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.GravarLog("ERRO geraçao do arquivo RedZ= \n" + ex.Message);
            }
}

        private IList<ReducaoZMensagemEcfDadosReducaoZTotalizadorParcial> retTotalizadores(string serie, string uNeg,DateTime dtRed,string CRZ,string COO)
        {
            IList<ReducaoZMensagemEcfDadosReducaoZTotalizadorParcial> listTotali = new List<ReducaoZMensagemEcfDadosReducaoZTotalizadorParcial>();
            string sql = "";
            try
            {

                sql = @"select l.* from glpafr02 f 
                        inner join glpafr03 l on l.un_negocio = f.un_negocio and l.serie = f.serie and l.coo = f.coo 
                        where f.dt_movto =  to_date('" + dtRed.ToString("dd/MM/yyyy") + "','DD/MM/YYYY') and f.un_negocio = '" + uNeg + "' and f.serie = '" + serie + "' "+ 
                " order by l.totaliz_parcial";

                IBancoDados db = BDFactory.CriarBancoOracle();
                DataTable dt = db.Select(sql, true);

                foreach (DataRow row in dt.Rows)
                {
                    ReducaoZMensagemEcfDadosReducaoZTotalizadorParcial redTotPar = new ReducaoZMensagemEcfDadosReducaoZTotalizadorParcial();
                    redTotPar.Nome = row["totaliz_parcial"].ToString();
                    redTotPar.Valor = Convert.ToDecimal(row["VLR"].ToString()).ToString(_decimal2casas);
                    redTotPar.ProdutosServicos = retornaProdutosServ(serie, uNeg, dtRed, row["totaliz_parcial"].ToString(),CRZ,COO);
                    listTotali.Add(redTotPar);
                }
            }
            catch(Exception ex)
            {
                LogHelper.GravarLog("ERRO Busca Totalizadores" + ex.Message);
            }
            return listTotali;
        }

        private ReducaoZMensagemEcfDadosReducaoZTotalizadorParcialProdutosServicos retornaProdutosServ(string serie, string uNeg, DateTime dtRed,string tot,string CRZ,string COOFinal)
        {
            ReducaoZMensagemEcfDadosReducaoZTotalizadorParcialProdutosServicos redProd = new ReducaoZMensagemEcfDadosReducaoZTotalizadorParcialProdutosServicos();
            string sql = "";
            int cooIni = 1;
            IList<ReducaoZMensagemEcfDadosReducaoZTotalizadorParcialProdutosServicosProduto> listProdServ = null;
            try
            {
                IBancoDados db = BDFactory.CriarBancoOracle();
               
                sql = @"select l.*  from glpafr02 f 
                        inner join glpaftra l on l.un_negocio = f.un_negocio and l.serie = f.serie and l.coo = f.coo and l.tipo_docto = 'RZ' and l.docto < "+CRZ +
                      "  where f.un_negocio = '"+uNeg+"' and f.serie = '"+ serie+ "'  " + 
                       " order by l.docto desc";

             

                DataTable de = db.Select(sql, true);

                if (de.Rows.Count > 0)
                {
                    DataRow row0 = de.Rows[0];
                    cooIni = Convert.ToInt32(row0["COO"].ToString());

                    
                }
                sql = @" select I.*  from GLPAFTRA F
                         INNER JOIN GLPAFR05 I ON I.UN_NEGOCIO = F.UN_NEGOCIO AND I.SERIE = F.SERIE AND I.COO = F.COO
                         WHERE F.SERIE = '"+serie+ "' and F.UN_NEGOCIO = '"+uNeg+ "' and i.totaliz_parcial = '" + tot +"' and f.COO between " + cooIni + " and " + COOFinal;

                
                db = BDFactory.CriarBancoOracle();
                DataTable dt = db.Select(sql, true);
                if (dt.Rows.Count > 0)
                {
                    listProdServ =
                            new List<ReducaoZMensagemEcfDadosReducaoZTotalizadorParcialProdutosServicosProduto>();
                }
                foreach (DataRow row in dt.Rows)
                {
                    ReducaoZMensagemEcfDadosReducaoZTotalizadorParcialProdutosServicosProduto prodServ =
                    new ReducaoZMensagemEcfDadosReducaoZTotalizadorParcialProdutosServicosProduto();
                    prodServ.Codigo = row["CODIGO"].ToString();
                    prodServ.Descricao = row["DESCRICAO"].ToString();
                    prodServ.Quantidade = row["QTDE"].ToString();
                    prodServ.Unidade = row["UNIDADE_MEDIDA"].ToString(); ;
                    prodServ.ValorUnitario = Convert.ToDecimal(row["PR_UNIT"].ToString()).ToString(_decimal2casas); 
                    prodServ.Tipo = (ReducaoZMensagemEcfDadosReducaoZTotalizadorParcialProdutosServicosProdutoCodigoTipo)Enum.Parse(typeof(ReducaoZMensagemEcfDadosReducaoZTotalizadorParcialProdutosServicosProdutoCodigoTipo), "2");
                    listProdServ.Add(prodServ);
                }
                if(listProdServ != null)
                        redProd.Produto = listProdServ.ToArray();
                else
                {
                    listProdServ = new List<ReducaoZMensagemEcfDadosReducaoZTotalizadorParcialProdutosServicosProduto>();
                    redProd.Produto = listProdServ.ToArray();
                }
            }
            catch (Exception ex)
            {
                LogHelper.GravarLog("ERRO gera arquivo RedZ..." + ex.Message);
                LogHelper.GravarLog("ERRO gera busca Produtos RedZ...= " + sql);
            }
            return redProd;
        }

        private ReducaoZMensagemEcfDadosReducaoZ buscaMensagemEcf(DateTime dtRed,string serie,string uNeg)
        {
            ReducaoZMensagemEcfDadosReducaoZ redDados = null;        
            string sql = "";
            try
            {
                
                 sql = @"select l.coo,l.docto as CRZ,f.cro,f.vlr_venda_bruta,f.valor_grande_total  from glpafr02 f 
                            inner join glpaftra l on l.un_negocio = f.un_negocio and l.serie = f.serie and l.coo = f.coo and l.tipo_docto = 'RZ'
                            where f.dt_movto =  to_date('" + dtRed.ToString("dd/MM/yyyy") + "','DD/MM/YYYY') and f.un_negocio = '" + uNeg + "' and f.serie = '" + serie + "'";
                IBancoDados db = BDFactory.CriarBancoOracle();
                DataTable dt = db.Select(sql, true);
                
                if (dt.Rows.Count > 0)
                {
                    redDados = new ReducaoZMensagemEcfDadosReducaoZ();
                    DataRow row = dt.Rows[0];
                    redDados.dataReferencia = dtRed.ToString("dd/MM/yyyy");
                    redDados.COO = row["COO"].ToString();
                    redDados.CRO = row["CRO"].ToString();
                    redDados.CRZ = row["CRZ"].ToString(); 
                    redDados.VendaBrutaDiaria = Convert.ToDecimal(row["vlr_venda_bruta"].ToString()).ToString(_decimal2casas);
                    redDados.GT = Convert.ToDecimal(row["valor_grande_total"].ToString()).ToString(_decimal2casas);
                 
                }
                return redDados;
            }
            catch (Exception ex)
            {
                LogHelper.GravarLog("ERRO gera arquivo RedZ.." + ex.Message);
                LogHelper.GravarLog("ERRO gera arquivo RedZ SQL = " + sql);
            }
            return redDados;
        }
    }
}
