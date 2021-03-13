using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica.SentenciaDeControl
{
    public class Repeat
    {

        List<Entorno> entorno;


        public Repeat(List<Entorno> entorno)
        {
            this.entorno = entorno;
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

                    while (paso)
                    {
                        if (Form1.indiceCiclos[Form1.indiceCiclos.Count - 1])
                        {
                            if (e.noce(temp[5].getNodos()[0]))
                            {
                                paso = false;
                                break;
                            }
                            else
                            {
                                Instruccion inst = new Instruccion(ref this.entorno);
                                inst.analizar(temp[2]);
                            }
                        }
                        else
                        {
                            break;
                        }
                        
                    }

                    
                }

            }


        }


    }
}
