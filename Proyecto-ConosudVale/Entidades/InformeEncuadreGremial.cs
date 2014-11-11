namespace Entidades
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Reflection;
    using System.Data.Objects;
    using System.Linq.Expressions;
    using System.Data;
    using System.Web.SessionState;
    using System.Web;

    /// <summary>
    /// Summary description for InformeMapaHojaDeRuta.
    /// </summary>
    public partial class InformeEncuadreGremial : Telerik.Reporting.Report
    {

        public List<InformeEncuadreGremialTemp> DataSouece
        {
            get
            {
                if (HttpContext.Current.Session["DataSouece"] != null)

                    return (List<InformeEncuadreGremialTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<InformeEncuadreGremialTemp>));
                else
                {
                    return (List<InformeEncuadreGremialTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<InformeEncuadreGremialTemp>));
                }
            }
            set
            {
                HttpContext.Current.Session["DataSouece"] = Helper.SerializeObject(value);
            }
        }


        public InformeEncuadreGremial()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();
            
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void InitReport(List<InformeEncuadreGremialTemp> datos,string Periodo)
        {
            this.ReportParameters["Periodo"].Value = Periodo;
            //this.crosstab2.DataSource = datos;
            DataSouece = datos;
        }

        private void InformeEncuadreGremial_NeedDataSource(object sender, EventArgs e)
        {
            (sender as Telerik.Reporting.Processing.Report).DataSource = DataSouece;
            this.crosstab2.DataSource = DataSouece;
        }
    }
}