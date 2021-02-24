using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.AST
{
    public class Hoja
    {

        private Semantica.Terminal valor;
        private string id;

        public Hoja()
        {

        }

        public Hoja(Semantica.Terminal valor)
        {
            this.valor = valor;
            this.id = Guid.NewGuid() + "";
        }

        public void setValor(Semantica.Terminal valor)
        {
            this.valor = valor;
        }

        public Semantica.Terminal getValor()
        {
            return this.valor;
        }

        public string getId()
        {
            return this.id;
        }

    }
}
