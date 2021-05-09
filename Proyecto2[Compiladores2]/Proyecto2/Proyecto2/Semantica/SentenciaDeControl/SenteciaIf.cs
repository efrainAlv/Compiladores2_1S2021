using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Proyecto2.Semantica.SentenciaDeControl
{
    class SenteciaIf
    {
        
        private List<Entorno> entorno;

        private List<string> valoresParametros = new List<string>();
        private List<string> valoresFunciones = new List<string>();


        public SenteciaIf(List<Entorno> entorno)
        {
            this.entorno = entorno;
        }

        public SenteciaIf(List<Entorno> entorno, List<string> procs, List<string> funcs)
        {
            this.entorno = entorno;
            this.valoresParametros = procs;
            this.valoresFunciones = funcs;
        }

        public bool analizar(AST.Nodo nodoAct, bool flag)
        {

            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (len>0)
            {
                if (tipo == "IF")
                {
                    ExpresionLogica e = new ExpresionLogica(ref this.entorno);
                    if (e.noce(temp[1].getNodos()[0]))
                    {
                        //MessageBox.Show("If verdadero");

                        Instruccion ins = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                        ins.analizar(temp[4]);

                        return true;
                    }
                    else
                    {
                        //cambiar a 6 cuando esten las instrucciones
                        return analizar(temp[7], false);
                    }
                    e = null;
                }
                else if (tipo == "IF1")
                {
                    if (len == 2)
                    {
                        flag = analizar(temp[0], flag);
                        return analizar(temp[1], flag);
                    }
                    else
                    {
                        if (temp[0].getNombre() == "ELSE")
                        {
                            return analizar(temp[0], flag);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if (tipo == "IF2")
                {
                    if (temp[0].getNombre() == "ELSE")
                    {
                        return analizar(temp[0], flag);
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (tipo == "ELSE_IF")
                {
                    ExpresionLogica e = new ExpresionLogica(ref this.entorno);

                    if (len ==9)
                    {
                        flag = analizar(temp[0], flag); // ELSE_IF

                        if (e.noce(temp[3].getNodos()[0]) && !flag)
                        {
                            //MessageBox.Show("Else if verdadero");

                            Instruccion ins = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                            ins.analizar(temp[6]);

                            return true;
                        }
                        else
                        {
                            return flag;//flag
                        }
                        e = null;
                    }
                    else
                    {
                        if (e.noce(temp[2].getNodos()[0]) && !flag)
                        {
                            //MessageBox.Show("Else if verdadero");

                            Instruccion ins = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                            ins.analizar(temp[5]);

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                        e = null;
                    }

                }
                else
                {

                    if (!flag)
                    {
                        //MessageBox.Show("Else verdadero");

                        if (tipo=="ELSE")
                        {
                            Instruccion ins = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                            ins.analizar(temp[2]);
                            ins = null;

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            else
            {
                return false;
            }
            
        }


        public void traducir(AST.Nodo nodoAct, int etiqueta)
        {

            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (len > 0)
            {
                if (tipo == "IF")
                {
                    ExpresionLogica el = new ExpresionLogica(ref this.entorno);
                    el.traducir(temp[1].getNodos()[0]);

                    el.imptimirVeraderas();
                    //instrucciones
                    Instruccion ins = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                    ins.analizar(temp[4]);

                    Form1.etiquetas++;
                    etiqueta = Form1.etiquetas;
                    Form1.richTextBox2.Text += "goto L" + etiqueta + ";\n";

                    el.imprimirFalsas();
                    traducir(temp[7], etiqueta);

                    Form1.richTextBox2.Text += "L"+ etiqueta +": \n";

                }
                else if (tipo == "IF1")
                {
                    if (len == 2)
                    {
                        traducir(temp[0], etiqueta);

                        traducir(temp[1], etiqueta);
                    }
                    else
                    {
                        if (temp[0].getNombre() == "ELSE")
                        {
                            traducir(temp[0], etiqueta);
                        }
                        else
                        {

                        }
                    }
                }
                else if (tipo == "IF2")
                {
                    if (temp[0].getNombre() == "ELSE")
                    {
                        traducir(temp[0], etiqueta);
                    }
                    else
                    {

                    }
                }
                else if (tipo == "ELSE_IF")
                {

                    if (len == 9)
                    {
                        traducir(temp[0], etiqueta);

                        ExpresionLogica el = new ExpresionLogica(ref this.entorno);
                        el.traducir(temp[3].getNodos()[0]);

                        el.imptimirVeraderas();
                        //instrucciones
                        Instruccion ins = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                        ins.analizar(temp[6]);

                        Form1.richTextBox2.Text += "goto L" + etiqueta + ";\n";

                        el.imprimirFalsas();
                    }
                    else
                    {
                        ExpresionLogica el = new ExpresionLogica(ref this.entorno);
                        el.traducir(temp[2].getNodos()[0]);

                        el.imptimirVeraderas();
                        //instrucciones
                        Instruccion ins = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                        ins.analizar(temp[5]);

                        Form1.richTextBox2.Text += "goto L" + etiqueta + ";\n";

                        el.imprimirFalsas();
                    }

                }
                else
                {

                    if (tipo == "ELSE")
                    {
                        //instrucciones
                        Instruccion ins = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                        ins.analizar(temp[2]);
                        Form1.richTextBox2.Text += "goto L" + etiqueta + ";\n";
                    }
                    else
                    {
                       
                    }

                }

            }
            else
            {

            }

        }


    }
}
