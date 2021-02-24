using Compiladores2_LabProyecto1.Arbol.ValoresImplicitos;
using System;
using System.Collections;

namespace Compiladores2_LabProyecto1.Arbol.ast
{
    class Entorno
    {

        private Hashtable tabla;
        private Entorno anterior;

        public Entorno(Entorno anterior)
        {
            Tabla = new Hashtable();
            this.Anterior = anterior;
        }

        public void agregar(string id, Simbolo simbolo)
        {
            id = id.ToLower();
            simbolo.indentificador = simbolo.indentificador.ToLower();
            Tabla.Add(id, simbolo);
        }

        public Entorno eliminar(string id)
        {
            id = id.ToLower();
            for (Entorno e = this; e != null; e = e.Anterior)
            {
                if (e.Tabla.Contains(id))
                {
                    e.Tabla.Remove(id);
                    return e;
                }
            }
            return this;

        }

        public bool existe(string id)
        {
            id = id.ToLower();
            for (Entorno e = this; e != null; e = e.Anterior)
            {
                if (e.Tabla.Contains(id))
                {
                    return true;
                }
            }
            return false;
        }

        public bool existeEnActual(string id)
        {
            id = id.ToLower();
            Simbolo encontrado = (Simbolo)(Tabla[id]);
            return encontrado != null;
        }

        public void agregarEntorno(Entorno ent)
        {
            bool bandera = false;
            Entorno local = this;
            do
            {
                if (local.Anterior == null)
                {
                    local.Anterior = ent;
                    bandera = true;
                }
                else
                {
                    local = local.Anterior;
                }


            } while (!bandera);
        }


        public Simbolo getSimbolo(String id)
        {
            id = id.ToLower();
            for (Entorno e = this; e != null; e = e.Anterior)
            {
                if (e.Tabla.Contains(id))
                {
                    return (Simbolo)e.Tabla[id];
                }
            }
            return null;
            
        }

        public void reemplazar(String id, Simbolo nuevoValor)
        {
            id = id.ToLower();
            for (Entorno e = this; e != null; e = e.Anterior)
            {
                Simbolo encontrado = (Simbolo)(e.Tabla[id]);
                if (encontrado != null)
                {
                    e.Tabla[id] = nuevoValor;
                }
            }
            Console.WriteLine("El simbolo \"" + id + "\" no ha sido declarado en el entorno actual ni en alguno externo");

        }

        public Hashtable Tabla
        {
            get
            {
                return tabla;
            }

            set
            {
                tabla = value;
            }
        }

        public Entorno Anterior
        {
            get
            {
                return anterior;
            }

            set
            {
                anterior = value;
            }
        }

    }
}
