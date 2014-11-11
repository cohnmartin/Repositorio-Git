using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using System.Data.Objects;
using System.Data;

public partial class ABMContratos : System.Web.UI.Page
{
    private EntidadesConosud _Contexto;

    public class TempContratos
    {

        public string Codigo { get; set; }
        public string Servicio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? Prorroga { get; set; }
        public object ContratoEmpresa { get; set; }
        public long IdContrato { get; set; }
        public string Contratista { get; set; }
        public string TipoContrato { get; set; }
        public string ContratadoPor { get; set; }
        public string Categoria { get; set; }
        public string Area { get; set; }
        public string IdEmpresaContratista { get; set; }
        public long? IdTipoContrato { get; set; }
        public long? IdContratadopor { get; set; }
        public long? IdCategoria { get; set; }
        public long? IdArea { get; set; }

        public string GestorNombre { get; set; }
        public string GestorEmail { get; set; }
        public string FiscalNombre { get; set; }
        public string FiscalEmail { get; set; }


        public string SubContratistas { get; set; }
        public object Contrato { get; set; }
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

    public List<TempContratos> Contratos
    {
        //get
        //{
        //    if (Session["Contratos"] != null)
        //        return (List<TempContratos>)Session["Contratos"];
        //    else
        //    {
        //        return (List<TempContratos>)Session["Contratos"];
        //    }
        //}
        //set
        //{
        //    Session["Contratos"] = value;
        //}

        get
        {
            if (Session["Contratos"] != null)

                return (List<TempContratos>)Helper.DeSerializeObject(Session["Contratos"], typeof(List<TempContratos>));
            else
            {
                return (List<TempContratos>)Helper.DeSerializeObject(Session["Contratos"], typeof(List<TempContratos>));
            }
        }
        set
        {
            Session["Contratos"] = Helper.SerializeObject(value);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RadGrid1.DataBound += new EventHandler(RadGrid1_DataBound);
        RadGrid1.NeedDataSource += new GridNeedDataSourceEventHandler(RadGrid1_NeedDataSource);

        if (!IsPostBack)
        {
            CargarSessionContratos();

            RadGrid1.AllowCustomPaging = true;
            RadGrid1.VirtualItemCount = Contratos.Count;
            RadGrid1.DataSource = Contratos.OrderBy(w => w.Codigo).Take(RadGrid1.PageSize);
            RadGrid1.Rebind();
        }
    }

    protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {

        List<TempContratos> contratosFiltrados;

        if (txtEmpresa.Text != "")
        {
            contratosFiltrados = (from c in Contratos
                                  where c.Contratista.Trim().ToUpper().StartsWith(txtEmpresa.Text.Trim().ToUpper())
                                  orderby c.Codigo
                                  select c).ToList();
        }
        else
        {
            contratosFiltrados = (from c in Contratos
                                  where c.Codigo.Trim().ToUpper().StartsWith(txtCodigo.Text.Trim().ToUpper())
                                  orderby c.Codigo
                                  select c).ToList();
        }


        this.RadGrid1.VirtualItemCount = contratosFiltrados.Count;
        this.RadGrid1.DataSource = contratosFiltrados.Skip(RadGrid1.CurrentPageIndex * RadGrid1.PageSize).Take(RadGrid1.PageSize).ToList();


        //if (txtCodigo.Text.Trim() == "")
        //    if (Contratos.Count > 0)
        //        RadGrid1.DataSource = Contratos.OrderBy(w => w.Codigo).Skip(RadGrid1.CurrentPageIndex * RadGrid1.PageSize).Take(RadGrid1.PageSize);
        //    else
        //        RadGrid1.DataSource = (from c in Contratos
        //                               where c.Codigo.ToUpper().StartsWith(txtCodigo.Text.ToUpper())
        //                               select c).ToList();

        //else
        //{
        //    var contratosFiltrados = (from c in Contratos
        //                              where c.Codigo.ToUpper().StartsWith(txtCodigo.Text.ToUpper())
        //                              select c).ToList();

        //    this.RadGrid1.DataSource = contratosFiltrados.Skip(RadGrid1.CurrentPageIndex * RadGrid1.PageSize).Take(RadGrid1.PageSize).ToList();
        //}

    }
    protected void RadGrid1_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Contratos, idUsuario);


