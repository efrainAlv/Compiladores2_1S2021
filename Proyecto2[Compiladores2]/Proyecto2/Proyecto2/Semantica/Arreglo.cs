using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Semantica
{
    public class Arreglo
    {

        private Variable[] datos;
        private string tipo;

        public Arreglo(string tipo, List<Variable> variables)
        {
            this.datos = variables.ToArray();
            this.tipo = tipo;
        }


        public Variable[] getDatos()
        {
            return this.datos;
        }



    }
}
