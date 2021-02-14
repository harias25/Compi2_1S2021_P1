
using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Instrucciones;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using IDE_C2.Arbol.Instrucciones;
using IDE_C2.Arbol.ValoresImplicitos;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using static Compiladores2_LabProyecto1.Arbol.ValoresImplicitos.Simbolo;

namespace Compiladores2_LabProyecto1.Gramaticas
{
    class GeneradorAST
    {

        private ParseTree raiz;
        public AST arbol { get; set; } 

        public GeneradorAST(ParseTree estructura)
        {
            raiz = estructura;
            generar(raiz.Root);
        }

        private void generar(ParseTreeNode raiz)
        {
            arbol = (AST)analizarNodo(raiz);
        }

        private object analizarNodo(ParseTreeNode actual)
        {
            //INICIO DE GRAMATICA
            if (validarUbicacion(actual, "INIT"))
            {
                return analizarNodo(actual.ChildNodes[0]);
            }
            //LISTADO DE INSTRUCCIONES
            else if (validarUbicacion(actual, "INSTRUCCIONES"))
            {
                LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    instrucciones.AddLast((Instruccion)analizarNodo(hijo));
                }
                return new AST(instrucciones);
            }
            if (validarUbicacion(actual, "INSTRUCCION"))
            {
                return analizarNodo(actual.ChildNodes[0]);
            }
            //INSTRUCCION PRINT
            else if (validarUbicacion(actual, "IMPRIMIR"))
            {
                bool salto = false;
                if (getLexema(actual, 0) == "println")
                    salto = true;

                return new Print((Expresion)analizarNodo(actual.ChildNodes[1]), salto, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
            }
            //INSTRUCCION ASIGNACION
            else if (validarUbicacion(actual, "ASIGNACION"))
            {
                return new Asignacion(actual.ChildNodes[0].Token.Text, (Expresion)analizarNodo(actual.ChildNodes[2]), actual.ChildNodes[1].Token.Location.Line, actual.ChildNodes[1].Token.Location.Column);
            }
            //INSTRUCCION DECLARACION 
            else if (validarUbicacion(actual, "DECLARACION"))
            {
                LinkedList<Simbolo> simbolos = new LinkedList<Simbolo>();
                Tipos tipo = (Tipos)analizarNodo(actual.ChildNodes[0]);

                if (actual.ChildNodes.Count == 4)
                {
                    Simbolo simbolo = new Simbolo(tipo, actual.ChildNodes[1].Token.Text, actual.ChildNodes[1].Token.Location.Line, actual.ChildNodes[1].Token.Location.Column);
                    simbolos.AddLast(simbolo);
                    return new Declaracion(tipo, simbolos, (Expresion)analizarNodo(actual.ChildNodes[3]), 0, 0);
                }
                else
                {
                    foreach (ParseTreeNode hijo in actual.ChildNodes[1].ChildNodes)
                    {
                        Simbolo simbolo = new Simbolo(tipo, hijo.Token.Text, hijo.Token.Location.Line, hijo.Token.Location.Column);
                        simbolos.AddLast(simbolo);
                    }
                    return new Declaracion(tipo, simbolos, null, 0, 0);
                }
            }
            else if (validarUbicacion(actual, "TIPO"))
            {
                if (validarUbicacion(actual.ChildNodes[0], "string"))
                {
                    return Tipos.STRING;
                }
                else if (validarUbicacion(actual.ChildNodes[0], "boolean"))
                {
                    return Tipos.BOOL;
                }
                else if (validarUbicacion(actual.ChildNodes[0], "double"))
                {
                    return Tipos.DOUBLE;
                }
                else
                    return Tipos.INT;
            }

            #region EXPRESIONES
            //EXPRESIONES
            else if (validarUbicacion(actual, "EXPRESION"))
            {
                return analizarNodo(actual.ChildNodes[0]);
            }
            //EXPRESIONES ARITMETICAS
            else if (validarUbicacion(actual, "EXPRESION_ARITMETICA"))
            {
                if (actual.ChildNodes.Count == 3)
                {
                    return new Operacion((Expresion)analizarNodo(actual.ChildNodes[0]), (Expresion)analizarNodo(actual.ChildNodes[2]), Operacion.getOperador(getLexema(actual, 1)));
                }
                else if (actual.ChildNodes.Count == 2)
                {
                    return new Operacion((Expresion)analizarNodo(actual.ChildNodes[1]), Operacion.Operador.MENOS_UNARIO);
                }
            }
            //EXPRESIONES RELACIONALES
            else if (validarUbicacion(actual, "EXPRESION_RELACIONAL"))
            {
                return new Operacion((Expresion)analizarNodo(actual.ChildNodes[0]), (Expresion)analizarNodo(actual.ChildNodes[2]), Operacion.getOperador(getLexema(actual, 1)));
            }
            //EXPRESIONES LOGICAS
            else if (validarUbicacion(actual, "EXPRESION_LOGICA"))
            {
                if (actual.ChildNodes.Count == 3)
                {
                    return new Operacion((Expresion)analizarNodo(actual.ChildNodes[0]), (Expresion)analizarNodo(actual.ChildNodes[2]), Operacion.getOperador(getLexema(actual, 1)));
                }
                else if (actual.ChildNodes.Count == 2)
                {
                    return new Operacion((Expresion)analizarNodo(actual.ChildNodes[1]), Operacion.Operador.NOT);
                }
            }
            //PRIMITIVOS
            else if (validarUbicacion(actual, "PRIMITIVA"))
            {
                object value = getLexema(actual, 0);
                if (validarUbicacion(actual.ChildNodes[0], "numero"))
                {
                    try
                    {
                        if (getLexema(actual, 0).Contains("."))
                        {
                            Decimal result = Decimal.Parse(value.ToString());
                            return new Primitivo(result, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
                        }
                        else
                        {
                            int result2 = Convert.ToInt32(value.ToString());
                            return new Primitivo(result2, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
                        }
                    }
                    catch
                    {
                        return new Primitivo(getLexema(actual, 0), actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
                    }

                }
                else if (validarUbicacion(actual.ChildNodes[0], "id"))
                {
                    return new Identificador(value.ToString(), actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
                }
                else if(validarUbicacion(actual.ChildNodes[0], "cadena"))
                {
                    return new Primitivo(value.ToString().Replace("'", ""), actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
                }
                else if (validarUbicacion(actual.ChildNodes[0], "true"))
                {
                    return new Primitivo(true, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);

                }
                else if (validarUbicacion(actual.ChildNodes[0], "false"))
                {
                    return new Primitivo(false, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
                }
            }
            #endregion
            return null;
        }

        private bool validarUbicacion(ParseTreeNode nodo, string nombre)
        {
            return nodo.Term.Name.Equals(nombre, System.StringComparison.InvariantCultureIgnoreCase);
        }

        private string getLexema(ParseTreeNode nodo, int num)
        {
            return nodo.ChildNodes[num].Token.Text;
        }
    }
}
