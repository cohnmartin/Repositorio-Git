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
    using System.Web.SessionState;
    using System.Web;
    /// <summary>
    /// Summary description for InformeMensualEmpresasAuditar.
    /// </summary>
    public partial class InformeMensualEmpresasAuditar : Telerik.Reporting.Report
    {
        public List<InformeMensualEmpresaAuditarTemp> DataSouece
        {
            get
            {
                if (HttpContext.Current.Session["DataSouece"] != null)

                    return (List<InformeMensualEmpresaAuditarTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<InformeMensualEmpresaAuditarTemp>));
                else
                {
                    return (List<InformeMensualEmpresaAuditarTemp>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<InformeMensualEmpresaAuditarTemp>));
                }
            }
            set
            {
                HttpContext.Current.Session["DataSouece"] = Helper.SerializeObject(value);
            }
        }

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

        public void InitReport(List<InformeMensualEmpresaAuditarTemp> cabs, string PeriodoConsulta )
        {
            this.txtPeriodo.Value = PeriodoConsulta;

            //this.DataSource = cabs.OrderBy(w => w.CodigoContrato)
            //    .ThenBy(w => w.DescContratista)
            //    .ThenBy(w => w.DescSubContratista);

            DataSouece = cabs.OrderBy(w => w.CodigoContrato)
                .ThenBy(w => w.DescContratista)
                .ThenBy(w => w.DescSubContratista).ToList();
        }

        private void InformeMensualEmpresasAuditar_NeedDataSource(object sender, EventArgs e)
        {
            (sender as Telerik.Reporting.Processing.Report).DataSource = DataSouece;
        }
    }
}