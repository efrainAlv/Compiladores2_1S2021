using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Irony.Parsing;
using Irony.Ast;

using System.IO;
using System.Threading.Tasks;

namespace Proyecto1
{
    public partial class Form1 : Form
    {

        public AST.Nodo raiz;
        private string declaraciones = "";
        private string relaciones = "";

        public static List<Semantica.Variable> variableGlobales = new List<Semantica.Variable>();
        public static List<Semantica.Objeto> objetos = new List<Semantica.Objeto>();
        public static List<Semantica.FuncsProcs.Procedimiento> procedimientos = new List<Semantica.FuncsProcs.Procedimiento>();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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


        private void button1_Click(object sender, EventArgs e)
        {
            this.relaciones = "";
            this.declaraciones = "";

            Gramatica.Gramatica g = new Gramatica.Gramatica();

            LanguageData lenguaje = new LanguageData(g);

            Parser parser = new Parser(lenguaje);

            ParseTree arbol = parser.Parse(richTextBox1.Text);

            if (arbol.Root!=null)
            {
                formarArbol(arbol);

                EscrbirArchivo(this.declaraciones + this.relaciones);

                //***********************************************************************************


                // MessageBox.Show("Valor: "+idValor[1]);

                //***********************************************************************************


                ejecutar(this.raiz.getNodos()[0]);
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

                    this.declaraciones += '"' + nodoAct.getId() + '"' + "[label=" + '"' + nodoAct.getNombre() + '"' + "shape=" + '"' + "rectangle" + '"' + "];\n";

                    AST.Nodo nodo = new AST.Nodo(nodoArbol.ChildNodes[i].Term.Name);

                    this.declaraciones += '"' + nodo.getId() + '"' + "[label=" + '"' + nodo.getNombre() + '"' + "shape=" + '"' + "rectangle" + '"' + "];\n";

                    nodoAct.addNodo(recorrerArbol(nodoArbol.ChildNodes[i], nodo));

                    this.relaciones += '"' + nodoAct.getId() + '"' + "->" + '"' + nodo.getId() + '"' + ";\n";
                }
            }
            else
            {
                if (nodoArbol.Token!=null)
                {
                    Semantica.Terminal tempT = new Semantica.Terminal(nodoArbol.Token.Value, Semantica.Terminal.TipoDato.ENTERO);
                    AST.Hoja tempH = new AST.Hoja(tempT);
                    nodoAct.setHoja(tempH);
                    //AST.Nodo nodo = new AST.Nodo("Terminal", tempH);
                    //nodoAct = nodo;

                    this.relaciones += '"' + nodoAct.getId() + '"' + "->" + '"' + nodoAct.getHoja().getId() + '"' + ";\n";

                    this.declaraciones += '"' + nodoAct.getHoja().getId() + '"' + "[label=" + '"' + nodoAct.getHoja().getValor().getValor() + '"' + "shape=" + '"' + "circle" + '"' + "style=" + '"' + "filled" + '"' + " fillcolor=green];\n";
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
                    Semantica.Cabecera cabecera = new Semantica.Cabecera();
                    cabecera.analizar(temp[3]);

                    variableGlobales = cabecera.getVariables();
                    objetos = cabecera.getObjetos();
                }
                if (temp[4].getNombre() == "CUERPO")
                {
                    Semantica.FuncsProcs.Cuerpo cuerpo = new Semantica.FuncsProcs.Cuerpo();
                    cuerpo.analizar(temp[4]);

                    procedimientos = cuerpo.getProcedimientos();
                }
                if (temp[6].getNombre()=="INSTRUCCIONES")
                {
                    Semantica.Instruccion ins = new Semantica.Instruccion();

                    ins.analizar(temp[6]);

                }

            }

        }




     
        
        public async Task EscrbirArchivo(string text)
        {
            await File.WriteAllTextAsync("c:\\compiladores2\\hola.txt", text);
        }


    }
}
