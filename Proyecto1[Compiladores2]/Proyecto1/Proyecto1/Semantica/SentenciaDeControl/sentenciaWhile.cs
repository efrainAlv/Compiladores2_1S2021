using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica.SentenciaDeControl
{
    public class sentenciaWhile
    {

        private List<Entorno> entorno;

        public sentenciaWhile(List<Entorno> entorno)
        {
            this.entorno = entorno;
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
                        Instruccion inst = new Instruccion(ref this.entorno);
                        inst.analizar(temp[4]);
                    }

                }

            }

        }


    }
}
