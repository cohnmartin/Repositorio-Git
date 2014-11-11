using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;

public partial class ABMVehiculosEquipos : System.Web.UI.Page
{

    public bool EsContratista
    {
        get
        {
            if (this.Session["TipoUsuario"] == "Cliente")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public bool PoseePermisoSoloLectura
    {
        get
        {

            long idUsuario = long.Parse(Session["idusu"].ToString());
            Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Legajos, idUsuario);

            if ((!PermisosPagina.Creacion && !PermisosPagina.Modificacion && PermisosPagina.Lectura))
            {
                return true;
            }
            else
                return false;
        }

    }

    public bool PoseePermisoSoloConsulta
    {
        get
        {

            long idUsuario = long.Parse(Session["idusu"].ToString());
            Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Legajos, idUsuario);

            if ((!PermisosPagina.Creacion && !PermisosPagina.Modificacion && !PermisosPagina.Lectura))
            {
                return true;
            }
            else
                return false;
        }

    }


    public string TipoAlta
    {
        get
        {
            if (Session["TipoAlta"] != null)
                return Session["TipoAlta"].ToString();
            else
            {
                if (Request.QueryString["Tipo"] == "V")
                {
                    Session["TipoAlta"] = "Vehículo";
                }
                else
                {
                    Session["TipoAlta"] = "Equipo";
                }

                return Session["TipoAlta"].ToString();
            }



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

        GridVehiculos.ExportToExcel += new ControlsAjaxNotti.ClickEventHandler(GridVehiculos_ExportToExcel);

        if (!IsPostBack)
        {
            Session["TipoAlta"] = null;

            if (TipoAlta == "Vehículo")
            {
                lblTipoGestion.Text = "GESTION DE VEHICULOS";
                lblTituloCaractesticas.Text = "CARACTERISTICAS DEL VEHICULO";
                lblPropio.Text = "Vehículo Propio?:";
                lblTiuloSeguro.Text = "SEGURO DEL VEHICULO";
                GridVehiculos.FunctionsGral[0].Text = "Nuevo Vehículo";
                CargarCombos("V");
            }
            else
            {
                lblTipoGestion.Text = "GESTION DE EQUIPOS";
                lblTituloCaractesticas.Text = "CARACTERISTICAS DEL EQUIPO";
                lblPropio.Text = "Equipo Propio?:";
                lblTiuloSeguro.Text = "SEGURO DEL EQUIPO";
                GridVehiculos.FunctionsGral[0].Text = "Nuevo Equipo";
                CargarCombos("E");
            }

            IDictionary<string, object> datosiniciales = GetData("", 0, GridVehiculos.PageSize);
            GridVehiculos.VirtualCount = int.Parse(datosiniciales["Cantidad"].ToString());
            GridVehiculos.DataSource = (datosiniciales["Datos"] as IList);

        }

        if (EsContratista)
        {
            RowHeaderAuditoria.Style.Add(HtmlTextWriterStyle.Display, "none");
            RowBodyAuditoria.Style.Add(HtmlTextWriterStyle.Display, "none");
            GridVehiculos.FunctionsColumns.RemoveAt(0);
            GridVehiculos.FunctionsGral.RemoveAt(0);
            btnAplicar.Visible = false;
        }
        else if (!EsContratista && PoseePermisoSoloLectura)
        {
            RowHeaderAuditoria.Style.Add(HtmlTextWriterStyle.Display, "none");
            RowBodyAuditoria.Style.Add(HtmlTextWriterStyle.Display, "none");
            GridVehiculos.FunctionsColumns.RemoveAt(0);
            GridVehiculos.FunctionsGral.RemoveAt(0);

            if (GridVehiculos.FunctionsGral.Count > 1)
                GridVehiculos.FunctionsGral.RemoveAt(1);

            btnAplicar.Visible = false;
        }
        else if (!EsContratista && PoseePermisoSoloConsulta)
        {
            RowHeaderAuditoria.Style.Add(HtmlTextWriterStyle.Display, "none");
            RowBodyAuditoria.Style.Add(HtmlTextWriterStyle.Display, "none");
            GridVehiculos.FunctionsColumns.Clear();

            GridVehiculos.FunctionsGral.RemoveAt(0);

            if (GridVehiculos.FunctionsGral.Count > 1)
                GridVehiculos.FunctionsGral.RemoveAt(1);

            btnAplicar.Visible = false;
        }
        else
        {
            RowHeaderAuditoria.Style.Add(HtmlTextWriterStyle.Display, "block");
            RowBodyAuditoria.Style.Add(HtmlTextWriterStyle.Display, "block");
            btnAplicar.Visible = true;
        }
    }

    void GridVehiculos_ExportToExcel(object sender)
    {
        EntidadesConosud Contexto = (HttpContext.Current.Session["Contexto"] as EntidadesConosud);

        long IdEmpresa = 0;
        if (this.Session["TipoUsuario"].ToString() == "Cliente")
        {
            IdEmpresa = long.Parse(Session["IdEmpresaContratista"].ToString());
        }


        if (IdEmpresa > 0)
        {
            var vehiculos = (from v in Contexto.VahiculosyEquipos
                             where v.Tipo == TipoAlta
                             && v.Empresa.Value == IdEmpresa
                             orderby v.Patente
                             select new
                             {
                                 v.Patente,
                                 NroInterno = v.NroInterno,
                                 DescTipoUnidad = v.objTipoUnidad.Descripcion,
                                 Marca = v.Marca,
                                 NroContrato = v.objContrato.Codigo,
                                 NombreEmpresa = v.objEmpresa.RazonSocial,
                                 v.IdVehiculoEquipo,
                                 v.TipoUnidad,
                                 v.NroChasis,
                                 v.CapacidadCarga,
                                 v.FechaFabricacion,
                                 v.Modelo,
                                 v.NroMotor,
                                 v.NroHabilitacion,
                                 v.PuestoIngreso,
                                 v.FechaHabilitacion,
                                 v.FechaVencimientoHabilitacion,
                                 v.NroHabilitacionEE,
                                 v.FechaHabilitacionEE,
                                 v.FechaVencimientoHabilitacionEE,
                                 v.EsPropio,
                                 v.NombreTitular,
                                 v.AltaEmpresa,
                                 v.BajaEmpresa,
                                 v.EquipamientoAgregado,
                                 v.Tipo,
                                 v.ContratoAfectado,
                                 v.Empresa,
                                 v.HabilitarCredencial,
                                 DescHabilitarCredencial = (v.HabilitarCredencial.Value && DateTime.Now < v.VencimientoCredencial.Value) ? "Sí" : "NO",
                                 v.VencimientoCredencial,
                                 v.NroPolizaSeguro,
                                 v.DescripcionSeguro,
                                 v.CompañiaSeguro,
                                 v.FechaInicialSeguro,
                                 v.FechaVencimientoSeguro,
                                 v.FechaUltimoPagoSeguro,
                                 DescCompañia = v.objCompañiaSeguro.Descripcion
                             });

            GridVehiculos.ExportToExcelFunction(TipoAlta, vehiculos.ToList());

        }
        else
        {
            var vehiculos = (from v in Contexto.VahiculosyEquipos
                             where v.Tipo == TipoAlta
                             orderby v.Patente
                             select new
                             {
                                 v.Patente,
                                 NroInterno = v.NroInterno,
                                 DescTipoUnidad = v.objTipoUnidad.Descripcion,
                                 Marca = v.Marca,
                                 NroContrato = v.objContrato.Codigo,
                                 NombreEmpresa = v.objEmpresa.RazonSocial,
                                 v.IdVehiculoEquipo,
                                 v.TipoUnidad,
                                 v.NroChasis,
                                 v.CapacidadCarga,
                                 v.FechaFabricacion,
                                 v.Modelo,
                                 v.NroMotor,
                                 v.NroHabilitacion,
                                 v.PuestoIngreso,
                                 v.FechaHabilitacion,
                                 v.FechaVencimientoHabilitacion,
                                 v.NroHabilitacionEE,
                                 v.FechaHabilitacionEE,
                                 v.FechaVencimientoHabilitacionEE,
                                 v.EsPropio,
                                 v.NombreTitular,
                                 v.AltaEmpresa,
                                 v.BajaEmpresa,
                                 v.EquipamientoAgregado,
                                 v.Tipo,
                                 v.ContratoAfectado,
                                 v.Empresa,
                                 v.HabilitarCredencial,
                                 DescHabilitarCredencial = (v.HabilitarCredencial.Value && DateTime.Now < v.VencimientoCredencial.Value) ? "Sí" : "NO",
                                 v.VencimientoCredencial,
                                 v.NroPolizaSeguro,
                                 v.DescripcionSeguro,
                                 v.CompañiaSeguro,
                                 v.FechaInicialSeguro,
                                 v.FechaVencimientoSeguro,
                                 v.FechaUltimoPagoSeguro,
                                 DescCompañia = v.objCompañiaSeguro.Descripcion
                             });


            GridVehiculos.ExportToExcelFunction(TipoAlta, vehiculos.ToList());
        }




    }

    private void CargarCombos(string Tipo)
    {

        // cboTipoUnidad,cboSeguro, cboEmpresa
        cboTipoUnidad.DataTextField = "Descripcion";
        cboTipoUnidad.DataValueField = "IdClasificacion";
        if (Tipo == "V")
        {
            cboTipoUnidad.DataSource = (from c in Contexto.Clasificacion
                                        where c.Tipo == "Tipo Vehiculos"
                                        select c).ToList();
        }
        else
        {
            cboTipoUnidad.DataSource = (from c in Contexto.Clasificacion
                                        where c.Tipo == "Tipo Equipos"
                                        select c).ToList();
        }


        cboCompañia.DataSource = (from c in Contexto.Clasificacion where c.Tipo == "Compañias Seguro" select c).ToList();
        cboCompañia.DataBind();

        cboTipoUnidad.DataBind();

    }

    protected void cboEmpresa_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {

        long idEmpresaContratista = 0;
        List<MaestroGenerico> Empresas = new List<MaestroGenerico>();
        if (this.Session["TipoUsuario"].ToString() == "Cliente")
        {
            idEmpresaContratista = long.Parse(Session["IdEmpresaContratista"].ToString());
            Empresas = (from l in Contexto.Empresa
                        where l.RazonSocial.StartsWith(e.Text)
                        && l.IdEmpresa == idEmpresaContratista
                        orderby l.RazonSocial
                        select new MaestroGenerico
                        {
                            Denominacion = l.RazonSocial,
                            Id = l.IdEmpresa
                        }).Take(10).ToList();
        }
        else
        {
            Empresas = (from l in Contexto.Empresa
                        where l.RazonSocial.StartsWith(e.Text)
                        orderby l.RazonSocial
                        select new MaestroGenerico
                        {
                            Denominacion = l.RazonSocial,
                            Id = l.IdEmpresa
                        }).Take(10).ToList();
        }

        cboEmpresa.Items.Clear();
        if (Empresas.Count() > 0)
        {
            foreach (var item in Empresas)
            {
                cboEmpresa.Items.Add(new RadComboBoxItem(item.Denominacion, item.Id.ToString()));
            }
        }
        else
        {
            cboEmpresa.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
        }

    }

    protected void cboSeguros_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        //long idEmpresa = long.Parse(e.Text);


        ///// Cargo los seguros dados de alta para la empresa seleccionada
        //var Seguros = (from s in Contexto.Seguros
        //               where s.EmpresaContratista == idEmpresa
        //               select new
        //               {
        //                   Descripcion = s.objCompañia.Descripcion + " " + s.NroPoliza + " " + s.objTipoSeguro.Descripcion,
        //                   s.IdSeguro
        //               }).Distinct();

        //cboSeguro.Items.Clear();
        //if (Seguros.Count() > 0)
        //{
        //    foreach (var item in Seguros)
        //    {
        //        cboSeguro.Items.Add(new RadComboBoxItem(item.Descripcion, item.IdSeguro.ToString()));
        //    }
        //}
        //else
        //{
        //    cboSeguro.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
        //}


    }

