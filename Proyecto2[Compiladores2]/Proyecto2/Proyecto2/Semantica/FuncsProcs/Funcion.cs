using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.Semantica.FuncsProcs
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


            int inicioHeap = Form1.finHeap;
            if (variablesLocales.Count > 0)
            {
                Form1.finHeap = variablesLocales[variablesLocales.Count - 1].indiceFinStackHeap;
            }

            for (int i = 0; i < this.parametros.Count; i++)
            {
                variablesLocales.Add(this.parametros[i].getVariable());
            }

            if (valor.getTipo() == Terminal.TipoDato.CADENA)
            {
                this.variablesLocales.Add(new Variable(nombre, valor, Form1.finHeap + 20, 20));
                Form1.finHeap += 20;
            }
            else if (valor.getTipo() == Terminal.TipoDato.OBJETO)
            {
                this.variablesLocales.Add(new Variable(nombre, valor, Form1.finHeap + valor.getValorObjeto().getTamanioObjeto(), valor.getValorObjeto().getTamanioObjeto()));
                Form1.finHeap += valor.getValorObjeto().getTamanioObjeto();
            }
            else
            {
                Variable var1 = new Variable(nombre, valor, Form1.finStack, -1);
                var1.indiceStack = Form1.finStack;
                this.variablesLocales.Add(var1);
                Form1.finStack++;
            }
            
            this.entorno.Add(new Entorno(ref variablesLocales, (Form1.finStack - variablesLocales.Count), Form1.finStack, variablesLocales.Count - parametros.Count, Form1.finHeap, inicioHeap));

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


        public List<Variable> getVariables()
        {
            return this.variablesLocales;
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


        public void traducir()
        {

            if (this.funciones.Count>0)
            {
                for (int i = 0; i < this.funciones.Count; i++)
                {
                    this.funciones[i].traducir();
                }
            }
            if (this.procedimientos.Count>0)
            {
                for (int i = 0; i < this.procedimientos.Count; i++)
                {
                    this.procedimientos[i].traducir();
                }
            }
            
            Form1.richTextBox2.Text += "void "+this.nombre + "(){\n";
            int inicioTemps = CodigoI.Temporal.cantidad;
            string codigo1 = Form1.richTextBox2.Text;
            Form1.richTextBox2.Clear();

            Instruccion ins = new Instruccion(ref this.entorno, this.valoresParametros, this.valoresFunciones);
            ins.analizar(this.instrucciones);
            string codigo2 = Form1.richTextBox2.Text;
            Form1.richTextBox2.Clear();

            Form1.richTextBox2.Text += codigo1;
            Form1.richTextBox2.Text += "int ";
            for (int i = inicioTemps; i < CodigoI.Temporal.cantidad-1; i++)
            {
                Form1.richTextBox2.Text += "T"+i+", ";
            }
            Form1.richTextBox2.Text += "T" + CodigoI.Temporal.cantidad + ";\n";
            Form1.richTextBox2.Text += codigo2;

            Form1.richTextBox2.Text += "return;\n";
            Form1.richTextBox2.Text += "}\n";

        }

    }
}
