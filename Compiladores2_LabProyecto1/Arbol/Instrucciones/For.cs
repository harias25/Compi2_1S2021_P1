using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDE_C2.Arbol.Instrucciones
{
    class For : Instruccion
    {
        public int linea { get; set; }
        public int columna { get ; set; }
        private Instruccion inicializacion;
        private Expresion condicion;
        private Instruccion actualizacion;
        private LinkedList<Instruccion> instrucciones;

        public For(Instruccion inicio, Expresion condicion, Instruccion aumento, LinkedList<Instruccion> instruccions, int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.inicializacion = inicio;
            this.condicion = condicion;
            this.actualizacion = aumento;
            this.instrucciones = instruccions;
        }


        public object ejecutar(Entorno ent, AST arbol)
        {
            Entorno local = new Entorno(ent);
            inicializacion.ejecutar(local, arbol);

            siguiente:
            if((bool)condicion.getValorImplicito(local, arbol))
            {
                foreach (Instruccion objIns in instrucciones)
                {
                    if(objIns is Break)
                    {
                        return null;
                    }
                    else if(objIns is Continue)
                    {
                        this.actualizacion.ejecutar(local, arbol);
                        goto siguiente;
                    }
                    else
                    {
                        object resultado = objIns.ejecutar(local, arbol);
                        if (resultado is Break)
                        {
                            return null;
                        }
                        else if (resultado is Continue)
                        {
                            this.actualizacion.ejecutar(local, arbol);
                            goto siguiente;
                        }
                    }
                }
                this.actualizacion.ejecutar(local, arbol);
                goto siguiente;
            }

            return null;
        }
    }
}
