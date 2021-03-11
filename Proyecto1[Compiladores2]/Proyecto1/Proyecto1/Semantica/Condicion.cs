using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Proyecto1.Semantica
{
    public class Condicion
    {
        List<Entorno> entorno;
        public Condicion(List<Entorno> entorno)
        {
            this.entorno = entorno;
        }

        //
        public void evaluarExpresion(AST.Nodo nodo)
        {

            MessageBox.Show("El resultado es: " + verificar(nodo));

        }

        
        public bool verificar(AST.Nodo nodoAct)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
       
            Expresion exp = new Expresion(this.entorno);
                    
            double n1 = Convert.ToSingle((exp.noce(temp[0])));
            double n2 = Convert.ToSingle((exp.noce(temp[2])));

            if (temp[1].getNodos().ToArray()[0].getHoja()!=null)
            {
                string tipo = temp[1].getNodos().ToArray()[0].getHoja().getValor().getValor()+"";

                if (tipo == "<")
                {
                    if (n1<n2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (tipo==">")
                {
                    if (n1 > n2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (tipo == "<=")
                {
                    if (n1 <= n2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (n1 >= n2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                string tipo = temp[1].getNodos().ToArray()[0].getNodos().ToArray()[0].getHoja().getValor().getValor() + "";

                if (tipo=="=")
                {
                    if (n1==n2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (n1 != n2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

        }
        

    }
}
