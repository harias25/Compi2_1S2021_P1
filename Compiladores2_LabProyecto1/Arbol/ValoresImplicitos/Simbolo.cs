using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiladores2_LabProyecto1.Arbol.ValoresImplicitos
{
    class Simbolo : Expresion
    {
        public enum Tipos
        {
            STRING,
            INT,
            DOUBLE,
            BOOL,
            VOID,
            STRUCT,
            ARRAY
        }

        public Simbolo(Tipos Tipo, string Identificador, int linea, int columna,String Struct)
        {
            this.tipo = Tipo;
            this.indentificador = Identificador;
            this.linea = linea;
            this.columna = columna;
            this.Struct = Struct;
        }

        public String indentificador { get; set; }
        
        public object valor { get; set; }

        public Tipos tipo { get; set; }

        public int linea { get; set; }
        public int columna { get; set; }
        public String Struct { get; set; }

        public Tipos getTipo(Entorno ent, AST arbol)
        {
            return tipo;
        }

        public object getValorImplicito(Entorno ent, AST arbol)
        {
            return valor;
        }
    }
}
