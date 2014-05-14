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
    /// <summary>
    /// Summary description for InformeMapaHojasRuta.
    /// </summary>
    public partial class InformeMapaHojasRuta : Telerik.Reporting.Report
    {
        static List<InformeMapaHojasRutaTemp> datos;
        public InformeMapaHojasRuta()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();
            this.NeedDataSource += new EventHandler(InformeMapaHojasRuta_NeedDataSource);
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        void InformeMapaHojasRuta_NeedDataSource(object sender, EventArgs e)
        {
            //this.DataSource = datos;
        }

        public void InitReport(List<InformeMapaHojasRutaTemp> cabs)
        {
            datos = cabs;
            this.crosstab1.DataSource = datos;
        }

        

    }
}