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
    /// Summary description for InformeAltaBajaLegajo.
    /// </summary>
    public partial class InformeAltaBajaLegajo : Telerik.Reporting.Report
    {
        public List<AltasBajasLegajosTemp> DataSouece
        {
            get
            {
                if (HttpContext.Current.Session["DataSouece"] != null)

                    return (List<AltasBajasLegajosTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<AltasBajasLegajosTemp>));
                else
                {
                    return (List<AltasBajasLegajosTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<AltasBajasLegajosTemp>));
                }
            }
            set
            {
                HttpContext.Current.Session["DataSouece"] = Helper.SerializeObject(value);
            }
        }

        public InformeAltaBajaLegajo()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void InitReport(List<AltasBajasLegajosTemp> AltasBajas, DateTime Periodo)
        {
            string periodo = String.Format("{0:yyyy/MM}", Periodo);
            this.ReportParameters[0].Value = periodo;

            //this.DataSource = AltasBajas;
            DataSouece = AltasBajas;
        }

        private void InformeAltaBajaLegajo_NeedDataSource(object sender, EventArgs e)
        {
            (sender as Telerik.Reporting.Processing.Report).DataSource = DataSouece;
        }
    }
}