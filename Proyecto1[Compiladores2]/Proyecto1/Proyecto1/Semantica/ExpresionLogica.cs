using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

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


        public bool noce(AST.Nodo nodoAct)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (temp.Length>0)
            {
                if (temp.Length == 1)
                {
                    if (temp[0].getNombre() == "ASIGNACION")
                    {
                        Instruccion ins = new Instruccion();

                        string cadena = ins.getAsignaciones(temp[0], "");

                        string[] ids = cadena.Split(".");

                        bool resultado = false;

                        for (int i = 0; i < Form1.variableGlobales.Count; i++)
                        {
                            if (Form1.variableGlobales.ElementAt(i).getNombre() == ids[0])
                            {
                                object nose = ins.asignarAVariable1(ids, 0, Form1.variableGlobales.ElementAt(i), null);

                                if (nose+"" == "false")
                                {
                                    resultado = false;
                                }
                                else
                                {
                                    resultado = true;
                                }

                                break;
                            }
                        }

                        return resultado;
                    }
                    else
                    {
                        string tipo = temp[0].getHoja().getValor().getValor() + "";
                        if (tipo == "true")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if (temp.Length == 2)
                {
                    return !noce(temp[1]);
                }
                else if (temp.Length == 3)
                {

                    if (temp[0].getHoja() == null)
                    {
                        bool n1;
                        bool n2;

                        if (temp[0].getNombre() == "EXP_LOG")
                        {
                            n1 = noce(temp[0]);
                            n2 = noce(temp[2]);

                            if (temp[1].getHoja().getValor().getValor() == "and")
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
                            Condicion c = new Condicion();
                            AST.Nodo n = new AST.Nodo("CONDICION");
                            n.addNodo(temp[0]);
                            n.addNodo(temp[1]);
                            n.addNodo(temp[2]);
                            return c.verificar(n);
                        }
                    }
                    else
                    {
                        return noce(temp[1]);
                    }
                }
                else
                {
                    if (temp[1].getNombre() == "EXP")
                    {
                        Condicion c = new Condicion();
                        AST.Nodo n = new AST.Nodo("CONDICION");
                        n.addNodo(temp[1]);
                        n.addNodo(temp[2]);
                        n.addNodo(temp[3]);
                        return c.verificar(n);
                    }
                    else
                    {
                        bool n1 = noce(temp[1]);
                        bool n2 = noce(temp[3]);

                        string tipo = temp[2].getNodos().ToArray()[0].getHoja().getValor().getValor() + "";
                        if (tipo == "=")
                        {
                            if (n1 == n2)
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
            else
            {
                return false;
            }

        }

    }
}
