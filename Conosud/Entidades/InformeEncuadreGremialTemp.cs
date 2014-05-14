using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class InformeEncuadreGremialTemp
    {
        public string Empresa{ get;set;}
        public string SubContratista { get; set; }  
        public string Contrato { get; set; }
        public string Convenio { get; set; }
        public long Cantidad { get; set; }
    }
}
