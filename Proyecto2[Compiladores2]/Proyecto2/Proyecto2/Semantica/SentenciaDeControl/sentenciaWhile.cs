using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Semantica.SentenciaDeControl
{
    public class sentenciaWhile
    {

        private List<Entorno> entorno;

        private List<string> valoresParametros = new List<string>();
        private List<string> valoresFunciones = new List<string>();

        public sentenciaWhile(List<Entorno> entorno)
        {
            this.entorno = entorno;
        }


        public sentenciaWhile(List<Entorno> entorno, List<string> procs, List<string> funcs)
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

            if (len>0)
            {

                if (tipo=="WHILE")
                {

                    ExpresionLogica e = new ExpresionLogica(ref this.entorno);

                    while (e.noce(temp[1].getNodos()[0]))
                    {
                        Form1.continueIns = false;

                        if (Form1.indiceCiclos[Form1.indiceCiclos.Count-1])
                        {
                            Instruccion inst = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                            inst.analizar(temp[4]);
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
            int inicio = Form1.etiquetas;
            Form1.richTextBox2.Text += "L" + inicio + ": \n";

            ExpresionLogica el = new ExpresionLogica(ref this.entorno);
            el.traducir(temp[1].getNodos()[0]);

            el.imptimirVeraderas();
            Form1.richTextBox2.Text += "goto L" + inicio + ";\n";

            el.imprimirFalsas();
        }

    }
}
