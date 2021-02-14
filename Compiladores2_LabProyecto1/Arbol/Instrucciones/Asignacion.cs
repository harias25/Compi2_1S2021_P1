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
    class Asignacion : Instruccion
    {
        public int linea { get ; set ; }
        public int columna { get ; set ; }

        Expresion valor;
        String id;

        public Asignacion(String id, Expresion valor, int linea, int columna)
        {
            this.id = id;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno ent, AST arbol)
        {
            object valor_simbolo = valor.getValorImplicito(ent, arbol);
            Tipos tipoResultado = valor.getTipo(ent, arbol);

            if (ent.existe(id))
            {
                Simbolo simbolo = ent.getSimbolo(id);
                if (simbolo.tipo == tipoResultado)
                {
                    simbolo.valor = valor_simbolo;
                    ent.reemplazar(id, simbolo);
                }
                else
                {
                    Form1.Consola.AppendText("Error semantico en Declaracion, no se permiten asignar valores de diferentes tipos en linea " + linea + " y columna " + columna + "\n");
                    return false;
                }
            }
            else
            {
                Form1.Consola.AppendText("Error semantico no existe el  id " + id + " " + linea + " y columna " + columna + "\n");
                return null;
            }
            return null;
        }
    }
}
