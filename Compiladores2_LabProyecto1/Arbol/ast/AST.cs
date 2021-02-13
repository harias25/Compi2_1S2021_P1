using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using System;
using System.Collections.Generic;

namespace Compiladores2_LabProyecto1.Arbol.ast
{
    class AST
    {

        private LinkedList<Objeto> objetos;
        private LinkedList<Instruccion> instrucciones;

        public AST(LinkedList<Instruccion> instrucciones)
        {
            objetos = new LinkedList<Objeto>();
            this.Instrucciones = instrucciones;
        }

        public void agregarObjetos(Objeto s)
        {
            Objetos.AddLast(s);
        }

        public bool existeObjeto(string nombre)
        {
            foreach (Objeto obj in Objetos)
            {
                if (obj.identificador.Equals(nombre, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }


        public LinkedList<Objeto> Objetos
        {
            get
            {
                return objetos;
            }

            set
            {
                objetos = value;
            }
        }

        public LinkedList<Instruccion> Instrucciones
        {
            get
            {
                return instrucciones;
            }

            set
            {
                instrucciones = value;
            }
        }

    }
}
