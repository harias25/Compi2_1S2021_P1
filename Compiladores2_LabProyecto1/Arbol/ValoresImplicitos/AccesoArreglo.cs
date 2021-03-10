using Compiladores2_LabProyecto1;
using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDE_C2.Arbol.ValoresImplicitos
{
    

    class AccesoArreglo : Expresion
    {
        public int linea { get; set; }
        public int columna { get; set; }
        private LinkedList<Expresion> niveles;
        public String id;

        public AccesoArreglo(String id, LinkedList<Expresion> expresions, int linea, int columna)
        {
            this.id = id;
            this.niveles = expresions;
            this.linea = linea;
            this.columna = columna;
        }

        public Simbolo.Tipos getTipo(Entorno ent, AST arbol)
        {
            if (ent.existe(id))
            {
                Simbolo simbolo = ent.getSimbolo(id);
                if (simbolo.tipo == Simbolo.Tipos.ARRAY)
                    return ((Arreglo)simbolo.valor).tipo;
                else
                    return simbolo.tipo;
            }
            else
            {
                Form1.Consola.AppendText("Error semantico no existe el  id " + id + " " + linea + " y columna " + columna + "\n");
                return Simbolo.Tipos.VOID;
            }
        }

        public object getValorImplicito(Entorno ent, AST arbol)
        {
            if (ent.existe(id))
            {
                Simbolo simbolo = ent.getSimbolo(id);
                if (simbolo.tipo != Simbolo.Tipos.ARRAY)
                {
                    Form1.Consola.AppendText("Error semantico el  simbolo " + id + ", no es un Arreglo!!, " + linea + " y columna " + columna + "\n");
                    return null;
                }
                else
                {
                    List<int> dimensiones = new List<int>();

                    foreach (Expresion exp in niveles)
                    {
                        dimensiones.Add((int)exp.getValorImplicito(ent, arbol));
                    }
                    Arreglo array = (Arreglo)simbolo.valor;

                    if(dimensiones.Count > array.niveles.Count)
                    {
                        Form1.Consola.AppendText("Error semantico, se intenta acceder a niveles inexistentes en el arreglo!!, " + linea + " y columna " + columna + "\n");
                        return null;
                    }

                    return array.getValor(dimensiones,array.valores, linea, columna); 
                }
            }
            else
            {
                Form1.Consola.AppendText("Error semantico no existe el  id " + id + " " + linea + " y columna " + columna + "\n");
                return null;
            }
        }
    }
}
