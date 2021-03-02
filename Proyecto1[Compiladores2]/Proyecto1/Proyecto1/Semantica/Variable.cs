using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica
{
    class Variable
    {

        private string nombre;
        private Terminal valor;

        public Variable(string nombre, Terminal valor)
        {
            this.nombre = nombre;
            this.valor = valor;

        }


        public Terminal getValor()
        {
            return this.valor;
        }
        public void setValor(Terminal valor)
        {
            this.valor = valor;
        }


        public string getNombre()
        {
            return this.nombre;
        }
        public void setValor(string nombre)
        {
            this.nombre = nombre;
        }


    }
}
