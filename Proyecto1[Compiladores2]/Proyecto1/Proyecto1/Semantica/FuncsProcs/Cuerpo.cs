using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Proyecto1.Semantica.FuncsProcs
{
    public class Cuerpo
    {

        private List<Procedimiento> procedimientos;


        public Cuerpo()
        {
            this.procedimientos = new List<Procedimiento>();
        }

        public List<Procedimiento> getProcedimientos()
        {
            return this.procedimientos;
        }


        public void analizar(AST.Nodo nodoAct)
        {
            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (len>0)
            {
                if (tipo == "CUERPO")
                {
                    if (len == 2)
                    {
                        analizar(temp[0]);

                        analizar(temp[1]);
                    }
                    else
                    {
                        if (temp[0].getNombre() == "CUERPO1")
                        {
                            analizar(temp[0]);
                        }
                        else
                        {
                            //epsilon
                        }
                    }
                }
                else
                {
                    if (temp[0].getNombre() == "FUNCION")
                    {

                    }
                    else
                    {
                        agregarProcedimiento(temp[0], new List<Parametro>());
                    }
                }

            }

        }



        
        public List<Parametro> agregarProcedimiento(AST.Nodo nodoAct, List<Parametro> parametros)
        {
            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;


            if (len>0)
            {
                if (tipo == "PROCEDIMIENTO")
                {
                    string nombre = temp[1].getHoja().getValor().getValor() + "";

                    parametros = agregarProcedimiento(temp[2], parametros);

                    Cabecera c = new Cabecera();
                    c.agregarVariableGlobal(temp[4], new List<string>(), Form1.objetos);

                    Procedimiento proc = new Procedimiento(nombre, c.getVariables(), parametros, temp[6]);

                    this.procedimientos.Add(proc);

                    return null;

                }
                else if (tipo == "PARAMETROS")
                {
                    if (len == 3)
                    {
                        return agregarProcedimiento(temp[1], parametros);
                    }
                    else
                    {
                        return new List<Parametro>();
                    }
                }
                else if (tipo == "PARAMETROS1")
                {
                    if (len == 3)
                    {
                        parametros = agregarProcedimiento(temp[0], parametros);

                        return agregarProcedimiento(temp[2], parametros);
                    }
                    else
                    {
                        return agregarProcedimiento(temp[0], parametros);
                    }
                }
                else
                {
                    if (len == 1)
                    {
                        List<Variable> vars = agregarParametro(temp[0], new List<Variable>());

                        for (int i = 0; i < vars.Count; i++)
                        {
                            parametros.Add(new Parametro(vars.ElementAt(i), "valor"));
                        }
                        return parametros;
                    }
                    else
                    {
                        List<Variable> vars = agregarParametro(temp[1], new List<Variable>());

                        for (int i = 0; i < vars.Count; i++)
                        {
                            parametros.Add(new Parametro(vars.ElementAt(i), "referencia"));
                        }

                        return parametros;
                    }
                }

            }
            else
            {
                return new List<Parametro>();
            }



        }
        
        public List<Variable> agregarParametro(AST.Nodo nodoAct, List<Variable> vars)
        {
            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (tipo == "LISTA_DEC")
            {
                if (len == 2)
                {
                    vars = agregarParametro(temp[0], vars);

                    return agregarParametro(temp[1], vars);
                }
                else
                {
                    return agregarParametro(temp[0], vars);
                }

            }
            else
            {
                List<string>ids = getIds(temp[0], new List<string>());

                string tipoTerminal = temp[2].getNodos().ToArray()[0].getHoja().getValor().getValor() + "";
                Terminal.TipoDato tipoDato;

                string[] nombres = ids.ToArray();
                Objeto o = null;
                object valor;

                if (tipoTerminal == "string")
                {
                    tipoDato = Terminal.TipoDato.CADENA;
                    valor = "";
                }
                else if (tipoTerminal == "integer")
                {
                    tipoDato = Terminal.TipoDato.ENTERO;
                    valor = 0;
                }
                else if (tipoTerminal == "real")
                {
                    tipoDato = Terminal.TipoDato.REAL;
                    valor = 0;
                }
                else if (tipoTerminal == "boolean")
                {
                    tipoDato = Terminal.TipoDato.BOOLEANO;
                    valor = false;
                }
                else
                {
                    tipoDato = Terminal.TipoDato.OBJETO;
                    o = Form1.buscarObjeto(tipoTerminal);
                    valor = tipoDato;

                }

                for (int i = 0; i < nombres.Length; i++)
                {
                    vars.Add(new Variable(nombres[i], new Terminal(valor, tipoDato, o)));
                }

                return vars;

            }
            



        }

        public List<string> getIds(AST.Nodo nodoAct, List<string> ids)
        {
            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (len == 3)
            {
                ids = getIds(temp[0], ids);
                ids.Add(temp[2].getHoja().getValor().getValor() + "");

                return ids;
            }
            else
            {
                ids.Add(temp[0].getHoja().getValor().getValor() + "");
                return ids;
            }
            
        }

    }
}
