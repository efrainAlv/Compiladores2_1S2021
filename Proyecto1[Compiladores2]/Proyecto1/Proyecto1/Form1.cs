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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.relaciones = "";
            this.declaraciones = "";

            Gramatica.Gramatica g = new Gramatica.Gramatica();

            LanguageData lenguaje = new LanguageData(g);

            Parser parser = new Parser(lenguaje);

            ParseTree arbol = parser.Parse(richTextBox1.Text);

            formarArbol(arbol);

            EscrbirArchivo(this.declaraciones + this.relaciones);

            Semantica.Cabecera cond = new Semantica.Cabecera();
            cond.agregarVaraibles(raiz.getNodos().ElementAt(0));
            //MessageBox.Show(cond.verificar(raiz.getNodos().ElementAt(0))+"");


            Semantica.ExpresionLogica exp = new Semantica.ExpresionLogica();

            //exp.evaluarExpresion(raiz.getNodos().ElementAt(0));

            


        }


        //(5+5+(2+3))+15
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


        public async Task EscrbirArchivo(string text)
        {
            await File.WriteAllTextAsync("c:\\compiladores2\\hola.txt", text);
        }


    }
}
