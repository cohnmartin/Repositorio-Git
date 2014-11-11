using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Entidades;
using System.Data.Objects;
using System.Data;

public partial class ABMClasificaciones : System.Web.UI.Page
{
    private EntidadesConosud _Contexto;
    public object Contexto
    {
        //get
        //{
        //    if (Session["Contexto"] != null)
        //        return (EntidadesConosud)Session["Contexto"];
        //    else
        //    {
               

        //        //Session["Contexto"] = new EntidadesConosud();
        //        //return (EntidadesConosud)Session["Contexto"];
            
        //    }
        //}

        get {

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
            cboTipoParametro.DataSource = (from c in (Contexto as EntidadesConosud).Clasificacion
                                           select new { c.Tipo }).Distinct().ToList();
            cboTipoParametro.DataBind();

            this.RadGrid1.Rebind();
        }
    }

    protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        this.RadGrid1.DataSource = (from c in (Contexto as EntidadesConosud).Clasificacion where c.Tipo.StartsWith(cboTipoParametro.SelectedValue) select c).ToList();
    }
    protected void RadGrid1_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Varios, idUsuario);


        LinkButton btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEdit");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
        btnAccion.Visible = PermisosPagina.Creacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDelete");
        btnAccion.Visible = PermisosPagina.Creacion;
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            if (e.Item.OwnerTableView.IsItemInserted)
            {
                (e.Item.FindControl("cboTipo") as RadComboBox).DataSource = (from c in (Contexto as EntidadesConosud).Clasificacion select new { c.Tipo }).Distinct().ToList();
                (e.Item.FindControl("cboTipo") as RadComboBox).DataBind();
            }
        }

        if (e.Item.ItemType == GridItemType.EditFormItem)
        {
            if (e.Item.FindControl("cboTipo") != null)
            {
                if (e.Item.DataItem is Clasificacion)
                {
                    Clasificacion clasif = (Clasificacion)e.Item.DataItem;

                    (e.Item.FindControl("cboTipo") as RadComboBox).DataSource = (from c in (Contexto as EntidadesConosud).Clasificacion select new { c.Tipo }).Distinct().ToList();
                    (e.Item.FindControl("cboTipo") as RadComboBox).DataBind();

                    (e.Item.FindControl("cboTipo") as RadComboBox).SelectedValue = clasif.Tipo;

                }
            }
        }
    }
    protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["idsel"] = RadGrid1.SelectedValue;
    }
    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.PerformInsertCommandName
            || e.CommandName == RadGrid.UpdateCommandName)
        {
            if (e.Item is GridEditFormInsertItem)
            {
                GridEditFormInsertItem editedItem = e.Item as GridEditFormInsertItem;
                RadGrid1.MasterTableView.IsItemInserted = false;
                InsertClasificacion(editedItem);
            }
            else if (e.Item is GridEditFormItem)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                UpdateClasificacion(editedItem);
                RadGrid1.MasterTableView.ClearEditItems();
            }
        }
        else if (e.CommandName == RadGrid.InitInsertCommandName)
        {
            RadGrid1.MasterTableView.ClearEditItems();
        }
        else if (e.CommandName == "DeleteSelected")
        {
            DeleteClasificacion();
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        ViewState["idsel"] = null;
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        this.RadGrid1.Rebind();
        this.updpnlGrilla.Update();
    }

    protected void UpdateClasificacion(GridEditableItem editedItem)
    {
        long idClasificacion = Convert.ToInt64(ViewState["idsel"]);


        Clasificacion _clasif = (from c in (Contexto as EntidadesConosud).Clasificacion
                                 where c.IdClasificacion == idClasificacion
                                 select c).FirstOrDefault();

        if (_clasif != null)
        {

            #region Recupero los  Datos Ingresado por el usuario
            GridEditManager editMan = editedItem.EditManager;
            string codigo_Ingresado = (editMan.GetColumnEditor("Codigo") as GridTextBoxColumnEditor).Text;
            string Descripcion_Ingresado = (editMan.GetColumnEditor("Descripcion") as GridTextBoxColumnEditor).Text;
            string Tipo_Ingresado = ((RadComboBox)editedItem.FindControl("cboTipo")).SelectedValue;
            #endregion

            try
            {
                _clasif.Codigo = codigo_Ingresado;
                _clasif.Descripcion = Descripcion_Ingresado;
                _clasif.Tipo = Tipo_Ingresado;
                (Contexto as EntidadesConosud).SaveChanges();
                this.RadGrid1.Rebind();
            }
            catch (Exception e)
            {
                ScriptManager.RegisterStartupScript(updpnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);

            }
        }
    }
    protected void InsertClasificacion(GridEditableItem editedItem)
    {
        long idClasificacion = Convert.ToInt64(ViewState["idsel"]);
        
        #region Recupero los  Datos Ingresado por el usuario
        GridEditManager editMan = editedItem.EditManager;
        string codigo_Ingresado = (editMan.GetColumnEditor("Codigo") as GridTextBoxColumnEditor).Text;
        string Descripcion_Ingresado = (editMan.GetColumnEditor("Descripcion") as GridTextBoxColumnEditor).Text;
        string Tipo_Ingresado = ((RadComboBox)editedItem.FindControl("cboTipo")).SelectedValue;
        #endregion

        try
        {
            Clasificacion _clasif = new Clasificacion();
            _clasif.Codigo = codigo_Ingresado;
            _clasif.Descripcion = Descripcion_Ingresado;
            _clasif.Tipo = Tipo_Ingresado;

            (Contexto as EntidadesConosud).AddToClasificacion(_clasif);
            (Contexto as EntidadesConosud).SaveChanges();
            this.RadGrid1.Rebind();
        }
        catch (Exception e)
        {
            ScriptManager.RegisterStartupScript(updpnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);

        }

    }
    protected void DeleteClasificacion()
    {
        long idClasif = Convert.ToInt64(ViewState["idsel"]);


        Clasificacion _ItemClasif = (from c in (Contexto as EntidadesConosud).Clasificacion
                                   where c.IdClasificacion == idClasif
                                   select c).FirstOrDefault();

        if (_ItemClasif != null)
        {

            try
            {
                (Contexto as EntidadesConosud).DeleteObject(_ItemClasif);
                (Contexto as EntidadesConosud).SaveChanges();
                this.RadGrid1.Rebind();
            }
            catch (Exception e)
            {
                ScriptManager.RegisterStartupScript(updpnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);

            }
        }
    }
}
