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

public partial class ReporteEstadoHojas : System.Web.UI.Page
{
    public Hashtable EmpresasContratista = new Hashtable();
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
    public List<EstadosRutaTemp> DetallesEstados
    {
        get
        {
            if (Session["DetallesEstados"] != null)
                return (List<EstadosRutaTemp>)Session["DetallesEstados"];
            else
            {
                Session["DetallesEstados"] = new List<EstadosRutaTemp>();
                return (List<EstadosRutaTemp>)Session["DetallesEstados"];
            }
        }
        set
        {
            Session["DetallesEstados"] = value;

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            for (int i = 0; i < 12; i++)
            {
                string fecha = string.Format("{0:MM/yyyy}", DateTime.Now.AddMonths(-1 * i));
                cboPeriodos.Items.Add(new Telerik.Web.UI.RadComboBoxItem(fecha, fecha));
            }


            gvEstadoContratos.DataSource = new List<EstadosRutaTemp>();
            gvEstadoContratos.DataBind();
        }
    }

    private string GetNombreContratista(bool? esContratista, string CodigoContrato, string NombreEmpresa)
    {
        if (esContratista.HasValue && esContratista.Value)
        {
            if (!EmpresasContratista.ContainsKey(CodigoContrato))
            {
                EmpresasContratista.Add(CodigoContrato, NombreEmpresa);
            }
            return NombreEmpresa;
        }
        else
        {
            if (EmpresasContratista[CodigoContrato] != null)
                return EmpresasContratista[CodigoContrato].ToString();
            else
                return "";
        }
    }

    public string GetNombreSubContratista(bool? esContratista, string CodigoContrato, string NombreEmpresa)
    {
        if (!esContratista.HasValue || !esContratista.Value)
        {
            return NombreEmpresa;
        }
        else
        {
            return "";
        }
    }

    public string GetFormatoFecha(DateTime? fecha)
    {

        if (fecha != null)
        {
            return fecha.Value.ToShortDateString();
        }
        else
            return "";
    }
    
