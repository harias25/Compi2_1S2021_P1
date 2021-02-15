using Compiladores2_LabProyecto1.Arbol.ast;
using Compiladores2_LabProyecto1.Arbol.Intefaces;
using System;
using System.Collections.Generic;
using System.Text;
using static Compiladores2_LabProyecto1.Arbol.ValoresImplicitos.Simbolo;

namespace Compiladores2_LabProyecto1.Arbol.ValoresImplicitos
{
    class Operacion : Expresion
    {
        public int linea { get; set; }
        public int columna { get; set; }
        public Expresion op_izquierda { get; set; }
        public Expresion op_derecha { get; set; }
        public Expresion operandoU { get; set; }
        public Operador operador { get; set; }

        public enum Operador
        {
            SUMA,
            RESTA,
            MULTIPLICACION,
            DIVISION,
            MODULO,
            MENOS_UNARIO,
            MAYOR_QUE,
            MENOR_QUE,
            IGUAL,
            IGUAL_IGUAL,
            DIFERENTE_QUE,
            OR,
            AND,
            NOT,
            MAYOR_IGUA_QUE,
            MENOR_IGUA_QUE,
            DESCONOCIDO
        }

        /// <summary>
        /// Constructor para operaciones BINARIAS
        /// </summary>
        /// <param name="op_izquierda"></param>
        /// <param name="op_derecha"></param>
        /// <param name="operador"></param>
        public Operacion(Expresion op_izquierda, Expresion op_derecha, Operador operador)
        {
            this.op_izquierda = op_izquierda;
            this.op_derecha = op_derecha;
            this.operador = operador;
        }

        /// <summary>
        /// Constructor para operaciones UNARIAS
        /// </summary>
        /// <param name="operandoU"></param>
        /// <param name="operador"></param>
        public Operacion(Expresion operandoU, Operador operador)
        {
            this.operandoU = operandoU;
            this.operador = operador;
        }

