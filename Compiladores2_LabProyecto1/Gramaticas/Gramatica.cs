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
                 pr_continue = ToTerm("continue"),
                 pr_break = ToTerm("break"),
                 pr_void = ToTerm("void"),
                 pr_return = ToTerm("return"),
                 pr_boolean = ToTerm("boolean");

                // Se procede a declarar todas las palabras reservadas que pertenezcan al lenguaje.
                MarkReservedWords("return","void","print","println","True", "False", "int", "double", "string","boolean","while","for","else","if", "break", "continue");

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
                       llaizq = ToTerm("{"),
                       llader = ToTerm("}"),
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
            NonTerminal IF = new NonTerminal("IF");
            NonTerminal BLOQUE_SENTENCIAS_IF = new NonTerminal("BLOQUE_SENTENCIAS_IF");
            NonTerminal BLOQUE_SENTENCIAS = new NonTerminal("BLOQUE_SENTENCIAS");
            NonTerminal LISTADO_ELSE_IF = new NonTerminal("LISTADO_ELSE_IF");
            NonTerminal ELSE_IF = new NonTerminal("ELSE_IF");
            NonTerminal FOR = new NonTerminal("FOR");
            NonTerminal INICIALIZACION = new NonTerminal("INICIALIZACION");
            NonTerminal WHILE = new NonTerminal("WHILE");
            NonTerminal CONTINUE = new NonTerminal("CONTINUE");
            NonTerminal BREAK = new NonTerminal("BREAK");
            NonTerminal RETURN = new NonTerminal("RETURN");
            NonTerminal GLOBALES = new NonTerminal("GLOBALES");
            NonTerminal GLOBAL = new NonTerminal("GLOBAL");

            NonTerminal FUNCION = new NonTerminal("FUNCION");
            NonTerminal PARAMETROS = new NonTerminal("PARAMETROS");
            NonTerminal PARAMETRO = new NonTerminal("PARAMETRO");
            
            NonTerminal LLAMADA = new NonTerminal("LLAMADA");
            NonTerminal EXPRESIONES = new NonTerminal("EXPRESIONES");
            #endregion

            #region DEFINICIÓN DE GRAMATICA

            INI.Rule = GLOBALES;

            GLOBALES.Rule = MakePlusRule(GLOBALES, GLOBAL);
            GLOBAL.Rule = DECLARACION + ptcoma
                        | FUNCION;

            //INSTRUCCIONES
            BLOQUE_SENTENCIAS.Rule = llaizq + INSTRUCCIONES + llader;

            INSTRUCCIONES.Rule = MakePlusRule(INSTRUCCIONES, INSTRUCCION);
            INSTRUCCION.Rule = IMPRIMIR + ptcoma
                             | DECLARACION + ptcoma
                             | ASIGNACION + ptcoma 
                             | IF 
                             | FOR 
                             | WHILE
                             | CONTINUE 
                             | BREAK 
                             | RETURN 
                             | LLAMADA + ptcoma;


            //DEFINICION DE FUNCIONES Y/O PROCEDIMIENTOS
            FUNCION.Rule = TIPO + id +parizq +PARAMETROS+ parder+ BLOQUE_SENTENCIAS
                         | pr_void + id + parizq + PARAMETROS + parder + BLOQUE_SENTENCIAS;

            PARAMETROS.Rule = MakeStarRule(PARAMETROS,coma, PARAMETRO);
            PARAMETRO.Rule = TIPO + id
                           | id + id ;

            //LLAMADA DE FUNCIONES Y/O PROCEDIMIENTOS
            LLAMADA.Rule = id + parizq + EXPRESIONES + parder;
            EXPRESIONES.Rule = MakeStarRule(EXPRESIONES, coma, EXPRESION);

            RETURN.Rule = pr_return + EXPRESION + ptcoma;

            //BLOQUE_SENTENCIAS IF
            BLOQUE_SENTENCIAS_IF.Rule = llaizq + INSTRUCCIONES + llader;
                                    //  | INSTRUCCION;   

            //FOR
            FOR.Rule = pr_for + parizq + INICIALIZACION + ptcoma + EXPRESION_RELACIONAL + ptcoma + ASIGNACION + parder + BLOQUE_SENTENCIAS_IF;
            INICIALIZACION.Rule = DECLARACION | ASIGNACION;

            //while
            WHILE.Rule = pr_while + parizq + EXPRESION + parder + BLOQUE_SENTENCIAS_IF;

            CONTINUE.Rule = pr_continue + ptcoma;
            BREAK.Rule = pr_break + ptcoma;

            //IF
            IF.Rule = pr_if + parizq + EXPRESION + parder + BLOQUE_SENTENCIAS_IF + LISTADO_ELSE_IF;

            LISTADO_ELSE_IF.Rule = MakePlusRule(LISTADO_ELSE_IF, ELSE_IF);

            ELSE_IF.Rule = pr_else + pr_if + parizq + EXPRESION + parder + BLOQUE_SENTENCIAS_IF
                         | pr_else + BLOQUE_SENTENCIAS_IF
                         | Empty;

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
                | EXPRESION_RELACIONAL
                | LLAMADA ;


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
