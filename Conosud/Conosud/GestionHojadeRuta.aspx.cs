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

public partial class GestionHojadeRuta : System.Web.UI.Page
{
    #region Variables
    EntidadesConosud dc = null;
    private bool _tieneLegajo = false;
    private DateTime _periodoActual;
    private string _CodigoContrato;
    private string _comentarioGralIngresado = "";
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

    public TempCabecera CurrentTempCabecera
    {
        get
        {
            if (Session["CurrentTempCabecera"] != null)
                return (TempCabecera)Session["CurrentTempCabecera"];
            else
            {
                return (TempCabecera)Session["CurrentTempCabecera"];
            }
        }
        set
        {
            Session["CurrentTempCabecera"] = value;
        }
    }

    public List<TempSueldos> InfoSueldos
    {
        get
        {
            if (Session["InfoSueldos"] != null)
                return (List<TempSueldos>)Session["InfoSueldos"];
            else
            {
                return (List<TempSueldos>)Session["InfoSueldos"];
            }
        }
        set
        {
            Session["InfoSueldos"] = value;
        }
    }

    public List<TempItemsHoja> ItemsHojasDeRuta
    {
        get
        {
            if (Session["ItemsHojasDeRuta"] != null)
                return (List<TempItemsHoja>)Session["ItemsHojasDeRuta"];
            else
            {
                return (List<TempItemsHoja>)Session["ItemsHojasDeRuta"];
            }
        }
        set
        {
            Session["ItemsHojasDeRuta"] = value;
        }
    }

    public List<TempComentarioGrales> ComentariosGrales
    {
        get
        {
            if (Session["ComentariosGrales"] != null)
                return (List<TempComentarioGrales>)Session["ComentariosGrales"];
            else
            {
                return (List<TempComentarioGrales>)Session["ComentariosGrales"];
            }
        }
        set
        {
            Session["ComentariosGrales"] = value;
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
        set
        {
            Session["Contexto"] = value;
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
            this.Contexto = new EntidadesConosud();

            CargarDatosSession();

            gvItemHoja.DataSource = ItemsHojasDeRuta;
            gvItemHoja.DataBind();


            _periodoActual = CurrentTempCabecera.Periodo;
            _CodigoContrato = CurrentTempCabecera.Codigo;


            /// Deteccion de si es o no la ultima hoja de ruta
            if (!CurrentTempCabecera.ContratoEmpresas.CabeceraHojasDeRuta.IsLoaded) { CurrentTempCabecera.ContratoEmpresas.CabeceraHojasDeRuta.Load(); }
            var cabeceras = from C in CurrentTempCabecera.ContratoEmpresas.CabeceraHojasDeRuta
                            orderby C.Periodo
                            select C;

            if (cabeceras.Last().Periodo == CurrentTempCabecera.Periodo
                ||
                cabeceras.Last().Periodo == CurrentTempCabecera.Periodo.AddMonths(1))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "showAviso", "ShowAvisoUltimoCertificado();", true);
            }
            else
            {
                tdUltimoCertificado.Visible = false;
            }



            gvLegajos.ItemDataBound += new GridItemEventHandler(gvLegajos_ItemDataBound);
            gvLegajos.DataSource = InfoSueldos.ToList();
            gvLegajos.DataBind();


            if (InfoSueldos.Count() > 0)
            {
                _tieneLegajo = true;
            }


            lblContrato.Text = CurrentTempCabecera.Servicio;
            lblPeriodo.Text = string.Format("{0:MM/yyyy}", CurrentTempCabecera.Periodo);

            if (!CurrentTempCabecera.ContratoEmpresas.EmpresaReference.IsLoaded) { CurrentTempCabecera.ContratoEmpresas.EmpresaReference.Load(); }
            if (CurrentTempCabecera.EsContratista.Value)
            {
                lblContratista.Text = CurrentTempCabecera.ContratoEmpresas.Empresa.RazonSocial;
                lblSubContratista.Text = "-";
            }
            else
            {

                lblContratista.Text = CurrentTempCabecera.ContratoEmpresas.ConstratistaParaSubConstratista;
                lblSubContratista.Text = CurrentTempCabecera.ContratoEmpresas.Empresa.RazonSocial;
            }




            if (CurrentTempCabecera.objCabecera.Aprobada.Contains("No"))
            {
                lblEstado.Text = CurrentTempCabecera.objCabecera.Aprobada.ToUpper();
                lblEstado.Style.Add(HtmlTextWriterStyle.Color, "Red");
            }
            else
            {
                lblEstado.Text = CurrentTempCabecera.objCabecera.Aprobada.ToUpper();
                lblEstado.Style.Add(HtmlTextWriterStyle.Color, "Blue");
                lblUsuarioAprobador.Text = "(" + CurrentTempCabecera.LoginName + ")";
            }

            lblTitulo.Text = "Gestión Contrato Nro: " + CurrentTempCabecera.Codigo;
            txtEstimacion.Text = CurrentTempCabecera.Estimacion;

            lblFechaIncial.Text = CurrentTempCabecera.FechaInicio.ToShortDateString();

            if (!CurrentTempCabecera.Prorroga.HasValue)
                lblFechaFinal.Text = CurrentTempCabecera.FechaVencimiento.ToShortDateString();
            else
                lblFechaFinal.Text = CurrentTempCabecera.Prorroga.Value.ToShortDateString();

            /// Verificacion de acceso por Rol Aprobador
            string DescRol = Helpers.RolesEspeciales.Aprobador.ToString();
            string DescRolAdmin = Helpers.RolesEspeciales.Administrador.ToString();

            Entidades.SegUsuario usuario = (Entidades.SegUsuario)this.Session["usuario"];


            int RolesAprobador = (from U in Contexto.SegUsuario
                                  from UR in U.SegUsuarioRol
                                  where U.IdSegUsuario == usuario.IdSegUsuario
                                  && UR.SegRol.Descripcion == DescRol
                                  select UR).Count();


            int RolesAdministrador = (from U in Contexto.SegUsuario
                                      from UR in U.SegUsuarioRol
                                      where U.IdSegUsuario == usuario.IdSegUsuario
                                      && UR.SegRol.Descripcion == DescRolAdmin
                                      select UR).Count();


            if (RolesAprobador > 0)
            {
                trAprobacion.Visible = true;
                trEstimacion.Visible = true;
                trReporte.Visible = true;

                if (RolesAdministrador > 0)
                    trDesaprobacion.Visible = true;
                else
                    trDesaprobacion.Visible = false;
            }
            else
            {
                trAprobacion.Visible = false;
                trEstimacion.Visible = false;
                trDesaprobacion.Visible = false;
            }

            if (CurrentTempCabecera.Aprobada == "Aprobada")
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
                                     where R.Plantilla.IdPlantilla == IdPlantillla
                                     select R.SegRol.IdSegRol).Distinct<long>().ToArray<long>();


