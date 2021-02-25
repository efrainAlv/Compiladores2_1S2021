using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Proyecto1.Semantica
{
    class ExpresionLogica
    {

        private AST.Nodo nodoE;

        private object resultado;

        public ExpresionLogica()
        {
            this.nodoE = null;
            this.resultado = null;
        }

        public ExpresionLogica(AST.Nodo nodoE)
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

                    if (temp[1].getHoja().getValor().getValor()+"" == "and")
                    {

                        if (n1+""=="true")
                        {
                            if (n2+""=="true")
                            {

                                // true and true
                                return "true";
                            }
                            else
                            {
                                // true and false
                                return "false";
                            }
                        }
                        else
                        {
                            if (n2+"" == "true")
                            {
                                // false and true
                                return "false";
                            }
                            else
                            {
                                // false and false
                                return "false";
                            }
                        }

                    }
                    else
                    {
                        if (n1+"" == "true")
                        {
                            if (n2+"" == "true")
                            {
                                // true or true
                                return "true";
                            }
                            else
                            {
                                // true or false
                                return "true";
                            }
                        }
                        else
                        {
                            if (n2+"" == "true")
                            {
                                // false or true
                                return "true";
                            }
                            else
                            {
                                // false or false
                                return "false";
                            }
                        }
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
