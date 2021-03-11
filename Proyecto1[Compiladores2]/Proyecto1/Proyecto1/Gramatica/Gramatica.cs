using System;
using System.Collections.Generic;
using System.Text;

using Irony.Ast;
using Irony.Parsing;

namespace Proyecto1.Gramatica
{
    public class Gramatica : Grammar
    {

        public Gramatica() : base(caseSensitive: true)
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
            var punto = ".";
            var t_coma = ",";
            var t_igualAritmetico = "=";
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

            var if_t = "if";
            var elseIf_t = "else if";
            var else_t = "else";
            var then_t = "then";
            var writeln = "writeln";
            var write = "write";


            #endregion


            #region No terminales

            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal PROYECTO = new NonTerminal("PROYECTO");
            NonTerminal CABECERA = new NonTerminal("CABECERA");
            NonTerminal CABECERA1 = new NonTerminal("CABECERA1");
            NonTerminal VARIABLE = new NonTerminal("VARIABLE");
            NonTerminal VARIABLE1 = new NonTerminal("VARIABLE1");
            NonTerminal VARIABLE2 = new NonTerminal("VARIABLE2");
            NonTerminal LISTA_DEC = new NonTerminal("LISTA_DEC");
            NonTerminal LISTA_DEC1 = new NonTerminal("LISTA_DEC1");
            NonTerminal DECLARACION = new NonTerminal("DECLARACION");
            NonTerminal DECLARACION_OBJETOS = new NonTerminal("DECLARACION_OBJETOS");
            NonTerminal DECLARACION_OBJETOS1 = new NonTerminal("DECLARACION_OBJETOS1");
            NonTerminal ARREGLO = new NonTerminal("ARREGLO");
            NonTerminal OBJETO = new NonTerminal("OBJETO");
            NonTerminal TIPO = new NonTerminal("TIPO");
            NonTerminal VALOR = new NonTerminal("VALOR");
            NonTerminal CONSTANTE = new NonTerminal("CONSTANTE");
            NonTerminal CONSTANTE1 = new NonTerminal("CONSTANTE1");
            NonTerminal CUERPO = new NonTerminal("CUERPO");
            NonTerminal CUERPO1 = new NonTerminal("CUERPO1");
            NonTerminal FUNCION = new NonTerminal("FUNCION");
            NonTerminal PROCEDIMIENTO = new NonTerminal("PROCEDIMIENTO");
            NonTerminal PARAMETROS = new NonTerminal("PARAMETROS");
            NonTerminal PARAMETROS1 = new NonTerminal("PARAMETROS1");
            NonTerminal PARAMETROS2 = new NonTerminal("PARAMETROS2");
            NonTerminal ANIDAR = new NonTerminal("ANIDAR");
            NonTerminal ANIDAR1 = new NonTerminal("ANIDAR1");
            NonTerminal LLAMADA = new NonTerminal("LLAMADA");
            NonTerminal LLAMADA1 = new NonTerminal("LLAMADA1");


            NonTerminal EXP = new NonTerminal("EXP");
            NonTerminal EXP_LOG = new NonTerminal("EXP_LOG");
            NonTerminal CONDICION = new NonTerminal("CONDICION");
            NonTerminal CONDICION_NUM = new NonTerminal("CONDICION_NUM");
            NonTerminal CONDICION_LOG = new NonTerminal("CONDICION_LOG");
            NonTerminal IF = new NonTerminal("IF");
            NonTerminal IF1 = new NonTerminal("IF1");
            NonTerminal IF2 = new NonTerminal("IF2");
            NonTerminal ELSE_IF = new NonTerminal("ELSE_IF");
            NonTerminal ELSE = new NonTerminal("ELSE");

            NonTerminal INSTRUCCIONES = new NonTerminal("INSTRUCCIONES");
            NonTerminal INSTRUCCION = new NonTerminal("INSTRUCCION");
            NonTerminal ASIGNACIONES = new NonTerminal("ASIGNACIONES");
            NonTerminal ASIGNACION = new NonTerminal("ASIGNACION");
            NonTerminal ASIGNACION1 = new NonTerminal("ASIGNACION1");
            NonTerminal ASIGNACION2 = new NonTerminal("ASIGNACION2");



            #endregion

            #region Gramatica

            INICIO.Rule = PROYECTO;

            PROYECTO.Rule = t_program + id + t_puntoComa + CABECERA + CUERPO + t_begin + INSTRUCCIONES + t_end + punto;

            CABECERA.Rule = CABECERA + CABECERA1 | CABECERA1 | Empty;

            CABECERA1.Rule = VARIABLE
                        | DECLARACION_OBJETOS
                        | CONSTANTE;

            VARIABLE.Rule = VARIABLE + t_var + VARIABLE1 | t_var + VARIABLE1 | Empty;

            VARIABLE1.Rule = VARIABLE1 + DECLARACION + t_dosPuntos + TIPO + VARIABLE2 + t_puntoComa
                           | DECLARACION + t_dosPuntos + TIPO + VARIABLE2 + t_puntoComa;

            VARIABLE2.Rule = t_igualAritmetico + VALOR
                            | Empty;

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

            VALOR.Rule = ASIGNACION1
                        | cadena
                        | EXP
                        | EXP_LOG;

