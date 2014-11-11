namespace Entidades
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Linq;
    using System.Collections.Generic;
    using System.Web.SessionState;
    using System.Web;

    /// <summary>
    /// Summary description for InformexPeriodoHistorico.
    /// </summary>
    public partial class InformexPeriodoHistorico : Telerik.Reporting.Report
    {
        public string Leyenda
        {

            get
            {
                if (HttpContext.Current.Session["Leyenda"] != null)
                    return HttpContext.Current.Session["Leyenda"].ToString();
                else
                {
                    return HttpContext.Current.Session["Leyenda"].ToString();
                }
            }
            set
            {
                HttpContext.Current.Session["Leyenda"] = value;
            }
        }

        public List<InformexPeriodoTemp> DataSouece
        {
            get
            {
                if (HttpContext.Current.Session["DataSouece"] != null)

                    return (List<InformexPeriodoTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<InformexPeriodoTemp>));
                else
                {
                    return (List<InformexPeriodoTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<InformexPeriodoTemp>));
                }
            }
            set
            {
                HttpContext.Current.Session["DataSouece"] = Helper.SerializeObject(value);
            }
        }


        public InformexPeriodoHistorico()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }


        public void InitReport(List<InformexPeriodoTemp> resultado, CabeceraHojasDeRuta Cabecera, bool EsHistorico)
        {
            EntidadesConosud dc = new EntidadesConosud();

            string periodo = Cabecera.Periodo.Year.ToString() + "/" + string.Format("{0:00}", Cabecera.Periodo.Month);
            long idContrato = Cabecera.ContratoEmpresas.Contrato.IdContrato;
            string strSubContraristas = "";
            string leyendaSubContratistas = "";


            var subcontratistas = from C in dc.ContratoEmpresas
                                  where C.Contrato.IdContrato == idContrato
                                  && C.EsContratista == false
                                  select C.Empresa;

            foreach (Empresa emp in subcontratistas)
            {
                strSubContraristas += emp.RazonSocial + ", ";
            }

            if (strSubContraristas != "")
            {
                leyendaSubContratistas = ", y subcontratistas: " + strSubContraristas;
            }

            if (EsHistorico)
            {
                Leyenda = "Luego de realizado el control de la documentación de la empresa " + Cabecera.ContratoEmpresas.ConstratistaParaSubConstratista +
                    " por el contrato Nº " + Cabecera.ContratoEmpresas.Contrato.Codigo + " con VALE S.A., " +
                    "por el período " + periodo + " y meses anteriores" + leyendaSubContratistas + ", se informa a continuación el resultado:";
            }
            else
            {

                Leyenda = "Luego de realizado el control de la documentación de la empresa " + Cabecera.ContratoEmpresas.ConstratistaParaSubConstratista +
                       " por el contrato Nº " + Cabecera.ContratoEmpresas.Contrato.Codigo + " con VALE S.A., " +
                       "por el período " + periodo + " " + leyendaSubContratistas + ", se informa a continuación el resultado:";
            }

            DataSouece = resultado;
            //          this.DataSource = Helper.SerializeObject(resultado);
            textBox2.Value = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Format("{0:D}", DateTime.Now));
        }

        private void detail_ItemDataBound(object sender, EventArgs e)
        {
            // Get the detail section object from sender
            Telerik.Reporting.Processing.DetailSection section = (Telerik.Reporting.Processing.DetailSection)sender;
            // From the section object get the DataRowView
            InformexPeriodoTemp dataRowView = (InformexPeriodoTemp)section.DataObject.RawData;
            if (dataRowView.Comentario == "")
            {
                section.Visible = false;
            }
        }

        private void InformexPeriodoHistorico_NeedDataSource(object sender, EventArgs e)
        {
            (sender as Telerik.Reporting.Processing.Report).DataSource = DataSouece;
            txtLeyendaEncabezado.Value = Leyenda;
        }
     
    }
}