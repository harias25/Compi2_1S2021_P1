using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDE_C2.Arbol.Instrucciones
{
    class Break : Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }

        public Break(int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
        }
        public object ejecutar(Entorno ent, AST arbol)
        {
            throw new NotImplementedException();
        }
    }
}
