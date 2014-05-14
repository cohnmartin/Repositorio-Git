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
    public partial class InformeEncuadreGremial : Telerik.Reporting.Report
    {
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
            this.crosstab2.DataSource = datos;
        }
    }
}