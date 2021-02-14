using Compiladores2_LabProyecto1;
using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using System;
using System.Collections.Generic;
using System.Text;
using static Compiladores2_LabProyecto1.Arbol.ValoresImplicitos.Simbolo;

namespace IDE_C2.Arbol.Instrucciones
{
    class Declaracion : Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }

        private Tipos tipo { get; set; }

        private Expresion valor { get; set; }

        private LinkedList<Simbolo> variables;

        public Declaracion(Tipos tipo, LinkedList<Simbolo> variables, Expresion valor, int linea, int columna)
        {
            this.variables = variables;
            this.columna = columna;
            this.linea = linea;
            this.valor = valor;
            this.tipo = tipo;
        }

        public object ejecutar(Entorno ent, AST arbol)
        {
            object valor_simbolo = null;
            Tipos tipoResultado;
            if (valor != null)
            {
                valor_simbolo = valor.getValorImplicito(ent, arbol);
                tipoResultado = valor.getTipo(ent, arbol);
            }
            else
            {
                tipoResultado = tipo;
                if (tipo == Tipos.BOOL)
                    valor_simbolo = false;
                else if (tipo == Tipos.DOUBLE)
                    valor_simbolo = 0.0;
                else if (tipo == Tipos.INT)
                    valor_simbolo = 0;
                else if (tipo == Tipos.STRING)
                    valor_simbolo = "";
            }

            foreach (Simbolo variable in variables)
            {
                if (!ent.existe(variable.indentificador))
                {
                    if (tipo == tipoResultado)
                    {
                        variable.valor = valor_simbolo;
                        ent.agregar(variable.indentificador, variable);
                    }
                    else
                    {
                        Form1.Consola.AppendText("Error semantico en Declaracion, no se permiten asignar valores de diferentes tipos en linea " + variable.linea + " y columna " + variable.columna + "\n");
                        return false;
                    }
                }
                else
                {
                    Form1.Consola.AppendText("Error semantico en Declaracion, no se permiten declarar dos id... con el mismo nombre en linea " + variable.linea + " y columna " + variable.columna + "\n");
                    return false;
                }
            }
            return null;
        }
    }
}
