using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDE_C2.Arbol.Instrucciones
{
    class While : Instruccion
    {
        public int linea { get; set; }
        public int columna { get ; set; }

        private Expresion Condicion;
        public LinkedList<Instruccion> instrucciones;

        public While(Expresion Condicion, LinkedList<Instruccion> instrucciones, int linea, int columna)
        {
            this.Condicion = Condicion;
            this.instrucciones = instrucciones;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno ent, AST arbol)
        {
            Entorno local = new Entorno(ent);
            siguiente:
            if ((bool)Condicion.getValorImplicito(local, arbol))
            {
                foreach (Instruccion objIns in instrucciones)
                {
                    objIns.ejecutar(local, arbol);
                }
                goto siguiente;
            }
            return null;
        }
    }
}
