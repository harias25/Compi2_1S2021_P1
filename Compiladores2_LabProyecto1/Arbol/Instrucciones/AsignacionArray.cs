using Compiladores2_LabProyecto1;
using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using IDE_C2.Arbol.ValoresImplicitos;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDE_C2.Arbol.Instrucciones
{
    class AsignacionArray : Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }
        private Expresion valor;
        private String id;
        private LinkedList<Expresion> niveles;

        public AsignacionArray(String id, LinkedList<Expresion> accesos, Expresion valor, int linea, int columna)
        {
            this.id = id;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
            this.niveles = accesos;
        }

        public object ejecutar(Entorno ent, AST arbol)
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
                    object value = valor.getValorImplicito(ent, arbol);

                    Arreglo array = (Arreglo)simbolo.valor;
                    array.valores = array.setValor(dimensiones, array.valores, value, linea, columna);

                    simbolo.valor = array;
                    ent.reemplazar(id, simbolo);
                    return null;
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
