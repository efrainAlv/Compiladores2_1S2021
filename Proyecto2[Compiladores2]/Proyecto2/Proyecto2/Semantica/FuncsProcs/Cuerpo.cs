﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Proyecto2.Semantica.FuncsProcs
{
    public class Cuerpo
    {

        private List<Procedimiento> procedimientos;
        private List<Funcion> funciones;
        private List<Entorno> entorno;

        public Cuerpo(List<Entorno> entorno)
        {
            this.procedimientos = new List<Procedimiento>();
            this.funciones = new List<Funcion>();
            this.entorno = entorno;
        }

        public List<Procedimiento> getProcedimientos()
        {
            return this.procedimientos;
        }

        public List<Funcion> getFunciones()
        {
            return this.funciones;
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
                        agregarFuncion(temp[0], new List<Parametro>());
                        Form1.addFunciones(this.funciones);
                        this.funciones = null;
                        this.funciones = new List<Funcion>();
                    }
                    else
                    {
                        agregarProcedimiento(temp[0], new List<Parametro>());
                        Form1.addProcedimientos(this.procedimientos);
                        this.procedimientos = null;
                        this.procedimientos = new List<Procedimiento>();
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

                    if (parametros.Count>0)
                    {
                        for (int i = 0; i < parametros.Count; i++)
                        {
                            parametros[i].getVariable().indiceStack = Form1.finStack;
                            if (parametros[i].getVariable().getValor().getTipo() == Terminal.TipoDato.CADENA)
                            {
                                parametros[i].getVariable().indiceFinStackHeap = Form1.finHeap + parametros[i].getVariable().tamanio;
                                Form1.finHeap += parametros[i].getVariable().tamanio;
                            }
                            else if (parametros[i].getVariable().getValor().getValorObjeto() != null )
                            {
                                parametros[i].getVariable().actualizarIndices(Form1.finHeap);
                                Form1.finHeap += parametros[i].getVariable().tamanio;
                            }
                            else
                            {
                                parametros[i].getVariable().indiceFinStackHeap = -1;
                            }
                            Form1.finStack++;
                        }
                        
                    }

                    
                    List<Entorno> ent = new List<Entorno>();
                    for (int i = 0; i < this.entorno.Count; i++)
                    {
                        ent.Add(this.entorno[i]);
                    }

                    Cabecera c = new Cabecera(ent);
                    c.agregarVariableGlobal(temp[4], new List<string>(), Form1.objetos);


                    Procedimiento proc = new Procedimiento(nombre, c.getVariables(), parametros, temp[7], ent);

                    Cuerpo cuer = new Cuerpo(proc.getEntorno());
                    cuer.anidarFuncionProcedimiento(temp[5]);
                    proc.addProcedimientos(cuer.getProcedimientos());
                    proc.addFunciones(cuer.getFunciones());


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


        public List<Parametro> agregarFuncion(AST.Nodo nodoAct, List<Parametro> parametros)
        {
            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;


            if (len > 0)
            {
                if (tipo == "FUNCION")
                {
                    string nombre = temp[1].getHoja().getValor().getValor() + "";

                    parametros = agregarFuncion(temp[2], parametros);

                    if (parametros.Count > 0)
                    {
                        for (int i = 0; i < parametros.Count; i++)
                        {
                            parametros[i].getVariable().indiceStack = Form1.finStack;
                            if (parametros[i].getVariable().getValor().getTipo() == Terminal.TipoDato.CADENA)
                            {
                                parametros[i].getVariable().indiceFinStackHeap = Form1.finHeap + parametros[i].getVariable().tamanio;
                                Form1.finHeap += parametros[i].getVariable().tamanio;
                            }
                            else if (parametros[i].getVariable().getValor().getValorObjeto() != null)
                            {
                                parametros[i].getVariable().actualizarIndices(Form1.finHeap);
                                Form1.finHeap += parametros[i].getVariable().tamanio;
                            }
                            else
                            {
                                parametros[i].getVariable().indiceFinStackHeap = -1;
                            }
                            Form1.finStack++;
                        }

                    }

                    List<Entorno> ent = new List<Entorno>();
                    for (int i = 0; i < this.entorno.Count; i++)
                    {
                        ent.Add(this.entorno[i]);
                    }

                    Cabecera c = new Cabecera(ent);
                    c.agregarVariableGlobal(temp[6], new List<string>(), Form1.objetos);


                    Funcion func = new Funcion(nombre, new Terminal(temp[4].getNodos()[0].getHoja().getValor().getValor(), temp[4].getNodos()[0].getHoja().getValor().getTipo()), c.getVariables(), parametros, temp[9], ent);
                    

                    Cuerpo cuer = new Cuerpo(func.getEntorno());
                    cuer.anidarFuncionProcedimiento(temp[7]);
                    func.addProcedimientos(cuer.getProcedimientos());
                    func.addFunciones(cuer.getFunciones());
                    
                    this.funciones.Add(func);

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
                List<string> ids = getIds(temp[0], new List<string>());

                string tipoTerminal = temp[2].getNodos().ToArray()[0].getHoja().getValor().getValor() + "";
                Terminal.TipoDato tipoDato;

                string[] nombres = ids.ToArray();
                Objeto o = null;
                object valor;

                int indiceFinHeapStack = 0;
                int tamanio = 0;

                if (tipoTerminal.ToLower() == "string")
                {
                    indiceFinHeapStack = 20;
                    tamanio = 20;
                    tipoDato = Terminal.TipoDato.CADENA;
                    valor = "";
                }
                else if (tipoTerminal.ToLower() == "integer")
                {
                    indiceFinHeapStack = 1;
                    tamanio = 1;
                    tipoDato = Terminal.TipoDato.ENTERO;
                    valor = 0;
                }
                else if (tipoTerminal.ToLower() == "real")
                {
                    indiceFinHeapStack = 1;
                    tamanio = 1;
                    tipoDato = Terminal.TipoDato.REAL;
                    valor = 0;
                }
                else if (tipoTerminal.ToLower() == "boolean")
                {
                    indiceFinHeapStack = 1;
                    tamanio = 1;
                    tipoDato = Terminal.TipoDato.BOOLEANO;
                    valor = false;
                }
                else
                {
                    tipoDato = Terminal.TipoDato.OBJETO;
                    o = Form1.buscarObjeto(tipoTerminal).clonar();
                    valor = tipoDato;
                    indiceFinHeapStack = o.getTamanioObjeto();
                    tamanio = o.getTamanioObjeto();
                }

                Objeto obj = null;
                for (int i = 0; i < nombres.Length; i++)
                {
                    if (o!=null)
                    {
                        obj = o.clonar();
                    }
                    vars.Add(new Variable(nombres[i], new Terminal(valor, tipoDato, obj), indiceFinHeapStack, tamanio));
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


        public void anidarFuncionProcedimiento(AST.Nodo nodoAct)
        {
            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (len>0)
            {
                if (tipo == "ANIDAR")
                {
                    if (len==2)
                    {
                        anidarFuncionProcedimiento(temp[0]);
                        anidarFuncionProcedimiento(temp[1]);
                    }
                    else
                    {
                        anidarFuncionProcedimiento(temp[0]);
                    }
                }
                else
                {
                    if (temp[0].getNombre()=="FUNCION")
                    {
                        agregarFuncion(temp[0], new List<Parametro>());
                    }
                    else if (temp[0].getNombre() == "PROCEDIMIENTO")
                    {
                        agregarProcedimiento(temp[0], new List<Parametro>());
                    }
                    else
                    {

                    }
                }
            }
        }

    }
}
