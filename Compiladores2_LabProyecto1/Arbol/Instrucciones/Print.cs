using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiladores2_LabProyecto1.Arbol.Instrucciones
{
    class Print : Instruccion
    {
        public int linea { get ; set ; }
        public int columna { get ; set ; }

        private bool SaltoLinea;
        public Expresion expresion { get; set; }

        public Print(Expresion exp, bool salto, int linea, int columna)
        {
            this.expresion = exp;
            this.linea = linea;
            this.columna = columna;
            this.SaltoLinea = salto;
        }

        public object ejecutar(Entorno ent, AST arbol)
        {
            object valor = expresion.getValorImplicito(ent, arbol);
            if (valor != null)
            {
                if(SaltoLinea)
                    Form1.Consola.AppendText(valor.ToString()+ "\n");
                else
                    Form1.Consola.AppendText(valor.ToString());

                return true;
            }
            Form1.Consola.AppendText("Error semantico en Print en linea "+linea+ " y columna "+columna+"\n");
            return false;
        }
    }
}