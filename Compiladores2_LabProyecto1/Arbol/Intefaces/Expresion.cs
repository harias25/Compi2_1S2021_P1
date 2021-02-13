using Compiladores2_LabProyecto1.Arbol.ast;
using static Compiladores2_LabProyecto1.Arbol.ValoresImplicitos.Simbolo;

namespace Compiladores2_LabProyecto1.Arbol.Intefaces
{
    interface Expresion
    {
        public int linea { get; set; }
        public int columna { get; set; }
        Tipos getTipo(Entorno ent, AST arbol);
        object getValorImplicito(Entorno ent, AST arbol);
    }
}
