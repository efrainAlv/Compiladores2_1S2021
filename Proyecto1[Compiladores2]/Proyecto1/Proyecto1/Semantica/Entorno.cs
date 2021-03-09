using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica
{
    class Entorno
    {

        List<Variable> variables;

        public Entorno(ref List<Variable> variables) 
        {
            this.variables = variables;
        }


        public Variable buscarVariable(string nombre)
        {
            for (int i = 0; i < this.variables.Count; i++)
            {
                if (variables[i].getNombre()==nombre)
                {
                    return this.variables[i];
                }
                
            }
            return null;
        }



    }
}
