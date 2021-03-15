using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica
{
    public class Write
    {

        List<Entorno> entorno;

        private List<string> valoresParametros = new List<string>();
        private List<string> valoresFunciones = new List<string>();


        public Write(List<Entorno> entorno)
        {
            this.entorno = entorno;
        }


        public Write(List<Entorno> entorno, List<string> procs, List<string> funcs)
        {
            this.entorno = entorno;
            this.valoresParametros = procs;
            this.valoresFunciones = funcs;
        }

        public string analizar(AST.Nodo nodoAct, string valor)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            string tipo = nodoAct.getNombre();
            int len = temp.Length;


            if (tipo == "WRITE")
            {
                valor = analizar(temp[2], valor);

                Form1.richTextBox2.Text = Form1.richTextBox2.Text + valor;

                return "";
            }
            else if (tipo == "WRITELN")
            {
                valor = analizar(temp[2], valor);

                Form1.richTextBox2.Text = Form1.richTextBox2.Text + valor + "\n";

                return "";
            }
            else
            {

                if (len == 3)
                {
                    valor += analizar(temp[0], valor);

                    Instruccion inst = new Instruccion(ref this.entorno, this.valoresParametros, this.valoresFunciones);
                    string valorCase = inst.valor(temp[2].getNodos().ToArray(), 0);

                    return valor += valorCase;

                }
                else
                {
                    Instruccion inst = new Instruccion(ref this.entorno, this.valoresParametros, this.valoresFunciones);
                    string valorCase = inst.valor(temp[0].getNodos().ToArray(), 0);

                    return valor += valorCase;

                }
            }
            

        }

        }


}
