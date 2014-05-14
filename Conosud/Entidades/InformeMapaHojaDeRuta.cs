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
    /// Summary description for InformeMapaHojaDeRuta.
    /// </summary>
    public partial class InformeMapaHojaDeRuta : Telerik.Reporting.Report
    {
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
            this.crosstab1.DataSource = cabs;
        }
    }
}