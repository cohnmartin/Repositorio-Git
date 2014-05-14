using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Entidades;

public partial class ABMEmpresas2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RadGrid1.DataBound += new EventHandler(RadGrid1_DataBound);
       
    }

    void RadGrid1_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Empresas, idUsuario);


        LinkButton btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEdit");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
        btnAccion.Visible = PermisosPagina.Creacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDelete");
        btnAccion.Visible = PermisosPagina.Creacion;
    }

    protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "ExportEmpresas")
        {
            ConfigureExportAndExport();
        }


        // ver si aca entra cdo se presiona eliminar y hace la logica que corrseponde
    }

    protected void EntityDataSourceEmpresas_Selecting(object sender, EntityDataSourceSelectingEventArgs e)
    {
        if (this.txtApellidoLegajo.Text == string.Empty)
        {
            e.DataSource.Where = "";
            e.DataSource.AutoGenerateWhereClause = true;
        }

    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        this.RadGrid1.Rebind();
        this.updpnlGrilla.Update();
    }

    public void ConfigureExportAndExport()
    {
        foreach (Telerik.Web.UI.GridColumn column in RadGrid1.MasterTableView.Columns)
        {
            if (!column.Visible || !column.Display )
            {
                column.Visible = true;
                column.Display = true;
            }
        }

        RadGrid1.ExportSettings.ExportOnlyData = true;
        RadGrid1.ExportSettings.IgnorePaging = true;
        RadGrid1.ExportSettings.FileName = "Empresas";
        RadGrid1.MasterTableView.ExportToExcel();

    }
  
    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();

        if (e.Argument == "Update")
        {
            long id = long.Parse(RadGrid1.SelectedValue.ToString());

            Entidades.Empresa EmpUpdate = (from L in dc.Empresa
                                           where L.IdEmpresa == id
                                           select L).FirstOrDefault<Entidades.Empresa>();

            int cant = (from l in dc.Empresa
                        where l.CUIT.Trim() == txtCUIT.Text.Trim()
                        && l.IdEmpresa != id
                        select l).Count();

            if (cant > 0)
            {
                txtCUIT.Attributes.Add("NroExistente", true.ToString());
                upNroCUIT.Update();
                return;
            }
            else
            {
                txtCUIT.Attributes.Add("NroExistente", false.ToString());
                upNroCUIT.Update();
            }



            if (EmpUpdate != null)
            {
                /// Controles Tipo TextBox
                EmpUpdate.RazonSocial= txtRazonSocial.Text.Trim();
                EmpUpdate.RepresentanteTecnico = txtTecnico.Text.Trim();
                EmpUpdate.PrestacionEmergencia = txtPrestacionEmergencias.Text.Trim();
                EmpUpdate.Direccion = txtDireccion.Text.Trim();
                EmpUpdate.Telefono = txtTelefono.Text.Trim();
                EmpUpdate.Emergencia = txtEmergencias.Text.Trim();
                EmpUpdate.CorreoElectronico = txtEmail.Text.Trim();

                /// Controles Tipo Telerik
                EmpUpdate.CUIT = txtCUIT.Text;

                /// Controles Tipo Fecha
                EmpUpdate.FechaAlta= txtFechaAlta.SelectedDate;

                /// Controles Tipo Combos
                long idCombo = 0;
                if (cboART.SelectedItem != null)
                {
                    idCombo = long.Parse(cboART.SelectedValue);
                    EmpUpdate.objART = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }
                else
                {
                    EmpUpdate.objART = null;
                }

                dc.SaveChanges();
            }
        }
        else if (e.Argument == "Eliminar")
        {
            long id = long.Parse(RadGrid1.SelectedValue.ToString());

            Entidades.Empresa EmpEliminar = (from L in dc.Empresa
                                             where L.IdEmpresa == id
                                             select L).FirstOrDefault<Entidades.Empresa>();

            if (EmpEliminar.ContratoEmpresas.Count == 0)
            {
                dc.DeleteObject(EmpEliminar);
            }
            else
            {
                ScriptManager.RegisterStartupScript(updpnlGrilla, updpnlGrilla.GetType(), "eliemp", "alert('La empresa seleccionada no puede ser eliminada ya que posee uno o más contratos asociados')", true);
            }
            
            dc.SaveChanges();
        }
        else
        {

            Entidades.Empresa EmpInsert = new Entidades.Empresa();

            int cant = (from l in dc.Empresa
                        where l.CUIT == txtCUIT.Text.Trim()
                        select l).Count();


            if (cant > 0)
            {
                txtCUIT.Attributes.Add("NroExistente", true.ToString());
                upNroCUIT.Update();
                return;
            }
            else
            {
                txtCUIT.Attributes.Add("NroExistente", false.ToString());
                upNroCUIT.Update();
            }


            /// Controles Tipo TextBox
            EmpInsert.RazonSocial = txtRazonSocial.Text.Trim();
            EmpInsert.RepresentanteTecnico = txtTecnico.Text.Trim();
            EmpInsert.PrestacionEmergencia = txtPrestacionEmergencias.Text.Trim();
            EmpInsert.Direccion = txtDireccion.Text.Trim();
            EmpInsert.Telefono = txtTelefono.Text.Trim();
            EmpInsert.Emergencia = txtEmergencias.Text.Trim();
            EmpInsert.CorreoElectronico = txtEmail.Text.Trim();

            /// Controles Tipo Telerik
            EmpInsert.CUIT = txtCUIT.Text;

            /// Controles Tipo Fecha
            EmpInsert.FechaAlta = txtFechaAlta.SelectedDate;

            /// Controles Tipo Combos
            long idCombo = 0;
            if (cboART.SelectedItem != null)
            {
                idCombo = long.Parse(cboART.SelectedValue);
                EmpInsert.objART = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
            }

            dc.AddToEmpresa(EmpInsert);
            dc.SaveChanges();


        }

        RadGrid1.Rebind();
        updpnlGrilla.Update();
    }
}
