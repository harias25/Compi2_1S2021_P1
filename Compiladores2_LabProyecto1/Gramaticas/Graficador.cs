using Irony.Parsing;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Compiladores2_LabProyecto1.Gramaticas
{
    public class Graficador
    {
        private int index;

        public void graficar(ParseTreeNode nodo)
        {
            StreamWriter archivo = new StreamWriter("ArbolSintactico.dot");
            string contenido = "graph G {";
            contenido += "node [shape = egg];";
            index = 0;
            definirNodos(nodo, ref contenido);
            index = 0;
            enlazarNodos(nodo, 0, ref contenido);
            contenido += "}";
            archivo.Write(contenido);
            archivo.Close();
            DialogResult verImagen = MessageBox.Show("¿Desea visualizar el AST de la cadena ingresada?", "Grafica AST", MessageBoxButtons.YesNo);
            if (verImagen == DialogResult.Yes)
            {
                /*
                ProcessStartInfo startInfo = new ProcessStartInfo(rutaExeDot);
                startInfo.Arguments = "-Tpng ArbolSintactico.dot -o ArbolSintactico.png";
                Process.Start(startInfo);
                Thread.Sleep(2000);
                startInfo.FileName = "ArbolSintactico.png";
                Process.Start(startInfo);*/

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "dot.exe",
                    Arguments = "-Tpng ArbolSintactico.dot -o ArbolSintactico.png",
                    UseShellExecute = false
                };
                Process.Start(startInfo);

                Thread.Sleep(2000);
                generarPagina();

                startInfo = new ProcessStartInfo
                {
                    FileName = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
                    Arguments = "ReporteAST.html",
                    UseShellExecute = false
                };
                Process.Start(startInfo);
            }


        }


        public void generarPagina()
        {
            string pagina = "<html>" + '\n' + "<head>" + '\n' + "<title>AST</title>" + '\n' + "</head>" + '\n';
            pagina = pagina + "<body bgcolor=\"black\">" + '\n' + "<center><Font size=22 color=darkred>" + "Reporte AST" + "</Font></center>" + '\n';
            pagina = pagina + "<hr >" + '\n' + "<font color=white>" + '\n' + "<center>" + '\n';
            pagina = pagina + " <img src='ArbolSintactico.png' alt='AST GENERADO'> ";
            pagina = pagina + '\n' + "</center>" + '\n' + "</table>" + "</body>" + '\n' + "</html>";

            using (StreamWriter outputFile = new StreamWriter("ReporteAST.html", false))
            {
                outputFile.Write(pagina);
            }

        }


        public void definirNodos(ParseTreeNode nodo, ref string contenido)
        {
            if (nodo != null)
            {
                contenido += "node" + index.ToString() + "[label = \"" + nodo.ToString() + "\", style = filled, color = lightblue];";
                index++;

                foreach (ParseTreeNode hijo in nodo.ChildNodes)
                {
                    definirNodos(hijo, ref contenido);
                }
            }
        }

        public void enlazarNodos(ParseTreeNode nodo, int actual, ref string contenido)
        {
            if (nodo != null)
            {
                foreach (ParseTreeNode hijo in nodo.ChildNodes)
                {
                    index++;
                    contenido += "\"node" + actual.ToString() + "\"--" + "\"node" + index.ToString() + "\"";
                    enlazarNodos(hijo, index, ref contenido);
                }
            }
        }

    }
}
