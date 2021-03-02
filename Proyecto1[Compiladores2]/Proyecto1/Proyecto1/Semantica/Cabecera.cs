using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica
{
    class Cabecera
    {


        private AST.Nodo nodo;
        private List<Variable> variablesGLobales;

       public Cabecera(AST.Nodo nodo)
        {
            this.nodo = nodo;
            this.variablesGLobales = new List<Variable>();
        }

        public Cabecera()
        {
            this.variablesGLobales = new List<Variable>();
        }


        public void analizar(AST.Nodo nodoAct)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            string tipo = nodo.getNombre();
            int len = temp.Length;

            if (tipo=="CABECERA")
            {
                if (len==2)
                {
                    analizar(temp[0]);
                    analizar(temp[1]);
                }
                else
                {
                    analizar(temp[0]);
                }
            }
            else if (tipo=="CABECERA1")
            {
                analizar(temp[0]);
                /*
                if (temp[0].getNombre()=="VARIABLE")
                {
                    analizar(temp[0]);
                }
                else if (temp[0].getNombre() == "DECLARACION_OBJETOS")
                {
                    analizar(temp[0]);
                }
                else
                {
                    analizar(temp[0]);
                }
                */
            }
            else if (tipo == "VARIABLE")
            {
                if (len == 3)
                {

                }
                else
                {

                }
            }
            else if (tipo == "VARIABLE1")
            {

            }
            else if (tipo == "LISTA_DEC")
            {

            }
            else if (tipo == "LISTA_DEC1")
            {

            }
            else if (tipo == "DECLARACION")
            {

            }
            else if (tipo == "DECLARACION_OBJETOS")
            {

            }
            else if (tipo == "DECLARACION_OBJETOS1")
            {

            }
            else if (tipo == "ARREGLO")
            {

            }
            else if (tipo == "OBJETO")
            {

            }


        }


        public void agregarVaraibles(AST.Nodo nodo)
        {
            List<string> strs = new List<string>();
            agregarVariableGlobal(nodo, strs);
        }

        public List<string> agregarVariableGlobal(AST.Nodo nodoAct, List<string>variables)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            string tipo = nodoAct.getNombre();
            int len = temp.Length;

            if (tipo=="VARIABLE")
            {
                if (len == 2)
                {
                    return agregarVariableGlobal(temp[1], variables);
                }
                else
                {
                    variables = agregarVariableGlobal(temp[0], variables);
                    return agregarVariableGlobal(temp[2], variables);
                }
            }
            else if (tipo=="VARIABLE1")
            {
                if (len==5)
                {
                    variables = agregarVariableGlobal(temp[0], variables);
                    variables = agregarVariableGlobal(temp[1], variables);

                    string[] vars = variables.ToArray();
                    string tipoVar = temp[3].getNodos()[0].getHoja().getValor().getValor()+"";
                    string valor;
                    Terminal.TipoDato tipoDato;

                    if (tipoVar=="integer")
                    {
                        valor = "0";
                        tipoDato = Terminal.TipoDato.ENTERO;
                    }
                    else if (tipoVar == "real")
                    {
                        valor = "0.0";
                        tipoDato = Terminal.TipoDato.REAL;
                    }
                    else if (tipoVar == "boolean")
                    {
                        valor = "false";
                        tipoDato = Terminal.TipoDato.BOOLEANO;
                    }
                    else if (tipoVar == "string")
                    {
                        valor = "";
                        tipoDato = Terminal.TipoDato.CADENA;
                    }
                    else
                    {
                        valor = "null";
                        tipoDato = Terminal.TipoDato.OBJETO;
                    }

                    for (int i = 0; i < vars.Length; i++)
                    {
                        this.variablesGLobales.Add(new Variable(vars[i], new Terminal(valor, tipoDato)));
                    }

                    return new List<string>();
                }
                else
                {
                    variables = agregarVariableGlobal(temp[0], variables);

                    string[] vars = variables.ToArray();
                    string tipoVar = temp[2].getNodos()[0].getHoja().getValor().getValor() + "";
                    string valor;
                    Terminal.TipoDato tipoDato;

                    if (tipoVar == "integer")
                    {
                        valor = "0";
                        tipoDato = Terminal.TipoDato.ENTERO;
                    }
                    else if (tipoVar == "real")
                    {
                        valor = "0.0";
                        tipoDato = Terminal.TipoDato.REAL;
                    }
                    else if (tipoVar == "boolean")
                    {
                        valor = "false";
                        tipoDato = Terminal.TipoDato.BOOLEANO;
                    }
                    else if (tipoVar == "string")
                    {
                        valor = "";
                        tipoDato = Terminal.TipoDato.CADENA;
                    }
                    else
                    {
                        valor = "null";
                        tipoDato = Terminal.TipoDato.OBJETO;
                    }

                    for (int i = 0; i < vars.Length; i++)
                    {
                        this.variablesGLobales.Add(new Variable(vars[i], new Terminal(valor, tipoDato)));
                    }

                    return new List<string>();
                }
            }
            else
            {
                if (len==1)
                {
                    variables.Add(temp[0].getHoja().getValor().getValor()+"");
                    return variables;
                }
                else
                {
                    variables = agregarVariableGlobal(temp[0], variables);

                    variables.Add(temp[2].getHoja().getValor().getValor() + "");
                    return variables;
                }
            }
            

        }

    }
}