    protected void cboPeriodos_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {

        /// Busca: las hojas de ruta donde el
        /// periodo coincida con el periodod de consulta         

        List<EstadosRutaTemp> DetallesEstadosTemp = new List<EstadosRutaTemp>();
        int mes = int.Parse(cboPeriodos.SelectedItem.Value.Split('/')[0]);
        int año = int.Parse(cboPeriodos.SelectedItem.Value.Split('/')[1]);

        var a = (from C in Contexto.CabeceraHojasDeRuta
                 where (C.Periodo.Month == mes && C.Periodo.Year == año)
                 //&& C.ContratoEmpresas.Contrato.Codigo == "4600002679"
                 orderby C.ContratoEmpresas.EsContratista.Value descending
                 select new
                 {
                     CodigoContrato = C.ContratoEmpresas.Contrato.Codigo,
                     Estado = C.Estado.Descripcion,
                     FechaFin = C.ContratoEmpresas.Contrato.FechaVencimiento,
                     FechaInicio = C.ContratoEmpresas.Contrato.FechaInicio,
                     FechaProrroga = C.ContratoEmpresas.Contrato.Prorroga,
                     NombreEmpresa = C.ContratoEmpresas.Empresa.RazonSocial.Trim(),
                     EsContratista = C.ContratoEmpresas.EsContratista.Value,
                     Contratadopor = C.ContratoEmpresas.Contrato.Contratadopor.Descripcion,
                     Servicio = C.ContratoEmpresas.Contrato.Servicio,
                     Categoria = C.ContratoEmpresas.Contrato.objCategoria,
                     //Gestor = C.ContratoEmpresas.Contrato.GestorNombre,
                     //GestorEmail = C.ContratoEmpresas.Contrato.GestorEmail,
                     //Fiscal = C.ContratoEmpresas.Contrato.FiscalNombre,
                     //FiscalEmail = C.ContratoEmpresas.Contrato.FiscalEmail,
                     //Area = C.ContratoEmpresas.Contrato.objArea.Descripcion,
                     Hojas = C.HojasDeRuta
                     //C.AprobacionEpecial
                 }).ToList().Distinct();


        foreach (var item in a)
        {
            EstadosRutaTemp estadoRuta = new EstadosRutaTemp();
            estadoRuta.CodigoContrato = item.CodigoContrato;
            estadoRuta.Estado = item.Estado;
            estadoRuta.FechaFin = GetFormatoFecha(item.FechaFin);
            estadoRuta.FechaInicio = GetFormatoFecha(item.FechaInicio);
            estadoRuta.FechaProrroga = GetFormatoFecha(item.FechaProrroga);
            estadoRuta.NombreEmpresaContratista = GetNombreContratista(item.EsContratista, item.CodigoContrato, item.NombreEmpresa);
            estadoRuta.NombreEmpresaSubContratista = GetNombreSubContratista(item.EsContratista, item.CodigoContrato, item.NombreEmpresa);
            estadoRuta.Contratadopor = item.Contratadopor;
            estadoRuta.Servicio = item.Servicio;
            estadoRuta.Periodo = string.Format("{0:00}/{1:0000}", mes, año);
            //estadoRuta.Gestor = item.Gestor;
            //estadoRuta.GestorEmail = item.GestorEmail;
            //estadoRuta.Fiscal = item.Fiscal;
            //estadoRuta.FiscalEmail = item.FiscalEmail;
            //estadoRuta.Area = item.Area;
            //estadoRuta.AprobacionEspecial = item.AprobacionEpecial.HasValue ? item.AprobacionEpecial.Value : false;

            if (item.Estado != "Aprobada")
            {

                /// 1. Si no tiene item con documentacion recepcionada 
                /// 2. Si no tiene ningun item aprobado
                /// 3. Si no tiene ningun item con auditoria terminada
                /// ENTONCES se asume que no posee documentacion presentada.
                if (!item.Hojas.Any(w => w.DocFechaEntrega.HasValue)
                    &&
                    !item.Hojas.Any(w => w.HojaAprobado.HasValue && !w.HojaAprobado.HasValue)
                    &&
                    !item.Hojas.Any(w => w.AuditoriaTerminada.HasValue && w.AuditoriaTerminada.HasValue)
                    )
                {
                    estadoRuta.PresentoDocumentacion = "NO";
                }
                else
                {
                    estadoRuta.PresentoDocumentacion = "SI";
                }

                List<Entidades.HojasDeRuta> hojas = item.Hojas.ToList();
                List<dynamic> itemsSubContratista = (from i in a
                                                     where i.CodigoContrato == item.CodigoContrato
                                                     && i.EsContratista == false
                                                     select i).ToList<dynamic>();



                ///// Calculo del riesgo que poses la hoja de ruta
                IDictionary<string, string> RiesgoGrado = CalcularRiesgoHoja(hojas, item.EsContratista, itemsSubContratista);
                estadoRuta.Riesgo = RiesgoGrado.First().Key;
                estadoRuta.Grado = RiesgoGrado.First().Value;
            }
            else
            {
                if (!estadoRuta.AprobacionEspecial)
                {
                    estadoRuta.PresentoDocumentacion = "SI";
                    estadoRuta.Riesgo = "NULO";
                    estadoRuta.Grado = "0";
                }
                else
                {
                    estadoRuta.PresentoDocumentacion = "NO";
                    estadoRuta.Riesgo = "NULO";
                    estadoRuta.Grado = "0";
                    estadoRuta.Estado = "Aprobada (Sin Actividad)";
                }
            }


            if (item.Categoria != null)
                estadoRuta.Categoria = item.Categoria.Descripcion;

            DetallesEstadosTemp.Add(estadoRuta);
        }

        DetallesEstados = DetallesEstadosTemp;
        gvEstadoContratos.Rebind();


        #region CODIGO ORIGINAL 
        ///// Busca: las hojas de ruta donde el
        ///// periodo coincida con el periodod de consulta         

        //DetallesEstados = new List<EstadosRutaTemp>();
        //int mes = int.Parse(cboPeriodos.SelectedItem.Value.Split('/')[0]);
        //int año = int.Parse(cboPeriodos.SelectedItem.Value.Split('/')[1]);

        //var a = from C in Contexto.CabeceraHojasDeRuta
        //        where (C.Periodo.Month == mes && C.Periodo.Year == año)
        //        orderby C.ContratoEmpresas.EsContratista.Value descending
        //        select new
        //        {
        //            CodigoContrato = C.ContratoEmpresas.Contrato.Codigo,
        //            Estado = C.Estado.Descripcion,
        //            FechaFin = C.ContratoEmpresas.Contrato.FechaVencimiento,
        //            FechaInicio = C.ContratoEmpresas.Contrato.FechaInicio,
        //            FechaProrroga = C.ContratoEmpresas.Contrato.Prorroga,
        //            NombreEmpresa = C.ContratoEmpresas.Empresa.RazonSocial.Trim(),
        //            EsContratista = C.ContratoEmpresas.EsContratista,
        //            Contratadopor = C.ContratoEmpresas.Contrato.Contratadopor.Descripcion,
        //            Servicio = C.ContratoEmpresas.Contrato.Servicio,
        //            Categoria = C.ContratoEmpresas.Contrato.objCategoria,
        //            Hojas = C.HojasDeRuta
        //        };


        //foreach (var item in a)
        //{
        //    EstadosRutaTemp estadoRuta = new EstadosRutaTemp();
        //    estadoRuta.CodigoContrato = item.CodigoContrato;
        //    estadoRuta.Estado = item.Estado;
        //    estadoRuta.FechaFin = GetFormatoFecha(item.FechaFin);
        //    estadoRuta.FechaInicio = GetFormatoFecha(item.FechaInicio);
        //    estadoRuta.FechaProrroga = GetFormatoFecha(item.FechaProrroga);
        //    estadoRuta.NombreEmpresaContratista = GetNombreContratista(item.EsContratista, item.CodigoContrato, item.NombreEmpresa);
        //    estadoRuta.NombreEmpresaSubContratista = GetNombreSubContratista(item.EsContratista, item.CodigoContrato, item.NombreEmpresa);
        //    estadoRuta.Contratadopor = item.Contratadopor;
        //    estadoRuta.Servicio = item.Servicio;
        //    estadoRuta.Periodo = string.Format("{0:00}/{1:0000}", mes, año);


        //    if (item.Estado != "Aprobada")
        //    {

        //        /// Si no tiene item con documentacion recepcionada y
        //        /// no tiene ningun item aprobado, entonces se asume que no 
        //        /// posee documentacion presentada.
        //        if (!item.Hojas.Any(w => w.DocFechaEntrega.HasValue)
        //            &&
        //            !item.Hojas.Any(w => w.HojaAprobado.HasValue && !w.HojaAprobado.HasValue)
        //            )
        //        {
        //            estadoRuta.PresentoDocumentacion = "NO";
        //        }
        //        else
        //        {
        //            estadoRuta.PresentoDocumentacion = "SI";
        //        }
        //    }
        //    else
        //    {
        //        estadoRuta.PresentoDocumentacion = "SI";
        //    }


        //    if (item.Categoria != null)
        //        estadoRuta.Categoria = item.Categoria.Descripcion;

        //    DetallesEstados.Add(estadoRuta);
        //}

        //gvEstadoContratos.Rebind();

        #endregion
    }

