using Compiladores2_LabProyecto1;
using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using IDE_C2.Arbol.ValoresImplicitos;
using System;
using System.Collections.Generic;
using System.Linq;
using static Compiladores2_LabProyecto1.Arbol.ValoresImplicitos.Simbolo;

namespace IDE_C2.Arbol.Instrucciones
{
    class Funcion : Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }
        public String id { get; set; }
        private LinkedList<Simbolo> parametros = new LinkedList<Simbolo>();
        private LinkedList<Expresion> valoresParametros;
        public LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
        public Tipos tipo;

        public Funcion(Tipos tipo,String id, LinkedList<Simbolo> parametros, LinkedList<Instruccion> instrucciones,int linea, int columna)
        {
            this.tipo = tipo;
            this.id = id;
            this.parametros = parametros;
            this.instrucciones = instrucciones;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno ent, AST arbol)
        {
            Entorno tablaLocal = new Entorno(ent);

            if (valoresParametros == null)
            {
                valoresParametros = new LinkedList<Expresion>();
            }

            //obtengo antes los valores de los parametros en su respectivo entorno.
            LinkedList<Expresion> resultados = new LinkedList<Expresion>();
            foreach (Expresion e in valoresParametros)
            {
                resultados.AddLast(new Primitivo(e.getValorImplicito(ent, arbol),0,0));
            }

            if (parametros.Count == resultados.Count)
            {
                //declaracion de variables
                for (int i = 0; i < parametros.Count; i++)
                {
                    Simbolo p = parametros.ElementAt(i);
                    Expresion exp = resultados.ElementAt(i);
                    LinkedList<Simbolo> simbolos = new LinkedList<Simbolo>();
                    simbolos.AddLast(p);

                    Declaracion declaracion = new Declaracion(p.tipo, simbolos, exp, exp.linea, exp.columna);
                    declaracion.ejecutar(tablaLocal, arbol);
                }


                foreach(Instruccion e in instrucciones)
                {
                    Object resultado = e.ejecutar(tablaLocal, arbol);

                    if (resultado != null)
                    {
                        return resultado;
                    }
                }
            } else {
                Form1.Consola.AppendText("Error semantico en ejecutar "+ this.id +" en linea " + linea + " y columna " + columna + ", no se tienen los mismos parametros!!\n");
                return null;
            }
            return null;
            }

        public void setValoresParametros(LinkedList<Expresion> a)
        {
            valoresParametros = a;
        }
    }

}
