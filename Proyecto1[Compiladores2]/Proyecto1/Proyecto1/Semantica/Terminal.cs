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
        private Objeto valorObjeto;
        private Arreglo arreglo;


        public Terminal()
        {

        }

        public Terminal(object valor, TipoDato tipo)
        {
            this.tipo = tipo;
            this.valor = valor;
            this.valorObjeto = null;
            this.arreglo = null;
        }


        public Terminal(object valor, TipoDato tipo, Objeto obj)
        {
            this.tipo = tipo;
            this.valor = valor;
            this.valorObjeto = obj;
            this.arreglo = null;
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

        public Objeto getValorObjeto()
        {
            return this.valorObjeto;
        }

        public void setValorObjeto(Objeto obj)
        {
            this.valorObjeto = obj;
        }


        public void setArreglo(Arreglo arreglo)
        {
            this.arreglo = arreglo;
        }

        public Arreglo getArreglo()
        {
            return this.arreglo;
        }


    }


}
