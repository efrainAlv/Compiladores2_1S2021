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
        public int indiceStack;
        public int tamanio;

        public Variable(string nombre, Terminal valor, int indice, int tamanio)
        {
            this.nombre = nombre;
            this.valor = valor;
            this.tamanio = tamanio;
            this.indiceFinStackHeap = indice;
            this.indiceStack = indice;
            
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


        public void actualizarIndicesObjeto(int i)
        {
            if (this.getValor().getValorObjeto()!=null)
            {
                if (Form1.variableGlobales.Count>1)
                {
                    this.getValor().getValorObjeto().actualizarIndices(0, false);
                }
                
            }
        }


        public void setValorTerminal(object valor)
        {
            this.valor.setValor(valor);
        }


        public void actualizarIndices(int fin)
        {
            this.indiceFinStackHeap = fin + this.indiceFinStackHeap;
            if (this.getValor().getValorObjeto()!=null)
            {
                this.getValor().getValorObjeto().moverIndices(fin);
            }
        }


        public Variable clonar()
        {
            Terminal temp = new Terminal();

            if (this.valor.getValorObjeto()!=null)
            {
                temp = new Terminal(this.valor.getValor(), this.valor.getTipo(), this.valor.getValorObjeto().clonar());
            }
            else
            {
                temp = new Terminal(this.valor.getValor(), this.valor.getTipo());
            }

            return (new Variable(this.nombre, temp, this.indiceFinStackHeap, this.tamanio));
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
