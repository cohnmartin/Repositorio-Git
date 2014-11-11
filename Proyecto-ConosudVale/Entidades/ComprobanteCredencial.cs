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
    public partial class ComprobanteCredencial : Telerik.Reporting.Report
    {
        public ComprobanteCredencial()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void InitReport(object img, List<CursosLegajos> Cursos, Legajos Leg)
        {
            pictureBox5.Value = img;

            /// Esto fue sacado de la credencial hasta
            /// nuevo aviso. 11/10/2011 Martin
            tblCursos.DataSource = (from c in Cursos
                                    select new
                                    {
                                        Cursos = c.objCurso.Descripcion
                                    }).ToList();


            if (Leg.NroPoliza!= null &&  Leg.NroPoliza.Trim() != "" && Leg.CompañiaSeguro != null)
            {
                lblSeguro.Value = "Acc Per:";
                txtSeguro.Value = Leg.NroPoliza + " - " + Leg.objCompañiaSeguro.Descripcion.Trim();
            }
            else
            {
                lblSeguro.Value = "ART:";
                txtSeguro.Value = Leg.objEmpresaLegajo.DescArt;
            }

        }
    }
}