using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Semantica.SentenciaDeControl
{
    public class Repeat
    {

        List<Entorno> entorno;

        private List<string> valoresParametros = new List<string>();
        private List<string> valoresFunciones = new List<string>();


        public Repeat(List<Entorno> entorno)
        {
            this.entorno = entorno;
        }

        public Repeat(List<Entorno> entorno, List<string> procs, List<string> funcs)
        {
            this.entorno = entorno;
            this.valoresParametros = procs;
            this.valoresFunciones = funcs;
        }


        public void analizar(AST.Nodo nodoAct)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            string tipo = nodoAct.getNombre();
            int len = temp.Length;

            if (len > 0)
            {

                if (tipo == "REPEAT")
                {

                    ExpresionLogica e = new ExpresionLogica(ref this.entorno);
                    bool paso = true;

                    while (paso && !Form1.continueIns)
                    {
                        Form1.continueIns = false;
                        if (Form1.indiceCiclos[Form1.indiceCiclos.Count - 1])
                        {
                            if (e.noce(temp[5].getNodos()[0]))
                            {
                                paso = false;
                                break;
                            }
                            else
                            {
                                Instruccion inst = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                                inst.analizar(temp[2]);
                            }
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


        public void traducir(AST.Nodo nodoAct)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            Form1.etiquetas++;
            Form1.richTextBox2.Text += "L" + Form1.etiquetas + ": \n\n"; //etiqueta inicio repeat
            int inicio = Form1.etiquetas;


            //TRADUCIR INSTRUCCIONES


            ExpresionLogica el = new ExpresionLogica(ref this.entorno);
            el.traducir(temp[5].getNodos()[0]);


            el.imprimirFalsas();
            Form1.richTextBox2.Text += "goto L" + inicio + ";\n"; //si la condicion no se cumple

            el.imptimirVeraderas(); //si la condicion se cumple
        }

    }
}
