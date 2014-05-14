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

public partial class ABMEmpresas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Session.Add("Empresas", null);
            CargarGrilla("");
        }

        (progressToolTip.FindControl("lbltituloCarga") as Label).Text = "Cargando datos para la edición de la empresa..";

    }
    protected void txtEmpresa_TextChanged(object sender, EventArgs e)
    {
        CargarGrilla(txtEmpresa.Text);
    }
    protected void gvEmpresas_ItemCommand(object source, GridCommandEventArgs e)
    {
        (progressToolTip.FindControl("lbltituloCarga") as Label).Text = "Actualizando datos de la empresa..";
        e.Canceled = true;
        gvEmpresas.DataSource = Session["Empresas"];
        gvEmpresas.DataBind();

        long id = long.Parse(gvEmpresas.Items[e.Item.DataSetIndex].GetDataKeyValue("IdEmpresa").ToString());

        Entidades.Empresa emp = (from E in (Session["Empresas"] as List<Entidades.Empresa>)
                       where E.IdEmpresa == id
                                 select E).Single<Entidades.Empresa>();

       

        txtCUIT.Text = emp.CUIT;
        txtDireccion.Text = emp.Direccion;
        txtEmail.Text = emp.CorreoElectronico;
        txtPEmergencia.Text = emp.PrestacionEmergencia;
        txtRazonSocial.Text = emp.RazonSocial;
        txtRTecnico.Text=emp.RepresentanteTecnico;
        txtTelefono.Text= emp.Telefono;
        
        if (emp.FechaAlta.HasValue)
            DpFechaAlta.SelectedDate = emp.FechaAlta.Value;

        HiddenId.Value = emp.IdEmpresa.ToString();
        toolTipEdicion.Title = "Edición Empresa: " + emp.RazonSocial;
        upToolTip.Update();
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();
        Entidades.Empresa emp = null;

        if (int.Parse(HiddenId.Value) > 0)
        {
            long id = long.Parse(HiddenId.Value);
            emp = (from E in dc.Empresa
                   where E.IdEmpresa == id
                   select E).First<Entidades.Empresa>();
        }
        else
        {
            emp = new Entidades.Empresa();

        }

        emp.CUIT = txtCUIT.Text;
        emp.Direccion = txtDireccion.Text;
        emp.CorreoElectronico = txtEmail.Text;
        emp.PrestacionEmergencia = txtPEmergencia.Text;
        emp.RazonSocial = txtRazonSocial.Text;
        emp.RepresentanteTecnico = txtRTecnico.Text;
        emp.Telefono = txtTelefono.Text;
        if (DpFechaAlta.SelectedDate.HasValue)
            emp.FechaAlta = DpFechaAlta.SelectedDate;

        if (int.Parse(HiddenId.Value) == 0)
        {
            dc.AddToEmpresa(emp);
        }

        dc.SaveChanges();
        HiddenId.Value = "0";
        CargarGrilla(txtEmpresa.Text);
        ScriptManager.RegisterClientScriptBlock(upToolTip, typeof(UpdatePanel), "show", "HideTooTil();", true);
        upToolTip.Update();
        upGrilla.Update();
    }

    private void CargarGrilla(string filtro)
    {
        EntidadesConosud dc = new EntidadesConosud();
        List<Entidades.Empresa> ListaEmpresas;

        if (filtro.Trim().Length > 0)
        {
            ListaEmpresas = (from E in dc.Empresa
                             where E.RazonSocial.StartsWith(filtro)
                             select E).Take(10).ToList<Entidades.Empresa>();
        }
        else
        {
            ListaEmpresas = (from E in dc.Empresa
                             select E).Take(10).ToList<Entidades.Empresa>();
        }

        Session["Empresas"] = ListaEmpresas;
        gvEmpresas.DataSource = ListaEmpresas;
        gvEmpresas.DataBind();
        upGrilla.Update();
    
    }
}
