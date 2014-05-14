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

public partial class ConsultDocumentacion : System.Web.UI.Page
{
    private EntidadesConosud _dc = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadEmpresas();

        }


        #region Seguridad
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Documentacion, idUsuario);

        if (PermisosPagina.Lectura && !PermisosPagina.Creacion && !PermisosPagina.Modificacion)
        {
            gridDoc.FunctionsColumns.RemoveAt(0);
            gridDoc.Columns[3].Display = false;
        }
        #endregion
    }


    private void LoadEmpresas()
    {

        cboEmpresas.DataTextField = "RazonSocial";
        cboEmpresas.DataValueField = "IdEmpresa";
        cboEmpresas.DataSource = Helpers.GetEmpresasContratistas(long.Parse(Session["idusu"].ToString()));
        cboEmpresas.DataBind();

        cboEmpresas.Items.Insert(0, new RadComboBoxItem("- Seleccione una Empresa -"));

    }

    private void LoadContratos(int id)
    {

        cboContratos.DataTextField = "Codigo";
        cboContratos.DataValueField = "IdContrato";
        cboContratos.DataSource = Helpers.GetContratos(id);
        cboContratos.DataBind();

        cboContratos.Items.Insert(0, new RadComboBoxItem("- Seleccione un Contrato -"));

    }

    private void LoadContratistas(int id)
    {
        cboContratistas.DataTextField = "RazonSocial";
        cboContratistas.DataValueField = "IdContratoEmpresas";
        cboContratistas.DataSource = Helpers.GetContratistas(id);
        cboContratistas.DataBind();

        cboContratistas.Items.Insert(0, new RadComboBoxItem("- Seleccione un Contratista -"));
    }

    private void LoadPeriodos(int id)
    {
        cboPeriodos.DataTextFormatString = "{0:yyyy/MM}";
        cboPeriodos.DataTextField = "Periodo";
        cboPeriodos.DataValueField = "IdCabeceraHojasDeRuta";
        cboPeriodos.DataSource = Helpers.GetPeriodos(id, long.Parse(this.Session["idusu"].ToString()));
        cboPeriodos.DataBind();

        cboPeriodos.Items.Insert(0, new RadComboBoxItem("- Seleccione un Periodo -"));
    }

    protected void cboEmpresas_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadEmpresas();
    }

    protected void cboContratos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadContratos(int.Parse(e.Text));
    }

    protected void cboContratistas_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadContratistas(int.Parse(e.Text));
    }

    protected void cboPriodos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadPeriodos(int.Parse(e.Text));
    }

    protected void cboPeriodos_SelectedIndexChanged1(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //OnSelectedIndexChanged="cboPeriodos_SelectedIndexChanged1"
        //System.Threading.Thread.Sleep(2000);

        _dc = new EntidadesConosud();
        if (e.Value != "")
        {

            long idUsuario = long.Parse(Session["idusu"].ToString());
            Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Documentacion, idUsuario);

            if (PermisosPagina.Lectura && !PermisosPagina.Creacion && !PermisosPagina.Modificacion)
            {
                gridDoc.FunctionsColumns.RemoveAt(0);
                gridDoc.Columns[3].Display = false;
            }

            int Id = int.Parse(e.Value);

            var cabecera = (from C in _dc.CabeceraHojasDeRuta
                            where C.IdCabeceraHojasDeRuta == Id
                            select C).FirstOrDefault();

            if (cabecera.EsFueraTermino.HasValue)
                chkFueraTermino.Checked = cabecera.EsFueraTermino.Value;

            upFueraTermino.Update();


            var ItemsHoja = (from H in _dc.HojasDeRuta
                             where H.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == Id
                             orderby H.Plantilla.Codigo
                             select new
                             {
                                 IdHoja = H.IdHojaDeRuta,
                                 Titulo = H.Plantilla.Descripcion,
                                 FechaEntrega = H.DocFechaEntrega,
                                 FechaEntregaOriginal = H.DocFechaEntrega,
                                 Comentario = H.DocComentario,
                                 Presento = false

                             }).ToList();

            gridDoc.DataSource = ItemsHoja.ToList();

            Session["datos"] = ItemsHoja;


        }
    }

    [WebMethod]
    public static IDictionary<string, object> GetData(long Id)
    {
        EntidadesConosud _dc = new EntidadesConosud();
        Dictionary<string, object> datos = new Dictionary<string, object>();

        
        var cabecera = (from C in _dc.CabeceraHojasDeRuta
                        where C.IdCabeceraHojasDeRuta == Id
                        select C).FirstOrDefault();

       
        var ItemsHoja = (from H in _dc.HojasDeRuta
                         where H.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == Id
                         orderby H.Plantilla.Codigo
                         select new
                         {
                             IdHoja = H.IdHojaDeRuta,
                             Titulo = H.Plantilla.Descripcion,
                             FechaEntrega = H.DocFechaEntrega,
                             FechaEntregaOriginal = H.DocFechaEntrega,
                             Comentario = H.DocComentario,
                             Presento = false

                         }).ToList();

        datos.Add("Datos", ItemsHoja.ToList());
        if (cabecera.EsFueraTermino.HasValue)
            datos.Add("check", cabecera.EsFueraTermino.Value);
        else
            datos.Add("check", false);

        return datos;

    }


    [WebMethod]
    public static object UpdateData(List<IDictionary<string, object>> datos, bool fueraTermino)
    {

        long idcabecera = 0;
        EntidadesConosud _dc = new EntidadesConosud();
        foreach (IDictionary<string, object> item in datos)
        {


            if (bool.Parse(item["Presento"].ToString()))
            {
                long id = long.Parse(item["IdHoja"].ToString());


                Entidades.HojasDeRuta itemsHoja = (from H in _dc.HojasDeRuta
                                                   where H.IdHojaDeRuta == id
                                                   select H).First<Entidades.HojasDeRuta>();

                itemsHoja.DocFechaEntrega = DateTime.Now;
                itemsHoja.DocComentario = "Sin Comentarios";

                /// al presnetar documentación para una hoja de ruta que esta publicada
                /// se des-publica automaticamente.
                itemsHoja.CabeceraHojasDeRuta.Publicar = false;
                itemsHoja.CabeceraHojasDeRuta.EsFueraTermino = fueraTermino;
                idcabecera = itemsHoja.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta;
            }




            //if ((item.FindControl("chkPresento") as CheckBox).Checked)
            //{
            //    long id = long.Parse(gvItemHoja.Items[item.DataSetIndex].GetDataKeyValue("IdHojaDeRuta").ToString());

            //    Entidades.HojasDeRuta itemsHoja = (from H in _dc.HojasDeRuta
            //                                       where H.IdHojaDeRuta == id
            //                                       select H).First<Entidades.HojasDeRuta>();

            //    itemsHoja.DocFechaEntrega = DateTime.Now;
            //    itemsHoja.DocComentario = "Sin Comentarios";
            //    (item.FindControl("chkPresento") as CheckBox).Checked = false;

            //    /// al presnetar documentación para una hoja de ruta que esta publicada
            //    /// se des-publica automaticamente.
            //    itemsHoja.CabeceraHojasDeRutaReference.Load();
            //    itemsHoja.CabeceraHojasDeRuta.Publicar = false;
            //    itemsHoja.CabeceraHojasDeRuta.EsFueraTermino = chkFueraTermino.Checked;
            //}
        }

        _dc.SaveChanges();


        return (from H in _dc.HojasDeRuta
                where H.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == idcabecera
                orderby H.Plantilla.Codigo
                select new
                {
                    IdHoja = H.IdHojaDeRuta,
                    Titulo = H.Plantilla.Descripcion,
                    FechaEntrega = H.DocFechaEntrega,
                    FechaEntregaOriginal = H.DocFechaEntrega,
                    Comentario = H.DocComentario,
                    Presento = false

                }).ToList();

    }
    [WebMethod]
    public static object UpdateDataItem(IDictionary<string, object> item, long id)
    {

        long idcabecera = 0;
        EntidadesConosud _dc = new EntidadesConosud();

        Entidades.HojasDeRuta itemsHoja = (from H in _dc.HojasDeRuta
                                           where H.IdHojaDeRuta == id
                                           select H).First<Entidades.HojasDeRuta>();

        if (item["FechaEntrega"] != null)
            itemsHoja.DocFechaEntrega = DateTime.Parse(item["FechaEntrega"].ToString());
        else
            itemsHoja.DocFechaEntrega = null;

        itemsHoja.DocComentario = item["Comentario"].ToString();

        /// al presnetar documentación para una hoja de ruta que esta publicada
        /// se des-publica automaticamente.
        itemsHoja.CabeceraHojasDeRuta.Publicar = false;
        idcabecera = itemsHoja.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta;

        _dc.SaveChanges();

        return (from H in _dc.HojasDeRuta
                where H.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == idcabecera
                orderby H.Plantilla.Codigo
                select new
                {
                    IdHoja = H.IdHojaDeRuta,
                    Titulo = H.Plantilla.Descripcion,
                    FechaEntrega = H.DocFechaEntrega,
                    FechaEntregaOriginal = H.DocFechaEntrega,
                    Comentario = H.DocComentario,
                    Presento = false

                }).ToList();

    }

}
