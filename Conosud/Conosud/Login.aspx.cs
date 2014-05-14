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
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Entidades;
//using PdfLibrary;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (! this.IsPostBack)
        {
            //// Put user code to initialize the page here
            Session.Abandon();
            Application.Clear();
            FormsAuthentication.SignOut();
            Session.Timeout = 60;

           

        }
    }

    protected void LoginButton_Click(object sender, EventArgs e)
    {

        EntidadesConosud dc = new EntidadesConosud();

       


        List<Entidades.SegUsuario> usuarios = (from u in dc.SegUsuario
                            where u.Login == this.UserName.Text && u.Password == this.Password.Text
                            select u).ToList();

        if (usuarios.Count > 0)
        {
            //if (!usuarios.First().EmpresaReference.IsLoaded){usuarios.First().EmpresaReference.Load();}
            //if (!usuarios.First().SegUsuarioRol.IsLoaded){usuarios.First().SegUsuarioRol.Load();}
            //if (!usuarios.First().SegUsuarioRol.IsLoaded){usuarios.First().SegUsuarioRol.Load();}


            if (usuarios.First().SegUsuarioRol.Count > 0)
            {

                this.Session["idusu"] = usuarios.First().IdSegUsuario;
                this.Session["idusuario"] = usuarios.First().IdSegUsuario;
                this.Session["nombreusu"] = usuarios.First().Login;
                this.Session["usuario"] = usuarios.First();

                if (usuarios.First().Empresa != null)
                {
                    this.Session["TipoUsuario"] = "Cliente";
                    this.Session["IdEmpresaContratista"] = usuarios.First().Empresa.IdEmpresa;
                }
                else
                {
                    this.Session["TipoUsuario"] = "NoCliente";
                    this.Session["IdEmpresaContratista"] = null;
                }

                Response.Redirect("~/Default.aspx");
                this.FailureText.Text = string.Empty;

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "alertesinpermisos", "ShowUsuarioSinPermisos();", true);
            }

        }
        else
        {
            this.FailureText.Text = "Usuario o clave incorrectos!";
            this.UserName.Text = string.Empty;
        }

    }
}
