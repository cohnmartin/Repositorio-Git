using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    partial class ArchivosSueldos
    {
        public long PeriodoNumerico
        {
            get {
                long orden = 0;
                if (!this.colDatosDeSueldos.IsLoaded) { this.colDatosDeSueldos.Load(); }
                if (colDatosDeSueldos.Count > 0)
                {
                    orden = long.Parse(this.colDatosDeSueldos.First().Periodo.Value.Year.ToString()
                        + this.colDatosDeSueldos.First().Periodo.Value.Month.ToString());
                }
                return orden;
            }
        }

        public string Periodo
        {
            get
            {
                try
                {
                    if (!this.colDatosDeSueldos.IsLoaded) { this.colDatosDeSueldos.Load(); }
                    if (this.colDatosDeSueldos.Count > 0)
                    {
                        if (this.colDatosDeSueldos.First().Periodo.HasValue)
                        {
                            return this.colDatosDeSueldos.First().Periodo.Value.Month.ToString()
                                + "/" + this.colDatosDeSueldos.First().Periodo.Value.Year.ToString();
                        }
                    }
                    return string.Empty;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public string Empresa
        {
            get
            {
                try
                {
                    if (!this.colDatosDeSueldos.IsLoaded) { this.colDatosDeSueldos.Load(); }
                    if (this.colDatosDeSueldos.Count > 0)
                    {
                        if (!this.colDatosDeSueldos.First().objEmpresaReference.IsLoaded)
                        { this.colDatosDeSueldos.First().objEmpresaReference.Load(); }
                        return this.colDatosDeSueldos.First().objEmpresa.RazonSocial;
                    }
                    return string.Empty;
                }
                catch (Exception)
                {
                    return string.Empty;                    
                }
            }
        }
    }
}
