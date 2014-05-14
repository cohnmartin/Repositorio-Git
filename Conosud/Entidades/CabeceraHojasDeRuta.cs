using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public partial class CabeceraHojasDeRuta
    {
        public string  FechaUltimaPresentacionPrevisional
        {
            get
            {
                try
                {
                    DateTime Fecha = (from C in this.HojasDeRuta
                                      where C.Plantilla.CategoriasItems.Nombre == Entidades.Emuneraciones.CATEGORIAPREVISIONAL && C.DocFechaEntrega != null
                                      select C.DocFechaEntrega).Max().Value;

                    return Fecha.ToShortDateString();
                }
                catch {
                    return "-";
                }
            
            }
        }
        public string FechaUltimaPresentacionSueldos
        {
            get{

                   try
                {
                DateTime Fecha = (from C in this.HojasDeRuta
                                  where C.Plantilla.CategoriasItems.Nombre == Entidades.Emuneraciones.CATEGORIASUELDOS && C.DocFechaEntrega != null
                                  select C.DocFechaEntrega).Max().Value;

                return Fecha.ToShortDateString();
                }
                   catch
                   {
                       return "-";
                   }

            }
        }
        public string FechaUltimaAuditoria
        {
            get{

                try
                {

                    DateTime Fecha = (from C in this.HojasDeRuta
                                      where (C.HojaFechaAprobacion != null || C.HojaFechaControlado != null)
                                      select C.DocFechaEntrega).Max().Value;

                    return Fecha.ToShortDateString();
                }
                catch {

                    return "-";
                }
    
            }
        }
        public string Aprobada
        {
            get 
            {
                if (this.FechaAprobacion.HasValue)
                {
                    if (this.FechaAprobacionSinLegajos.HasValue)
                        return "Aprobada SL";
                    else
                        return "Aprobada";
                }
                else
                {
                    return "No Aprobada";
                }
            }
        }
        public string Publicada
        {
            get
            {
                if (this.Publicar.HasValue && this.Publicar.Value)
                    return "Si";
                else
                {
                    return "No";
                }
                
            }
        }

    }
}
