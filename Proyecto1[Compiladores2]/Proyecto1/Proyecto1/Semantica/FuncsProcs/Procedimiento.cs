﻿using System;
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

        public Procedimiento()
        {
            this.variablesLocales = new List<Variable>();
            this.parametros = new List<Parametro>();
            this.instrucciones = null;
        }

        public Procedimiento(string nombre, List<Variable> variablesLocales, List<Parametro> parametros, AST.Nodo instrucciones)
        {
            this.nombre = nombre;
            this.variablesLocales = variablesLocales;
            this.parametros = parametros;
            this.instrucciones = instrucciones;
        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }

        


        

    }
}