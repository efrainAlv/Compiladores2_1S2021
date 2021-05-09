using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;


namespace Proyecto2.Semantica
{
    public class Cabecera
    {


        private AST.Nodo nodo;
        private List<Variable> variablesGLobales;
        private List<Objeto> objetos;
        private List<Entorno> entorno;

        public Cabecera()
        {
            this.variablesGLobales = new List<Variable>();
        }

        public Cabecera(List<Entorno> entorno)
        {
            this.variablesGLobales = new List<Variable>();
            this.objetos = new List<Objeto>();
            this.entorno = entorno;
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

                        if (temp[0].getNodos()[3].getNodos()[0].getNombre()=="OBJETO")
                        {
                            agregarObjetos(temp[0]);
                        }
                        else
                        {
                            string nombreTipo;
                            nombreTipo = temp[0].getNodos()[1].getHoja().getValor().getValor()+"";

                            int inicio = Int32.Parse(temp[0].getNodos()[3].getNodos()[0].getNodos()[2].getHoja().getValor().getValor() + "");

                            int fin = Int32.Parse(temp[0].getNodos()[3].getNodos()[0].getNodos()[4].getHoja().getValor().getValor()+"");

                            string tipoObj = temp[0].getNodos()[3].getNodos()[0].getNodos()[7].getNodos()[0].getHoja().getValor().getValor() + "";

                            List<Variable> vars = new List<Variable>();

                            if (tipoObj=="integer")
                            {
                                for (int j = inicio; j <= fin; j++)
                                {
                                    vars.Add(new Variable("", new Terminal(0 ,Terminal.TipoDato.ENTERO)));
                                }
                                Objeto o = new Objeto(nombreTipo, new Arreglo(tipoObj, vars));
                                Form1.objetos.Add(o);
                            }
                            else if (tipoObj=="string")
                            {
                                vars.Add(new Variable("", new Terminal(0, Terminal.TipoDato.CADENA)));
                                Objeto o = new Objeto(nombreTipo, new Arreglo(tipoObj, vars));
                                Form1.objetos.Add(o);
                            }
                            else if (tipoObj == "real")
                            {
                                vars.Add(new Variable("", new Terminal(0, Terminal.TipoDato.REAL)));
                                Objeto o = new Objeto(nombreTipo, new Arreglo(tipoObj, vars));
                                Form1.objetos.Add(o);
                            }
                            else if (tipoObj == "boolean")
                            {
                                vars.Add(new Variable("", new Terminal(0, Terminal.TipoDato.BOOLEANO)));
                                Objeto o = new Objeto(nombreTipo, new Arreglo(tipoObj, vars));
                                Form1.objetos.Add(o);
                            }
                            else
                            {
                                for (int i = 0; i < Form1.objetos.Count; i++)
                                {
                                    if (Form1.objetos[i].getNombre() == tipoObj)
                                    {
                                        for (int j = inicio; j <= fin; j++)
                                        {
                                            vars.Add(new Variable("", new Terminal(null, Terminal.TipoDato.OBJETO, Form1.objetos[i])));
                                        }
                                    }
                                }

                                Objeto o = new Objeto(nombreTipo, new Arreglo(tipoObj, vars));
                                Form1.objetos.Add(o);
                            }


                        }

                        
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
                    Form1.objetos.Add(o);
                    return null;
                }
                else
                {

                    if (temp[0].getNombre() == "OBJETO")
                    {
                        Cabecera c = new Cabecera(this.entorno);
                        c.agregarVaraibles(temp[0].getNodos()[1], this.objetos);
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


        public List<Variable> agregarArreglos(AST.Nodo nodoAct)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            string tipo = nodoAct.getNombre();
            int len = temp.Length;

            if (len > 0)
            {
                if (tipo == "DECLARACION_OBJETOS")
                {
                    string nombreTipo;
                    nombreTipo = temp[1].getHoja().getValor().getValor() + "";
                    Objeto o = new Objeto(nombreTipo);
                    Form1.objetos.Add(o);
                    return null;

                }
                else
                {

                    if (temp[0].getNombre() == "OBJETO")
                    {
                        Cabecera c = new Cabecera(this.entorno);
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



        //Agregar variables de forma global --> VARIABLE
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
            int tamanioVariable = 0;

            if (tipoVar.ToLower() == "integer")
            {
                valor = "0";
                tamanioVariable = 1;
                tipoDato = Terminal.TipoDato.ENTERO;
            }
            else if (tipoVar.ToLower() == "real")
            {
                valor = "0.0";
                tamanioVariable = 1;
                tipoDato = Terminal.TipoDato.REAL;
            }
            else if (tipoVar.ToLower() == "boolean")
            {
                tamanioVariable = 1;
                valor = "false";
                tipoDato = Terminal.TipoDato.BOOLEANO;
            }
            else if (tipoVar.ToLower() == "string")
            {
                tamanioVariable = 20;
                valor = "";
                tipoDato = Terminal.TipoDato.CADENA;
            }
            else
            {
                tipoDato = Terminal.TipoDato.OBJETO;
                valor = tipoVar;

                Objeto[] tempO = Form1.objetos.ToArray();
                for (int i = 0; i < tempO.Length; i++)
                {
                    if (tempO[i].getNombre() == tipoVar)
                    {
                        o = tempO[i];
                        tamanioVariable = o.getTamanioObjeto();
                        break;
                    }
                }
            }

            if (variables.Count == 1)
            {
                if (temp[n].getNodos().Count > 0)
                {
                    Instruccion ins = new Instruccion(ref this.entorno);

                    if (temp[n].getNodos()[1].getNodos()[0].getNombre() == "ASIGNACION1")
                    {
                        string resultado = "";

                        resultado = validarAsignacionAVariable(temp[n].getNodos()[1].getNodos()[0], "", ins);

                        if (resultado == "false")
                        {
                            tipoDato = Terminal.TipoDato.BOOLEANO;
                        }
                        else if (resultado == "true")
                        {
                            tipoDato = Terminal.TipoDato.BOOLEANO;
                        }
                        else if (resultado.Contains("'"))
                        {
                            tipoDato = Terminal.TipoDato.CADENA;
                        }
                        else if (resultado.Contains("."))
                        {
                            tipoDato = Terminal.TipoDato.REAL;
                        }
                        else
                        {
                            try
                            {
                                int result = Int32.Parse(tipoVar);
                                tipoDato = Terminal.TipoDato.ENTERO;
                            }
                            catch (FormatException)
                            {
                                tipoDato = Terminal.TipoDato.ID;
                            }
                        }


                        valor = resultado + "";

                        //this.variablesGLobales.Add(new Variable(vars[0], new Terminal(valor, tipoDato, o), this.variablesGLobales.Count, 1));
                    }
                    else if (temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0].getNombre() == "EXP")
                    {
                        Expresion exp = new Expresion(this.entorno);
                        valor = exp.noce(temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0]) + "";

                        CodigoI.Temporal t = exp.generarTemporales(temp[n].getNodos()[1].getNodos()[0]);

                        if (valor.ToString().Contains("."))
                        {
                            tipoDato = Terminal.TipoDato.REAL;
                        }
                        else
                        {
                            tipoDato = Terminal.TipoDato.ENTERO;
                        }


                        if (t != null)
                        {
                            Form1.richTextBox2.Text += "STACK [" + (this.variablesGLobales.Count) + "] = T" + t.indice + "; \n";
                        }
                        else
                        {
                            Form1.richTextBox2.Text += "STACK [" + (this.variablesGLobales.Count) + "] = " + valor + "; \n";
                        }


                    }
                    else if (temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0].getNombre() == "EXP_LOG")
                    {
                        ExpresionLogica expL = new ExpresionLogica(ref this.entorno);
                        valor = expL.noce(temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0]) + "";
                        tipoDato = Terminal.TipoDato.BOOLEANO;

                        expL.traducir(temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0]);
                        expL.imptimirVeraderas();
                        Form1.richTextBox2.Text += "STACK [" + (this.variablesGLobales.Count) + "] =  1; \n";
                        expL.imprimirFalsas();
                        Form1.richTextBox2.Text += "STACK [" + (this.variablesGLobales.Count)+ "] =  0; \n";

                    }
                    else
                    {
                        valor = temp[n].getNodos().ToArray()[1].getNodos().ToArray()[0].getHoja().getValor().getValor() + ""; //cadena

                    }

                }
            }


