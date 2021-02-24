using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.AST
{
    public class Nodo
    {

        private List<Nodo> nodos;
        private Hoja hoja;
        private string nombre;
        private string id;

        private object valorExp;

        public Nodo(string nombre)
        {
            this.nodos = new List<Nodo>();
            this.hoja = null;
            this.nombre = nombre;
            this.id = Guid.NewGuid() + "";
            this.valorExp = 0;
        }

        public Nodo(string nombre, Hoja hoja)
        {
            this.nodos = new List<Nodo>();
            this.hoja = hoja;
            this.nombre = nombre;
            this.id = Guid.NewGuid() + "";
        }


        public void addNodo(Nodo nodo)
        {
            this.nodos.Add(nodo);
        }

        public List<Nodo> getNodos()
        {
            return this.nodos;
        }


        public int getNodosLength()
        {
            return this.nodos.Count;
        }



        public void setHoja(Hoja hoja)
        {
            this.hoja = hoja;
        }

        public Hoja getHoja()
        {
            return this.hoja;
        }

        public string getNombre()
        {
            return this.nombre;
        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }

        public string getId()
        {
            return this.id;
        }


        public void setValorExp(object valor)
        {
            this.valorExp = valor;
        }


        public object getValorExp()
        {
            return this.valorExp;
        }

    }
}
