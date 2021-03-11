using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica
{
    public class Parametro
    {

        Variable variable;
        string tipo;

        public Parametro(string nombre, Terminal valor, string tipo)
        {
            this.variable = new Variable(nombre, valor);
            this.tipo = tipo;
        }

        public Parametro(ref Variable variable, string tipo)
        {
            this.variable = variable;
            this.tipo = tipo;
        }

        public Parametro(Variable variable, string tipo)
        {
            this.variable = variable;
            this.tipo = tipo;
        }


        public Variable getVariable()
        {
            return this.variable;
        }

        public string getTipo()
        {
            return this.tipo;
        }




    }
}
