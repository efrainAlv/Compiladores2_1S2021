using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica
{
    public class Objeto
    {

        private List<Variable> atributos;
        private string nombre;

        public Objeto(string nombre)
        {
            this.nombre = nombre;
            this.atributos = new List<Variable>(); ;
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


        public string getNombre()
        {
            return this.nombre;
        }
        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }

    }
}
