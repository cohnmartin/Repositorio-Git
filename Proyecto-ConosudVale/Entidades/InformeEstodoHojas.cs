namespace Entidades
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Linq;
    using System.Web.SessionState;
    using System.Web;

    /// <summary>
    /// Summary description for InformeEstodoHojas.
    /// </summary>
    public partial class InformeEstodoHojas : Telerik.Reporting.Report
    {
        public List<EstadosRutaTemp> DataSouece
        {
            get
            {
                if (HttpContext.Current.Session["DataSouece"] != null)

                    return (List<EstadosRutaTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<EstadosRutaTemp>));
                else
                {
                    return (List<EstadosRutaTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<EstadosRutaTemp>));
                }
            }
            set
            {
                HttpContext.Current.Session["DataSouece"] = Helper.SerializeObject(value);
            }
        }

        public InformeEstodoHojas()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void InitReport(List<EstadosRutaTemp> EstadosHojas, DateTime Periodo)
        {
            string periodo = String.Format("{0:yyyy/MM}", Periodo);
            this.ReportParameters[0].Value = periodo;

            //this.DataSource = EstadosHojas;
            DataSouece= EstadosHojas;
        }

        private void InformeEstodoHojas_NeedDataSource(object sender, EventArgs e)
        {
            (sender as Telerik.Reporting.Processing.Report).DataSource = DataSouece;
        }

    }
}