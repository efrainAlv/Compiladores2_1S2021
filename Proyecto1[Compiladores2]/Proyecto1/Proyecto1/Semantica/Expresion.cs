using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

using System.Windows.Forms;

namespace Proyecto1.Semantica
{
    public class Expresion
    {

        private AST.Nodo nodoE;

        private object resultado;
        private List<Entorno> entorno;

        public Expresion(List<Entorno> entorno)
        {
            this.nodoE = null;
            this.resultado = null;
            this.entorno = entorno;
        }

        public Expresion(AST.Nodo nodoE)
        {
            this.nodoE = nodoE;
            this.resultado = null;
        }


        public void evaluarExpresion(AST.Nodo nodo)
        {

            //MessageBox.Show("El resultado es: " + noce(nodo));

        }


        public double noce(AST.Nodo nodoAct)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (temp.Length>0)
            {
                if (temp.Length == 3)
                {
                    double n1;
                    double n2;

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
                else if (temp.Length == 2)
                {
                    return -1*noce(temp[1]);
                }
                else
                {

                    if (temp[0].getNombre() == "ASIGNACION1")
                    {
                        double resultado = 0;

                        Instruccion ins = new Instruccion(ref this.entorno);
                        
                        Cabecera c = new Cabecera();
                        string valor = c.validarAsignacionAVariable(temp[0], "", ins);
                        c = null;
                        ins = null;

                        try
                        {
                            resultado = Double.Parse(valor);
                        }
                        catch (FormatException e)
                        {
                            resultado = 0;
                        }

                        temp[0].setValorExp(resultado);
                        return resultado;
                    }
                    else
                    {
                        if (temp[0].getHoja().getValor().getTipo() == Terminal.TipoDato.REAL)
                        {
                            double n1 = Double.Parse(temp[0].getHoja().getValor().getValor() + "");
                            temp[0].setValorExp(n1);

                            return n1;

                        }
                        else
                        {
                            int n1 = Int32.Parse(temp[0].getHoja().getValor().getValor() + "");
                            temp[0].setValorExp(n1);

                            return n1;

                        }
                    }
                    
                }
            }
            else
            {
                return 0;
            }



        }

    }
}
