using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;

public partial class GestionPublicacion : System.Web.UI.Page
{
    public class TempPublicaciones
    {
        public long IdCabeceraHojasDeRuta { get; set; }
        public string ConstratistaParaSubConstratista { get; set; }
        public bool EsContratista { get; set; }
        public string ContratoCodigo { get; set; }
        public string EmpresaRazonSocial { get; set; }
        public string EstadoDescripcion { get; set; }
        public DateTime Periodo { get; set; }
        public string Publicada { get; set; }
        public Entidades.ContratoEmpresas ContratoEmpresas { get; set; }
        public Entidades.Empresa Empresa { get; set; }
        public Entidades.CabeceraHojasDeRuta Cabecera { get; set; }
        public string ImagenPublicacion { get; set; }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtAño.Value = DateTime.Now.Year;
            txtMes.Value = DateTime.Now.Month;
        }

        gvCabeceras.DataBound += new EventHandler(gvCabeceras_DataBound);
    }

    void gvCabeceras_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.GestionPublicacion, idUsuario);

        if (PermisosPagina.Lectura && !PermisosPagina.Creacion && !PermisosPagina.Modificacion)
        {
            gvCabeceras.MasterTableView.GetColumn("ClientSelectColumn").Visible = false;
            btnPublicar.Visible = false;
        }

    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        CargarGrilla();
    }

    protected void btnPublicar_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();
        List<long> AllId = new List<long>();
        List<long> IdSel = new List<long>();

        foreach (GridItem item in gvCabeceras.SelectedItems)
        {
            IdSel.Add(long.Parse(gvCabeceras.Items[item.DataSetIndex].GetDataKeyValue("IdCabeceraHojasDeRuta").ToString()));

        }

        foreach (GridDataItem item in gvCabeceras.Items)
        {
            AllId.Add(long.Parse(item.GetDataKeyValue("IdCabeceraHojasDeRuta").ToString()));
        }


        List<Entidades.CabeceraHojasDeRuta> cabs = dc.CabeceraHojasDeRuta.Where(
            Helpers.ContainsExpression<Entidades.CabeceraHojasDeRuta, long>(cab => cab.IdCabeceraHojasDeRuta, AllId)).ToList<Entidades.CabeceraHojasDeRuta>();

        foreach (Entidades.CabeceraHojasDeRuta item in cabs)
        {

            item.Publicar = false;
        }


        cabs = dc.CabeceraHojasDeRuta.Include("HojasDeRuta").Where(
            Helpers.ContainsExpression<Entidades.CabeceraHojasDeRuta, long>(cab => cab.IdCabeceraHojasDeRuta, IdSel)).ToList<Entidades.CabeceraHojasDeRuta>();

        int i = 0;
        foreach (Entidades.CabeceraHojasDeRuta item in cabs)
        {
            item.Publicar = true;
            if (item.EsFueraTermino.HasValue && !item.EsFueraTermino.Value)
            {

                #region Codigo Anterior
                ////// 1. Al publicar si los items de la hoja tiene Doc Recpcionada 
                ////// pero no hay comentario ni items aprobados no se debe borrar la doc recepcionada 
                ////// para que siga apareceindo en el visor y ademas se pueda mostrar a estas hojas como en "poceso de auditoria". 

                //bool ItemsAuditados = (from I in item.HojasDeRuta
                //                       where (I.HojaComentario != null && I.HojaComentario != "")
                //                       || I.HojaFechaAprobacion != null
                //                       select I).Any();

                //if (ItemsAuditados)
                //{
                //    foreach (Entidades.HojasDeRuta itemH in item.HojasDeRuta)
                //    {
                //        itemH.DocComentario = "";
                //        itemH.DocFechaEntrega = null;
                //    }
                //}
                #endregion

                //1. Al publicar, si los items de la hoja poseen Doc Recpcionada 
                //y los mismo tienen comentarios de auditoria, estan aprobados o poseen la auditoria termianda
                //se debe borrar la doc recepcionada. En caso contrario
                //no se debe limpiar la doc para que siga apareceindo en el visor y ademas se pueda mostrar 
                //a esta hoja como en "poceso de auditoria". 
                List<Entidades.HojasDeRuta> ItemsAuditados = (from I in item.HojasDeRuta
                                                              where (I.HojaComentario != null && I.HojaComentario != "")
                                                              || I.HojaFechaAprobacion != null || (I.AuditoriaTerminada.HasValue && I.AuditoriaTerminada.Value)
                                                              select I).ToList();

                if (ItemsAuditados.Count > 0)
                {
                    foreach (Entidades.HojasDeRuta itemH in ItemsAuditados)
                    {
                        itemH.DocComentario = "";
                        itemH.DocFechaEntrega = null;
                    }
                }

                //cuando se publica se debe sacar la marca de auditoria terminada sobre los items
                //no aprobados ya que justamente por ese motivo se publica, porque se termino de controlar.
                //Luego puede pasar que ese item se tenga que volver a auditar porque le falto documentacion
                foreach (Entidades.HojasDeRuta itemH in item.HojasDeRuta)
                {
                    if (!itemH.HojaFechaAprobacion.HasValue)
                        itemH.AuditoriaTerminada = false;
                }

                /// Finalmente se debe acualizar la fecha de la ultima vez
                /// que se publico, si la hoja se ha publicado.
                if (item.Publicar.Value)
                    item.FechaUltimaPubicacion = DateTime.Now;

            }
            else
            {
                item.EsFueraTermino = false;
            }
            i++;
        }

        dc.SaveChanges();
        CargarGrilla();

    }

    protected void gvCabeceras_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            TempPublicaciones CurrentCabecera = (TempPublicaciones)e.Item.DataItem;

            if (bool.Parse(e.Item.Cells[2].Text))
            {
                (e.Item.FindControl("lblSubContratista") as Label).Text = "-";
            }
            //else
            //{
            //    (e.Item.FindControl("lblContratista") as Label).Text = CurrentCabecera.ContratoEmpresas.ConstratistaParaSubConstratista;
            //}

            if (CurrentCabecera.Publicada == "Si")
            {
                e.Item.Selected = true;
                (e.Item.FindControl("imgAprobacion") as Image).Visible = false;
            }
            /// Si no se ha publicado, puede ser que la hoja esta aprobada o no
            else
            {


                // Si la hoja NO esta aprobada
                if (!CurrentCabecera.Cabecera.FechaAprobacion.HasValue)
                {
                    if (!CurrentCabecera.Cabecera.HojasDeRuta.IsLoaded) { CurrentCabecera.Cabecera.HojasDeRuta.Load(); }
                    /// Si posee hojas con documentación recepcionada
                    if (CurrentCabecera.Cabecera.HojasDeRuta.Count(w => w.DocFechaEntrega.HasValue) > 0)
                    {
                        ///  busco si hay items con doc recepcionada porsterior a la fecha 
                        ///  de la ultima publicacion o con documentación recepcionada y que la audirotia 
                        ///  de los mismo no este terminada, con que uno cumpla la condicion no esta lista para publicar.
                        bool PoseeItemsNoTerminados = CurrentCabecera.Cabecera.HojasDeRuta.Any(w =>
                            (
                                (w.DocFechaEntrega.HasValue || w.DocFechaEntrega > w.CabeceraHojasDeRuta.FechaUltimaPubicacion) &&
                                (!w.AuditoriaTerminada.HasValue || w.AuditoriaTerminada.Value == false))
                            );

                        /// Si posee item no terminado entonces no la muestro como hoja a publicar.
                        if (PoseeItemsNoTerminados)
                        {
                            (e.Item.FindControl("imgAprobacion") as Image).Visible = false;
                        }
                        else
                        {
                            (e.Item.FindControl("imgAprobacion") as Image).Visible = true;
                        }
                    }
                    else
                    {
                        /// Verifico si existen item terminados aunque no posean documentacion recepcionada
                        bool PoseeItemsTerminadosSinDoc = CurrentCabecera.Cabecera.HojasDeRuta.Any(w =>
                            ((w.AuditoriaTerminada.HasValue && w.AuditoriaTerminada.Value == true)));

                        if (PoseeItemsTerminadosSinDoc)
                            (e.Item.FindControl("imgAprobacion") as Image).Visible = true;
                        else
                            (e.Item.FindControl("imgAprobacion") as Image).Visible = false;
                    }

                }
                else
                {

                    (e.Item.FindControl("imgAprobacion") as Image).Visible = true;
                    return;

                    /////  Para una hoja de ruta si hay items con doc recepcionada porsterior a la fecha 
                    /////  de la ultima publicacion y la audirotia de los mismo esta terminada, entonces 
                    /////  esta hoja esta lista para ser publicada.
                    /////  Con que uno no cumpla la condicion, no esta lista para publicar.

                    //bool ItemsNoTerminados = CurrentCabecera.Cabecera.HojasDeRuta.Any(w =>
                    //        w.DocFechaEntrega > w.CabeceraHojasDeRuta.FechaUltimaPubicacion
                    //        && w.AuditoriaTerminada.HasValue
                    //        && w.AuditoriaTerminada.Value == false);

                    //if (ItemsNoTerminados)
                    //    (e.Item.FindControl("imgAprobacion") as Image).Visible = false;
                    //else
                    //    (e.Item.FindControl("imgAprobacion") as Image).Visible = true;
                }


            }
        }

    }

    private void CargarGrilla()
    {
        EntidadesConosud dc = new EntidadesConosud();

        DateTime FI = DateTime.Now;
        if (txtMes.Value - 1 > 0)
            FI = DateTime.Parse("01/" + Convert.ToString(txtMes.Value - 1) + "/" + txtAño.Value.ToString());
        else
            FI = DateTime.Parse("01/12/" + Convert.ToString(txtAño.Value - 1));

        DateTime FF = FI.AddMonths(1).AddDays(-1);

        DateTime FIAct = DateTime.Parse("01/" + txtMes.Value.ToString() + "/" + txtAño.Value.ToString());
        DateTime FFAct = FIAct.AddMonths(1).AddDays(-1);


        bool IncHojaPeriodo = chkPeriodo.Checked;
        List<TempPublicaciones> cabsTemp = null;

        if (IncHojaPeriodo)
        {
            /// Busca: las hojas de ruta donde
            ///     Tenga documentacion entregada en el periodo de consulta sin importar el periodo de la hoja o
            ///     que el periodo de la hoja de ruta sea justo el anterior al mes de consulta.
            cabsTemp = (from C in dc.CabeceraHojasDeRuta

                        where (C.HojasDeRuta.Where(h => h.DocFechaEntrega != null & h.DocFechaEntrega >= FIAct && h.DocFechaEntrega <= FFAct).Count() > 0)
                        || (C.Periodo.Month == FI.Month && C.Periodo.Year == FI.Year)
                        select new TempPublicaciones
                        {
                            IdCabeceraHojasDeRuta = C.IdCabeceraHojasDeRuta,
                            ConstratistaParaSubConstratista = "",
                            EsContratista = C.ContratoEmpresas.EsContratista.Value,
                            ContratoCodigo = C.ContratoEmpresas.Contrato.Codigo,
                            EmpresaRazonSocial = C.ContratoEmpresas.Empresa.RazonSocial,
                            EstadoDescripcion = C.Estado.Descripcion,
                            Periodo = C.Periodo,
                            Publicada = "",
                            ContratoEmpresas = C.ContratoEmpresas,
                            Empresa = C.ContratoEmpresas.Empresa,
                            Cabecera = C
                        }).ToList();

        }
        else
        {
            /// Busca: las hojas de ruta donde
            ///     Tenga documentacion entregada en el periodo de consulta sin importar el periodo de la hoja
            cabsTemp = (from C in dc.CabeceraHojasDeRuta
                        where (C.HojasDeRuta.Where(h => h.DocFechaEntrega != null & h.DocFechaEntrega >= FIAct && h.DocFechaEntrega <= FFAct).Count() > 0)
                        select new TempPublicaciones
                        {
                            IdCabeceraHojasDeRuta = C.IdCabeceraHojasDeRuta,
                            ConstratistaParaSubConstratista = "",
                            EsContratista = C.ContratoEmpresas.EsContratista.Value,
                            ContratoCodigo = C.ContratoEmpresas.Contrato.Codigo,
                            EmpresaRazonSocial = C.ContratoEmpresas.Empresa.RazonSocial,
                            EstadoDescripcion = C.Estado.Descripcion,
                            Periodo = C.Periodo,
                            Publicada = "",
                            ContratoEmpresas = C.ContratoEmpresas,
                            Empresa = C.ContratoEmpresas.Empresa,
                            Cabecera = C
                        }).ToList();
        }


        foreach (TempPublicaciones item in cabsTemp.Distinct().ToList())
        {
            item.ConstratistaParaSubConstratista = item.ContratoEmpresas.ConstratistaParaSubConstratista;
            item.Publicada = item.Cabecera.Publicada;
        }

        cabsTemp = cabsTemp.Distinct().ToList();

        if (cabsTemp.Count > 0)
        {
            gvCabeceras.DataSource = cabsTemp;
            gvCabeceras.DataBind();
            trReporte.Visible = true;
            trResultadoVacio.Visible = false;
        }
        else
        {
            trReporte.Visible = false;
            trResultadoVacio.Visible = true;
        }


        //cabs = cabs.Distinct(new Helpers.ComparerByContratoEmpresa()).ToList<Entidades.CabeceraHojasDeRuta>();

        ////orderby C.ContratoEmpresas.Empresa.RazonSocial, C.ContratoEmpresas.Contrato.Codigo, C.ContratoEmpresas.EsContratista descending
        //if (cabs.Count > 0)
        //{
        //    gvCabeceras.DataSource = cabs;
        //    gvCabeceras.DataBind();
        //    trReporte.Visible = true;
        //    trResultadoVacio.Visible = false;
        //}
        //else
        //{
        //    trReporte.Visible = false;
        //    trResultadoVacio.Visible = true;
        //}

        upResultado.Update();
    }

    public class ComparerByContratoEmpresa : IEqualityComparer<TempPublicaciones>
    {
        public bool Equals(TempPublicaciones x, TempPublicaciones y)
        {
            return x.ContratoEmpresas.IdContratoEmpresas == y.ContratoEmpresas.IdContratoEmpresas;
        }
        public int GetHashCode(TempPublicaciones obj)
        {
            return obj.ContratoEmpresas.IdContratoEmpresas.GetHashCode();
        }
    }
}
