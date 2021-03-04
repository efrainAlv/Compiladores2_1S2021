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

        private List<Semantica.Variable> variableGlobales = new List<Semantica.Variable>();
        private List<Semantica.Objeto> objetos = new List<Semantica.Objeto>();

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

            if (arbol.Root!=null)
            {
                formarArbol(arbol);

                EscrbirArchivo(this.declaraciones + this.relaciones);

                string cadena = getDeclaracion(this.raiz.getNodos()[0], "");

                string[] idValor = cadena.Split(":=");
                string[] ids = idValor[0].Split('.');

                for (int i = 0; i < ids.Length; i++)
                {
                    MessageBox.Show(ids[i]);
                }
                MessageBox.Show("Valor: "+idValor[1]);
                //ejecutar(this.raiz.getNodos()[0]);
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

                    this.variableGlobales = cabecera.getVariables();
                    this.objetos = cabecera.getObjetos();
                }

            }

        }




        public string getDeclaracion(AST.Nodo nodoAct, string cadena)
        {
            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (tipo=="ASIGNACIONES")
            {
                if (len==6)
                {
                    cadena += getDeclaracion(temp[0], cadena);
                    cadena += getDeclaracion(temp[1], cadena);

                    cadena+= temp[2].getHoja().getValor().getValor();
                    cadena += temp[3].getHoja().getValor().getValor();
                    cadena += temp[4].getNodos().ToArray()[0].getHoja().getValor().getValor();
                    
                    return cadena;
                }
                else
                {
                    cadena += getDeclaracion(temp[0], cadena);
                    cadena += temp[1].getHoja().getValor().getValor();
                    cadena += temp[2].getHoja().getValor().getValor();
                    cadena += temp[3].getNodos().ToArray()[0].getHoja().getValor().getValor();

                    return cadena;
                }
                
            }
            else
            {
                if (len==3)
                {
                    cadena += getDeclaracion(temp[0], cadena);
                    cadena += temp[1].getHoja().getValor().getValor();
                    cadena += temp[2].getHoja().getValor().getValor();
                    
                    return cadena;
                }
                else
                {
                    return cadena += temp[0].getHoja().getValor().getValor();
                }
            }

        }




        public async Task EscrbirArchivo(string text)
        {
            await File.WriteAllTextAsync("c:\\compiladores2\\hola.txt", text);
        }


    }
}
