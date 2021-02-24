using Compiladores2_LabProyecto1;
using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using IDE_C2.Arbol.Instrucciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDE_C2.Arbol.ValoresImplicitos
{
    class Llamada : Expresion, Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }
        public String Id;
        public LinkedList<Expresion> parametros;
        public Llamada(String id, LinkedList<Expresion> valores, int linea, int columna)
        {
            this.Id = id;
            this.parametros = valores;
            this.linea = linea;
            this.columna = columna;
        }

        public Simbolo.Tipos getTipo(Entorno ent, AST arbol)
        {
            Funcion f = arbol.getFuncion(Id);//IDE.getFunction(id);
            if (null != f)
            {
                return f.tipo;
            }
            else
            {
                Form1.Consola.AppendText("La funcion/procedimiento " + Id + " no existen en linea " + linea + " y columna " + columna + "\n");
                return Simbolo.Tipos.VOID;
            }
        }

        public object getValorImplicito(Entorno ent, AST arbol)
        {
            Funcion f = arbol.getFuncion(Id);//IDE.getFunction(id);
            if (null != f)
            {
                f.setValoresParametros(parametros);
                Object rFuncion = f.ejecutar(ent, arbol);
                if (rFuncion is Return) {
                    return null;
                } else
                {
                    return rFuncion;
                }
            }
            else
            {
                Form1.Consola.AppendText("La funcion/procedimiento "+Id+" no existen en linea " + linea + " y columna " + columna + "\n");
            }
            return null;
        }

        public object ejecutar(Entorno ent, AST arbol)
        {
            return getValorImplicito(ent, arbol);
        }
    }
}
