using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;
using static Compiladores2_LabProyecto1.Arbol.ValoresImplicitos.Simbolo;

namespace Compiladores2_LabProyecto1.Arbol.ValoresImplicitos
{
    class PrimitivoTest : Expresion
    {
        public int linea { get ; set ; }
        public int columna { get ; set; }

        private object valor;

        public PrimitivoTest(object valor, int fila, int columna)
        {
            this.valor = valor;
            this.linea = fila;
            this.columna = columna;
        }

        public Simbolo.Tipos getTipo(Entorno ent, AST arbol)
        {
            object valor = this.getValorImplicito(ent, arbol);
            if (valor is bool)
            {
                return Tipos.BOOL;
            }
            else if (valor is string)
            {
                return Tipos.STRING;
            }
            else if (valor is int)
            {
                return Tipos.INT;
            }
            else if (valor is double)
            {
                return Tipos.DOUBLE;
            }
            else if (valor is Decimal)
            {
                return Tipos.DOUBLE;
            }
            else
                return Tipos.STRING;
        }

        public object getValorImplicito(Entorno ent, AST arbol)
        {
            return valor;
        }
    }
}
