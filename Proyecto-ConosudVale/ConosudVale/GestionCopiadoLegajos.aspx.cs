﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using Telerik.Web.UI;
using Entidades;
using System.Linq;
using System.Data.Linq;

/// <summary>
/// Se realiza el copiado de legajos del ultimo periodo de un contrato a otro contrato,de la misma empresa,
/// siempre y cuando el ultimo periodo del contrato origen sea menor al periodo seleccionado del contrato destino.
/// </summary>
public partial class GestionCopiadoLegajos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadContratosHabilitados();
            gvLegajos.DataSource = new List<Legajos>();
            gvLegajos.DataBind();
        }
    }

    private void LoadContratosHabilitados()
    {
        long Empresa = long.Parse(Request.QueryString["Empresa"].ToString());
        long Contrato = long.Parse(Request.QueryString["Contrato"].ToString());
        long EmpresaContratista = long.Parse(Request.QueryString["Contratista"].ToString());
        long Periodo = long.Parse(Request.QueryString["Periodo"].ToString());

        EntidadesConosud dc = new EntidadesConosud();

        DateTime periodoSeleccionado = (from C in dc.CabeceraHojasDeRuta
                                        where C.IdCabeceraHojasDeRuta == Periodo
                                        select C.Periodo).FirstOrDefault();


        var ContratosSeleccionables = from C in dc.ContratoEmpresas
                                      where C.Empresa.IdEmpresa == Empresa &&
                                      C.IdContratoEmpresas != EmpresaContratista &&
                                      C.CabeceraHojasDeRuta.Max(w => w.Periodo) < periodoSeleccionado
                                      select new { Codigo = C.Contrato.Codigo, IdContratoEmpresa = C.IdContratoEmpresas };

        cboContratoSeleccionable.DataTextField = "Codigo";
        cboContratoSeleccionable.DataValueField = "IdContratoEmpresa";
        cboContratoSeleccionable.DataSource = ContratosSeleccionables;
        cboContratoSeleccionable.DataBind();
    }


    protected void cboContratoSeleccionable_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();

        long IdContratoEmpresaSeleccionado = long.Parse(cboContratoSeleccionable.SelectedItem.Value);

        var legajos = (from L in dc.ContEmpLegajos
                       where L.ContratoEmpresas.IdContratoEmpresas == IdContratoEmpresaSeleccionado
                       orderby L.Legajos.Apellido
                       select L.Legajos).Distinct();

        gvLegajos.DataSource = legajos.OrderBy(w => w.Apellido);
        gvLegajos.DataBind();
        btnCopiado.Enabled = true;
        upAsignar.Update();

    }
    protected void btnCopiado_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();
        List<long> IdSel = new List<long>();
        long IdCabeceraDestinoIncial = long.Parse(Request.QueryString["Periodo"].ToString());
        long EmpresaContratista = long.Parse(Request.QueryString["Contratista"].ToString());




        foreach (GridItem item in gvLegajos.SelectedItems)
        {
            IdSel.Add(long.Parse(gvLegajos.Items[item.DataSetIndex].GetDataKeyValue("IdLegajos").ToString()));

        }

        List<Entidades.Legajos> LegajosSeleccionados = (from l in dc.Legajos
                                                        where IdSel.Contains(l.IdLegajos)
                                                        select l).ToList();

        if (LegajosSeleccionados.Count > 0)
        {

            DateTime periodoSeleccionado = (from C in dc.CabeceraHojasDeRuta
                                            where C.IdCabeceraHojasDeRuta == IdCabeceraDestinoIncial
                                            select C.Periodo).FirstOrDefault();

            /// Recupero las cabeceras donde se tiene que asociar los legajos seleccionados
            var cabeceras = (from C in dc.CabeceraHojasDeRuta
                             where C.Periodo >= periodoSeleccionado
                             && C.ContratoEmpresas.IdContratoEmpresas == EmpresaContratista
                             && C.Estado.IdClasificacion == 15
                             select C).ToList();


            var IdCategoriaContrato = cabeceras.First().ContratoEmpresas.Contrato.Categoria;
            DateTime FechaVencimientoContrato = !cabeceras.First().ContratoEmpresas.Contrato.Prorroga.HasValue ? cabeceras.First().ContratoEmpresas.Contrato.FechaVencimiento.Value : cabeceras.First().ContratoEmpresas.Contrato.Prorroga.Value;
            DateTime FechaAsociacion = new DateTime(periodoSeleccionado.Year, periodoSeleccionado.Month, DateTime.Now.Day);
            DateTime FechaVencCredencial = IdCategoriaContrato == 91 ? DateTime.Now.AddDays(-1) : FechaVencimientoContrato;





            /// Para cada cabecera verifico si el legajo existe y si no existe
            /// entonces creo la asociacion.
            foreach (Entidades.CabeceraHojasDeRuta cab in cabeceras)
            {
                foreach (Legajos leg in LegajosSeleccionados)
                {
                    List<ContEmpLegajos> legajosExistente = cab.ContEmpLegajos.ToList();
                    bool Existe = (from L in legajosExistente
                                  where L.IdLegajos == leg.IdLegajos
                                  select L).Any();

                    if (!Existe)
                    {
                        Entidades.ContEmpLegajos ContEmpLeg = new Entidades.ContEmpLegajos();
                        ContEmpLeg.IdLegajos = leg.IdLegajos;
                        ContEmpLeg.ContratoEmpresas = cab.ContratoEmpresas;
                        ContEmpLeg.CabeceraHojasDeRuta = cab;
                        dc.AddToContEmpLegajos(ContEmpLeg);
                    }

                    /// Actualizo la fecha de vencimiento credencial de cada uno de los 
                    /// legajos copiados.
                    leg.CredVencimiento = FechaVencCredencial;



                    /// Codigo para setear la fecha de baja, pero no se utiliza mas.
                    //try
                    //{
                    //    long IdContratoEmpresaSeleccionadoOrigen = long.Parse(cboContratoSeleccionable.SelectedItem.Value);
                    //    using (EntidadesConosud dc1 = new EntidadesConosud())
                    //    {
                    //        DateTime periodoAnterior = cab.Periodo.AddMonths(-1);
                    //        Entidades.ContEmpLegajos LegajoAsignacionActual = (from L in dc1.ContEmpLegajos
                    //                                                           where L.Legajos.IdLegajos == leg.IdLegajos
                    //                                                           && L.ContratoEmpresas.IdContratoEmpresas == IdContratoEmpresaSeleccionadoOrigen
                    //                                                           && (L.CabeceraHojasDeRuta.Periodo.Month == periodoAnterior.Month && L.CabeceraHojasDeRuta.Periodo.Year == periodoAnterior.Year)
                    //                                                           select L).FirstOrDefault();

                    //        if (LegajoAsignacionActual != null)
                    //        {
                    //            LegajoAsignacionActual.FechaTramiteBaja = DateTime.Now;
                    //        }


                    //    }
                    //}
                    //catch { 

                    //}


                }
            }


            dc.SaveChanges();


            ScriptManager.RegisterStartupScript(upAsignar, typeof(UpdatePanel), "Cerrar", "CloseWindows();", true);
        }

    }
}
