using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Web.SessionState;
using System.Linq;
using System.Collections.Generic;

public partial class DefaultMasterPage : System.Web.UI.MasterPage
{
    public void OcultarEncabezado()
    {
        trImagenEnc.Visible = false;
        trMenuEnc.Visible = false;
    
    }

    public void OcultarMenu()
    {
        //lbCambiarClave.Visible = false;
        trMenuEnc.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (this.Session["idusu"] == null)
                Response.Redirect("~/Login.aspx");

            if (!this.IsPostBack)
            {
                this.lblNombreUsu.Text = Convert.ToString(this.Session["nombreusu"]);

                Entidades.EntidadesConosud dcAux = new Entidades.EntidadesConosud();
                long IdSegUsuario = (long)this.Session["idusu"];
                Entidades.SegUsuario usu = (from us in dcAux.SegUsuario
                                            .Include("SegUsuarioRol.SegRol.SegRolMenu.SegMenu.Padre")
                                            where us.IdSegUsuario == IdSegUsuario
                                            select us).First<Entidades.SegUsuario>();

                List<Entidades.SegMenu> menues = new List<Entidades.SegMenu>();

                foreach (Entidades.SegUsuarioRol UsuRol in usu.SegUsuarioRol)
                {
                    foreach (Entidades.SegRolMenu confseg in UsuRol.SegRol.SegRolMenu)
                    {
                        if (menues.FindAll(d => d.IdSegMenu == confseg.SegMenu.IdSegMenu).Count == 0)
                        {
                            menues.Add(confseg.SegMenu);
                        }
                    }
                }

                menues = (from M in menues
                         orderby M.Posicion
                         select M).ToList<Entidades.SegMenu>();

                if (menues.Count > 0)
                {
                    DataTable dt = Helpers.LINQToDataTable<Entidades.SegMenu>(menues);

                    RadMenu1.DataFieldID = "IdSegMenu";
                    RadMenu1.DataFieldParentID = "PadreId";
                    RadMenu1.DataTextField = "Descripcion";
                    RadMenu1.DataNavigateUrlField = "Url";

                    DataRow drSalir = dt.NewRow();
                    drSalir["Url"] = "Login.aspx";
                    drSalir["PadreId"] = System.DBNull.Value;
                    drSalir["Descripcion"] = "Salir Sistema";
                    drSalir["IdSegMenu"] = "10000";

                    dt.Rows.Add(drSalir);
                    RadMenu1.DataSource = dt;
                    RadMenu1.DataBind();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }

    protected void RadMenu1_ItemDataBound(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    {
        //if (e.Item.NavigateUrl.Length > 0)
        //{
        //    if (e.Item.NavigateUrl.Contains("?"))
        //        e.Item.NavigateUrl = e.Item.NavigateUrl + "&IdUnicoSesion=" + DateTime.Now.Ticks.ToString();
        //    else
        //        e.Item.NavigateUrl = e.Item.NavigateUrl + "?IdUnicoSesion=" + DateTime.Now.Ticks.ToString();
        //}
    }


}
