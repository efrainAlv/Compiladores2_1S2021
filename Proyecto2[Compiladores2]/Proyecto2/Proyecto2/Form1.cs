using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections;

using Irony.Parsing;
using Irony.Ast;

using System.Diagnostics;

using System.IO;
using System.Threading.Tasks;

namespace Proyecto2
{
    public partial class Form1 : Form
    {

        public AST.Nodo raiz;
        private string declaraciones = "";
        private string relaciones = "";
        public static int etiquetas = 0;

        public static List<Semantica.Variable> variableGlobales = new List<Semantica.Variable>();
        public static List<Semantica.Objeto> objetos = new List<Semantica.Objeto>();
        public static List<Semantica.FuncsProcs.Procedimiento> procedimientos = new List<Semantica.FuncsProcs.Procedimiento>();
        public static List<Semantica.FuncsProcs.Funcion> funciones = new List<Semantica.FuncsProcs.Funcion>();
        public Semantica.Entorno entorno;

        public static List<bool> indiceCiclos = new List<bool>();

        public static List<bool> indiceFunciones = new List<bool>();

        public static bool continueIns = false;

        public static bool exitIns = false;

        public static List<Semantica.FuncsProcs.Procedimiento> procemientoAnalizado = new List<Semantica.FuncsProcs.Procedimiento>();
        public static List<Semantica.FuncsProcs.Funcion> funcionAnalizada = new List<Semantica.FuncsProcs.Funcion>();

        public static RichTextBox richTextBox2 = new RichTextBox();

        public static List<CodigoI.Temporal> temps = new List<CodigoI.Temporal>();

