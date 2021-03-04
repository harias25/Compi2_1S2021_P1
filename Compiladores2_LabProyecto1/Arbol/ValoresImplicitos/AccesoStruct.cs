using Compiladores2_LabProyecto1;
using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using System;
using System.Collections.Generic;
using System.Text;
using static Compiladores2_LabProyecto1.Arbol.ValoresImplicitos.Simbolo;

namespace IDE_C2.Arbol.ValoresImplicitos
{
    class AccesoStruct : Expresion
    {
        public int linea { get; set; }
        public int columna { get; set ; }
        private LinkedList<String> accesos;
        private String id;

        public AccesoStruct(String id, LinkedList<String> accesos, int linea, int columna)
        {
            this.id = id;
            this.accesos = accesos;
            this.linea = linea;
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
            if (ent.existe(id))
            {
                Simbolo simbolo = ent.getSimbolo(id);
                if(simbolo.valor is Objeto)
                {
                    return getValorRecursivo((Objeto)simbolo.valor, accesos,arbol);
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
        }

        private object getValorRecursivo(Objeto obj, LinkedList<String> accesos, AST arbol)
        {
            LinkedList<String> temporales = new LinkedList<String>(accesos);
            String acceso = temporales.First.Value;
            temporales.RemoveFirst();

            //si no esta inicializado el entorno se inicializa
            if (obj.entorno.Tabla.Count == 0)
            {
                //INICIALIZANDO
                obj.ejecutar(null, arbol);
            }

            if (!obj.entorno.existeEnActual(acceso))
            {
                Form1.Consola.AppendText("Error semantico no existe el acceso "+acceso+" dentro del objeto "+ id +", " + linea + " y columna " + columna + "\n");
                return null;
            }

            Simbolo simbolo = obj.entorno.getSimbolo(acceso);
            if (temporales.Count > 0)
            {
                if (simbolo.valor is Objeto)
                {
                    return getValorRecursivo((Objeto)simbolo.valor, temporales,arbol);
                }
                else
                {
                    Form1.Consola.AppendText("Error semantico no el acceso " + acceso + " no es de tipo struct, dentro del objeto " + id + ", linea " + linea + " y columna " + columna + "\n");
                    return null;
                }
            }
            else
            {
                return simbolo.valor;
            }
        }
    }
}
