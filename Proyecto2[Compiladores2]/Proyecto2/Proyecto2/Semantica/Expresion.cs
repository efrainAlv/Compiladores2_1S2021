using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

using System.Windows.Forms;

namespace Proyecto2.Semantica
{
    public class Expresion
    {

        private AST.Nodo nodoE;

        public object resultado;
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


        public void traducir(AST.Nodo nodoAct, AST.Nodo nodoSig, AST.Nodo Simbolo)
        {
            CodigoI.Temporal t1 = generarTemporales(nodoAct);
            object r1 = this.resultado;
            CodigoI.Temporal t2 = generarTemporales(nodoSig);
            object r2 = this.resultado;

            Form1.richTextBox2.Text += "if ( ";

            if (t1 == null)
            {
                Form1.richTextBox2.Text += r1 + " ";
            }
            else
            {
                Form1.richTextBox2.Text += "T" + t1.indice + " ";
            }

            if (Simbolo.getNodos()[0].getHoja() != null)
            {
                Form1.richTextBox2.Text += Simbolo.getNodos()[0].getHoja().getValor().getValor();
            }
            else
            {
                Form1.richTextBox2.Text += Simbolo.getNodos()[0].getNodos()[0].getHoja().getValor().getValor();
            }

            if (t2 == null)
            {
                Form1.richTextBox2.Text += " " + r2;
            }
            else
            {
                Form1.richTextBox2.Text += " T" + t2.indice;
            }

            Form1.richTextBox2.Text += " ) ";
        }


        public CodigoI.Temporal generarTemporales(AST.Nodo nodoAct)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (temp.Length>0)
            {
                if (temp.Length==1)
                {
                    this.resultado = temp[0].getHoja().getValor().getValor();
                    return null;
                }
                else if (temp.Length==2)
                {
                    return null;
                }
                else if (temp.Length==3)
                {

                    if (temp[0].getHoja()==null)
                    {
                        AST.Hoja h1 = temp[0].getNodos()[0].getHoja();
                        AST.Hoja h2 = temp[2].getNodos()[0].getHoja();

                        if (h1 != null)
                        {
                            if (h1.getValor().getTipo()==Terminal.TipoDato.SIMBOLO)
                            {
                                h1 = null;
                            }
                        }
                            

                        if (h2 != null)
                        {
                            if (h2.getValor().getTipo() == Terminal.TipoDato.SIMBOLO)
                            {
                                h2 = null;
                            }
                        }
                            
                        if (h1!=null && h2!=null)
                        {
                            CodigoI.Temporal t = new CodigoI.Temporal(Convert.ToDouble(h1.getValor().getValor()), Convert.ToDouble(h2.getValor().getValor()), temp[1].getHoja().getValor().getValor() + "");
                            t = Form1.agregarTemporal(t);

                            return t;
                        }
                        else if (h1!=null && h2==null)
                        {
                            CodigoI.Temporal t2 = generarTemporales(temp[2]);

                            CodigoI.Temporal t1 = new CodigoI.Temporal(Convert.ToDouble(h1.getValor().getValor()), t2, temp[1].getHoja().getValor().getValor() + "");
                            t2 = Form1.agregarTemporal(t1);
                            t1 = null;

                            return t2;
                        }
                        else if (h1 == null && h2 != null)
                        {
                            CodigoI.Temporal t1 = generarTemporales(temp[0]);

                            CodigoI.Temporal t2 = new CodigoI.Temporal(t1, Convert.ToDouble(h2.getValor().getValor()), temp[1].getHoja().getValor().getValor() + "");
                            t1 = Form1.agregarTemporal(t2);
                            return t1;
                        }
                        else
                        {
                            CodigoI.Temporal t1 = generarTemporales(temp[0]);
                            CodigoI.Temporal t2 = generarTemporales(temp[2]);

                            CodigoI.Temporal t3 = new CodigoI.Temporal(t1, t2, temp[1].getHoja().getValor().getValor() + "");
                            t3 = Form1.agregarTemporal(t3);
                            return t3;
                        }


                    }
                    else
                    {
                        return generarTemporales(temp[1]);
                    }

                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

        }




    }
}
