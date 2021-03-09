using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;


namespace Proyecto1.Semantica
{
    public class Cabecera
    {


        private AST.Nodo nodo;
        private List<Variable> variablesGLobales;
        private List<Objeto> objetos;

        public Cabecera(AST.Nodo nodo)
        {
            this.nodo = nodo;
            this.variablesGLobales = new List<Variable>();
        }

        public Cabecera()
        {
            this.variablesGLobales = new List<Variable>();
            this.objetos = new List<Objeto>();
        }


        public Objeto buscarObjeto(string nombre)
        {
            Objeto[] temp = this.objetos.ToArray();
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].getNombre()==nombre)
                {
                    return temp[i];
                }
            }

            return null;
        }


        public void analizar(AST.Nodo nodoAct)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            string tipo = nodoAct.getNombre();
            int len = temp.Length;

            if (len>0)
            {
                if (tipo == "CABECERA")
                {
                    if (len == 2)
                    {
                        analizar(temp[0]);
                        analizar(temp[1]);
                    }
                    else
                    {
                        analizar(temp[0]);
                    }
                }
                else if (tipo == "CABECERA1")
                {
                    if (temp[0].getNombre() == "VARIABLE")
                    {
                        agregarVariableGlobal(temp[0], new List<string>(), this.objetos);
                    }
                    else if (temp[0].getNombre() == "DECLARACION_OBJETOS")
                    {
                        agregarObjetos(temp[0]);
                    }

                }

            }


        }


        public void agregarVaraibles(AST.Nodo nodo, List<Objeto> objetos)
        {
            List<string> strs = new List<string>();
            agregarVariableGlobal(nodo, strs, objetos);
        }


        public List<Variable> agregarObjetos(AST.Nodo nodoAct)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            string tipo = nodoAct.getNombre();
            int len = temp.Length;

            if (len>0)
            {
                if (tipo == "DECLARACION_OBJETOS")
                {
                    string nombreTipo;
                    nombreTipo = temp[1].getHoja().getValor().getValor() + "";
                    Objeto o = new Objeto(nombreTipo);
                    o.agregarAtributos(agregarObjetos(temp[3]));
                    this.objetos.Add(o);
                    return null;

                }
                else
                {

                    if (temp[0].getNombre() == "OBJETO")
                    {
                        Cabecera c = new Cabecera();
                        c.agregarVaraibles(temp[0].getNodos().ToArray()[1], this.objetos);
                        return c.getVariables();
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            else
            {
                return new List<Variable>();
            }


        }


        public List<string> agregarVariableGlobal(AST.Nodo nodoAct, List<string>variables, List<Objeto> objetos)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            string tipo = nodoAct.getNombre();
            int len = temp.Length;

            if (len>0)
            {
                if (tipo == "VARIABLE")
                {
                    if (len == 2)
                    {
                        return agregarVariableGlobal(temp[1], variables, objetos);
                    }
                    else
                    {
                        variables = agregarVariableGlobal(temp[0], variables, objetos);
                        return agregarVariableGlobal(temp[2], variables, objetos);
                    }
                }
                else if (tipo == "VARIABLE1")
                {
                    if (temp[0].getNombre() == "VARIABLE1")
                    {
                        variables = agregarVariableGlobal(temp[0], variables, objetos);
                        variables = agregarVariableGlobal(temp[1], variables, objetos);

                        //NUMERO 4

                        asignarValorYTipo(temp, variables, 4, objetos);

                        return new List<string>();
                    }
                    else
                    {
                        variables = agregarVariableGlobal(temp[0], variables, objetos);

                        //NUMERO TRES

                        asignarValorYTipo(temp, variables, 3, objetos);

                        return new List<string>();
                    }
                }
                else
                {
                    if (len == 1)
                    {
                        variables.Add(temp[0].getHoja().getValor().getValor() + "");
                        return variables;
                    }
                    else
                    {
                        variables = agregarVariableGlobal(temp[0], variables, objetos);

                        variables.Add(temp[2].getHoja().getValor().getValor() + "");
                        return variables;
                    }
                }
            }
            else
            {
                return new List<string>();
            }
            
        }


        public void asignarValorYTipo(AST.Nodo[] temp, List<string> variables, int n, List<Objeto>objetos)
        {

            string[] vars = variables.ToArray();
            string tipoVar = temp[n-1].getNodos()[0].getHoja().getValor().getValor() + "";
            string valor;
            Terminal.TipoDato tipoDato;
            Objeto o = null;

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
                tipoDato = Terminal.TipoDato.OBJETO;
                valor = tipoVar;

                Objeto[] tempO = objetos.ToArray();
                for (int i = 0; i < tempO.Length; i++)
                {
                    if (tempO[i].getNombre() == tipoVar)
                    {
                        o = tempO[i];
                        break;
                    }
                }
            }

            if (variables.Count == 1)
            {
                if (temp[n].getNodos().Count > 0)
                {
                    Instruccion ins = new Instruccion();

                    if (temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0].getNombre() == "ASIGNACION")
                    {
                        string referencia = ins.getAsignaciones(temp[n], "");

                        string[] ids = referencia.Split(".");

                        string resultado = "";

                        for (int i = 0; i < Form1.variableGlobales.Count; i++)
                        {
                            if (Form1.variableGlobales.ElementAt(i).getNombre() == ids[0])
                            {
                                resultado = ins.asignarAVariable1(ids, 0, Form1.variableGlobales.ElementAt(i), null) + "";

                                if (resultado == "false")
                                {
                                    tipoDato = Semantica.Terminal.TipoDato.BOOLEANO;
                                }
                                else if (resultado == "true")
                                {
                                    tipoDato = Semantica.Terminal.TipoDato.BOOLEANO;
                                }
                                else if (resultado.Contains("'"))
                                {
                                    tipoDato = Semantica.Terminal.TipoDato.CADENA;
                                }
                                else if (resultado.Contains("."))
                                {
                                    tipoDato = Semantica.Terminal.TipoDato.REAL;
                                }
                                else
                                {
                                    try
                                    {
                                        int result = Int32.Parse(tipoVar);
                                        tipoDato = Semantica.Terminal.TipoDato.ENTERO;
                                    }
                                    catch (FormatException)
                                    {
                                        tipoDato = Semantica.Terminal.TipoDato.ID;
                                    }
                                }

                                break;
                            }
                        }

                        valor = resultado + "";
                    }
                    else if (temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0].getNombre() == "EXP")
                    {
                        Expresion exp = new Expresion();
                        valor = exp.noce(temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0]) + "";

                        if (valor.ToString().Contains("."))
                        {
                            tipoDato = Terminal.TipoDato.REAL;
                        }
                        else
                        {
                            tipoDato = Terminal.TipoDato.ENTERO;
                        }

                    }
                    else if (temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0].getNombre() == "EXP_LOG")
                    {
                        ExpresionLogica expL = new ExpresionLogica();
                        valor = expL.noce(temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0]) + "";
                        tipoDato = Terminal.TipoDato.BOOLEANO;
                    }
                    else
                    {
                        valor = temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0].getHoja().getValor().getValor() + "";
                    }

                }
            }


            for (int i = 0; i < vars.Length; i++)
            {
                this.variablesGLobales.Add(new Variable(vars[i], new Terminal(valor, tipoDato, o)));
            }

        }


        public List<Variable> getVariables()
        {
            return this.variablesGLobales;
        }


        public  List<Objeto> getObjetos()
        {
            return this.objetos;
        }

    }
}
