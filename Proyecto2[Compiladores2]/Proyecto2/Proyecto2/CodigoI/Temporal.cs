using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.CodigoI
{
    public class Temporal
    {

        public static int cantidad = 1;

        private double arg1;
        private double arg2;

        private Temporal t1;
        private Temporal t2;

        public int indice;

        private string operacion;

        private double valor;

        public Temporal() { }


        public Temporal(double arg1, double arg2, string operacion)
        {

            this.arg1 = arg1;
            this.arg2 = arg2;
            this.operacion = operacion;
            this.valor = operar(arg1, arg2, operacion);

        }

        public Temporal(Temporal t1, double arg2, string operacion)
        {

            this.t1 = t1;
            this.arg2 = arg2;
            this.operacion = operacion;
            this.valor = operar(t1.getValor(), arg2, operacion);

        }

        public Temporal(double arg1, Temporal t2, string operacion)
        {

            this.arg1 = arg1;
            this.t2 = t2;
            this.operacion = operacion;
            this.valor = operar(arg1, t2.getValor(), operacion);

        }

        public Temporal(Temporal t1, Temporal t2, string operacion)
        {

            this.t1 = t1;
            this.t2 = t2;
            this.operacion = operacion;
            this.valor = operar(t1.getValor(), t2.getValor(), operacion);

        }

        private double operar(double arg1, double arg2, string operacion)
        {

            double valor = 0;

            switch (operacion)
            {
                case "+":

                    valor = arg1 + arg2;

                    break;

                case "-":

                    valor = arg1 -arg2;

                    break;

                case "*":

                    valor = arg1 * arg2;

                    break;

                case "/":

                    valor = arg1 / arg2;

                    break;

            }

            return valor;
        }


        public double getValor()
        {
            return this.valor;
        }

        public double getArg1()
        {
            return this.arg1;
        }

        public Temporal getT1()
        {
            return this.t1;
        }

        public Temporal getT2()
        {
            return this.t2;
        }

        public double getArg2()
        {
            return this.arg2;
        }


        public String getOP()
        {
            return this.operacion;
        }


        public bool comparar(Temporal t)
        {
            if (t.arg1!=0 && this.arg1!=0)
            {
                if (t.arg1 == this.arg1)
                {
                    if (t.arg2 != 0 && this.arg2 != 0)
                    {
                        if (t.arg2 == this.arg2)
                        {
                            if (t.valor == this.valor && t.operacion.Equals(this.operacion))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (t.arg2 == 0 && this.arg2 == 0)
                    {
                        if (t.t2 != null && this.t2 != null)
                        {
                            if (t.t2.comparar(this.t2))
                            {
                                if (t.valor == this.valor && t.operacion.Equals(this.operacion))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else if (t.t2 == null && this.t2 == null)
                        {
                            if (t.valor == this.valor && t.operacion.Equals(this.operacion))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (t.arg1 == 0 && this.arg1 == 0)
            {
                if (t.t1.comparar(t1))
                {
                    if (t.arg2 != 0 && this.arg2 != 0)
                    {
                        if (t.arg2 == this.arg2)
                        {

                            if (t.valor == this.valor && t.operacion.Equals(this.operacion))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (t.arg2 == 0 && this.arg2 == 0)
                    {
                        if (t.t2 != null && this.t2 != null)
                        {

                            if (t.t2.comparar(this.t2))
                            {
                                if (t.valor == this.valor && t.operacion.Equals(this.operacion))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else if (t.t2 == null && this.t2 == null)
                        {
                            if (t.valor == this.valor && t.operacion.Equals(this.operacion))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
                

        }


    }
}
