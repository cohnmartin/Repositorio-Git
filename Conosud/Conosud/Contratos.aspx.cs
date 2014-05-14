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

public partial class Contratos : System.Web.UI.Page
{
    static object _datos = null;
    static int  _RowSelected = 0;
    public int RowSelected
    {
        get
        {
            return _RowSelected;
        }
        set
        {
           _RowSelected = value;
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //PostBackOptions a = new PostBackOptions(btnEditar);
        //a.TrackFocus = true;
        //a.ClientSubmit = true;

        //Page.InitComplete += new EventHandler(Page_InitComplete);
        
        dvContratos.Attributes.Add("onkeydown", "NoSubmit();");
        
        //string jscript = "function Editar(){ " +
        //        ClientScript.GetPostBackEventReference(a) + ";" +
        //           "" + "};";
        
       // Page.ClientScript.RegisterClientScriptBlock(typeof(UpdatePanel), "Editar", jscript, true);


        if (!this.IsPostBack)
        {
  
            _RowSelected = 1;
            _datos = ODSSelContratos.Select();
            gvContratos.DataSource = _datos;
            gvContratos.DataBind();
        }

        GridViewRow pagerRow = gvContratos.BottomPagerRow;
        ImageButton igbExcel = (ImageButton)pagerRow.FindControl("igbExcel");
        if (igbExcel != null)
        {
            igbExcel.Click += new ImageClickEventHandler(igbExcel_Click);
        }
    }

    void Page_InitComplete(object sender, EventArgs e)
    {
        //if (!ClientScript.IsStartupScriptRegistered("PopupScript"))
        //{
        //    string popupScript = "<script language='JavaScript'>" +
        //            "rowBind();" +
        //            "</script>";

        //    ClientScript.RegisterStartupScript(typeof(UpdatePanel), "PopupScript", popupScript);
        //}


       // Page.ClientScript.RegisterStartupScript(typeof(UpdatePanel), "rowstt", "alert('');", true);
    }

    protected void dvContratos_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        _datos = ODSSelContratos.Select();
        gvContratos.DataSource = _datos;
        gvContratos.DataBind();
    }

    protected void dvContratos_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        DSConosudTableAdapters.ContratoEmpresasTableAdapter TAContratoEmpresa = new DSConosudTableAdapters.ContratoEmpresasTableAdapter();
        DSConosud.ContratoEmpresasDataTable dtContEmp = TAContratoEmpresa.GetDataByIdContratoContratista(Convert.ToInt64(this.gvContratos.SelectedValue));
        if (dtContEmp.Count != 0)
        {
            DropDownList DDLEditContratista = (DropDownList)this.dvContratos.FindControl("DDLEditContratista");

            if (dtContEmp[0].IdEmpresa != Convert.ToInt64(DDLEditContratista.SelectedValue))
            {
                dtContEmp[0].IdEmpresa = Convert.ToInt64(DDLEditContratista.SelectedValue);
                TAContratoEmpresa.Update(dtContEmp[0]);
            }
        }

