using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ReporteGestionAuditoria : System.Web.UI.Page
{
    static Hashtable _ContadoresMesActual;
    static Hashtable _ContadoresMesAnteriores;
    static Hashtable _ContadoresOtrasMeses;

    protected void Page_Load(object sender, EventArgs e)
    {
        txtPeriodo.Attributes.Add("onkeydown", "NoSubmit();");
    }
    
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {

        #region Generacion de Fchas

        DateTime FechaInicialConsulta = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));

        int ultimodia = FechaInicialConsulta.AddMonths(1).AddDays(-1).Day;
        DateTime FechaFinalConsulta = Convert.ToDateTime(ultimodia.ToString() + "/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));

        DateTime FechaInicialPeriodo = FechaInicialConsulta.AddMonths(-1);
        DateTime FechaFinalPeriodo = Convert.ToDateTime(FechaInicialConsulta.AddDays(-1).Day.ToString() + "/" + FechaInicialPeriodo.Month.ToString() + "/" + FechaInicialPeriodo.Year.ToString());

        #endregion

        GenerarDatosMesActual(FechaInicialConsulta, FechaFinalConsulta, FechaInicialPeriodo, FechaFinalPeriodo);

        GenerarDatosDocActualMesesAnteriores(FechaInicialConsulta, FechaFinalConsulta, FechaInicialPeriodo);

        GenerarDatosAuditoriaDesplazada(FechaInicialConsulta, FechaFinalConsulta, FechaInicialPeriodo);

    }
      
    private void GenerarDatosMesActual(DateTime FechaInicialConsulta, DateTime FechaFinalConsulta, DateTime FechaInicialPeriodo, DateTime FechaFinalPeriodo)
    {
        DSConosudTableAdapters.GestionAuditoriaTableAdapter DAGestionAuditoria = new DSConosudTableAdapters.GestionAuditoriaTableAdapter();
        DSConosud.GestionAuditoriaDataTable DTGestionAuditoria = DAGestionAuditoria.GetDataMesCuestion(FechaInicialConsulta, FechaFinalConsulta, FechaInicialPeriodo, FechaFinalPeriodo);
        DSConosud.GestionAuditoriaDataTable DTGestionAuditoriaFinal = new DSConosud.GestionAuditoriaDataTable();

        _ContadoresMesActual = new Hashtable();

        foreach (DSConosud.GestionAuditoriaRow row in DTGestionAuditoria.Rows)
        {
            string identificador = row.Auditor + row.Nombre.ToString();

            if (!_ContadoresMesActual.ContainsKey(identificador))
            {
                DTGestionAuditoriaFinal.Rows.Add(new object[] { row.IdCabeceraHojaDeRuta, row.CantidadHojas, row.Auditor, "0", row.CantidadLegajos, row.ItemsControlados, row.TotalItems, row.Nombre });

                IList datosAnexos = new ArrayList();
                datosAnexos.Add(DTGestionAuditoriaFinal.Rows[DTGestionAuditoriaFinal.Rows.Count - 1]);
                datosAnexos.Add(row.IdCabeceraHojaDeRuta.ToString());

                _ContadoresMesActual.Add(identificador, datosAnexos);
            }
            else
            {
                DSConosud.GestionAuditoriaRow rowCalculo = (DSConosud.GestionAuditoriaRow)((IList)_ContadoresMesActual[identificador])[0];
                string IdsCalculo = (string)((IList)_ContadoresMesActual[identificador])[1];

                IdsCalculo += "-" + row.IdCabeceraHojaDeRuta.ToString();



                rowCalculo.TotalItems += row.TotalItems;
                rowCalculo.ItemsControlados += row.ItemsControlados;
                rowCalculo.CantidadLegajos += row.CantidadLegajos;
                rowCalculo.CantidadHojas += row.CantidadHojas;

                ((IList)_ContadoresMesActual[identificador])[0] = rowCalculo;
                ((IList)_ContadoresMesActual[identificador])[1] = IdsCalculo;

            }
        }

        gvAuditoriaActual.DataSource = DTGestionAuditoriaFinal;
        gvAuditoriaActual.DataBind();



    }

    private void GenerarDatosDocActualMesesAnteriores(DateTime FechaInicialConsulta, DateTime FechaFinalConsulta, DateTime FechaPeriodo)
    {
        DSConosudTableAdapters.GestionAuditoriaTableAdapter DAGestionAuditoria = new DSConosudTableAdapters.GestionAuditoriaTableAdapter();
        DSConosud.GestionAuditoriaDataTable DTGestionAuditoria = DAGestionAuditoria.GetDataMesesAnteriores(FechaInicialConsulta, FechaFinalConsulta, FechaPeriodo);

        DSConosud.GestionAuditoriaDataTable DTGestionAuditoriaFinal = new DSConosud.GestionAuditoriaDataTable();

        _ContadoresMesAnteriores  = new Hashtable();

        foreach (DSConosud.GestionAuditoriaRow row in DTGestionAuditoria.Rows)
        {
            string identificador = row.Auditor + row.Nombre.ToString();

            if (!_ContadoresMesAnteriores.ContainsKey(identificador))
            {
                DTGestionAuditoriaFinal.Rows.Add(new object[] { row.IdCabeceraHojaDeRuta, row.CantidadHojas, row.Auditor, "0", row.CantidadLegajos, row.ItemsControlados, row.TotalItems, row.Nombre });

                IList datosAnexos = new ArrayList();
                datosAnexos.Add(DTGestionAuditoriaFinal.Rows[DTGestionAuditoriaFinal.Rows.Count - 1]);
                datosAnexos.Add(row.IdCabeceraHojaDeRuta.ToString());

                _ContadoresMesAnteriores.Add(identificador, datosAnexos);
            }
            else
            {
                DSConosud.GestionAuditoriaRow rowCalculo = (DSConosud.GestionAuditoriaRow)((IList)_ContadoresMesAnteriores[identificador])[0];
                string IdsCalculo = (string)((IList)_ContadoresMesAnteriores[identificador])[1];

                IdsCalculo += "-" + row.IdCabeceraHojaDeRuta.ToString();



                rowCalculo.TotalItems += row.TotalItems;
                rowCalculo.ItemsControlados += row.ItemsControlados;
                rowCalculo.CantidadLegajos += row.CantidadLegajos;
                rowCalculo.CantidadHojas += row.CantidadHojas;

                ((IList)_ContadoresMesAnteriores[identificador])[0] = rowCalculo;
                ((IList)_ContadoresMesAnteriores[identificador])[1] = IdsCalculo;

            }
        }

        gvAuditoriaMesesAnterior.DataSource = DTGestionAuditoriaFinal;
        gvAuditoriaMesesAnterior.DataBind();
    }

    private void GenerarDatosAuditoriaDesplazada(DateTime FechaInicialConsulta, DateTime FechaFinalConsulta, DateTime FechaPeriodo)
    {
        DSConosudTableAdapters.GestionAuditoriaTableAdapter DAGestionAuditoria = new DSConosudTableAdapters.GestionAuditoriaTableAdapter();
        DSConosud.GestionAuditoriaDataTable DTGestionAuditoria = DAGestionAuditoria.GetAuditoriaDesplazada(FechaInicialConsulta, FechaFinalConsulta, FechaPeriodo);

        DSConosud.GestionAuditoriaDataTable DTGestionAuditoriaFinal = new DSConosud.GestionAuditoriaDataTable();

        _ContadoresOtrasMeses = new Hashtable();

        foreach (DSConosud.GestionAuditoriaRow row in DTGestionAuditoria.Rows)
        {
            string identificador = row.Auditor + row.Nombre.ToString();

            if (!_ContadoresOtrasMeses.ContainsKey(identificador))
            {
                DTGestionAuditoriaFinal.Rows.Add(new object[] { row.IdCabeceraHojaDeRuta, row.CantidadHojas, row.Auditor, "0", row.CantidadLegajos, row.ItemsControlados, row.TotalItems, row.Nombre });

                IList datosAnexos = new ArrayList();
                datosAnexos.Add(DTGestionAuditoriaFinal.Rows[DTGestionAuditoriaFinal.Rows.Count - 1]);
                datosAnexos.Add(row.IdCabeceraHojaDeRuta.ToString());

                _ContadoresOtrasMeses.Add(identificador, datosAnexos);
            }
            else
            {
                DSConosud.GestionAuditoriaRow rowCalculo = (DSConosud.GestionAuditoriaRow)((IList)_ContadoresOtrasMeses[identificador])[0];
                string IdsCalculo = (string)((IList)_ContadoresOtrasMeses[identificador])[1];

                IdsCalculo += "-" + row.IdCabeceraHojaDeRuta.ToString();



                rowCalculo.TotalItems += row.TotalItems;
                rowCalculo.ItemsControlados += row.ItemsControlados;
                rowCalculo.CantidadLegajos += row.CantidadLegajos;
                rowCalculo.CantidadHojas += row.CantidadHojas;

                ((IList)_ContadoresOtrasMeses[identificador])[0] = rowCalculo;
                ((IList)_ContadoresOtrasMeses[identificador])[1] = IdsCalculo;

            }
        }

        gvAuditoriaDesplazada.DataSource = DTGestionAuditoriaFinal;
        gvAuditoriaDesplazada.DataBind();
    }

    protected void gvAuditoriaActual_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#CCCCCC';this.style.cursor='hand';");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F1DCDC'");
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference((Control)sender, "Select$" + e.Row.RowIndex.ToString()));
        }
    }
    protected void gvAuditoriaActual_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (gvAuditoriaActual.SelectedRow != null)
        {
            foreach (GridViewRow row in gvAuditoriaActual.Rows)
            {
                foreach (Control ctr in row.Cells[7].Controls)
                {
                    if (ctr.ID == "btnComponentes")
                    {
                        ((Button)ctr).Enabled = false;
                        row.Attributes.Clear();
                        InicializarRow(sender, row);
                    }
                }

            }

            foreach (Control ctr in gvAuditoriaActual.SelectedRow.Cells[7].Controls)
            {
                if (ctr.ID == "btnComponentes")
                {
                    ((Button)ctr).Enabled = true;
                    AEDetalle.TargetControlID = ctr.UniqueID;


                    AEDetalle.OnClick.Children.RemoveAt(0);

                    AjaxControlToolkit.Animation a = new AjaxControlToolkit.Animation();
                    a.Name = "ScriptAction";
                    a.Properties.Add("Script", "Cover($get('" + ctr.UniqueID + "'), $get('flyout'));");
                    AEDetalle.OnClick.Children.Insert(0, a);

                    gvAuditoriaActual.SelectedRow.Attributes.Clear();
                    break;
                }
            }

            string identificador = gvAuditoriaActual.SelectedRow.Cells[6].Text + gvAuditoriaActual.SelectedRow.Cells[2].Text;
            string IdsCalculo = (string)((IList)_ContadoresMesActual[identificador])[1];

            DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter DACabecera = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
            DSConosud.CabeceraHojasDeRutaDataTable DTCabecera = DACabecera.GetDataByCabeceras(IdsCalculo);


            gvDetalle.DataSource = DTCabecera;
            gvDetalle.DataBind();



        }
    }

    protected void gvAuditoriaMesesAnterior_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#CCCCCC';this.style.cursor='hand';");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F1DCDC'");
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference((Control)sender, "Select$" + e.Row.RowIndex.ToString()));
        }
    }
    protected void gvAuditoriaMesesAnterior_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (gvAuditoriaMesesAnterior.SelectedRow != null)
        {
            foreach (GridViewRow row in gvAuditoriaMesesAnterior.Rows)
            {
                foreach (Control ctr in row.Cells[7].Controls)
                {
                    if (ctr.ID == "btnDetalleMA")
                    {
                        ((Button)ctr).Enabled = false;
                        row.Attributes.Clear();
                        InicializarRow(sender, row);

                    }
                }

            }

            foreach (Control ctr in gvAuditoriaMesesAnterior.SelectedRow.Cells[7].Controls)
            {
                if (ctr.ID == "btnDetalleMA")
                {
                    AjaxControlToolkit.Animation a = new AjaxControlToolkit.Animation();
                    a.Name = "ScriptAction";
                    a.Properties.Add("Script", "Cover($get('" + ctr.UniqueID + "'), $get('flyout'));");

                    AEDetalleMA.TargetControlID = ctr.UniqueID;
                    AEDetalleMA.OnClick.Children.RemoveAt(0);
                    AEDetalleMA.OnClick.Children.Insert(0, a);

                    gvAuditoriaMesesAnterior.SelectedRow.Attributes.Clear();
                    ((Button)ctr).Enabled = true;
                    break;
                }
            }

            string identificador = gvAuditoriaMesesAnterior.SelectedRow.Cells[6].Text + gvAuditoriaMesesAnterior.SelectedRow.Cells[2].Text;
            string IdsCalculo = (string)((IList)_ContadoresMesAnteriores[identificador])[1];

            DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter DACabecera = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
            DSConosud.CabeceraHojasDeRutaDataTable DTCabecera = DACabecera.GetDataByCabeceras(IdsCalculo);


            gvDetalleMA.DataSource = DTCabecera;
            gvDetalleMA.DataBind();



        }
    }

    protected void gvAuditoriaDesplazada_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#CCCCCC';this.style.cursor='hand';");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F1DCDC'");
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference((Control)sender, "Select$" + e.Row.RowIndex.ToString()));
        }
    }
    protected void gvAuditoriaDesplazada_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (gvAuditoriaDesplazada.SelectedRow != null)
        {
            foreach (GridViewRow row in gvAuditoriaDesplazada.Rows)
            {
                foreach (Control ctr in row.Cells[7].Controls)
                {
                    if (ctr.ID == "btnDetalleOM")
                    {
                        ((Button)ctr).Enabled = false;
                        row.Attributes.Clear();
                        InicializarRow(sender, row);
                    }
                }

            }

            foreach (Control ctr in gvAuditoriaDesplazada.SelectedRow.Cells[7].Controls)
            {
                if (ctr.ID == "btnDetalleOM")
                {
                    AjaxControlToolkit.Animation a = new AjaxControlToolkit.Animation();
                    a.Name = "ScriptAction";
                    a.Properties.Add("Script", "Cover($get('" + ctr.UniqueID + "'), $get('flyout'));");
                   
                    AEDetalleOM.TargetControlID = ctr.UniqueID;
                    AEDetalleOM.OnClick.Children.RemoveAt(0);
                    AEDetalleOM.OnClick.Children.Insert(0, a);

                    gvAuditoriaDesplazada.SelectedRow.Attributes.Clear();

                    ((Button)ctr).Enabled = true;
                    break;
                }
            }

            string identificador = gvAuditoriaDesplazada.SelectedRow.Cells[6].Text + gvAuditoriaDesplazada.SelectedRow.Cells[2].Text;
            string IdsCalculo = (string)((IList)_ContadoresOtrasMeses[identificador])[1];

            DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter DACabecera = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
            DSConosud.CabeceraHojasDeRutaDataTable DTCabecera = DACabecera.GetDataByCabeceras(IdsCalculo);


            gvDetalleOM.DataSource = DTCabecera;
            gvDetalleOM.DataBind();



        }
        
    }

    private void InicializarRow(object sender, GridViewRow row)
    {
        row.Attributes.Add("onmouseover", "this.style.backgroundColor='#CCCCCC';this.style.cursor='hand';");
        row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F1DCDC'");
        row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference((Control)sender, "Select$" + row.RowIndex.ToString()));
    }
}