using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    partial class Legajos
    {
        private string _nombreCompleto;
        public string NombreCompleto
        {
            get
            {
                return Helper.ToCapitalize(this.Apellido.ToLower()) + ", " + Helper.ToCapitalize(this.Nombre.ToLower());
            }
            set {
                _nombreCompleto = value;
            }
        }



        public string PrestacionEmergencia
        {
            get
            {
                if (this.objEmpresaLegajo != null)
                {
                    return this.objEmpresaLegajo.PrestacionEmergencia;
                }
                return string.Empty;
            }
        }

        public string AutorizadoConducir
        {
            get
            {
                if (this.AutorizadoCond.HasValue && this.AutorizadoCond.Value)
                {
                    return "SI";
                }
                return "NO";
            }
        }

        public string Empresa
        {
            get
            {
                if (this.objEmpresaLegajo != null)
                    return this.objEmpresaLegajo.RazonSocial;
                else
                    return string.Empty;
            }
        }

        public string Contratista
        {
            get
            {
                if (this.objEmpresaLegajo != null)
                    return this.objEmpresaLegajo.RazonSocial;
                else
                    return string.Empty;
            }
        }

        public string RAC1
        {
            get
            {
                CursosLegajos cur = this.CursosLegajos.FirstOrDefault();
                if (cur != null)
                {
                    return cur.objCurso.Descripcion;
                }
                return string.Empty;
            }
        }

        public string RAC2
        {
            get
            {
                if (this.CursosLegajos.Count > 1)
                {
                    return this.CursosLegajos.ToList()[1].objCurso.Descripcion;
                }
                return string.Empty;
            }
        }

        public string RAC3
        {
            get
            {
                if (this.CursosLegajos.Count > 2)
                {
                    return this.CursosLegajos.ToList()[2].objCurso.Descripcion;
                }
                return string.Empty;
            }
        }

    }
}