            if (this.entorno.Count > 0 )
            {
                
                for (int i = 0; i < vars.Length; i++)
                {
                    Variable var = null;
                    Objeto obj = null;
                    if (o!=null)
                    {
                        obj = o.clonar();
                    }
                    var = new Variable(vars[i], new Terminal(valor, tipoDato, obj), tamanioVariable, tamanioVariable);
                    if (this.variablesGLobales.Count > 0)
                    {
                        var.actualizarIndices(this.variablesGLobales[this.variablesGLobales.Count - 1].indiceFinStackHeap);
                    }
                    else
                    {
                        var.actualizarIndices(Form1.finHeap);
                    }
                    this.variablesGLobales.Add(var);
                }
            }
            else
            {

                for (int i = 0; i < vars.Length; i++)
                {
                    Objeto obj = null;
                    if (o!=null)
                    {
                        obj = o.clonar();
                    }
                    if (this.variablesGLobales.Count>0)
                    {
                        this.variablesGLobales.Add(new Variable(vars[i], new Terminal(valor, tipoDato, obj), (this.variablesGLobales[this.variablesGLobales.Count-1].indiceFinStackHeap + tamanioVariable), tamanioVariable));
                    }
                    else
                    {
                        this.variablesGLobales.Add(new Variable(vars[i], new Terminal(valor, tipoDato, obj), tamanioVariable, tamanioVariable));
                    }

                }
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



        //METODO PARA ANALIZAR PRODUCCION ASIGNAICON1
        //RETORNA EL VALOR DE UNA VARIABLE, OBJETO ANINDADO, FUNCION



        public string validarAsignacionAVariable(AST.Nodo nodoAct, string referencia, Instruccion ins)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            string tipo = nodoAct.getNombre();


            if (tipo=="ASIGNACION1")
            {

                referencia = temp[0].getHoja().getValor().getValor()+"";

                referencia = validarAsignacionAVariable(temp[1], referencia, ins); //PRODUCCION ASIGNACION2

                return referencia;

            }
            else
            {
                if (temp.Length == 2)
                {
                    referencia += ".";

                    referencia += ins.getAsignaciones(temp[1], "");


                    string[] ids = referencia.Split(".");
                    string resultado = "";

                    resultado = ins.getResultadoDeVariable(ids);

                    referencia = resultado;

                    return referencia;

                }
                else if (temp.Length == 3)
                {
                    double valorRetorno = 0;

                    Instruccion inst = new Instruccion(ref this.entorno);

                    FuncsProcs.Funcion funcion = Form1.buscarFuncion(referencia, "funcion");

                    if (funcion != null)
                    {
                        if (funcion.getLongParams()>0)
                        {
                            funcion = ins.llamadasFunciones(temp[1], funcion, funcion.getLongParams()-1);   
                        }
                    }


                    if (funcion != null)
                    {
                        //funcion.ejecutar();

                        try
                        {
                            valorRetorno = Double.Parse(funcion.getEntorno()[funcion.getEntorno().Count - 1].buscarVariable(funcion.getNombre()).getValor().getValor() + "");

                            referencia  = valorRetorno + "";
                        }
                        catch (FormatException e)
                        {
                            string valorRetornoAux = funcion.getEntorno()[funcion.getEntorno().Count - 1].buscarVariable(funcion.getNombre()).getValor().getValor() + "";

                            referencia = valorRetornoAux + "";
                        }

                        return referencia;
                    }
                    else
                    {
                        return referencia +"";
                    }

                }
                else
                {

                    referencia = ins.getResultadoDeVariable(referencia.Split("."));
                    return referencia;
                    //epsilon
                }
            }


        }






