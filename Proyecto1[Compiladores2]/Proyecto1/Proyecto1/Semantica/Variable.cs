using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica
{
    public class Variable
    {

        private string nombre;
        private Terminal valor; 


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
