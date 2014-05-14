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

public partial class ReporteSemaforoDinamico : System.Web.UI.Page
{
    private static DSConosud.SemaforoDinamicoDataTable DTSem;
    private static Hashtable _RowsHtml;
    private static Hashtable _RowsHtmlC;

    protected void Page_Load(object sender, EventArgs e)
    {
        txtPeriodo.Attributes.Add("onkeydown", "NoSubmit();");
    }
    
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DSConosudTableAdapters.SemaforoDinamicoTableAdapter DASemEstatico = new DSConosudTableAdapters.SemaforoDinamicoTableAdapter();
            Hashtable _Contadores = new Hashtable();
            Hashtable _ContadorLegajos = new Hashtable();
            Hashtable _Empresas = new Hashtable();
            _RowsHtml = new Hashtable();
            _RowsHtmlC = new Hashtable();

            #region Calculo de Fechas
                DateTime FechaInicialPeriodo = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
                int UltimoDia = FechaInicialPeriodo.AddMonths(1).AddDays(-1).Day;
                DateTime FechaFinalPeriodo = Convert.ToDateTime(UltimoDia.ToString() + "/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));

                DateTime FechaInicial = FechaFinalPeriodo.AddDays(1);
                DateTime FechaFinal = FechaFinalPeriodo.AddMonths(1);

            #endregion


            DTSem = DASemEstatico.GetData(FechaFinal.Date, FechaInicialPeriodo.Date,FechaFinalPeriodo.Date,FechaInicial.Date);

            foreach (DSConosud.SemaforoDinamicoRow row in DTSem.Rows)
            {
                string identificado = row.RazonSocial + "-" + row.Codigo;

                if (!_Empresas.ContainsKey(identificado))
                {
                    IList Estados = new ArrayList();
                    Estados.Add(row.EstadoGeneral);
                   
                    _Empresas.Add(identificado, Estados);
                }
                else
                {
                    if (!((IList)_Empresas[identificado]).Contains(row.EstadoGeneral))
                    {
                        ((IList)_Empresas[identificado]).Add(row.EstadoGeneral);
                    }
                }



                string descripcion = row.RazonSocial + "-" + row.Codigo + "-" + string.Format("{0:yyyy/MM}",row.Periodo);
                if (!_Contadores.ContainsKey(row.EstadoInterno))
                {
                    _RowsHtmlC.Add(row.EstadoInterno, ConstruirRowHtmlC(descripcion));
                    _Contadores.Add(row.EstadoInterno, 1);
                    _ContadorLegajos.Add(row.EstadoInterno, row.Legajos);
                }
                else
                {
                    _RowsHtmlC[row.EstadoInterno]  += ConstruirRowHtmlC(descripcion);
                    _Contadores[row.EstadoInterno] = Convert.ToInt32(_Contadores[row.EstadoInterno]) + 1;
                    _ContadorLegajos[row.EstadoInterno] = Convert.ToInt32(_ContadorLegajos[row.EstadoInterno]) + row.Legajos;
                }
            }

            #region Estadistica por Empresa

            int Aprobadas = 0;
            int NoAprobadas = 0;
            int Mixtas = 0;

            _RowsHtml.Add("Mixtas", "");
            _RowsHtml.Add("Aprobadas", "");
            _RowsHtml.Add("No Aprobadas", "");
            foreach (string empresa in _Empresas.Keys)
            {
                if (((IList)_Empresas[empresa]).Count > 1)
                {
                    _RowsHtml["Mixtas"] += ConstruirRowHtmlE(empresa);
                    Mixtas++;
                }
                else
                {
                    if (((IList)_Empresas[empresa])[0] == "Aprobadas")
                    {
                        _RowsHtml["Aprobadas"] += ConstruirRowHtmlE(empresa);
                        Aprobadas++;
                    }
                    else
                    {
                        _RowsHtml["No Aprobadas"] += ConstruirRowHtmlE(empresa);
                        NoAprobadas++;
                    }
                }
            }

            lblTotalAprobadasE.Text = Aprobadas.ToString();
            lblTotalNoAprobadasE.Text = NoAprobadas.ToString();
            lblTotalMixtas.Text = Mixtas.ToString();
            lblTotalEmpresas.Text = _Empresas.Count.ToString();


            string cad = "var myArrayE = new Array(" + GenerarArrayTablaE() + ")";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "aa", cad, true);

