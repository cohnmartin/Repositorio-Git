using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    partial class HojasDeRuta
    {
        private string _tipoDesaprobacion="";
        public string TipoDesaprobacion
        {
            get
            {
               return _tipoDesaprobacion;
            }

            set
            {
               _tipoDesaprobacion = value;
            }

        }


    }
}
