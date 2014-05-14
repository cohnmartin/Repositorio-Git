namespace Entidades
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Summary description for InformeMensualEmpresasAuditar.
    /// </summary>
    public partial class InformeMensualEmpresasAuditar : Telerik.Reporting.Report
    {
        public InformeMensualEmpresasAuditar()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void InitReport(List<InformeMensualEmpresaAuditarTemp> cabs)
        {
            this.DataSource = cabs.OrderBy(w => w.CodigoContrato)
                .ThenBy(w => w.DescContratista)
                .ThenBy(w => w.DescSubContratista);
        }
    }
}