    protected void gvEstadoContratos_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "ExportContratos")
        {
            gvEstadoContratos.DataSource = DetallesEstados.OrderBy(w => w.CodigoContrato).OrderBy(w => w.NombreEmpresaContratista).ToList();
            gvEstadoContratos.DataBind();
            ConfigureExportAndExport();
        }
    }

    protected void gvEstadoContratos_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
            {

                EstadosRutaTemp EstadoHoja = EntityDataSourceExtensions.GetItemObject<EstadosRutaTemp>(e.Item.DataItem);
                string colorEstado = "";
                switch (EstadoHoja.Riesgo)
                {
                    case "ALTO":
                        colorEstado = "Red";
                        break;
                    case "MEDIO":
                        colorEstado = "Orange";
                        break;
                    case "BAJO":
                        colorEstado = "Yellow";
                        break;
                    default:
                        colorEstado = "Green";
                        break;
                }

                e.Item.Cells[11].Style.Add(HtmlTextWriterStyle.BackgroundColor, colorEstado);

            }
        }
        catch (Exception err)
        {
            var aa = err.Message;
        }
    }

    protected void gvEstadoContratos_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        this.gvEstadoContratos.VirtualItemCount = DetallesEstados.Count;
        this.gvEstadoContratos.AllowCustomPaging = true;
        this.gvEstadoContratos.DataSource = DetallesEstados.OrderBy(w => w.CodigoContrato).OrderBy(w => w.NombreEmpresaContratista).Skip(gvEstadoContratos.CurrentPageIndex * gvEstadoContratos.PageSize).Take(gvEstadoContratos.PageSize).ToList();
    }

    private IDictionary<string, string> CalcularRiesgoHoja(List<Entidades.HojasDeRuta> hojas, bool EsContratista, List<dynamic> itemsSubContratistas)
    {
        Dictionary<string, string> riesgoGrado = new Dictionary<string, string>();

        /// 1. Si ningun item tiene docementacion recepcionada el riesgo es ALTO grado 5 directamente
        if (!hojas.Any(w => w.HojaFechaControlado != null || w.DocFechaEntrega.HasValue))
        {
            riesgoGrado.Add("ALTO", "5");
            //estadoRuta.Riesgo = "ALTO";
            //estadoRuta.Grado = "5";
        }
        /// 2. Controlo si hay almenos un item con documentacion recepcionada
        else if (hojas.Any(w => w.HojaFechaControlado != null || w.DocFechaEntrega.HasValue))
        {
            /// 2.1 Si todos lo items estan aprobados indica que el riesgo es NULO
            if (hojas.Where(w => w.HojaFechaAprobacion.HasValue).Count() == hojas.Count)
            {
                /// SI TIENE SUCONTRATISTAS TENGO QUE VERIFICAR EL ESTADO DE LA MISMA, SI NO ESTA APROBADA ENTONCES DEBO PONER
                /// EL ESTADO A LA CONTRATISTA DE LA SUBCONTRATISTA. SINO EL RIESGO ES NULO YA QUE TIENE TODOS LOS ITEMS APROBADOS.
                if (EsContratista && itemsSubContratistas.Count > 0)
                {
                    Dictionary<string, string> riesgoGradoSub = new Dictionary<string, string>();

                    foreach (var item in itemsSubContratistas)
                    {
                        List<Entidades.HojasDeRuta> hojasSub = new List<Entidades.HojasDeRuta>();
                        hojasSub.AddRange(item.Hojas);
                        IDictionary<string, string> result = CalcularRiesgoHoja(hojasSub, false, null);

                        if (!riesgoGradoSub.ContainsKey(result.First().Key))
                            riesgoGradoSub.Add(result.First().Key, result.First().Value);
                    }

                    if (riesgoGradoSub.ContainsKey("ALTO"))
                    {
                        riesgoGrado.Add(riesgoGradoSub.Where(w => w.Key == "ALTO").First().Key, riesgoGradoSub.Where(w => w.Key == "ALTO").First().Value);
                    }
                    else if (riesgoGradoSub.ContainsKey("MEDIO"))
                    {
                        riesgoGrado.Add(riesgoGradoSub.Where(w => w.Key == "MEDIO").First().Key, riesgoGradoSub.Where(w => w.Key == "MEDIO").First().Value);
                    }
                    else if (riesgoGradoSub.ContainsKey("BAJO"))
                    {
                        riesgoGrado.Add(riesgoGradoSub.Where(w => w.Key == "BAJO").First().Key, riesgoGradoSub.Where(w => w.Key == "BAJO").First().Value);
                    }
                    else if (riesgoGradoSub.ContainsKey("NULO"))
                    {
                        riesgoGrado.Add(riesgoGradoSub.Where(w => w.Key == "NULO").First().Key, riesgoGradoSub.Where(w => w.Key == "NULO").First().Value);
                    }
                }
                else
                {
                    riesgoGrado.Add("NULO", "0");
                }

            }
            /// 2.2 Si todos lo item tiene documentacion recepcionada y hay algunos aprobados y otros no 
            /// significa que el riesgo es BAJO
            else if (hojas.Where(w => w.DocFechaEntrega.HasValue || w.HojaFechaControlado.HasValue).Count() == hojas.Count)
            {
                riesgoGrado.Add("BAJO", "1");
            }
            /// 2.3 Si existen algunos items con documentacion recepcionada y otros no, tengo que hacer la evaluación
            /// según los items sin documentacion.
            else if (hojas.Where(w => w.DocFechaEntrega.HasValue || w.HojaFechaControlado.HasValue).Count() < hojas.Count)
            {

                if (hojas.Any(w => (w.HojaFechaControlado == null && !w.DocFechaEntrega.HasValue) && w.Plantilla.Riesgo == "ALTO"))
                {
                    riesgoGrado.Add("ALTO", hojas.Where(w => w.HojaFechaControlado == null && !w.DocFechaEntrega.HasValue && w.Plantilla.Riesgo == "ALTO").Max(w => w.Plantilla.Grado).Value.ToString());
                }
                else if (hojas.Any(w => (w.HojaFechaControlado == null && !w.DocFechaEntrega.HasValue) && w.Plantilla.Riesgo == "MEDIO"))
                {
                    riesgoGrado.Add("MEDIO", hojas.Where(w => w.HojaFechaControlado == null && !w.DocFechaEntrega.HasValue && w.Plantilla.Riesgo == "MEDIO").Max(w => w.Plantilla.Grado).Value.ToString());
                }
                else if (hojas.Any(w => (w.HojaFechaControlado == null && !w.DocFechaEntrega.HasValue) && w.Plantilla.Riesgo == "BAJO"))
                {
                    riesgoGrado.Add("BAJO", hojas.Where(w => w.HojaFechaControlado == null && !w.DocFechaEntrega.HasValue && w.Plantilla.Riesgo == "BAJO").Max(w => w.Plantilla.Grado).Value.ToString());
                }
            }

        }

        return riesgoGrado;
    }


    public void ConfigureExportAndExport()
    {
        gvEstadoContratos.CurrentPageIndex = 0;
        foreach (Telerik.Web.UI.GridColumn column in gvEstadoContratos.MasterTableView.Columns)
        {
            if (!column.Visible || !column.Display)
            {
                column.Visible = true;
                column.Display = true;
            }
        }

        gvEstadoContratos.ExportSettings.ExportOnlyData = true;
        gvEstadoContratos.ExportSettings.IgnorePaging = true;
        gvEstadoContratos.ExportSettings.FileName = "EstadoContratos";
        gvEstadoContratos.MasterTableView.ExportToExcel();



    }
}
