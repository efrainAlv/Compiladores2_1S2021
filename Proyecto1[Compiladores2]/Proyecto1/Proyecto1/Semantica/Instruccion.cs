using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using System.Windows.Forms;

namespace Proyecto1.Semantica
{
    class Instruccion
    {


        public void analizar(AST.Nodo nodoAct)
        {

            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (tipo=="INSTRUCCIONES")
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
                if (temp[0].getNombre()=="IF")
                {

                }
                else
                {
                    string cadena = getAsignaciones(temp[0], "");
                    string[] vars = cadena.Split(';');
                    string[] ids = cadena.Split(":=")[0].Split('.');
                    string valor = "165645";

                    for (int i = 0; i < Form1.variableGlobales.Count; i++)
                    {
                        if (Form1.variableGlobales.ElementAt(i).getNombre() == ids[0])
                        {   
                            Variable var = asignarAVariable(ids, valor, 0, Form1.variableGlobales.ElementAt(i), null);

                            if (var!=null)
                            {
                                Form1.variableGlobales.ElementAt(i).setValorObjeto(var.getValor().getValorObjeto());
                                MessageBox.Show("LA VARIABLE SI EXISTE");
                            }
                            else
                            {
                                MessageBox.Show("LA VARIABLE NO EXISTE");
                            }

                            break;
                        }
                    }

                }
            }


        }


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
                    cadena += temp[4].getNodos().ToArray()[0].getHoja().getValor().getValor();
                    cadena += temp[5].getHoja().getValor().getValor();

                    return cadena;
                }
                else
                {
                    cadena = getAsignaciones(temp[0], cadena);
                    cadena += temp[1].getHoja().getValor().getValor();
                    cadena += temp[2].getHoja().getValor().getValor();
                    cadena += temp[3].getNodos().ToArray()[0].getHoja().getValor().getValor();
                    cadena += temp[4].getHoja().getValor().getValor();

                    return cadena;
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
                var.getValor().setValor(valor);
                return var;
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


        

    }
}
