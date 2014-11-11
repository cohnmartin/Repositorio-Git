using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    partial class Empresa
    {

        public string DescArt
        {
            get
            {
                

                
                if (this.Seguros != null)
                {
                    Seguros segART = this.Seguros.Where(w => w.objTipoSeguro != null && w.objTipoSeguro.Descripcion.Contains("ART")).FirstOrDefault();
                    if (segART != null)
                    {
                        if (segART.objCompañia != null)
                            return segART.NroPoliza + " - " + segART.objCompañia.Descripcion;
                        else
                            return segART.NroPoliza;
                    }
                    else
                        return "Sin Asignar";
                }
                else
                    return "Sin Asignar";
            }
        }

        public string DescVida
        {
            get
            {
                Seguros segART = this.Seguros.Where(w => w.objTipoSeguro != null && w.objTipoSeguro.Descripcion.Contains("Vida")).FirstOrDefault();
                if (segART != null)
                {
                    return segART.NroPoliza + " - " + segART.objCompañia != null ? segART.objCompañia.Descripcion : "";
                }
                else
                    return "Sin Asignar";
            }
        }

    }
}
