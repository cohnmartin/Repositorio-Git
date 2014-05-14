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

public partial class ABMIngresosEventuales : System.Web.UI.Page
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

        GridIngresos.ExportToExcel += new ControlsAjaxNotti.ClickEventHandler(GridIngresos_ExportToExcel);

        if (!IsPostBack)
        {
            var a = Contexto;
            GridIngresos.DataSource = (GetData("", "-1", 0, GridIngresos.PageSize) as IList);
        }
    }

    void GridIngresos_ExportToExcel(object sender)
    {
        EntidadesConosud Contexto = (HttpContext.Current.Session["Contexto"] as EntidadesConosud);

        GridIngresos.ExportToExcelFunction("IngresosEventuales", (GetData("", "-1", 0, 10000) as IList));

    }
    
    protected void cboEmpresa_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {

        //long idEmpresaContratista = 0;
        //List<MaestroGenerico> Empresas = new List<MaestroGenerico>();
        //if (this.Session["TipoUsuario"].ToString() == "Cliente")
        //{
        //    idEmpresaContratista = long.Parse(Session["IdEmpresaContratista"].ToString());
        //    Empresas = (from l in Contexto.Empresa
        //                where l.RazonSocial.StartsWith(e.Text)
        //                && l.IdEmpresa == idEmpresaContratista
        //                orderby l.RazonSocial
        //                select new MaestroGenerico
        //                {
        //                    Denominacion = l.RazonSocial,
        //                    Id = l.IdEmpresa
        //                }).Take(10).ToList();
        //}
        //else
        //{
        //    Empresas = (from l in Contexto.Empresa
        //                where l.RazonSocial.StartsWith(e.Text)
        //                orderby l.RazonSocial
        //                select new MaestroGenerico
        //                {
        //                    Denominacion = l.RazonSocial,
        //                    Id = l.IdEmpresa
        //                }).Take(10).ToList();
        //}

        //cboEmpresa.Items.Clear();
        //if (Empresas.Count() > 0)
        //{
        //    foreach (var item in Empresas)
        //    {
        //        cboEmpresa.Items.Add(new RadComboBoxItem(item.Denominacion, item.Id.ToString()));
        //    }
        //}
        //else
        //{
        //    cboEmpresa.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
        //}

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
        //long idEmpresa = long.Parse(e.Text);


        ///// Cargo los contratos para la empresa seleccionada
        //var Contratos = (from c in Contexto.ContratoEmpresas
        //                 where c.IdEmpresa == idEmpresa
        //                 select new
        //                 {
        //                     c.Contrato.Codigo,
        //                     c.Contrato.IdContrato,
        //                 }).Distinct();

        //cboContrato.Items.Clear();
        //if (Contratos.Count() > 0)
        //{
        //    foreach (var item in Contratos)
        //    {
        //        cboContrato.Items.Add(new RadComboBoxItem(item.Codigo, item.IdContrato.ToString()));
        //    }
        //}
        //else
        //{
        //    cboContrato.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
        //}


    }


    [WebMethod(EnableSession = true)]
    public static object Grabar(IDictionary<string, object> Datos, string id, int start, int take)
    {
        EntidadesConosud Contexto = (HttpContext.Current.Session["Contexto"] as EntidadesConosud);
        IngresosEventuales CurrentIngresoEventual = null;

        if (id != "")
        {
            long idIngresoEventual = long.Parse(id);

            CurrentIngresoEventual = (from L in Contexto.IngresosEventuales
                                      where L.IdIngresoEventual == idIngresoEventual
                               select L).FirstOrDefault<IngresosEventuales>();
        }
        else
        {
            CurrentIngresoEventual = new IngresosEventuales();
            Contexto.AddToIngresosEventuales(CurrentIngresoEventual);
        }

        CurrentIngresoEventual.ApellidoNombre = Datos["ApellidoNombre"].ToString();
        CurrentIngresoEventual.DNI = Datos["DNI"].ToString();
        CurrentIngresoEventual.Empresa = Datos["Empresa"].ToString();
        CurrentIngresoEventual.Contrato = Datos["Contrato"].ToString();
        CurrentIngresoEventual.Actividad = Datos["Actividad"].ToString();
        CurrentIngresoEventual.Citadopor = Datos["Citadopor"].ToString();


        if (Datos["FechaSolicitud"] != null)
        {
            CurrentIngresoEventual.FechaSolicitud = Convert.ToDateTime(Datos["FechaSolicitud"].ToString());
        }
        else
            CurrentIngresoEventual.FechaSolicitud = null;


        if (Datos["FechaIngreso1"] != null)
        {
            CurrentIngresoEventual.FechaIngreso1 = Convert.ToDateTime(Datos["FechaIngreso1"].ToString());
        }
        else
            CurrentIngresoEventual.FechaIngreso1 = null;


        if (Datos["FechaIngreso2"] != null)
        {
            CurrentIngresoEventual.FechaIngreso2 = Convert.ToDateTime(Datos["FechaIngreso2"].ToString());
        }
        else
            CurrentIngresoEventual.FechaIngreso2 = null;


        if (Datos["FechaIngreso3"] != null)
        {
            CurrentIngresoEventual.FechaIngreso3 = Convert.ToDateTime(Datos["FechaIngreso3"].ToString());
        }
        else
            CurrentIngresoEventual.FechaIngreso3 = null;


        if (Datos["FechaIngreso4"] != null)
        {
            CurrentIngresoEventual.FechaIngreso4 = Convert.ToDateTime(Datos["FechaIngreso4"].ToString());
        }
        else
            CurrentIngresoEventual.FechaIngreso4 = null;


        if (Datos["FechaIngreso5"] != null)
        {
            CurrentIngresoEventual.FechaIngreso5 = Convert.ToDateTime(Datos["FechaIngreso5"].ToString());
        }
        else
            CurrentIngresoEventual.FechaIngreso5 = null;

        
        Contexto.SaveChanges();

        return GetData("", "-1", start, take);
    }


    [WebMethod(EnableSession = true)]
    public static object GetData(string patente, string nroDoc, int start, int take)
    {

        #region Recupero los Datos
        EntidadesConosud Contexto = (HttpContext.Current.Session["Contexto"] as EntidadesConosud);

        if (patente.Trim() != "" || nroDoc=="-1")
        {
            var ingresos = (from v in Contexto.IngresosEventuales
                             where v.ApellidoNombre.StartsWith(patente)
                             orderby v.ApellidoNombre
                             select v).Skip(start).Take(take).ToList();

            return ingresos;
        }
        else
        {
            var ingresos = (from v in Contexto.IngresosEventuales
                             where v.DNI.StartsWith(nroDoc)
                             orderby v.ApellidoNombre
                             select v).Skip(start).Take(take).ToList();

            return ingresos;
        
        }


        #endregion
    }

    [WebMethod(EnableSession = true)]
    public static object EliminarRegistro(string Id, string patente, int start, int take)
    {
        long id = long.Parse(Id);
        EntidadesConosud Contexto = (HttpContext.Current.Session["Contexto"] as EntidadesConosud);

        IngresosEventuales objEliinar = (from v in Contexto.IngresosEventuales
                                        where v.IdIngresoEventual == id
                                        select v).First();

        Contexto.DeleteObject(objEliinar);
        Contexto.SaveChanges();


        return GetData(patente, "-1",start, take);
    }

}
