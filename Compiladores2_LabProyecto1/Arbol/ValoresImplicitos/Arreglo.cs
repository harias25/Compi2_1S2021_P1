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
    class Arreglo
    {
        public Tipos tipo;
        public String tipoObjeto;
        public Object[] valores;
        public List<int> niveles;

        public Arreglo(Tipos tipo, String objeto, Object[] valores, List<int> dimensiones)
        {
            this.tipo = tipo;
            this.tipoObjeto = objeto;
            this.valores = valores;
            this.niveles = dimensiones;
        }

        public object getValor(List<int> posiciones, Object[] niveles, int linea, int columna)
        {
            List<int> clonedList = new List<int>(posiciones);
            int nivel = clonedList[0];
            clonedList.RemoveAt(0);


            if (clonedList.Count > 0)
            {
                if(nivel > niveles.Length-1)
                {
                    Form1.Consola.AppendText("Error semantico, se intenta acceder a una posición inexistente en el arreglo, linea " + linea + " y columna " + columna + "\n");
                    return null;
                }
                else
                {
                    object posicion = niveles[nivel];
                    if (posicion is object[])
                        return getValor(clonedList, (object[])posicion, linea, columna);
                    else
                    {
                        Form1.Consola.AppendText("Error semantico, se intenta acceder a una posición a un valor que no es un arreglo!!, linea " + linea + " y columna " + columna + "\n");
                        return null;
                    }
                }
            }
            else
            {
                if (nivel > niveles.Length - 1)
                {
                    Form1.Consola.AppendText("Error semantico, se intenta acceder a una posición inexistente en el arreglo, linea " + linea + " y columna " + columna + "\n");
                    return null;
                }
                return niveles[nivel];
            }

        }

        public Object[] setValor(List<int> posiciones, Object[] niveles, Object value, int linea, int columna)
        {
            List<int> clonedList = new List<int>(posiciones);
            int nivel = clonedList[0];
            clonedList.RemoveAt(0);


            if (clonedList.Count > 0)
            {
                if (nivel > niveles.Length - 1)
                {
                    Form1.Consola.AppendText("Error semantico, se intenta acceder a una posición inexistente en el arreglo, linea " + linea + " y columna " + columna + "\n");
                    return niveles;
                }
                else
                {
                    object posicion = niveles[nivel];
                    if (posicion is object[])
                    {
                        object[] sub_valores = (object[])posicion;
                        sub_valores = setValor(clonedList, sub_valores, value, linea, columna);
                        niveles[nivel] = sub_valores;
                        return niveles;
                    }
                    else
                    {
                        Form1.Consola.AppendText("Error semantico, se intenta acceder a una posición a un valor que no es un arreglo!!, linea " + linea + " y columna " + columna + "\n");
                        return niveles;
                    }
                }
            }
            else
            {
                if (nivel > niveles.Length - 1)
                {
                    Form1.Consola.AppendText("Error semantico, se intenta acceder a una posición inexistente en el arreglo, linea " + linea + " y columna " + columna + "\n");
                    return niveles;
                }
                niveles[nivel] = value;
                return niveles;
            }
        }



    }
}