        public void traducir(AST.Nodo nodoAct)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (temp.Length > 0)
            {
                if (nodoAct.getNombre() == "CABECERA")
                {
                    if (temp.Length == 2)
                    {
                        traducir(temp[0]); //CABECERA

                        traducir(temp[1]); //CABECERA1
                    }
                    else
                    {
                        if (temp[0].getNombre() == "CABECERA1")
                        {
                            traducir(temp[0]); //CABECERA1
                        }
                        else
                        {
                            //episilon
                        }
                    }
                }
                else if (nodoAct.getNombre() == "CABECERA1")
                {
                    if (temp[0].getNombre() == "VARIABLE")
                    {
                        agregarVariableGlobal(temp[0], new List<string>(), this.objetos);

                    }
                    else if (temp[0].getNombre() == "DECLARACION_OBJETOS")
                    {

                        if (temp[0].getNodos()[3].getNodos()[0].getNombre() == "OBJETO")
                        {
                            agregarObjetos(temp[0]);
                        }
                        else
                        {
                            

                        }


                    }

                    else
                    {

                    }
                }
                else
                {
                    //epsilon
                }

            }

        }


        //PRODUCCION VARIABLE
        public void traducirVariable(AST.Nodo nodoAct)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (nodoAct.getNombre()=="VARIABLE")
            {

            }
            else if (nodoAct.getNombre() == "VARIABLE")
            {

            }
            else
            {

            }


        }



    }
}