            #endregion

            #region Estadistica por Contrato
            if (_Contadores.ContainsKey("En Termino"))
                lblEnTermino.Text = _Contadores["En Termino"].ToString();
            else
                lblEnTermino.Text = "0";

            if (_Contadores.ContainsKey("Fuera Termino"))
                lblFueraTermino.Text = _Contadores["Fuera Termino"].ToString();
            else
                lblFueraTermino.Text = "0";

            if (_Contadores.ContainsKey("Sin Presentaciones"))
                lblSinPresentaciones.Text = _Contadores["Sin Presentaciones"].ToString();
            else
                lblSinPresentaciones.Text = "0";

            if (_Contadores.ContainsKey("Con Presentaciones"))
                lblConPresentaciones.Text = _Contadores["Con Presentaciones"].ToString();
            else
                lblConPresentaciones.Text = "0";

            lblAprobadas.Text = Convert.ToString(Convert.ToInt32(lblEnTermino.Text) + Convert.ToInt32(lblFueraTermino.Text));
            lblNoAprobadas.Text = Convert.ToString(Convert.ToInt32(lblSinPresentaciones.Text) + Convert.ToInt32(lblConPresentaciones.Text));

            lblTotalContratos.Text = Convert.ToString(Convert.ToInt32(lblAprobadas.Text) + Convert.ToInt32(lblNoAprobadas.Text));
            lblTotalHojas.Text = Convert.ToString(Convert.ToInt32(lblAprobadas.Text) + Convert.ToInt32(lblNoAprobadas.Text));