    protected void cboContratos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {


        if (e.Text != "")
        {
            long idEmpresa = long.Parse(e.Text);
            DateTime fechaAlta = DateTime.Parse("10/10/2080");

            /// Cargo los contratos para la empresa seleccionada
            var Contratos = (from c in Contexto.ContratoEmpresas
                             where c.Empresa.IdEmpresa == idEmpresa
                             && (c.Contrato.FechaVencimiento >= DateTime.Now || (c.Contrato.Prorroga.HasValue && c.Contrato.Prorroga > DateTime.Now))
                             select new
                             {
                                 c.Contrato.Codigo,
                                 c.Contrato.IdContrato,
                                 FechaVencimiento = c.Contrato != null ? (c.Contrato.Prorroga != null && c.Contrato.Prorroga > c.Contrato.FechaVencimiento ? c.Contrato.Prorroga : c.Contrato.FechaVencimiento) : fechaAlta,
                             }).Distinct();

            cboContrato.Items.Clear();
            if (Contratos.Count() > 0)
            {
                foreach (var item in Contratos)
                {
                    RadComboBoxItem a = new RadComboBoxItem("", "");
                    a.Value = item.IdContrato.ToString();
                    a.Text = item.Codigo + " - FV: " + item.FechaVencimiento.Value.ToShortDateString();
                    a.Attributes.Add("FechaMinima", item.FechaVencimiento.Value.ToShortDateString());
                    cboContrato.Items.Add(a);
                }
            }
            else
            {
                cboContrato.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
            }

        }
    }


