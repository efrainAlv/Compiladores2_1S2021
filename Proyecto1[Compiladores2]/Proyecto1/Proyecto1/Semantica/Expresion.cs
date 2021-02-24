using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Proyecto1.Semantica
{
    public class Expresion
    {

        private AST.Nodo nodoE;

        private object resultado;

        public Expresion()
        {
            this.nodoE = null;
            this.resultado = null;
        }

        public Expresion(AST.Nodo nodoE)
        {
            this.nodoE = nodoE;
            this.resultado = null;
        }


        public void evaluarExpresion(AST.Nodo nodo)
        {

            MessageBox.Show("El resultado es: " + noce(nodo));

        }


        public object noce(AST.Nodo nodoAct)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (temp.Length == 3)
            {
                object n1;
                object n2;

                if (temp[0].getHoja() == null)
                {
                    //Recursividad
                    n1 = noce(temp[0]);
                    n2 = noce(temp[2]);

                    if (temp[1].getHoja().getValor().getValor() == "+")
                    {
                        return int.Parse(n1 + "") + int.Parse(n2 + "");
                    }
                    else if (temp[1].getHoja().getValor().getValor() == "-")
                    {
                        return int.Parse(n1 + "") - int.Parse(n2 + "");
                    }
                    else if (temp[1].getHoja().getValor().getValor() == "*")
                    {
                        return int.Parse(n1 + "") * int.Parse(n2 + "");
                    }
                    else
                    {
                        return int.Parse(n1 + "") / int.Parse(n2 + "");
                    }

                }
                else
                {
                    return noce(temp[1]);
                }


            }
            else
            {
                temp[0].setValorExp(temp[0].getHoja().getValor().getValor());

                return temp[0].getValorExp();

            }



        }

    }
}
