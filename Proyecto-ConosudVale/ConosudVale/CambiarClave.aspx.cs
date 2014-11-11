using System;
using System.Collections;
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
using Entidades;
using System.Collections.Generic;

public partial class CambiarClave : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            (Page.Master as DefaultMasterPage).OcultarMenu();
        }
    }

    protected void btnCambiar_Click(object sender, EventArgs e)
    {
        if (this.txtConfClave.Text != this.txtNuevaClave.Text)
        {
            this.Label4.Text = "No coincide confirmacion de la clave nueva";
            this.txtConfClave.Focus();
            return;
        }

        EntidadesConosud dc = new EntidadesConosud();
        long idusu = Convert.ToInt64(this.Session["idusu"]);

        Entidades.SegUsuario usuario = (from u in dc.SegUsuario
                                               where u.IdSegUsuario == idusu
                                               && u.Password == this.txtClave.Text
                                        select u).FirstOrDefault<Entidades.SegUsuario>();

        //ConosudDataContext dc = new ConosudDataContext();
        //long idusu = Convert.ToInt64(this.Session["idusu"]);
        //SegUsuario usuario = (from u in dc.SegUsuarios
        //                                   where u.IdSegUsuario == idusu && u.Password == this.txtClave.Text
        //                                   select u).First();

        if (usuario == null)
        {
            this.Label4.Text = "Clave Actual Incorrecta";
            this.txtConfClave.Focus();
        }
        else
        {
            usuario.Password = this.txtNuevaClave.Text;
            dc.SaveChanges();
            Response.Redirect("~/Default.aspx");
            this.Label4.Text = string.Empty;
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
        this.Label4.Text = string.Empty;

    }
}
