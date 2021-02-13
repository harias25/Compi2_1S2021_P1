using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDE_C2.Arbol.Instrucciones
{
    class Declaracion : Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }

        public object ejecutar(Entorno ent, AST arbol)
        {
            throw new NotImplementedException();
        }
    }
}
