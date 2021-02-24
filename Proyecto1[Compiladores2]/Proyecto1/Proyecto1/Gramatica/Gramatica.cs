using System;
using System.Collections.Generic;
using System.Text;

using Irony.Ast;
using Irony.Parsing;

namespace Proyecto1.Gramatica
{
    public class Gramatica : Grammar
    {

        public Gramatica() : base(caseSensitive: false)
        {

            #region Expresiones regulares del lenguaje

            RegexBasedTerminal entero = new RegexBasedTerminal("[0-9]+");
            RegexBasedTerminal real = new RegexBasedTerminal("[0-9]+[.][0-9]+");
            IdentifierTerminal id = new IdentifierTerminal("id");
            CommentTerminal cadena = new CommentTerminal("string", "'", ".", "'");

            #endregion


            #region Comentario

            CommentTerminal comentarioLinea = new CommentTerminal("comentarioLinea", "//", "\n", "\r");
            CommentTerminal comentarioMultiLinea1 = new CommentTerminal("comentarioMultiLinea1", "(*", "*)");
            CommentTerminal comentarioMultiLinea2 = new CommentTerminal("comentarioMultiLinea2", "{", "}");

            base.NonGrammarTerminals.Add(comentarioLinea);
            base.NonGrammarTerminals.Add(comentarioMultiLinea1);
            base.NonGrammarTerminals.Add(comentarioMultiLinea2);

            #endregion

            #region Terminales

            var t_string = "string";
            var t_integer = "integer";
            var t_real = "real";
            var t_boolean = "boolean";
            var t_void = "void";
            var t_object = "object";
            var t_program = "program";
            var t_var = "var";
            var t_type = "type";
            var t_array = "array";
            var t_of = "of";
            var t_end = "end";

            var t_true = "true";
            var t_false = "false";

            var t_puntoComa = ";";
            var t_dosPuntos = ":";
            var t_coma = ",";
            var t_igualAritmetico = "=";
            //var t_corcheteApertura = "[";
            //var t_corcheteCierre = "]";
            var t_puntoPunto = "..";

            var t_function = "function";
            var t_procedure = "procedure";
            var t_begin = "begin";

            var t_parentesisApertura = "(";
            var t_parentesisCierre = ")";

            var mas = "+";
            var menos = "-";
            var mult = "*";
            var div = "/";

            var and = "and";
            var or = "or";
            var not = "not";

            var diferente = "<>";
            var mayorIgual = ">=";
            var menorIgual = "<=";
            var mayor = ">";
            var menor = "<";


            #endregion


            #region No terminales

            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal PROYECTO = new NonTerminal("PROYECTO");
            NonTerminal CABECERA = new NonTerminal("CABECERA");
            NonTerminal CABECERA1 = new NonTerminal("CABECERA1");
            NonTerminal VARIABLE = new NonTerminal("VARIABLE");
            NonTerminal VARIABLE1 = new NonTerminal("VARIABLE1");
            NonTerminal LISTA_DEC = new NonTerminal("LISTA_DEC");
            NonTerminal LISTA_DEC1 = new NonTerminal("LISTA_DEC1");
            NonTerminal DECLARACION = new NonTerminal("DECLARACION");
            NonTerminal DECLARACION1 = new NonTerminal("DECLARACION1");
            NonTerminal DECLARACION_OBJETOS = new NonTerminal("DECLARACION_OBJETOS");
            NonTerminal DECLARACION_OBJETOS1 = new NonTerminal("DECLARACION_OBJETOS1");
            NonTerminal ARREGLO = new NonTerminal("ARREGLO");
            NonTerminal OBJETO = new NonTerminal("OBJETO");
            //NonTerminal OBJETO1 = new NonTerminal("OBJETO1");
            NonTerminal TIPO = new NonTerminal("TIPO");
            NonTerminal VALOR = new NonTerminal("VALOR");
            NonTerminal CONSTANTE = new NonTerminal("CONSTANTE");
            NonTerminal CONSTANTE1 = new NonTerminal("CONSTANTE1");
            NonTerminal CUERPO = new NonTerminal("CUERPO");
            NonTerminal CUERPO1 = new NonTerminal("CUERPO1");
            NonTerminal FUNCION = new NonTerminal("FUNCION");
            NonTerminal PARAMETROS_FUNC = new NonTerminal("PARAMETROS_FUNC");
            NonTerminal PARAMETROS_FUNC1 = new NonTerminal("PARAMETROS_FUNC1");
            NonTerminal PARAMETROS_FUNC2 = new NonTerminal("PARAMETROS_FUNC2");
            NonTerminal PROCEDIMIENTO = new NonTerminal("PROCEDIMIENTO");
            NonTerminal PARAMETROS_PROC = new NonTerminal("PARAMETROS_PROC");
            NonTerminal PARAMETROS_PROC1 = new NonTerminal("PARAMETROS_PROC1");
            NonTerminal PARAMETROS_PROC2 = new NonTerminal("PARAMETROS_PROC2");

            NonTerminal EXP = new NonTerminal("EXP");
            NonTerminal EXP_LOG = new NonTerminal("EXP_LOG");
            NonTerminal CONDICION = new NonTerminal("CONDICION");
            NonTerminal CONDICION1 = new NonTerminal("CONDICION1");
            NonTerminal CONDICION_NUM = new NonTerminal("CONDICION_NUM");
            NonTerminal CONDICION_LOG = new NonTerminal("CONDICION_LOG");

            //NonTerminal PARAMETROS_PROC3 = new NonTerminal("PARAMETROS_PROC3");


            #endregion

            #region Gramatica

            INICIO.Rule = EXP;

            PROYECTO.Rule = t_program + id + t_puntoComa + CABECERA + CUERPO;

            CABECERA.Rule = CABECERA + CABECERA1 | CABECERA1;

            CABECERA1.Rule = VARIABLE
                        | DECLARACION_OBJETOS
                        | CONSTANTE
                        | Empty;

            VARIABLE.Rule = VARIABLE + t_var + VARIABLE1 | t_var + VARIABLE1 | Empty;

            VARIABLE1.Rule = VARIABLE1 + DECLARACION + t_dosPuntos + TIPO + t_puntoComa
                           | DECLARACION + t_dosPuntos + TIPO + t_puntoComa;

            LISTA_DEC.Rule = LISTA_DEC + LISTA_DEC1 | LISTA_DEC1;

            LISTA_DEC1.Rule = DECLARACION + t_dosPuntos + TIPO;

            DECLARACION.Rule = DECLARACION + ToTerm(",") + id | id;

            /*
            DECLARACION1.Rule = t_coma + id
                                | id;
            */

            DECLARACION_OBJETOS.Rule = t_type + id + t_igualAritmetico + DECLARACION_OBJETOS1;

            DECLARACION_OBJETOS1.Rule = ARREGLO
                                        | OBJETO;

            ARREGLO.Rule = t_array + ToTerm("[") + entero + t_puntoPunto + entero + ToTerm("]") + t_of + TIPO + t_puntoComa;

            OBJETO.Rule = t_object + VARIABLE + t_end + t_puntoComa;

            //OBJETO1.Rule = MakePlusRule(OBJETO1, LISTA_DEC);

            TIPO.Rule = TIPO
                        | t_real
                        | t_integer
                        | t_boolean
                        | t_string
                        | t_object
                        | id;

            VALOR.Rule = id
                        | entero
                        | cadena
                        | t_true
                        | t_false
                        | real;

            CONSTANTE.Rule = CONSTANTE + CONSTANTE1 | CONSTANTE1;

            CONSTANTE1.Rule = id + t_igualAritmetico + VALOR + t_puntoComa;

            CUERPO.Rule = CUERPO + CUERPO1 | CUERPO1;

            CUERPO1.Rule = FUNCION
                          | PROCEDIMIENTO
                          | Empty;

            FUNCION.Rule = t_function + id + PARAMETROS_FUNC + t_dosPuntos + TIPO + t_puntoComa + VARIABLE + t_begin /*INSTRUCCIONES*/ + t_end + t_puntoComa;

            PARAMETROS_FUNC.Rule = t_parentesisApertura + PARAMETROS_FUNC1 + t_parentesisCierre
                                    | Empty;

            PARAMETROS_FUNC1.Rule = PARAMETROS_FUNC1 + ToTerm(";") + LISTA_DEC | LISTA_DEC
                                    | Empty;

            /*
            PARAMETROS_FUNC2.Rule = t_puntoComa + LISTA_DEC
                                    | LISTA_DEC;
            */

            PROCEDIMIENTO.Rule = t_procedure + id + PARAMETROS_PROC + t_puntoComa + VARIABLE + t_begin /*INSTRUCCIONES*/ + t_end + t_puntoComa;

            PARAMETROS_PROC.Rule = t_parentesisApertura + PARAMETROS_PROC1 + t_parentesisCierre
                                    | Empty;

            PARAMETROS_PROC1.Rule = PARAMETROS_PROC1 + ToTerm(";") + PARAMETROS_PROC2 | PARAMETROS_PROC2;

            PARAMETROS_PROC2.Rule = LISTA_DEC
                                    | t_var + LISTA_DEC;


            //******************************************* EXPRESIONES ***************************************************
            EXP.Rule = EXP + mas + EXP
                     | EXP + menos + EXP
                     | EXP + div + EXP
                     | EXP + mult + EXP
                     | ToTerm("(") + EXP + ToTerm(")")
                     | menos + EXP
                     | entero
                     | real
                     | id;

            EXP_LOG.Rule = EXP_LOG + and + EXP_LOG
                          | EXP_LOG + or + EXP_LOG
                          | ToTerm("(") + EXP_LOG + ToTerm(")")
                          | not + EXP_LOG
                          | t_true
                          | t_false
                          | id;


            CONDICION.Rule = EXP + CONDICION + EXP
                            | EXP_LOG + CONDICION1;

            CONDICION1.Rule = CONDICION_LOG + EXP_LOG
                             | Empty;

            CONDICION_NUM.Rule = CONDICION_LOG
                                | menor
                                | mayor
                                | menorIgual
                                | mayorIgual;

            CONDICION_LOG.Rule = CONDICION_LOG
                                | t_igualAritmetico
                                | diferente;


            #endregion

            RegisterOperators(1, mas, menos);
            RegisterOperators(2, mult, div, and, or);
            RegisterOperators(3, Associativity.Left, menos, not);

            this.Root = INICIO;

        }


    }
}
