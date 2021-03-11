using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using System.Windows.Forms;

namespace Proyecto1.Semantica
{
    public class Instruccion
    {

        private List<Entorno>  entorno;

        public Instruccion(List<Entorno> entorno)
        {
            this.entorno = entorno;
        }


        public void analizar(AST.Nodo nodoAct)
        {

            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (len>0)
            {
                if (tipo == "INSTRUCCIONES")
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
                else
                {
                    if (temp[0].getNombre() == "IF")
                    {
                        SentenciaDeControl.SenteciaIf senIf = new SentenciaDeControl.SenteciaIf(this.entorno);

                        senIf.analizar(temp[0], false);

                    }
                    else if (temp[0].getNombre()=="LLAMADA")
                    {
                        FuncsProcs.Procedimiento proc = llamadasProcedimientos(temp[0], null, 0);
                        proc.ejecutar();

                        if (proc==null)
                        {
                            FuncsProcs.Funcion func = llamadasFunciones(temp[0], null, 0);
                            func.ejecutar();
                        }

                        /*for (int k = 0; k < proc.getLongParams(); k++)
                        {
                            if (proc.getParametro(k).getTipo()=="referencia")
                            {
                                for (int i = this.entorno.Count - 1; i >= 0; i--)
                                {
                                    Variable varEntorno = this.entorno[i].buscarVariable();

                                    if (varEntorno != null)
                                    {
                                        Variable var = asignarAVariable(idss, valor, 0, varEntorno, null);

                                        if (var != null)
                                        {
                                            this.entorno[i].buscarVariable(idss[0]).setValorObjeto(var.getValor().getValorObjeto());
                                        }
                                        else
                                        {
                                            MessageBox.Show("LA VARIABLE " + idss[0] + "NO EXISTE");
                                        }

                                        break;
                                    }
                                }
                            }
                        }*/
                    }
                    else
                    {
                        string cadena = getAsignaciones(temp[0], "");
                        
                    }
                }

            }

        }


        //ANALIZA LA PRODUCCION ASIGNACIONES Y 
        public string getAsignaciones(AST.Nodo nodoAct, string cadena)
        {
            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (tipo == "ASIGNACIONES")
            {
                if (len == 6)
                {
                    cadena = getAsignaciones(temp[0], cadena);
                    cadena += getAsignaciones(temp[1], cadena);

                    cadena += temp[2].getHoja().getValor().getValor();
                    cadena += temp[3].getHoja().getValor().getValor();

                    //numero 4

                    asignarValor(temp, cadena, 4);

                    return "";
                }
                else
                {
                    cadena = getAsignaciones(temp[0], cadena);
                    cadena += temp[1].getHoja().getValor().getValor();
                    cadena += temp[2].getHoja().getValor().getValor();

                    //numero 3

                    asignarValor(temp, cadena, 3);

                    return "";
                }

            }
            else
            {
                if (len == 3)
                {
                    cadena = getAsignaciones(temp[0], cadena);
                    cadena += temp[1].getHoja().getValor().getValor();
                    cadena += temp[2].getHoja().getValor().getValor();

                    return cadena;
                }
                else
                {
                    cadena = temp[0].getHoja().getValor().getValor() + "";

                    return cadena;
                }
            }

        }


