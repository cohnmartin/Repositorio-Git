using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Script.Services;
using System.IO;


using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class GestionHojadeRuta : System.Web.UI.Page
{

    public class itsEvents : iTextSharp.text.pdf.PdfPageEventHelper
    {
        private int pageCount = 0;
        private iTextSharp.text.pdf.PdfTemplate moTemplate;
        private iTextSharp.text.pdf.PdfContentByte moCB;
        private iTextSharp.text.pdf.BaseFont moBF;
        public string direccion;

        public override void OnStartPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            //pageCount++;
            //iTextSharp.text.Font fontCalibri11 = iTextSharp.text.FontFactory.GetFont("Calibri", 10);

            //iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(3);
            //ptable.DefaultCell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            //ptable.DefaultCell.PaddingLeft = 0;
            //ptable.DefaultCell.PaddingTop = 0;
            //ptable.DefaultCell.PaddingBottom = 10;
            //ptable.DefaultCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

            //ptable.AddCell(new iTextSharp.text.Phrase(param.NombreEmpresa, fontCalibri11));
            //ptable.AddCell(new iTextSharp.text.Phrase(CurrentFumigacion.CamaraFumigacion, fontCalibri11));
            //ptable.AddCell(new iTextSharp.text.Phrase(CurrentFumigacion.NroFumigacion, fontCalibri11));

            //document.Add(ptable);

        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {


            iTextSharp.text.Font fontTimes = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10);

            iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(2);
            ptable.DefaultCell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            ptable.DefaultCell.PaddingLeft = 0;
            ptable.DefaultCell.PaddingTop = 0;
            ptable.DefaultCell.PaddingBottom = 8;
            ptable.DefaultCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            ptable.TotalWidth = document.PageSize.Width;

            ptable.AddCell(new iTextSharp.text.Phrase("Fecha Impresión:" + DateTime.Now.ToShortDateString(), fontTimes));
            ptable.AddCell(new iTextSharp.text.Phrase("Firma Responsable de la Empresa", fontTimes));

            //iTextSharp.text.pdf.PdfPCell cellTitFum = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Omar Vizzari - Versión 2.0", fontTimes));
            //cellTitFum.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            //cellTitFum.BorderColor = iTextSharp.text.BaseColor.WHITE;
            //cellTitFum.PaddingLeft = 10;
            //ptable.AddCell(cellTitFum);


            iTextSharp.text.pdf.PdfPCell cellTitFum = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Página " + document.PageNumber + " de ", fontTimes));
            cellTitFum.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            cellTitFum.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cellTitFum.PaddingRight = 10;
            cellTitFum.Colspan = 2;
            ptable.AddCell(cellTitFum);


            cellTitFum = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(direccion, fontTimes));
            cellTitFum.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            cellTitFum.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cellTitFum.PaddingRight = 0;
            cellTitFum.Colspan = 2;
            ptable.AddCell(cellTitFum);


            ptable.WriteSelectedRows(0, -1, 0, document.BottomMargin - 30, writer.DirectContent);

            /// Agrego un template en una posicion especifica para luego escribir el
            /// nro total de paganias del archivo.
            moCB.AddTemplate(moTemplate, (document.PageSize.Width / 2) + 24, 40);



        }

        public override void OnOpenDocument(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            moBF = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.TIMES_ROMAN, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
            moCB = writer.DirectContent;
            moTemplate = moCB.CreateTemplate(70, 50);

        }

        public override void OnCloseDocument(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            moTemplate.BeginText();
            moTemplate.SetFontAndSize(moBF, 10);
            moTemplate.ShowText(Convert.ToString(writer.PageNumber - 1));
            moTemplate.EndText();

        }
    }

    #region Variables
    EntidadesConosud dc = null;
    private bool _tieneLegajo = false;
    private DateTime _periodoActual;
    private string _CodigoContrato;
    private string _comentarioGralIngresado = "";
    private EntidadesConosud _Contexto;
    private TempCabecera _CurrentTempCabecera;
    #endregion

    #region Propiedades

    public class TempSueldos
    {
        public long IdLegajos { get; set; }
        public string NombreCompleto { get; set; }
        public string NroDoc { get; set; }
        public string Convenio { get; set; }
        public bool PoseeInfoSueldo { get; set; }
        public DateTime? FechaTramiteBaja { get; set; }
        public string CUIT { get; set; }
        public string Provincia { get; set; }
        public DateTime PeriodoAfecatacion { get; set; }
        public DateTime? UltimoExamen { get; set; }
    }


    public class TempCabecera
    {
        public long IdCaebcera { get; set; }
        public string Codigo { get; set; }
        public string Servicio { get; set; }
        public DateTime Periodo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? Prorroga { get; set; }
        public bool? EsContratista { get; set; }
        public string Contratista { get; set; }
        public string SubContratista { get; set; }
        public string LoginName { get; set; }
        public DateTime UltimoPeriodo { get; set; }
        public string Aprobada { get; set; }
        public string Estimacion { get; set; }
        public Entidades.ContratoEmpresas ContratoEmpresas { get; set; }
        public Entidades.CabeceraHojasDeRuta objCabecera { get; set; }
        public string Categoria { get; set; }

    }

    public class TempItemsHoja
    {
        public long IdHoja { get; set; }
        public long IdCabecera { get; set; }
        public long IdContratoEmpresa { get; set; }
        public long IdPlantilla { get; set; }


        public string AuditadoPor { get; set; }
        public string HojaComentario { get; set; }
        public DateTime? DocFechaEntrega { get; set; }
        public object ComentarioGeneral { get; set; }


        public string Descripcion { get; set; }
        public string DocComentario { get; set; }
        public DateTime? HojaFechaAprobacion { get; set; }
        public bool? AuditoriaTerminada { get; set; }


    }

    public class TempComentarioGrales
    {
        public long IdComentarioGel { get; set; }
        public long IdPlantilla { get; set; }
        public string Comentario { get; set; }
    }

   

    public List<TempSueldos> InfoSueldos
    {
        get
        {
            if (Session["InfoSueldos"] != null)

                return (List<TempSueldos>)Helper.DeSerializeObject(Session["InfoSueldos"], typeof(List<TempSueldos>));
            else
            {
                return (List<TempSueldos>)Helper.DeSerializeObject(Session["InfoSueldos"], typeof(List<TempSueldos>));
            }
        }
        set
        {
            Session["InfoSueldos"] = Helper.SerializeObject(value);
        }
    }

    public List<TempItemsHoja> ItemsHojasDeRuta
    {
        get
        {
            if (Session["ItemsHojasDeRuta"] != null)
                return (List<TempItemsHoja>)Helper.DeSerializeObject(Session["ItemsHojasDeRuta"], typeof(List<TempItemsHoja>));
            else
            {
                return (List<TempItemsHoja>)Helper.DeSerializeObject(Session["ItemsHojasDeRuta"], typeof(List<TempItemsHoja>));
            }
        }
        set
        {
            Session["ItemsHojasDeRuta"] = Helper.SerializeObject(value);
        }
    }

    public List<TempComentarioGrales> ComentariosGrales
    {
        get
        {
            if (Session["ComentariosGrales"] != null)
                return (List<TempComentarioGrales>)Helper.DeSerializeObject(Session["ComentariosGrales"], typeof(List<TempComentarioGrales>));
            else
            {
                return (List<TempComentarioGrales>)Helper.DeSerializeObject(Session["ComentariosGrales"], typeof(List<TempComentarioGrales>));
            }
        }
        set
        {
            Session["ComentariosGrales"] = Helper.SerializeObject(value);
        }
    }

    public EntidadesConosud Contexto
    {
        //get
        //{
        //    if (Session["Contexto"] != null)
        //        return (EntidadesConosud)Session["Contexto"];
        //    else
        //    {
        //        Session["Contexto"] = new EntidadesConosud();
        //        return (EntidadesConosud)Session["Contexto"];
        //    }
        //}

        get
        {

            if (_Contexto == null)
            {
                _Contexto = new EntidadesConosud();
                return _Contexto;
            }
            else
                return _Contexto;
        }
    }

    public string IdCabecera
    {
        get { return Request.QueryString["IdCabecera"].ToString(); }
    }

    public string IdUsuario
    {
        get { return ((Entidades.SegUsuario)this.Session["usuario"]).IdSegUsuario.ToString(); }
    }

    public bool TieneLegajos
    {
        get { return _tieneLegajo; }
        set { _tieneLegajo = value; }
    }
    #endregion

    #region Eventos
    protected void Page_Load(object sender, EventArgs e)
    {
        gvItemHoja.NeedDataSource += new GridNeedDataSourceEventHandler(gvItemHoja_NeedDataSource);

        if (!IsPostBack)
        {
            (Page.Master as DefaultMasterPage).OcultarEncabezado();
            Session["Contexto"] = null;
            CargarDatosSession();

            gvItemHoja.DataSource = ItemsHojasDeRuta;
            gvItemHoja.DataBind();


            _periodoActual = _CurrentTempCabecera.Periodo;
            _CodigoContrato = _CurrentTempCabecera.Codigo;


            /// Deteccion de si es o no la ultima hoja de ruta
            //if (!CurrentTempCabecera.ContratoEmpresas.CabeceraHojasDeRuta.IsLoaded) { CurrentTempCabecera.ContratoEmpresas.CabeceraHojasDeRuta.Load(); }
            var cabeceras = from C in _CurrentTempCabecera.ContratoEmpresas.CabeceraHojasDeRuta
                            orderby C.Periodo
                            select C;

            if (cabeceras.Last().Periodo == _CurrentTempCabecera.Periodo
                ||
                cabeceras.Last().Periodo == _CurrentTempCabecera.Periodo.AddMonths(1))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "showAviso", "ShowAvisoUltimoCertificado();", true);
            }
            else
            {
                tdUltimoCertificado.Visible = false;
            }


            /// Comentado por el cambio de la exportación directa a PDF
            //gvLegajos.ItemDataBound += new GridItemEventHandler(gvLegajos_ItemDataBound);
            //gvLegajos.DataSource = InfoSueldos.ToList();
            //gvLegajos.DataBind();


            if (InfoSueldos.Count() > 0)
            {
                _tieneLegajo = true;
            }


            lblContrato.Text = _CurrentTempCabecera.Servicio;
            lblPeriodo.Text = string.Format("{0:MM/yyyy}", _CurrentTempCabecera.Periodo);

            //if (!CurrentTempCabecera.ContratoEmpresas.EmpresaReference.IsLoaded) { CurrentTempCabecera.ContratoEmpresas.EmpresaReference.Load(); }
            if (_CurrentTempCabecera.EsContratista.Value)
            {
                lblContratista.Text = _CurrentTempCabecera.ContratoEmpresas.Empresa.RazonSocial;
                lblSubContratista.Text = "-";
            }
            else
            {

                lblContratista.Text = _CurrentTempCabecera.ContratoEmpresas.ConstratistaParaSubConstratista;
                lblSubContratista.Text = _CurrentTempCabecera.ContratoEmpresas.Empresa.RazonSocial;
            }




            if (_CurrentTempCabecera.objCabecera.Aprobada.Contains("No"))
            {
                lblEstado.Text = _CurrentTempCabecera.objCabecera.Aprobada.ToUpper();
                lblEstado.Style.Add(HtmlTextWriterStyle.Color, "Red");
            }
            else
            {
                if (_CurrentTempCabecera.objCabecera.AprobacionEpecial.HasValue && _CurrentTempCabecera.objCabecera.AprobacionEpecial.Value)
                {
                    lblEstado.Text = "APROBADA (Sin Actividad)";
                }
                else
                {
                    lblEstado.Text = _CurrentTempCabecera.objCabecera.Aprobada.ToUpper();
                }
                lblEstado.Style.Add(HtmlTextWriterStyle.Color, "Blue");
                lblUsuarioAprobador.Text = "(" + _CurrentTempCabecera.LoginName + ")";
            }

            lblTitulo.Text = "Gestión Contrato Nro: " + _CurrentTempCabecera.Codigo + " - " + _CurrentTempCabecera.Categoria.ToUpper();
            txtEstimacion.Text = _CurrentTempCabecera.Estimacion;

            lblFechaIncial.Text = _CurrentTempCabecera.FechaInicio.ToShortDateString();

            if (!_CurrentTempCabecera.Prorroga.HasValue)
                lblFechaFinal.Text = _CurrentTempCabecera.FechaVencimiento.ToShortDateString();
            else
                lblFechaFinal.Text = _CurrentTempCabecera.Prorroga.Value.ToShortDateString();

            /// Verificacion de acceso por Rol Aprobador
            string DescRol = Helpers.RolesEspeciales.Aprobador.ToString();
            Entidades.SegUsuario usuario = (Entidades.SegUsuario)this.Session["usuario"];


            int RolesAdministrador = (from U in Contexto.SegUsuario
                                      from UR in U.SegUsuarioRol
                                      where U.IdSegUsuario == usuario.IdSegUsuario
                                      && UR.SegRol.Descripcion == DescRol
                                      select UR).Count();

            if (RolesAdministrador > 0)
            {
                trAprobacion.Visible = true;
                trEstimacion.Visible = true;
                trReporte.Visible = true;
                trDesaprobacion.Visible = true;

                /// si la hoja no tiene documentación recepcionada, ni se ha auditado entonces habilito la posibilidad
                /// de realizar la aprobación especial.
                //var hojasss = ItemsHojasDeRuta.Where(w => w.DocFechaEntrega.HasValue || w.HojaComentario != "" || w.HojaFechaAprobacion.HasValue).ToList();
                if (ItemsHojasDeRuta.Any(w => w.DocFechaEntrega.HasValue || w.HojaComentario != "" || w.HojaFechaAprobacion.HasValue))
                    trAprobacionEspecial.Visible = false;
                else
                    trAprobacionEspecial.Visible = true;
            }
            else
            {
                trAprobacion.Visible = false;
                trEstimacion.Visible = false;
                trAprobacionEspecial.Visible = false;
                trDesaprobacion.Visible = false;
            }

            if (_CurrentTempCabecera.Aprobada == "Aprobada")
            {
                trReporte.Visible = false;
            }
        }



    }

    protected void gvLegajos_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            TempSueldos contE = (TempSueldos)e.Item.DataItem;
            if (contE.PoseeInfoSueldo)
            {
                (e.Item.FindControl("lblInfoSueldos") as Label).Text = "Si";
            }
            else
            {
                (e.Item.FindControl("lblInfoSueldos") as Label).Text = "No";
            }

            (e.Item.FindControl("lblPeriodo") as Label).Text = string.Format("{0:MM/yyyy}", _periodoActual);
            (e.Item.FindControl("lblContrato") as Label).Text = _CodigoContrato;
            if (contE.FechaTramiteBaja.HasValue)
            {
                e.Item.Style.Add(HtmlTextWriterStyle.Color, "Red");
                e.Item.ToolTip = "Fecha Baja: " + contE.FechaTramiteBaja.Value.ToShortDateString();
            }

            //foreach (DatosDeSueldos sueldo in contE.Legajos.DatosDeSueldos)
            //{
            //    if (_periodoActual.Date == sueldo.Periodo.Value.Date)
            //    {
            //        (e.Item.FindControl("lblInfoSueldos") as Label).Text = "Si";
            //        string strToolTip = "";
            //        (e.Item.FindControl("lblInfoSueldos") as Label).ToolTip = strToolTip;
            //        return;
            //    }
            //}

            //(e.Item.FindControl("lblInfoSueldos") as Label).Text = "No";

        }
    }

    protected void gvItemHoja_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        gvItemHoja.DataSource = ItemsHojasDeRuta;
    }

    protected void gvItemHoja_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {

            TempItemsHoja hoja = (TempItemsHoja)e.Item.DataItem;

            long IdPlantillla = hoja.IdPlantilla;


            long[] rolesAsignados = (from R in Contexto.RolesPlanilla
                                     where R.IdPlanilla == IdPlantillla
                                     select R.SegRol.IdSegRol).Distinct<long>().ToArray<long>();


            Entidades.SegUsuario usuario = (Entidades.SegUsuario)this.Session["usuario"];

            //if (!usuario.SegUsuarioRol.IsLoaded) { usuario.SegUsuarioRol.Load(); }
            //foreach (Entidades.SegUsuarioRol item in usuario.SegUsuarioRol)
            //{
            //    if (!item.SegRolReference.IsLoaded) { item.SegRolReference.Load(); }
            //}

            int PoseeRol = (from R in usuario.SegUsuarioRol
                            where rolesAsignados.Contains(R.SegRol.IdSegRol)
                            select R).Count();




            if (PoseeRol == 0)
            {
                (e.Item.FindControl("chkAprobo") as CheckBox).Enabled = false;
                (e.Item.FindControl("imgEdit") as ImageButton).Visible = false;
                (e.Item.FindControl("chkTerminoAuditoria") as CheckBox).Enabled = false;
            }

            if (hoja.AuditadoPor != null && hoja.AuditadoPor.Trim() != "" && hoja.AuditadoPor != usuario.Login)
            {
                (e.Item.FindControl("chkAprobo") as CheckBox).Visible = false;
                (e.Item.FindControl("imagenAuditadoPor") as System.Web.UI.WebControls.Image).Visible = true;
                (e.Item.FindControl("chkTerminoAuditoria") as CheckBox).Enabled = false;
            }

            if (hoja.AuditadoPor != null && hoja.AuditadoPor.Trim() != "" && hoja.AuditadoPor == usuario.Login
                && hoja.HojaFechaAprobacion != null)
            {
                (e.Item.FindControl("chkAprobo") as CheckBox).Visible = false;
                (e.Item.FindControl("chkTerminoAuditoria") as CheckBox).Enabled = false;
            }

            if (hoja.AuditoriaTerminada.HasValue && hoja.AuditoriaTerminada.Value)
            {
                (e.Item.FindControl("chkTerminoAuditoria") as CheckBox).Checked = true;

                if (hoja.HojaFechaAprobacion.HasValue)
                {
                    (e.Item.FindControl("chkTerminoAuditoria") as CheckBox).Enabled = false;
                }
                else
                {
                    (e.Item.FindControl("chkTerminoAuditoria") as CheckBox).Enabled = true;
                }
            }



            if (hoja.HojaComentario == null || hoja.HojaComentario.Trim() == "")
            {
                (e.Item.FindControl("imgcomentarioitem") as System.Web.UI.WebControls.Image).Visible = false;
            }

            if (hoja.DocFechaEntrega != null)
            {
                (e.Item.FindControl("imgcomentariodoc") as System.Web.UI.WebControls.Image).Visible = true;
            }
            else
            {
                (e.Item.FindControl("imgcomentariodoc") as System.Web.UI.WebControls.Image).Visible = false;
            }




            TempComentarioGrales CurrentComentario = (from C in ComentariosGrales
                                                      where C.IdPlantilla == hoja.IdPlantilla
                                                      select C).FirstOrDefault<TempComentarioGrales>();


            if (CurrentComentario != null && CurrentComentario.Comentario != "")
                (e.Item.FindControl("imgcomentariogral") as System.Web.UI.WebControls.Image).Attributes.Add("coment", CurrentComentario.Comentario);
            else
                (e.Item.FindControl("imgcomentariogral") as System.Web.UI.WebControls.Image).Visible = false;


            /// Si el usuario es de una empresa, entonces oculto los comentarios generales
            if (!usuario.EmpresaReference.IsLoaded) { usuario.EmpresaReference.Load(); }
            if (usuario.Empresa != null)
            {
                gvItemHoja.Columns.FindByUniqueName("imgComentarioGral").Visible = false;
                gvItemHoja.Columns.FindByUniqueName("chkAproboColumn").Visible = false;

            }





        }

        if ((e.Item is GridEditableItem) && (e.Item.IsInEditMode))
        {
            TempItemsHoja hoja = (TempItemsHoja)e.Item.DataItem;
            GridEditableItem edititem = (GridEditableItem)e.Item;
            TextBox txtbx = (TextBox)edititem.FindControl("txtComentarioGralEdit");

            TempComentarioGrales CurrentComentario = (from C in ComentariosGrales
                                                      where C.IdPlantilla == hoja.IdPlantilla
                                                      select C).FirstOrDefault<TempComentarioGrales>();

            if (CurrentComentario != null)
                txtbx.Text = CurrentComentario.Comentario;
            else

                txtbx.Text = "";




            ((System.Web.UI.WebControls.Table)(((Telerik.Web.UI.GridEditFormItem)(edititem)).FormColumns[0])).Rows[0].Visible = false;
            ((System.Web.UI.WebControls.Table)(((Telerik.Web.UI.GridEditFormItem)(edititem)).FormColumns[0])).Rows[1].Cells[0].Text = "Comentario Item";
            ((System.Web.UI.WebControls.Table)(((Telerik.Web.UI.GridEditFormItem)(edititem)).FormColumns[0])).Rows[2].Cells[0].Text = "Comentario Gral";


        }



    }

    protected void gvItemHoja_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.UpdateCommandName)
        {
            if (e.Item is GridEditFormItem)
            {
                UpdateItemHojaRuta(e.Item as GridEditFormItem,
                    long.Parse(gvItemHoja.Items[e.Item.DataSetIndex].GetDataKeyValue("IdHoja").ToString()),
                    long.Parse(gvItemHoja.Items[e.Item.DataSetIndex].GetDataKeyValue("IdPlantilla").ToString()));

                gvItemHoja.MasterTableView.ClearEditItems();
            }

        }
    }

    //protected void EntityDataSourceHojas_Updating(object sender, EntityDataSourceChangingEventArgs e)
    //{
    //    Entidades.HojasDeRuta _hoja = (Entidades.HojasDeRuta)e.Entity;
    //    _hoja.AuditadoPor = (Session["usuario"] as Entidades.SegUsuario).Login;
    //    _hoja.HojaFechaControlado = DateTime.Now;

    //    if (!_hoja.PlantillaReference.IsLoaded) { _hoja.PlantillaReference.Load(); }
    //    if (!_hoja.CabeceraHojasDeRutaReference.IsLoaded) { _hoja.CabeceraHojasDeRutaReference.Load(); }
    //    if (!_hoja.CabeceraHojasDeRuta.ContratoEmpresasReference.IsLoaded) { _hoja.CabeceraHojasDeRuta.ContratoEmpresasReference.Load(); }
    //    if (!_hoja.CabeceraHojasDeRuta.ContratoEmpresas.ComentariosGral.IsLoaded) { _hoja.CabeceraHojasDeRuta.ContratoEmpresas.ComentariosGral.Load(); }

    //    foreach (ComentariosGral item in _hoja.CabeceraHojasDeRuta.ContratoEmpresas.ComentariosGral)
    //    {
    //        if (!item.PlantillaReference.IsLoaded) { item.PlantillaReference.Load(); }
    //    }


    //    var CurrentComentario = (from C in _hoja.CabeceraHojasDeRuta.ContratoEmpresas.ComentariosGral
    //                             where C.Plantilla.IdPlantilla == _hoja.Plantilla.IdPlantilla
    //                             select C).FirstOrDefault<ComentariosGral>();


    //    if (CurrentComentario == null)
    //    {
    //        ComentariosGral coment = new ComentariosGral();
    //        coment.Comentario = _comentarioGralIngresado;
    //        coment.ContratoEmpresas = _hoja.CabeceraHojasDeRuta.ContratoEmpresas;
    //        coment.Plantilla = _hoja.Plantilla;
    //        e.Context.AddObject("ComentariosGral", coment);
    //    }
    //    else
    //    {

    //        CurrentComentario.Comentario = _comentarioGralIngresado;
    //    }

    //    e.Context.SaveChanges();

    //}

    protected void btnAplicarEstimacion_Click(object sender, EventArgs e)
    {

        Entidades.CabeceraHojasDeRuta cab = (from C in Contexto.CabeceraHojasDeRuta
                                             where C.IdCabeceraHojasDeRuta == _CurrentTempCabecera.IdCaebcera
                                             select C).First();

        cab.Estimacion = txtEstimacion.Text;
        Contexto.SaveChanges();

    }

    protected void btnAplicar_Click(object sender, EventArgs arg)
    {
        long idcabecera = 0;
        foreach (GridItem item in gvItemHoja.Items)
        {

            long id = long.Parse(gvItemHoja.Items[item.DataSetIndex].GetDataKeyValue("IdHoja").ToString());

            Entidades.HojasDeRuta itemsHoja = (from H in Contexto.HojasDeRuta
                                               where H.IdHojaDeRuta == id
                                               select H).First<Entidades.HojasDeRuta>();

            idcabecera = itemsHoja.IdCabeceraHojaDeRuta;

            if ((item.FindControl("chkAprobo") as CheckBox).Checked)
            {
                itemsHoja.HojaFechaAprobacion = DateTime.Now;
                itemsHoja.HojaFechaControlado = DateTime.Now;
                itemsHoja.AuditadoPor = (Session["usuario"] as Entidades.SegUsuario).Login;
            }

            itemsHoja.AuditoriaTerminada = (item.FindControl("chkTerminoAuditoria") as CheckBox).Checked;

            (item.FindControl("chkAprobo") as CheckBox).Checked = false;

        }
        Contexto.SaveChanges();
        CargarDatosSession();
        gvItemHoja.Rebind();
        AprobacionAutomatica(idcabecera);
    }
    protected void AprobacionAutomatica(long idcabecera)
    {

        /// Se tiene que recuperar todos los items de la hoja 
        string resultado = AprobarHojaDeRuta(idcabecera.ToString(), (this.Session["usuario"] as Entidades.SegUsuario).IdSegUsuario.ToString());
        ScriptManager.RegisterClientScriptBlock(upGrilla, upGrilla.GetType(), "Aprobacion", "js_Aprobacion('" + resultado + "');", true);
    }

    protected void ExportExcel_Click(object sender, EventArgs e)
    {
        //foreach (Telerik.Web.UI.GridColumn column in gvLegajos.MasterTableView.Columns)
        //{
        //    if (!column.Visible)
        //    {
        //        column.Visible = true;
        //    }
        //}

        //upGrillaLegajos.Update();
        //gvLegajos.ExportSettings.IgnorePaging = true;
        //gvLegajos.ExportSettings.FileName = "Legajos";
        //gvLegajos.MasterTableView.ExportToExcel();


        CargarDatosSession();
        string file = Server.MapPath("Documentos") + @"\DDJJ-ListadoMensualLegajos.pdf";
        string html = "";

        #region INICIO TITULOS

        html += "<table id='Detalle' width='100%' style='height: 100%;font-family:Calibri;font-size:11pt' border='0' cellpadding='0' cellspacing='0'>" +
               "<tr>" +
                   "<td rowspan='2' align='center' valign='top'>" + "<img alt=\"Logo iText\" src='http://www.conosudsrlgestionva.com.ar/images/grifo cono sud srl.jpg' height=\"73px\" width=\"113px\">" + "</td>" +
                    "<td  align='center' valign='top' style='height: 100%;font-family:Calibri;font-size:18pt'>FORMULARIO II</td>" +
               "</tr>";

        html += "<tr>" +
                         "<td align='center' valign='top' style='height: 100%;font-family:Calibri;font-size:12pt'>DDJJ: LISTADO MENSUAL DE PERSONAL</td>" +
                "</tr>";


        html += "</table><br />";
        #endregion

        #region INICION ENCABEZADO
        /// Fila 1 Encabezado
        html += "<table width='100%' style='height: 100%;font-family:Calibri;font-size:9pt' border='1' cellpadding='0' cellspacing='0'>" +
                "<tr>" +
                    "<td align='left' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Empresa Contratista:</td>" +
                    "<td align='left' valign='top' colspan='3' style='padding-left:2px'>" + _CurrentTempCabecera.ContratoEmpresas.DescConstratista + "</td>" +
                    "<td align='center' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Cuit Nº:</td>" +
                    "<td align='left' valign='top' style='padding-left:2px' >" + string.Format("{0:##-########-#}", long.Parse(_CurrentTempCabecera.ContratoEmpresas.DescCUITConstratista)) + "</td>" +
                "</tr>";




        /// Fila 2 Encabezado
        if (!_CurrentTempCabecera.EsContratista.Value)
        {
            html += "<tr>" +
                       "<td align='left' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Sub Empresa Contratista:</td>" +
                       "<td align='left' valign='top' colspan='3' style='padding-left:2px'>" + _CurrentTempCabecera.ContratoEmpresas.DescSubConstratista + "</td>" +
                       "<td align='center' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Cuit Nº:</td>" +
                       "<td align='left' valign='top'  style='padding-left:2px'>" + string.Format("{0:##-########-#}", long.Parse(_CurrentTempCabecera.ContratoEmpresas.DescCUITSubConstratista)) + "</td>" +
                   "</tr>";
        }
        else
        {
            html += "<tr>" +
                          "<td align='left' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Sub Empresa Contratista:</td>" +
                          "<td align='left' valign='top' colspan='3' style='padding-left:2px'>-</td>" +
                          "<td align='center' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Cuit Nº:</td>" +
                          "<td align='left' valign='top'  style='padding-left:2px'>-</td>" +
                    "</tr>";
        }

        /// Fila 3 Encabezado
        html += "<tr>" +
                         "<td align='left' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Contrato Nº:</td>" +
                         "<td colspan='5' align='left' valign='top'  style='padding-left:2px'>" + _CurrentTempCabecera.Codigo + "</td>" +
                   "</tr>";

        /// Fila 4 Encabezado
        html += "<tr>" +
                         "<td align='left' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Vigencia Contrato:</td>" +
                         "<td colspan='5' align='left' valign='top' >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Desde:" + _CurrentTempCabecera.FechaInicio.ToShortDateString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Hasta:" + _CurrentTempCabecera.FechaVencimiento.ToShortDateString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Prórroga:" + (_CurrentTempCabecera.Prorroga.HasValue ? _CurrentTempCabecera.Prorroga.Value.ToShortDateString() : "__/__/____") + "</td>" +
                   "</tr>";


        /// Fila 5 Encabezado
        html += "<tr>" +
                         "<td align='left' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Descripción del Servicio/Obra:</td>" +
                         "<td colspan='5' align='left' valign='top'  style='padding-left:2px'>" + Helper.ToCapitalize(_CurrentTempCabecera.Servicio.ToUpper()) + "</td>" +
                   "</tr>";


        /// Fila 6 Encabezado
        /// Contratado por: 	PRC	Area:	Recursos Humanos	Gestor Contrato:	Paula Cassia
        html += "<tr>" +
                       "<td align='left' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Contratado por:</td>" +
                       "<td align='left' valign='top'  style='padding-left:2px'>" + _CurrentTempCabecera.objCabecera.ContratoEmpresas.Contrato.objContratadopor.Descripcion.ToUpper() + "</td>" +
                       "<td align='center' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Area:</td>" +
                       "<td align='left' valign='top'  style='padding-left:2px'>" + Helper.ToCapitalize(_CurrentTempCabecera.objCabecera.ContratoEmpresas.Contrato.objArea.Descripcion.ToLower()) + "</td>" +
                       "<td align='center' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Gestor Contrato:</td>" +
                       "<td align='left' valign='top'  style='padding-left:2px'>" + Helper.ToCapitalize(_CurrentTempCabecera.objCabecera.ContratoEmpresas.Contrato.GestorNombre.ToLower()) + "</td>" +
                   "</tr>";



        /// Fila 7 Encabezado
        /// Documentación del mes de: 	MARZO del 2013		
        DateTime periodo = _CurrentTempCabecera.Periodo;
        html += "<tr>" +
                         "<td align='left' valign='top' bgcolor='#C5BE97' style='font-weight:bold'>Documentación del mes de:</td>" +
                         "<td colspan='5' align='left' valign='top' style='padding-left:2px' >" + Helper.ToCapitalize(string.Format("{0:MMMM}", periodo)) + string.Format(" del {0:yyyy}", periodo) + "</td>" +
                   "</tr>";



        html += "</table><br />";

        #endregion

        #region INICIO DETALLE

        html += "<table id='Detalle' width='100%' style='height: 100%;font-family:Calibri;font-size:9pt' border='1' cellpadding='0' cellspacing='0'>" +
               "<tr>" +
                   "<td align='center' valign='top' bgcolor='#C5BE97'>Apellido y Nombre</td>" +
                   "<td align='center' valign='top' bgcolor='#C5BE97'>N° CUIL</td>" +
                   "<td align='center' valign='top' bgcolor='#C5BE97'>Convenio</td>" +
                   "<td align='center' valign='top' bgcolor='#C5BE97'>Provincia Residencia</td>" +
                   "<td align='center' valign='top' bgcolor='#C5BE97'>Período Afectación</td>" +
                   "<td align='center' valign='top' bgcolor='#C5BE97'>Fecha Ult. Ex. Médico</td>" +
               "</tr>";

        foreach (TempSueldos item in InfoSueldos)
        {
            //"<td align='left' valign='top'>" + Helper.ToCapitalize(item.NombreCompleto.ToLower()) + "</td>" +
            html += "<tr>" +
                    "<td align='left' valign='top'>" + item.NombreCompleto.ToUpper() + "</td>" +
                    "<td align='center' valign='top'>" + string.Format("{0:##-########-#}", long.Parse(item.CUIT)) + "</td>" +
                    "<td align='center' valign='top'>" + item.Convenio + "</td>" +
                    "<td align='center' valign='top'>" + item.Provincia + "</td>" +
                    "<td align='center' valign='top'>" + string.Format("{0:MMM-yyyy}", item.PeriodoAfecatacion) + "</td>" +
                    "<td align='center' valign='top'>" + (item.UltimoExamen.HasValue ? item.UltimoExamen.Value.ToShortDateString() : "-") + "</td>" +
                "</tr>";
        }


        html += "</table>";

        #endregion


        Document document = new Document(PageSize.A4, 20, 20, 20, 100);
        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(file, FileMode.Create));
        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

        itsEvents ev = new itsEvents();
        ev.direccion = HttpContext.Current.Request.Url.AbsoluteUri + "&TI" + DateTime.Now.Ticks.ToString();
        writer.PageEvent = ev;

        document.Open();


        Rectangle rect = document.PageSize;
        float pageWidth = rect.Width;
        int i = 0;

        foreach (IElement E in HTMLWorker.ParseToList(new StringReader(html), new StyleSheet()))
        {

            PdfPTable table = E as PdfPTable;


            if (table != null && i == 2)
            {
                table.SetWidthPercentage(new float[] { (float).25 * pageWidth, (float).15 * pageWidth, (float).15 * pageWidth, (float).15 * pageWidth, (float).15 * pageWidth, (float).15 * pageWidth }, rect);
                i++;
            }
            else if (table != null && i == 1)
            {
                table.SetWidthPercentage(new float[] { (float).25 * pageWidth, (float).15 * pageWidth, (float).15 * pageWidth, (float).15 * pageWidth, (float).15 * pageWidth, (float).15 * pageWidth }, rect);
                i++;
            }
            else if (table != null && i == 0)
            {
                table.SetWidthPercentage(new float[] { (float).10 * pageWidth, (float).90 * pageWidth }, rect);
                i++;
            }



            document.Add(E);
        }

        document.Close();
        //upImpresionLegajos.Update();

    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument == "Guardar")
        {
            long IdHoja = long.Parse(hdIdHoja.Value);
            long IdPlantilla = long.Parse(hdIdPlantilla.Value);


            UpdateItemHojaRuta(IdHoja, IdPlantilla);


            hdIdHoja.Value = "";
            hdIdPlantilla.Value = "";
            txtComentarioGral.Text = "";
            txtComentarioItem.Text = "";
            txtFechaAprobacion.Clear();
            upEdicion.Update();

        }
    }

    #endregion

    #region Metodos
    private void CargarDatosSession()
    {

        long id = long.Parse(Request.QueryString["IdCabecera"].ToString());

        _CurrentTempCabecera = (from c in Contexto.CabeceraHojasDeRuta
                                    .Include("ContratoEmpresas")
                                    .Include("ContratoEmpresas.Empresa")
                                    .Include("ContratoEmpresas.CabeceraHojasDeRuta")
                                where c.IdCabeceraHojasDeRuta == id
                                select new TempCabecera
                                {
                                    Codigo = c.ContratoEmpresas.Contrato.Codigo,
                                    Prorroga = c.ContratoEmpresas.Contrato.Prorroga,
                                    Servicio = c.ContratoEmpresas.Contrato.Servicio,
                                    FechaInicio = c.ContratoEmpresas.Contrato.FechaInicio.Value,
                                    FechaVencimiento = c.ContratoEmpresas.Contrato.FechaVencimiento.Value,
                                    EsContratista = c.ContratoEmpresas.EsContratista,
                                    Estimacion = c.Estimacion,
                                    IdCaebcera = c.IdCabeceraHojasDeRuta,
                                    LoginName = c.SegUsuario.Login,
                                    Periodo = c.Periodo,
                                    ContratoEmpresas = c.ContratoEmpresas,
                                    objCabecera = c,
                                    Categoria = c.ContratoEmpresas.Contrato.objCategoria.Descripcion
                                }).FirstOrDefault();


        ComentariosGrales = (from c in Contexto.ComentariosGral
                             where c.ContratoEmpresas.IdContratoEmpresas == _CurrentTempCabecera.ContratoEmpresas.IdContratoEmpresas
                             select new TempComentarioGrales
                             {
                                 Comentario = c.Comentario,
                                 IdPlantilla = c.Plantilla.Value,
                                 IdComentarioGel = c.IdComentarioGral

                             }).ToList();


        ItemsHojasDeRuta = (from h in Contexto.HojasDeRuta
                            where h.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == id
                            orderby h.Plantilla.Codigo
                            select new TempItemsHoja
                            {
                                AuditadoPor = h.AuditadoPor,
                                DocFechaEntrega = h.DocFechaEntrega,
                                HojaComentario = h.HojaComentario,
                                IdCabecera = id,
                                //IdContratoEmpresa = h.CabeceraHojasDeRuta.ContratoEmpresas.IdContratoEmpresas,
                                IdHoja = h.IdHojaDeRuta,
                                IdPlantilla = h.Plantilla.IdPlantilla,
                                Descripcion = h.Plantilla.Descripcion,
                                DocComentario = h.DocComentario,
                                HojaFechaAprobacion = h.HojaFechaAprobacion,
                                AuditoriaTerminada = h.AuditoriaTerminada
                            }).ToList();



        /// Carga de los legajos de la hoja de ruta
        InfoSueldos = (from L in Contexto.ContEmpLegajos
                       where L.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == _CurrentTempCabecera.IdCaebcera
                       && L.IdLegajos != null
                       orderby L.Legajos.Apellido
                       select new TempSueldos
                       {
                           Convenio = L.Legajos.objConvenio.Descripcion,
                           FechaTramiteBaja = L.FechaTramiteBaja,
                           IdLegajos = L.Legajos.IdLegajos,
                           NombreCompleto = L.Legajos.Apellido + ", " + L.Legajos.Nombre,
                           NroDoc = L.Legajos.NroDoc,
                           PoseeInfoSueldo = L.Legajos.DatosDeSueldos.Any(W => W.Periodo.HasValue && W.Periodo.Value.Month == _CurrentTempCabecera.Periodo.Month && W.Periodo.Value.Year == _CurrentTempCabecera.Periodo.Year),
                           CUIT = L.Legajos.CUIL,
                           Provincia = L.Legajos.objProvincia.Descripcion,
                           UltimoExamen = L.Legajos.FechaUltimoExamen,
                           PeriodoAfecatacion = L.Legajos.objContEmpLegajos.Where(w => w.IdContratoEmpresas == _CurrentTempCabecera.ContratoEmpresas.IdContratoEmpresas).FirstOrDefault().CabeceraHojasDeRuta.Periodo
                       }).ToList();


    }

    private void UpdateItemHojaRuta(long IdItemHoja, long IdPlantilla)
    {

        #region Recupero los  Datos Ingresado por el usuario


        string AuditadoPor = (Session["usuario"] as Entidades.SegUsuario).Login;
        DateTime HojaFechaControlado = DateTime.Now;
        string ComentarioGralIngresado = txtComentarioGral.Text;
        string ComentarioItemHoja = txtComentarioItem.Text;
        DateTime? FechaAprobacionItemHoja = txtFechaAprobacion.SelectedDate;


        #endregion


        Entidades.HojasDeRuta CurrentItem = (from i in Contexto.HojasDeRuta
                                             where i.IdHojaDeRuta == IdItemHoja
                                             select i).FirstOrDefault();



        TempComentarioGrales CurrentComentario = (from C in ComentariosGrales
                                                  where C.IdPlantilla == IdPlantilla
                                                  select C).FirstOrDefault<TempComentarioGrales>();


        if (!CurrentItem.PlantillaReference.IsLoaded) { CurrentItem.PlantillaReference.Load(); }
        if (CurrentComentario == null)
        {
            ComentariosGral coment = new ComentariosGral();
            coment.Comentario = ComentarioGralIngresado;
            coment.ContratoEmpresas = CurrentItem.CabeceraHojasDeRuta.ContratoEmpresas;
            coment.Plantilla = CurrentItem.IdPlanilla;
            Contexto.AddObject("ComentariosGral", coment);
        }
        else
        {
            Entidades.ComentariosGral CurrentComentarioGral = (from c in Contexto.ComentariosGral
                                                               where c.Plantilla.Value == IdPlantilla
                                                               && c.ContratoEmpresas.IdContratoEmpresas == CurrentItem.CabeceraHojasDeRuta.ContratoEmpresas.IdContratoEmpresas
                                                               select c).FirstOrDefault();

            CurrentComentarioGral.Comentario = ComentarioGralIngresado;
        }



        CurrentItem.AuditadoPor = AuditadoPor;
        CurrentItem.HojaFechaControlado = HojaFechaControlado;
        CurrentItem.HojaComentario = ComentarioItemHoja;
        CurrentItem.HojaFechaAprobacion = FechaAprobacionItemHoja;

        if (FechaAprobacionItemHoja != null)
        {
            CurrentItem.HojaAprobado = true;
            CurrentItem.AuditoriaTerminada = true;
        }
        else
        {
            CurrentItem.HojaAprobado = false;
            CurrentItem.AuditoriaTerminada = false;
        }


        Contexto.SaveChanges();
        CargarDatosSession();
        gvItemHoja.Rebind();
        upGrilla.Update();
        AprobacionAutomatica(CurrentItem.IdCabeceraHojaDeRuta);

    }

    private void UpdateItemHojaRuta(GridEditableItem editedItem, long IdItemHoja, long IdPlantilla)
    {
        #region Recupero los  Datos Ingresado por el usuario

        GridEditManager editMan = editedItem.EditManager;

        string AuditadoPor = (Session["usuario"] as Entidades.SegUsuario).Login;
        DateTime HojaFechaControlado = DateTime.Now;
        string ComentarioGralIngresado = ((TextBox)editedItem.FindControl("txtComentarioGralEdit")).Text;
        string ComentarioItemHoja = ((TextBox)editedItem.FindControl("txtComentarioItemHoja")).Text;
        DateTime? FechaAprobacionItemHoja = null;

        if ((editMan.GetColumnEditor("HojaFechaAprobacioncolumn") as GridDateTimeColumnEditor).Text != "")
            FechaAprobacionItemHoja = DateTime.Parse((editMan.GetColumnEditor("HojaFechaAprobacioncolumn") as GridDateTimeColumnEditor).Text);


        #endregion


        Entidades.HojasDeRuta CurrentItem = (from i in Contexto.HojasDeRuta
                                             where i.IdHojaDeRuta == IdItemHoja
                                             select i).FirstOrDefault();



        TempComentarioGrales CurrentComentario = (from C in ComentariosGrales
                                                  where C.IdPlantilla == IdPlantilla
                                                  select C).FirstOrDefault<TempComentarioGrales>();

        if (CurrentComentario == null)
        {
            ComentariosGral coment = new ComentariosGral();
            coment.Comentario = ComentarioGralIngresado;
            coment.ContratoEmpresas = CurrentItem.CabeceraHojasDeRuta.ContratoEmpresas;
            coment.Plantilla = CurrentItem.IdPlanilla;
            Contexto.AddObject("ComentariosGral", coment);
        }
        else
        {
            Entidades.ComentariosGral CurrentComentarioGral = (from c in Contexto.ComentariosGral
                                                               where c.Plantilla == IdPlantilla
                                                               && c.ContratoEmpresas.IdContratoEmpresas == CurrentItem.CabeceraHojasDeRuta.ContratoEmpresas.IdContratoEmpresas
                                                               select c).FirstOrDefault();

            CurrentComentarioGral.Comentario = ComentarioGralIngresado;
        }



        CurrentItem.AuditadoPor = AuditadoPor;
        CurrentItem.HojaFechaControlado = HojaFechaControlado;
        CurrentItem.HojaComentario = ComentarioItemHoja;
        CurrentItem.HojaFechaAprobacion = FechaAprobacionItemHoja;
        if (FechaAprobacionItemHoja != null)
            CurrentItem.HojaAprobado = true;
        else
            CurrentItem.HojaAprobado = false;


        Contexto.SaveChanges();
        CargarDatosSession();
        gvItemHoja.Rebind();
        AprobacionAutomatica(CurrentItem.IdCabeceraHojaDeRuta);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static String AprobarHojaDeRuta(string IdHojaRuta, string IdUsuario)
    {


        string retorno = "false";
        bool UltimaHoja = false;

        EntidadesConosud dc = new EntidadesConosud();

        long id = long.Parse(IdHojaRuta);
        int ItemNoAprobados = (from H in dc.HojasDeRuta
                               where H.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == id
                               && H.HojaFechaAprobacion == null
                               select H).Count();


        Entidades.CabeceraHojasDeRuta cab = (from C in dc.CabeceraHojasDeRuta
                                                .Include("ContratoEmpresas")
                                                .Include("ContratoEmpresas.Contrato")
                                                .Include("ContratoEmpresas.CabeceraHojasDeRuta")
                                                .Include("ContratoEmpresas.Empresa")
                                                .Include("ContEmpLegajos")
                                                .Include("ContEmpLegajos.Legajos")
                                                .Include("ContEmpLegajos.Legajos.objConvenio")
                                             where C.IdCabeceraHojasDeRuta == id
                                             select C).First();


        List<Entidades.CabeceraHojasDeRuta> hojaContratoOrdenadas = (from C in cab.ContratoEmpresas.CabeceraHojasDeRuta
                                                                     orderby C.Periodo
                                                                     select C).ToList<Entidades.CabeceraHojasDeRuta>();



        if (hojaContratoOrdenadas.Last().Periodo == cab.Periodo
           ||
           hojaContratoOrdenadas.Last().Periodo == cab.Periodo.AddMonths(-1))
        {
            UltimaHoja = true;
        }
        else
            UltimaHoja = false;



        if (ItemNoAprobados > 0)
            retorno = "Items|" + UltimaHoja.ToString();
        else
        {
            // Si es la ultima hoja de ruta tengo que controlar
            // que las hojas anteriores esten aprobadas, para poder 
            // aprobar la hoja en cuentión.
            if (UltimaHoja)
            {
                /// Si la hoja pertenece a un Sub-Contratista
                /// solo controlo las hojas de ruta de él.
                if (!cab.ContratoEmpresas.EsContratista.Value)
                {
                    int HayHojasSinAprobar = (from H in dc.CabeceraHojasDeRuta
                                              where H.Periodo <= cab.Periodo
                                              && H.IdCabeceraHojasDeRuta != cab.IdCabeceraHojasDeRuta
                                              && H.FechaAprobacion == null
                                              && H.ContratoEmpresas.IdContratoEmpresas == cab.ContratoEmpresas.IdContratoEmpresas
                                              select H).Count();

                    if (HayHojasSinAprobar > 0)
                        retorno = "Hojas|" + UltimaHoja.ToString();
                    else
                    {
                        GrabarAprobacion(cab, dc, long.Parse(IdUsuario));
                        dc.SaveChanges();
                        retorno = "Aprobada|" + UltimaHoja.ToString();
                    }


                }
                /// Si la hoja pertenece a un Contratista tengo
                /// que controlar las hojas de él y de todos los 
                /// subcontratistas que tenga.
                else
                {

                    int HayHojasSinAprobar = (from H in dc.CabeceraHojasDeRuta
                                              where H.Periodo <= cab.Periodo
                                              && H.FechaAprobacion == null
                                              && H.ContratoEmpresas.Contrato.IdContrato == cab.ContratoEmpresas.Contrato.IdContrato
                                              && H.IdCabeceraHojasDeRuta != id
                                              select H).Count();
                    if (HayHojasSinAprobar > 0)
                        retorno = "SubContratistas|" + UltimaHoja.ToString();
                    else
                    {
                        GrabarAprobacion(cab, dc, long.Parse(IdUsuario));
                        dc.SaveChanges();
                        retorno = "Aprobada|" + UltimaHoja.ToString();
                    }
                }

            }
            /// Si no es la ultima hoja de ruta. 
            else
            {
                /// Si la hoja pertenece a un Contratista
                /// tengo que controlar que las hojas de sus sub-Contratistas 
                /// esten aprobadas, para el mismo periodo, para poder
                /// aprobar la hoja en cuestion.
                if (cab.ContratoEmpresas.EsContratista.Value)
                {

                    int HayHojasSinAprobar = (from H in dc.CabeceraHojasDeRuta
                                              where (H.Periodo.Month == cab.Periodo.Month)
                                              && (H.Periodo.Year == cab.Periodo.Year)
                                              && H.FechaAprobacion == null
                                              && H.ContratoEmpresas.Contrato.IdContrato == cab.ContratoEmpresas.Contrato.IdContrato
                                              && H.ContratoEmpresas.Empresa.IdEmpresa != cab.ContratoEmpresas.Empresa.IdEmpresa
                                              select H).Count();

                    if (HayHojasSinAprobar > 0)
                        retorno = "SUBACTUAL|" + UltimaHoja.ToString();
                    else
                    {
                        GrabarAprobacion(cab, dc, long.Parse(IdUsuario));
                        dc.SaveChanges();
                        retorno = "Aprobada|" + UltimaHoja.ToString();
                    }

                }
                else
                {

                    GrabarAprobacion(cab, dc, long.Parse(IdUsuario));
                    dc.SaveChanges();
                    retorno = "Aprobada|" + UltimaHoja.ToString();
                }


            }
        }

        return retorno.ToUpper();
    }

    private static void GrabarAprobacion(Entidades.CabeceraHojasDeRuta cab, EntidadesConosud dc, long IdUsuario)
    {
        cab.FechaAprobacion = DateTime.Now;

        cab.Estado = (from E in dc.Clasificacion
                      where E.Tipo == "EstadoHoja" && E.Descripcion == "Aprobada"
                      select E).FirstOrDefault();

        cab.SegUsuario = (from S in dc.SegUsuario
                          where S.IdSegUsuario == IdUsuario
                          select S).FirstOrDefault();

        int CantidadLegajos = (from L in cab.ContEmpLegajos
                               select L).Count();

        if (CantidadLegajos == 0)
        {
            cab.FechaAprobacionSinLegajos = DateTime.Now;
        }

    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static String GrabarAprobacionEspecial(string IdCabecera, string IdUsuario)
    {

        EntidadesConosud dc = new EntidadesConosud();

        long id = long.Parse(IdCabecera);
        long idusua = long.Parse(IdUsuario);

        CabeceraHojasDeRuta cab = (from c in dc.CabeceraHojasDeRuta
                                   where c.IdCabeceraHojasDeRuta == id
                                   select c).FirstOrDefault();


        cab.FechaAprobacion = DateTime.Now;

        cab.Estado = (from E in dc.Clasificacion
                      where E.Tipo == "EstadoHoja" && E.Descripcion == "Aprobada"
                      select E).FirstOrDefault();

        cab.SegUsuario = (from S in dc.SegUsuario
                          where S.IdSegUsuario == idusua
                          select S).FirstOrDefault();

        int CantidadLegajos = (from L in cab.ContEmpLegajos
                               select L).Count();

        if (CantidadLegajos == 0)
        {
            cab.FechaAprobacionSinLegajos = DateTime.Now;
        }


        cab.AprobacionEpecial = true;

        dc.SaveChanges();

        /// Retorna que fue aprobado y que no es la ultima hoja de ruta
        return "APROBADA|false";
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static String DesaprobarHojaDeRuta(string IdCabecera, string IdUsuario)
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {
            long id = long.Parse(IdCabecera);
            long idusua = long.Parse(IdUsuario);

            Entidades.CabeceraHojasDeRuta cab = (from c in dc.CabeceraHojasDeRuta
                                                 where c.IdCabeceraHojasDeRuta == id
                                                 select c).FirstOrDefault();


            cab.FechaAprobacion = null;
            cab.FechaAprobacionSinLegajos = null;
            cab.SegUsuario = null;
            cab.Estado = (from E in dc.Clasificacion
                          where E.Tipo == "EstadoHoja" && E.Descripcion == "No Aprobada"
                          select E).FirstOrDefault();


            dc.SaveChanges();


            return "DESAPROBADA";
        }




    }
    #endregion
}



