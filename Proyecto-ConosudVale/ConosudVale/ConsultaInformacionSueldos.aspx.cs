using System;
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
using System.Reflection;

public partial class ConsultaInformacionSueldos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime fechaInicial = DateTime.Now;
            //cboPeriodos.Items.Add(new RadComboBoxItem("", ""));

            for (int i = 0; i < 13; i++)
            {
                DateTime fechaActual = fechaInicial.AddMonths(-1 * i);
                string FechaFormat = string.Format("{0:MM/yyyy}", fechaActual);
                cboPeriodos.Items.Add(new RadComboBoxItem(FechaFormat, FechaFormat));
            }

            if (Request.QueryString["PeriodoEspecifico"] != null)
            {
                TR_Periodos.Visible = false;
                CargarDatos(long.Parse(Request.QueryString["IdLegajo"].ToString()), Request.QueryString["PeriodoEspecifico"]);
            }
            else
                TR_Periodos.Visible = true;
        }
    }
    protected void cboPeriodos_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {

        CargarDatos(long.Parse(Request.QueryString["IdLegajo"].ToString()), cboPeriodos.Text);
    }

    private void CargarDatos(long IdLegajo, string Periodo)
    {

        EntidadesConosud dc = new EntidadesConosud();

        DateTime FechaInicial = Convert.ToDateTime("01/" + Periodo);
        DateTime FechaFinal = Convert.ToDateTime("01/" + Periodo).AddMonths(1).AddDays(-1);


        Entidades.DatosDeSueldos datosSueldo = (from D in dc.DatosDeSueldos
                                                where D.Periodo >= FechaInicial && D.Periodo <= FechaFinal
                                                && D.objLegajo.IdLegajos == IdLegajo
                                                select D).FirstOrDefault();
        if (datosSueldo != null)
        {

            foreach (PropertyInfo item in datosSueldo.GetType().GetProperties())
            {
                string nombreControl = "lbl" + item.Name;
                Label ctr = (Label)this.FindControl(nombreControl);
                if (ctr != null)
                {
                    decimal? valor = (decimal?)item.GetValue(datosSueldo, null);
                    if (valor != null)
                        ((Label)ctr).Text = string.Format("{0:###,##0.##}", valor);
                    else
                        ((Label)ctr).Text = "0";
                }
            }
            Tbl_SinDatos.Visible = false;
            Tbl_Datos.Visible = true;
        }
        else
        {
            Tbl_SinDatos.Visible = true;
            Tbl_Datos.Visible = false;
        }
    }
}
