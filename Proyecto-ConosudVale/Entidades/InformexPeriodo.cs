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
    /// Summary description for InformexPeriodo.
    /// </summary>
    public partial class InformexPeriodo : Telerik.Reporting.Report
    {
        public List<HojasDeRuta> DataSouece
        {
            get
            {
                if (HttpContext.Current.Session["DataSouece"] != null)

                    return (List<HojasDeRuta>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<HojasDeRuta>));
                else
                {
                    return (List<HojasDeRuta>)Helper.DeSerializeObject(HttpContext.Current.Session["DataSouece"], typeof(List<HojasDeRuta>));
                }
            }
            set
            {
                HttpContext.Current.Session["DataSouece"] = Helper.SerializeObject(value);
            }
        }

        public InformexPeriodo()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void InitReport(CabeceraHojasDeRuta Cabecera, bool EsHistorico)
        {
            string periodo = String.Format("{0:yyyy/MM}", Cabecera.Periodo);

            Cabecera.ContratoEmpresasReference.Load();
            Cabecera.ContratoEmpresas.ContratoReference.Load();
            Cabecera.ContratoEmpresas.EmpresaReference.Load();


            if (EsHistorico)
            {
                txtLeyendaEncabezado.Value = "Luego de recibida y controlada la documentación por el contrato " + Cabecera.ContratoEmpresas.Contrato.Codigo +
                    " de la empresa " + Cabecera.ContratoEmpresas.Empresa.RazonSocial + " y subcontratistas (si tuviese), por el período " + periodo +
                    " y meses anteriores, si corresponde, se informa a continuación los pendientes:";
            }
            else
            {
                txtLeyendaEncabezado.Value = "Luego de recibida y controlada la documentación por el contrato " + Cabecera.ContratoEmpresas.Contrato.Codigo +
                      " de la empresa " + Cabecera.ContratoEmpresas.Empresa.RazonSocial + " y subcontratistas (si tuviese), por el período " + periodo +
                      ", se informan a continuación los pendientes:";
            }



            this.ReportParameters[0].Value = periodo;

            List<Entidades.HojasDeRuta> HojasConComentarios = (from H in Cabecera.HojasDeRuta
                                                               where H.HojaComentario != null && H.HojaComentario.Trim() != ""
                                                               select H).ToList();

            //this.DataSource = HojasConComentarios;
            DataSouece = HojasConComentarios;
        }

        private void InformexPeriodo_NeedDataSource(object sender, EventArgs e)
        {
            (sender as Telerik.Reporting.Processing.Report).DataSource = DataSouece;
        }
    }
}