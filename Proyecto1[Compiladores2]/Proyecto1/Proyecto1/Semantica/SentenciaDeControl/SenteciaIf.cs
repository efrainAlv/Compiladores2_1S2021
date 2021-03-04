using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Proyecto1.Semantica.SentenciaDeControl
{
    class SenteciaIf
    {

        public SenteciaIf()
        {

        }



        public bool analizar(AST.Nodo nodoAct, bool flag)
        {

            string tipo = nodoAct.getNombre();
            AST.Nodo[] temp = nodoAct.getNodos().ToArray();
            int len = temp.Length;

            if (tipo=="IF")
            {
                ExpresionLogica e = new ExpresionLogica();
                if (e.noce(temp[1].getNodos()[0]))
                {
                    MessageBox.Show("If verdadero");

                    return true;
                }
                else
                {
                    //cambiar a 6 cuando esten las instrucciones
                    return analizar(temp[6], false);
                }
                
            }
            else if (tipo=="IF1")
            {
                if (len==2)
                {
                    flag = analizar(temp[0], flag);
                    return analizar(temp[1], flag);
                }
                else
                {
                    if (temp[0].getNombre()=="ELSE")
                    {
                        return analizar(temp[0], flag);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (tipo == "IF2")
            {
                if (temp[0].getNombre()=="ELSE")
                {
                    return analizar(temp[0], flag);
                }
                else
                {
                    return false;
                }
            }
            else if (tipo=="ELSE_IF")
            {
                ExpresionLogica e = new ExpresionLogica();

                if (len==8)
                {
                    flag = analizar(temp[0], flag); // ELSE_IF
                   
                    if (e.noce(temp[3].getNodos()[0]) && !flag)
                    {
                        MessageBox.Show("Else if verdadero");
                        return true;
                    }
                    else
                    {
                        return flag;//flag
                    }

                }
                else
                {
                    if (e.noce(temp[2].getNodos()[0]) && !flag)
                    {
                        MessageBox.Show("Else if verdadero");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            else
            {

                if (!flag)
                {
                    MessageBox.Show("Else verdadero");
                    return true;
                }
                else
                {
                    return false;
                }
                
            }

        }

    }
}
