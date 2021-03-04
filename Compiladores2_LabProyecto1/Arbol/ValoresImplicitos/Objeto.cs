using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiladores2_LabProyecto1.Arbol.ValoresImplicitos
{
    class Objeto : Instruccion,  ICloneable
    {
        public int linea { get; set ; }
        public int columna { get; set ; }
        private LinkedList<Instruccion> declaraciones;
        public Entorno entorno;
        public String id;

        public Objeto(String id, LinkedList<Instruccion> declaraciones, int linea, int columna)
        {
            entorno = new Entorno(null);
            this.id = id;
            this.declaraciones = declaraciones;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno ent, AST arbol)
        {
            this.entorno = new Entorno(null);
            foreach (Instruccion instruccion in declaraciones)
            {
                instruccion.ejecutar(this.entorno, arbol);
            }

            return null;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
