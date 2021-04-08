using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Semantica
{
    public class Entorno
    {

        List<Variable> variables;

        public Entorno(ref List<Variable> variables) 
        {
            this.variables = variables;
        }


        public List<Variable> getVariables()
        {
            return this.variables;
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



        public void actualizarValorDeVariable(string nombre, object valor)
        {
            for (int i = 0; i < this.variables.Count; i++)
            {
                if (variables[i].getNombre() == nombre)
                {
                    this.variables[i].setValorTerminal(valor);
                    break;
                }

            }
        }



    }
}