        public static Operador getOperador(string op)
        {
            switch (op)
            {
                case "+":
                    return Operador.SUMA;
                case "-":
                    return Operador.RESTA;
                case "*":
                    return Operador.MULTIPLICACION;
                case "/":
                    return Operador.DIVISION;
                case "%":
                    return Operador.MODULO;
                case ">":
                    return Operador.MAYOR_QUE;
                case "<":
                    return Operador.MENOR_QUE;
                case "=":
                    return Operador.IGUAL;
                case "==":
                    return Operador.IGUAL_IGUAL;
                case "!=":
                    return Operador.DIFERENTE_QUE;
                case "||":
                    return Operador.OR;
                case "&&":
                    return Operador.AND;
                case ">=":
                    return Operador.MAYOR_IGUA_QUE;
                case "<=":
                    return Operador.MENOR_IGUA_QUE;
                default:
                    return Operador.DESCONOCIDO;
            }
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
            try
            {
                if (operandoU == null)
                {
                    object op1 = op_izquierda.getValorImplicito(ent, arbol);
                    object op2 = op_derecha.getValorImplicito(ent, arbol);
                    #region SUMA
                    if (operador == Operador.SUMA)
                    {
                        //Tipo resultante de datos: Int
                        if (op1 is int && op2 is int)
                        {
                            return (int)op1 + (int)op2;
                        }
                        //Tipo resultante de datos: Double
                        else if ((op1 is int || op1 is double || op1 is Decimal) && (op2 is double || op2 is int || op2 is Decimal))
                        {
                            return Convert.ToDecimal(op1) + Convert.ToDecimal(op2);
                        }
                        //Tipo resultante de datos: String
                        else if (op1 is string || op2 is string)
                        {
                            if (op1 == null) op1 = "null";
                            if (op2 == null) op2 = "null";
                            return op1.ToString() + op2.ToString();
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando una suma");
                            return null;
                        }
                    }
                    #endregion
                    #region RESTA
                    else if (operador == Operador.RESTA)
                    {
                        //Tipo resultante de datos: Int
                        if (op1 is int && op2 is int)
                        {
                            return (int)op1 - (int)op2;
                        }
                        //Tipo resultante de datos: Double
                        else if ((op1 is int || op1 is double || op1 is Decimal) && (op2 is double || op2 is int || op2 is Decimal))
                        {
                            return Convert.ToDecimal(op1) - Convert.ToDecimal(op2);
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando una resta");
                            return null;
                        }
                    }
                    #endregion
                    #region MULTIPLICACION
                    else if (operador == Operador.MULTIPLICACION)
                    {
                        //Tipo resultante de datos: Int
                        if (op1 is int && op2 is int)
                        {
                            return (int)op1 * (int)op2;
                        }
                        //Tipo resultante de datos: Double
                        else if ((op1 is int || op1 is double || op1 is Decimal) && (op2 is double || op2 is int || op2 is Decimal))
                        {
                            return Convert.ToDecimal(op1) * Convert.ToDecimal(op2);
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando una multiplicación");
                            return null;
                        }
                    }
                    #endregion
                    #region DIVISION
                    else if (operador == Operador.DIVISION)
                    {
                        //Tipo resultante de datos: Double
                        if ((op1 is int || op1 is double || op1 is Decimal) && (op2 is double || op2 is int || op2 is Decimal))
                        {
                            if ((int)op2 == 0)
                            {
                                Form1.Consola.AppendText("Resultado indefinido, no puede ejecutarse operación sobre cero.");
                                return null;
                            }

                            //Tipo resultante de datos: Int
                            if (op1 is int && op2 is int)
                            {
                                return (int)op1 / (int)op2;
                            }
                            return Convert.ToDecimal(op1) / Convert.ToDecimal(op2);
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando una división");
                            return null;
                        }
                    }
                    #endregion
                    #region MODULO
                    else if (operador == Operador.MODULO)
                    {
                        //Tipo resultante de datos: Double
                        if ((op1 is int || op1 is double || op1 is Decimal) && (op2 is double || op2 is int || op2 is Decimal))
                        {
                            if ((int)op2 == 0)
                            {
                                Form1.Consola.AppendText("Resultado indefinido, no puede ejecutarse operación sobre cero.");
                                return null;
                            }

                            //Tipo resultante de datos: Int
                            if (op1 is int && op2 is int)
                            {
                                return (int)op1 % (int)op2;
                            }
                            return Convert.ToDecimal(op1) % Convert.ToDecimal(op2);
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando una división");
                            return null;
                        }
                    }
                    #endregion
                    #region MAYOR
                    else if (operador == Operador.MAYOR_QUE)
                    {
                        if ((op1 is int || op1 is double || op1 is Decimal) && (op2 is double || op2 is int || op2 is Decimal))
                        {
                            return Convert.ToDecimal(op1) > Convert.ToDecimal(op2);
                        }
                        else{
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando >");
                            return null;
                        }
                    }
                    #endregion
                    #region MAYOR_QUE
                    else if (operador == Operador.MAYOR_IGUA_QUE)
                    {
                        if ((op1 is int || op1 is double || op1 is Decimal) && (op2 is double || op2 is int || op2 is Decimal))
                        {
                            return Convert.ToDecimal(op1) >= Convert.ToDecimal(op2);
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando >=");
                            return null;
                        }
                    }
                    #endregion
                    #region MENOR
                    else if (operador == Operador.MENOR_QUE)
                    {
                        if ((op1 is int || op1 is double || op1 is Decimal) && (op2 is double || op2 is int || op2 is Decimal))
                        {
                            return Convert.ToDecimal(op1) < Convert.ToDecimal(op2);
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando <");
                            return null;
                        }
                    }
                    #endregion
                    #region MENOR_IGUAL
                    else if (operador == Operador.MENOR_IGUA_QUE)
                    {
                        if ((op1 is int || op1 is double || op1 is Decimal) && (op2 is double || op2 is int || op2 is Decimal))
                        {
                            return Convert.ToDecimal(op1) <= Convert.ToDecimal(op2);
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando <=");
                            return null;
                        }
                    }
                    #endregion
                    #region IGUAL
                    else if (operador == Operador.IGUAL_IGUAL)
                    {
                        return op1.Equals(op2);
                    }
                    #endregion
                    #region DIFERENTE
                    else if (operador == Operador.DIFERENTE_QUE)
                    {
                        return op1 != op2;
                    }
                    #endregion
                    #region AND
                    else if (operador == Operador.AND)
                    {
                        if (op1 is bool && op2 is bool)
                        {
                            return (bool)op1 && (bool)op2;
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando &&");
                            return null;
                        }
                    }
                    #endregion
                    #region OR
                    else if (operador == Operador.OR)
                    {
                        if (op1 is bool && op2 is bool)
                        {
                            return (bool)op1 || (bool)op2;
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando ||");
                            return null;
                        }
                    }
                    #endregion
                }
                else
                {
                    object opU = operandoU.getValorImplicito(ent, arbol);
                    if (this.operador == Operador.MENOS_UNARIO)
                    {
                        if (opU is Decimal || opU is double)
                        {
                            return -1 * (Decimal)opU;
                        }
                        else if (opU is int)
                        {
                            return -1 * (int)opU;
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error aplicando operador menos unario en la expresio incompatible");
                            return null;
                        }
                    }
                    else if (operador == Operador.NOT)
                    {
                        if (opU is bool)
                        {
                            return !(bool)opU;
                        }
                        else
                        {
                            Form1.Consola.AppendText("Error de tipos de datos no permitidos realizando !");
                            return null;
                        }
                    }
                }
                return null;
            }
            catch
            {
                object op1 = new object(), op2 = new object(), opU = new object();
                Form1.Consola.AppendText("Error realizando la ejecución de la operacion");
                return null;
            }
        }
    }
}
