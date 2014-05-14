using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Entidades;

public partial class ConsultaHojadeRuta : System.Web.UI.Page
{

    public class TempHojaRuta
    {

        public Entidades.CabeceraHojasDeRuta CabeceraHojasDeRuta { get; set; }
        public Entidades.ContratoEmpresas ContratoEmp { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaInicio { get; set; }
        public string Estado { get; set; }
        public int NroCarpeta { get; set; }
        public DateTime Periodo { get; set; }
        public bool? EsFueraTermino { get; set; }
        public Entidades.Empresa Empresa { get; set; }
        public bool EsContratista { get; set; }
        public long IdCabeceraHojasDeRuta { get; set; }
        public string ConstratistaParaSubConstratista { get; set; }

    }

    private List<TempHojaRuta> ResultadoConsulta
    {
        get {
            return (List<TempHojaRuta>)Session["ResultadoConsulta"];
        }
        set
        {
            if (Session["ResultadoConsulta"] == null)
                Session.Add("ResultadoConsulta", value);
            else
                Session["ResultadoConsulta"] = value;
        }
    
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtInicial.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            txtFinal.SelectedDate = DateTime.Now;
            CargarGrilla();
        }

        gvHojadeRuta.DataBound += new EventHandler(gvHojadeRuta_DataBound);
    }

    void gvHojadeRuta_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Visor, idUsuario);

        if (PermisosPagina.Lectura && !PermisosPagina.Creacion && !PermisosPagina.Modificacion)
        {
            gvHojadeRuta.MasterTableView.GetColumn("AuditarColumn").Visible = false;
        }

    }

    protected void gvHojadeRuta_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        CargarGrilla();
    }



    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {
            ResultadoConsulta = new List<TempHojaRuta>(); 
            CargarGrilla();
            this.gvHojadeRuta.DataBind();
            ScriptManager.RegisterStartupScript(upGrilla, typeof(UpdatePanel), "sss", "ShowResult();", true);
        }
        catch (Exception ex)
        {

        }
    }

    private void CargarGrilla()
    {
        DateTime? desde = this.txtInicial.SelectedDate.Value.Date;
        DateTime? hasta = this.txtFinal.SelectedDate.Value.Date.AddHours(23);

        EntidadesConosud _dc = new EntidadesConosud();

        if (ResultadoConsulta== null || ResultadoConsulta.Count == 0)
        {

            ResultadoConsulta = (from cab in _dc.CabeceraHojasDeRuta
                                 where cab.HojasDeRuta.Any(w => w.DocFechaEntrega >= desde && w.DocFechaEntrega <= hasta)
                                  && cab.FechaAprobacion == null
                                  && cab.ContratoEmpresas.Empresa != null
                                  && cab.ContratoEmpresas.Contrato != null
                                 orderby cab.ContratoEmpresas.Empresa.RazonSocial
                                 , cab.ContratoEmpresas.Contrato.Codigo
                                 select new TempHojaRuta
                                 {
                                     CabeceraHojasDeRuta = cab,
                                     ContratoEmp = cab.ContratoEmpresas,
                                     Codigo = cab.ContratoEmpresas.Contrato.Codigo,
                                     FechaInicio = cab.ContratoEmpresas.Contrato.FechaInicio.Value,
                                     Estado = cab.Estado.Descripcion,
                                     NroCarpeta = cab.NroCarpeta,
                                     Periodo = cab.Periodo,
                                     Empresa = cab.ContratoEmpresas.Empresa,
                                     EsContratista = cab.ContratoEmpresas.EsContratista.Value,
                                     EsFueraTermino = cab.EsFueraTermino,
                                     IdCabeceraHojasDeRuta = cab.IdCabeceraHojasDeRuta,
                                     ConstratistaParaSubConstratista = ""
                                 }).ToList();



            //ResultadoConsulta = (from hoja in _dc.HojasDeRuta
            //                     where hoja.DocFechaEntrega >= desde && hoja.DocFechaEntrega <= hasta
            //                      && hoja.CabeceraHojasDeRuta.FechaAprobacion == null
            //                      && hoja.CabeceraHojasDeRuta.ContratoEmpresas.Empresa != null
            //                      && hoja.CabeceraHojasDeRuta.ContratoEmpresas.Contrato != null
            //                     orderby hoja.CabeceraHojasDeRuta.ContratoEmpresas.Empresa.RazonSocial
            //                     , hoja.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.Codigo
            //                     select new TempHojaRuta
            //                     {
            //                         CabeceraHojasDeRuta = hoja.CabeceraHojasDeRuta,
            //                         ContratoEmp = hoja.CabeceraHojasDeRuta.ContratoEmpresas,
            //                         Codigo = hoja.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.Codigo,
            //                         FechaInicio = hoja.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.FechaInicio.Value,
            //                         Estado = hoja.CabeceraHojasDeRuta.Estado.Descripcion,
            //                         NroCarpeta = hoja.CabeceraHojasDeRuta.NroCarpeta,
            //                         Periodo = hoja.CabeceraHojasDeRuta.Periodo,
            //                         EsFueraTermino = hoja.CabeceraHojasDeRuta.EsFueraTermino.Value,
            //                         Empresa = hoja.CabeceraHojasDeRuta.ContratoEmpresas.Empresa,
            //                         EsContratista = hoja.CabeceraHojasDeRuta.ContratoEmpresas.EsContratista.Value,
            //                         IdCabeceraHojasDeRuta = hoja.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta,
            //                         ConstratistaParaSubConstratista = ""
            //                     }).ToList();

            foreach (var hoja in ResultadoConsulta)
            {
                hoja.ConstratistaParaSubConstratista = hoja.CabeceraHojasDeRuta.ContratoEmpresas.ConstratistaParaSubConstratista;
            }

        }

       

        /// Esta es la forma de hacer un distinct sobre una colección de objetos 
        /// anonimos.
        var itemsHojaSimple = (from I in ResultadoConsulta
                               group I by I.IdCabeceraHojasDeRuta into g
                               let cab = g.FirstOrDefault()
                               select cab).ToList();


        this.gvHojadeRuta.DataSource = itemsHojaSimple;


        if (ResultadoConsulta.Count() > 0)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "showinicial", "ShowResult();", true);
        }
    }

    protected void gvHojadeRuta_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {

            if (bool.Parse(e.Item.Cells[2].Text))
                (e.Item.FindControl("lblSubContratista") as Label).Text = "-";
            else
                (e.Item.FindControl("lblContratista") as Label).Text = e.Item.Cells[3].Text;

        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        CargarGrilla();
        ConfigureExport();
        gvHojadeRuta.Rebind();
        gvHojadeRuta.MasterTableView.ExportToExcel();


    }

    public void ConfigureExport()
    {
        gvHojadeRuta.ExportSettings.ExportOnlyData = true;
        gvHojadeRuta.ExportSettings.IgnorePaging = true;
        gvHojadeRuta.ExportSettings.OpenInNewWindow = true;
    }

}
