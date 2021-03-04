using Compiladores2_LabProyecto1;
using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using System;
using System.Collections.Generic;
using static Compiladores2_LabProyecto1.Arbol.ValoresImplicitos.Simbolo;

namespace IDE_C2.Arbol.Instrucciones
{
    class AsignacionStruct : Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }
        private Expresion valor;
        private String id;
        private LinkedList<String> accesos;

        public AsignacionStruct(String id, LinkedList<String> accesos, Expresion valor, int linea, int columna)
        {
            this.id = id;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
            this.accesos = accesos;
        }

        public object ejecutar(Entorno ent, AST arbol)
        {
            object resultado = valor.getValorImplicito(ent, arbol);
            if (ent.existe(id))
            {
                Simbolo simbolo = ent.getSimbolo(id);
                if (simbolo.valor is Objeto)
                {
                    Objeto obj = (Objeto)simbolo.valor;
                    setValorRecursivo(simbolo,ref obj, resultado, accesos, ent,arbol);
                }
                else
                {
                    Form1.Consola.AppendText("Error semantico no el  id " + id + " no es un objeto, linea " + linea + " y columna " + columna + "\n");
                    return null;
                }
            }
            else
            {
                Form1.Consola.AppendText("Error semantico no existe el  id " + id + " " + linea + " y columna " + columna + "\n");
                return null;
            }
            return null;
        }

        private void setValorRecursivo(Simbolo simbol,ref Objeto obj, Object valor_asigna, LinkedList<String> accesos,Entorno ent, AST arbol)
        {
            LinkedList<String> temporales = new LinkedList<String>(accesos);
            String acceso = temporales.First.Value;
            temporales.RemoveFirst();

            //si no esta inicializado el entorno se inicializa
            if (obj.entorno.Tabla.Count == 0)
            {
                obj.ejecutar(null, arbol);
            }

            if (!obj.entorno.existeEnActual(acceso))
            {
                Form1.Consola.AppendText("Error semantico no existe el acceso " + acceso + " dentro del objeto " + id + ", " + linea + " y columna " + columna + "\n");
                return;
            }

            Simbolo simbolo = obj.entorno.getSimbolo(acceso);
            if (temporales.Count > 0)
            {
                if (simbolo.valor is Objeto)
                {
                    Objeto ob = (Objeto)simbolo.valor;
                    setValorRecursivo(simbolo,ref ob, valor_asigna, temporales, ((Objeto)simbolo.valor).entorno, arbol);
                    simbolo.valor = ob;
                    ent.reemplazar(simbolo.indentificador, simbolo);
                }
                else
                {
                    Form1.Consola.AppendText("Error semantico no el acceso " + acceso + " no es de tipo struct, dentro del objeto " + id + ", linea " + linea + " y columna " + columna + "\n");
                    return;
                }
            }
            else
            {
                Asignacion asignacion = new Asignacion(acceso,new Primitivo(valor_asigna,linea,columna), linea, columna);
                asignacion.ejecutar(obj.entorno, arbol);
                simbol.valor = obj;
                ent.reemplazar(simbolo.indentificador, simbolo);
            }
        }
    }
}
