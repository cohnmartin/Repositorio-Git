using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using System.Collections;

public partial class ABMUsuarios2 : System.Web.UI.Page
{
    private EntidadesConosud _Contexto;
    public EntidadesConosud Contexto
    {
        //get
        //{
        //    if (Session["Contexto"] != null)
        //        return (EntidadesConosud)Session["Contexto"];
        //    else
        //    {
        //        Session["Contexto"] = new EntidadesConosud();
        //        return (EntidadesConosud)Session["Contexto"];
        //    }
        //}

        get
        {

            if (_Contexto == null)
            {
                _Contexto = new EntidadesConosud();
                return _Contexto;
            }
            else
                return _Contexto;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.grillaUsuarios.Rebind();
        }
    }


    protected void grillaUsuarios_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        this.grillaUsuarios.DataSource = (from c in Contexto.SegUsuario select c).ToList();
    }
    protected void grillaUsuarios_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            if (e.Item.OwnerTableView.IsItemInserted)
            {
                var emp = (from c in Contexto.Empresa select new { c.IdEmpresa, c.RazonSocial }).Distinct().ToList().OrderBy(w => w.RazonSocial);

                (e.Item.FindControl("cboEmpresa") as RadComboBox).DataSource = emp;
                (e.Item.FindControl("cboEmpresa") as RadComboBox).DataBind();
            }
        }

        if (e.Item.ItemType == GridItemType.EditFormItem)
        {
            if (e.Item.FindControl("cboEmpresa") != null)
            {
                if (e.Item.DataItem is SegUsuario)
                {
                    SegUsuario clasif = (SegUsuario)e.Item.DataItem;
                    var emp = (from c in Contexto.Empresa select new { c.IdEmpresa, c.RazonSocial }).Distinct().ToList().OrderBy(w => w.RazonSocial);

                    (e.Item.FindControl("cboEmpresa") as RadComboBox).DataSource = emp;
                    (e.Item.FindControl("cboEmpresa") as RadComboBox).DataBind();

                    if (clasif.IdEmpresa != null)
                        (e.Item.FindControl("cboEmpresa") as RadComboBox).SelectedValue = clasif.IdEmpresa.ToString();

                }
            }
        }
    }
    protected void grillaUsuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["idsel"] = grillaUsuarios.SelectedValue;
    }
    protected void grillaUsuarios_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.PerformInsertCommandName
            || e.CommandName == RadGrid.UpdateCommandName)
        {
            if (e.Item is GridEditFormInsertItem)
            {
                GridEditFormInsertItem editedItem = e.Item as GridEditFormInsertItem;
                grillaUsuarios.MasterTableView.IsItemInserted = false;
                InsertUsuario(editedItem);
            }
            else if (e.Item is GridEditFormItem)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                UpdateUsuario(editedItem);
                grillaUsuarios.MasterTableView.ClearEditItems();
            }
        }
        else if (e.CommandName == RadGrid.InitInsertCommandName)
        {
            grillaUsuarios.MasterTableView.ClearEditItems();
        }
        else if (e.CommandName == "DeleteSelected")
        {
            DeleteUsuario();
        }


    }
    protected void grillaUsuarios_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.GestionUsuarios, idUsuario);


        LinkButton btnAccion = (LinkButton)grillaUsuarios.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEdit");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)grillaUsuarios.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
        btnAccion.Visible = PermisosPagina.Creacion;

        btnAccion = (LinkButton)grillaUsuarios.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDelete");
        btnAccion.Visible = PermisosPagina.Creacion;

        if (!PermisosPagina.Modificacion)
        {
            grillaUsuarios.MasterTableView.GetColumn("MenuColumn").Visible = false;
        }

    }

    protected void InsertUsuario(GridEditableItem editedItem)
    {
        long idPlanilla = Convert.ToInt64(ViewState["idsel"]);

        #region Recupero los  Datos Ingresado por el usuario
        GridEditManager editMan = editedItem.EditManager;
        string Login_Ingresado = ((TextBox)editedItem.FindControl("LoginTextBox")).Text;
        string password_Ingresado = ((TextBox)editedItem.FindControl("PasswordTextBox")).Text;
        string IdEmpres_Ingresado = ((RadComboBox)editedItem.FindControl("cboEmpresa")).SelectedValue;
        #endregion

        try
        {
            SegUsuario _ItemUsuario = new SegUsuario();
            _ItemUsuario.Login = Login_Ingresado;
            _ItemUsuario.Password = password_Ingresado;

            if (IdEmpres_Ingresado.Trim() != "")
                _ItemUsuario.IdEmpresa = long.Parse(IdEmpres_Ingresado);

            Contexto.AddToSegUsuario(_ItemUsuario);
            Contexto.SaveChanges();
            this.grillaUsuarios.Rebind();
        }
        catch (Exception e)
        {
            ScriptManager.RegisterStartupScript(UpdPnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);

        }

    }
    protected void UpdateUsuario(GridEditableItem editedItem)
    {
        long idUsuario = Convert.ToInt64(ViewState["idsel"]);


        SegUsuario _ItemUsuario = (from c in Contexto.SegUsuario
                                   where c.IdSegUsuario == idUsuario
                                   select c).FirstOrDefault();

        if (_ItemUsuario != null)
        {

            #region Recupero los  Datos Ingresado por el usuario
            GridEditManager editMan = editedItem.EditManager;
            string Login_Ingresado = ((TextBox)editedItem.FindControl("LoginTextBox")).Text;
            string password_Ingresado = ((TextBox)editedItem.FindControl("PasswordTextBox")).Text;
            string IdEmpres_Ingresado = ((RadComboBox)editedItem.FindControl("cboEmpresa")).SelectedValue;
            #endregion

            try
            {
                _ItemUsuario.Login = Login_Ingresado;
                _ItemUsuario.Password = password_Ingresado;

                if (IdEmpres_Ingresado.Trim() != "")
                    _ItemUsuario.IdEmpresa = long.Parse(IdEmpres_Ingresado);
                else
                    _ItemUsuario.IdEmpresa = null;

                Contexto.SaveChanges();
                this.grillaUsuarios.Rebind();
            }
            catch (Exception e)
            {
                ScriptManager.RegisterStartupScript(UpdPnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);

            }
        }
    }
    protected void DeleteUsuario()
    {
        long idUsuario = Convert.ToInt64(ViewState["idsel"]);


        SegUsuario _ItemUsuario = (from c in Contexto.SegUsuario
                                   where c.IdSegUsuario == idUsuario
                                   select c).FirstOrDefault();

        if (_ItemUsuario != null)
        {

            try
            {
                List<SegUsuarioRol> roles = _ItemUsuario.SegUsuarioRol.ToList();
                foreach (var item in roles)
                {
                    Contexto.DeleteObject(item);
                }

                Contexto.DeleteObject(_ItemUsuario);
                Contexto.SaveChanges();
                this.grillaUsuarios.Rebind();
            }
            catch (Exception e)
            {
                ScriptManager.RegisterStartupScript(UpdPnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);

            }
        }
    }
}