            CONSTANTE.Rule = CONSTANTE + CONSTANTE1 | CONSTANTE1;

            CONSTANTE1.Rule = id + t_igualAritmetico + VALOR + t_puntoComa;

            CUERPO.Rule = CUERPO + CUERPO1 | CUERPO1 | Empty;

            CUERPO1.Rule = FUNCION
                          | PROCEDIMIENTO;

            //******************************************* FUNCIONES Y PROCEDIMIENTOS ***************************************************

            FUNCION.Rule = t_function + id + PARAMETROS + t_dosPuntos + TIPO + t_puntoComa + VARIABLE + ANIDAR + t_begin + INSTRUCCIONES + t_end + t_puntoComa;

            ANIDAR.Rule = ANIDAR + ANIDAR1
                        | ANIDAR1;

            ANIDAR1.Rule = FUNCION
                          | PROCEDIMIENTO
                          | Empty;

            PROCEDIMIENTO.Rule = t_procedure + id + PARAMETROS + t_puntoComa + VARIABLE + ANIDAR + t_begin + INSTRUCCIONES + t_end + t_puntoComa;

            PARAMETROS.Rule = t_parentesisApertura + PARAMETROS1 + t_parentesisCierre
                                    | Empty;

            PARAMETROS1.Rule = PARAMETROS1 + ToTerm(";") + PARAMETROS2 | PARAMETROS2;

            PARAMETROS2.Rule = LISTA_DEC
                                    | t_var + LISTA_DEC;

            LLAMADA.Rule = id + t_parentesisApertura + LLAMADA1 + t_parentesisCierre + t_puntoComa;

            LLAMADA1.Rule = LLAMADA1 + t_coma + VALOR
                          | VALOR
                          | Empty;

            //******************************************* EXPRESIONES ***************************************************

            EXP.Rule = EXP + mas + EXP
                     | EXP + menos + EXP
                     | EXP + div + EXP
                     | EXP + mult + EXP
                     | ToTerm("(") + EXP + ToTerm(")")
                     | menos + EXP
                     | entero
                     | real
                     | ASIGNACION1;


            EXP_LOG.Rule = EXP_LOG + and + EXP_LOG
                          | EXP_LOG + or + EXP_LOG
                          | ToTerm("(") + EXP_LOG + ToTerm(")")
                          | ToTerm("(") + EXP_LOG + CONDICION_LOG + EXP_LOG + ToTerm(")")
                          | ToTerm("(") + EXP + CONDICION_NUM + EXP + ToTerm(")")
                          | EXP + CONDICION_NUM + EXP
                          | not + EXP_LOG
                          | t_true
                          | t_false
                          | ASIGNACION1;

            //******************************************* CONDICIONES ***************************************************

            CONDICION.Rule = EXP_LOG;

            CONDICION_NUM.Rule = CONDICION_LOG
                                | menor
                                | mayor
                                | menorIgual
                                | mayorIgual;

            CONDICION_LOG.Rule = CONDICION_LOG
                                | t_igualAritmetico
                                | diferente;

            //******************************************* SENTENCIAS DE CONTROL ***************************************************


            IF.Rule = if_t + CONDICION + ToTerm("then") + t_begin + INSTRUCCIONES + t_end + IF1;

            IF1.Rule = ELSE_IF + IF2
                        | ELSE
                        | Empty;

            IF2.Rule = ELSE
                      | Empty;

            ELSE_IF.Rule = ELSE_IF + else_t + if_t + CONDICION + then_t + t_begin + INSTRUCCIONES + t_end
                        | ToTerm("else") + if_t + CONDICION + then_t + t_begin + INSTRUCCIONES + t_end;

            ELSE.Rule = else_t + ToTerm("begin") + INSTRUCCIONES + t_end + ToTerm(";")
                        | Empty;



            INSTRUCCIONES.Rule = INSTRUCCIONES + INSTRUCCION
                                | INSTRUCCION;

            INSTRUCCION.Rule = IF
                             | ASIGNACIONES
                             | LLAMADA
                             | Empty;


            ASIGNACIONES.Rule = ASIGNACIONES + ASIGNACION + t_dosPuntos + t_igualAritmetico + VALOR + t_puntoComa
                                | ASIGNACION + t_dosPuntos + t_igualAritmetico + VALOR + t_puntoComa;

            ASIGNACION.Rule = ASIGNACION + punto + id
                            | id;

            ASIGNACION1.Rule = id + ASIGNACION2;
            
            ASIGNACION2.Rule =  punto + ASIGNACION
                              | ToTerm("(") + LLAMADA1 + ToTerm(")")
                              | Empty ;

            #endregion


            RegisterOperators(6, Associativity.Left, menos, not);
            RegisterOperators(5, mult, div);
            RegisterOperators(4, mas, menos);
            RegisterOperators(3, and, or);
            RegisterOperators(2, diferente, t_igualAritmetico);





            /*
            RegisterOperators(60, "^");
            RegisterOperators(50, "*", "/", "\\", "mod");
            RegisterOperators(40, "+", "-", "&");
            RegisterOperators(30, "=", "<=", ">=", "<", ">", "<>");
            RegisterOperators(20, "and", "or");
             */




            this.Root = INICIO;

        }


    }
}
