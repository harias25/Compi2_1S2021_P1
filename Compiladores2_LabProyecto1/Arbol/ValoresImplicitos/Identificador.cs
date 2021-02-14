using Compiladores2_LabProyecto1;
using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDE_C2.Arbol.ValoresImplicitos
{
    class Identificador : Expresion
    {
        public int linea { get; set ; }
        public int columna { get ; set; }

        private string id;

        public Identificador(String id, int linea, int columna)
        {
            this.id = id;
            this.linea = linea;
            this.columna = columna;
        }

        public Simbolo.Tipos getTipo(Entorno ent, AST arbol)
        {
            if (ent.existe(id))
            {
                Simbolo simbolo = ent.getSimbolo(id);
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
                return simbolo.valor;
            }
            else
            {
                Form1.Consola.AppendText("Error semantico no existe el  id "+id+" "+ linea + " y columna " + columna + "\n");
                return null;
            }
        }
    }
}
