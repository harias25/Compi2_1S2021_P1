
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
                LinkedList<Instruccion> pila = (LinkedList<Instruccion>)analizarNodo(actual.ChildNodes[0]);
                LinkedList<Objeto> objetos = new LinkedList<Objeto>();
                LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>(); ;
                LinkedList<Funcion> funciones = new LinkedList<Funcion>();
                foreach(Instruccion ins in pila)
                {
                    if (ins is Funcion)
                        funciones.AddLast((Funcion)ins);
                    else if (ins is Objeto)
                        objetos.AddLast((Objeto)ins);
                    else
                        instrucciones.AddLast(ins);
                }

                return new AST(instrucciones,funciones,objetos);
            }
            //LISTADO DE INSTRUCCIONES GLOBALES
            else if (validarUbicacion(actual, "GLOBALES"))
            {
                LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    instrucciones.AddLast((Instruccion)analizarNodo(hijo));
                }
                return instrucciones;
            }
            //LISTADO DE INSTRUCCIONES
            else if (validarUbicacion(actual, "INSTRUCCIONES"))
            {
                LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    instrucciones.AddLast((Instruccion)analizarNodo(hijo));
                }
                return instrucciones;
            }
            else if (validarUbicacion(actual, "INSTRUCCION"))
            {
                return analizarNodo(actual.ChildNodes[0]);
            }
            else if (validarUbicacion(actual, "GLOBAL"))
            {
                return analizarNodo(actual.ChildNodes[0]);
            }
            //definicion de objetos
            else if (validarUbicacion(actual, "STRUCT"))
            {
                LinkedList<Instruccion> declaraciones = new LinkedList<Instruccion>();
                foreach (ParseTreeNode hijo in actual.ChildNodes[3].ChildNodes)
                {
                    declaraciones.AddLast((Instruccion)analizarNodo(hijo));
                }

                return new Objeto(actual.ChildNodes[1].Token.Text, declaraciones, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);

            }
            //acceso ARRAY 
            else if (validarUbicacion(actual, "ACCESO_ARRAY"))
            {
                LinkedList<Expresion> dimensiones = new LinkedList<Expresion>();
                foreach (ParseTreeNode hijo in actual.ChildNodes[1].ChildNodes)
                {
                    dimensiones.AddLast((Expresion)analizarNodo(hijo.ChildNodes[1]));
                }

                return new AccesoArreglo(actual.ChildNodes[0].Token.Text, dimensiones, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
            }
            //asignación a struct 
            else if (validarUbicacion(actual, "ASIGNACION_STRUCT"))
            {
                LinkedList<String> accesos = (LinkedList<String>)analizarNodo(actual.ChildNodes[1]);
                return new AsignacionStruct(actual.ChildNodes[0].Token.Text, accesos,(Expresion)analizarNodo(actual.ChildNodes[3]), actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
            }
            //acceso a struct 
            else if (validarUbicacion(actual, "ACCESO_STRUCT"))
            {
                LinkedList<String> accesos = (LinkedList<String>)analizarNodo(actual.ChildNodes[1]);
                return new AccesoStruct(actual.ChildNodes[0].Token.Text, accesos, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
            }
            else if (validarUbicacion(actual, "ACCESOS"))
            {
                LinkedList<string> listado = new LinkedList<string>();
                int contador = 0;
                foreach (ParseTreeNode hijo in actual.ChildNodes)
                {
                    if (hijo.ChildNodes.Count == 0)
                    {
                        if(getLexema(actual, contador)!=".")
                            listado.AddLast(getLexema(actual, contador));
                    }
                    else
                    {
                        listado.AddLast(getLexema(actual.ChildNodes[contador], 1));
                    }
                    contador = contador + 1;
                }
                return listado;
            }
            //llamada de funciones y procedimientos
            else if (validarUbicacion(actual, "LLAMADA"))
            {
                LinkedList<Expresion> expresions = new LinkedList<Expresion>();
                if (actual.ChildNodes[1].ChildNodes.Count > 0)
                {
                    foreach (ParseTreeNode hijo in actual.ChildNodes[1].ChildNodes)
                    {
                        expresions.AddLast((Expresion)analizarNodo(hijo));
                    }
                }
                return new Llamada(actual.ChildNodes[0].Token.Text, expresions, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
            }
            //funciones y procedimientos
            else if (validarUbicacion(actual, "FUNCION"))
            {
                Tipos tipo = Tipos.STRUCT;
                if (actual.ChildNodes[0].Term.Name == "void")
                    tipo = Tipos.VOID;
                else if (actual.ChildNodes[0].Term.Name == "id")
                    tipo = Tipos.STRUCT;
                else
                    tipo = (Tipos)analizarNodo(actual.ChildNodes[0]);

                LinkedList<Simbolo> parametros = new LinkedList<Simbolo>();
                if (actual.ChildNodes[2].ChildNodes.Count > 0)
                {
                    foreach (ParseTreeNode hijo in actual.ChildNodes[2].ChildNodes)
                    {
                        Tipos subtipo = Tipos.VOID;
                        String Struct = "";
                        if (hijo.ChildNodes[0].Term.Name == "id")
                        {
                            subtipo = Tipos.STRUCT;
                            Struct = hijo.Token.Text;
                        }
                        else
                        {
                            subtipo = (Tipos)analizarNodo(hijo.ChildNodes[0]);
                        }
                        Simbolo simbolo = new Simbolo(subtipo, hijo.ChildNodes[1].Token.Text, hijo.ChildNodes[1].Token.Location.Line, hijo.ChildNodes[1].Token.Location.Column,Struct);
                        parametros.AddLast(simbolo);
                    }
                }
                return new Funcion(tipo, actual.ChildNodes[1].Token.Text, parametros, (LinkedList<Instruccion>)analizarNodo(actual.ChildNodes[3]), actual.ChildNodes[1].Token.Location.Line, actual.ChildNodes[1].Token.Location.Column);
            }
            //return 
            else if (validarUbicacion(actual, "RETURN"))
            {
                return new Return((Expresion)analizarNodo(actual.ChildNodes[1]),actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
            }
            //break
            else if(validarUbicacion(actual, "BREAK"))
            {
                return new Break(actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
            }
            //continue
            else if(validarUbicacion(actual, "CONTINUE"))
            {
                return new Continue(actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
            }
            //while
            else if (validarUbicacion(actual, "WHILE"))
            {
                Expresion condicion = (Expresion)analizarNodo(actual.ChildNodes[1]);
                LinkedList<Instruccion> instruccioenes = (LinkedList<Instruccion>)analizarNodo(actual.ChildNodes[2]);

                return new While(condicion, instruccioenes, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
            }
            //FOR
            else if (validarUbicacion(actual, "FOR"))
            {
                Instruccion inicializacion = (Instruccion)analizarNodo(actual.ChildNodes[1]);
                Instruccion actualizacion = (Instruccion)analizarNodo(actual.ChildNodes[3]);
                LinkedList<Instruccion> bloque = (LinkedList<Instruccion>)analizarNodo(actual.ChildNodes[4]);

                return new For(inicializacion, (Expresion)analizarNodo(actual.ChildNodes[2]), actualizacion, bloque, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
            }
            else if (validarUbicacion(actual, "INICIALIZACION"))
            {

                return analizarNodo(actual.ChildNodes[0]);
            }
            //IF
            else if (validarUbicacion(actual, "IF"))
            {
                Expresion condicionIF = (Expresion)analizarNodo(actual.ChildNodes[1]);
                LinkedList<Instruccion> instruccioenesIF = (LinkedList<Instruccion>)analizarNodo(actual.ChildNodes[2]);
                LinkedList<Instruccion> instruccioenesElse = new LinkedList<Instruccion>();
                LinkedList<If> listadoElseIF = new LinkedList<If>();

                if (actual.ChildNodes[3].ChildNodes.Count > 0)
                {
                    foreach (ParseTreeNode hijo in actual.ChildNodes[3].ChildNodes)
                    {
                        if (hijo.ChildNodes.Count > 0)
                        {
                            if (hijo.ChildNodes.Count == 2)
                            {
                                instruccioenesElse = (LinkedList<Instruccion>)analizarNodo(hijo.ChildNodes[1]);
                            }
                            else
                            {
                                listadoElseIF.AddLast(new If((Expresion)analizarNodo(hijo.ChildNodes[2]), (LinkedList<Instruccion>)analizarNodo(hijo.ChildNodes[3]), null, null, hijo.ChildNodes[0].Token.Location.Line, hijo.ChildNodes[0].Token.Location.Column));
                            }
                        }
                    }
                }

                return new If(condicionIF, instruccioenesIF, instruccioenesElse, listadoElseIF, actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
            }
            //BLOQUE DE SENTENCIAS 
            else if (validarUbicacion(actual, "BLOQUE_SENTENCIAS"))
            {
                return analizarNodo(actual.ChildNodes[1]);
            }
            //DIMENSION
            else if (validarUbicacion(actual, "DIMENSION"))
            {

                return analizarNodo(actual.ChildNodes[0]);
            }
            //BLOQUE DE SENTENCIAS PARA EL IF
            else if (validarUbicacion(actual, "BLOQUE_SENTENCIAS_IF"))
            {

                if (actual.ChildNodes.Count == 3)
                    return analizarNodo(actual.ChildNodes[1]);
                else
                {
                    LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
                    instrucciones.AddLast((Instruccion)analizarNodo(actual.ChildNodes[0]));
                    return instrucciones;
                }
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
                if(actual.ChildNodes.Count == 4)
                {
                    LinkedList<Expresion> dimensiones = new LinkedList<Expresion>();
                    foreach (ParseTreeNode hijo in actual.ChildNodes[1].ChildNodes)
                    {
                        dimensiones.AddLast((Expresion)analizarNodo(hijo.ChildNodes[1]));
                    }
                    return new AsignacionArray(actual.ChildNodes[0].Token.Text, dimensiones, (Expresion)analizarNodo(actual.ChildNodes[3]), actual.ChildNodes[0].Token.Location.Line, actual.ChildNodes[0].Token.Location.Column);
                }
                else
                    return new Asignacion(actual.ChildNodes[0].Token.Text, (Expresion)analizarNodo(actual.ChildNodes[2]), actual.ChildNodes[1].Token.Location.Line, actual.ChildNodes[1].Token.Location.Column);
            }
            //INSTRUCCION DECLARACION 
            else if (validarUbicacion(actual, "DECLARACION"))
            {
                LinkedList<Simbolo> simbolos = new LinkedList<Simbolo>();
                Tipos tipo = Tipos.VOID;
                String nameStruct = "";
                if (actual.ChildNodes[0].Term.Name == "id")
                {
                    tipo = Tipos.STRUCT;
                    nameStruct = actual.ChildNodes[0].Token.Text;
                }
                else
                    tipo = (Tipos)analizarNodo(actual.ChildNodes[0]);

                if (actual.ChildNodes.Count == 4)
                {
                    Simbolo simbolo = new Simbolo(tipo, actual.ChildNodes[1].Token.Text, actual.ChildNodes[1].Token.Location.Line, actual.ChildNodes[1].Token.Location.Column, nameStruct);
                    simbolos.AddLast(simbolo);
                    return new Declaracion(tipo, simbolos, (Expresion)analizarNodo(actual.ChildNodes[3]), 0, 0, nameStruct);
                }
                else if(actual.ChildNodes.Count == 3)
                {
                    LinkedList<Expresion> dimensiones = new LinkedList<Expresion>();
                    foreach (ParseTreeNode hijo in actual.ChildNodes[2].ChildNodes)
                    {
                        dimensiones.AddLast((Expresion)analizarNodo(hijo.ChildNodes[1]));
                    }

                    return new DeclaracionArray(actual.ChildNodes[1].Token.Text, dimensiones, nameStruct, tipo, actual.ChildNodes[1].Token.Location.Line, actual.ChildNodes[1].Token.Location.Column);
                }
                else
                {
                    foreach (ParseTreeNode hijo in actual.ChildNodes[1].ChildNodes)
                    {
                        Simbolo simbolo = new Simbolo(tipo, hijo.Token.Text, hijo.Token.Location.Line, hijo.Token.Location.Column, nameStruct);
                        simbolos.AddLast(simbolo);
                    }
                    return new Declaracion(tipo, simbolos, null, 0, 0, nameStruct);
                }
            }
            else if (validarUbicacion(actual, "TIPO"))
            {
                if (validarUbicacion(actual.ChildNodes[0], "string"))
                {
                    return Tipos.STRING;
                }
                else if (validarUbicacion(actual.ChildNodes[0], "int"))
                {
                    return Tipos.INT;
                }
                else if (validarUbicacion(actual.ChildNodes[0], "void"))
                {
                    return Tipos.VOID;
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
                    return Tipos.STRUCT;
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
                else if (validarUbicacion(actual.ChildNodes[0], "cadena"))
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
