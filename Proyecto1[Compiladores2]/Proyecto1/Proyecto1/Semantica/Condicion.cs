using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Proyecto1.Semantica
{
    class Condicion
    {

        //
        public void evaluarExpresion(AST.Nodo nodo)
        {

            MessageBox.Show("El resultado es: " + verificar(nodo));

        }

        
        public bool verificar(AST.Nodo nodoAct)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (nodoAct.getNombre()=="CONDICIONES")
            {
                if (temp.Length==3)
                {
                    bool n1 = verificar(temp[0]);
                    bool n2 = verificar(temp[2]);

                    if (temp[1].getNodos().ToArray()[0].getHoja().getValor().getValor()+""=="and")
                    {
                        return n1 && n2;
                    }
                    else
                    {
                        return n1 || n2;
                    }
                }
                else
                {
                    return verificar(temp[0]);
                }
            }
            else
            {
                if (temp[0].getNombre()=="EXP")
                {
                    Expresion exp = new Expresion();
                    
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
                else
                {
                    ExpresionLogica exp = new ExpresionLogica();
                    object n1 = exp.noce(temp[0]);

                    AST.Nodo[] temp1 = temp[0].getNodos().ToArray();

                    if (temp1.Length>0)
                    {
                        object n2 = exp.noce(temp1[1]);

                        if (temp1[0].getHoja().getValor().getValor()+""=="=")
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
                    else
                    {
                        if (n1+""=="true")
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
}
