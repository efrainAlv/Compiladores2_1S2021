using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Semantica.FuncsProcs
{
    public class cicloFor
    {

        List<Entorno> entorno;

        private List<string> valoresParametros = new List<string>();
        private List<string> valoresFunciones = new List<string>();

        private string stackHeap = "STACK";

        public cicloFor(List<Entorno> entorno)
        {
            this.entorno = entorno;
        }


        public cicloFor(List<Entorno> entorno, List<string> procs, List<string> funcs)
        {
            this.entorno = entorno;
            this.valoresParametros = procs;
            this.valoresFunciones = funcs;
        }


        public void analizar(AST.Nodo nodoAct)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            string tipo = nodoAct.getNombre();

            if (temp.Length>0)
            {

                if (tipo=="FOR")
                {
                    Expresion exp = new Expresion(this.entorno);
                    double valorInicial = exp.noce(temp[4]);
                    double valorFinal = exp.noce(temp[6]);
                    string nombreID = temp[1].getHoja().getValor().getValor() + "";

                    this.entorno[this.entorno.Count - 1].actualizarValorDeVariable(nombreID, valorInicial);


                    if (valorInicial<=valorFinal && nombreID.Length>0)
                    {
                        while (Double.Parse(getResultadoDeVariable(new String[] { nombreID })) <= valorFinal)
                        {
                            Form1.continueIns = false;

                            if (Form1.indiceCiclos[Form1.indiceCiclos.Count - 1])
                            {
                                Instruccion inst = new Instruccion(ref this.entorno, this.valoresParametros, this.valoresFunciones);
                                inst.analizar(temp[9]);

                                this.entorno[this.entorno.Count - 1].actualizarValorDeVariable(nombreID, Double.Parse(getResultadoDeVariable(new String[] { nombreID })) + 1);
                            }
                            else
                            {
                                break;
                            }
                            
                        }

                        Form1.continueIns = false;

                    }

                }
            }


        }


        public void traducir(AST.Nodo nodoAct) {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            string tipo = nodoAct.getNombre();

            if(temp.Length>0)
            {

                if (tipo=="FOR")
                {

                    Expresion e = new Expresion(this.entorno);
                    CodigoI.Temporal t1 = e.generarTemporales(temp[4]);
                    object r1 = e.resultado;
                    CodigoI.Temporal t2 = e.generarTemporales(temp[6]);
                    object r2 = e.resultado;

                    //for id : = 10*(5+5) to 20*(2*4) do begin end ;

                    Variable var = buscarVariableEnEntornos(temp[1].getHoja().getValor().getValor() + "");
                    if (t1 == null)
                    {
                        
                        Form1.richTextBox2.Text +=  this.stackHeap+ " = " + r1 + "; \n";
                        
                    }
                    else
                    {
                        Form1.richTextBox2.Text += this.stackHeap+ " = " + "T" + t1.indice + "; \n";
                    }

                    Form1.etiquetas++;
                    int inicio = Form1.etiquetas;
                    Form1.richTextBox2.Text += "L" + Form1.etiquetas + ": ";

                    Form1.richTextBox2.Text += "if ( ";

                    if (t1 == null)
                    {
                        Form1.richTextBox2.Text += this.stackHeap + " >= ";
                    }
                    else
                    {
                        Form1.richTextBox2.Text += "T" + t1.indice + " > ";
                    }

                    if (t2 == null)
                    {
                        Form1.richTextBox2.Text += r2 + " ";
                    }
                    else
                    {
                        Form1.richTextBox2.Text += "T" + t2.indice + " ";
                    }

                    Form1.richTextBox2.Text += ") ";


                    Form1.etiquetas++;
                    int fin = Form1.etiquetas;
                    Form1.richTextBox2.Text += "goto L"+ Form1.etiquetas+";\n";

                    int temporal = CodigoI.Temporal.cantidad;
                    CodigoI.Temporal.cantidad++;
                    Form1.richTextBox2.Text += "T"+temporal+" = "+this.stackHeap+" + 1;\n";
                    Form1.richTextBox2.Text += this.stackHeap +" = T"+temporal+";\n";
                    Form1.richTextBox2.Text += "goto L" + Form1.etiquetas + ";\n";
                    Form1.richTextBox2.Text += "goto L" + inicio + ";\n";

                    Form1.richTextBox2.Text += "L" + fin + ": ";

                }
            
            }

        }


        public string getResultadoDeVariable(string[] ids)
        {

            for (int i = this.entorno.Count - 1; i >= 0; i--)
            {
                Variable varEntorno = this.entorno[i].buscarVariable(ids[0]);

                if (varEntorno != null)
                {
                    Instruccion ins = new Instruccion(ref this.entorno);
                    object resultado = ins.asignarAVariable1(ids, 0, this.entorno[i].buscarVariable(ids[0]), null);

                    return resultado + "";

                }
            }

            return "";
        }


        public Variable buscarVariableEnEntornos(string nombreVar)
        {
            for (int i = this.entorno.Count - 1; i >= 0; i--)
            {
                Variable varEntorno = this.entorno[i].buscarVariable(nombreVar);

                if (varEntorno != null)
                {
                    if (i==0){
                        this.stackHeap = "STACK["+varEntorno.indiceStack+"]";
                    }
                    else
                    {
                        this.stackHeap = "HEAP["+varEntorno.indiceFinStackHeap+"]";
                    }

                    return varEntorno;
                }
            }
            return null;
        }

    }
}
