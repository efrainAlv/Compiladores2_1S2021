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
        bool fin = true;

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



        public int[] generarEtiquetas(AST.Nodo nodoAct, int[] etiquetas)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (temp.Length > 0)
            {

                if (temp.Length == 3)
                {
                    if (temp[0].getNombre() == "EXP_LOG")
                    {

                        etiquetas = generarEtiquetas(temp[0], etiquetas);

                        if (temp[1].getHoja().getValor().getValor() + "" == "or")
                        {
                            if (etiquetas[1]!=1)
                            {

                                for (int i = 0; i < this.falsas.Count; i++)
                                {
                                    Form1.richTextBox2.Text += "L" + falsas[i] + ": "; //si es falso
                                }
                                this.falsas.Clear();

                            }
                            else
                            {
                                Form1.richTextBox2.Text += "L" + etiquetas[0] + ": "; //si es falso
                                this.falsas.Remove(etiquetas[0]);
                            }


                            etiquetas = generarEtiquetas(temp[2], etiquetas);

                        }
                        else
                        {

                            if (etiquetas[1]!=1)
                            {
                                for (int i = 0; i < this.verdaderas.Count; i++)
                                {
                                    Form1.richTextBox2.Text += "L" + verdaderas[i] + ": "; //si es verdadero
                                }
                                this.verdaderas.Clear();

                            }
                            else
                            {
                                Form1.richTextBox2.Text += "L" + etiquetas[1] + ": "; //si es verdadero
                                this.verdaderas.Remove(etiquetas[1]);
                            }
                            
                            etiquetas = generarEtiquetas(temp[2], etiquetas);

                        }

                        return new int[] { etiquetas[0], etiquetas[1], 1 };

                    }
                    else if (temp[0].getNombre() == "(")
                    {
                        return generarEtiquetas(temp[1], etiquetas);
                    }
                    else
                    {
                        if (temp[0].getHoja() == null)
                        {
                            Expresion e = new Expresion(this.entorno);
                            e.traducir(temp[0], temp[2], temp[1]);

                            /*if (temp[0].getNodos()[0].getNombre()=="ASIGNACION1" && temp[2].getNodos()[0].getNombre() == "ASIGNACION1")
                            {
                                e.traducir(temp[0].getNodos()[0], temp[2].getNodos()[0], temp[1]);
                            }
                            else if (temp[0].getNodos()[0].getNombre() != "ASIGNACION1" && temp[2].getNodos()[0].getNombre() == "ASIGNACION1")
                            {
                                e.traducir(temp[0], temp[2].getNodos()[0], temp[1]);
                            }
                            else if (temp[0].getNodos()[0].getNombre() == "ASIGNACION1" && temp[2].getNodos()[0].getNombre() != "ASIGNACION1")
                            {
                                e.traducir(temp[0].getNodos()[0], temp[2], temp[1]);
                            }
                            else if (temp[0].getNodos()[0].getNombre() != "ASIGNACION1" && temp[2].getNodos()[0].getNombre() != "ASIGNACION1")
                            {
                                e.traducir(temp[0], temp[2], temp[1]);
                            }*/

                            Form1.etiquetas+=2;
                            
                            Form1.richTextBox2.Text += "goto L" + (Form1.etiquetas - 1) + ";\n"; //si es verdadero
                            this.verdaderas.Add((Form1.etiquetas - 1));
                            Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + ";\n"; //si es falso
                            this.falsas.Add(Form1.etiquetas);

                            etiquetas = new int[] { Form1.etiquetas, Form1.etiquetas - 1, 0 };
                        }
                        else
                        {
                            etiquetas = generarEtiquetas(temp[1], etiquetas);
                        }

                        return etiquetas;

                    }

                }
                else if (temp.Length == 5)
                {

                    if (temp[1].getNombre()=="EXP")
                    {
                        Expresion e = new Expresion(this.entorno);

                        e.traducir(temp[1], temp[3], temp[2]);
                        /*if (temp[1].getNodos()[0].getNombre() == "ASIGNACION1" && temp[3].getNodos()[0].getNombre() == "ASIGNACION1")
                        {
                            e.traducir(temp[1].getNodos()[0], temp[3].getNodos()[0], temp[2]);
                        }
                        else if (temp[1].getNodos()[0].getNombre() != "ASIGNACION1" && temp[3].getNodos()[0].getNombre() == "ASIGNACION1")
                        {
                            e.traducir(temp[1], temp[3].getNodos()[0], temp[2]);
                        }
                        else if (temp[1].getNodos()[0].getNombre() == "ASIGNACION1" && temp[3].getNodos()[0].getNombre() != "ASIGNACION1")
                        {
                            e.traducir(temp[1].getNodos()[0], temp[3], temp[2]);
                        }
                        else if (temp[1].getNodos()[0].getNombre() != "ASIGNACION1" && temp[3].getNodos()[0].getNombre() != "ASIGNACION1")
                        {
                            e.traducir(temp[1], temp[3], temp[2]);
                        }*/


                        Form1.etiquetas += 2;

                        Form1.richTextBox2.Text += "goto L" + (Form1.etiquetas-1) + ";\n"; //si es verdadero
                        this.verdaderas.Add((Form1.etiquetas - 1));
                        Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + ";\n"; //si es falso
                        this.falsas.Add(Form1.etiquetas);

                        return new int[] { Form1.etiquetas, Form1.etiquetas - 1, 0 }; ;
                    }
                    else
                    {
                        int indiceT1 = CodigoI.Temporal.cantidad;
                        CodigoI.Temporal.cantidad++;

                        int indiceT2 = CodigoI.Temporal.cantidad;
                        CodigoI.Temporal.cantidad++;

                        //*******************************************

                        ExpresionLogica e1 = new ExpresionLogica(ref this.entorno);
                        e1.traducir(temp[1]);

                        e1.imptimirVeraderas();
                        Form1.richTextBox2.Text += "T" + indiceT1 + " = 1; \n";

                        Form1.etiquetas++;
                        int n = Form1.etiquetas;
                        Form1.richTextBox2.Text += "goto L" + n + "; \n";

                        e1.imprimirFalsas();
                        Form1.richTextBox2.Text += "T"+indiceT1 +" = 0; \n";

                        Form1.richTextBox2.Text += "L" + n + ": ";

                        //*******************************************

                        ExpresionLogica e2 = new ExpresionLogica(ref this.entorno);
                        e2.traducir(temp[3]);

                        e2.imptimirVeraderas();
                        Form1.richTextBox2.Text += "T" + indiceT2 + " = 1; \n";

                        Form1.etiquetas++;
                        int m = Form1.etiquetas;
                        Form1.richTextBox2.Text += "goto L" + m + "; \n";

                        e2.imprimirFalsas();
                        Form1.richTextBox2.Text += "T" + indiceT2 + " = 0; \n";

                        Form1.richTextBox2.Text += "L" + m + ": ";

                        //**************************************************************************************

                        Form1.etiquetas++;
                        etiquetas[1] = Form1.etiquetas;
                        Form1.richTextBox2.Text += "if ( T" + indiceT1 + " == T" + indiceT2 + " ) goto L" + Form1.etiquetas +"; \n";
                        this.verdaderas.Add(Form1.etiquetas);
                        Form1.etiquetas++;
                        etiquetas[0] = Form1.etiquetas;
                        Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + "; \n";
                        this.falsas.Add(Form1.etiquetas);

                    }

                }
                else if (temp.Length == 2)
                {
                    etiquetas = generarEtiquetas(temp[1], etiquetas);

                    List<int> listaTemp = this.verdaderas;
                    this.verdaderas = this.falsas;
                    this.falsas = listaTemp;

                    int v = etiquetas[1];
                    etiquetas[1] = etiquetas[0];
                    etiquetas[0] = v;

                }
                else
                {
                    if (temp[0].getHoja()!=null)
                    {
                        Form1.etiquetas += 2;
                        if (temp[0].getHoja().getValor().getValor() + "" == "true")
                        {
                            Form1.richTextBox2.Text += "if ( 1 == 1 ) goto L" + (Form1.etiquetas - 1) + "; \n";
                            this.verdaderas.Add(Form1.etiquetas - 1);

                        }
                        else
                        {
                            Form1.richTextBox2.Text += "if ( 1 == 0 ) goto L" + (Form1.etiquetas - 1) + "; \n";
                            this.verdaderas.Add(Form1.etiquetas - 1);
                        }
                        Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + "; \n";
                        this.falsas.Add(Form1.etiquetas);

                        etiquetas = new int[] { Form1.etiquetas, Form1.etiquetas - 1, 0 };
                    }
                    else
                    {

                    }
                }

            }

            return etiquetas;
        }


        public string generarTemporalesParaAsignaciones(AST.Nodo nodoAct, string varaible)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (nodoAct.getNombre() == "ASIGNACION1")
            {
                varaible += temp[0].getHoja().getValor().getValor();

                return generarTemporalesParaAsignaciones(temp[1], varaible);
            }
            else if (nodoAct.getNombre() == "ASIGNACION2")
            {
                if (temp.Length == 2)
                {
                    varaible += temp[0].getHoja().getValor().getValor();

                    return generarTemporalesParaAsignaciones(temp[1], varaible);
                }
                else if (temp.Length == 3)
                {
                    if (temp[0].getHoja() != null)
                    {
                        return varaible;
                        //llamada
                    }
                    else
                    {
                        return varaible;
                        //arreglo
                    }
                }
                else
                {
                    return varaible;
                    //epsilon
                }
            }
            else
            {
                if (temp.Length == 1)
                {
                    return varaible += temp[0].getHoja().getValor().getValor();
                }
                else
                {
                    varaible += generarTemporalesParaAsignaciones(temp[0], varaible);
                    varaible += temp[1].getHoja().getValor().getValor();

                    return varaible += temp[2].getHoja().getValor().getValor();
                }
            }

        }




        public void traducir(AST.Nodo nodoAct)
        {
            generarEtiquetas(nodoAct, new int[]{ 0,0,0 }) ;
        }


        public void imptimirVeraderas()
        {
            Form1.richTextBox2.Text += "\n";

            for (int i = 0; i < this.verdaderas.Count; i++)
            {
                Form1.richTextBox2.Text += "L" + this.verdaderas[i] + ": ";
            }
        }



        public void imprimirFalsas()
        {
            Form1.richTextBox2.Text += "\n";
            for (int i = 0; i < this.falsas.Count; i++)
            {
                Form1.richTextBox2.Text += "L" + this.falsas[i] + ": ";
            }
        }



        public string getVerdaderasYFalsas()
        {
            string etiquetas = "";
            for (int i = 0; i < this.verdaderas.Count; i++)
            {
                etiquetas += "L"+this.verdaderas[i]+": ";
            }
            etiquetas += "|";
            for (int i = 0; i < this.falsas.Count; i++)
            {
                etiquetas += "L"+this.falsas[i]+": ";
            }
            return etiquetas;
        }


    }
}
