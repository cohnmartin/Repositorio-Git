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

public partial class ABMLegajos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Session.Add("Legajos", null);
            CargarGrilla("");
            CargarCombos();
        }

        (UpdateProgress2.FindControl("lbltituloCarga") as Label).Text = "Cargando datos para la edición del legajo..";
    }
    protected void txtApellidoLegajo_TextChanged(object sender, EventArgs e)
    {
        CargarGrilla(txtApellidoLegajo.Text);
    }
    protected void gvLegajos_ItemCommand(object source, GridCommandEventArgs e)
    {
        (UpdateProgress2.FindControl("lbltituloCarga") as Label).Text = "Actualizando datos del legajo..";

        e.Canceled = true;
        gvLegajos.DataSource = Session["Legajos"];
        gvLegajos.DataBind();

        long id = long.Parse(gvLegajos.Items[e.Item.DataSetIndex].GetDataKeyValue("IdLegajos").ToString());

        Entidades.Legajos leg = (from E in (Session["Legajos"] as List<Entidades.Legajos>)
                                 where E.IdLegajos == id
                                 select E).First<Entidades.Legajos>();

        

        txtApellido.Text = leg.Apellido;
        txtNombre.Text  = leg.Nombre;
        txtNroDocumento.Text =  leg.NroDoc;
        txtFechaNacimiento.SelectedDate = leg.FechaNacimiento;
        txtCuil.Text = leg.CUIL;
        txtDireccion.Text = leg.Direccion;
        txtCodigoPostal.Text = leg.CodigoPostal;
        txtTelefono.Text = leg.TelefonoFijo;
        txtEmail.Text = leg.CorreoElectronico;

        if (leg.objEstadoCivil != null)
            cboEstadoCivil.Items.FindItemByValue(leg.objEstadoCivil.IdClasificacion.ToString()).Selected = true;

        if (leg.objTipoDocumento != null)
            cboTipoDoc.Items.FindItemByValue(leg.objTipoDocumento.IdClasificacion.ToString()).Selected = true;

        if (leg.objProvincia != null)
            cboProvincia.Items.FindItemByValue(leg.objProvincia.IdClasificacion.ToString()).Selected = true;

        if (leg.objNacionalidad != null)
            cboNacionalidad.Items.FindItemByValue(leg.objNacionalidad.IdClasificacion.ToString()).Selected = true;

      

        HiddenId.Value = leg.IdLegajos.ToString();
        toolTipEdicion.Title = "Edición Legajo: " + leg.Apellido + ", " + leg.Nombre;
        upToolTip.Update();
    }

  
    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();
        Entidades.Legajos leg = null;

        if (int.Parse(HiddenId.Value) > 0)
        {
            long id = long.Parse(HiddenId.Value);
            leg = (from E in dc.Legajos
                   where E.IdLegajos == id
                   select E).First<Entidades.Legajos>();
        }
        else
        {
            leg = new Entidades.Legajos();

        }

        leg.Apellido = txtApellido.Text ;
        leg.Nombre = txtNombre.Text;
        leg.NroDoc = txtNroDocumento.Text;
        leg.CUIL = txtCuil.Text  ;
        leg.Direccion = txtDireccion.Text;
        leg.CodigoPostal = txtCodigoPostal.Text;
        leg.TelefonoFijo = txtTelefono.Text;
        leg.CorreoElectronico = txtEmail.Text  ;

        if (txtFechaNacimiento.SelectedDate.HasValue)
            leg.FechaNacimiento = txtFechaNacimiento.SelectedDate;

        long idCla = long.Parse(cboEstadoCivil.SelectedValue);
        leg.objEstadoCivil = (from C in dc.Clasificacion
                             where C.IdClasificacion == idCla
                                  select C).First<Entidades.Clasificacion>();


        idCla = long.Parse(cboTipoDoc.SelectedValue);
        leg.objTipoDocumento = (from C in dc.Clasificacion
                             where C.IdClasificacion == idCla
                                    select C).First<Entidades.Clasificacion>();

        idCla = long.Parse(cboProvincia.SelectedValue);
        leg.objProvincia = (from C in dc.Clasificacion
                             where C.IdClasificacion == idCla
                                select C).First<Entidades.Clasificacion>();

        idCla = long.Parse(cboNacionalidad.SelectedValue);
        leg.objNacionalidad = (from C in dc.Clasificacion
                             where C.IdClasificacion == idCla
                               select C).First<Entidades.Clasificacion>();


        if (int.Parse(HiddenId.Value) == 0)
        {
            dc.AddToLegajos(leg);
        }

        dc.SaveChanges();


        CargarGrilla(txtApellidoLegajo.Text);
        HiddenId.Value = "0";
        ScriptManager.RegisterClientScriptBlock(upToolTip, typeof(UpdatePanel), "show", "HideTooTil();", true);
        upToolTip.Update();
        upGrilla.Update();
    }

    private void CargarGrilla(string filtro)
    {
        EntidadesConosud dc = new EntidadesConosud();
        List<Entidades.Legajos> ListaLegajos;

        if (filtro.Trim().Length > 0)
        {
            ListaLegajos = (from E in dc.Legajos.Include("objTipoDocumento")
                            .Include("objEstadoCivil").Include("objProvincia").Include("objNacionalidad")
                             where E.Apellido.StartsWith(filtro)
                             select E).Take(10).ToList<Entidades.Legajos>();
        }
        else
        {
            ListaLegajos = (from E in dc.Legajos.Include("objTipoDocumento")
                            .Include("objEstadoCivil").Include("objProvincia").Include("objNacionalidad")
                             select E).Take(10).ToList<Entidades.Legajos>();
        }

        Session["Legajos"] = ListaLegajos;
        gvLegajos.DataSource = ListaLegajos;
        gvLegajos.DataBind();
        upGrilla.Update();

    }

    private void CargarCombos()
    {
        EntidadesConosud dc = new EntidadesConosud();
        List<Entidades.Clasificacion> listaClasificacion = (from E in dc.Clasificacion
                                                            where E.Tipo == "Tipo Documento"    
                        select E).ToList<Entidades.Clasificacion>();

        cboTipoDoc.DataTextField = "Descripcion";
        cboTipoDoc.DataValueField = "IdClasificacion";
        cboTipoDoc.DataSource = listaClasificacion;
        cboTipoDoc.DataBind();


        listaClasificacion = (from E in dc.Clasificacion
                              where E.Tipo == "Estado Civil"
            select E).ToList<Entidades.Clasificacion>();

        cboEstadoCivil.DataTextField = "Descripcion";
        cboEstadoCivil.DataValueField = "IdClasificacion";
        cboEstadoCivil.DataSource = listaClasificacion;
        cboEstadoCivil.DataBind();

        listaClasificacion = (from E in dc.Clasificacion
                              where E.Tipo == "Provincia"
                              select E).ToList<Entidades.Clasificacion>();

        cboProvincia.DataTextField = "Descripcion";
        cboProvincia.DataValueField = "IdClasificacion";
        cboProvincia.DataSource = listaClasificacion;
        cboProvincia.DataBind();

        listaClasificacion = (from E in dc.Clasificacion
                              where E.Tipo == "Nacionalidad"
                              select E).ToList<Entidades.Clasificacion>();

        cboNacionalidad.DataTextField = "Descripcion";
        cboNacionalidad.DataValueField = "IdClasificacion";
        cboNacionalidad.DataSource = listaClasificacion;
        cboNacionalidad.DataBind();


        listaClasificacion = (from E in dc.Clasificacion
                              where E.Tipo == "Sexo"
                              select E).ToList<Entidades.Clasificacion>();

        cboSexo.DataTextField = "Descripcion";
        cboSexo.DataValueField = "IdClasificacion";
        cboSexo.DataSource = listaClasificacion;
        cboSexo.DataBind();

    }
}