        LinkButton btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEdit");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
        btnAccion.Visible = PermisosPagina.Creacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDelete");
        btnAccion.Visible = PermisosPagina.Creacion;


    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            if (e.Item.OwnerTableView.IsItemInserted)
            {
                (e.Item.FindControl("cboTipoContrato") as RadComboBox).DataSource = (from c in Contexto.Clasificacion where c.Tipo == "Tipo Contrato" select new { c.IdClasificacion, c.Descripcion }).ToList();
                (e.Item.FindControl("cboTipoContrato") as RadComboBox).DataBind();

                (e.Item.FindControl("cboContratadoPor") as RadComboBox).DataSource = (from c in Contexto.Clasificacion where c.Tipo == "Contratado  por" select new { c.IdClasificacion, c.Descripcion }).ToList();
                (e.Item.FindControl("cboContratadoPor") as RadComboBox).DataBind();

                (e.Item.FindControl("cboContratista") as RadComboBox).DataSource = (from c in Contexto.Empresa orderby c.RazonSocial select new { c.IdEmpresa, c.RazonSocial }).ToList();
                (e.Item.FindControl("cboContratista") as RadComboBox).DataBind();

                (e.Item.FindControl("cboCategoria") as RadComboBox).DataSource = (from c in Contexto.Clasificacion where c.Tipo == "Categorias" select new { c.IdClasificacion, c.Descripcion }).ToList();
                (e.Item.FindControl("cboCategoria") as RadComboBox).DataBind();

                (e.Item.FindControl("cboArea") as RadComboBox).DataSource = (from c in Contexto.Clasificacion where c.Tipo == "Areas" orderby c.Codigo select new { c.IdClasificacion, c.Descripcion }).ToList();
                (e.Item.FindControl("cboArea") as RadComboBox).DataBind();
            }
        }

