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

public partial class AsignacionPaginaMenu : System.Web.UI.Page
{
    


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Add("MenuAsignado", null);
            long idRol = long.Parse(Request.QueryString["IdRol"].ToString());

            Entidades.EntidadesConosud dcAux = new Entidades.EntidadesConosud();


            Session["MenuAsignado"] = (from M in dcAux.SegRolMenu
                                       where M.SegRol.IdSegRol == idRol
                                       select M).ToList<Entidades.SegRolMenu>();


            gvSubContratistas.DataSource = from E in dcAux.SegMenu
                                           orderby E.Posicion
                                           select E;

            gvSubContratistas.DataBind();

            


        }
    }

    protected void btnAsignar_Click(object sender, EventArgs e)
    {
        Entidades.EntidadesConosud dcAux = new Entidades.EntidadesConosud();

        long idRol = long.Parse(Request.QueryString["IdRol"].ToString());

        Entidades.SegRol CurrentRol = (from M in dcAux.SegRol
                              where M.IdSegRol == idRol
                             select M).First<Entidades.SegRol>();

        List < Entidades.SegRolMenu> MenuEliminar = (from M in dcAux.SegRolMenu
                                   where M.SegRol.IdSegRol == idRol
                                   select M).ToList<Entidades.SegRolMenu>();

        foreach (Entidades.SegRolMenu item in MenuEliminar)
	    {
            dcAux.DeleteObject(item);
	    }
        dcAux.SaveChanges();
       


        foreach (GridDataItem item in gvSubContratistas.Items)
        {
            if ((item.FindControl("chkSeleccion") as CheckBox).Checked)
            {

                long idMenu = long.Parse(gvSubContratistas.Items[item.DataSetIndex].GetDataKeyValue("IdSegMenu").ToString());
                Entidades.SegMenu CurrentMenu = (from M in dcAux.SegMenu
                                                 where M.IdSegMenu == idMenu
                                                 select M).First<Entidades.SegMenu>();


                Entidades.SegRolMenu segMenu = new Entidades.SegRolMenu();
                segMenu.SegMenu = CurrentMenu;
                segMenu.SegRol = CurrentRol;
                segMenu.Creacion = (item.FindControl("chkCreacion") as CheckBox).Checked;
                segMenu.Modificacion = (item.FindControl("chkModificacion") as CheckBox).Checked;
                segMenu.Lectura = (item.FindControl("chkLectura") as CheckBox).Checked;

                               
                dcAux.AddObject("EntidadesConosud.SegRolMenu", segMenu);
            }

        }

        dcAux.SaveChanges();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "ocultar", "CloseWindows();", true);
    }
    protected void gvSubContratistas_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Entidades.SegMenu memu = (Entidades.SegMenu)e.Item.DataItem;

            if (!memu.PadreReference.IsLoaded) { memu.PadreReference.Load(); }
            if (memu.Padre == null)
            {
                e.Item.Font.Bold = true;
                e.Item.Font.Size = new FontUnit("13px");
                (e.Item.FindControl("chkCreacion") as CheckBox).Visible = false;
                (e.Item.FindControl("chkModificacion") as CheckBox).Visible = false;
                (e.Item.FindControl("chkLectura") as CheckBox).Visible = false;

                Label aa = new Label();
                aa.ID = "blankCreacion";
                aa.Text = "&nbsp;";
                e.Item.FindControl("chkCreacion").Parent.Controls.Add(aa);

                aa = new Label();
                aa.ID = "blankModificacion";
                aa.Text = "&nbsp;";
                e.Item.FindControl("chkModificacion").Parent.Controls.Add(aa);

                aa = new Label();
                aa.ID = "blankLectura";
                aa.Text = "&nbsp;";
                e.Item.FindControl("chkLectura").Parent.Controls.Add(aa);

            }
                 
      
            Entidades.SegRolMenu exist = (from M in (Session["MenuAsignado"] as List<Entidades.SegRolMenu>)
                         where M.SegMenu.IdSegMenu == memu.IdSegMenu
                         select M).FirstOrDefault();

            if (exist != null)
            {
                (e.Item.FindControl("chkSeleccion") as CheckBox).Checked = true;

                (e.Item.FindControl("chkCreacion") as CheckBox).Checked = exist.Creacion;
                (e.Item.FindControl("chkModificacion") as CheckBox).Checked = exist.Modificacion;
                (e.Item.FindControl("chkLectura") as CheckBox).Checked = exist.Lectura;

                if (exist.Creacion)
                {
                    (e.Item.FindControl("chkModificacion") as CheckBox).InputAttributes.Add("disabled", "disabled");
                    (e.Item.FindControl("chkLectura") as CheckBox).InputAttributes.Add("disabled", "disabled");
                }
                else if (exist.Modificacion)
                {
                    (e.Item.FindControl("chkLectura") as CheckBox).InputAttributes.Add("disabled", "disabled");
                }
            }
            else
            {
                (e.Item.FindControl("chkCreacion") as CheckBox).InputAttributes.Add("disabled", "disabled");
                (e.Item.FindControl("chkModificacion") as CheckBox).InputAttributes.Add("disabled", "disabled");
                (e.Item.FindControl("chkLectura") as CheckBox).InputAttributes.Add("disabled", "disabled");
            }
        }

    }
    protected void gvSubContratistas_ItemCreated(object sender, GridItemEventArgs e)
    {
        
    }
}
