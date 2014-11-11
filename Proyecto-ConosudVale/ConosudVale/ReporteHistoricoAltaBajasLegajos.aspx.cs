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

public partial class ReporteHistoricoAltaBajasLegajos : System.Web.UI.Page
{

    public class TempHistoricoLegajos
    {
        public long IdContratoEmpresa { get; set; }
        public long IdLegajos { get; set; }
        public DateTime PrimerPeriodo { get; set; }
        public DateTime UltimoPeriodo { get; set; }
        public string NombreEmpresaContratista { get; set; }
        public string NombreEmpresaSubContratista { get; set; }
        public string NroContrato { get; set; }
        public bool EsContratista { get; set; }
        public string NroDocumento { get; set; }
        public string NombreLegajo { get; set; }
        public string PrimerPeriodoString { get; set; }
        public string UltimoPeriodoString { get; set; }
        public DateTime? FechaTramiteBaja { get; set; }
    }

    public List<TempHistoricoLegajos> _TempHistoricoLegajos
    {
        //get
        //{
        //    if (Session["TempHistoricoLegajos"] != null)
        //        return (List<TempHistoricoLegajos>)Session["TempHistoricoLegajos"];
        //    else
        //    {
        //        Session.Add("LegajosEncontrados", new List<AltasBajasLegajosTemp>());
        //        return (List<TempHistoricoLegajos>)Session["TempHistoricoLegajos"];
        //    }
        //}

        get
        {
            if (Session["TempHistoricoLegajos"] != null)

                return (List<TempHistoricoLegajos>)Helper.DeSerializeObject(Session["TempHistoricoLegajos"], typeof(List<TempHistoricoLegajos>));
            else
            {
                return (List<TempHistoricoLegajos>)Helper.DeSerializeObject(Session["TempHistoricoLegajos"], typeof(List<TempHistoricoLegajos>));
            }
        }
        set
        {
            Session["TempHistoricoLegajos"] = Helper.SerializeObject(value);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        GridAltaBajas.ExportToExcel += new ControlsAjaxNotti.ClickEventHandler(GridAltaBajas_ExportToExcel);
    }

    void GridAltaBajas_ExportToExcel(object sender)
    {
        GridAltaBajas.ExportToExcelFunction("AltaBajasHistoricoLegajos", _TempHistoricoLegajos);
    }

    [WebMethod(EnableSession = true)]
    public static object GetData(string nroDoc)
    {

        #region Recupero los Datos
        EntidadesConosud dc = new EntidadesConosud();


        var historial = (from u in dc.ContEmpLegajos
                         where u.Legajos.NroDoc == nroDoc
                         group u by new { u.IdContratoEmpresas, u.IdLegajos } into g
                         select new TempHistoricoLegajos
                         {
                             IdContratoEmpresa = g.Key.IdContratoEmpresas.Value,
                             IdLegajos = g.Key.IdLegajos.Value,
                             PrimerPeriodo = g.FirstOrDefault().CabeceraHojasDeRuta.Periodo,
                             UltimoPeriodo = g.Any(w => w.FechaTramiteBaja != null) ? g.Where(w => w.FechaTramiteBaja != null).FirstOrDefault().CabeceraHojasDeRuta.Periodo : DateTime.Now,
                             NroContrato = g.FirstOrDefault().ContratoEmpresas.Contrato.Codigo,
                             NombreEmpresaContratista = g.FirstOrDefault().ContratoEmpresas.Contrato.ContratoEmpresas.Where(w => w.EsContratista.Value).FirstOrDefault().Empresa.RazonSocial,
                             NombreEmpresaSubContratista = g.FirstOrDefault().ContratoEmpresas.EsContratista.Value == false ? g.FirstOrDefault().ContratoEmpresas.Empresa.RazonSocial : "",
                             EsContratista = g.FirstOrDefault().ContratoEmpresas.EsContratista.Value,
                             NombreLegajo = g.FirstOrDefault().Legajos.Apellido.ToUpper() + ", " + g.FirstOrDefault().Legajos.Nombre.ToUpper(),
                             NroDocumento = g.FirstOrDefault().Legajos.NroDoc,
                             FechaTramiteBaja = g.Where(w => w.FechaTramiteBaja != null).FirstOrDefault().FechaTramiteBaja
                         }).ToList();


        foreach (var item in historial)
        {
            if (item.UltimoPeriodo.Date == DateTime.Now.Date)
            {
                /// Recurpero el ultimo periodo del contrato.
                item.UltimoPeriodo = (from u in dc.ContEmpLegajos
                                      where u.IdContratoEmpresas == item.IdContratoEmpresa
                                      select u).ToList().LastOrDefault().CabeceraHojasDeRuta.Periodo;
            }

            item.NombreLegajo = item.NombreLegajo;
            item.PrimerPeriodoString = string.Format("{0:MM/yyyy}", item.PrimerPeriodo);
            item.UltimoPeriodoString = string.Format("{0:MM/yyyy}", item.UltimoPeriodo);
        }

        HttpContext.Current.Session["TempHistoricoLegajos"] = Helper.SerializeObject(historial.ToList()); 
        return historial.ToList();
        #endregion
    }
}