        public Form1()
        {
            InitializeComponent();
            AST.Consola con = new AST.Consola();
            richTextBox2 = con.richTextBox2;
            this.Controls.Add(richTextBox2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        public static CodigoI.Temporal agregarTemporal(CodigoI.Temporal temp)
        {
            for (int i = 0; i < temps.Count; i++)
            {
                if (temp.comparar(temps[i]))
                {
                    return temps[i];
                }
            }

            temp.indice = CodigoI.Temporal.cantidad;
            CodigoI.Temporal.cantidad++;

            if (temp.getT1()==null)
            {
                richTextBox2.Text += "T"+temp.indice+" = "+temp.getArg1()+" ";
            }
            else
            {
                richTextBox2.Text += "T" + temp.indice + " = T" + temp.getT1().indice+" ";
            }
            richTextBox2.Text += temp.getOP();
            if (temp.getT2() == null)
            {
                richTextBox2.Text += " "+temp.getArg2() + "; \n";
            }
            else
            {
                richTextBox2.Text += " T" + temp.getT2().indice+"; \n";
            }

            temps.Add(temp);
            return temp;
        }


        public static Semantica.Objeto buscarObjeto(string nombre)
        {
            for (int i = 0; i < objetos.Count; i++)
            {
                if (objetos.ElementAt(i).getNombre()==nombre)
                {
                    return objetos.ElementAt(i);
                }
            }

            return null; 
        }

        public static Semantica.FuncsProcs.Procedimiento buscarProcedimiento(string nombre, string tipo)
        {
            
            if (procemientoAnalizado.Count > 0)
            {
                for (int i = 0; i < procemientoAnalizado[procemientoAnalizado.Count - 1].getProcedimientos().Count; i++)
                {
                    if (procemientoAnalizado[procemientoAnalizado.Count - 1].getProcedimientos()[i].getNombre() == nombre)
                    {
                        return procemientoAnalizado[procemientoAnalizado.Count - 1].getProcedimientos()[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < procedimientos.Count; i++)
                {
                    if (procedimientos[i].getNombre() == nombre)
                    {
                        return procedimientos[i];
                    }
                }
            }
            
            
            if (funcionAnalizada.Count > 0)
            {
                for (int i = 0; i < funcionAnalizada[funcionAnalizada.Count - 1].getProcedimientos().Count; i++)
                {
                    if (funcionAnalizada[funcionAnalizada.Count - 1].getProcedimientos()[i].getNombre() == nombre)
                    {
                        return funcionAnalizada[funcionAnalizada.Count - 1].getProcedimientos()[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < procedimientos.Count; i++)
                {
                    if (procedimientos[i].getNombre() == nombre)
                    {
                        return procedimientos[i];
                    }
                }
            }
            

            return null;
        }

        public static Semantica.FuncsProcs.Funcion buscarFuncion(string nombre, string tipo)
        {


            if (funcionAnalizada.Count > 0)
            {
                for (int i = 0; i < funcionAnalizada[funcionAnalizada.Count - 1].getFunciones().Count; i++)
                {
                    if (funcionAnalizada[funcionAnalizada.Count - 1].getFunciones()[i].getNombre() == nombre)
                    {
                        return funcionAnalizada[funcionAnalizada.Count - 1].getFunciones()[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < funciones.Count; i++)
                {
                    if (funciones[i].getNombre() == nombre)
                    {
                        return funciones[i];
                    }
                }
            }

            if (procemientoAnalizado.Count > 0)
            {
                for (int i = 0; i < procemientoAnalizado[procemientoAnalizado.Count - 1].getFunciones().Count; i++)
                {
                    if (procemientoAnalizado[procemientoAnalizado.Count - 1].getFunciones()[i].getNombre() == nombre)
                    {
                        return procemientoAnalizado[procemientoAnalizado.Count - 1].getFunciones()[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < funciones.Count; i++)
                {
                    if (funciones[i].getNombre() == nombre)
                    {
                        return funciones[i];
                    }
                }
            }


            return null;
        }


        public static Semantica.Variable buscarVariable(string nombre)
        {
            for (int i = 0; i < variableGlobales.Count; i++)
            {
                if (variableGlobales.ElementAt(i).getNombre() == nombre)
                {
                    return variableGlobales.ElementAt(i);
                }
            }

            return null;
        }

        public static void addProcedimientos(List<Semantica.FuncsProcs.Procedimiento> procs)
        {
            for (int i = 0; i < procs.Count; i++)
            {
                procedimientos.Add(procs[i]);
            }
        }

        public static void addFunciones(List<Semantica.FuncsProcs.Funcion> funcs)
        {
            for (int i = 0; i < funcs.Count; i++)
            {
                funciones.Add(funcs[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            variableGlobales = new List<Semantica.Variable>();
            objetos = new List<Semantica.Objeto>();
            procedimientos = new List<Semantica.FuncsProcs.Procedimiento>();
            funciones = new List<Semantica.FuncsProcs.Funcion>();
            richTextBox2.Text = "";

            indiceCiclos = new List<bool>();
            indiceFunciones = new List<bool>();
            continueIns = false;
            exitIns = false;
            procemientoAnalizado = new List<Semantica.FuncsProcs.Procedimiento>();
            funcionAnalizada = new List<Semantica.FuncsProcs.Funcion>();
            temps = new List<CodigoI.Temporal>();
            CodigoI.Temporal.cantidad = 1;
            etiquetas = 0;

            this.relaciones = "";
            this.declaraciones = "";

            Gramatica.Gramatica g = new Gramatica.Gramatica();

            LanguageData lenguaje = new LanguageData(g);

            Parser parser = new Parser(lenguaje);

            ParseTree arbol = parser.Parse(richTextBox1.Text);

            if (arbol.Root!=null)
            {
                MessageBox.Show("Sintaxis correcta");

                formarArbol(arbol);

                arbol = null;
                GC.Collect();



                EscrbirArchivo(this.declaraciones + this.relaciones);

                //***********************************************************************************


                // MessageBox.Show("Valor: "+idValor[1]);

                //***********************************************************************************

                List<Semantica.Entorno> ent = new List<Semantica.Entorno>();

                //Semantica.ExpresionLogica cf = new Semantica.ExpresionLogica(ref ent);
                //Semantica.FuncsProcs.cicloFor cf = new Semantica.FuncsProcs.cicloFor(ent);
                //Semantica.SentenciaDeControl.Repeat cf = new Semantica.SentenciaDeControl.Repeat(ent);
                //Semantica.SentenciaDeControl.sentenciaWhile cf = new Semantica.SentenciaDeControl.sentenciaWhile(ent);
                //Semantica.SentenciaDeControl.SenteciaIf cf = new Semantica.SentenciaDeControl.SenteciaIf(ent);
                //Semantica.Cabecera cf = new Semantica.Cabecera(ent);

                //cf.traducir(this.raiz.getNodos()[0]);
                //cf.imptimirVeraderas();
                //cf.imprimirFalsas();

                traducir(this.raiz.getNodos()[0]);

            }
            else
            {
                MessageBox.Show("Error en la sintaxis");
                richTextBox2.Text = ">>>> Error en la fila: " + arbol.ParserMessages[0].Location.Line + " Columna: " + arbol.ParserMessages[0].Location.Column+"\n";
                richTextBox2.Text += ">>>> Se esperaba: \n";
                for (int i = 0; i < arbol.ParserMessages[0].ParserState.ExpectedTerminals.Count; i++)
                {
                    richTextBox2.Text += ">> "+ arbol.ParserMessages[0].ParserState.ExpectedTerminals.ElementAt(i).Name+"\n";
                }

            }

        }
       

        private void formarArbol(ParseTree arbol)
        {
            raiz = new AST.Nodo("INICIO");
            raiz = recorrerArbol(arbol.Root, raiz);
        }

        private AST.Nodo recorrerArbol(ParseTreeNode nodoArbol, AST.Nodo nodoAct)
        {

            if (nodoArbol.ChildNodes.Count != 0)
            {
                for (int i = 0; i < nodoArbol.ChildNodes.Count; i++)
                {

                    //this.declaraciones += '"' + nodoAct.getId() + '"' + "[label=" + '"' + nodoAct.getNombre() + '"' + "shape=" + '"' + "rectangle" + '"' + "];\n";

                    AST.Nodo nodo = new AST.Nodo(nodoArbol.ChildNodes[i].Term.Name);

                    //this.declaraciones += '"' + nodo.getId() + '"' + "[label=" + '"' + nodo.getNombre() + '"' + "shape=" + '"' + "rectangle" + '"' + "];\n";

                    nodoAct.addNodo(recorrerArbol(nodoArbol.ChildNodes[i], nodo));

                    //this.relaciones += '"' + nodoAct.getId() + '"' + "->" + '"' + nodo.getId() + '"' + ";\n";
                }
            }
            else
            {
                if (nodoArbol.Token!=null)
                {

                    String tipoVar = nodoArbol.Token.Value + "";
                    Semantica.Terminal.TipoDato tipoDato;


                    if (tipoVar == "false")
                    {
                        tipoDato = Semantica.Terminal.TipoDato.BOOLEANO;
                    }
                    else if (tipoVar == "true")
                    {
                        tipoDato = Semantica.Terminal.TipoDato.BOOLEANO;
                    }
                    else if (tipoVar.Contains("'"))
                    {
                        tipoDato = Semantica.Terminal.TipoDato.CADENA;
                    }
                    else if (tipoVar.Contains("."))
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
                            tipoDato = Semantica.Terminal.TipoDato.SIMBOLO;
                        }
                    }

                    Semantica.Terminal tempT = new Semantica.Terminal(nodoArbol.Token.Value.ToString().ToLower(), tipoDato);
                    AST.Hoja tempH = new AST.Hoja(tempT);
                    nodoAct.setHoja(tempH);

                    //this.relaciones += '"' + nodoAct.getId() + '"' + "->" + '"' + nodoAct.getHoja().getId() + '"' + ";\n";

                    //this.declaraciones += '"' + nodoAct.getHoja().getId() + '"' + "[label=" + '"' + nodoAct.getHoja().getValor().getValor() + '"' + "shape=" + '"' + "circle" + '"' + "style=" + '"' + "filled" + '"' + " fillcolor=green];\n";
                }
            }

            return nodoAct;
        }


        private void ejecutar(AST.Nodo nodoAct)
        {

            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (tipo=="PROYECTO")
            {
                if (temp[3].getNombre() == "CABECERA") //CABECERA
                {
                    List<Semantica.Entorno> entornos = new List<Semantica.Entorno>();
                    Semantica.Cabecera cabecera = new Semantica.Cabecera(entornos);
                    cabecera.analizar(temp[3]);

                    variableGlobales = cabecera.getVariables();
                    entorno = new Semantica.Entorno(ref variableGlobales, 0, variableGlobales.Count, variableGlobales.Count, 0, 0);
                    //objetos = cabecera.getObjetos();
                }
                if (temp[4].getNombre() == "CUERPO")
                {
                    List<Semantica.Entorno> entornos = new List<Semantica.Entorno>();
                    entornos.Add(entorno);
                    Semantica.FuncsProcs.Cuerpo cuerpo = new Semantica.FuncsProcs.Cuerpo(entornos);
                    cuerpo.analizar(temp[4]);

                    //procedimientos = cuerpo.getProcedimientos();
                }
                if (temp[6].getNombre()=="INSTRUCCIONES")
                {

                    GC.Collect();

                    List<Semantica.Entorno> entornos = new List<Semantica.Entorno>();
                    entornos.Add(entorno);

                    Semantica.Instruccion ins = new Semantica.Instruccion(ref entornos);

                    ins.analizar(temp[6]);

                }

            }

        }



        private void traducir(AST.Nodo nodoAct)
        {
            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (tipo == "PROYECTO")
            {
                if (temp[3].getNombre() == "CABECERA") //CABECERA
                {
                    List<Semantica.Entorno> entornos = new List<Semantica.Entorno>();
                    Semantica.Cabecera cabecera = new Semantica.Cabecera(entornos);
                    cabecera.traducir(temp[3]);

                    variableGlobales = cabecera.getVariables();
                    entorno = new Semantica.Entorno(ref variableGlobales, 0, variableGlobales.Count, variableGlobales.Count, 0, 0);
                    //objetos = cabecera.getObjetos();
                }
                if (temp[4].getNombre() == "CUERPO")
                {
                    List<Semantica.Entorno> entornos = new List<Semantica.Entorno>();
                    entornos.Add(entorno);
                    //Semantica.FuncsProcs.Cuerpo cuerpo = new Semantica.FuncsProcs.Cuerpo(entornos);
                    //cuerpo.analizar(temp[4]);

                    //procedimientos = cuerpo.getProcedimientos();
                }
                if (temp[6].getNombre() == "INSTRUCCIONES")
                {

                    GC.Collect();

                    List<Semantica.Entorno> entornos = new List<Semantica.Entorno>();
                    entornos.Add(entorno);

                    Semantica.Instruccion ins = new Semantica.Instruccion(ref entornos);

                    ins.analizar(temp[6]);

                }

            }

        }


        public void graficarAST()
        {
            string path = "dot -Tpng " + "c:\\compiladores2\\ast.txt" + " -o " + " c:\\compiladores2\\ast.png";

            using (var dot = new Process())
            {
                dot.StartInfo.Verb = "runas"; // Run process as admin.
                dot.StartInfo.FileName = "cmd.exe";
                dot.StartInfo.Arguments = "/C "+path;
                dot.Start();
                dot.WaitForExit();
            }

        }

        public async Task EscrbirArchivo(string text)
        {
            text = "digraph G{ \n" + text + " \n}";

            await File.WriteAllTextAsync("c:\\compiladores2\\ast.txt", text);
        }



        private void button2_Click(object sender, EventArgs e)
        {
            graficarAST();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}

