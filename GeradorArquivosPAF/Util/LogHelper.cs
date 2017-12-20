using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeradorArquivosPAF.Util
{
    public class LogHelper
    {
        private static string _arquivo
        {
            get
            {
                string caminho = System.Reflection.Assembly.GetExecutingAssembly().Location;
                caminho = System.IO.Path.GetDirectoryName(caminho);

                if (!System.IO.File.Exists("Logs"))
                    System.IO.Directory.CreateDirectory("Logs");

                string nome = caminho
                   + @"\"
                   + "Logs\\"
                   + "log_" + Environment.UserName + "_"
                   + DateTime.Now.ToString("yyyyMMdd")
                   + ".txt";
                return nome;
            }
        }
        private static string _arquivo2
        {
            get
            {
                string caminho = System.Reflection.Assembly.GetExecutingAssembly().Location;
                caminho = System.IO.Path.GetDirectoryName(caminho);

                if (!System.IO.File.Exists("Logs"))
                    System.IO.Directory.CreateDirectory("Logs");

                string nome = caminho
                   + @"\"
                   + "Logs\\"
                    + "log_" + Environment.UserName + "_"
                   + DateTime.Now.ToString("yyyyMMddHHmmss")
                   + ".txt";
                return nome;
            }
        }
        public static void GravarLog(Exception exp, string msg)
        {
            try
            {
                StreamWriter writer = new StreamWriter(_arquivo, true);

                writer.WriteLine(string.Format("Log: {0}", DateTime.Now.ToString()));
                writer.WriteLine("----------------------------------------------------------------");

                writer.WriteLine(msg);

                writer.WriteLine(exp.Message);

                if (exp.InnerException != null)
                    writer.WriteLine(exp.InnerException.Message);

                writer.WriteLine("-------------------------------//-------------------------------");
                writer.WriteLine(string.Empty);
                writer.WriteLine("-------------------");
                writer.WriteLine("==== StackTrace ===");
                writer.WriteLine("-------------------");
                writer.WriteLine(exp.StackTrace);

                writer.WriteLine("-------------------------------//-------------------------------");
                writer.WriteLine(string.Empty);
                writer.WriteLine(string.Empty);
                writer.Close();
            }
            catch (Exception ex)
            {
                StreamWriter writer = new StreamWriter(_arquivo2, true);

                writer.WriteLine(string.Format("Log: {0}", DateTime.Now.ToString()));
                writer.WriteLine("----------------------------------------------------------------");

                writer.WriteLine(ex.Message);

                writer.WriteLine("-------------------------------//-------------------------------");
                writer.Close();
            }
        }

        public static void GravarLog(string msg)
        {
            try
            {
                StreamWriter writer = new StreamWriter(_arquivo, true);

                writer.WriteLine(string.Format("Log: {0}", DateTime.Now.ToString()));
                writer.WriteLine("----------------------------------------------------------------");

                writer.WriteLine(msg);

                writer.WriteLine("-------------------------------//-------------------------------");
                writer.Close();
            }
            catch (Exception ex)
            {
                StreamWriter writer = new StreamWriter(_arquivo2, true);

                writer.WriteLine(string.Format("Log: {0}", DateTime.Now.ToString()));
                writer.WriteLine("----------------------------------------------------------------");

                writer.WriteLine(ex.Message);

                writer.WriteLine("-------------------------------//-------------------------------");
                writer.Close();
            }
        }
    }
}
