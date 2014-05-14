using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ReporteHojadeRuta : System.Web.UI.Page
{
    private static Hashtable _Cabeceras;

    protected void Page_Load(object sender, EventArgs e)
    {


    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        if (txtPeriodo.Text != "" || txtPeriodoFinal.Text != "")
        {

            if (dgConsulta.Columns.Count > 3)
            {
                for (int i = dgConsulta.Columns.Count - 1; i > 3; i--)
                {
                    dgConsulta.Columns.RemoveAt(i);
                }
            }

            DateTime fechainicio = new DateTime(Convert.ToInt32(txtPeriodo.Text.Substring(0, 4)), Convert.ToInt32(txtPeriodo.Text.Substring(5, 2)), 1);
            fechainicio = new DateTime(fechainicio.Year, fechainicio.Month, 1);
            DateTime fechafin = new DateTime(Convert.ToInt32(txtPeriodoFinal.Text.Substring(0, 4)), Convert.ToInt32(txtPeriodoFinal.Text.Substring(5, 2)), 1);

            DSConosudTableAdapters.ConsultaEstadosHRTableAdapter DTCons = new DSConosudTableAdapters.ConsultaEstadosHRTableAdapter();
            DSConosudTableAdapters.PlantillaTableAdapter DTPlan = new DSConosudTableAdapters.PlantillaTableAdapter();
            DSConosud.ConsultaEstadosHRDataTable tbDatosConsulta = new DSConosud.ConsultaEstadosHRDataTable();
            DSConosud.ConsultaEstadosHRDataTable tbConsulta= new DSConosud.ConsultaEstadosHRDataTable();
            try
            {
                if (DDLContratos.SelectedItem.Text == "")
                {
                    tbDatosConsulta = DTCons.GetDataEstadoHR(fechainicio, fechafin, "", "1");
                    tbConsulta = DTCons.GetDataConsultaCabecera("", "1");
                }
                else
                {
                    tbDatosConsulta = DTCons.GetDataEstadoHR(fechainicio, fechafin, DDLContratos.SelectedItem.Text, "0");
                    tbConsulta = DTCons.GetDataConsultaCabecera(DDLContratos.SelectedItem.Text, "0");
                }
            }
            catch
            {
                dgConsulta.DataSource = null;
                string alert = "alert('Por algun motivo este contrato no tiene generadas las hojas de ruta correctamente, por favor actualice el contrato para que se realice la generación de las hojas de ruta.')";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel2, this.GetType(), "click", alert, true);
                return;
            }

            int? iTotItems = DTPlan.ScalarQueryCantidadItems();
            DataColumn col;
            HyperLinkField colLink;

            while (fechafin >= fechainicio)
            {
                //col = new DataColumn(fechafin.Month.ToString() + " / " + fechafin.Year.ToString().Substring(2, 2), typeof(String));

                col = new DataColumn(string.Format("{0:yyyy/MM}", fechafin), typeof(string));
                tbConsulta.Columns.Add(col);


                colLink = new HyperLinkField();
                colLink.HeaderText = string.Format("{0:yyyy/MM}", fechafin);
                colLink.DataTextField = string.Format("{0:yyyy/MM}", fechafin);
                colLink.DataNavigateUrlFields = new string[] { "IdCabeceraHojadeRuta" };
                colLink.DataNavigateUrlFormatString = "CargarHojaRuta.aspx?Id={0}";
                dgConsulta.Columns.Add(colLink);

                fechafin = fechafin.AddMonths(-1);
            }

            tbConsulta.Columns.Remove("Periodo");
            tbConsulta.Columns.Remove("NoAprobados");
            tbConsulta.Columns.Remove("Aprobados");
            //tbConsulta.Columns.Remove("IdCabeceraHojadeRuta");


            _Cabeceras = new Hashtable();
            foreach (DSConosud.ConsultaEstadosHRRow row in tbDatosConsulta)
            {
                //tbConsulta[0].IdCabeceraHojaDeRuta = row.IdCabeceraHojaDeRuta;

                int pos = GetRow(tbConsulta, row.Contratistas);

                if (_Cabeceras.ContainsKey(pos))
                {
                    ((Hashtable)_Cabeceras[pos]).Add(string.Format("{0:yyyy/MM}", row.Periodo), row.IdCabeceraHojaDeRuta);
                }
                else
                {
                    Hashtable listaIds = new Hashtable();
                    listaIds.Add(string.Format("{0:yyyy/MM}", row.Periodo), row.IdCabeceraHojaDeRuta);

                    _Cabeceras.Add(pos, listaIds);
                }

                if (row.NoAprobados == iTotItems)
                {
                    //tbConsulta[0][row.Periodo.Month.ToString() + " / " + row.Periodo.Year.ToString().Substring(2, 2)] = "T";
                    tbConsulta[pos][string.Format("{0:yyyy/MM}", row.Periodo)] = "T";
                }
                else if (row.NoAprobados < iTotItems)
                {
                    tbConsulta[pos][string.Format("{0:yyyy/MM}", row.Periodo)] = "P";
                }
                else
                {
                    tbConsulta[pos][string.Format("{0:yyyy/MM}", row.Periodo)] = string.Empty;
                }
            }

            if (tbDatosConsulta.Count > 0)
            {
                dgConsulta.DataSource = tbConsulta;
                Helpers.GenExcell c = new Helpers.GenExcell();
                //c.DoExcell(Server.MapPath(Request.ApplicationPath) + @"\ReporteExcel.xls", tbConsulta);
            }
            else
            {
                dgConsulta.DataSource = null;
                string alert = "alert('Por algun motivo este contrato no tiene generadas las hojas de ruta correctamente, por favor actualice el contrato para que se realice la generación de las hojas de ruta.')";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel2, this.GetType(), "click", alert, true);
                return;

            }

            dgConsulta.DataBind();

        }
        else
        {
            string alert = "alert('Debe Selecionar el Período de Consulta.')";
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel2, this.GetType(), "click", alert, true);
            return;
        }

    }

    protected void ibExcel_Click(object sender, ImageClickEventArgs e)
    {

        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", "window.open('ReporteExcel.xls')", true);
    }
    protected void DDLContratos_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (DDLContratos.SelectedItem.Text != "" && Helpers._Contratos.ContainsKey(DDLContratos.SelectedItem.Text))
        {
            txtPeriodo.Text = string.Format("{0:yyyy/MM}", ((DSConosud.ContratoRow)Helpers._Contratos[DDLContratos.SelectedItem.Text]).FechaInicio);
            if (((DSConosud.ContratoRow)Helpers._Contratos[DDLContratos.SelectedItem.Text]).IsProrrogaNull())
                txtPeriodoFinal.Text = string.Format("{0:yyyy/MM}", ((DSConosud.ContratoRow)Helpers._Contratos[DDLContratos.SelectedItem.Text]).FechaVencimiento);
            else
                txtPeriodoFinal.Text = string.Format("{0:yyyy/MM}", ((DSConosud.ContratoRow)Helpers._Contratos[DDLContratos.SelectedItem.Text]).Prorroga);
        }
    }
    protected void txtInicial_TextChanged(object sender, EventArgs e)
    {

    }
    protected void cboEmpresas_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPeriodo.Text = "";
        txtPeriodoFinal.Text = "";
    }
    protected void dgConsulta_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void dgConsulta_DataBinding(object sender, EventArgs e)
    {

    }
    protected void dgConsulta_DataBound(object sender, EventArgs e)
    {

    }
    protected void dgConsulta_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 4; i < e.Row.Cells.Count; i++)
                {
                    HyperLink hl = e.Row.Cells[i].Controls[0] as HyperLink;
                    string Periodo = ((System.Web.UI.WebControls.DataControlFieldCell)(e.Row.Cells[i])).ContainingField.HeaderText;
                    hl.NavigateUrl = "CargarHojaRuta.aspx?Id=" + ((Hashtable)_Cabeceras[e.Row.RowIndex])[Periodo].ToString();

                }
            }
        }
        catch
        {
            string alert = "alert('Por algún motivo este contrato no tiene todas las hojas de rutas generadas, por favor contacte al administrador.')";
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel2, this.GetType(), "click", alert, true);
        }

    }
    private int GetRow(DSConosud.ConsultaEstadosHRDataTable tbConsulta, string Contratista)
    {
        int cont = 0;
        foreach (DSConosud.ConsultaEstadosHRRow row in tbConsulta)
        {
            if (row.Contratistas == Contratista)
                break;
            else
                cont++;
        }
        return cont;

    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {

    }
}
