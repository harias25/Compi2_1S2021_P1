using Irony.Parsing;
using System;


namespace Compiladores2_LabProyecto1.Gramaticas
{
    class Gramatica : Irony.Parsing.Grammar
    {
        public Gramatica() : base(false)
        {
            #region DEFINICIÓN DE TERMINALES

            #region PALABRAS RESERVADAS
            KeyTerm
                 pr_print = ToTerm("print"),
                 pr_true = ToTerm("True"),
                 pr_false = ToTerm("False"),
                 pr_println = ToTerm("println"),
                 pr_int = ToTerm("int"),
                 pr_double = ToTerm("double"),
                 pr_string = ToTerm("string"),
                 pr_if = ToTerm("if"),
                 pr_else = ToTerm("else"),
                 pr_for = ToTerm("for"),
                 pr_while = ToTerm("while"),
                 pr_boolean = ToTerm("boolean");

                // Se procede a declarar todas las palabras reservadas que pertenezcan al lenguaje.
                MarkReservedWords("print","println","True", "False", "int", "double", "string","boolean","while","for","else","if");

                #endregion

                #region DECLRACIÓN DE TERMINALES
                    Terminal ptcoma = ToTerm(";"),
                       parizq = ToTerm("("),
                       parder = ToTerm(")"),
                       signo_mas = ToTerm("+"),
                       signo_menos = ToTerm("-"),
                       signo_por = ToTerm("*"),
                       signo_div = ToTerm("/"),
                       or = ToTerm("||"),
                       and = ToTerm("&&"),
                       not = ToTerm("!"),
                       igual_que = ToTerm("=="),
                       distinto = ToTerm("!="),
                       menor = ToTerm("<"),
                       mayor = ToTerm(">"),
                       menor_igual = ToTerm("<="),
                       mayor_igual = ToTerm(">="),
                       igual = ToTerm("="),
                       coma = ToTerm(","),
                       resto = ToTerm("%");
                #endregion

                #region TERMINALES MEDIANTE UNA EXPRESIÓN REGULAR
                    IdentifierTerminal id = new IdentifierTerminal("id");
                    StringLiteral cadena = new StringLiteral("cadena", "\'", StringOptions.IsTemplate);
                    NumberLiteral numero = new NumberLiteral("numero", NumberOptions.None);
                #endregion

                #region COMENTARIOS
                    CommentTerminal comentarioMultilinea = new CommentTerminal("comentarioMultiLinea", "/*", "*/");
                    base.NonGrammarTerminals.Add(comentarioMultilinea);

                    CommentTerminal comentarioUnilinea = new CommentTerminal("comentarioUniLinea", "//", "\n", "\r\n");
                    base.NonGrammarTerminals.Add(comentarioUnilinea);
            #endregion

            #endregion

            #region PRECEDENCIA DE OPERADORES
            RegisterOperators(1, Associativity.Right, igual);
            RegisterOperators(2, Associativity.Left, or);
            RegisterOperators(3, Associativity.Left, and);
            RegisterOperators(4, Associativity.Left, igual_que, distinto);
            RegisterOperators(5, Associativity.Neutral, mayor, menor, mayor_igual, menor_igual);
            RegisterOperators(6, Associativity.Left, signo_mas, signo_menos);
            RegisterOperators(7, Associativity.Left, signo_por, signo_div, resto);
            RegisterOperators(8, Associativity.Right, not);   
            RegisterOperators(9, Associativity.Left, parizq, parder);
            #endregion

            #region DEFINICIÓN DE NO TERMINALES
            NonTerminal INI = new NonTerminal("INIT");
                NonTerminal INSTRUCCIONES = new NonTerminal("INSTRUCCIONES");
                NonTerminal INSTRUCCION = new NonTerminal("INSTRUCCION");
                NonTerminal IMPRIMIR = new NonTerminal("IMPRIMIR");
                NonTerminal ERROR = new NonTerminal("ERROR");
                NonTerminal EXPRESION = new NonTerminal("EXPRESION");
                NonTerminal PRIMITIVA = new NonTerminal("PRIMITIVA");
                NonTerminal EXPRESION_ARITMETICA = new NonTerminal("EXPRESION_ARITMETICA");
                NonTerminal EXPRESION_LOGICA = new NonTerminal("EXPRESION_LOGICA");
                NonTerminal EXPRESION_RELACIONAL = new NonTerminal("EXPRESION_RELACIONAL");
                NonTerminal DECLARACION = new NonTerminal("DECLARACION");
                NonTerminal TIPO = new NonTerminal("TIPO");
                NonTerminal LISTA_SIM = new NonTerminal("LISTA_SIM");
            NonTerminal ASIGNACION = new NonTerminal("ASIGNACION");
            #endregion

            #region DEFINICIÓN DE GRAMATICA

            INI.Rule = INSTRUCCIONES;

            //INSTRUCCIONES
            INSTRUCCIONES.Rule = MakePlusRule(INSTRUCCIONES, INSTRUCCION);
            INSTRUCCION.Rule = IMPRIMIR + ptcoma
                             | DECLARACION + ptcoma
                             | ASIGNACION + ptcoma ;

            //IMPRIMIR
            IMPRIMIR.Rule = pr_print + parizq + EXPRESION + parder
                          | pr_println + parizq + EXPRESION + parder;

            //DECLARACIONES
            DECLARACION.Rule = TIPO + LISTA_SIM
                             | TIPO + id + igual + EXPRESION;  //declaracion

            LISTA_SIM.Rule = MakePlusRule(LISTA_SIM, coma, id);

            //asignacion
            ASIGNACION.Rule = id + igual + EXPRESION;  //asignacion

            //EXPRESIONES
            EXPRESION.Rule = PRIMITIVA
                | EXPRESION_ARITMETICA
                | parizq + EXPRESION + parder
                | EXPRESION_LOGICA
                | EXPRESION_RELACIONAL;


            //NUMERICA
            EXPRESION_ARITMETICA.Rule = EXPRESION + signo_mas + EXPRESION
                            | EXPRESION + signo_menos + EXPRESION
                            | EXPRESION + signo_por + EXPRESION
                            | EXPRESION + signo_div + EXPRESION
                            | EXPRESION + resto + EXPRESION
                            | signo_menos + EXPRESION ; //UNARIA 

            EXPRESION_LOGICA.Rule = EXPRESION + or + EXPRESION
                | EXPRESION + and + EXPRESION
                | not + EXPRESION;

            EXPRESION_RELACIONAL.Rule = EXPRESION + mayor + EXPRESION
                | EXPRESION + menor + EXPRESION
                | EXPRESION + igual_que + EXPRESION
                | EXPRESION + distinto + EXPRESION
                | EXPRESION + mayor_igual + EXPRESION
                | EXPRESION + menor_igual + EXPRESION;

            PRIMITIVA.Rule = numero 
                           | cadena 
                           | pr_true
                           | pr_false
                           | id;  //simbolo

            TIPO.Rule = pr_int | pr_string | pr_boolean | pr_double;

            #endregion


            MarkPunctuation(parizq, parder, ptcoma);
            this.Root = INI;

        }
    }
}
