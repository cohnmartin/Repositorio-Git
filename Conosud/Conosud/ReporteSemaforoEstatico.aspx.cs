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

public partial class ReporteSemaforoEstatico : System.Web.UI.Page
{
    private static DSConosud.SemaforoEstaticoDataTable DTSem;
    private static Hashtable _Contadores;
    private static Hashtable _ContadoresxLegajos;
    private static Hashtable _RowsHtml;
    string arrayTable="";

    protected void Page_Load(object sender, EventArgs e)
    {
        txtPeriodo.Attributes.Add("onkeydown", "NoSubmit();");
        txtPeriodoFinal.Attributes.Add("onkeydown", "NoSubmit();");
    }
    private string ConstruirRowHtml(string Empresa, string Contrato, string Periodo)
    {
        string arrayTable = "";
        arrayTable += "        <tr>";
        arrayTable += "            <td style='WIDTH: 100px; BORDER-BOTTOM: #843431 thin solid'>";
        arrayTable += "                " + Empresa + "</td>";
        arrayTable += "            <td style='WIDTH: 100px; BORDER-BOTTOM: #843431 thin solid'>";
        arrayTable += "                " + Contrato + "</td>";
        arrayTable += "            <td style='WIDTH: 100px; BORDER-BOTTOM: #843431 thin solid'>";
        arrayTable += "                " + Periodo + "</td>";
        arrayTable += "        </tr>";
        return arrayTable;
    }
    private string GenerarArrayTabla()
    {
        int pos = 1;
        DateTime FechaFinalTemp = Convert.ToDateTime("01/" + txtPeriodoFinal.Text.Substring(5, 2) + "/" + txtPeriodoFinal.Text.Substring(0, 4));
        int UltimoDia = FechaFinalTemp.AddMonths(1).AddDays(-1).Day;
        DateTime FechaFinal = Convert.ToDateTime(UltimoDia.ToString() + "/" + txtPeriodoFinal.Text.Substring(5, 2) + "/" + txtPeriodoFinal.Text.Substring(0, 4));

        DateTime FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
        while (FechaFinal >= FechaInicialTemp)
        {
            if (gvDatosPorContratos.Rows[0].Cells[pos].Text != "0" && gvDatosPorContratos.Rows[0].Cells[pos].Text != " ")
            {
                //row Total Contratos
                arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid; COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
                arrayTable += GetRowsHtml("En Termino", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += GetRowsHtml("Fuera Termino", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += GetRowsHtml("Con Presenaciones", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += GetRowsHtml("Sin Presentaciones", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += "    </table>\",";
            }

            pos++;
            FechaInicialTemp = FechaInicialTemp.AddMonths(1);
        }
        
        pos = 1;
        FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
        while (FechaFinal >= FechaInicialTemp)
        {

            if (gvDatosPorContratos.Rows[2].Cells[pos].Text != "0" && gvDatosPorContratos.Rows[2].Cells[pos].Text != " ")
            {
                //row Total Aprobadas
                arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid;  COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
                arrayTable += GetRowsHtml("En Termino", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += GetRowsHtml("Fuera Termino", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += "    </table>\",";
            }

            pos++;
            FechaInicialTemp = FechaInicialTemp.AddMonths(1);
        }

        pos = 1;
        FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
        while (FechaFinal >= FechaInicialTemp)
        {

            //row .En Termino
            if (gvDatosPorContratos.Rows[3].Cells[pos].Text != "0" && gvDatosPorContratos.Rows[3].Cells[pos].Text != " ")
            {
                arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid;  COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
                arrayTable += GetRowsHtml("En Termino", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += "    </table>\",";
                
            }
            pos++;
            FechaInicialTemp = FechaInicialTemp.AddMonths(1);
        }

        pos = 1;
        FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
        while (FechaFinal >= FechaInicialTemp)
        {

            //row .Fura Termino
            if (gvDatosPorContratos.Rows[4].Cells[pos].Text != "0" && gvDatosPorContratos.Rows[4].Cells[pos].Text != " ")
            {
                arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid;  COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
                arrayTable += GetRowsHtml("Fuera Termino", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += "    </table>\",";
                
            }
            pos++;
            FechaInicialTemp = FechaInicialTemp.AddMonths(1);
        }

        pos = 1;
        FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
        while (FechaFinal >= FechaInicialTemp)
        {

            //row Total No Aprobada
            if (gvDatosPorContratos.Rows[6].Cells[pos].Text != "0" && gvDatosPorContratos.Rows[6].Cells[pos].Text != " ")
            {
                arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid;  COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
                arrayTable += GetRowsHtml("Con Presenaciones", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += GetRowsHtml("Sin Presentaciones", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += "    </table>\",";
            }
            pos++;
            FechaInicialTemp = FechaInicialTemp.AddMonths(1);
        }

        pos = 1;
        FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
        while (FechaFinal >= FechaInicialTemp)
        {
            //row .Con Presenaciones
            if (gvDatosPorContratos.Rows[7].Cells[pos].Text != "0" && gvDatosPorContratos.Rows[7].Cells[pos].Text != " ")
            {
                arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid;  COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
                arrayTable += GetRowsHtml("Con Presenaciones", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += "    </table>\",";
            }
            pos++;
            FechaInicialTemp = FechaInicialTemp.AddMonths(1);
        }

        pos = 1;
        FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
        while (FechaFinal >= FechaInicialTemp)
        {

            //row .Sin Presentaciones
            if (gvDatosPorContratos.Rows[8].Cells[pos].Text != "0" && gvDatosPorContratos.Rows[8].Cells[pos].Text != " ")
            {
                arrayTable += "\"<table style='BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; FONT-SIZE: 1em; BORDER-LEFT: #843431 thin solid;  COLOR: #b5494a; BORDER-BOTTOM: #843431 thin solid; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #f1dcdc'>";
                arrayTable += GetRowsHtml("Sin Presentaciones", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                arrayTable += "    </table>\",";
            }
            pos++;
            FechaInicialTemp = FechaInicialTemp.AddMonths(1);
        }

    
        arrayTable = arrayTable.Substring(0, arrayTable.Length - 1);
        return arrayTable;

    }
    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        CalcularDatos();
        string cad = "var myArray = new Array(" + GenerarArrayTabla() + ")";
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "aa", cad, true);
    }

    private void CalcularDatos()
    {
        try
        {

            #region Calculo de Fechas

            DateTime FechaFinalTemp = Convert.ToDateTime("01/" + txtPeriodoFinal.Text.Substring(5, 2) + "/" + txtPeriodoFinal.Text.Substring(0, 4));
            int UltimoDia = FechaFinalTemp.AddMonths(1).AddDays(-1).Day;

            DateTime FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
            int UltimoDiabis = FechaInicialTemp.AddMonths(1).AddDays(-1).Day;


            DateTime FechaInicial = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
            DateTime FechaFinal = Convert.ToDateTime(UltimoDia.ToString() + "/" + txtPeriodoFinal.Text.Substring(5, 2) + "/" + txtPeriodoFinal.Text.Substring(0, 4));
            DateTime FechaFinalPrimerPeriodo = Convert.ToDateTime(UltimoDiabis.ToString() + "/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));


            #endregion

            #region Calculo los valores de cada celda

            //Logica:
            //    No Aprobadas
            //        Sin Presentaciones
            //            Si el PeriodoIn es <= al Current Periodo se incluye dentro de la categoria de este periodo
            //        Con Presentaciones
            //            Si el periodoIn es = al Current Periodo se incluye dentro de la categoria de este periodo
            //    Aprobadas
            //        En Termino
            //            Si el periodoIn es = al Current Periodo se incluye dentro de la categoria de este periodo
            //        Fuera Termino
            //            Si el periodoIn es = al Current Periodo se incluye dentro de la categoria de este periodo


            _Contadores = new Hashtable();
            _ContadoresxLegajos = new Hashtable();
            _RowsHtml = new Hashtable();

            DSConosudTableAdapters.SemaforoEstaticoTableAdapter DASemEstatico = new DSConosudTableAdapters.SemaforoEstaticoTableAdapter();
            DTSem = DASemEstatico.GetData(FechaFinal, FechaInicial, FechaFinalPrimerPeriodo);

            FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
            //while (FechaFinal >= FechaInicialTemp)
            //{
                foreach (DSConosud.SemaforoEstaticoRow row in DTSem.Rows)
                {
                    //string identificador = row.EstadoInterno + "-" + string.Format("{0:yyyy/MM}", FechaInicialTemp);
                    string identificador = row.EstadoInterno + "-" + string.Format("{0:yyyy/MM}", row.Periodo);

                    if (!_Contadores.ContainsKey(identificador))
                    {
                        if (row.EstadoInterno == "Sin Presentaciones")
                        {
                            //if (Convert.ToDateTime("01/" + row.PeriodoIn) <= FechaInicialTemp)
                            //{
                                _RowsHtml.Add(identificador, ConstruirRowHtml(row.RazonSocial, row.Codigo, string.Format("{0:yyyy/MM}", row.Periodo)));
                                _Contadores.Add(identificador, 1);
                                _ContadoresxLegajos.Add(identificador, row.Legajos);
                            //}

                        }
                        else if (row.EstadoInterno == "Con Presentaciones")
                        {
                            //if (Convert.ToDateTime("01/" + row.PeriodoIn) == FechaInicialTemp)
                            //{
                                _RowsHtml.Add(identificador, ConstruirRowHtml(row.RazonSocial, row.Codigo, string.Format("{0:yyyy/MM}", row.Periodo)));
                                _Contadores.Add(identificador, 1);
                                _ContadoresxLegajos.Add(identificador, row.Legajos);
                            //}
                        }
                        else if (row.EstadoInterno == "En Termino")
                        {
                            //if (Convert.ToDateTime("01/" + row.PeriodoIn) == FechaInicialTemp)
                            //{
                                _RowsHtml.Add(identificador, ConstruirRowHtml(row.RazonSocial, row.Codigo, string.Format("{0:yyyy/MM}", row.Periodo)));
                                _Contadores.Add(identificador, 1);
                                _ContadoresxLegajos.Add(identificador, row.Legajos);
                            //}
                        }
                        else if (row.EstadoInterno == "Fuera Termino")
                        {
                            //if (Convert.ToDateTime("01/" + row.PeriodoIn) == FechaInicialTemp.AddMonths(1))
                            //{
                                _RowsHtml.Add(identificador, ConstruirRowHtml(row.RazonSocial, row.Codigo, string.Format("{0:yyyy/MM}", row.Periodo)));
                                _Contadores.Add(identificador, 1);
                                _ContadoresxLegajos.Add(identificador, row.Legajos);
                            //}
                        }

                    }
                    else
                    {
                        if (row.EstadoInterno == "Sin Presentaciones")
                        {
                            //if (Convert.ToDateTime("01/" + row.PeriodoIn) <= FechaInicialTemp)
                            //{
                                _RowsHtml[identificador] += ConstruirRowHtml(row.RazonSocial, row.Codigo, string.Format("{0:yyyy/MM}", row.Periodo));
                                _Contadores[identificador] = Convert.ToInt32(_Contadores[identificador]) + 1;
                                _ContadoresxLegajos[identificador] = Convert.ToInt32(_ContadoresxLegajos[identificador]) + row.Legajos;
                            //}
                        }
                        else if (row.EstadoInterno == "Con Presentaciones")
                        {
                            //if (Convert.ToDateTime("01/" + row.PeriodoIn) == FechaInicialTemp)
                            //{
                                _RowsHtml[identificador] += ConstruirRowHtml(row.RazonSocial, row.Codigo, string.Format("{0:yyyy/MM}", row.Periodo));
                                _Contadores[identificador] = Convert.ToInt32(_Contadores[identificador]) + 1;
                                _ContadoresxLegajos[identificador] = Convert.ToInt32(_ContadoresxLegajos[identificador]) + row.Legajos;
                            //}
                        }
                        else if (row.EstadoInterno == "En Termino")
                        {
                            //if (Convert.ToDateTime("01/" + row.PeriodoIn) == FechaInicialTemp)
                            //{
                                _RowsHtml[identificador] += ConstruirRowHtml(row.RazonSocial, row.Codigo, string.Format("{0:yyyy/MM}", row.Periodo));
                                _Contadores[identificador] = Convert.ToInt32(_Contadores[identificador]) + 1;
                                _ContadoresxLegajos[identificador] = Convert.ToInt32(_ContadoresxLegajos[identificador]) + row.Legajos;
                            //}
                        }
                        else if (row.EstadoInterno == "Fuera Termino")
                        {
                            //if (Convert.ToDateTime("01/" + row.PeriodoIn) == FechaInicialTemp.AddMonths(1))
                            //{
                                _RowsHtml[identificador] += ConstruirRowHtml(row.RazonSocial, row.Codigo, string.Format("{0:yyyy/MM}", row.Periodo));
                                _Contadores[identificador] = Convert.ToInt32(_Contadores[identificador]) + 1;
                                _ContadoresxLegajos[identificador] = Convert.ToInt32(_ContadoresxLegajos[identificador]) + row.Legajos;
                            //}
                        }
                    }
                }

            //    FechaInicialTemp = FechaInicialTemp.AddMonths(1);
            //}

            #endregion

            #region Generacion e Inicializo la Tabla
            DataTable tblSemEstatico = new DataTable();
            DataTable tblSemEstaticoLeg = new DataTable();


            DataColumn col;
            int CantidadColumnas = 0;
            col = new DataColumn("Descripción", typeof(string));
            tblSemEstatico.Columns.Add(col);

            col = new DataColumn("Descripción", typeof(string));
            tblSemEstaticoLeg.Columns.Add(col);


            FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
            while (FechaFinal >= FechaInicialTemp)
            {
                col = new DataColumn(string.Format("{0:yyyy/MM}", FechaInicialTemp), typeof(string));
                tblSemEstatico.Columns.Add(col);

                col = new DataColumn(string.Format("{0:yyyy/MM}", FechaInicialTemp), typeof(string));
                tblSemEstaticoLeg.Columns.Add(col);

                FechaInicialTemp = FechaInicialTemp.AddMonths(1);
                CantidadColumnas++;
            }

            tblSemEstatico.Rows.Add(InitRow("Total Contratos", CantidadColumnas));
            tblSemEstatico.Rows.Add(InitRow(" ", CantidadColumnas));
            tblSemEstatico.Rows.Add(InitRow("      Total Aprobadas", CantidadColumnas));
            tblSemEstatico.Rows.Add(InitRow("          .En Termino", CantidadColumnas));
            tblSemEstatico.Rows.Add(InitRow("          .Fura Termino", CantidadColumnas));
            tblSemEstatico.Rows.Add(InitRow(" ", CantidadColumnas));
            tblSemEstatico.Rows.Add(InitRow("      Total No Aprobada", CantidadColumnas));
            tblSemEstatico.Rows.Add(InitRow("            .Con Presenaciones", CantidadColumnas));
            tblSemEstatico.Rows.Add(InitRow("            .Sin Presentaciones", CantidadColumnas));

            tblSemEstaticoLeg.Rows.Add(InitRow("Total Contratos", CantidadColumnas));
            tblSemEstaticoLeg.Rows.Add(InitRow(" ", CantidadColumnas));
            tblSemEstaticoLeg.Rows.Add(InitRow("      Total Aprobadas", CantidadColumnas));
            tblSemEstaticoLeg.Rows.Add(InitRow("          .En Termino", CantidadColumnas));
            tblSemEstaticoLeg.Rows.Add(InitRow("          .Fura Termino", CantidadColumnas));
            tblSemEstaticoLeg.Rows.Add(InitRow(" ", CantidadColumnas));
            tblSemEstaticoLeg.Rows.Add(InitRow("      Total No Aprobada", CantidadColumnas));
            tblSemEstaticoLeg.Rows.Add(InitRow("            .Con Presenaciones", CantidadColumnas));
            tblSemEstaticoLeg.Rows.Add(InitRow("            .Sin Presentaciones", CantidadColumnas));

            #endregion

            #region Cargo los valores ESTADISTICA POR CONTRATO y LEGAJOS

            // Calculo los valores por contratos
            FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
            while (FechaFinal >= FechaInicialTemp)
            {
                tblSemEstatico.Rows[3][string.Format("{0:yyyy/MM}", FechaInicialTemp)] = GetValueCalc("En Termino", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                tblSemEstatico.Rows[4][string.Format("{0:yyyy/MM}", FechaInicialTemp)] = GetValueCalc("Fuera Termino", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                tblSemEstatico.Rows[7][string.Format("{0:yyyy/MM}", FechaInicialTemp)] = GetValueCalc("Con Presenaciones", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                tblSemEstatico.Rows[8][string.Format("{0:yyyy/MM}", FechaInicialTemp)] = GetValueCalc("Sin Presentaciones", string.Format("{0:yyyy/MM}", FechaInicialTemp));

                tblSemEstaticoLeg.Rows[3][string.Format("{0:yyyy/MM}", FechaInicialTemp)] = GetValueCalcLegajos("En Termino", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                tblSemEstaticoLeg.Rows[4][string.Format("{0:yyyy/MM}", FechaInicialTemp)] = GetValueCalcLegajos("Fuera Termino", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                tblSemEstaticoLeg.Rows[7][string.Format("{0:yyyy/MM}", FechaInicialTemp)] = GetValueCalcLegajos("Con Presenaciones", string.Format("{0:yyyy/MM}", FechaInicialTemp));
                tblSemEstaticoLeg.Rows[8][string.Format("{0:yyyy/MM}", FechaInicialTemp)] = GetValueCalcLegajos("Sin Presentaciones", string.Format("{0:yyyy/MM}", FechaInicialTemp));


                FechaInicialTemp = FechaInicialTemp.AddMonths(1);
            }

            // Calculo los valores por legajos
            FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
            while (FechaFinal >= FechaInicialTemp)
            {
                tblSemEstatico.Rows[2][string.Format("{0:yyyy/MM}", FechaInicialTemp)] =
                    Convert.ToString(Convert.ToInt32(tblSemEstatico.Rows[3][string.Format("{0:yyyy/MM}", FechaInicialTemp)]) + Convert.ToInt32(tblSemEstatico.Rows[4][string.Format("{0:yyyy/MM}", FechaInicialTemp)]));

                tblSemEstatico.Rows[6][string.Format("{0:yyyy/MM}", FechaInicialTemp)] =
                    Convert.ToString(Convert.ToInt32(tblSemEstatico.Rows[7][string.Format("{0:yyyy/MM}", FechaInicialTemp)]) + Convert.ToInt32(tblSemEstatico.Rows[8][string.Format("{0:yyyy/MM}", FechaInicialTemp)]));


                tblSemEstatico.Rows[0][string.Format("{0:yyyy/MM}", FechaInicialTemp)] =
                   Convert.ToString(Convert.ToInt32(tblSemEstatico.Rows[2][string.Format("{0:yyyy/MM}", FechaInicialTemp)]) + Convert.ToInt32(tblSemEstatico.Rows[6][string.Format("{0:yyyy/MM}", FechaInicialTemp)]));


                tblSemEstaticoLeg.Rows[2][string.Format("{0:yyyy/MM}", FechaInicialTemp)] =
                    Convert.ToString(Convert.ToInt32(tblSemEstaticoLeg.Rows[3][string.Format("{0:yyyy/MM}", FechaInicialTemp)]) + Convert.ToInt32(tblSemEstaticoLeg.Rows[4][string.Format("{0:yyyy/MM}", FechaInicialTemp)]));

                tblSemEstaticoLeg.Rows[6][string.Format("{0:yyyy/MM}", FechaInicialTemp)] =
                    Convert.ToString(Convert.ToInt32(tblSemEstaticoLeg.Rows[7][string.Format("{0:yyyy/MM}", FechaInicialTemp)]) + Convert.ToInt32(tblSemEstaticoLeg.Rows[8][string.Format("{0:yyyy/MM}", FechaInicialTemp)]));


                tblSemEstaticoLeg.Rows[0][string.Format("{0:yyyy/MM}", FechaInicialTemp)] =
                   Convert.ToString(Convert.ToInt32(tblSemEstaticoLeg.Rows[2][string.Format("{0:yyyy/MM}", FechaInicialTemp)]) + Convert.ToInt32(tblSemEstaticoLeg.Rows[6][string.Format("{0:yyyy/MM}", FechaInicialTemp)]));



                FechaInicialTemp = FechaInicialTemp.AddMonths(1);
            }

            gvDatosPorContratos.DataSource = tblSemEstatico;
            gvDatosPorContratos.DataBind();

            gvDatosPorLegajos.DataSource = tblSemEstaticoLeg;
            gvDatosPorLegajos.DataBind();

            FormatoCeldas();
           

            #endregion

        }
        catch
        { }
    }
    private void FormatoCeldas()
    {
        int posInArray = 0;
        // Formateo las celdas
        if (gvDatosPorContratos.Rows.Count>0)
        {
            DateTime FechaFinalTemp = Convert.ToDateTime("01/" + txtPeriodoFinal.Text.Substring(5, 2) + "/" + txtPeriodoFinal.Text.Substring(0, 4));
            int UltimoDia = FechaFinalTemp.AddMonths(1).AddDays(-1).Day;

            DateTime FechaFinal = Convert.ToDateTime(UltimoDia.ToString() + "/" + txtPeriodoFinal.Text.Substring(5, 2) + "/" + txtPeriodoFinal.Text.Substring(0, 4));


            for (int i = 0; i < 9; i++)
            {
                int pos = 1;
                DateTime FechaInicialTemp = Convert.ToDateTime("01/" + txtPeriodo.Text.Substring(5, 2) + "/" + txtPeriodo.Text.Substring(0, 4));
                while (FechaFinal >= FechaInicialTemp)
                {
                    gvDatosPorContratos.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    gvDatosPorContratos.Rows[i].Cells[0].Wrap = false;

                    gvDatosPorLegajos.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    gvDatosPorLegajos.Rows[i].Cells[0].Wrap = false;

                    if (gvDatosPorContratos.Rows[i].Cells[pos].Text != "0" && gvDatosPorContratos.Rows[i].Cells[pos].Text != " ")
                    {
                        LinkButton hyp = new LinkButton();
                        hyp.EnableViewState = true;
                        hyp.ID = "LinkDetalle" + i.ToString() + FechaInicialTemp.Year.ToString() + FechaInicialTemp.Month.ToString();
                        hyp.Text = gvDatosPorContratos.Rows[i].Cells[pos].Text;
                        hyp.ForeColor = System.Drawing.Color.Maroon;
                        hyp.OnClientClick = "CargarDiv(" + posInArray.ToString() + ");return false;";
                        gvDatosPorContratos.Rows[i].Cells[pos].Controls.Add(hyp);
                        posInArray++;
                    }

                    if (i == 0)
                    {
                        gvDatosPorContratos.Rows[i].Cells[pos].Font.Bold = true;
                        gvDatosPorContratos.Rows[i].Cells[pos].Font.Size = new FontUnit(18);

                        gvDatosPorLegajos.Rows[i].Cells[pos].Font.Bold = true;
                        gvDatosPorLegajos.Rows[i].Cells[pos].Font.Size = new FontUnit(18);
                    }
                    else if (i == 2)
                    {
                        gvDatosPorContratos.Rows[i].Cells[pos].Font.Bold = true;
                        gvDatosPorContratos.Rows[i].Cells[pos].Font.Size = new FontUnit(14);

                        gvDatosPorLegajos.Rows[i].Cells[pos].Font.Bold = true;
                        gvDatosPorLegajos.Rows[i].Cells[pos].Font.Size = new FontUnit(14);

                    }
                    else if (i == 6)
                    {

                        gvDatosPorContratos.Rows[i].Cells[pos].Font.Bold = true;
                        gvDatosPorContratos.Rows[i].Cells[pos].Font.Size = new FontUnit(14);

                        gvDatosPorLegajos.Rows[i].Cells[pos].Font.Bold = true;
                        gvDatosPorLegajos.Rows[i].Cells[pos].Font.Size = new FontUnit(14);

                    }


                    FechaInicialTemp = FechaInicialTemp.AddMonths(1);
                    pos++;


                }
            }
        }
    }

    private string GetValueCalc(string EstadoInterno, string periodo)
    {
        if (_Contadores.ContainsKey(EstadoInterno + "-" + periodo))
        {
            return _Contadores[EstadoInterno + "-" + periodo].ToString() ;
        }
        else 
        {
            return "0";
        }
    
    }
    private string GetRowsHtml(string EstadoInterno, string periodo)
    {
        if (_RowsHtml.ContainsKey(EstadoInterno + "-" + periodo))
        {
            return _RowsHtml[EstadoInterno + "-" + periodo].ToString();
        }
        else
        {
            return "";
        }

    }
    private string GetValueCalcLegajos(string EstadoInterno, string periodo)
    {
        if (_ContadoresxLegajos.ContainsKey(EstadoInterno + "-" + periodo))
        {
            return _ContadoresxLegajos[EstadoInterno + "-" + periodo].ToString();
        }
        else
        {
            return "0";
        }

    }
    private object[] InitRow(string Nombre, int CantCol)
    {
        object[] cols = new object[CantCol+1];
        cols[0] = Nombre;

        for (int i = 1; i < CantCol+1; i++)
        {
            cols[i] = " ";
        }
        return cols;
    }

   
}
