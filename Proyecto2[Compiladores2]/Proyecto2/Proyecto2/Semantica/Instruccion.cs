using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using System.Windows.Forms;
using System.IO;

namespace Proyecto2.Semantica
{
    public class Instruccion
    {

        private List<Entorno>  entorno;

        private List<string> valoresParametros = new List<string>();
        private List<string> valoresFunciones = new List<string>();

        public Instruccion(ref List<Entorno> entorno)
        {
            this.entorno = entorno;
        }

        public Instruccion(ref List<Entorno> entorno, List<string> procs, List<string> funcs)
        {
            this.entorno = entorno;
            this.valoresParametros = procs;
            this.valoresFunciones = funcs;
        }


        public List<Entorno> getEntorno()
        {
            return this.entorno;
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

                    if (!Form1.continueIns)
                    {
                        if (temp[0].getNombre() == "IF")
                        {
                            SentenciaDeControl.SenteciaIf senIf = new SentenciaDeControl.SenteciaIf(this.entorno, this.valoresParametros, this.valoresFunciones);

                            senIf.analizar(temp[0], false);

                        }
                        else if (temp[0].getNombre() == "WHILE")
                        {
                            Form1.indiceCiclos.Add(true);


                            SentenciaDeControl.sentenciaWhile senWhile = new SentenciaDeControl.sentenciaWhile(this.entorno, this.valoresParametros, this.valoresFunciones);

                            senWhile.analizar(temp[0]);


                            Form1.indiceCiclos.RemoveAt(Form1.indiceCiclos.Count - 1);
                        }
                        else if (temp[0].getNombre() == "FOR")
                        {
                            Form1.indiceCiclos.Add(true);

                            FuncsProcs.cicloFor cicloFor = new FuncsProcs.cicloFor(this.entorno, this.valoresParametros, this.valoresFunciones);

                            cicloFor.analizar(temp[0]);

                            cicloFor = null;

                            Form1.indiceCiclos.RemoveAt(Form1.indiceCiclos.Count - 1);

                        }
                        else if (temp[0].getNombre() == "CASE")
                        {
                            Form1.indiceCiclos.Add(true);


                            SentenciaDeControl.SentenciaCase sentenciaCase = new SentenciaDeControl.SentenciaCase(this.entorno, this.valoresParametros, this.valoresFunciones);

                            sentenciaCase.analizar(temp[0], "");

                            sentenciaCase = null;

                            Form1.indiceCiclos.RemoveAt(Form1.indiceCiclos.Count - 1);

                        }
                        else if (temp[0].getNombre() == "REPEAT")
                        {
                            Form1.indiceCiclos.Add(true);

                            SentenciaDeControl.Repeat repeat = new SentenciaDeControl.Repeat(this.entorno, this.valoresParametros, this.valoresFunciones);

                            repeat.analizar(temp[0]);

                            repeat = null;

                            Form1.indiceCiclos.RemoveAt(Form1.indiceCiclos.Count - 1);

                        }
                        else if (temp[0].getNombre() == "WRITE" || temp[0].getNombre() == "WRITELN")
                        {
                            Write write = new Write(this.entorno, this.valoresParametros, this.valoresFunciones);

                            write.analizar(temp[0], "");

                            write = null;

                        }
                        else if (temp[0].getNombre() == "DEC_ARREGLO")
                        {

                            string referencia = getAsignaciones(temp[0].getNodos()[0], "");

                            string[] idss = referencia.Split(".");

                            for (int i = this.entorno.Count - 1; i >= 0; i--)
                            {
                                Variable varEntorno = this.entorno[i].buscarVariable(idss[0]);

                                if (varEntorno != null)
                                {
                                    if (varEntorno.getValor().getTipo()==Terminal.TipoDato.OBJETO)
                                    {

                                        if (varEntorno.getValor().getArreglo()!=null)
                                        {
                                            Expresion e = new Expresion(this.entorno);
                                            double val = e.noce(temp[0].getNodos()[2]);

                                            if (varEntorno.getValor().getArreglo().getDatos().Length < val)
                                            {
                                                varEntorno.getValor().getArreglo().getDatos()[Int32.Parse(varEntorno.getValor().getArreglo()+"")].setValorTerminal(valor(temp[0].getNodos()[6].getNodos().ToArray(), 0));
                                            }
                                        }
                                        
                                        //Variable var = asignarAVariable(idss, valor(), 0, varEntorno, null);
                                    }

                                    break;
                                }
                            }

                        }
                        else if (temp[0].getNombre() == "break")
                        {
                            if (Form1.indiceCiclos.Count>0)
                            {
                                Form1.indiceCiclos[Form1.indiceCiclos.Count - 1] = false;
                                Form1.continueIns = true;
                            }
                            
                        }
                        else if (temp[0].getNombre() == "continue")
                        {
                            Form1.continueIns = true;
                        }
                        else if (temp[0].getNombre() == "graficar_ts")
                        {
                            graficarTabla(this.entorno);
                        }
                        else if (temp[0].getNombre() == "EXIT")
                        {
                            
                            try
                            {
                                if (Form1.indiceFunciones[Form1.indiceFunciones.Count - 1])
                                {
                                    int l = this.entorno[this.entorno.Count - 1].getVariables().Count;
                                    this.entorno[this.entorno.Count - 1].getVariables()[l - 1].setValorTerminal(valor(temp[0].getNodos()[2].getNodos().ToArray(), 0));

                                    Form1.continueIns = true;
                                }
                           }
                           
                            catch (IndexOutOfRangeException e)
                            {

                            }
                            
                        }
                        else if (temp[0].getNombre() == "LLAMADA")
                        {

                            FuncsProcs.Procedimiento proc = null;

                            if (Form1.procemientoAnalizado.Count>0)
                            {
                                proc = llamadasProcedimientos(temp[0], Form1.procemientoAnalizado[Form1.procemientoAnalizado.Count-1], 0);
                            }
                            else
                            {
                                proc = llamadasProcedimientos(temp[0], null, 0);
                            }

 
                            if(proc==null)
                            {
                                FuncsProcs.Funcion func = null;

                                if (Form1.funcionAnalizada.Count > 0)
                                {
                                    func = llamadasFunciones(temp[0], Form1.funcionAnalizada[Form1.funcionAnalizada.Count - 1], 0);
                                }
                                else
                                {
                                    func = llamadasFunciones(temp[0], null, 0);
                                }

                                if (func!=null)
                                {
                                    func.ejecutar();
                                    restablecerValoresFunc(func);

                                    
                                    if (func.getLongParams() > 0)
                                    {
                                        List<string> ids = valoresPorRef(temp[0].getNodos()[2], new List<string>());
                                        List<string> vals = new List<string>();

                                        for (int i = 0; i < func.getLongParams(); i++)
                                        {
                                            if (func.getParametro(i).getTipo() == "referencia")
                                            {
                                                vals.Add(func.getParametro(i).getVariable().getValor().getValor() + "");
                                            }
                                        }


                                        for (int k = 0; k < ids.Count && k < vals.Count; k++)
                                        {
                                            for (int i = this.entorno.Count - 1; i >= 0; i--)
                                            {
                                                for (int j = 0; j < this.entorno[i].getVariables().Count; j++)
                                                {
                                                    if (this.entorno[i].getVariables()[j].getNombre() == ids[k])
                                                    {
                                                        this.entorno[i].getVariables()[j].setValorTerminal(vals[k]);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    

                                }

                                func = null;
                            }
                            else
                            {
                                Form1.procemientoAnalizado.Add(proc);

                                proc.ejecutar();
                                restablecerValoresProc(proc);


                                if (proc.getLongParams() > 0)
                                {
                                    List<string> ids = valoresPorRef(temp[0].getNodos()[2], new List<string>());
                                    List<string> vals = new List<string>();


                                    for (int i = 0; i < proc.getLongParams(); i++)
                                    {
                                        if (proc.getParametro(i).getTipo() == "referencia")
                                        {
                                            vals.Add(proc.getParametro(i).getVariable().getValor().getValor() + "");
                                        }
                                    }


                                    for (int k = 0; k < ids.Count && k < vals.Count; k++)
                                    {
                                        for (int i = this.entorno.Count - 1; i >= 0; i--)
                                        {
                                            for (int j = 0; j < this.entorno[i].getVariables().Count; j++)
                                            {
                                                if (this.entorno[i].getVariables()[j].getNombre() == ids[k])
                                                {
                                                    this.entorno[i].getVariables()[j].setValorTerminal(vals[k]);
                                                }
                                            }
                                        }
                                    }

                                }

                                Form1.procemientoAnalizado.RemoveAt(Form1.procemientoAnalizado.Count - 1);


                            }

                            proc = null;

                        }
                        else
                        {
                            string cadena = getAsignaciones(temp[0], "");

                        }

                    }

                }

            }

            temp = null;
        }


        public void restablecerValoresProc(FuncsProcs.Procedimiento proc)
        {
            if (valoresParametros.Count>0 && proc.getLongParams()>0)
            {
                for (int i = 0; i < proc.getParametros().Count; i++)
                {
                    if (proc.getParametros()[i].getTipo() == "valor")
                    {
                        proc.getParametros()[i].getVariable().setValorTerminal(this.valoresParametros[i]);
                    }
                }


                for (int i = 0; i < this.entorno[this.entorno.Count - 1].getVariables().Count; i++)
                {
                    if (proc.getParametros()[i].getTipo() == "valor")
                    {
                        this.entorno[this.entorno.Count - 1].getVariables()[i].setValorTerminal(this.valoresParametros[i]);
                    }
                }
            }

        }


        public void restablecerValoresFunc(FuncsProcs.Funcion func)
        {
            if (valoresFunciones.Count > 0 && func.getLongParams() > 0)
            {
                for (int i = 0; i < func.getParametros().Count; i++)
                {
                    if (func.getParametros()[i].getTipo() == "valor")
                    {
                        func.getParametros()[i].getVariable().setValorTerminal(this.valoresFunciones[i]);
                    }
                }


                for (int i = 0; i < this.entorno[this.entorno.Count - 1].getVariables().Count; i++)
                {
                    if (func.getParametros()[i].getTipo() == "valor")
                    {
                        this.entorno[this.entorno.Count - 1].getVariables()[i].setValorTerminal(this.valoresFunciones[i]);
                    }
                }
            }

        }

        public List<string> valoresPorRef(AST.Nodo nodoAct, List<string>ids)
        {
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            string tipo = nodoAct.getNombre();

            if (tipo=="LLAMADA1")
            {
                if (temp.Length==3)
                {
                    ids = valoresPorRef(temp[0], ids);

                    return ids = valoresPorRef(temp[2], ids);
                }
                else
                {
                    return ids = valoresPorRef(temp[0], ids);
                }
            }
            else
            {
                if (temp[0].getNombre()=="ASIGNACION1")
                {
                    string referencia = getAsignaciones(temp[0], "");

                    ids.Add(referencia);

                    return ids;
                }
                else
                {
                    return ids;
                }
            }

            temp = null;
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

                exp = null;
            }
            else if (temp[n].getNodos()[0].getNombre() == "EXP_LOG")
            {
                ExpresionLogica expL = new ExpresionLogica(ref this.entorno);
                cadena += expL.noce(temp[n].getNodos()[0]);

                expL = null;
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

                    if (temp[0].getHoja().getValor().getValor()=="(")
                    {
                        double valorRetorno = 0;

                        FuncsProcs.Funcion funcion = Form1.buscarFuncion(referencia, "funcion");

                        if (funcion != null)
                        {
                            if (funcion.getLongParams() > 0)
                            {
                                funcion = llamadasFunciones(temp[1], funcion, funcion.getLongParams() - 1);
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

                        for (int i = this.entorno.Count-1; i >= 0 ; i--)
                        {
                            if (this.entorno[i].getVariables()[i].getNombre() == referencia)
                            {
                                if (this.entorno[i].getVariables()[i].getValor().getArreglo()!=null)
                                {

                                }
                            }
                        }

                        return  "";

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
                       // MessageBox.Show("LA VARIABLE "+ idss[0] +"NO EXISTE");
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
                proc = Form1.buscarProcedimiento(temp[0].getHoja().getValor().getValor() + "", "procedimiento");

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
                        proc = llamadasProcedimientos(temp[2], proc, indice);
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

                proc.setValoresParametros(listaParams);

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
                func = Form1.buscarFuncion(temp[0].getHoja().getValor().getValor() + "", "funcion");

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
                    if (proc.getParametro(indice).getTipo() == "referencia" || proc.getParametro(indice).getTipo() == "valor")
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
                        ExpresionLogica expl = new ExpresionLogica(ref this.entorno);

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

                    Instruccion inst = new Instruccion(ref this.entorno);

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
                        ExpresionLogica expl = new ExpresionLogica(ref this.entorno);

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


        //REPORTNA EL VALOR DE UNA PRODUCCION VALOR 
        public string valor(AST.Nodo[] temp, int n)
        {
            string resultado = "";

            if (temp[n].getHoja()!=null)
            {

                resultado = temp[n].getHoja().getValor().getValor() + "";

                string resp = "";

                for (int i = 1; i < resultado.Length-1; i++)
                {
                    resp += resultado[i];
                }

                return resp;


            }
            else
            {
                if (temp[n].getNombre() == "ASIGNACION1")
                {

                    return validarAsignacionAVariable(temp[n].getNodos()[1], temp[n].getNodos()[0].getHoja().getValor().getValor()+"");
                    //RETORNA UN RESULTADO
                }
                else if (temp[n].getNombre() == "EXP")
                {
                    Expresion exp = new Expresion(this.entorno);
                    resultado = exp.noce(temp[n]) + "";
                    return resultado;
                }
                else if (temp[n].getNombre() == "EXP_LOG")
                {
                    ExpresionLogica expL = new ExpresionLogica(ref this.entorno);
                    resultado = expL.noce(temp[n]) + "";
                    return resultado;
                }
                else
                {
                    resultado = temp[n].getNodos().ToArray()[0].getHoja().getValor().getValor() + "";
                    return resultado;
                }
            }


        }



        public void graficarTabla(List<Semantica.Entorno> entorno)
        {

            string tabla = "";
            tabla += "<!DOCTYPE html>\n";
            tabla += "<html>\n";
            tabla += "<head>\n";
            tabla += "<title>TABLA DE SIMBOLOS</title>\n";
            tabla += "<meta charset=" + "utf-8" + '"' + ">\n";
            tabla += "<meta name=" + '"' + "viewport" + '"' + "content=" + '"' + "width=device-width, initial-scale=1" + '"' + ">\n";
            tabla += "<link rel=" + '"' + "stylesheet" + '"' + "href=" + '"' + @"https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" + '"' + ">\n";
            tabla += "<script src=" + '"' + @"https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js" + '"' + "></script>\n";
            tabla += "<script src=" + '"' + @"https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js" + '"' + "></script>\n";
            tabla += "<script src=" + '"' + @"https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js" + '"' + "></script>\n";
            tabla += "</head>\n";
            tabla += "<body>\n";

            for (int i = 0; i < entorno.Count; i++)
            {
                tabla += "<div class=" + '"' + "container" + '"' + ">\n";
                tabla += "<h1>PROYECTO 1 - COMPILADORES 2</h1>\n";
                tabla += "<h5>Tabla de simbolos reconocidos</h5>\n";
                tabla += "<table class=" + '"' + "table table-dark table-striped" + '"' + ">\n";
                tabla += "<thead>\n";
                tabla += "<th>Nombre</th>\n";
                tabla += "<th>Tipo</th>\n";
                tabla += "<th>Valor</th>\n";
                tabla += "<th>Ambito</th>\n";
                tabla += "</thead>\n";
                tabla += "<tbody>\n";

                for (int j = 0; j < entorno[i].getVariables().Count; j++)
                {
                    tabla += "<tr>\n";

                    tabla += "<td>\n";
                    tabla += entorno[i].getVariables()[j].getNombre();
                    tabla += "</td>\n";
                    tabla += "<td>\n";
                    tabla += entorno[i].getVariables()[j].getValor().getTipo();
                    tabla += "</td>\n";
                    tabla += "<td>\n";
                    tabla += entorno[i].getVariables()[j].getValor().getValor();
                    tabla += "<t/d>\n";

                    if (j== entorno[i].getVariables().Count-1)
                    {
                        tabla += "<td>\n";
                        tabla += "GLOBAL";
                        tabla += "<t/d>\n";
                    }
                    else
                    {
                        tabla += "<td>\n";
                        tabla += "LOCAL";
                        tabla += "<t/d>\n";
                    }

                    tabla += "</tr>\n";
                }

                tabla += "</tbody>\n";
                tabla += "</table>\n";
                tabla += "</div>\n";
                tabla += "<br/>\n";
                tabla += "<br/>\n";
            }

            tabla += " </body>\n";
            tabla += "</html>\n";


            File.WriteAllTextAsync("c:\\compiladores2\\tabla.html", tabla);

        }




    }
}
