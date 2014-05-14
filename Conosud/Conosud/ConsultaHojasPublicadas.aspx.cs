using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using Entidades;

public partial class ConsultaHojasPublicadas : System.Web.UI.Page
{
    private EntidadesConosud _dc = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadEmpresas();
        }

    }

    private void LoadEmpresas()
    {

        cboEmpresas.DataTextField = "RazonSocial";
        cboEmpresas.DataValueField = "IdEmpresa";
        cboEmpresas.DataSource = Helpers.GetEmpresas(long.Parse(Session["idusu"].ToString()));
        cboEmpresas.DataBind();

        if (cboEmpresas.Items.Count > 1)
            cboEmpresas.Items.Insert(0, new RadComboBoxItem("- Seleccione una Empresa -"));
        else
            LoadContratos(int.Parse(cboEmpresas.Items[0].Value));

    }

    private void LoadContratos(int id)
    {

        cboContratos.DataTextField = "Codigo";
        cboContratos.DataValueField = "IdContrato";
        cboContratos.DataSource = Helpers.GetContratos(id);
        cboContratos.DataBind();

        cboContratos.Items.Insert(0, new RadComboBoxItem("- Seleccione un Contrato -"));

    }

    private void LoadContratistas(int id)
    {
        cboContratistas.DataTextField = "RazonSocial";
        cboContratistas.DataValueField = "IdContratoEmpresas";
        cboContratistas.DataSource = Helpers.GetContratistas(id);
        cboContratistas.DataBind();

        cboContratistas.Items.Insert(0, new RadComboBoxItem("- Seleccione un Contratista -"));
    }

    protected void cboEmpresas_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadEmpresas();
    }

    protected void cboContratos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadContratos(int.Parse(e.Text));
    }

    protected void cboContratistas_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadContratistas(int.Parse(e.Text));
    }

    protected void cboContratistas_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();
        long id = long.Parse(cboContratistas.SelectedValue);

        List<Entidades.CabeceraHojasDeRuta> cabs = (from C in dc.CabeceraHojasDeRuta
                                            .Include("HojasDeRuta").Include("HojasDeRuta.Plantilla").Include("HojasDeRuta.Plantilla.CategoriasItems").Include("ContratoEmpresas").Include("ContratoEmpresas.Contrato")
                                            .Include("ContratoEmpresas.Empresa")
                                            .Include("ContratoEmpresas.CabeceraHojasDeRuta")
                                            .Include("ContratoEmpresas.Contrato")
                    where C.ContratoEmpresas.IdContratoEmpresas == id && C.Publicar.Value
                    orderby C.Periodo ascending
                    select C).ToList<Entidades.CabeceraHojasDeRuta>();


        gvCabeceras.DataSource = cabs;
        gvCabeceras.DataBind();
        upResultado.Update();
    }

    protected void gvCabeceras_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            Entidades.CabeceraHojasDeRuta CurrentCabecera = (Entidades.CabeceraHojasDeRuta)e.Item.DataItem;
             //.Aprobada != "Aprobada" || CurrentCabecera.Aprobada != "Aprobada SL"
            if (CurrentCabecera.FechaAprobacion == null)
            {

                int SinDoc = (from H in CurrentCabecera.HojasDeRuta
                                       where H.DocComentario != null
                                       && H.DocComentario.Trim() != ""
                                       select H).Count();

                if (SinDoc == 0)
                {
                    /// Si no tiene comentarios de pendientes y no tiene todos los items aprobados,
                    /// entonces no se ha aprobado porque alguna de las sub contratistas 
                    /// no esta aprobada y por lo tanto no puede aprobarce esta hoja.                    
                    int ItemsAprobados = CurrentCabecera.HojasDeRuta.Where(w => w.HojaAprobado.HasValue && w.HojaAprobado.Value).Count();
                    if (ItemsAprobados == 0)
                        (e.Item.FindControl("lblMotivo") as Label).Text = "No Presentó Documentación";
                    else
                        (e.Item.FindControl("lblMotivo") as Label).Text = "Por pendientes de Subcontratista";

                }


                int CantComentarios = (from H in CurrentCabecera.HojasDeRuta
                                       where H.HojaComentario != null
                                       && H.HojaComentario.Trim() != ""
                                       select H).Count();
                

                if (CantComentarios > 0)
                {
                    (e.Item.FindControl("lblMotivo") as Label).Text = "Con Pendientes";
                }
            }


        }

    }
}
