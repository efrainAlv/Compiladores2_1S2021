using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica
{
    class Objeto
    {

        List<Variable> atributos;

        public Objeto()
        {
            this.atributos = null;
        }



        public void agregarAtributo(Variable var)
        {
            this.atributos.Add(var);
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


    }
}
