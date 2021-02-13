using Compiladores2_LabProyecto1.Arbol.ast;

namespace Compiladores2_LabProyecto1.Arbol.Intefaces
{
    interface Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }
        object ejecutar(Entorno ent, AST arbol);

    }
}
