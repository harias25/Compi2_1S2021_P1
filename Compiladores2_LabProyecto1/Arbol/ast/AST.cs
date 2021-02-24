using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using IDE_C2.Arbol.Instrucciones;
using System;
using System.Collections.Generic;

namespace Compiladores2_LabProyecto1.Arbol.ast
{
    class AST
    {

        private LinkedList<Objeto> objetos;
        private LinkedList<Instruccion> instrucciones;
        public LinkedList<Funcion> funciones;

        public AST(LinkedList<Instruccion> instrucciones, LinkedList<Funcion> funciones, LinkedList<Objeto> objetos)
        {
            this.Instrucciones = instrucciones;
            this.funciones = funciones;
            this.objetos = objetos;
        }

        public void agregarObjetos(Objeto s)
        {
            Objetos.AddLast(s);
        }

        public bool existeFuncion(String id)
        {
            foreach (Funcion f in funciones)
            {
                if (f.id.ToLower().Equals(id.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public Funcion getFuncion(String id)
        {
            foreach (Funcion f in funciones)
            {
                if (f.id.ToLower().Equals(id.ToLower()))
                {
                    return f;
                }
            }
            return null;
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
