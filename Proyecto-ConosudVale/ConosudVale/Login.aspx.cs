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
        if (!this.IsPostBack)
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

        List<Entidades.SegUsuario> usuarios = (from u in dc.SegUsuario.Include("SegUsuarioRol.SegRol")
                                               where u.Login == this.UserName.Text && u.Password == this.Password.Text
                                               select u).ToList();

        if (usuarios.Count > 0)
        {
            if (!usuarios.First().EmpresaReference.IsLoaded) { usuarios.First().EmpresaReference.Load(); }
            if (!usuarios.First().SegUsuarioRol.IsLoaded) { usuarios.First().SegUsuarioRol.Load(); }


            if (usuarios.First().SegUsuarioRol.Count > 0)
            {

                this.Session["idusu"] = usuarios.First().IdSegUsuario;
                this.Session["idusuario"] = usuarios.First().IdSegUsuario;
                this.Session["nombreusu"] = usuarios.First().Login;
                this.Session["usuario"] = usuarios.First();

                if (usuarios.First().Empresa != null)
                {
                    this.Session["TipoUsuario"] = "Cliente";
                    this.Session["IdEmpresaContratista"] = usuarios.First().IdEmpresa.Value;
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

    //private void ProcesoCorreccionLegajos()
    //{
    //    //select count(idlegajos) ,idlegajos,idcabecerhojaRuta from dbo.ContEmpLegajos where idcontratoempresas in
    //    //    (select idcontratoempresas from dbo.ContratoEmpresas where idcontrato in
    //    //    (select idcontrato from contrato))
    //    //group by idlegajos,idcabecerhojaRuta
    //    //having count(idlegajos)>1

    //    EntidadesConosud dc = new EntidadesConosud();
    //    var legajosAgrupados = (from l in dc.ContEmpLegajos
    //                            where l.IdLegajos != null
    //                            group l by new { l.IdLegajos, l.IdCabecerHojaRuta } into g
    //                            where g.Count() > 1
    //                            select new
    //                            {
    //                                CantidadAsociacion = g.Count(),
    //                                LegajosContrato = g,
    //                                IdLegajo = g.Key.IdLegajos,
    //                                IdCabecerHojaRuta = g.Key.IdCabecerHojaRuta

    //                            }).ToList();


    //    List<ContEmpLegajos> asociacionesEliminar = new List<ContEmpLegajos>();
    //    foreach (var item in legajosAgrupados)
    //    {
    //        List<ContEmpLegajos> asociaciones = item.LegajosContrato.ToList();
    //        bool poseeBaja = asociaciones.Any(w => w.FechaTramiteBaja != null);

    //        if (poseeBaja)
    //        {
    //            foreach (ContEmpLegajos aso in asociaciones.Where(w => w.FechaTramiteBaja == null))
    //            {
    //                asociacionesEliminar.Add(aso);
    //            }
    //        }
    //        else
    //        {
    //            foreach (ContEmpLegajos aso in asociaciones.Take(asociaciones.Count - 1))
    //            {
    //                asociacionesEliminar.Add(aso);
    //            }
    //        }

    //    }

    //    foreach (var item in asociacionesEliminar)
    //    {
    //        dc.DeleteObject(item);
    //    }

    //    dc.SaveChanges();
    //}
}
