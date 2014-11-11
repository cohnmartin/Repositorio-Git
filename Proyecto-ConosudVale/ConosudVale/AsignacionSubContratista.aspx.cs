using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using System.Data.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using Telerik.Web.UI;

public partial class AsignacionSubContratista : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            Entidades.EntidadesConosud dcAux = new Entidades.EntidadesConosud();
            long idContrado = long.Parse(Request.QueryString["IdContrato"].ToString());

            long[] empresasExistentes = (from CE in dcAux.ContratoEmpresas 
                            where  CE.Contrato.IdContrato ==idContrado
                            select CE.Empresa.IdEmpresa).ToArray<long>();


            var empresas = dcAux.Empresa.Where(
            Helpers.NotContainsExpression<Entidades.Empresa, long>(emp => emp.IdEmpresa, empresasExistentes));
            txtFechaAlta.SelectedDate = DateTime.Now;


            gvSubContratistas.DataSource = from E in empresas
                                         orderby E.RazonSocial
                                         select E;

            gvSubContratistas.DataBind();

        }
    }
    protected void btnAsignar_Click(object sender, EventArgs e)
    {
        Entidades.EntidadesConosud dcAux = new Entidades.EntidadesConosud();

        foreach (GridDataItem item in gvSubContratistas.Items)
        {
            if ((item.FindControl("chkSeleccion") as CheckBox).Checked)
            { 
                
                long idContrado = long.Parse(Request.QueryString["IdContrato"].ToString());
                long idEmpresa = long.Parse(gvSubContratistas.Items[item.DataSetIndex].GetDataKeyValue("IdEmpresa").ToString());

                Entidades.Contrato contrato = (from CE in dcAux.Contrato
                                        where  CE.IdContrato ==idContrado
                                        select CE).First<Entidades.Contrato>();

                Entidades.Empresa empresa = (from E in dcAux.Empresa
                            where  E.IdEmpresa ==idEmpresa
                            select E).First<Entidades.Empresa>();


                ContratoEmpresas contemp = new ContratoEmpresas();
                contemp.EsContratista = false;
                contemp.Empresa =  empresa;
                contemp.Contrato = contrato;
                dcAux.AddObject("EntidadesConosud.ContratoEmpresas", contemp);


                DateTime FFin = DateTime.Now;
                if (! contrato.Prorroga.HasValue)
                {
                    FFin = contrato.FechaVencimiento.Value;
                }
                else
                {
                    FFin = contrato.Prorroga.Value;
                }

                Helpers.GenerarHojadeRuta(dcAux, txtFechaAlta.SelectedDate.Value, FFin, contemp);
                //Helpers.GenerarHojadeRuta(dcAux, contrato.FechaInicio.Value, FFin, contemp);

            }
                      
        }

        dcAux.SaveChanges();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "ocultar", "CloseWindows();", true);
    }
}

