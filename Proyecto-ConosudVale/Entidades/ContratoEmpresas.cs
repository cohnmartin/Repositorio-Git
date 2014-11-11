using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    partial class ContratoEmpresas
    {
        public string ConstratistaParaSubConstratista
        {
            get
            {
                if (! this.EsContratista.Value)
                {
                    if (!this.ContratoReference.IsLoaded) { this.ContratoReference.Load(); }
                    if (!this.Contrato.ContratoEmpresas.IsLoaded) { this.Contrato.ContratoEmpresas.Load(); }
                    
                    ContratoEmpresas contEmp = this.Contrato.ContratoEmpresas.Where(c => c.EsContratista.Value).FirstOrDefault();

                    if (contEmp != null)
                    {
                        if (!contEmp.EmpresaReference.IsLoaded) { contEmp.EmpresaReference.Load(); }
                        return contEmp.Empresa.RazonSocial;
                    }
                }
                
                return this.Constratista;
            }
        }

        public string CUITConstratistaParaSubConstratista
        {
            get
            {
                if (!this.EsContratista.Value)
                {
                    if (!this.ContratoReference.IsLoaded) { this.ContratoReference.Load(); }
                    if (!this.Contrato.ContratoEmpresas.IsLoaded) { this.Contrato.ContratoEmpresas.Load(); }

                    ContratoEmpresas contEmp = this.Contrato.ContratoEmpresas.Where(c => c.EsContratista.Value).FirstOrDefault();

                    if (contEmp != null)
                    {
                        if (!contEmp.EmpresaReference.IsLoaded) { contEmp.EmpresaReference.Load(); }
                        return contEmp.Empresa.CUIT;
                    }
                }

                return this.Constratista;
            }
        }


        public string Constratista
        {
            get
            {
                if (this.EsContratista.Value)
                {
                    return this.Empresa.RazonSocial;
                }
                return string.Empty;
            }
        }


        public string SubConstratista
        {
            get
            {
                if (! this.EsContratista.Value)
                {
                    return this.Empresa.RazonSocial;
                }
                return string.Empty;
            }
        }


        public string DescSubConstratista
        {
            get
            {
                if (!this.EsContratista.Value)
                {
                    if (!this.EmpresaReference.IsLoaded) { this.EmpresaReference.Load(); }
                    return this.Empresa.RazonSocial;
                }
                return " ";
            }
        }

        public string DescCUITSubConstratista
        {
            get
            {
                if (!this.EsContratista.Value)
                {
                    if (!this.EmpresaReference.IsLoaded) { this.EmpresaReference.Load(); }
                    return this.Empresa.CUIT;
                }
                return " ";
            }
        }

        public string DescConstratista
        {
            get
            {
                if (this.EsContratista.Value)
                {
                    if (!this.EmpresaReference.IsLoaded) { this.EmpresaReference.Load(); }
                    return this.Empresa.RazonSocial;
                }
                else
                    return ConstratistaParaSubConstratista;
                
            }
        }

        public string DescCUITConstratista
        {
            get
            {
                if (this.EsContratista.Value)
                {
                    if (!this.EmpresaReference.IsLoaded) { this.EmpresaReference.Load(); }
                    return this.Empresa.CUIT;
                }
                else
                    return CUITConstratistaParaSubConstratista;

            }
        }

    }
}