        if (e.Item.ItemType == GridItemType.EditFormItem)
        {
            if (e.Item.FindControl("cboContratista") != null)
            {
                if (e.Item.DataItem is TempContratos)
                {
                    TempContratos CurrentContrato = (TempContratos)e.Item.DataItem;

                    (e.Item.FindControl("cboTipoContrato") as RadComboBox).DataSource = (from c in Contexto.Clasificacion where c.Tipo == "Tipo Contrato" select new { c.IdClasificacion, c.Descripcion }).ToList();
                    (e.Item.FindControl("cboTipoContrato") as RadComboBox).DataBind();

                    (e.Item.FindControl("cboContratadoPor") as RadComboBox).DataSource = (from c in Contexto.Clasificacion where c.Tipo == "Contratado  por" select new { c.IdClasificacion, c.Descripcion }).ToList();
                    (e.Item.FindControl("cboContratadoPor") as RadComboBox).DataBind();

                    (e.Item.FindControl("cboCategoria") as RadComboBox).DataSource = (from c in Contexto.Clasificacion where c.Tipo == "Categorias" select new { c.IdClasificacion, c.Descripcion }).ToList();
                    (e.Item.FindControl("cboCategoria") as RadComboBox).DataBind();


                    (e.Item.FindControl("cboArea") as RadComboBox).DataSource = (from c in Contexto.Clasificacion where c.Tipo == "Areas" orderby c.Codigo select new { c.IdClasificacion, c.Descripcion }).ToList();
                    (e.Item.FindControl("cboArea") as RadComboBox).DataBind();


                    (e.Item.FindControl("cboContratista") as RadComboBox).DataSource = (from c in Contexto.Empresa orderby c.RazonSocial select new { c.IdEmpresa, c.RazonSocial }).ToList();
                    (e.Item.FindControl("cboContratista") as RadComboBox).DataBind();


                    (e.Item.FindControl("cboContratista") as RadComboBox).SelectedValue = CurrentContrato.IdEmpresaContratista;
                    (e.Item.FindControl("cboTipoContrato") as RadComboBox).SelectedValue = CurrentContrato.IdTipoContrato.ToString();
                    (e.Item.FindControl("cboCategoria") as RadComboBox).SelectedValue = CurrentContrato.IdCategoria.ToString();
                    (e.Item.FindControl("cboArea") as RadComboBox).SelectedValue = CurrentContrato.IdArea.ToString();
                    (e.Item.FindControl("cboContratadoPor") as RadComboBox).SelectedValue = CurrentContrato.IdContratadopor.ToString();


                }
            }
        }
    }
    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.PerformInsertCommandName
            || e.CommandName == RadGrid.UpdateCommandName)
        {
            if (e.Item is GridEditFormInsertItem)
            {
                GridEditFormInsertItem editedItem = e.Item as GridEditFormInsertItem;
                RadGrid1.MasterTableView.IsItemInserted = false;
                InsertContrato(editedItem);
            }
            else if (e.Item is GridEditFormItem)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                UpdateContrato(editedItem);
                RadGrid1.MasterTableView.ClearEditItems();
            }
        }
        else if (e.CommandName == RadGrid.InitInsertCommandName)
        {
            RadGrid1.MasterTableView.ClearEditItems();
        }


        if (e.CommandName == "ExportContratos")
        {
            ConfigureExportAndExport();
        }


    }
    protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["idsel"] = RadGrid1.SelectedValue;

    }

    private void CargarSessionContratos()
    {
        List<TempContratos> ContratosTemp = (from c in Contexto.Contrato
                     .Include("ContratoEmpresas")
                     .Include("ContratoEmpresas.Empresa")
                     select new TempContratos
                     {
                         IdContrato = c.IdContrato,
                         Codigo = c.Codigo,
                         Servicio = c.Servicio,
                         FechaInicio = c.FechaInicio.Value,
                         FechaVencimiento = c.FechaVencimiento.Value,
                         Prorroga = c.Prorroga,
                         TipoContrato = c.objTipoContrato.Descripcion,
                         ContratadoPor = c.objContratadopor.Descripcion,
                         IdTipoContrato = c.TipoContrato.Value,
                         IdContratadopor = c.Contratadopor.Value,
                         IdCategoria = c.Categoria.Value,
                         IdArea = c.Area.Value,
                         Categoria = c.objCategoria.Descripcion,
                         Area = c.objArea.Descripcion,
                         ContratoEmpresa = c.ContratoEmpresas.Where(w => w.EsContratista.Value).FirstOrDefault(),
                         GestorNombre = c.GestorNombre,
                         GestorEmail = c.GestorEmail,
                         FiscalNombre = c.FiscalNombre,
                         FiscalEmail = c.FiscalEmail,
                         Contrato = c
                     }).ToList();


        foreach (TempContratos item in ContratosTemp)
        {

            if (((item).ContratoEmpresa) != null )
            {

                item.Contratista = (((item).ContratoEmpresa) as Entidades.ContratoEmpresas).Empresa.RazonSocial;
                item.IdEmpresaContratista = (((item).ContratoEmpresa ) as Entidades.ContratoEmpresas).Empresa.IdEmpresa.ToString();

                //if (!((Entidades.Contrato)item.Contrato).ContratoEmpresas.IsLoaded) { ((Entidades.Contrato)item.Contrato).ContratoEmpresas.Load(); }
                List<Entidades.ContratoEmpresas> SubContras = ((Entidades.Contrato)item.Contrato).ContratoEmpresas.Where(w => w.EsContratista.Value == false).ToList();

                string cadenaSubCont = "";
                int i = 1;
                foreach (Entidades.ContratoEmpresas SubContratistas in SubContras)
                {
                    SubContratistas.EmpresaReference.Load();
                    cadenaSubCont += "(" + i.ToString() + ") " + SubContratistas.Empresa.RazonSocial;
                    i++;
                }

                item.SubContratistas = cadenaSubCont;
            }

            else
            {
                item.Contratista = "";
                item.SubContratistas = "";
            }
        }


        Contratos = ContratosTemp;
        //Session["Contexto"] = null;
    }
    private void UpdateContrato(GridEditableItem editedItem)
    {
        long idContrato = Convert.ToInt64(ViewState["idsel"]);


        Entidades.Contrato _ContratoAnt = (from c in Contexto.Contrato
                                           where c.IdContrato == idContrato
                                           select c).FirstOrDefault();

        if (_ContratoAnt != null)
        {
            try
            {

                #region Recupero los  Datos Ingresado por el usuario

                GridEditManager editMan = editedItem.EditManager;
                string codigo_Ingresado = (editMan.GetColumnEditor("Codigo") as GridTextBoxColumnEditor).Text;
                string servicio_Ingresado = (editMan.GetColumnEditor("Servicio") as GridTextBoxColumnEditor).Text;
                long idContratista_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboContratista")).SelectedValue);
                long idTipoContrato_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboTipoContrato")).SelectedValue);
                long idContratadoPor_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboContratadoPor")).SelectedValue);
                long idCategoria_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboCategoria")).SelectedValue);
                long idArea_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboArea")).SelectedValue);

                string GestorNombre_Ingresado = ((TextBox)editedItem.FindControl("txtNombreGestor")).Text;
                string GestorEmail_Ingresado = ((TextBox)editedItem.FindControl("txtEmailGestor")).Text;
                string FiscalNombre_Ingresado = ((TextBox)editedItem.FindControl("txtNombreFiscales")).Text;
                string FiscalEmail_Ingresado = ((TextBox)editedItem.FindControl("txtEmailFiscales")).Text;


                DateTime fechaInicioContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxFechaInicio") as TextBox).Text);
                DateTime fechaFinContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxFechaVencimiento") as TextBox).Text);
                DateTime? fechaPorrogaContrato_Ingresado = null;

                if ((editedItem.FindControl("TextBoxProrroga") as TextBox).Text != "")
                    fechaPorrogaContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxProrroga") as TextBox).Text);

                #endregion

                #region Actualización de las hojas de ruta segun las nuevas fechas
                IEnumerable<KeyValuePair<string, object>> entityKeyValues = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("IdEmpresa", idContratista_Ingresado) };

                EntityKey key = new EntityKey("EntidadesConosud.Empresa", entityKeyValues);
                Entidades.Empresa _emp = (Entidades.Empresa)Contexto.GetObjectByKey(key);

                if (!_ContratoAnt.ContratoEmpresas.IsLoaded) { _ContratoAnt.ContratoEmpresas.Load(); }
                _ContratoAnt.ContratoEmpresas.Where(w => w.EsContratista.Value).First().Empresa = _emp;


                DateTime? FechaInicioAnt = _ContratoAnt.FechaInicio;
                DateTime? FechaFinAnt = DateTime.MinValue;
                DateTime? FechaInicioNuevo = fechaInicioContrato_Ingresado;
                DateTime? FechaFinNuevo = DateTime.MinValue;

                FechaFinAnt = Helpers.DeterminarFinPeriodo(_ContratoAnt.FechaVencimiento, _ContratoAnt.Prorroga, ref FechaFinAnt);
                FechaFinNuevo = Helpers.DeterminarFinPeriodo(fechaFinContrato_Ingresado, fechaPorrogaContrato_Ingresado, ref FechaFinNuevo);


                foreach (Entidades.ContratoEmpresas ContEmp in _ContratoAnt.ContratoEmpresas)
                {
                    if (FechaInicioNuevo > FechaInicioAnt)
                    {
                        if ((from h in Contexto.HojasDeRuta
                             where h.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.IdContrato == idContrato
                             && h.HojaFechaAprobacion != null
                             && FechaInicioNuevo > h.CabeceraHojasDeRuta.Periodo
                             select h).Count() == 0)
                        {
                            List<object> objEliminar = new List<object>();
                            if (ContEmp.CabeceraHojasDeRuta.Count == 0) { ContEmp.CabeceraHojasDeRuta.Load(); }

                            ///Borro Diferencias
                            foreach (Entidades.CabeceraHojasDeRuta rowCabHR in ContEmp.CabeceraHojasDeRuta)
                            {
                                if (FechaInicioNuevo > rowCabHR.Periodo)
                                {
                                    objEliminar.Add(rowCabHR);

                                    if (!rowCabHR.ContEmpLegajos.IsLoaded) { rowCabHR.ContEmpLegajos.Load(); }
                                    foreach (Entidades.ContEmpLegajos itemC in rowCabHR.ContEmpLegajos)
                                    {
                                        objEliminar.Add(itemC);
                                    }

                                }

                            }

                            foreach (object item in objEliminar)
                            {
                                Contexto.DeleteObject(item);
                            }
                        }
                    }
                    else
                    {
                        if (FechaInicioNuevo < FechaInicioAnt)
                        {
                            ///Agrega Diferencias
                            Helpers.GenerarHojadeRuta(Contexto, FechaInicioNuevo.Value, FechaInicioAnt.Value, ContEmp);
                        }
                    }


                    // Agrego los periodos que superan la fecha de fin.
                    if (FechaFinNuevo > FechaFinAnt)
                    {
                        ///Agrega Diferencias
                        Helpers.GenerarHojadeRuta(Contexto, FechaFinAnt.Value, FechaFinNuevo.Value, ContEmp, true);
                    }
                    else
                    {
                        List<object> objEliminar = new List<object>();

                        if ((from h in Contexto.HojasDeRuta
                             where h.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.IdContrato == idContrato
                             && h.HojaFechaAprobacion == null
                             && h.CabeceraHojasDeRuta.Periodo > FechaFinNuevo
                             select h).Count() > 0)
                        {
                            ///Borro Diferencias
                            if (ContEmp.CabeceraHojasDeRuta.Count == 0) { ContEmp.CabeceraHojasDeRuta.Load(); }
                            foreach (Entidades.CabeceraHojasDeRuta rowCabHR in ContEmp.CabeceraHojasDeRuta)
                            {
                                if (rowCabHR.Periodo > FechaFinNuevo)
                                {
                                    objEliminar.Add(rowCabHR);
                                    if (!rowCabHR.ContEmpLegajos.IsLoaded) { rowCabHR.ContEmpLegajos.Load(); }
                                    foreach (Entidades.ContEmpLegajos itemC in rowCabHR.ContEmpLegajos)
                                    {
                                        objEliminar.Add(itemC);
                                    }
                                }
                            }
                        }

                        foreach (object item in objEliminar)
                        {
                            Contexto.DeleteObject(item);
                        }

                    }
                }

                #endregion


                #region  CAMBIO DE FECHAS DE FINALIZACION CONTRATO.
                /// Si las fechas de finalización son distintas, entonces debo actualizar
                /// el vencimiento de la credenciales de los legajos asociados al contrato
                if (FechaFinNuevo != FechaFinAnt)
                {
                    /// 91: Categoria del contrato Auditable al ingreso
                    /// Solo se tiene que actualizar los datos de la credencial si la cateria
                    /// del contrato es distinta a esta.
                    if (_ContratoAnt.objCategoria.IdClasificacion != 91)
                    {
                        foreach (ContratoEmpresas ContEmp in _ContratoAnt.ContratoEmpresas)
                        {
                            foreach (ContEmpLegajos ContLeg in ContEmp.ContEmpLegajos)
                            {
                                if (ContLeg.Legajos != null)
                                    ContLeg.Legajos.CredVencimiento = FechaFinNuevo;
                            }
                        }
                    }
                }
                #endregion

                #region CAMBIO DE CATEGORIA DEL CONTRATO.
                /// Caso 1: Al momento de cambiar la clasificacion de un contrato el cual pasa de Auditable a Auditable al ingreso  
                /// se debe sacar todas las fechas de venc. cred. de los legajos asociaciodos al contrato.
                /// 
                /// Caso 2: Caso contrario, es decir de Aud al Ingreso a Auditable 
                /// se les tiene que poner la fecha de finalizacion del contrato 
                /// como fecha de ven. cred.

                long CategoriaAnt = _ContratoAnt.objCategoria.IdClasificacion;
                if (CategoriaAnt != idCategoria_Ingresado)
                {
                    /// 91: Auditable al ingreso
                    if (idCategoria_Ingresado == 91)
                    {
                        foreach (ContratoEmpresas ContEmp in _ContratoAnt.ContratoEmpresas)
                        {
                            foreach (ContEmpLegajos ContLeg in ContEmp.ContEmpLegajos)
                            {
                                ContLeg.Legajos.CredVencimiento = null;
                            }
                        }
                    }
                    else
                    {
                        foreach (ContratoEmpresas ContEmp in _ContratoAnt.ContratoEmpresas)
                        {
                            foreach (ContEmpLegajos ContLeg in ContEmp.ContEmpLegajos)
                            {
                                if (ContLeg.Legajos != null)
                                    ContLeg.Legajos.CredVencimiento = FechaFinNuevo;
                            }
                        }
                    }

                }

                #endregion

                #region ACTUALIZACION DE LOS DATOS DEL CONTRATO
                _ContratoAnt.Codigo = codigo_Ingresado.Trim();
                _ContratoAnt.Servicio = servicio_Ingresado;
                _ContratoAnt.FechaInicio = fechaInicioContrato_Ingresado;
                _ContratoAnt.FechaVencimiento = fechaFinContrato_Ingresado;
                _ContratoAnt.Prorroga = fechaPorrogaContrato_Ingresado;
                _ContratoAnt.Contratadopor = idContratadoPor_Ingresado;
                _ContratoAnt.TipoContrato = idTipoContrato_Ingresado;
                _ContratoAnt.Categoria = idCategoria_Ingresado;
                _ContratoAnt.Area = idArea_Ingresado;
                _ContratoAnt.GestorNombre = GestorNombre_Ingresado;
                _ContratoAnt.GestorEmail = GestorEmail_Ingresado;
                _ContratoAnt.FiscalNombre = FiscalNombre_Ingresado;
                _ContratoAnt.FiscalEmail = FiscalEmail_Ingresado;
                #endregion

                Contexto.SaveChanges();
                CargarSessionContratos();
                RadGrid1.Rebind();

            }
            catch (Exception e)
            {
                ScriptManager.RegisterStartupScript(updpnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);

            }
        }
    }
    private void InsertContrato(GridEditFormInsertItem editedItem)
    {


        #region Genero el CONTRATO con los datos ingresado por el usuario

        GridEditManager editMan = editedItem.EditManager;
        string codigo_Ingresado = (editMan.GetColumnEditor("Codigo") as GridTextBoxColumnEditor).Text;
        string servicio_Ingresado = (editMan.GetColumnEditor("Servicio") as GridTextBoxColumnEditor).Text;
        long idContratista_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboContratista")).SelectedValue);
        long idTipoContrato_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboTipoContrato")).SelectedValue);
        long idContratadoPor_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboContratadoPor")).SelectedValue);
        long idCategoria_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboCategoria")).SelectedValue);
        long idArea_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboArea")).SelectedValue);


        string GestorNombre_Ingresado = ((TextBox)editedItem.FindControl("txtNombreGestor")).Text;
        string GestorEmail_Ingresado = ((TextBox)editedItem.FindControl("txtEmailGestor")).Text;
        string FiscalNombre_Ingresado = ((TextBox)editedItem.FindControl("txtNombreFiscales")).Text;
        string FiscalEmail_Ingresado = ((TextBox)editedItem.FindControl("txtEmailFiscales")).Text;


        DateTime fechaInicioContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxFechaInicio") as TextBox).Text);
        DateTime fechaFinContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxFechaVencimiento") as TextBox).Text);
        DateTime? fechaPorrogaContrato_Ingresado = null;

        if ((editedItem.FindControl("TextBoxProrroga") as TextBox).Text != "")
            fechaPorrogaContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxProrroga") as TextBox).Text);

        Entidades.Contrato _Contrato = new Entidades.Contrato();
        _Contrato.Codigo = codigo_Ingresado.Trim();
        _Contrato.Servicio = servicio_Ingresado;
        _Contrato.FechaInicio = fechaInicioContrato_Ingresado;
        _Contrato.FechaVencimiento = fechaFinContrato_Ingresado;
        _Contrato.Contratadopor = idContratadoPor_Ingresado;
        _Contrato.TipoContrato = idTipoContrato_Ingresado;
        _Contrato.Categoria = idCategoria_Ingresado;
        _Contrato.Area = idArea_Ingresado;
        _Contrato.GestorNombre = GestorNombre_Ingresado;
        _Contrato.GestorEmail = GestorEmail_Ingresado;
        _Contrato.FiscalNombre = FiscalNombre_Ingresado;
        _Contrato.FiscalEmail = FiscalEmail_Ingresado;

        #endregion

        #region Genero el CONTRATO-EMPRESA con los datos ingresado por el usuario

        Entidades.ContratoEmpresas _ContEmp = new Entidades.ContratoEmpresas();
        IEnumerable<KeyValuePair<string, object>> entityKeyValues =
            new KeyValuePair<string, object>[] { 
                    new KeyValuePair<string, object>("IdEmpresa", idContratista_Ingresado) };
        EntityKey key = new EntityKey("EntidadesConosud.Empresa", entityKeyValues);
        Entidades.Empresa _emp = (Entidades.Empresa)Contexto.GetObjectByKey(key);

        _ContEmp.EsContratista = true;
        _ContEmp.Contrato = _Contrato;
        _ContEmp.Empresa = _emp;

        Contexto.AddObject("EntidadesConosud.ContratoEmpresas", _ContEmp);

        #endregion

        #region Genero el ENCABEZADO Y LAS HOJAS con los datos ingresado por el usuario

        DateTime FFin = DateTime.Now;
        if (_Contrato.Prorroga.HasValue) { FFin = _Contrato.Prorroga.Value; }
        else { FFin = _Contrato.FechaVencimiento.Value; }

        DateTime FechaInicio = new DateTime(_Contrato.FechaInicio.Value.Year, _Contrato.FechaInicio.Value.Month, 1);

        Helpers.GenerarHojadeRuta(Contexto, FechaInicio, FFin, _ContEmp);

        #endregion

        Contexto.SaveChanges();
        CargarSessionContratos();
        RadGrid1.Rebind();
    }

    protected void LinkButtonEliminar_OnClick(object sender, EventArgs e)
    {
        try
        {
            int i = RadGrid1.SelectedItems[0].DataSetIndex - (this.RadGrid1.CurrentPageIndex * this.RadGrid1.PageSize);
            string IdContrato = RadGrid1.Items[i].GetDataKeyValue("IdContrato").ToString();
            long id = Convert.ToInt64(IdContrato);


            if ((from h in Contexto.HojasDeRuta
                 where h.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.IdContrato == id
                 && h.HojaFechaAprobacion != null
                 select h).Count() > 0)
            {
                string scriptstring = "radalert('<h4>No se puede eliminar el contrato porque tiene cargada infomacion en su hoja de ruta!</h4>', 330, 100, 'Contratos');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring, true);
                return;
            }

            Entidades.Contrato _contrato = (from c in Contexto.Contrato
                        .Include("ContratoEmpresas.CabeceraHojasDeRuta.HojasDeRuta")
                                            where c.IdContrato == id
                                            select c).First();




            int j = _contrato.ContratoEmpresas.Count();
            while (j > 0)
            {



                Entidades.ContratoEmpresas _ContratoEmpresas = _contrato.ContratoEmpresas.Take(1).First();
                if (!_ContratoEmpresas.ComentariosGral.IsLoaded) { _ContratoEmpresas.ComentariosGral.Load(); }
                foreach (ComentariosGral itemComent in _ContratoEmpresas.ComentariosGral)
                {
                    Contexto.DeleteObject(itemComent);
                }


                int j2 = _ContratoEmpresas.CabeceraHojasDeRuta.Count();
                while (j2 > 0)
                {
                    Entidades.CabeceraHojasDeRuta _CabeceraHojasDeRuta = _ContratoEmpresas.CabeceraHojasDeRuta.Take(1).First();

                    int j3 = _CabeceraHojasDeRuta.HojasDeRuta.Count();
                    while (j3 > 0)
                    {
                        Entidades.HojasDeRuta _HojasDeRuta = _CabeceraHojasDeRuta.HojasDeRuta.Take(1).First();
                        Contexto.DeleteObject(_HojasDeRuta);
                        j3--;
                    }
                    Contexto.DeleteObject(_CabeceraHojasDeRuta);
                    j2--;
                }
                Contexto.DeleteObject(_ContratoEmpresas);
                j--;
            }

            Contexto.DeleteObject(_contrato);
            Contexto.SaveChanges();

            this.CargarSessionContratos();
            this.RadGrid1.Rebind();
        }
        catch (Exception err)
        {

            string scriptstring = "radalert('<h4>" + err.InnerException.Message.Substring(0, 150) + "</h4>', 630, 100, 'Contratos');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring, true);

        }

    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        ViewState["idsel"] = null;
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        List<TempContratos> contratosFiltrados;

        if (txtEmpresa.Text != "")
        {
            contratosFiltrados = (from c in Contratos
                                  where c.Contratista.Trim().ToUpper().StartsWith(txtEmpresa.Text.Trim().ToUpper())
                                  orderby c.Codigo
                                  select c).ToList();
        }
        else
        {
            contratosFiltrados = (from c in Contratos
                                  where c.Codigo.Trim().ToUpper().StartsWith(txtCodigo.Text.Trim().ToUpper())
                                  orderby c.Codigo
                                  select c).ToList();
        }

        RadGrid1.CurrentPageIndex = 0;
        this.RadGrid1.DataSource = contratosFiltrados.Skip(RadGrid1.CurrentPageIndex * RadGrid1.PageSize).Take(RadGrid1.PageSize).ToList();
        RadGrid1.VirtualItemCount = contratosFiltrados.Count;
        this.RadGrid1.Rebind();
        this.updpnlGrilla.Update();
    }
    public void ConfigureExportAndExport()
    {
        //RadGrid1.CurrentPageIndex = 0;
        //foreach (Telerik.Web.UI.GridColumn column in RadGrid1.MasterTableView.Columns)
        //{
        //    if (!column.Visible)
        //    {
        //        column.Visible = true;
        //    }
        //}

        //RadGrid1.ExportSettings.ExportOnlyData = true;
        //RadGrid1.ExportSettings.IgnorePaging = true;
        //RadGrid1.ExportSettings.FileName = "Contratos";
        //RadGrid1.MasterTableView.ExportToExcel();


        List<dynamic> datosExportar = Contratos.ToList<dynamic>();


        List<string> camposExcluir = new List<string>(); ;
        Dictionary<string, string> alias = new Dictionary<string, string>() {
            {"Codigo","Nro Contrato" },
            {"Servicio" ,"Servicio" },
            {"FechaInicio","Fecha Inicio" },
            {"FechaVencimiento" ,"Fecha Vencimiento" },
            {"TipoContrato","Tipo Contrato"   },
            {"Contratadopor","Contratado Por" },
            {"Categoria","Clasificación" },
            {"GestorNombre" ,"Gestor Nombre"  },
            {"GestorEmail"  ,"Gestor Email" },
            {"FiscalNombre"  ,"Fiscales Nombre"  },
            {"FiscalEmail"  ,"Fiscales Email"  }};


        camposExcluir.Add("ContratoEmpresa");
        camposExcluir.Add("IdContrato");
        camposExcluir.Add("IdEmpresaContratista");
        camposExcluir.Add("IdTipoContrato");
        camposExcluir.Add("IdContratadopor");
        camposExcluir.Add("IdCategoria");
        camposExcluir.Add("IdArea");
        camposExcluir.Add("Contrato");


        List<string> DatosReporte = new List<string>();
        DatosReporte.Add("Listado de Contratos");
        DatosReporte.Add("Fecha y Hora emisi&oacute;n:" + DateTime.Now.ToString());
        DatosReporte.Add("Incluye todos los contratos registrados en el SCS, se encuentren vigentes o no");


        GridView gv = Helpers.GenerarExportExcel(datosExportar.ToList<dynamic>(), alias, camposExcluir, DatosReporte);

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gv.RenderControl(htmlWrite);

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Contratos" + "_" + DateTime.Now.ToString("M_dd_yyyy_H_M_s") + ".xls");
        HttpContext.Current.Response.ContentType = "application/xls";
        HttpContext.Current.Response.Write(stringWrite.ToString());
        HttpContext.Current.Response.End();

    }


}




