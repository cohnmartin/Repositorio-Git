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
        public string IdEmpresaContratista { get; set; }
        public long IdTipoContrato { get; set; }
        public long IdContratadopor { get; set; }
        public string SubContratistas { get; set; }
        public string Categoria { get; set; }
        public long IdCategoria { get; set; }
        public object Contrato { get; set; }
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

    public List<TempContratos> Contratos
    {
        get
        {
            if (Session["Contratos"] != null)
                return (List<TempContratos>)Session["Contratos"];
            else
            {
                return (List<TempContratos>)Session["Contratos"];
            }
        }
        set
        {
            Session["Contratos"] = value;
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
            RadGrid1.DataSource = Contratos.Take(RadGrid1.PageSize);
            RadGrid1.Rebind();

        }
    }


    protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (txtCodigo.Text.Trim() == "")
            RadGrid1.DataSource = Contratos.Skip(RadGrid1.CurrentPageIndex * RadGrid1.PageSize).Take(RadGrid1.PageSize);
        else
        {
            var contratosFiltrados = (from c in Contratos
                                      where c.Codigo.ToUpper().StartsWith(txtCodigo.Text.ToUpper())
                                      select c).ToList();

            this.RadGrid1.DataSource = contratosFiltrados.Skip(RadGrid1.CurrentPageIndex * RadGrid1.PageSize).Take(RadGrid1.PageSize).ToList();
        }

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



        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {

            //Entidades.Contrato contrato = EntityDataSourceExtensions.GetItemObject<Entidades.Contrato>(e.Item.DataItem);
            //contrato.ContratoEmpresas.Load();
            //if (contrato.ContratoEmpresas.Count > 0)
            //{
            //    Entidades.ContratoEmpresas CurrentContEmp = contrato.ContratoEmpresas.Where(w => w.EsContratista.Value).First();
            //    CurrentContEmp.EmpresaReference.Load();
            //    (e.Item.FindControl("LabelContratista") as Label).Text = CurrentContEmp.Empresa.RazonSocial;

            //    string cadenaSubCont = "";
            //    int i = 1;
            //    foreach (Entidades.ContratoEmpresas SubContratistas in contrato.ContratoEmpresas.Where(w => w.EsContratista.Value == false))
            //    {
            //        SubContratistas.EmpresaReference.Load();
            //        cadenaSubCont += "(" + i.ToString() + ") " + SubContratistas.Empresa.RazonSocial;
            //        i++;
            //    }

            //    (e.Item.FindControl("LblSubContratista") as Label).Text = cadenaSubCont;

            //}
        }
        else if (e.Item.ItemType == GridItemType.EditFormItem)
        {
            if (e.Item.FindControl("RadComboBoxContratista") != null)
            {
                if (e.Item.DataItem is TempContratos)
                {
                    TempContratos CurrentContrato = (TempContratos)e.Item.DataItem;

                    (e.Item.FindControl("RadComboBoxContratista") as RadComboBox).SelectedValue = CurrentContrato.IdEmpresaContratista;
                    (e.Item.FindControl("cboTipoContrato") as RadComboBox).SelectedValue = CurrentContrato.IdTipoContrato.ToString();
                    (e.Item.FindControl("cboContratadoPor") as RadComboBox).SelectedValue = CurrentContrato.IdContratadopor.ToString();

                    if (CurrentContrato.Categoria != null)
                    {
                        (e.Item.FindControl("cboCategoria") as RadComboBox).FindItemByText(CurrentContrato.Categoria).Selected = true;
                        //(e.Item.FindControl("cboCategoria") as RadComboBox).SelectedValue = CurrentContrato.Categoria.ToString();
                    }

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
        Contratos = (from c in Contexto.Contrato
                     orderby c.Codigo descending
                     select new TempContratos
                     {
                         IdContrato = c.IdContrato,
                         Codigo = c.Codigo,
                         Servicio = c.Servicio,
                         FechaInicio = c.FechaInicio.Value,
                         FechaVencimiento = c.FechaVencimiento.Value,
                         Prorroga = c.Prorroga,
                         TipoContrato = c.TipoContrato.Descripcion,
                         ContratadoPor = c.Contratadopor.Descripcion,
                         IdTipoContrato = c.TipoContrato.IdClasificacion,
                         IdContratadopor = c.Contratadopor.IdClasificacion,
                         ContratoEmpresa = c.ContratoEmpresas.Where(w => w.EsContratista.Value),
                         Contrato = c,
                         Categoria = c.objCategoria.Descripcion
                     }).ToList();


        foreach (TempContratos item in Contratos)
        {
            if (!(((item).ContratoEmpresa as IList)[0] as Entidades.ContratoEmpresas).EmpresaReference.IsLoaded)
            {
                (((item).ContratoEmpresa as IList)[0] as Entidades.ContratoEmpresas).EmpresaReference.Load();
            }

            item.Contratista = (((item).ContratoEmpresa as IList)[0] as Entidades.ContratoEmpresas).Empresa.RazonSocial;
            item.IdEmpresaContratista = (((item).ContratoEmpresa as IList)[0] as Entidades.ContratoEmpresas).Empresa.IdEmpresa.ToString();

            if (!((Entidades.Contrato)item.Contrato).ContratoEmpresas.IsLoaded) { ((Entidades.Contrato)item.Contrato).ContratoEmpresas.Load(); }
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
    }
    private void UpdateContrato(GridEditableItem editedItem)
    {

        Contexto.CommandTimeout = 30000;
        long idContrato = Convert.ToInt64(ViewState["idsel"]);


        Entidades.Contrato _ContratoAnt = (from c in Contexto.Contrato
                                           where c.IdContrato == idContrato
                                           select c).FirstOrDefault();

        if (_ContratoAnt != null)
        {
            try
            {
                #region Recupero los  Datos Ingresado por el usuario
                long idCategoria_Ingresada = 0;

                GridEditManager editMan = editedItem.EditManager;
                string codigo_Ingresado = ((TextBox)editedItem.FindControl("TextBoxCodigo")).Text;
                string servicio_Ingresado = ((TextBox)editedItem.FindControl("TextBoxServicio")).Text;

                long idContratista_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("RadComboBoxContratista")).SelectedValue);
                long idTipoContrato_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboTipoContrato")).SelectedValue);

                if (((RadComboBox)editedItem.FindControl("cboCategoria")).SelectedValue != "")
                    idCategoria_Ingresada = long.Parse(((RadComboBox)editedItem.FindControl("cboCategoria")).SelectedValue);

                long idContratadoPor_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboContratadoPor")).SelectedValue);
                DateTime fechaInicioContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxFechaInicio") as TextBox).Text);
                DateTime fechaFinContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxFechaVencimiento") as TextBox).Text);
                DateTime? fechaPorrogaContrato_Ingresado = null;

                if ((editedItem.FindControl("TextBoxProrroga") as TextBox).Text != "")
                    fechaPorrogaContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxProrroga") as TextBox).Text);

                #endregion


                /// Actualizar el campo emprsa contratista del contrato
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
                        Helpers.GenerarHojadeRuta(Contexto, FechaFinAnt.Value, FechaFinNuevo.Value, ContEmp);

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
                                    // Elimino la cabecera
                                    objEliminar.Add(rowCabHR);

                                    // Elimino los legajos asociados
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


                _ContratoAnt.Codigo = codigo_Ingresado;
                _ContratoAnt.Servicio = servicio_Ingresado;
                _ContratoAnt.FechaInicio = fechaInicioContrato_Ingresado;
                _ContratoAnt.FechaVencimiento = fechaFinContrato_Ingresado;
                _ContratoAnt.Prorroga = fechaPorrogaContrato_Ingresado;
                _ContratoAnt.Contratadopor = Contexto.Clasificacion.Where(w => w.IdClasificacion == idContratadoPor_Ingresado).FirstOrDefault();
                _ContratoAnt.TipoContrato = Contexto.Clasificacion.Where(w => w.IdClasificacion == idTipoContrato_Ingresado).FirstOrDefault();

                if (idCategoria_Ingresada > 0)
                    _ContratoAnt.objCategoria = Contexto.Clasificacion.Where(w => w.IdClasificacion == idCategoria_Ingresada).FirstOrDefault();


                Contexto.SaveChanges();
                CargarSessionContratos();
            }
            catch (Exception e)
            {
                DivError.InnerText = e.Message + ":" + e.InnerException.Message;
                ScriptManager.RegisterStartupScript(updpnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ":" + e.InnerException.Message + ")", true);
                updpnlGrilla.Update();
            }
        }
    }
    private void InsertContrato(GridEditFormInsertItem editedItem)
    {


        #region Genero el CONTRATO con los datos ingresado por el usuario
        long idCategoria_Ingresada = 0;
        GridEditManager editMan = editedItem.EditManager;
        string codigo_Ingresado = ((TextBox)editedItem.FindControl("TextBoxCodigo")).Text;
        string servicio_Ingresado = ((TextBox)editedItem.FindControl("TextBoxServicio")).Text;
        long idContratista_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("RadComboBoxContratista")).SelectedValue);
        long idTipoContrato_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboTipoContrato")).SelectedValue);
        long idContratadoPor_Ingresado = long.Parse(((RadComboBox)editedItem.FindControl("cboContratadoPor")).SelectedValue);
        DateTime fechaInicioContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxFechaInicio") as TextBox).Text);
        DateTime fechaFinContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxFechaVencimiento") as TextBox).Text);
        DateTime? fechaPorrogaContrato_Ingresado = null;

        if (((RadComboBox)editedItem.FindControl("cboCategoria")).SelectedValue != "")
            idCategoria_Ingresada = long.Parse(((RadComboBox)editedItem.FindControl("cboCategoria")).SelectedValue);


        if ((editedItem.FindControl("TextBoxProrroga") as TextBox).Text != "")
            fechaPorrogaContrato_Ingresado = DateTime.Parse((editedItem.FindControl("TextBoxProrroga") as TextBox).Text);


        if ((from c in Contexto.Contrato
             where c.Codigo == codigo_Ingresado.Trim()
             select c).Count() > 0)
        {
            string scriptstring = "radalert('<h4>No se puede crear el contrato ya que el nro del mismo ya existe!</h4>', 330, 100, 'Contratos');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring, true);
            return;
        }





        Entidades.Contrato _Contrato = new Entidades.Contrato();
        _Contrato.Codigo = codigo_Ingresado;
        _Contrato.Servicio = servicio_Ingresado;
        _Contrato.FechaInicio = fechaInicioContrato_Ingresado;
        _Contrato.FechaVencimiento = fechaFinContrato_Ingresado;
        _Contrato.Contratadopor = Contexto.Clasificacion.Where(w => w.IdClasificacion == idContratadoPor_Ingresado).FirstOrDefault();
        _Contrato.TipoContrato = Contexto.Clasificacion.Where(w => w.IdClasificacion == idTipoContrato_Ingresado).FirstOrDefault();

        if (idCategoria_Ingresada > 0)
            _Contrato.objCategoria = Contexto.Clasificacion.Where(w => w.IdClasificacion == idCategoria_Ingresada).FirstOrDefault();


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

    }

    protected void LinkButtonEliminar_OnClick(object sender, EventArgs e)
    {
        try
        {
            int i = RadGrid1.SelectedItems[0].DataSetIndex - (this.RadGrid1.CurrentPageIndex * this.RadGrid1.PageSize);
            string IdContrato = RadGrid1.Items[i].GetDataKeyValue("IdContrato").ToString();
            long id = Convert.ToInt64(IdContrato);


            //if ((from h in Contexto.HojasDeRuta
            //     where h.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.IdContrato == id
            //     && h.HojaFechaAprobacion != null
            //     select h).Count() > 0)
            //{
            //    string scriptstring = "radalert('<h4>No se puede eliminar el contrato porque tiene cargada infomacion en su hoja de ruta!</h4>', 330, 100, 'Contratos');";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring, true);
            //    return;
            //}

            Entidades.Contrato _contrato = (from c in Contexto.Contrato
                        .Include("ContratoEmpresas.CabeceraHojasDeRuta.HojasDeRuta")
                                            where c.IdContrato == id
                                            select c).First();




            int j = _contrato.ContratoEmpresas.Count();
            while (j > 0)
            {



                Entidades.ContratoEmpresas _ContratoEmpresas = _contrato.ContratoEmpresas.Take(1).First();
                if (!_ContratoEmpresas.ComentariosGral.IsLoaded) { _ContratoEmpresas.ComentariosGral.Load(); }
                List<ComentariosGral> coment = _ContratoEmpresas.ComentariosGral.ToList();

                foreach (ComentariosGral itemComent in coment)
                {
                    Contexto.DeleteObject(itemComent);
                }

                if (!_ContratoEmpresas.Contrato.DatosDeSueldos.IsLoaded) { _ContratoEmpresas.Contrato.DatosDeSueldos.Load(); }
                List<DatosDeSueldos> infoSueldos = _ContratoEmpresas.Contrato.DatosDeSueldos.ToList();

                foreach (DatosDeSueldos itemsueldo in infoSueldos)
                {
                    Contexto.DeleteObject(itemsueldo);
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
        var contratosFiltrados = (from c in Contratos
                                  where c.Codigo.ToUpper().StartsWith(txtCodigo.Text.ToUpper())
                                  select c).ToList();

        RadGrid1.CurrentPageIndex = 0;
        this.RadGrid1.DataSource = contratosFiltrados.Skip(RadGrid1.CurrentPageIndex * RadGrid1.PageSize).Take(RadGrid1.PageSize).ToList();
        RadGrid1.VirtualItemCount = contratosFiltrados.Count;
        this.RadGrid1.Rebind();
        this.updpnlGrilla.Update();
    }


    public void ConfigureExportAndExport()
    {
        RadGrid1.CurrentPageIndex = 0;
        foreach (Telerik.Web.UI.GridColumn column in RadGrid1.MasterTableView.Columns)
        {
            if (!column.Visible)
            {
                column.Visible = true;
            }
        }

        RadGrid1.ExportSettings.ExportOnlyData = true;
        RadGrid1.ExportSettings.IgnorePaging = true;
        RadGrid1.ExportSettings.FileName = "Contratos";
        RadGrid1.MasterTableView.ExportToExcel();



    }
}
