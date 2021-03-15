using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica.FuncsProcs
{
    public class cicloFor
    {

        List<Entorno> entorno;

        private List<string> valoresParametros = new List<string>();
        private List<string> valoresFunciones = new List<string>();

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


    }
}
