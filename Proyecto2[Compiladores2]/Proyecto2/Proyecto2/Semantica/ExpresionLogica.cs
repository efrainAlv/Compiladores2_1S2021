using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

using System.Windows.Forms;

namespace Proyecto2.Semantica
{
    class ExpresionLogica
    {

        private AST.Nodo nodoE;

        private object resultado;

        private List<Entorno> entorno;
        private List<int> verdaderas;
        private List<int> falsas;

        public ExpresionLogica(ref List<Entorno> entorno)
        {
            this.nodoE = null;
            this.resultado = null;
            this.entorno = entorno;
            this.verdaderas = new List<int>();
            this.falsas= new List<int>();
        }

        public ExpresionLogica(AST.Nodo nodoE)
        {
            this.nodoE = nodoE;
            this.resultado = null;
        }


        public void evaluarExpresion(AST.Nodo nodo)
        {

            //MessageBox.Show("El resultado es: " + noce(nodo));

        }


        public bool noce(AST.Nodo nodoAct)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (temp.Length>0)
            {
                if (temp.Length == 1)
                {
                    if (temp[0].getNombre() == "ASIGNACION1")
                    {
                        Instruccion ins = new Instruccion(ref this.entorno);

                        Cabecera c = new Cabecera();
                        string valor = c.validarAsignacionAVariable(temp[0], "", ins);
                        c = null;
                        ins = null;

                        if (valor == "false")
                        {
                            return false;
                        }
                        else if (valor == "true")
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
                            Condicion c = new Condicion(this.entorno);
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
                        Condicion c = new Condicion(this.entorno);
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


        public int traducir(AST.Nodo nodoAct, int sigEtiqueta)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (temp.Length>0)
            {

                if (temp.Length==3)
                {
                    if (temp[0].getNombre()=="EXP_LOG")
                    {
                        
                        if (temp[1].getHoja().getValor().getValor()+""=="or")
                        {

                            sigEtiqueta = traducir(temp[0], sigEtiqueta);

                            if (sigEtiqueta == 0)
                            {
                                Form1.etiquetas++;
                                Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es verdadero
                                Form1.etiquetas++;
                                Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es falso
                                sigEtiqueta = Form1.etiquetas;

                                Form1.richTextBox2.Text += "L" + Form1.etiquetas + ": "; //si es verdadero
                                sigEtiqueta = traducir(temp[2], sigEtiqueta);

                                if (sigEtiqueta==0)
                                {
                                    Form1.etiquetas++;
                                    Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es verdadero
                                    Form1.etiquetas++;
                                    Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es falso
                                }

                                sigEtiqueta = Form1.etiquetas;
                            }
                            else
                            {
                                Form1.richTextBox2.Text += "L" + sigEtiqueta + ": "; //si es verdadero
                                sigEtiqueta = traducir(temp[2], sigEtiqueta);

                                if (sigEtiqueta==0)
                                {
                                    Form1.etiquetas++;
                                    Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es verdadero
                                    Form1.etiquetas++;
                                    Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es falso
                                }

                                sigEtiqueta = Form1.etiquetas;
                            }
                        }
                        else
                        {
                            sigEtiqueta = traducir(temp[0], sigEtiqueta);

                            if (sigEtiqueta==0)
                            {
                                Form1.etiquetas++;
                                Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es verdadero
                                Form1.etiquetas++;
                                Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es verdadero
                                sigEtiqueta = Form1.etiquetas;

                                Form1.richTextBox2.Text += "L" + (Form1.etiquetas - 1) + ": "; //si es verdadero
                                sigEtiqueta = traducir(temp[2], sigEtiqueta);

                                if (sigEtiqueta==0)
                                {
                                    Form1.etiquetas++;
                                    Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es verdadero
                                    Form1.etiquetas++;
                                    Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es falso
                                }

                                sigEtiqueta = Form1.etiquetas;
                            }
                            else
                            {
                                Form1.richTextBox2.Text += "L" + (sigEtiqueta) + ": "; //si es verdadero
                                sigEtiqueta = traducir(temp[2], sigEtiqueta);

                                if (sigEtiqueta==0)
                                {
                                    Form1.etiquetas++;
                                    Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es verdadero
                                    Form1.etiquetas++;
                                    Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + '\n'; //si es falso
                                }

                                sigEtiqueta = Form1.etiquetas;
                            }
                        }

                    }
                    else
                    {
                        if (temp[0].getHoja()==null)
                        {
                            Expresion e = new Expresion(this.entorno);
                            CodigoI.Temporal t1 = e.traducir(temp[0]);
                            object r1 = e.resultado;
                            CodigoI.Temporal t2 = e.traducir(temp[2]);
                            object r2 = e.resultado;

                            Form1.richTextBox2.Text += "if ( ";

                            if (t1 == null)
                            {
                                Form1.richTextBox2.Text += r1 + " ";
                            }
                            else
                            {
                                Form1.richTextBox2.Text += "T" + t1.indice + " ";
                            }

                            if (temp[1].getNodos()[0].getHoja() != null)
                            {
                                Form1.richTextBox2.Text += temp[1].getNodos()[0].getHoja().getValor().getValor();
                            }
                            else
                            {
                                Form1.richTextBox2.Text += temp[1].getNodos()[0].getNodos()[0].getHoja().getValor().getValor();
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

                            sigEtiqueta = 0;
                        }
                        else
                        {
                            sigEtiqueta = traducir(temp[1], sigEtiqueta);
                        }

                    }

                    return sigEtiqueta;
                }
                else
                {
                    Expresion e = new Expresion(this.entorno);
                    CodigoI.Temporal t1 = e.traducir(temp[1]);
                    object r1 = e.resultado;
                    CodigoI.Temporal t2 = e.traducir(temp[3]);
                    object r2 = e.resultado;

                    Form1.richTextBox2.Text += "if ( ";

                    if (t1==null)
                    {
                        Form1.richTextBox2.Text += r1 + " ";
                    }
                    else
                    {
                        Form1.richTextBox2.Text +=  "T"+t1.indice+ " ";
                    }

                    if (temp[2].getNodos()[0].getHoja() != null)
                    {
                        Form1.richTextBox2.Text += temp[2].getNodos()[0].getHoja().getValor().getValor();
                    }
                    else
                    {
                        Form1.richTextBox2.Text += temp[2].getNodos()[0].getNodos()[0].getHoja().getValor().getValor();
                    }

                    if (t2 == null)
                    {
                        Form1.richTextBox2.Text += " "+r2;
                    }
                    else
                    {
                        Form1.richTextBox2.Text += " T" + t2.indice;
                    }

                    Form1.richTextBox2.Text += " ) ";

                    return 0;
                }


            }

            return sigEtiqueta;
        }


        public void generarEtiquetas(AST.Nodo nodoAct)
        {
            int sigEtiqueta = traducir(nodoAct, 0);

            /*
            verdaderas.Add(sigEtiqueta);
            for (int i = 0; i < this.verdaderas.Count; i++)
            {
                Form1.richTextBox2.Text += "L" + this.verdaderas[i] + ": \n";
            }
            Form1.richTextBox2.Text += "\n";
            for (int i = 0; i < this.falsas.Count; i++)
            {
                Form1.richTextBox2.Text += "L" + this.falsas[i] + ": \n";
            }
            */
        }

    }
}
