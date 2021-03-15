using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica.FuncsProcs
{
    public class Funcion
    {

        private string nombre;
        private List<Variable> variablesLocales;
        private List<Parametro> parametros;
        private AST.Nodo instrucciones;
        private Terminal valor;
        private List<Entorno> entorno;

        private List<Procedimiento> procedimientos;
        private List<Funcion> funciones;

        private List<string> valoresParametros;
        private List<string> valoresFunciones;

        public Funcion()
        {
            this.variablesLocales = new List<Variable>();
            this.parametros = new List<Parametro>();
            this.instrucciones = null;
        }

        public Funcion(string nombre, Terminal valor, List<Variable> variablesLocales, List<Parametro> parametros, AST.Nodo instrucciones, List<Entorno> entorno)
        {
            this.nombre = nombre;
            this.variablesLocales = variablesLocales;
            this.parametros = parametros;
            this.instrucciones = instrucciones;
            this.entorno = entorno;

            this.valoresParametros = new List<string>();
            this.valoresFunciones = new List<string>();
            this.procedimientos = new List<Procedimiento>();
            this.funciones = new List<Funcion>();

            if (this.parametros.Count > 0)
            {
                //List<Variable> vars = new List<Variable>();
                for (int i = 0; i < this.parametros.Count; i++)
                {
                    variablesLocales.Add(this.parametros[i].getVariable());
                }
                this.variablesLocales.Add(new Variable(nombre, valor));
                this.entorno.Add(new Entorno(ref variablesLocales));
            }
            else
            {
                this.variablesLocales.Add(new Variable(nombre, valor));
                this.entorno.Add(new Entorno(ref variablesLocales));
            }
            

        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }

        public string getNombre()
        {
            return this.nombre;
        }

        public List<Entorno> getEntorno()
        {
            return this.entorno;
        }

        public void addProcedimiento(Procedimiento proc)
        {
            this.procedimientos.Add(proc);
        }

        public void addProcedimientos(List<Procedimiento> proc)
        {
            this.procedimientos = proc;
        }

        public void addFuncion(Funcion func)
        {
            this.funciones.Add(func);
        }

        public void addFunciones(List<Funcion> funcs)
        {
            this.funciones = funcs;
        }

        public Parametro getParametro(int n)
        {
            return this.parametros[n];
        }


        public int getLongParams()
        {
            return this.parametros.Count;
        }


        public List<Procedimiento> getProcedimientos()
        {
            return this.procedimientos;
        }


        public List<Funcion> getFunciones()
        {
            return this.funciones;
        }


        public List<Parametro> getParametros()
        {
            return this.parametros;
        }



        public void ejecutar()
        {
            Instruccion ins = new Instruccion(ref this.entorno, this.valoresParametros, this.valoresFunciones);

            Form1.indiceFunciones.Add(true);

            ins.analizar(this.instrucciones);

            Form1.indiceFunciones.RemoveAt(Form1.indiceFunciones.Count - 1);

            Form1.continueIns = false;

            ins = null;
        }


    }
}
