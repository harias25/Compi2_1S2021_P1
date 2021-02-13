using System;
using System.Collections.Generic;
using System.Text;

namespace Compiladores2_LabProyecto1.Arbol.ValoresImplicitos
{
    class Simbolo
    {
        public enum Tipos
        {
            STRING,
            INT,
            DOUBLE,
            BOOL,
            OBJETO,
            FUNCTION,
            PROCEDURE
        }

        public String indentificador { get; set; }
        
        public object valor { get; set; }

        public Tipos tipo { get; set; }

        public int linea { get; set; }
        public int columna { get; set; }

    }
}
