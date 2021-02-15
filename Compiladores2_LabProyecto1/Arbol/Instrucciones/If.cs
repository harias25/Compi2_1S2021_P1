using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDE_C2.Arbol.Instrucciones
{
    class If : Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }
        public Expresion condicion;
        public LinkedList<Instruccion> instrucciones;
        private LinkedList<Instruccion> instrucciones_else;
        private LinkedList<If> listado_else_if;

        public If(Expresion condicion, LinkedList<Instruccion> instrucciones, LinkedList<Instruccion> instrucciones_else, LinkedList<If> else_if, int linea, int columna)
        {
            this.condicion = condicion;
            this.instrucciones = instrucciones;
            this.instrucciones_else = instrucciones_else;
            this.listado_else_if = else_if;
            this.linea = linea;
            this.columna = columna;
        }


        public object ejecutar(Entorno ent, AST arbol)
        {
            if ((bool)condicion.getValorImplicito(ent, arbol)) //se cumple la condicion del else if
            {
                // Si la condicion del if se cumple, entonces ejecuto las instrucciones.
                Entorno local = new Entorno(ent);
                foreach (Instruccion nodo in instrucciones)
                {
                    nodo.ejecutar(local, arbol);
                }
                return null;
            }
            else
            {
                foreach (If elseIf in listado_else_if)
                {
                    if ((bool)(elseIf.condicion.getValorImplicito(ent, arbol)))
                    {
                        // Si la condicion del else - if se cumple, entonces ejecuto las instrucciones.
                        Entorno localElseIf = new Entorno(ent);
                        foreach (Instruccion nodo in elseIf.instrucciones)
                        {
                            nodo.ejecutar(localElseIf, arbol);
                        }
                        return null;
                    }
                }

                //por ultimo si no hay else if, voy a validar si existe un else
                if (instrucciones_else != null)
                {
                    Entorno local = new Entorno(ent);
                    foreach (Instruccion nodo in instrucciones_else)
                    {
                        nodo.ejecutar(local, arbol);
                    }
                    return null;
                }
            }
            return null;
        }
    }
}