            Entidades.SegUsuario usuario = (Entidades.SegUsuario)this.Session["usuario"];

            if (!usuario.SegUsuarioRol.IsLoaded) { usuario.SegUsuarioRol.Load(); }
            foreach (Entidades.SegUsuarioRol item in usuario.SegUsuarioRol)
            {
                if (!item.SegRolReference.IsLoaded) { item.SegRolReference.Load(); }
            }

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
                (e.Item.FindControl("imagenAuditadoPor") as Image).Visible = true;
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
                (e.Item.FindControl("imgcomentarioitem") as Image).Visible = false;
            }

            if (hoja.DocFechaEntrega != null)
            {
                (e.Item.FindControl("imgcomentariodoc") as Image).Visible = true;
            }
            else
            {
                (e.Item.FindControl("imgcomentariodoc") as Image).Visible = false;
            }




            TempComentarioGrales CurrentComentario = (from C in ComentariosGrales
                                                      where C.IdPlantilla == hoja.IdPlantilla
                                                      select C).FirstOrDefault<TempComentarioGrales>();


            if (CurrentComentario != null && CurrentComentario.Comentario != "")
                (e.Item.FindControl("imgcomentariogral") as Image).Attributes.Add("coment", CurrentComentario.Comentario);
            else
                (e.Item.FindControl("imgcomentariogral") as Image).Visible = false;


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
                                             where C.IdCabeceraHojasDeRuta == CurrentTempCabecera.IdCaebcera
                                             select C).First();

        cab.Estimacion = txtEstimacion.Text;
        Contexto.SaveChanges();

    }

    protected void btnAplicar_Click(object sender, EventArgs arg)
    {
        foreach (GridItem item in gvItemHoja.Items)
        {

            long id = long.Parse(gvItemHoja.Items[item.DataSetIndex].GetDataKeyValue("IdHoja").ToString());

            Entidades.HojasDeRuta itemsHoja = (from H in Contexto.HojasDeRuta
                                               where H.IdHojaDeRuta == id
                                               select H).First<Entidades.HojasDeRuta>();

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
    }

    protected void ExportExcel_Click(object sender, EventArgs e)
    {
        foreach (Telerik.Web.UI.GridColumn column in gvLegajos.MasterTableView.Columns)
        {
            if (!column.Visible)
            {
                column.Visible = true;
            }
        }

        upGrillaLegajos.Update();
        gvLegajos.ExportSettings.IgnorePaging = true;
        gvLegajos.ExportSettings.FileName = "Legajos";
        gvLegajos.MasterTableView.ExportToExcel();
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

        CurrentTempCabecera = (from c in Contexto.CabeceraHojasDeRuta
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
                                   objCabecera = c
                               }).FirstOrDefault();


        ComentariosGrales = (from c in Contexto.ComentariosGral
                             where c.ContratoEmpresas.IdContratoEmpresas == CurrentTempCabecera.ContratoEmpresas.IdContratoEmpresas
                             select new TempComentarioGrales
                             {
                                 Comentario = c.Comentario,
                                 IdPlantilla = c.Plantilla.IdPlantilla,
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
                                IdContratoEmpresa = h.CabeceraHojasDeRuta.ContratoEmpresas.IdContratoEmpresas,
                                IdHoja = h.IdHojaDeRuta,
                                IdPlantilla = h.Plantilla.IdPlantilla,
                                Descripcion = h.Plantilla.Descripcion,
                                DocComentario = h.DocComentario,
                                HojaFechaAprobacion = h.HojaFechaAprobacion,
                                AuditoriaTerminada = h.AuditoriaTerminada
                            }).ToList();


        if (!IsPostBack)
        {
            /// Carga de los legajos de la hoja de ruta
            InfoSueldos = (from L in Contexto.ContEmpLegajos
                           where L.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == CurrentTempCabecera.IdCaebcera
                           orderby L.Legajos.Apellido
                           select new TempSueldos
                           {
                               Convenio = L.Legajos.objConvenio.Descripcion,
                               FechaTramiteBaja = L.FechaTramiteBaja,
                               IdLegajos = L.Legajos.IdLegajos,
                               NombreCompleto = L.Legajos.Apellido + ", " + L.Legajos.Nombre,
                               NroDoc = L.Legajos.NroDoc,
                               PoseeInfoSueldo = L.Legajos.DatosDeSueldos.Any(W => W.Periodo.HasValue && W.Periodo.Value.Month == CurrentTempCabecera.Periodo.Month && W.Periodo.Value.Year == CurrentTempCabecera.Periodo.Year)
                           }).ToList();

        }
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
            coment.ContratoEmpresas = CurrentTempCabecera.ContratoEmpresas;
            coment.Plantilla = CurrentItem.Plantilla;
            Contexto.AddObject("ComentariosGral", coment);
        }
        else
        {
            Entidades.ComentariosGral CurrentComentarioGral = (from c in Contexto.ComentariosGral
                                                               where c.Plantilla.IdPlantilla == IdPlantilla
                                                               && c.ContratoEmpresas.IdContratoEmpresas == CurrentTempCabecera.ContratoEmpresas.IdContratoEmpresas
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


        if (!CurrentItem.PlantillaReference.IsLoaded) { CurrentItem.PlantillaReference.Load(); }
        if (CurrentComentario == null)
        {
            ComentariosGral coment = new ComentariosGral();
            coment.Comentario = ComentarioGralIngresado;
            coment.ContratoEmpresas = CurrentTempCabecera.ContratoEmpresas;
            coment.Plantilla = CurrentItem.Plantilla;
            Contexto.AddObject("ComentariosGral", coment);
        }
        else
        {
            Entidades.ComentariosGral CurrentComentarioGral = (from c in Contexto.ComentariosGral
                                                               where c.Plantilla.IdPlantilla == IdPlantilla
                                                               && c.ContratoEmpresas.IdContratoEmpresas == CurrentTempCabecera.ContratoEmpresas.IdContratoEmpresas
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

    #endregion
}