        _datos = ODSSelContratos.Select();
        gvContratos.DataSource = _datos;
        gvContratos.DataBind();
    }

    protected void dvContratos_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        DSConosud Ds = new DSConosud();

        DSConosud.ContratoRow drContrato = Ds.Contrato.NewContratoRow();
        drContrato.Codigo = (string) e.Values["Codigo"];
        drContrato.Servicio = (string) e.Values["Servicio"];
        if (e.Values["FechaInicio"] != null) drContrato.FechaInicio = Convert.ToDateTime(e.Values["FechaInicio"]);
        if (e.Values["FechaVencimiento"] != null) drContrato.FechaVencimiento = Convert.ToDateTime(e.Values["FechaVencimiento"]);
        if (e.Values["Prorroga"] != null) drContrato.Prorroga = Convert.ToDateTime(e.Values["Prorroga"]);
        if (e.Values["TipoContrato"] != null) drContrato.TipoContrato = Convert.ToInt64(e.Values["TipoContrato"]);
        if (e.Values["Contratadopor"] != null) drContrato.Contratadopor = Convert.ToInt64(e.Values["Contratadopor"]);
        Ds.Contrato.AddContratoRow(drContrato);

        DropDownList DDLInsertContra = (DropDownList)this.dvContratos.FindControl("DDLInsertContratista");

        DSConosud.ContratoEmpresasRow drEmpresaContrato = Ds.ContratoEmpresas.NewContratoEmpresasRow();
        drEmpresaContrato.EsContratista = true;
        drEmpresaContrato.IdContrato = drContrato.IdContrato;
        drEmpresaContrato.IdEmpresa = Convert.ToInt32(DDLInsertContra.SelectedValue);
        Ds.ContratoEmpresas.AddContratoEmpresasRow(drEmpresaContrato);

        DSConosudTableAdapters.ContratoTableAdapter TAContrato = new DSConosudTableAdapters.ContratoTableAdapter();
        TAContrato.Update(Ds);

        DSConosudTableAdapters.ContratoEmpresasTableAdapter TAContratoEmpresa = new DSConosudTableAdapters.ContratoEmpresasTableAdapter();
        TAContratoEmpresa.Update(Ds);

        DSConosudTableAdapters.EmpresaTableAdapter TAEmpresas = new DSConosudTableAdapters.EmpresaTableAdapter();
        TAEmpresas.FillByIdEmpresa(Ds.Empresa, Convert.ToInt32(drEmpresaContrato.IdEmpresa));

        DateTime FFin = DateTime.Now;
        if (drContrato.IsProrrogaNull())
        {
            FFin = drContrato.FechaVencimiento;
        }
        else
        {
            FFin = drContrato.Prorroga;
        }

        Helpers.GenerarHojadeRuta(Ds, drContrato.FechaInicio, FFin, drEmpresaContrato.IdContratoEmpresas);

        DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter TACabHojaRuta = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
        DSConosudTableAdapters.HojasDeRutaTableAdapter TAHojaRuta = new DSConosudTableAdapters.HojasDeRutaTableAdapter();
        TACabHojaRuta.Update(Ds);
        TAHojaRuta.Update(Ds);

        e.Cancel = true;
        dvContratos.ChangeMode(DetailsViewMode.ReadOnly);

        _datos = ODSSelContratos.Select();
        gvContratos.DataSource = _datos;
        gvContratos.DataBind();
    }

    protected void dvContratos_ItemDeleting(object sender, DetailsViewDeleteEventArgs e)
    {
        try
        {
            DSConosudTableAdapters.ContratoEmpresasTableAdapter TAContratoEmpresa = new DSConosudTableAdapters.ContratoEmpresasTableAdapter();
            DSConosud.ContratoEmpresasDataTable dtContEmp = TAContratoEmpresa.GetDataByIdContrato(Convert.ToInt64(this.gvContratos.SelectedValue));

            foreach (DSConosud.ContratoEmpresasRow rowContEmp in dtContEmp)
            {
                rowContEmp.Delete();
            }

            TAContratoEmpresa.Update(dtContEmp);
        }
        catch (Exception ex)
        {
            //if (-1 != ex.Message.IndexOf("FK_ContEmpLegajos",0))
            //{
            //    string alert = "alert('El Contrato no puede ser eliminado, tiene legajos asociados')";
            //    System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
            //}
            
            e.Cancel = true;
        }
    }

    protected void dvContratos_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        try
        {
            DateTime FechaInicioAnt = Convert.ToDateTime(e.OldValues[2]);
            DateTime FechaFinAnt = DateTime.MinValue;
            DateTime FechaInicioNuevo = Convert.ToDateTime(e.NewValues[2]);
            DateTime FechaFinNuevo = DateTime.MinValue;

            FechaFinAnt = DeterminarFinPeriodo(e.OldValues[3], e.OldValues[4], ref FechaFinAnt);
            FechaFinNuevo = DeterminarFinPeriodo(e.NewValues[3], e.NewValues[4], ref FechaFinNuevo);


            DSConosud Ds = new DSConosud();
            Ds.EnforceConstraints = false;
            DSConosud DsFinal = new DSConosud();
            DsFinal.EnforceConstraints = false;

            long IdContrato = Convert.ToInt64(gvContratos.SelectedValue);

            DSConosudTableAdapters.ContratoEmpresasTableAdapter TAContratoEmpresa = new DSConosudTableAdapters.ContratoEmpresasTableAdapter();
            DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter TACabHojaRuta = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
            DSConosudTableAdapters.HojasDeRutaTableAdapter TAHojaRuta = new DSConosudTableAdapters.HojasDeRutaTableAdapter();

            TAContratoEmpresa.FillByIdContrato(Ds.ContratoEmpresas, IdContrato);

            foreach (DSConosud.ContratoEmpresasRow rowContEmp in Ds.ContratoEmpresas)
            {
                Ds.CabeceraHojasDeRuta.Clear();
                Ds.HojasDeRuta.Clear();

                TACabHojaRuta.FillByIdContEmp(Ds.CabeceraHojasDeRuta, rowContEmp.IdContratoEmpresas);

                if (Ds.CabeceraHojasDeRuta.Count > 0)
                {
                    foreach (DSConosud.CabeceraHojasDeRutaRow CabHR in Ds.CabeceraHojasDeRuta)
                    {
                        Ds.Merge(TAHojaRuta.GetDataByCabecera(CabHR.IdCabeceraHojasDeRuta));
                    }

                    if (FechaInicioNuevo != FechaInicioAnt)
                    {
                        if (FechaInicioNuevo > FechaInicioAnt)
                        {
                            ///Borro Diferencias
                            foreach (DSConosud.CabeceraHojasDeRutaRow rowCabHR in Ds.CabeceraHojasDeRuta)
                            {
                                if (FechaInicioNuevo > rowCabHR.Periodo)
                                {
                                    rowCabHR.Delete();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ///Agrega Diferencias
                            FechaInicioAnt = FechaInicioAnt.AddMonths(-1);
                            Helpers.GenerarHojadeRuta(Ds, FechaInicioNuevo, FechaInicioAnt, rowContEmp.IdContratoEmpresas);
                        }

                    }

                    if (FechaFinNuevo != FechaFinAnt)
                    {
                        if (FechaFinNuevo > FechaFinAnt)
                        {
                            ///Agrega Diferencias
                            FechaFinAnt = FechaFinAnt.AddMonths(1);
                            Helpers.GenerarHojadeRuta(Ds, FechaFinAnt, FechaFinNuevo, rowContEmp.IdContratoEmpresas);
                        }
                        else
                        {
                            ///Borro Diferencias
                            foreach (DSConosud.CabeceraHojasDeRutaRow rowCabHR in Ds.CabeceraHojasDeRuta)
                            {
                                if (rowCabHR.Periodo > FechaFinNuevo)
                                {
                                    rowCabHR.Delete();
                                }
                            }
                        }
                    }
                }
                else
                {
                    Helpers.GenerarHojadeRuta(Ds, FechaInicioNuevo, FechaFinNuevo, rowContEmp.IdContratoEmpresas);
                }

                DsFinal.Merge(Ds.CabeceraHojasDeRuta);
                DsFinal.Merge(Ds.HojasDeRuta);
                TACabHojaRuta.Update(DsFinal);
                TAHojaRuta.Update(DsFinal);
        }
    }
    catch (Exception ex)
    {
        e.Cancel = true;
    }
   
    }

    private static DateTime DeterminarFinPeriodo(object FechaVenc, object FechaPro, ref DateTime FechaFinAnt)
    {
        if (FechaPro != null)
        {
            if (Convert.ToDateTime(FechaVenc) > Convert.ToDateTime(FechaPro))
            {
                FechaFinAnt = Convert.ToDateTime(FechaPro);
            }
            else
            {
                FechaFinAnt = Convert.ToDateTime(FechaVenc);
            }
        }
        else
        {
            FechaFinAnt = Convert.ToDateTime(FechaVenc);
        }

        return FechaFinAnt;
    }

    protected void ODSContratos_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {


    }

    protected void ODSContratos_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
       
    }

    protected void dvContratos_DataBound(object sender, EventArgs e)
    {
        DSConosudTableAdapters.ContratoEmpresasTableAdapter TAContratoEmpresa = new DSConosudTableAdapters.ContratoEmpresasTableAdapter();
        DSConosud.ContratoEmpresasDataTable dtContEmp = TAContratoEmpresa.GetDataByIdContratoContratista(Convert.ToInt64(this.gvContratos.SelectedValue));
        if (dtContEmp.Count != 0)
        {
            DropDownList DDLContratista = null;
            if (dvContratos.CurrentMode == DetailsViewMode.Edit)
            {
                DDLContratista = (DropDownList)this.dvContratos.FindControl("DDLEditContratista");
            }
            else if (dvContratos.CurrentMode == DetailsViewMode.ReadOnly)
            {
                DDLContratista = (DropDownList)this.dvContratos.FindControl("DDLItemContratista");
            }

            if (dtContEmp[0].IdEmpresa != 0 && DDLContratista != null)
            {
                DDLContratista.SelectedValue = dtContEmp[0].IdEmpresa.ToString();
            }
        }

    }

    protected void igbExcel_Click(object sender, ImageClickEventArgs e)
    {
        DSConosudTableAdapters.ContratoTableAdapter TACont = new DSConosudTableAdapters.ContratoTableAdapter();
        DSConosud.ContratoDataTable dtCont = TACont.GetDataByContratosCompleto();
        Helpers.GenExcell c = new Helpers.GenExcell();
        c.DoExcell(Server.MapPath(Request.ApplicationPath) + @"\ReporteExcel.xls", dtCont, this.gvContratos.Columns);

        System.Web.UI.ScriptManager.RegisterStartupScript(upGrilla, this.GetType(), "click", "window.open('ReporteExcel.xls')", true);
    }

    protected void dvContratos_ItemCreated(object sender, EventArgs e)
    {
        if (!(dvContratos.HeaderRow == null))
        {
             int commandRowIndex = 0;
             if ((dvContratos.Rows.Count - 1) != -1)
             {
                  DetailsViewRow commandRow = dvContratos.Rows[commandRowIndex];
                  DataControlFieldCell cell = (DataControlFieldCell)commandRow.Controls[0]; 

                  foreach (Control ctl in cell.Controls)
                  {
                       if (ctl.GetType().Name == "DataControlImageButton")
                       {
                            ImageButton btn = (ImageButton)ctl;

                            if (btn.CommandName == "Delete")
                            {
                                 btn.OnClientClick = "if(!confirm('Realmente desea eliminar el Contrato seleccionado')){return false;}";
                            }
                       }
                  }
             }
        }
    }

    protected void gvContratos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Attributes.Add("onclick", "rowBind();");
        //}

        
    }

    protected void gvContratos_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void gvContratos_PageIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvContratos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (e.NewPageIndex >= 0)
        {
            gvContratos.PageIndex = e.NewPageIndex;
            _datos = ODSSelContratos.Select();
            gvContratos.DataSource = _datos;
            gvContratos.DataBind();
        }
    }
}
