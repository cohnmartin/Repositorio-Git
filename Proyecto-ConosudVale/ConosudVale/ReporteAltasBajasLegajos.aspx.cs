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

public partial class ReporteAltasBajasLegajos : System.Web.UI.Page
{
    public List<AltasBajasLegajosTemp> _LegajosEncontrados
    {
        //get
        //{
        //    if (Session["LegajosEncontrados"] != null)
        //        return (List<AltasBajasLegajosTemp>)Session["LegajosEncontrados"];
        //    else
        //    {
        //        Session.Add("LegajosEncontrados", new List<AltasBajasLegajosTemp>());
        //        return (List<AltasBajasLegajosTemp>)Session["LegajosEncontrados"];
        //    }
        //}

         get
        {
            if (Session["LegajosEncontrados"] != null)

                return (List<AltasBajasLegajosTemp>)Helper.DeSerializeObject(Session["LegajosEncontrados"], typeof(List<AltasBajasLegajosTemp>));
            else
            {
                return (List<AltasBajasLegajosTemp>)Helper.DeSerializeObject(Session["LegajosEncontrados"], typeof(List<AltasBajasLegajosTemp>));
            }
        }
        set
        {
            Session["LegajosEncontrados"] = Helper.SerializeObject(value);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        GridAltaBajas.ExportToExcel += new ControlsAjaxNotti.ClickEventHandler(GridAltaBajas_ExportToExcel);

        if (!IsPostBack)
        {
            List<AltasBajasLegajosTemp> datos = (GetData(string.Format("{0:MM/yyyy}", DateTime.Now), 0, GridAltaBajas.PageSize) as List<AltasBajasLegajosTemp>);
            GridAltaBajas.VirtualCount = datos.Count; // _LegajosEncontrados.Count;
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
        //GridAltaBajas.ExportToExcelFunction("AltaBajasRecursos", _LegajosEncontrados);
        GridAltaBajas.ExportToExcelFunction("AltaBajasRecursos", (GetData(cboPeriodos.Text, 0, 100000) as List<AltasBajasLegajosTemp>));
        
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
        var empActuales = (from emp in dc.ContEmpLegajos
                           where ((emp.CabeceraHojasDeRuta.Periodo >= FechaInicial &&
                           emp.CabeceraHojasDeRuta.Periodo < FechaFinal) || (emp.FechaTramiteBaja >= FechaInicial && emp.FechaTramiteBaja < FechaFinal))
                           && emp.Legajos != null
                           select new
                           {
                               idcontempleg = emp.IdContEmpLegajos,
                               idCab = emp.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta,
                               idLeg = emp.Legajos.IdLegajos != null ? emp.Legajos.IdLegajos : 0,
                               idConEmp = emp.ContratoEmpresas.IdContratoEmpresas,
                               idCon = emp.ContratoEmpresas.Contrato.IdContrato,
                               NombreCompleto = emp.Legajos.Apellido.ToUpper() + ", " + emp.Legajos.Nombre.ToUpper(),
                               Nrodoc = emp.Legajos.NroDoc,
                               CodigoContrato = emp.ContratoEmpresas.Contrato.Codigo,
                               NombreEmpresa = emp.ContratoEmpresas.Empresa.RazonSocial,
                               Encuadre = emp.Legajos.objConvenio,
                               FechaBaja = emp.CabeceraHojasDeRuta.Periodo,
                               FechaTramite = emp.FechaTramiteBaja,
                               EsContratista = emp.ContratoEmpresas.EsContratista,
                               VencimientoContrato = emp.ContratoEmpresas.Contrato.Prorroga.HasValue ? emp.ContratoEmpresas.Contrato.Prorroga.Value : emp.ContratoEmpresas.Contrato.FechaVencimiento.Value

                           }).OrderBy(w => w.idCab).ToList();


        // PA: Busco los legajos del Periodo Anterior
        var empPA = (from emp in dc.ContEmpLegajos
                     where emp.CabeceraHojasDeRuta.Periodo >= FechaInicioPA &&
                     emp.CabeceraHojasDeRuta.Periodo < FechaFinalPA &&
                       (
                            (emp.ContratoEmpresas.Contrato.FechaVencimiento > FechaFinalPA)
                            ||
                            (emp.ContratoEmpresas.Contrato.Prorroga.HasValue && emp.ContratoEmpresas.Contrato.Prorroga > FechaFinalPA)
                       )
                       && emp.Legajos != null
                     select new
                     {
                         idCab = emp.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta,
                         idLeg = emp.Legajos.IdLegajos != null ? emp.Legajos.IdLegajos : 0,
                         idConEmp = emp.ContratoEmpresas.IdContratoEmpresas,
                         idCon = emp.ContratoEmpresas.Contrato.IdContrato,
                         NombreCompleto = emp.Legajos.Apellido.ToUpper() + ", " + emp.Legajos.Nombre.ToUpper(),
                         Nrodoc = emp.Legajos.NroDoc,
                         CodigoContrato = emp.ContratoEmpresas.Contrato.Codigo,
                         NombreEmpresa = emp.ContratoEmpresas.Empresa.RazonSocial,
                         Encuadre = emp.Legajos.objConvenio,
                         FechaBaja = emp.CabeceraHojasDeRuta.Periodo,
                         FechaTramite = emp.FechaTramiteBaja,
                         EsContratista = emp.ContratoEmpresas.EsContratista

                     }).ToList();

        List<AltasBajasLegajosTemp> LegajosEncontrados = new List<AltasBajasLegajosTemp>();
        int uniqueId = 0;

        foreach (var item in empActuales.Where(emp => (emp.FechaTramite >= FechaInicial && emp.FechaTramite < FechaFinal)))
        {

            AltasBajasLegajosTemp LegajoEliminado = new AltasBajasLegajosTemp();
            LegajoEliminado.Accion = "Baja";
            LegajoEliminado.CodigoContrato = item.CodigoContrato;
            LegajoEliminado.NombreCompleto = item.NombreCompleto.ToUpper();
            LegajoEliminado.NombreEmpresaContratista = Capitalize(item.NombreEmpresa.ToLower());
            LegajoEliminado.Nrodoc = item.Nrodoc;
            LegajoEliminado.UniqueID = uniqueId;
            LegajoEliminado.Periodo = periodo;

            if (item.Encuadre != null)
                LegajoEliminado.Encuadre = item.Encuadre.Descripcion;
            else
                LegajoEliminado.Encuadre = "";

            if (item.FechaTramite.HasValue)
            {
                LegajoEliminado.FechaTramite = item.FechaTramite.Value.ToShortDateString();
                LegajoEliminado.FechaBaja = string.Format("{0:MMM-yy}", item.FechaBaja);
            }
            else
            {
                LegajoEliminado.FechaTramite = "";
                LegajoEliminado.FechaBaja = "";
            }

            if (!item.EsContratista.HasValue || !item.EsContratista.Value)
            {
                LegajoEliminado.NombreEmpresaSubContratista = Capitalize(item.NombreEmpresa.ToLower());
                LegajoEliminado.NombreEmpresaContratista = Capitalize((from c in dc.ContratoEmpresas
                                                                       where c.Contrato.IdContrato == item.idCon
                                                                       && c.EsContratista.Value
                                                                       select new
                                                                       {
                                                                           Contratista = c.Empresa.RazonSocial
                                                                       }).First().Contratista.ToLower());
            }
            else
            {
                LegajoEliminado.NombreEmpresaSubContratista = "";
            }

            LegajosEncontrados.Add(LegajoEliminado);
            uniqueId++;
        }


        /// Solo se verifican los legajos que no han sido dado de baja ya que nunca
        /// se pueden dar de baja y de alta en el mismo periodo y contrato.
        foreach (var item in empActuales.Where(w => w.FechaTramite == null))
        {

            /// Si el legajo del periodo actual no existe en el periodo anterior 
            /// se infiere que el mismo fue dado de alta en el perio actual.
            int existe = (from emp in empPA
                          where emp.idConEmp == item.idConEmp
                          && emp.idLeg == item.idLeg
                          select emp).Count();

            if (existe == 0)
            {
                AltasBajasLegajosTemp LegajoEliminado = new AltasBajasLegajosTemp();
                LegajoEliminado.Accion = "Alta";
                LegajoEliminado.CodigoContrato = item.CodigoContrato;
                LegajoEliminado.NombreCompleto = item.NombreCompleto.ToUpper();
                LegajoEliminado.NombreEmpresaContratista = Capitalize(item.NombreEmpresa.ToLower());
                LegajoEliminado.Nrodoc = item.Nrodoc;
                LegajoEliminado.FechaTramite = "";
                LegajoEliminado.FechaBaja = "";
                LegajoEliminado.UniqueID = uniqueId;
                LegajoEliminado.Periodo = periodo;

                if (item.Encuadre != null)
                    LegajoEliminado.Encuadre = item.Encuadre.Descripcion;
                else
                    LegajoEliminado.Encuadre = "";


                if (!item.EsContratista.HasValue || !item.EsContratista.Value)
                {
                    LegajoEliminado.NombreEmpresaSubContratista = Capitalize(item.NombreEmpresa.ToLower());
                    LegajoEliminado.NombreEmpresaContratista = Capitalize((from c in dc.ContratoEmpresas
                                                                           where c.Contrato.IdContrato == item.idCon
                                                                           && c.EsContratista.Value
                                                                           select new
                                                                           {
                                                                               Contratista = c.Empresa.RazonSocial
                                                                           }).First().Contratista.ToLower());
                }
                else
                {
                    LegajoEliminado.NombreEmpresaSubContratista = "";
                }


                LegajosEncontrados.Add(LegajoEliminado);
                uniqueId++;
            }
        }

        //if (LegajosEncontrados.Count > 0)
        //{
        //    HttpContext.Current.Session["LegajosEncontrados"] = Helper.SerializeObject(LegajosEncontrados.ToList());
        //}
        //else
        //{
        //    HttpContext.Current.Session["LegajosEncontrados"] = null;
        //}

        return LegajosEncontrados.Skip(start).Take(take).ToList();
        #endregion
    }
}
