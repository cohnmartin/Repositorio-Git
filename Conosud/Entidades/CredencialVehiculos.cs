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

    /// <summary>
    /// Summary description for InformeAltaBajaLegajo.
    /// </summary>
    public partial class CredencialVehiculos : Telerik.Reporting.Report
    {
        public CredencialVehiculos()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void InitReport(string Tipo,string Marca ,string Modelo,string Patente, string Vigencia,string NroTarjeta,string PuestoIngreso, string TipoUnidad)
        {
            txtTituloF.Value = Tipo;
            txtTituloR.Value = Tipo;
            txtMarca.Value = Marca;
            txtModelo.Value = Modelo;
            txtPatente.Value = Patente;
            txtVigencia.Value = Vigencia;
            txtTarjetF.Value = NroTarjeta;
            txtTarjetR.Value = NroTarjeta;
            barcode1.Value = Patente;
            txtPuestoIngreso.Value = PuestoIngreso;
            txtTipo.Value = TipoUnidad;

            if (Tipo != "OFICIAL")
            {
                pnlPrincipalF.Style.BackgroundColor = Color.Red;
                pnlPrincipalReverso.Style.BackgroundColor = Color.Red;
                txtTituloDominio.Style.Color = Color.Red;
                txtTituloVehiculo.Style.Color = Color.Red;
                txtTituloVigencia.Style.Color = Color.Red;

            }

        }
    }
}