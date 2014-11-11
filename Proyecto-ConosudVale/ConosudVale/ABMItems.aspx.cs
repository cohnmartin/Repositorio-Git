using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Entidades;
using System.Collections;

public partial class ABMItems : System.Web.UI.Page
{
    public EntidadesConosud Contexto
    {
        get
        {
            if (Session["Contexto"] != null)
                return (EntidadesConosud)Session["Contexto"];
            else
            {
                Session["Contexto"] = new EntidadesConosud();
                return (EntidadesConosud)Session["Contexto"];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            this.RadGrid1.Rebind();
        }

    }


    protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (txtApellidoLegajo.Text.Trim() != "")
        {
            this.RadGrid1.DataSource = (from c in Contexto.Plantilla orderby c.Codigo where c.Descripcion.Contains(txtApellidoLegajo.Text.Trim()) select c).ToList();
        }
        else
        {
            this.RadGrid1.DataSource = (from c in Contexto.Plantilla orderby c.Codigo select c).ToList();
        }

    }
    protected void RadGrid1_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Plantilla, idUsuario);


        LinkButton btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEdit");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
        btnAccion.Visible = PermisosPagina.Creacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDelete");
        btnAccion.Visible = PermisosPagina.Creacion;
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {

            Entidades.Plantilla hoja = EntityDataSourceExtensions.GetItemObject<Entidades.Plantilla>(e.Item.DataItem);
            if (hoja != null)
            {
                if (hoja.RolesPlanilla.Count > 0)
                {
                    (e.Item.FindControl("lblRol") as Label).Text = hoja.RolesPlanilla.First().SegRol.Descripcion;
                    (e.Item.FindControl("lblRiesgo") as Label).Text = hoja.Riesgo;
                    (e.Item.FindControl("lblGrado") as Label).Text = string.Format("Grado {0}" ,hoja.Grado);
                }
            }

        }
        else if (e.Item.ItemType == GridItemType.EditFormItem)
        {

            Entidades.Plantilla hoja = EntityDataSourceExtensions.GetItemObject<Entidades.Plantilla>(e.Item.DataItem);

            if (hoja != null)
            {
                if (hoja.RolesPlanilla.Count > 0)
                {
                    if (e.Item.FindControl("cboRoles") != null)
                    {
                        (e.Item.FindControl("cboRoles") as RadComboBox).SelectedValue = hoja.RolesPlanilla.First().SegRol.IdSegRol.ToString();
                   
                        Dictionary<string, string> riesgos = new Dictionary<string, string>();
                        riesgos.Add("ALTO", "ALTO");
                        riesgos.Add("MEDIO", "MEDIO");
                        riesgos.Add("BAJO", "BAJO");
                        
                        (e.Item.FindControl("cboRiesgos") as RadComboBox).DataSource = riesgos;
                        (e.Item.FindControl("cboRiesgos") as RadComboBox).DataBind();
                        (e.Item.FindControl("cboRiesgos") as RadComboBox).SelectedValue = hoja.Riesgo;


                        Dictionary<string, string> grados = new Dictionary<string, string>();
                        grados.Add("1", "Grado 1");
                        grados.Add("2", "Grado 2");
                        grados.Add("3", "Grado 3");
                        grados.Add("4", "Grado 4");
                        grados.Add("5", "Grado 5");


                        (e.Item.FindControl("cboGrados") as RadComboBox).DataSource = grados;
                        (e.Item.FindControl("cboGrados") as RadComboBox).DataBind();
                        (e.Item.FindControl("cboGrados") as RadComboBox).SelectedValue = hoja.Grado.ToString();


                    }

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
                InsertPlanilla(editedItem);
            }

            if (e.Item is GridEditFormItem)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                UpdatePlanilla(editedItem);
            }

        }

        else if (e.CommandName == "DeleteSelected")
        {
            DeletePlanilla();
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
    protected void btnBuscar_Click1(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();
        var a = (from p in dc.Plantilla
                 select p).ToList();

        gvFiles.DataSource = a;
        gvFiles.DataBind();

        ExportToExcel();
    }

    protected void ExportToExcel()
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=MyFiles.xls");
        Response.Charset = "";
        this.EnableViewState = false;

        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);

        gvFiles.RenderControl(htw);

        Response.Write(sw.ToString());
        Response.End();
    }
    protected void InsertPlanilla(GridEditableItem editedItem)
    {
        long idPlanilla = Convert.ToInt64(ViewState["idsel"]);

        #region Recupero los  Datos Ingresado por el usuario
        GridEditManager editMan = editedItem.EditManager;
        string codigo_Ingresado = (editMan.GetColumnEditor("Codigo") as GridTextBoxColumnEditor).Text;
        string Descripcion_Ingresado = (editMan.GetColumnEditor("Descripcion") as GridTextBoxColumnEditor).Text;
        string Categoria_Ingresado = (editMan.GetColumnEditor("IdCategoria") as GridDropDownColumnEditor).SelectedValue;
        string Riesgo_Ingresado = ((RadComboBox)editedItem.FindControl("cboRiesgos")).SelectedValue;
        string Grado_Ingresado = ((RadComboBox)editedItem.FindControl("cboGrados")).SelectedValue;
        string Rol_Ingresado = ((RadComboBox)editedItem.FindControl("cboRoles")).SelectedValue;
        #endregion

        try
        {
            Plantilla _ItemPlanilla = new Plantilla();
            _ItemPlanilla.Codigo = codigo_Ingresado;
            _ItemPlanilla.Descripcion = Descripcion_Ingresado;
            _ItemPlanilla.IdCategoria = long.Parse(Categoria_Ingresado);
            _ItemPlanilla.Riesgo = Riesgo_Ingresado;
            _ItemPlanilla.Grado = int.Parse(Grado_Ingresado);

            RolesPlanilla _rolPlantilla = new RolesPlanilla();
            _rolPlantilla.IdRol = long.Parse(Rol_Ingresado);
            _rolPlantilla.Plantilla = _ItemPlanilla;

            _ItemPlanilla.RolesPlanilla.Add(_rolPlantilla);

            Contexto.AddToPlantilla(_ItemPlanilla);
            Contexto.SaveChanges();
            this.RadGrid1.Rebind();
        }
        catch (Exception e)
        {
            ScriptManager.RegisterStartupScript(updpnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);

        }

    }
    protected void UpdatePlanilla(GridEditableItem editedItem)
    {
        long idPlanilla = Convert.ToInt64(ViewState["idsel"]);


        Plantilla _ItemPlanilla = (from c in Contexto.Plantilla
                                   where c.IdPlantilla == idPlanilla
                                   select c).FirstOrDefault();

        if (_ItemPlanilla != null)
        {

            #region Recupero los  Datos Ingresado por el usuario
            GridEditManager editMan = editedItem.EditManager;
            string codigo_Ingresado = (editMan.GetColumnEditor("Codigo") as GridTextBoxColumnEditor).Text;
            string Descripcion_Ingresado = (editMan.GetColumnEditor("Descripcion") as GridTextBoxColumnEditor).Text;
            string Categoria_Ingresado = (editMan.GetColumnEditor("IdCategoria") as GridDropDownColumnEditor).SelectedValue;
            string Riesgo_Ingresado = ((RadComboBox)editedItem.FindControl("cboRiesgos")).SelectedValue;
            string Grado_Ingresado = ((RadComboBox)editedItem.FindControl("cboGrados")).SelectedValue;

            string Rol_Ingresado = ((RadComboBox)editedItem.FindControl("cboRoles")).SelectedValue;
            
            #endregion

            try
            {
                _ItemPlanilla.Codigo = codigo_Ingresado;
                _ItemPlanilla.Descripcion = Descripcion_Ingresado;
                _ItemPlanilla.IdCategoria = long.Parse(Categoria_Ingresado);
                _ItemPlanilla.Riesgo = Riesgo_Ingresado;
                _ItemPlanilla.Grado = int.Parse(Grado_Ingresado);

                if (_ItemPlanilla.RolesPlanilla.FirstOrDefault() != null)
                    _ItemPlanilla.RolesPlanilla.First().IdRol = long.Parse(Rol_Ingresado);

                Contexto.SaveChanges();
                this.RadGrid1.Rebind();
            }
            catch (Exception e)
            {
                ScriptManager.RegisterStartupScript(updpnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);

            }
        }
    }
    protected void DeletePlanilla()
    {
        long idPlanilla = Convert.ToInt64(ViewState["idsel"]);


        Plantilla _ItemPlanilla = (from c in Contexto.Plantilla
                                   where c.IdPlantilla == idPlanilla
                                   select c).FirstOrDefault();

        if (_ItemPlanilla != null)
        {

            try
            {
                List<RolesPlanilla> roles = _ItemPlanilla.RolesPlanilla.ToList();
                foreach (var item in roles)
                {
                    Contexto.DeleteObject(item);
                }

                Contexto.DeleteObject(_ItemPlanilla);
                Contexto.SaveChanges();
                this.RadGrid1.Rebind();
            }
            catch (Exception e)
            {
                ScriptManager.RegisterStartupScript(updpnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);

            }
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }


}
