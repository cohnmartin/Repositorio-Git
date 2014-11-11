using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class AltasBajasLegajosTemp
    {
        public string CodigoContrato { get; set; }
        public string NombreCompleto { get; set; }
        public string Nrodoc { get; set; }
        public string NombreEmpresaContratista { get; set; }
        public string NombreEmpresaSubContratista { get; set; }
        public string Accion { get; set; }
        public string DNI { get; set; }
        public string Encuadre { get; set; }
        public string FechaTramite { get; set; }
        public string FechaBaja { get; set; }
        public int UniqueID { get; set; }
        public string Periodo { get; set; }


    }

    public class AltaModificaionVehiculosEquipos
    {
        public long idVehiculo { get; set; }
        public long? idCon { get; set; }
        public string Patente { get; set; }
        public string Marca { get; set; }
        public string CodigoContrato { get; set; }
        public string NombreEmpresa { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool? EsContratista { get; set; }
        public string Accion { get; set; }
        public string Tipo { get; set; }
        public string Periodo { get; set; }
    }
}
