using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeradorArquivosPAF.Model
{
    class DadosPaf
    {
        public string estabCli { get; set; }
        public string iestadualCli { get; set; }
        public string cnpjCli { get; set; }
        public string numCredenCli { get; set; }
        public string nomeCli { get; set; }
        public string versaoPafEcf { get; set; }
        public string nomComercial { get; set; }
        public string cnpjDesenv { get; set; }
        public string nomDesenv { get; set; }
        public string nFabEcf { get; set; }
        public string nTipoEcf { get; set; }
        public string marcaEcf { get; set; }
        public string modeloEcf { get; set; }
        public string versaoSofwEcf { get; set; }
        public string caixa { get; set; }
        public DateTime dataRedz { get; set; }
    }
}
