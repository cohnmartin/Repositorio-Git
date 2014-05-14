using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using Entidades;
using System.Web.Services;
using System.Collections;

public partial class ReporteAltaModificacionVehiculoEquipos : System.Web.UI.Page
{
    public List<AltaModificaionVehiculosEquipos> _VehiculosEquiposEncontrados
    {
        get
        {
            if (Session["VehiculosEquiposEncontrados"] != null)
                return (List<AltaModificaionVehiculosEquipos>)Session["VehiculosEquiposEncontrados"];
            else
            {
                Session.Add("VehiculosEquiposEncontrados", new List<AltaModificaionVehiculosEquipos>());
                return (List<AltaModificaionVehiculosEquipos>)Session["VehiculosEquiposEncontrados"];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        GridAltaBajas.ExportToExcel += new ControlsAjaxNotti.ClickEventHandler(GridAltaBajas_ExportToExcel);

        if (!IsPostBack)
        {
            IList datos = (GetData(string.Format("{0:MM/yyyy}", DateTime.Now), 0, GridAltaBajas.PageSize) as IList);
            GridAltaBajas.VirtualCount = _VehiculosEquiposEncontrados.Count;
            GridAltaBajas.DataSource = datos;



            DateTime fechaInicial = DateTime.Now;
            for (int i = 0; i < 25; i++)
            {
                DateTime fechaActual = fechaInicial.AddMonths(-1 * i);
                string FechaFormat = string.Format("{0:MM/yyyy}", fechaActual);
                cboPeriodos.Items.Add(new ListItem(FechaFormat, FechaFormat));
            }


        }
    }

    void GridAltaBajas_ExportToExcel(object sender)
    {
        GridAltaBajas.ExportToExcelFunction("AltaModificaionVehiculosEquipos", (List<AltaModificaionVehiculosEquipos>)Session["VehiculosEquiposEncontrados"]);
    }

    public static string Capitalize(string value)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
    }

    [WebMethod(EnableSession = true)]
    public static object GetData(string periodo, int start, int take)
    {

        #region Recupero los Datos
        EntidadesConosud dc = new EntidadesConosud();

        DateTime FechaInicial = Convert.ToDateTime("01/" + periodo);
        DateTime FechaFinal = Convert.ToDateTime("01/" + periodo).AddMonths(1);

        DateTime FechaInicioPA = FechaInicial.AddMonths(-1);
        DateTime FechaFinalPA = FechaFinal.AddMonths(-1);



        // Empleados del period actual, es decir del seleccionado
        var VehiculosActuales = (from v in dc.VahiculosyEquipos
                                 where ((v.FechaCreacion >= FechaInicial &&
                                 v.FechaCreacion < FechaFinal) || (v.FechaUltimaActualizacion >= FechaInicial && v.FechaUltimaActualizacion < FechaFinal))
                                  && v.Empresa.HasValue && v.objContrato != null
                                 select new AltaModificaionVehiculosEquipos
                                 {
                                     idVehiculo = v.IdVehiculoEquipo,
                                     Patente = v.Patente,
                                     Marca = v.Marca,
                                     FechaCreacion = v.FechaCreacion,
                                     FechaActualizacion = v.FechaUltimaActualizacion,
                                     Accion = v.FechaCreacion == v.FechaUltimaActualizacion ? "Alta" : "Modificacion",
                                     Tipo = v.Tipo,
                                     idCon = v.objContrato != null ? v.objContrato.IdContrato : 0,
                                     CodigoContrato = v.objContrato.Codigo,
                                     NombreEmpresa = v.objEmpresa.RazonSocial,
                                     EsContratista = v.objEmpresa != null ? v.objContrato.ContratoEmpresas.Where(w => w.Empresa.IdEmpresa == v.objEmpresa.IdEmpresa).FirstOrDefault().EsContratista.Value : false,
                                     Periodo = periodo
                                 }).ToList();



        if (VehiculosActuales.Count > 0)
        {
            HttpContext.Current.Session["VehiculosEquiposEncontrados"] = VehiculosActuales.ToList();
        }
        else
        {
            HttpContext.Current.Session["VehiculosEquiposEncontrados"] = null;
        }


        return VehiculosActuales.Skip(start).Take(take).ToList();
        #endregion
    }
}