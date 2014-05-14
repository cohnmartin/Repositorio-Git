using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;

public partial class ABMLegajos2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RadGrid1.DataBound += new EventHandler(RadGrid1_DataBound);
    }

    void RadGrid1_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Legajos, idUsuario);


        LinkButton btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEdit");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
        btnAccion.Visible = PermisosPagina.Creacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDelete");
        btnAccion.Visible = PermisosPagina.Creacion;
    }
  

    protected void EntityDataSourceLegajos_Selecting(object sender, EntityDataSourceSelectingEventArgs e)
    {
        if (this.txtApellidoLegajo.Text == string.Empty && this.txtNroDoc.Text == string.Empty)
        {
            e.DataSource.Where = "";
            e.DataSource.AutoGenerateWhereClause = true;
        }
    }

    public void ConfigureExportAndExport()
    {
        foreach (Telerik.Web.UI.GridColumn column in RadGrid1.MasterTableView.Columns)
        {
            if (!column.Visible || !column.Display)
            {
                column.Visible = true;
                column.Display = true;
            }
        }

        RadGrid1.ExportSettings.ExportOnlyData = true;
        RadGrid1.ExportSettings.IgnorePaging = true;
        RadGrid1.ExportSettings.FileName = "Legajos";
        RadGrid1.MasterTableView.ExportToExcel();



    }

    protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "ExportLegajos")
        {
            ConfigureExportAndExport();
        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        this.RadGrid1.Rebind();
        this.UpdPnlGrilla.Update();
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            long id = long.Parse(RadGrid1.SelectedValue.ToString());
            EntidadesConosud dc = new EntidadesConosud();
            Entidades.Legajos LegEliminar = (from L in dc.Legajos
                                             where L.IdLegajos == id
                                             select L).FirstOrDefault<Entidades.Legajos>();

            dc.DeleteObject(LegEliminar);
            dc.SaveChanges();
            RadGrid1.Rebind();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "eliminacino", "alert('El legajo no puede ser eliminado ya que es parte de algún contrato.');", true);
        }

    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();

        if (e.Argument == "Update")
        {
            long id = long.Parse(RadGrid1.SelectedValue.ToString());

            Entidades.Legajos LegUpdate = (from L in dc.Legajos
                                           where L.IdLegajos == id
                                           select L).FirstOrDefault<Entidades.Legajos>();

            int cant = (from l in dc.Legajos
                        where l.NroDoc == txtNroDocEdit.Text.Trim()
                        && l.IdLegajos != id
                        select l).Count();

            if (cant > 0)
            {
                txtNroDocEdit.Attributes.Add("NroExistente", true.ToString());
                upNroDoc.Update();
                return;
            }
            else
            {
                txtNroDocEdit.Attributes.Add("NroExistente", false.ToString());
                upNroDoc.Update();
            }



            if (LegUpdate != null)
            {
                /// Controles Tipo TextBox
                LegUpdate.Apellido = txtApellido.Text.Trim();
                LegUpdate.Nombre = txtNombre.Text.Trim();
                LegUpdate.NroDoc = txtNroDocEdit.Text.Trim();
                LegUpdate.Direccion = txtDireccion.Text.Trim();
                LegUpdate.CodigoPostal = txtCodigoPostal.Text.Trim();
                LegUpdate.TelefonoFijo = txtTelFijo.Text.Trim();
                LegUpdate.CorreoElectronico = txtEmail.Text.Trim();

                /// Controles Tipo Telerik
                LegUpdate.CUIL = txtCUIL.Text;

                /// Controles Tipo Fecha
                LegUpdate.FechaNacimiento = txtFechaNacimiento.SelectedDate;
                LegUpdate.FechaIngreos = txtFechaIngreso.SelectedDate;

                /// Controles Tipo Combos
                long idCombo = 0;
                if (cboTipoDoc.SelectedItem != null)
                {
                    idCombo = long.Parse(cboTipoDoc.SelectedValue);
                    LegUpdate.objTipoDocumento = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }

                if (cboEstadoCivil.SelectedItem != null)
                {
                    idCombo = long.Parse(cboEstadoCivil.SelectedValue);
                    LegUpdate.objEstadoCivil = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }

                if (cboNacionalidad.SelectedItem != null)
                {
                    idCombo = long.Parse(cboNacionalidad.SelectedValue);
                    LegUpdate.objNacionalidad = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }

                if (cboConvenio.SelectedItem != null)
                {
                    idCombo = long.Parse(cboConvenio.SelectedValue);
                    LegUpdate.objConvenio = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }

                if (cboProvincia.SelectedItem != null)
                {
                    idCombo = long.Parse(cboProvincia.SelectedValue);
                    LegUpdate.objProvincia = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }

                dc.SaveChanges();
            }
        }
        else
        {

            Entidades.Legajos LegInsert = new Entidades.Legajos();

            int cant = (from l in dc.Legajos
                        where l.NroDoc == txtNroDocEdit.Text.Trim()
                        select l).Count();


            if (cant > 0)
            {
                txtNroDocEdit.Attributes.Add("NroExistente", true.ToString());
                upNroDoc.Update();
                return;
            }
            else
            {
                txtNroDocEdit.Attributes.Add("NroExistente", false.ToString());
                upNroDoc.Update();
            }


            /// Controles Tipo TextBox
            LegInsert.Apellido = txtApellido.Text.Trim();
            LegInsert.Nombre = txtNombre.Text.Trim();
            LegInsert.NroDoc = txtNroDocEdit.Text.Trim();
            LegInsert.Direccion = txtDireccion.Text.Trim();
            LegInsert.CodigoPostal = txtCodigoPostal.Text.Trim();
            LegInsert.TelefonoFijo = txtTelFijo.Text.Trim();
            LegInsert.CorreoElectronico = txtEmail.Text.Trim();

            /// Controles Tipo Telerik
            LegInsert.CUIL = txtCUIL.Text;

            /// Controles Tipo Fecha
            LegInsert.FechaNacimiento = txtFechaNacimiento.SelectedDate;
            LegInsert.FechaIngreos = txtFechaIngreso.SelectedDate;

            /// Controles Tipo Combos
            long idCombo = 0;
            if (cboTipoDoc.SelectedItem != null)
            {
                idCombo = long.Parse(cboTipoDoc.SelectedValue);
                LegInsert.objTipoDocumento = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
            }

            if (cboEstadoCivil.SelectedItem != null)
            {
                idCombo = long.Parse(cboEstadoCivil.SelectedValue);
                LegInsert.objEstadoCivil = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
            }

            if (cboNacionalidad.SelectedItem != null)
            {
                idCombo = long.Parse(cboNacionalidad.SelectedValue);
                LegInsert.objNacionalidad = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
            }

            if (cboConvenio.SelectedItem != null)
            {
                idCombo = long.Parse(cboConvenio.SelectedValue);
                LegInsert.objConvenio = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
            }

            if (cboProvincia.SelectedItem != null)
            {
                idCombo = long.Parse(cboProvincia.SelectedValue);
                LegInsert.objProvincia = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
            }

            dc.AddToLegajos(LegInsert);
            dc.SaveChanges();


        }

        RadGrid1.Rebind();
        UpdPnlGrilla.Update();
    }

    //protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ViewState["idsel"] = RadGrid1.SelectedValue;
    //}

    //protected void EntityDataSourceLegajos_Inserting(object sender, EntityDataSourceChangingEventArgs e)
    //{
    //    if ((e.Entity as Entidades.Legajos).objEstadoCivilReference.EntityKey.EntityKeyValues[0].Value.ToString() == "0")
    //    {
    //        (e.Entity as Entidades.Legajos).objEstadoCivil = null;
    //    }
    //    if ((e.Entity as Entidades.Legajos).objConvenioReference.EntityKey.EntityKeyValues[0].Value.ToString() == "0")
    //    {
    //        (e.Entity as Entidades.Legajos).objConvenio = null;
    //    }
    //    if ((e.Entity as Entidades.Legajos).objNacionalidadReference.EntityKey.EntityKeyValues[0].Value.ToString() == "0")
    //    {
    //        (e.Entity as Entidades.Legajos).objNacionalidad = null;
    //    }
    //    if ((e.Entity as Entidades.Legajos).objProvinciaReference.EntityKey.EntityKeyValues[0].Value.ToString() == "0")
    //    {
    //        (e.Entity as Entidades.Legajos).objProvincia = null;
    //    }
    //    if ((e.Entity as Entidades.Legajos).objTipoDocumentoReference.EntityKey.EntityKeyValues[0].Value.ToString() == "0")
    //    {
    //        (e.Entity as Entidades.Legajos).objTipoDocumento = null;
    //    }
    //}

    //protected void EntityDataSourceLegajos_Updating(object sender, EntityDataSourceChangingEventArgs e)
    //{
    //    if ((e.Entity as Entidades.Legajos).objEstadoCivilReference.EntityKey == null || (e.Entity as Entidades.Legajos).objEstadoCivilReference.EntityKey.EntityKeyValues[0].Value.ToString() == "0")
    //    {
    //        (e.Entity as Entidades.Legajos).objEstadoCivil = null;
    //    }
    //    if ((e.Entity as Entidades.Legajos).objConvenioReference.EntityKey == null || (e.Entity as Entidades.Legajos).objConvenioReference.EntityKey.EntityKeyValues[0].Value.ToString() == "0")
    //    {
    //        (e.Entity as Entidades.Legajos).objConvenio = null;
    //    }
    //    if ((e.Entity as Entidades.Legajos).objNacionalidadReference.EntityKey == null || (e.Entity as Entidades.Legajos).objNacionalidadReference.EntityKey.EntityKeyValues[0].Value.ToString() == "0")
    //    {
    //        (e.Entity as Entidades.Legajos).objNacionalidad = null;
    //    }
    //    if ((e.Entity as Entidades.Legajos).objProvinciaReference.EntityKey == null || (e.Entity as Entidades.Legajos).objProvinciaReference.EntityKey.EntityKeyValues[0].Value.ToString() == "0")
    //    {
    //        (e.Entity as Entidades.Legajos).objProvincia = null;
    //    }
    //    if ((e.Entity as Entidades.Legajos).objTipoDocumentoReference.EntityKey == null || (e.Entity as Entidades.Legajos).objTipoDocumentoReference.EntityKey.EntityKeyValues[0].Value.ToString() == "0")
    //    {
    //        (e.Entity as Entidades.Legajos).objTipoDocumento = null;
    //    }
    //}
}
