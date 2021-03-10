using Compiladores2_LabProyecto1;
using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using IDE_C2.Arbol.ValoresImplicitos;
using System;
using System.Collections.Generic;
using System.Text;
using static Compiladores2_LabProyecto1.Arbol.ValoresImplicitos.Simbolo;

namespace IDE_C2.Arbol.Instrucciones
{
    class DeclaracionArray : Instruccion
    {
        public int linea { get ; set; }
        public int columna { get ; set ; }

        private String id;
        private LinkedList<Expresion> dimensiones;
        private Tipos tipo;
        private string objeto;

        public DeclaracionArray(String id, LinkedList<Expresion> dimensiones, String objeto, Tipos tipo, int linea, int columna)
        {
            this.id = id;
            this.dimensiones = dimensiones;
            this.tipo = tipo;
            this.objeto = objeto;
            this.linea = linea;
            this.columna = columna;
        }

        private object[] getValores(List<int> niveles, object valor_default)
        {
            List<int> clonedList = new List<int>(niveles);
            int nivel = clonedList[0];
            clonedList.RemoveAt(0);

            object[] arr = new object[nivel];

            //si existen mas niveles
            if (clonedList.Count>0)
            {
                for (int j = 0; j < nivel; j++)
                {
                    arr[j] = getValores(clonedList, valor_default);
                }
            }
            else
            {
                for (int j = 0; j < nivel; j++)
                {
                    if (valor_default is Objeto)
                        arr[j] = ((Objeto)valor_default).Clone();
                    else
                        arr[j] = valor_default;
                }
                
            }

            return arr;
        }

        public object ejecutar(Entorno ent, AST arbol)
        {
            Object valor_default = null;
            if (tipo == Tipos.BOOL)
                valor_default = false;
            else if (tipo == Tipos.DOUBLE)
                valor_default = 0.0;
            else if (tipo == Tipos.INT)
                valor_default = 0;
            else if (tipo == Tipos.STRING)
                valor_default = "";
            else if (tipo == Tipos.STRUCT)  //seccion nueva para objetos
            {
                Objeto obj = arbol.getObjeto(objeto);
                if (obj != null)
                {
                    valor_default = obj;
                }
                else
                {
                    Form1.Consola.AppendText("Error semantico en Declaracion, no existe el Struct " + objeto + " en la linea " + this.linea + " y columna " + this.columna + "\n");
                    return false;
                }
            }


            if (!ent.existeEnActual(id))
            {
                
                List<int> niveles = new List<int>();

                foreach(Expresion exp in dimensiones)
                {
                    niveles.Add((int)exp.getValorImplicito(ent, arbol));
                }

                //se inicializan las dimensiones
                Object[] inicializacion = getValores(niveles, valor_default);

                Arreglo array = new Arreglo(tipo, objeto, inicializacion, niveles);
                Simbolo variable = new Simbolo(Tipos.ARRAY, id, linea, columna, objeto);
                variable.valor = array;
                ent.agregar(id, variable);
            }
            else
            {
                Form1.Consola.AppendText("Error semantico en Declaracion, no se permiten declarar dos id... con el mismo nombre en linea " + linea + " y columna " + columna + "\n");
                return false;
            }
            return null;
        }
    }
}
