using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Entidades;

public partial class ConsultaDatosSueldos : System.Web.UI.Page
{
    public object DatosConsultados
    {
        get
        {
            if (Session["DatosConsultados"] != null)
                return Session["DatosConsultados"];
            else
            {
                return null;
            }
        }
        set
        {
            Session["DatosConsultados"] = value;
        }
    }

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
            gvDatosSueldos.DataSource = new List<object>();
            gvDatosSueldos.DataBind();
            DatosConsultados = null;

        }
    }

    protected void gvDatosSueldos_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (txtPeriodo.SelectedDate != null)
        {
            if (DatosConsultados == null)
            {
                var datos = from d in Contexto.DatosDeSueldos
                            where d.Periodo.Value.Month == txtPeriodo.SelectedDate.Value.Month &&
                            d.Periodo.Value.Year == txtPeriodo.SelectedDate.Value.Year
                            select new
                            {

                                RazonSocial = d.objEmpresa.RazonSocial,
                                CUIT = d.objEmpresa.CUIT,
                                Contrato = d.objContrato.Codigo,
                                NombreLegajo = d.objLegajo.Apellido + "," + d.objLegajo.Nombre,
                                MesLiquidacion = d.Periodo,
                                FechaNacimiento = d.objLegajo.FechaNacimiento,
                                CUIL = d.objLegajo.CUIL,
                                FECHADEINGRESO = d.objLegajo.FechaIngreos,
                                CATEGORIACCT = d.CategoriaCCT,
                                ENCUADREGREMIAL = d.objLegajo.objConvenio.Descripcion,
                                FUNCION = d.FuncionCCT,
                                BASICO = d.Basico_ValorHora,
                                BASICOLIQUIDADO = d.BasicoLiquidado,
                                HORASEXTRAS = d.HorasExtras,
                                ADICIONALES = d.AdicionalesRemunerativos,
                                VACACIONES = d.Vacaciones,
                                SAC = d.SAC,
                                TOTALBRUTO = d.TotalBruto,
                                ASIGNFLIARES = d.AsignacionFamiliar,
                                ADICIONALESNOREMUNERATIVOS = d.AdicionalesNORemunerativos,
                                DESCUENTOS = d.Descuentos,
                                TOTALNETO = d.TotalNeto
                            };

                DatosConsultados = datos;


                gvDatosSueldos.DataSource = DatosConsultados;


            }
            else
            {
                gvDatosSueldos.DataSource = DatosConsultados;
            }
        }

    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        DatosConsultados = null;
        this.gvDatosSueldos.Rebind();
        this.upGrilla.Update();
    }

    protected void gvDatosSueldos_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "ExportEmpresas")
        {
            ConfigureExportAndExport();
        }

    }

    public void ConfigureExportAndExport()
    {
        foreach (Telerik.Web.UI.GridColumn column in gvDatosSueldos.MasterTableView.Columns)
        {
            if (!column.Visible)
            {
                column.Visible = true;
            }
        }

        gvDatosSueldos.ExportSettings.ExportOnlyData = true;
        gvDatosSueldos.ExportSettings.IgnorePaging = true;
        gvDatosSueldos.ExportSettings.FileName = "DatosSueldos_Periodo_" + string.Format("{0:MMM_yyyy}", txtPeriodo.SelectedDate.Value);
        gvDatosSueldos.MasterTableView.ExportToExcel();

    }
}
