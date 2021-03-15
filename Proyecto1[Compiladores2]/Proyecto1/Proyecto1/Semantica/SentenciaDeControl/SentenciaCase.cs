using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica.SentenciaDeControl
{
    public class SentenciaCase
    {
        List<Entorno> entorno;

        private List<string> valoresParametros = new List<string>();
        private List<string> valoresFunciones = new List<string>();


        public SentenciaCase(List<Entorno> entorno)
        {
            this.entorno = entorno;
        }


        public SentenciaCase(List<Entorno> entorno, List<string> procs, List<string> funcs)
        {
            this.entorno = entorno;
            this.valoresParametros = procs;
            this.valoresFunciones = funcs;
        }

        public bool analizar(AST.Nodo nodoAct, string valorID)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            string tipo = nodoAct.getNombre();
            int len = nodoAct.getNodos().Count;

            if (tipo=="CASE")
            {

                valorID = getResultadoDeVariable(new string[] { temp[1].getHoja().getValor().getValor()+"" });

                if (analizar(temp[3], valorID))
                {
                    return true;
                }
                else
                {
                    return analizar(temp[4], valorID);
                }


            }
            else if (tipo=="LISTA_CASE")
            {
                if (len==7)
                {
                    if (!analizar(temp[0], valorID))
                    {
                        Instruccion inst = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);

                        if (analizar(temp[1], valorID))
                        {
                            inst.analizar(temp[4]);

                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (analizar(temp[0], valorID))
                    {
                        Instruccion inst = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                        inst.analizar(temp[3]);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if(tipo=="LISTA_CASE1")
            {
                if (len == 3)
                {
                    if (!analizar(temp[0], valorID))
                    {
                        
                        Instruccion inst = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                        string valorCase = inst.valor(temp[2].getNodos().ToArray(), 0);

                        if (valorCase == valorID)
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
                        return true;
                    }

                }
                else
                {
                    Instruccion inst = new Instruccion(ref this.entorno , this.valoresParametros, valoresFunciones);
                    string valorCase = inst.valor(temp[0].getNodos().ToArray(), 0);

                    if (valorCase == valorID)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                }
            }
            else
            {
                Instruccion inst = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                inst.analizar(temp[2]);

                return true;
            }


        }


        public string getResultadoDeVariable(string[] ids)
        {

            for (int i = this.entorno.Count - 1; i >= 0; i--)
            {
                Variable varEntorno = this.entorno[i].buscarVariable(ids[0]);

                if (varEntorno != null)
                {
                    Instruccion ins = new Instruccion(ref this.entorno, this.valoresParametros, valoresFunciones);
                    object resultado = ins.asignarAVariable1(ids, 0, this.entorno[i].buscarVariable(ids[0]), null);

                    return resultado + "";

                }
            }

            return "";
        }


    }
}
