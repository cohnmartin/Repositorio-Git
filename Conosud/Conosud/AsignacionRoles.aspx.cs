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

public partial class AsignacionRoles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Add("RolesAsignados", null);

            long idUsuario = long.Parse(Request.QueryString["IdUsuario"].ToString());

            Entidades.EntidadesConosud dcAux = new Entidades.EntidadesConosud();


            Session["RolesAsignados"] = (from U in dcAux.SegUsuarioRol
                                         where U.SegUsuario.IdSegUsuario == idUsuario
                                       select U).ToList<Entidades.SegUsuarioRol>();


            gvRoles.DataSource = from E in dcAux.SegRol
                                           orderby E.IdSegRol
                                           select E;

            gvRoles.DataBind();




        }
    }

    protected void btnAsignar_Click(object sender, EventArgs e)
    {
        Entidades.EntidadesConosud dcAux = new Entidades.EntidadesConosud();

        long idUsuario = long.Parse(Request.QueryString["IdUsuario"].ToString());

        Entidades.SegUsuario CurrentUsuario = (from M in dcAux.SegUsuario
                                       where M.IdSegUsuario == idUsuario
                                       select M).First<Entidades.SegUsuario>();


        List<Entidades.SegUsuarioRol> rolesEliminar = (from M in dcAux.SegUsuarioRol
                                                    where M.SegUsuario.IdSegUsuario == idUsuario
                                                   select M).ToList<Entidades.SegUsuarioRol>();

        foreach (Entidades.SegUsuarioRol item in rolesEliminar)
        {
            dcAux.DeleteObject(item);
        }
        dcAux.SaveChanges();



        foreach (GridDataItem item in gvRoles.Items)
        {
            if ((item.FindControl("chkSeleccion") as CheckBox).Checked)
            {

                long idRol = long.Parse(gvRoles.Items[item.DataSetIndex].GetDataKeyValue("IdSegRol").ToString());
                
                Entidades.SegRol CurrentRol = (from M in dcAux.SegRol
                                               where M.IdSegRol == idRol
                                               select M).First<Entidades.SegRol>();


                Entidades.SegUsuarioRol segRol = new Entidades.SegUsuarioRol();
                segRol.SegRol = CurrentRol;
                segRol.SegUsuario = CurrentUsuario;


                dcAux.AddObject("EntidadesConosud.SegUsuarioRol", segRol);
            }

        }

        dcAux.SaveChanges();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "ocultar", "CloseWindows();", true);
    }
 
    protected void gvRoles_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Entidades.SegRol rol = (Entidades.SegRol)e.Item.DataItem;

            int exist = (from M in (Session["RolesAsignados"] as List<Entidades.SegUsuarioRol>)
                         where M.SegRol.IdSegRol == rol.IdSegRol
                         select M).Count();

            if (exist > 0)
            {
                (e.Item.FindControl("chkSeleccion") as CheckBox).Checked = true;
            }
        }

    }
   
}
