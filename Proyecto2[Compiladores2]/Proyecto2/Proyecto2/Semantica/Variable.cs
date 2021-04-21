using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Semantica
{
    public class Variable
    {

        private string nombre;
        private Terminal valor; 
        public int indiceFinStackHeap;
        public int tamanio;

        public Variable(string nombre, Terminal valor, int indice, int tamanio)
        {
            this.nombre = nombre;
            this.valor = valor;
            this.indiceFinStackHeap = indice;
            this.tamanio = tamanio;

        }

        public Variable(string nombre, Terminal valor)
        {
            this.nombre = nombre;
            this.valor = valor;

        }

        public Variable()
        {

        }


        public Terminal getValor()
        {
            return this.valor;
        }
        public void setValor(Terminal valor)
        {
            this.valor = valor;
        }


        public void setValorTerminal(object valor)
        {
            this.valor.setValor(valor);
        }


        public string getNombre()
        {
            return this.nombre;
        }
        public void setValo(string nombre)
        {
            this.nombre = nombre;
        }


        public void setValorObjeto(Objeto obj)
        {
            this.valor.setValorObjeto(obj);
        }

    }
}
