using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica
{
    public class Terminal
    {

        public enum TipoDato
        {
            CADENA,
            ENTERO,
            REAL,
            BOOLEANO,
            ID,
            SIMBOLO,
            OBJETO
        }

        private object valor;
        private TipoDato tipo;

        public Terminal()
        {

        }

        public Terminal(object valor, TipoDato tipo)
        {
            this.valor = valor;
            this.tipo = tipo;
        }

        public void setValor(object valor)
        {
            this.valor = valor;
        }

        public void setTipo(TipoDato tipo)
        {
            this.tipo = tipo;
        }

        public object getValor()
        {
            return this.valor;
        }

        public TipoDato getTipo()
        {
            return this.tipo;
        }

    }
}
