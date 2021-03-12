using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Semantica.FuncsProcs
{
    public class Procedimiento
    {

        private string nombre;
        private List<Variable> variablesLocales;
        private List<Parametro> parametros;
        private AST.Nodo instrucciones;
        private List<Entorno> entorno;

        private List<Procedimiento> procedimientos;
        private List<Funcion> funciones;
        
        public Procedimiento()
        {
            this.variablesLocales = new List<Variable>();
            this.parametros = new List<Parametro>();
            this.instrucciones = null;
        }

        public Procedimiento(string nombre, List<Variable> variablesLocales, List<Parametro> parametros, AST.Nodo instrucciones, List<Entorno> entorno)
        {
            this.nombre = nombre;
            this.variablesLocales = variablesLocales;
            this.parametros = parametros;
            this.instrucciones = instrucciones;
            this.entorno = entorno;

            if (this.parametros.Count>0)
            {
                //List<Variable> vars = new List<Variable>();
                for (int i = 0; i < this.parametros.Count; i++)
                {
                    variablesLocales.Add(this.parametros[i].getVariable());
                }
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


        public void ejecutar()
        {
            Instruccion ins = new Instruccion(ref this.entorno);

            ins.analizar(this.instrucciones);

            ins = null;
        }

    }
}
