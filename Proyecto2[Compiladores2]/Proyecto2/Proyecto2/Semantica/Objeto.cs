using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Semantica
{
    public class Objeto
    {

        private List<Variable> atributos;
        private string nombre;
        private Arreglo arreglo;


        public Objeto(string nombre)
        {
            this.nombre = nombre;
            this.atributos = new List<Variable>(); ;

        }


        public Objeto(string nombre, Arreglo arreglo)
        {
            this.nombre = nombre;
            this.atributos = new List<Variable>(); ;
            this.arreglo = arreglo;
        }


        public void actualizarIndices()
        {
            for (int k = 1; k < this.atributos.Count; k++)
            {
                if (this.atributos[k].getValor().getValorObjeto() != null)
                {
                    this.atributos[k].getValor().getValorObjeto().getAtributo(0).indiceFinStackHeap = this.atributos[k].indiceFinStackHeap - this.atributos[k].getValor().getValorObjeto().getAtributo(0).indiceFinStackHeap;
                    for (int i = 1; i < this.atributos[k].getValor().getValorObjeto().getAtributosLength(); i++)
                    {
                        this.atributos[k].getValor().getValorObjeto().getAtributo(i).indiceFinStackHeap = this.atributos[k].getValor().getValorObjeto().getAtributo(i - 1).indiceFinStackHeap + this.atributos[k].getValor().getValorObjeto().getAtributo(i).tamanio;
                    }
                    //this.atributos[k].getValor().getValorObjeto().getTamanioObjeto() + this.atributos[k].indiceFinStackHeap;
                }
            }
        }


        public void agregarAtributo(Variable var)
        {
            this.atributos.Add(var);
        }

        public void agregarAtributos(List<Variable> vars)
        {
            Variable[] vrs = vars.ToArray();
            for (int i = 0; i < vrs.Length; i++)
            {
                this.atributos.Add(vrs[i]);
            }
            actualizarIndices();
        }

        public Variable buscarAtributo(string nombre)
        {
            Variable[] temp = atributos.ToArray();
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].getNombre()==nombre)
                {
                    return temp[i];
                }
            }
            return null;
        }


        public void actualizarVariable(string nombre, Variable var)
        {
            Variable[] temp = atributos.ToArray();
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].getNombre() == nombre)
                {
                    temp[i] = var;
                    break;
                }
            }
        }

        public int getTamanioObjeto()
        {
            int tamanio = 0;

            for (int i = 0; i < this.atributos.Count; i++)
            {
                tamanio += this.atributos[i].tamanio;
            }

            return tamanio;
        }

        public string getNombre()
        {
            return this.nombre;
        }
        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }


        public List<Variable> getAtributos()
        {
            return this.atributos;
        }

        public Variable getAtributo(int i)
        {
            return this.atributos[i];
        }

        public int getAtributosLength()
        {
            return this.atributos.Count;
        }

    }
}