    [WebMethod(EnableSession = true)]
    public static object Grabar(IDictionary<string, object> Datos, string id, int start, int take)
    {
        EntidadesConosud Contexto = (HttpContext.Current.Session["Contexto"] as EntidadesConosud);
        VahiculosyEquipos CurrentVehiEqui = null;

        if (id != "")
        {
            long idVehiculo = long.Parse(id);

            CurrentVehiEqui = (from L in Contexto.VahiculosyEquipos
                               where L.IdVehiculoEquipo == idVehiculo
                               select L).FirstOrDefault<VahiculosyEquipos>();
        }
        else
        {
            CurrentVehiEqui = new VahiculosyEquipos();
            CurrentVehiEqui.Tipo = (HttpContext.Current.Session["TipoAlta"] as string);
            CurrentVehiEqui.FechaCreacion = DateTime.Now;
            CurrentVehiEqui.FechaUltimaActualizacion = DateTime.Now;
            Contexto.AddToVahiculosyEquipos(CurrentVehiEqui);
        }

        bool hayCambios = false;
        hayCambios = CurrentVehiEqui.CapacidadCarga != Datos["CapacidadCarga"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.CapacidadCarga = Datos["CapacidadCarga"].ToString();
        hayCambios = CurrentVehiEqui.EquipamientoAgregado != Datos["EquipamientoAgregado"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.EquipamientoAgregado = Datos["EquipamientoAgregado"].ToString();
        hayCambios = CurrentVehiEqui.EsPropio != bool.Parse(Datos["EsPropio"].ToString()) ? true : hayCambios != true ? false : true; CurrentVehiEqui.EsPropio = bool.Parse(Datos["EsPropio"].ToString());
        hayCambios = CurrentVehiEqui.Marca != Datos["Marca"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.Marca = Datos["Marca"].ToString();
        hayCambios = CurrentVehiEqui.Modelo != Datos["Modelo"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.Modelo = Datos["Modelo"].ToString();
        hayCambios = CurrentVehiEqui.NombreTitular != Datos["NombreTitular"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.NombreTitular = Datos["NombreTitular"].ToString();
        hayCambios = CurrentVehiEqui.NroChasis != Datos["NroChasis"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.NroChasis = Datos["NroChasis"].ToString();
        hayCambios = CurrentVehiEqui.NroHabilitacion != Datos["NroHabilitacion"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.NroHabilitacion = Datos["NroHabilitacion"].ToString();
        hayCambios = CurrentVehiEqui.PuestoIngreso != Datos["PuestoIngreso"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.PuestoIngreso = Datos["PuestoIngreso"].ToString();
        hayCambios = CurrentVehiEqui.NroHabilitacionEE != Datos["NroHabilitacionEE"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.NroHabilitacionEE = Datos["NroHabilitacionEE"].ToString();
        hayCambios = CurrentVehiEqui.NroMotor != Datos["NroMotor"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.NroMotor = Datos["NroMotor"].ToString();
        hayCambios = CurrentVehiEqui.Patente != Datos["Patente"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.Patente = Datos["Patente"].ToString();
        hayCambios = CurrentVehiEqui.NombreTitular != Datos["NombreTitular"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.NombreTitular = Datos["NombreTitular"].ToString();
        hayCambios = CurrentVehiEqui.NroPolizaSeguro != Datos["NroPolizaSeguro"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.NroPolizaSeguro = Datos["NroPolizaSeguro"].ToString();
        hayCambios = CurrentVehiEqui.DescripcionSeguro != Datos["DescripcionSeguro"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.DescripcionSeguro = Datos["DescripcionSeguro"].ToString();
        hayCambios = CurrentVehiEqui.HabilitarCredencial != bool.Parse(Datos["HabilitarCredencial"].ToString()) ? true : hayCambios != true ? false : true; CurrentVehiEqui.HabilitarCredencial = bool.Parse(Datos["HabilitarCredencial"].ToString());
        hayCambios = CurrentVehiEqui.NroInterno.ToString() != Datos["NroInterno"].ToString() ? true : hayCambios != true ? false : true; CurrentVehiEqui.NroInterno = Datos["NroInterno"].ToString() != "" ? long.Parse(Datos["NroInterno"].ToString()) : 0;



        if (Datos["TipoUnidad"] != null)
        {
            hayCambios = CurrentVehiEqui.TipoUnidad != long.Parse(Datos["TipoUnidad"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.TipoUnidad = long.Parse(Datos["TipoUnidad"].ToString());

        }
        else
            CurrentVehiEqui.TipoUnidad = null;


        if (Datos["Empresa"] != null)
        {
            hayCambios = CurrentVehiEqui.Empresa != long.Parse(Datos["Empresa"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.Empresa = long.Parse(Datos["Empresa"].ToString());
        }
        else
            CurrentVehiEqui.Empresa = null;


        if (Datos["ContratoAfectado"] != null)
        {
            if (Datos["ContratoAfectado"].ToString() != "")
            {
                hayCambios = CurrentVehiEqui.ContratoAfectado != long.Parse(Datos["ContratoAfectado"].ToString()) ? true : hayCambios != true ? false : true;
                CurrentVehiEqui.ContratoAfectado = long.Parse(Datos["ContratoAfectado"].ToString());
            }
        }
        else
            CurrentVehiEqui.ContratoAfectado = null;



        if (Datos["CompañiaSeguro"] != null)
        {
            hayCambios = CurrentVehiEqui.CompañiaSeguro != long.Parse(Datos["CompañiaSeguro"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.CompañiaSeguro = long.Parse(Datos["CompañiaSeguro"].ToString());
        }
        else
            CurrentVehiEqui.CompañiaSeguro = null;


        if (Datos["FechaInicialSeguro"] != null)
        {
            hayCambios = CurrentVehiEqui.FechaInicialSeguro != Convert.ToDateTime(Datos["FechaInicialSeguro"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.FechaInicialSeguro = Convert.ToDateTime(Datos["FechaInicialSeguro"].ToString());
        }
        else
            CurrentVehiEqui.FechaInicialSeguro = null;

        if (Datos["FechaVencimientoSeguro"] != null)
        {
            hayCambios = CurrentVehiEqui.FechaVencimientoSeguro != Convert.ToDateTime(Datos["FechaVencimientoSeguro"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.FechaVencimientoSeguro = Convert.ToDateTime(Datos["FechaVencimientoSeguro"].ToString());
        }
        else
            CurrentVehiEqui.FechaVencimientoSeguro = null;


        if (Datos["FechaUltimoPagoSeguro"] != null)
        {
            hayCambios = CurrentVehiEqui.FechaUltimoPagoSeguro != Convert.ToDateTime(Datos["FechaUltimoPagoSeguro"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.FechaUltimoPagoSeguro = Convert.ToDateTime(Datos["FechaUltimoPagoSeguro"].ToString());
        }
        else
            CurrentVehiEqui.FechaUltimoPagoSeguro = null;


        if (Datos["FechaFabricacion"] != null)
        {
            hayCambios = CurrentVehiEqui.FechaFabricacion != Convert.ToDateTime(Datos["FechaFabricacion"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.FechaFabricacion = Convert.ToDateTime(Datos["FechaFabricacion"].ToString());
        }
        else
            CurrentVehiEqui.FechaFabricacion = null;


        if (Datos["FechaHabilitacion"] != null)
        {
            hayCambios = CurrentVehiEqui.FechaHabilitacion != Convert.ToDateTime(Datos["FechaHabilitacion"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.FechaHabilitacion = Convert.ToDateTime(Datos["FechaHabilitacion"].ToString());
        }
        else
            CurrentVehiEqui.FechaHabilitacion = null;

        if (Datos["FechaVencimientoHabilitacion"] != null)
        {
            hayCambios = CurrentVehiEqui.FechaVencimientoHabilitacion != Convert.ToDateTime(Datos["FechaVencimientoHabilitacion"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.FechaVencimientoHabilitacion = Convert.ToDateTime(Datos["FechaVencimientoHabilitacion"].ToString());
        }
        else
            CurrentVehiEqui.FechaVencimientoHabilitacion = null;


        if (Datos["AltaEmpresa"] != null)
        {
            hayCambios = CurrentVehiEqui.AltaEmpresa != Convert.ToDateTime(Datos["AltaEmpresa"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.AltaEmpresa = Convert.ToDateTime(Datos["AltaEmpresa"].ToString());
        }
        else
            CurrentVehiEqui.AltaEmpresa = null;


        if (Datos["BajaEmpresa"] != null)
        {
            hayCambios = CurrentVehiEqui.BajaEmpresa != Convert.ToDateTime(Datos["BajaEmpresa"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.BajaEmpresa = Convert.ToDateTime(Datos["BajaEmpresa"].ToString());
        }
        else
            CurrentVehiEqui.BajaEmpresa = null;


        if (Datos["VencimientoCredencial"] != null)
        {
            hayCambios = CurrentVehiEqui.VencimientoCredencial != Convert.ToDateTime(Datos["VencimientoCredencial"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.VencimientoCredencial = Convert.ToDateTime(Datos["VencimientoCredencial"].ToString());
        }
        else
            CurrentVehiEqui.VencimientoCredencial = null;


        if (Datos["FechaHabilitacionEE"] != null)
        {
            hayCambios = CurrentVehiEqui.FechaHabilitacionEE != Convert.ToDateTime(Datos["FechaHabilitacionEE"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.FechaHabilitacionEE = Convert.ToDateTime(Datos["FechaHabilitacionEE"].ToString());
        }
        else
            CurrentVehiEqui.FechaHabilitacionEE = null;

        if (Datos["FechaVencimientoHabilitacionEE"] != null)
        {
            hayCambios = CurrentVehiEqui.FechaVencimientoHabilitacionEE != Convert.ToDateTime(Datos["FechaVencimientoHabilitacionEE"].ToString()) ? true : hayCambios != true ? false : true;
            CurrentVehiEqui.FechaVencimientoHabilitacionEE = Convert.ToDateTime(Datos["FechaVencimientoHabilitacionEE"].ToString());
        }
        else
            CurrentVehiEqui.FechaVencimientoHabilitacionEE = null;


        if (hayCambios)
            CurrentVehiEqui.FechaUltimaActualizacion = DateTime.Now;

        Contexto.SaveChanges();


        #region Recupero los Datos

        string tipoUsuario = HttpContext.Current.Session["TipoUsuario"].ToString();
        string tipoAlta = HttpContext.Current.Session["TipoAlta"].ToString();
        long IdEmpresa = 0;

        if (tipoUsuario == "Cliente")
        {
            IdEmpresa = long.Parse(HttpContext.Current.Session["IdEmpresaContratista"].ToString());
        }


        if (IdEmpresa > 0)
        {
            var vehiculos = (from v in Contexto.VahiculosyEquipos
                             where v.Patente.StartsWith("")
                             && v.Tipo == tipoAlta
                             && v.Empresa.Value == IdEmpresa
                             orderby v.Patente
                             select new
                             {
                                 v.Patente,
                                 NroInterno = v.NroInterno,
                                 DescTipoUnidad = v.objTipoUnidad.Descripcion,
                                 Marca = v.Marca,
                                 NroContrato = v.objContrato.Codigo,
                                 NombreEmpresa = v.objEmpresa.RazonSocial,
                                 v.IdVehiculoEquipo,
                                 v.TipoUnidad,
                                 v.NroChasis,
                                 v.CapacidadCarga,
                                 v.FechaFabricacion,
                                 v.Modelo,
                                 v.NroMotor,
                                 v.NroHabilitacion,
                                 v.PuestoIngreso,
                                 v.FechaHabilitacion,
                                 v.FechaVencimientoHabilitacion,
                                 v.NroHabilitacionEE,
                                 v.FechaHabilitacionEE,
                                 v.FechaVencimientoHabilitacionEE,
                                 v.EsPropio,
                                 v.NombreTitular,
                                 v.AltaEmpresa,
                                 v.BajaEmpresa,
                                 v.EquipamientoAgregado,
                                 v.Tipo,
                                 v.ContratoAfectado,
                                 v.Empresa,
                                 v.HabilitarCredencial,
                                 DescHabilitarCredencial = (v.HabilitarCredencial.Value && DateTime.Now < v.VencimientoCredencial.Value) ? "Sí" : "NO",
                                 v.VencimientoCredencial,
                                 v.NroPolizaSeguro,
                                 v.DescripcionSeguro,
                                 v.CompañiaSeguro,
                                 v.FechaInicialSeguro,
                                 v.FechaVencimientoSeguro,
                                 v.FechaUltimoPagoSeguro,
                                 DescCompañia = v.objCompañiaSeguro.Descripcion
                             }).Skip(start).Take(take).ToList();

            return vehiculos;

        }
        else
        {
            var vehiculos = (from v in Contexto.VahiculosyEquipos
                             where v.Patente.StartsWith("")
                             && v.Tipo == tipoAlta
                             orderby v.Patente
                             select new
                             {
                                 v.Patente,
                                 NroInterno = v.NroInterno,
                                 DescTipoUnidad = v.objTipoUnidad.Descripcion,
                                 Marca = v.Marca,
                                 NroContrato = v.objContrato.Codigo,
                                 NombreEmpresa = v.objEmpresa.RazonSocial,
                                 v.IdVehiculoEquipo,
                                 v.TipoUnidad,
                                 v.NroChasis,
                                 v.CapacidadCarga,
                                 v.FechaFabricacion,
                                 v.Modelo,
                                 v.NroMotor,
                                 v.NroHabilitacion,
                                 v.PuestoIngreso,
                                 v.FechaHabilitacion,
                                 v.FechaVencimientoHabilitacion,
                                 v.NroHabilitacionEE,
                                 v.FechaHabilitacionEE,
                                 v.FechaVencimientoHabilitacionEE,
                                 v.EsPropio,
                                 v.NombreTitular,
                                 v.AltaEmpresa,
                                 v.BajaEmpresa,
                                 v.EquipamientoAgregado,
                                 v.Tipo,
                                 v.ContratoAfectado,
                                 v.Empresa,
                                 v.HabilitarCredencial,
                                 DescHabilitarCredencial = (v.HabilitarCredencial.Value && DateTime.Now < v.VencimientoCredencial.Value) ? "Sí" : "NO",
                                 v.VencimientoCredencial,
                                 v.NroPolizaSeguro,
                                 v.DescripcionSeguro,
                                 v.CompañiaSeguro,
                                 v.FechaInicialSeguro,
                                 v.FechaVencimientoSeguro,
                                 v.FechaUltimoPagoSeguro,
                                 DescCompañia = v.objCompañiaSeguro.Descripcion

                             }).Skip(start).Take(take).ToList();

            Dictionary<string, object> datos = new Dictionary<string, object>();
            datos.Add("Datos", vehiculos);
            return datos;
        }
        #endregion
    }

    [WebMethod(EnableSession = true)]
    public static int GetDataCount(string patente)
    {

        #region Recupero los Datos
        EntidadesConosud Contexto = (HttpContext.Current.Session["Contexto"] as EntidadesConosud);
        string tipoUsuario = HttpContext.Current.Session["TipoUsuario"].ToString();
        string tipoAlta = HttpContext.Current.Session["TipoAlta"].ToString();
        long IdEmpresa = 0;

        if (tipoUsuario == "Cliente")
        {
            IdEmpresa = long.Parse(HttpContext.Current.Session["IdEmpresaContratista"].ToString());
        }


        if (IdEmpresa > 0)
        {
            int vehiculos = (from v in Contexto.VahiculosyEquipos
                             where v.Patente.StartsWith(patente)
                             && v.Tipo == tipoAlta
                             && v.Empresa.Value == IdEmpresa
                             orderby v.Patente
                             select new
                             {
                                 v.Patente,
                                 NroInterno = v.NroInterno,
                                 DescTipoUnidad = v.objTipoUnidad.Descripcion,
                                 Marca = v.Marca,
                                 NroContrato = v.objContrato.Codigo,
                                 NombreEmpresa = v.objEmpresa.RazonSocial,
                                 v.IdVehiculoEquipo,
                                 v.TipoUnidad,
                                 v.NroChasis,
                                 v.CapacidadCarga,
                                 v.FechaFabricacion,
                                 v.Modelo,
                                 v.NroMotor,
                                 v.NroHabilitacion,
                                 v.PuestoIngreso,
                                 v.FechaHabilitacion,
                                 v.FechaVencimientoHabilitacion,
                                 v.NroHabilitacionEE,
                                 v.FechaHabilitacionEE,
                                 v.FechaVencimientoHabilitacionEE,
                                 v.EsPropio,
                                 v.NombreTitular,
                                 v.AltaEmpresa,
                                 v.BajaEmpresa,
                                 v.EquipamientoAgregado,
                                 v.Tipo,
                                 v.ContratoAfectado,
                                 v.Empresa,
                                 v.HabilitarCredencial,
                                 DescHabilitarCredencial = (v.HabilitarCredencial.Value && DateTime.Now < v.VencimientoCredencial.Value) ? "Sí" : "NO",
                                 v.VencimientoCredencial,
                                 v.NroPolizaSeguro,
                                 v.DescripcionSeguro,
                                 v.CompañiaSeguro,
                                 v.FechaInicialSeguro,
                                 v.FechaVencimientoSeguro,
                                 v.FechaUltimoPagoSeguro,
                                 DescCompañia = v.objCompañiaSeguro.Descripcion
                             }).Count();

            return vehiculos;

        }
        else
        {
            int vehiculos = (from v in Contexto.VahiculosyEquipos
                             where v.Patente.StartsWith(patente)
                             && v.Tipo == tipoAlta
                             orderby v.Patente
                             select new
                             {
                                 v.Patente,
                                 NroInterno = v.NroInterno,
                                 DescTipoUnidad = v.objTipoUnidad.Descripcion,
                                 Marca = v.Marca,
                                 NroContrato = v.objContrato.Codigo,
                                 NombreEmpresa = v.objEmpresa.RazonSocial,
                                 v.IdVehiculoEquipo,
                                 v.TipoUnidad,
                                 v.NroChasis,
                                 v.CapacidadCarga,
                                 v.FechaFabricacion,
                                 v.Modelo,
                                 v.NroMotor,
                                 v.NroHabilitacion,
                                 v.PuestoIngreso,
                                 v.FechaHabilitacion,
                                 v.FechaVencimientoHabilitacion,
                                 v.NroHabilitacionEE,
                                 v.FechaHabilitacionEE,
                                 v.FechaVencimientoHabilitacionEE,
                                 v.EsPropio,
                                 v.NombreTitular,
                                 v.AltaEmpresa,
                                 v.BajaEmpresa,
                                 v.EquipamientoAgregado,
                                 v.Tipo,
                                 v.ContratoAfectado,
                                 v.Empresa,
                                 v.HabilitarCredencial,
                                 DescHabilitarCredencial = (v.HabilitarCredencial.Value && DateTime.Now < v.VencimientoCredencial.Value) ? "Sí" : "NO",
                                 v.VencimientoCredencial,
                                 v.NroPolizaSeguro,
                                 v.DescripcionSeguro,
                                 v.CompañiaSeguro,
                                 v.FechaInicialSeguro,
                                 v.FechaVencimientoSeguro,
                                 v.FechaUltimoPagoSeguro,
                                 DescCompañia = v.objCompañiaSeguro.Descripcion

                             }).Count();

            return vehiculos;
        }
        #endregion
    }

    [WebMethod(EnableSession = true)]
    public static IDictionary<string, object> GetData(string patente, int start, int take)
    {
        Dictionary<string, object> datos = new Dictionary<string, object>();

        #region Recupero los Datos
        EntidadesConosud Contexto = (HttpContext.Current.Session["Contexto"] as EntidadesConosud);
        string tipoUsuario = HttpContext.Current.Session["TipoUsuario"].ToString();
        string tipoAlta = HttpContext.Current.Session["TipoAlta"].ToString();
        long IdEmpresa = 0;
        DateTime fechaAlta = DateTime.Parse("10/10/2080");

        if (tipoUsuario == "Cliente")
        {
            IdEmpresa = long.Parse(HttpContext.Current.Session["IdEmpresaContratista"].ToString());
        }


        if (IdEmpresa > 0)
        {
            var vehiculos = (from v in Contexto.VahiculosyEquipos
                             where v.Patente.StartsWith(patente)
                             && v.Tipo == tipoAlta
                             && v.Empresa.Value == IdEmpresa
                             orderby v.Patente
                             select new
                             {
                                 v.Patente,
                                 NroInterno = v.NroInterno,
                                 DescTipoUnidad = v.objTipoUnidad.Descripcion,
                                 Marca = v.Marca,
                                 NroContrato = v.objContrato.Codigo,
                                 NombreEmpresa = v.objEmpresa.RazonSocial,
                                 v.IdVehiculoEquipo,
                                 v.TipoUnidad,
                                 v.NroChasis,
                                 v.CapacidadCarga,
                                 v.FechaFabricacion,
                                 v.Modelo,
                                 v.NroMotor,
                                 v.NroHabilitacion,
                                 v.PuestoIngreso,
                                 v.FechaHabilitacion,
                                 v.FechaVencimientoHabilitacion,
                                 v.NroHabilitacionEE,
                                 v.FechaHabilitacionEE,
                                 v.FechaVencimientoHabilitacionEE,
                                 v.EsPropio,
                                 v.NombreTitular,
                                 v.AltaEmpresa,
                                 v.BajaEmpresa,
                                 v.EquipamientoAgregado,
                                 v.Tipo,
                                 v.ContratoAfectado,
                                 v.Empresa,
                                 v.HabilitarCredencial,
                                 DescHabilitarCredencial = (v.HabilitarCredencial.Value && DateTime.Now < v.VencimientoCredencial.Value) ? "Sí" : "NO",
                                 v.VencimientoCredencial,
                                 v.NroPolizaSeguro,
                                 v.DescripcionSeguro,
                                 v.CompañiaSeguro,
                                 v.FechaInicialSeguro,
                                 v.FechaVencimientoSeguro,
                                 v.FechaUltimoPagoSeguro,
                                 DescCompañia = v.objCompañiaSeguro.Descripcion,
                                 FechaVencimientoContrato = v.objContrato != null ? (v.objContrato.Prorroga != null && v.objContrato.Prorroga > v.objContrato.FechaVencimiento ? v.objContrato.Prorroga : v.objContrato.FechaVencimiento) : fechaAlta,
                             }).Skip(start).Take(take).ToList();

            datos.Add("Datos", vehiculos);
            datos.Add("Cantidad", GetDataCount(patente));
            return datos;

        }
        else
        {
            var vehiculos = (from v in Contexto.VahiculosyEquipos
                             where v.Patente.StartsWith(patente)
                             && v.Tipo == tipoAlta
                             orderby v.Patente
                             select new
                             {
                                 v.Patente,
                                 NroInterno = v.NroInterno,
                                 DescTipoUnidad = v.objTipoUnidad.Descripcion,
                                 Marca = v.Marca,
                                 NroContrato = v.objContrato.Codigo,
                                 NombreEmpresa = v.objEmpresa.RazonSocial,
                                 v.IdVehiculoEquipo,
                                 v.TipoUnidad,
                                 v.NroChasis,
                                 v.CapacidadCarga,
                                 v.FechaFabricacion,
                                 v.Modelo,
                                 v.NroMotor,
                                 v.NroHabilitacion,
                                 v.PuestoIngreso,
                                 v.FechaHabilitacion,
                                 v.FechaVencimientoHabilitacion,
                                 v.NroHabilitacionEE,
                                 v.FechaHabilitacionEE,
                                 v.FechaVencimientoHabilitacionEE,
                                 v.EsPropio,
                                 v.NombreTitular,
                                 v.AltaEmpresa,
                                 v.BajaEmpresa,
                                 v.EquipamientoAgregado,
                                 v.Tipo,
                                 v.ContratoAfectado,
                                 v.Empresa,
                                 v.HabilitarCredencial,
                                 DescHabilitarCredencial = (v.HabilitarCredencial.Value && DateTime.Now < v.VencimientoCredencial.Value) ? "Sí" : "NO",
                                 v.VencimientoCredencial,
                                 v.NroPolizaSeguro,
                                 v.DescripcionSeguro,
                                 v.CompañiaSeguro,
                                 v.FechaInicialSeguro,
                                 v.FechaVencimientoSeguro,
                                 v.FechaUltimoPagoSeguro,
                                 DescCompañia = v.objCompañiaSeguro.Descripcion,
                                 FechaVencimientoContrato = v.objContrato != null ? (v.objContrato.Prorroga != null && v.objContrato.Prorroga > v.objContrato.FechaVencimiento ? v.objContrato.Prorroga : v.objContrato.FechaVencimiento) : fechaAlta,

                             }).Skip(start).Take(take).ToList();
            
            datos.Add("Datos", vehiculos);
            datos.Add("Cantidad", GetDataCount(patente));
            return datos;
        }
        #endregion
    }

    [WebMethod(EnableSession = true)]
    public static object EliminarRegistro(string Id, string patente, int start, int take)
    {
        long id = long.Parse(Id);
        EntidadesConosud Contexto = (HttpContext.Current.Session["Contexto"] as EntidadesConosud);

        VahiculosyEquipos objEliinar = (from v in Contexto.VahiculosyEquipos
                                        where v.IdVehiculoEquipo == id
                                        select v).First();

        Contexto.DeleteObject(objEliinar);
        Contexto.SaveChanges();


        return GetData(patente, start, take);
    }

}
