using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class InformeMensualEmpresaAuditarTemp
    {
        public string DescContratista { get; set; }
        public string DescSubContratista { get; set; }
        public DateTime Periodo { get; set; }
        public string CodigoContrato { get; set; }
        public DateTime? FechaUltimaPresentacionPrevisional { get; set; }
        public DateTime? FechaUltimaPresentacionSueldos { get; set; }
        public DateTime? FechaUltimaAuditoria { get; set; }
        public string Aprobada { get; set; }
        public string Publicada { get; set; }
        public Entidades.CabeceraHojasDeRuta Cab { get; set; }
        public Entidades.ContratoEmpresas ContratoEmpresa { get; set; }

    }
}