            string cadC = "var myArrayC = new Array(" + GenerarArrayTablaC() + ")";
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "bb", cadC, true);

            #endregion
            
            #region Estadistica por Legajos
            if (_ContadorLegajos.ContainsKey("En Termino"))
                lblEnTerminoL.Text = _ContadorLegajos["En Termino"].ToString();
            else
                lblEnTerminoL.Text = "0";

            if (_ContadorLegajos.ContainsKey("Fuera Termino"))
                lblFueraTerminoL.Text = _ContadorLegajos["Fuera Termino"].ToString();
            else
                lblFueraTerminoL.Text = "0";


            if (_ContadorLegajos.ContainsKey("Sin Presentaciones"))
                lblSinPresentacionesL.Text = _ContadorLegajos["Sin Presentaciones"].ToString();
            else
                lblSinPresentacionesL.Text = "0";


            if (_ContadorLegajos.ContainsKey("Con Presentaciones"))
                lblConPresentacionesL.Text = _ContadorLegajos["Con Presentaciones"].ToString();
            else
                lblConPresentacionesL.Text = "0";


            lblAprobadasL.Text = Convert.ToString(Convert.ToInt32(lblEnTerminoL.Text) + Convert.ToInt32(lblFueraTerminoL.Text));
            lblNoAprobadasL.Text = Convert.ToString(Convert.ToInt32(lblSinPresentacionesL.Text) + Convert.ToInt32(lblConPresentacionesL.Text));

            lblTotalContratosL.Text = Convert.ToString(Convert.ToInt32(lblAprobadasL.Text) + Convert.ToInt32(lblNoAprobadasL.Text));
            lblTotalHojasL.Text = Convert.ToString(Convert.ToInt32(lblAprobadasL.Text) + Convert.ToInt32(lblNoAprobadasL.Text));
            #endregion


        }
        catch
        { }

    }
    
    protected void lblTotalEmpresas_Click(object sender, EventArgs e)
    {
        //gvDatos.DataSource = DTSem;
        //gvDatos.DataBind();

        //divImg.Visible = true;
        //DivEmpresasAprobadas.Visible = true;
        //DivEmpresasMixtas.Visible = true;
        //DivEmpresasNoAprobadas.Visible = true;

        //imgTotalEmpresas.OnClientClick = "return false;";
        //AnimationExtender1.TargetControlID = imgTotalEmpresas.UniqueID;
        
    }

    private string ConstruirRowHtmlE(string Descripcion)
    {
        string[] valore = Descripcion.Split('-');

        string arrayTable = "";
        arrayTable += "        <tr>";
        arrayTable += "            <td style='WIDTH: 100px; BORDER-BOTTOM: #843431 thin solid'>";
        arrayTable += "                " + valore[0] + "</td>";
        arrayTable += "            <td style='WIDTH: 100px; BORDER-BOTTOM: #843431 thin solid'>";
        arrayTable += "                " + valore[1] + "</td>";
        arrayTable += "        </tr>";
        return arrayTable;
    }

    private string ConstruirRowHtmlC(string Descripcion)
    {
        string[] valore = Descripcion.Split('-');

        string arrayTable = "";
        arrayTable += "        <tr>";
        arrayTable += "            <td style='WIDTH: 100px; BORDER-BOTTOM: #843431 thin solid'>";
        arrayTable += "                " + valore[0] + "</td>";
        arrayTable += "            <td style='WIDTH: 100px; BORDER-BOTTOM: #843431 thin solid'>";
        arrayTable += "                " + valore[1] + "</td>";
        arrayTable += "            <td style='WIDTH: 100px; BORDER-BOTTOM: #843431 thin solid'>";
        arrayTable += "                " + valore[2] + "</td>";
        arrayTable += "        </tr>";
        return arrayTable;
    }

    private string GetRowsHtmlE(string Estado)
    {
        if (_RowsHtml.ContainsKey(Estado))
        {
            return _RowsHtml[Estado].ToString();
        }
        else
        {
            return "";
        }

    }
    private string GetRowsHtmlC(string Estado)
    {
        if (_RowsHtmlC.ContainsKey(Estado))
        {
            return _RowsHtmlC[Estado].ToString();
        }
        else
        {
            return "";
        }

    }

    private string GenerarArrayTablaE()
    {
        string arrayTable = "";
        //row Total Empresas
        arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
        arrayTable += GetRowsHtmlE("Aprobadas");
        arrayTable += GetRowsHtmlE("No Aprobadas");
        arrayTable += GetRowsHtmlE("Mixtas");
        arrayTable += "    </table>\",";

        //row Total Aprobadas
        arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
        arrayTable += GetRowsHtmlE("Aprobadas");
        arrayTable += "    </table>\",";

        //row Total No Aprobadas
        arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
        arrayTable += GetRowsHtmlE("No Aprobadas");
        arrayTable += "    </table>\",";

        //row Total Mixtas
        arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
        arrayTable += GetRowsHtmlE("Mixtas");
        arrayTable += "    </table>\",";


        arrayTable = arrayTable.Substring(0, arrayTable.Length - 1);
        return arrayTable;

    }

    private string GenerarArrayTablaC()
    {
        string arrayTable = "";
        //row Total Hojas
        arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
        arrayTable += GetRowsHtmlC("En Termino");
        arrayTable += GetRowsHtmlC("Fuera Termino");
        arrayTable += GetRowsHtmlC("Sin Presentaciones");
        arrayTable += GetRowsHtmlC("Con Presentaciones");
        arrayTable += "    </table>\",";

        //row Total Hojas Aprobadas
        arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
        arrayTable += GetRowsHtmlC("En Termino");
        arrayTable += GetRowsHtmlC("Fuera Termino");
        arrayTable += "    </table>\",";

        //row Total Hojas No Aprobadas
        arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
        arrayTable += GetRowsHtmlC("Sin Presentaciones");
        arrayTable += GetRowsHtmlC("Con Presentaciones");
        arrayTable += "    </table>\",";


        //row Total en Termino
        arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
        arrayTable += GetRowsHtmlC("En Termino");
        arrayTable += "    </table>\",";


        //row Total Fuera Termino
        arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
        arrayTable += GetRowsHtmlC("Fuera Termino");
        arrayTable += "    </table>\",";


        //row Total Con Presentaciones
        arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
        arrayTable += GetRowsHtmlC("Con Presentaciones");
        arrayTable += "    </table>\",";


        //row Total Sin Presentacionesjas
        arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
        arrayTable += GetRowsHtmlC("Sin Presentaciones");
        arrayTable += "    </table>\",";




        arrayTable = arrayTable.Substring(0, arrayTable.Length - 1);
        return arrayTable;

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string script = " CargarDiv(1);";
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "onclick1", script, true);
    }
}
