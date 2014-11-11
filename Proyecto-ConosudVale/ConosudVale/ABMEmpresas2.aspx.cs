using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Entidades;

public partial class ABMEmpresas2 : System.Web.UI.Page
{

    private EntidadesConosud _Contexto;

    public long IdEmpresaContratistaLogin
    {
        get
        {
            return ((SegUsuario)this.Session["usuario"]).Empresa.IdEmpresa;
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

    public List<Empresa> DatosEmpresas
    {
        //get
        //{
        //    if (Session["DatosEmpresas"] != null)
        //        return (List<Empresa>)Session["DatosEmpresas"];
        //    else
        //    {
        //        Session["DatosEmpresas"] = new List<Empresa>();
        //        return (List<Empresa>)Session["DatosEmpresas"];
        //    }
        //}
        //set
        //{
        //    Session["DatosEmpresas"] = value;
        //}


        get
        {
            if (Session["DatosEmpresas"] != null)

                return (List<Empresa>)Helper.DeSerializeObject(Session["DatosEmpresas"], typeof(List<Empresa>));
            else
            {
                return (List<Empresa>)Helper.DeSerializeObject(Session["DatosEmpresas"], typeof(List<Empresa>));
            }
        }
        set
        {
            Session["DatosEmpresas"] = Helper.SerializeObject(value);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RadGrid1.DataBound += new EventHandler(RadGrid1_DataBound);

        if (!IsPostBack)
        {
            if (this.Session["TipoUsuario"].ToString() != "Cliente")
            {
                DatosEmpresas = (from emp in Contexto.Empresa
                                 orderby emp.RazonSocial
                                 select emp).ToList();

                trFiltro.Style.Add(HtmlTextWriterStyle.Display, "block");
            }
            else
            {
                DatosEmpresas = (from emp in Contexto.Empresa
                                 where emp.IdEmpresa == IdEmpresaContratistaLogin
                                 orderby emp.RazonSocial
                                 select emp).ToList();

                trFiltro.Style.Add(HtmlTextWriterStyle.Display, "none");

            }


            RadGrid1.DataSource = DatosEmpresas.Take(RadGrid1.PageSize);
            RadGrid1.DataBind();



        }



    }

    void RadGrid1_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Empresas, idUsuario);


        LinkButton btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEdit");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
        btnAccion.Visible = PermisosPagina.Creacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDelete");
        btnAccion.Visible = PermisosPagina.Creacion;
    }

    protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "ExportEmpresas")
        {
            //this.RadGrid1.DataSource = DatosEmpresas;
            //this.RadGrid1.DataBind();

            ConfigureExportAndExport();
        }

    }

    protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
    {
        FiltrarEmpresas(txtApellidoLegajo.Text.Trim());
    }

    public void ConfigureExportAndExport()
    {
        //foreach (Telerik.Web.UI.GridColumn column in RadGrid1.MasterTableView.Columns)
        //{
        //    if (!column.Visible || !column.Display)
        //    {
        //        column.Visible = true;
        //        column.Display = true;
        //    }
        //}

        //RadGrid1.ExportSettings.ExportOnlyData = true;
        //RadGrid1.ExportSettings.IgnorePaging = true;
        //RadGrid1.ExportSettings.FileName = "Empresas";
        //RadGrid1.MasterTableView.ExportToExcel();


        List<dynamic> datosExportar = DatosEmpresas.ToList<dynamic>();
        List<string> camposExcluir = new List<string>(); 


        Dictionary<string, string> alias = new Dictionary<string, string>() {
            {"RazonSocial" ,"RazonSocial" },
            {"CUIT" ,"CUIT" },
            {"FechaAlta"   ,"FechaAlta" },
            {"RepresentanteTecnico" ,"Rep. Técnico"},
            {"PrestacionEmergencia" ,"Prestacion Emergencia"},
            {"Direccion" ,"Direccion" },
            {"Telefono" ,"Telefono" },
            {"CorreoElectronico" ,"Correo Electronico"},
            {"Emergencia" ,"Emergencias"},
            {"DescArt" ,"Seguro ART" },
            {"DescVida" ,"Seguro Vida"}};


        camposExcluir.Add("IdEmpresa");
        camposExcluir.Add("ContratoEmpresas");
        camposExcluir.Add("DatosDeSueldos");
        camposExcluir.Add("SegUsuario");
        camposExcluir.Add("Seguros");
        camposExcluir.Add("VahiculosyEquipos");
        camposExcluir.Add("Legajos");
        camposExcluir.Add("EntityState");
        camposExcluir.Add("EntityKey");


        


        List<string> DatosReporte = new List<string>();
        DatosReporte.Add("Listado de Empresas Contratistas de VALE");
        DatosReporte.Add("Fecha y Hora emisi&oacute;n:" + DateTime.Now.ToString());
        DatosReporte.Add("Incluye a todas las Empresas Contratistas registradas en el SCS");


        GridView gv = Helpers.GenerarExportExcel(datosExportar.ToList<dynamic>(), alias, camposExcluir, DatosReporte);

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gv.RenderControl(htmlWrite);

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Empresas" + "_" + DateTime.Now.ToString("M_dd_yyyy_H_M_s") + ".xls");
        HttpContext.Current.Response.ContentType = "application/xls";
        HttpContext.Current.Response.Write(stringWrite.ToString());
        HttpContext.Current.Response.End();
    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        EntidadesConosud dc = Contexto;

        if (e.Argument == "Update")
        {
            long id = long.Parse(RadGrid1.SelectedValue.ToString());

            Entidades.Empresa EmpUpdate = (from L in dc.Empresa
                                           where L.IdEmpresa == id
                                           select L).FirstOrDefault<Entidades.Empresa>();

            int cant = (from l in dc.Empresa
                        where l.CUIT.Trim() == txtCUIT.Text.Trim()
                        && l.IdEmpresa != id
                        select l).Count();

            txtCUIT.Attributes.Remove("NroExistente");

            if (cant > 0)
            {
                txtCUIT.Attributes.Add("NroExistente", true.ToString());
                upNroCUIT.Update();
                return;
            }
            else
            {
                txtCUIT.Attributes.Add("NroExistente", false.ToString());
                upNroCUIT.Update();
            }


            if (EmpUpdate != null)
            {
                /// Controles Tipo TextBox
                EmpUpdate.RazonSocial = txtRazonSocial.Text.Trim();
                EmpUpdate.RepresentanteTecnico = txtTecnico.Text.Trim();
                EmpUpdate.PrestacionEmergencia = txtPrestacionEmergencias.Text.Trim();
                EmpUpdate.Direccion = txtDireccion.Text.Trim();
                EmpUpdate.Telefono = txtTelefono.Text.Trim();
                EmpUpdate.Emergencia = txtEmergencias.Text.Trim();
                EmpUpdate.CorreoElectronico = txtEmail.Text.Trim();

                /// Controles Tipo Telerik
                EmpUpdate.CUIT = txtCUIT.Text;

                /// Controles Tipo Fecha
                EmpUpdate.FechaAlta = txtFechaAlta.SelectedDate;

                dc.SaveChanges();
            }

            FiltrarEmpresas(txtApellidoLegajo.Text.Trim());
            return;
        }
        if (e.Argument == "delete")
        {
            long id = long.Parse(RadGrid1.SelectedValue.ToString());


            Entidades.Empresa EmpDelete = (from L in Contexto.Empresa
                                           where L.IdEmpresa == id
                                           select L).FirstOrDefault<Entidades.Empresa>();


            /// Saco a todos los lejos asociados a la empresa.
            List<Legajos> legajosAsociados = EmpDelete.Legajos.ToList();
            foreach (Legajos leg in legajosAsociados)
            {
                leg.objEmpresaLegajo = null;
            }


            /// Saco a todos vehiculos asociados
            List<VahiculosyEquipos> VehiculosAsociados = EmpDelete.VahiculosyEquipos.ToList();
            foreach (VahiculosyEquipos vec in VehiculosAsociados)
            {
                vec.objEmpresa = null;
            }


            /// Saco el usario a asociado a la empresa
            List<SegUsuario> SegUsuairoEmp = (from L in Contexto.SegUsuario
                                              where L.IdEmpresa == id
                                              select L).ToList<SegUsuario>();

            foreach (SegUsuario item in SegUsuairoEmp)
            {
                item.Empresa = null;
            }


            int j = EmpDelete.ContratoEmpresas.Count();
            while (j > 0)
            {

                Entidades.ContratoEmpresas _ContratoEmpresas = EmpDelete.ContratoEmpresas.Take(1).First();
                //if (!_ContratoEmpresas.ComentariosGral.IsLoaded) { _ContratoEmpresas.ComentariosGral.Load(); }
                List<ComentariosGral> comentsGrales = _ContratoEmpresas.ComentariosGral.ToList();
                foreach (ComentariosGral itemComent in comentsGrales)
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


                    int j4 = _CabeceraHojasDeRuta.ContEmpLegajos.Count();
                    while (j4 > 0)
                    {
                        ContEmpLegajos _Leghoja = _CabeceraHojasDeRuta.ContEmpLegajos.Take(1).First();
                        Contexto.DeleteObject(_Leghoja);
                        j4--;
                    }


                    Contexto.DeleteObject(_CabeceraHojasDeRuta);
                    j2--;
                }
                Contexto.DeleteObject(_ContratoEmpresas);
                j--;
            }

            Contexto.DeleteObject(EmpDelete);

            try
            {
                Contexto.SaveChanges();
                FiltrarEmpresas(txtApellidoLegajo.Text.Trim());
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "eliminacionEmpresa", "radalert('La emprsa no puede ser eliminada ya que posee mucha información asociada, por favor tome contacto con el administrador del sistema.',300,150)", true);
                FiltrarEmpresas(txtApellidoLegajo.Text.Trim());
            }

        }
        if (e.Argument == "Insert")
        {

            Entidades.Empresa EmpInsert = new Entidades.Empresa();

            int cant = (from l in dc.Empresa
                        where l.CUIT == txtCUIT.Text.Trim()
                        select l).Count();


            if (cant > 0)
            {
                txtCUIT.Attributes.Add("NroExistente", true.ToString());
                upNroCUIT.Update();
                return;
            }
            else
            {
                txtCUIT.Attributes.Add("NroExistente", false.ToString());
                upNroCUIT.Update();
            }


            /// Controles Tipo TextBox
            EmpInsert.RazonSocial = txtRazonSocial.Text.Trim();
            EmpInsert.RepresentanteTecnico = txtTecnico.Text.Trim();
            EmpInsert.PrestacionEmergencia = txtPrestacionEmergencias.Text.Trim();
            EmpInsert.Direccion = txtDireccion.Text.Trim();
            EmpInsert.Telefono = txtTelefono.Text.Trim();
            EmpInsert.Emergencia = txtEmergencias.Text.Trim();
            EmpInsert.CorreoElectronico = txtEmail.Text.Trim();

            /// Controles Tipo Telerik
            EmpInsert.CUIT = txtCUIT.Text;

            /// Controles Tipo Fecha
            EmpInsert.FechaAlta = txtFechaAlta.SelectedDate;


            dc.AddToEmpresa(EmpInsert);
            dc.SaveChanges();


        }

        FiltrarEmpresas(txtApellidoLegajo.Text.Trim());
    }

    public void FiltrarEmpresas(string filtro, bool RefreshContexto = false)
    {

        List<Empresa> DatosEmpresasFiltrados = (from emp in Contexto.Empresa
                                                where emp.RazonSocial.StartsWith(filtro)
                                                orderby emp.RazonSocial
                                                select emp).ToList();

        this.RadGrid1.DataSource = DatosEmpresasFiltrados.Take(RadGrid1.PageSize);
        this.RadGrid1.DataBind();
        this.updpnlGrilla.Update();
    }

}