        public Semantica.Variable asignarAVariable(string[] ids, string valor, int indice, Semantica.Variable var, Semantica.Objeto obj)
        {
            if (ids.Length == 1)
            {
                if (var.getValor().getValorObjeto()==null)
                {
                    var.getValor().setValor(valor);
                    return var;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (indice == 0)
                {
                    if (var.getNombre() == ids[indice])
                    {
                        var = asignarAVariable(ids, valor, indice+1, var, var.getValor().getValorObjeto());
                        return var;
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (indice == ids.Length - 1)
                {
                    Semantica.Objeto objeto = var.getValor().getValorObjeto();
                    

                    if (objeto!=null)
                    {
                        Semantica.Variable v = objeto.buscarAtributo(ids[indice]);

                        if (v != null)
                        {

                            if (v.getValor().getValorObjeto() == null)
                            {
                                v.getValor().setValor(valor);
                                return var;
                            }
                            else
                            {
                                return null;
                            }

                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    Semantica.Variable v = obj.buscarAtributo(ids[indice]);

                    if (v != null)
                    {
                        Semantica.Variable vari = asignarAVariable(ids, valor, indice + 1, v, v.getValor().getValorObjeto());

                        if (vari!=null)
                        {
                            var.getValor().getValorObjeto().actualizarVariable(ids[indice], vari);
                            return var;
                        }
                        else
                        {
                            return null;
                        }
                        
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }



        //RETORNA EL VALOR DE UNA VARIABLE, POR EJEMPLO RETORNA EL VALOR DE figuraGeometrica.cuadrado.altura
        // O TAMBIEN RETORNA EL VALOR DE altura
        public Object asignarAVariable1(string[] ids, int indice, Semantica.Variable var, Semantica.Objeto obj)
        {
            if (ids.Length == 1)
            {
                if (var.getValor().getValorObjeto()==null)
                {
                    return var.getValor().getValor();
                }
                else
                {
                    return null;
                }

            }
            else
            {
                if (indice == 0)
                {
                    if (var.getNombre() == ids[indice])
                    {
                        object valor = asignarAVariable1(ids, indice + 1, var, var.getValor().getValorObjeto());
                        return valor;
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (indice == ids.Length - 1)
                {
                    Semantica.Objeto objeto = var.getValor().getValorObjeto();


                    if (objeto != null)
                    {
                        Semantica.Variable v = objeto.buscarAtributo(ids[indice]);

                        if (v != null)
                        {

                            if (v.getValor().getValorObjeto() == null)
                            {
                                return v.getValor().getValor();
                            }
                            else
                            {
                                return null;
                            }

                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    Semantica.Variable v = obj.buscarAtributo(ids[indice]);

                    if (v != null)
                    {
                        object vari = asignarAVariable1(ids, indice + 1, v, v.getValor().getValorObjeto());

                        if (vari != null)
                        {
                            return vari;
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }



        //RETORNA EL VALOR QUE SE LE ESTA ASIGNANDO A UNA VARIABLE, EL VALOR PUEDE SER UNA EXPRESION,
        //EXPRESION LOGICA, O UNA ASIGNACION
        public void asignarValor(AST.Nodo[] temp, string cadena, int n)
        {

            if (temp[n].getNodos()[0].getNombre() == "ASIGNACION1")
            {

                /*for (int i = 0; i < Form1.variableGlobales.Count; i++)
                {
                    if (Form1.variableGlobales.ElementAt(i).getNombre() == ids[0])
                    {
                        resultado = asignarAVariable1(ids, 0, Form1.variableGlobales.ElementAt(i), null);

                        break;
                    }
                }*/

                cadena += validarAsignacionAVariable(temp[n].getNodos()[0], "");
                //RETORNA UN RESULTADO
            }
            else if (temp[n].getNodos()[0].getNombre() == "EXP")
            {
                Expresion exp = new Expresion(this.entorno);
                cadena += exp.noce(temp[n].getNodos()[0]);
            }
            else if (temp[n].getNodos()[0].getNombre() == "EXP_LOG")
            {
                ExpresionLogica expL = new ExpresionLogica(this.entorno);
                cadena += expL.noce(temp[n].getNodos()[0]);
            }
            else
            {
                cadena += temp[n].getNodos().ToArray()[0].getHoja().getValor().getValor();
            }

            //cadena += temp[5].getHoja().getValor().getValor();


            string[] idss = cadena.Split(":=")[0].Split('.');
            string valor = cadena.Split(":=")[1];

            buscarVariableEnEntornos(idss, valor);

            /*
            for (int i = 0; i < Form1.variableGlobales.Count; i++)
            {
                if (Form1.variableGlobales.ElementAt(i).getNombre() == idss[0])
                {
                    Variable var = asignarAVariable(idss, valor, 0, Form1.variableGlobales.ElementAt(i), null);
                    //object nose = asignarAVariable1(ids, 0, Form1.variableGlobales.ElementAt(i), null);
                    //MessageBox.Show("Valor " + nose);

                    if (var != null)
                    {
                        Form1.variableGlobales.ElementAt(i).setValorObjeto(var.getValor().getValorObjeto());
                        //MessageBox.Show("LA VARIABLE SI EXISTE");
                    }
                    else
                    {
                        //MessageBox.Show("LA VARIABLE NO EXISTE");
                    }

                    break;
                }
            }*/



        }



        //METODO PARA ANALIZAR PRODUCCION ASIGNAICON1
        //RETORNA EL VALOR DE UNA VARIABLE, OBJETO ANINDADO, FUNCION
        public string validarAsignacionAVariable(AST.Nodo nodoAct, string referencia)
        {

            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            string tipo = nodoAct.getNombre();


            if (tipo == "ASIGNACION1")
            {

                referencia = temp[0].getHoja().getValor().getValor() + "";

                referencia = validarAsignacionAVariable(temp[1], referencia); //PRODUCCION ASIGNACION2

                return referencia;

            }
            else
            {
                if (temp.Length == 2)
                {
                    referencia += ".";

                    referencia += getAsignaciones(temp[1], "");


                    string[] ids = referencia.Split(".");

                    referencia = getResultadoDeVariable(ids);

                    return referencia;

                }
                else if (temp.Length == 3)
                {
                    double valorRetorno = 0;

                    FuncsProcs.Funcion funcion = Form1.buscarFuncion(referencia);

                    if (funcion!=null)
                    {
                        if (funcion.getLongParams()>0)
                        {
                            funcion = llamadasFunciones(temp[1], funcion, funcion.getLongParams()-1);
                        }
                    }

                    if (funcion != null)
                    {
                        funcion.ejecutar();

                        try
                        {
                            valorRetorno = Double.Parse(funcion.getEntorno()[funcion.getEntorno().Count - 1].buscarVariable(funcion.getNombre()).getValor().getValor() + "");

                            referencia = valorRetorno + "";
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
                        return referencia + "";
                    }

                }
                else
                {
                    
                    referencia = getResultadoDeVariable(referencia.Split("."));

                    return referencia;

                    //epsilon
                }
            }


        }






        //BUSCA UNA VARIABLE EN LOS ENTORNOS Y LE ASIGNA UN VALOR A LA VARIABLE ENCONTRADA
        public void buscarVariableEnEntornos(string[] idss, string valor)
        {
            for (int i = this.entorno.Count-1; i >= 0; i--)
            {
                Variable varEntorno = this.entorno[i].buscarVariable(idss[0]);

                if (varEntorno!=null)
                {
                    Variable var = asignarAVariable(idss, valor, 0, varEntorno, null);

                    if (var!=null)
                    {
                        this.entorno[i].buscarVariable(idss[0]).setValorObjeto(var.getValor().getValorObjeto());
                    }
                    else
                    {
                        MessageBox.Show("LA VARIABLE "+ idss[0] +"NO EXISTE");
                    }

                    break;
                }
            }
        }


        public string getResultadoDeVariable(string[]ids)
        {

            for (int i = this.entorno.Count - 1; i >= 0; i--)
            {
                Variable varEntorno = this.entorno[i].buscarVariable(ids[0]);

                if (varEntorno != null)
                {
                    object resultado = asignarAVariable1(ids, 0, this.entorno[i].buscarVariable(ids[0]), null);
                    
                    return resultado+"";

                }
            }

            return "";
        }


        public FuncsProcs.Procedimiento llamadasProcedimientos(AST.Nodo nodoAct, FuncsProcs.Procedimiento proc, int indice)
        {
            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (tipo=="LLAMADA")
            {
                proc = Form1.buscarProcedimiento(temp[0].getHoja().getValor().getValor() + "");

                if (proc==null)
                {
                    return proc;
                }
                else
                {
                    indice = proc.getLongParams() - 1;

                    if (indice<0)
                    {
                        indice = 0;
                        return proc;
                    }
                    else
                    {
                        llamadasProcedimientos(temp[2], proc, indice);
                        return proc;
                    }
                }
            }
            else
            {

                List<string> listaParams = llamadaProcedimientos2(nodoAct, new List<string>(), indice, proc);

                for (int i = 0; i < proc.getLongParams(); i++)
                {
                    proc.getParametro(i).getVariable().getValor().setValor(listaParams[i]);
                }

                return proc;

            }

        }


        public List<string> llamadaProcedimientos2(AST.Nodo nodoAct, List<string> valores, int indice, FuncsProcs.Procedimiento proc)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (temp.Length == 3)
            {
                indice--;
                valores = llamadaProcedimientos2(temp[0], valores, indice, proc);
                indice++;

                if (proc != null)
                {
                    valores.Add(valorDeLlamadas(temp, 2, indice, proc));
                    return valores;
                }
                else
                {
                    return valores;
                }
            }
            else
            {
                valores.Add(valorDeLlamadas(temp, 0, indice, proc));
                return valores;
            }
        }

        public FuncsProcs.Funcion llamadasFunciones(AST.Nodo nodoAct, FuncsProcs.Funcion func, int indice)
        {
            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (tipo == "LLAMADA")
            {
                func = Form1.buscarFuncion(temp[0].getHoja().getValor().getValor() + "");

                //AGREGAR ALGO PARA VER FUNCIONES O PROCEDIMIENTOS ANIDADOS


                if (func == null)
                {
                    return func;
                }
                else
                {
                    indice = func.getLongParams() - 1;

                    if (indice<0)
                    {
                        indice = 0;
                        return func;
                    }
                    else
                    {
                        llamadasFunciones(temp[2], func, indice); //AGREGAR AQUI
                        return func;
                    }

                }
            }
            else
            {

                List<string> listaParams = llamadasFunciones2(nodoAct, new List<string>(), indice, func);

                for (int i = 0; i < func.getLongParams(); i++)
                {
                    func.getParametro(i).getVariable().getValor().setValor(listaParams[i]);
                }

                return func;

            }

        }

        //  SE ENCARGA DE LA PRODUCCION 'LLAMADA1'
        public List<string> llamadasFunciones2(AST.Nodo nodoAct, List<string> valores, int indice, FuncsProcs.Funcion func)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();

            if (temp.Length == 3)
            {
                indice--;
                valores = llamadasFunciones2(temp[0], valores, indice, func);
                indice++;

                if (func != null)
                {
                    valores.Add(valorDeLlamadasFuncion(temp, 2, indice, func));
                    return valores;
                }
                else
                {
                    return valores;
                }
            }
            else
            {
                valores.Add(valorDeLlamadasFuncion(temp, 0, indice, func));
                return valores;
            }
        }

        public string valorDeLlamadas(AST.Nodo[] temp, int n, int indice, FuncsProcs.Procedimiento proc)
        {
            if (temp[n].getNombre() == "VALOR")
            {
                if (temp[n].getNodos()[0].getNombre() == "ASIGNACION1") //AGREGAR POR REFERENCIA
                {
                    if (proc.getParametro(indice).getTipo() == "referencia")
                    {
                        string resultado = validarAsignacionAVariable(temp[n].getNodos()[0], "");

                        //proc.getParametro(indice).getVariable().getValor().setValor(resultado);

                        return resultado;
                    }
                    else
                    {
                        return "";
                    }
                }
                else if (temp[n].getNodos()[0].getNombre() == "EXP")
                {
                    if (proc.getParametro(indice).getVariable().getValor().getTipo() == Terminal.TipoDato.ENTERO
                        || proc.getParametro(indice).getVariable().getValor().getTipo() == Terminal.TipoDato.REAL)
                    {

                        Expresion exp = new Expresion(this.entorno);

                        double resp = exp.noce(temp[n].getNodos()[0]);
                        //proc.getParametro(indice).getVariable().getValor().setValor(resp);

                        return resp+"";
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (temp[n].getNodos()[0].getNombre() == "EXP_LOG")
                {
                    if (proc.getParametro(indice).getVariable().getValor().getTipo() == Terminal.TipoDato.BOOLEANO)
                    {
                        ExpresionLogica expl = new ExpresionLogica(this.entorno);

                        bool resp = expl.noce(temp[n].getNodos()[0]);
                        //proc.getParametro(indice).getVariable().getValor().setValor(resp);

                        return resp+"";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    if (proc.getParametro(indice).getVariable().getValor().getTipo() == Terminal.TipoDato.CADENA)
                    {

                        //proc.getParametro(indice).getVariable().getValor().setValor();

                        return temp[n].getNodos()[0].getHoja().getValor().getValor() + "";
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            else
            {
                return "";
            }
        }

        public string valorDeLlamadasFuncion(AST.Nodo[] temp, int n, int indice, FuncsProcs.Funcion func)
        {
            if (temp[n].getNombre() == "VALOR")
            {
                if (temp[n].getNodos()[0].getNombre() == "ASIGNACION1") //AGREGAR POR REFERENCIA
                {
                    //PROBLEMAS CON PARAMETROS POR REFERENCIA
                    if (func.getParametro(indice).getTipo() =="referencia"  || func.getParametro(indice).getTipo() == "valor")
                    {

                        string resultado = validarAsignacionAVariable(temp[n].getNodos()[0], "");

                        //func.getParametro(indice).getVariable().getValor().setValor(resultado);

                        return resultado;
                    }
                    else
                    {
                        return "";
                    }
                }
                else if (temp[n].getNodos()[0].getNombre() == "LLAMADA")
                {
                    double valorRetorno = 0;

                    Instruccion inst = new Instruccion(this.entorno);

                    FuncsProcs.Funcion funcion = inst.llamadasFunciones(temp[1], null, 0);

                    if (func != null)
                    {
                        func.ejecutar();

                        try
                        {
                            valorRetorno = Double.Parse(funcion.getEntorno()[funcion.getEntorno().Count - 1].buscarVariable(funcion.getNombre()).getValor().getValor() + "");

                            //func.getParametro(indice).getVariable().getValor().setValor(valorRetorno);

                            return valorRetorno+"";
                        }
                        catch (FormatException e)
                        {
                            string valorRetornoAux = funcion.getEntorno()[funcion.getEntorno().Count - 1].buscarVariable(funcion.getNombre()).getValor().getValor() + "";
                            //func.getParametro(indice).getVariable().getValor().setValor(valorRetornoAux);
                            return valorRetornoAux ;
                        }
                    }
                    else
                    {
                        return "";
                    }


                }
                else if (temp[n].getNodos()[0].getNombre() == "EXP")
                {
                    if (func.getParametro(indice).getVariable().getValor().getTipo() == Terminal.TipoDato.ENTERO
                        || func.getParametro(indice).getVariable().getValor().getTipo() == Terminal.TipoDato.REAL)
                    {

                        Expresion exp = new Expresion(this.entorno);

                        double resp = exp.noce(temp[n].getNodos()[0]);
                        //func.getParametro(indice).getVariable().getValor().setValor(resp);

                        return resp+"";
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (temp[n].getNodos()[0].getNombre() == "EXP_LOG")
                {
                    if (func.getParametro(indice).getVariable().getValor().getTipo() == Terminal.TipoDato.BOOLEANO)
                    {
                        ExpresionLogica expl = new ExpresionLogica(this.entorno);

                        bool resp = expl.noce(temp[n].getNodos()[0]);
                        //func.getParametro(indice).getVariable().getValor().setValor(resp);

                        return resp+"";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    if (func.getParametro(indice).getVariable().getValor().getTipo() == Terminal.TipoDato.CADENA)
                    {

                        //func.getParametro(indice).getVariable().getValor().setValor(temp[n].getNodos()[0].getHoja().getValor().getValor() + "");

                        return temp[n].getNodos()[0].getHoja().getValor().getValor() + "";
                    }
                    else
                    {
                        return temp[n].getNodos()[0].getHoja().getValor().getValor() + "";
                    }
                }
            }
            else
            {
                return "";
            }
        }

    }
}
