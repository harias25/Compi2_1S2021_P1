using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using System;

namespace IDE_C2.Arbol.ValoresImplicitos
{
    class Return : Expresion, Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }
        private Expresion valorRetorno;

        public Return(Expresion exp, int linea, int columna)
        {
            this.valorRetorno = exp;
            this.linea = linea;
            this.columna=columna;
        }

        public Simbolo.Tipos getTipo(Entorno ent, AST arbol)
        {
            throw new NotImplementedException();
        }

        public object getValorImplicito(Entorno ent, AST arbol)
        {
            return valorRetorno.getValorImplicito(ent,arbol);
        }

        public object ejecutar(Entorno ent, AST arbol)
        {
            return valorRetorno.getValorImplicito(ent, arbol);
        }
    }
}
