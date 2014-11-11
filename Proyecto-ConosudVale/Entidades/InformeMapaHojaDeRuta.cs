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
    public partial class InformeMapaHojaDeRuta : Telerik.Reporting.Report
    {
        public List<InformeMapaHojasRutaTemp> DataSouece
        {
            get
            {
                if (HttpContext.Current.Session["DataSouece"] != null)

                    return (List<InformeMapaHojasRutaTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<InformeMapaHojasRutaTemp>));
                else
                {
                    return (List<InformeMapaHojasRutaTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<InformeMapaHojasRutaTemp>));
                }
            }
            set
            {
                HttpContext.Current.Session["DataSouece"] = Helper.SerializeObject(value);
            }
        }

        public InformeMapaHojaDeRuta()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();
            
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void InitReport(List<InformeMapaHojasRutaTemp> cabs)
        {
            //this.crosstab1.DataSource = cabs;
            DataSouece = cabs;
        }

        private void InformeMapaHojaDeRuta_NeedDataSource(object sender, EventArgs e)
        {
            (sender as Telerik.Reporting.Processing.Report).DataSource = DataSouece;
            this.crosstab1.DataSource = DataSouece;
        }
    }
}