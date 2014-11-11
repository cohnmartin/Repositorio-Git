using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Entidades;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (this.Session["idusu"] == null)
            Response.Redirect("~/Login.aspx");
        else
        {
            DateTime fechaLimite = DateTime.Now.AddYears(-1);
            using (EntidadesConosud dc = new EntidadesConosud())
            {
                List<Legajos> legs = (from l in dc.Legajos
                                      where l.FechaUltimoExamen.HasValue && l.FechaUltimoExamen.Value < fechaLimite
                                      && (l.EstudiosBasicos.Value || l.ComplementarioRacs.Value || l.AdicionalQuimicos.Value)
                                      select l).ToList();


                if (legs.Count > 0)
                {
                    /// Genero el body con los estados actuales antes de actualizar los mismos.
                    string body = GenerarBodyMail(legs.OrderBy(w => w.Apellido).ToList());

                    foreach (Legajos item in legs)
                    {
                        item.EstudiosBasicos = false;
                        item.ComplementarioRacs = false;
                        item.AdicionalQuimicos = false;
                    }

                    dc.SaveChanges();


                    //List<string> DireccionDestino = new List<string>() { "martacorrea@conosudsrl.com.ar", "dfcorrea@conosudsrl.com.ar" };
                    //string DireccionOrigen = "soporteconosud@infolegacy.com.ar";
                    //string Subject = "Actualización Legajos - Vencimiento Examen Médico";
                    //string SMTPMailOrigen = "mail.infolegacy.com.ar";
                    //string UsuarioMailOrigen = "soporteconosud@infolegacy.com.ar";
                    //string ClaveMailOrigen = "Conosud123";

                    //Helpers.EnvioActualizacionExamenMedico(body, DireccionDestino, DireccionOrigen, Subject, SMTPMailOrigen, UsuarioMailOrigen, ClaveMailOrigen);

                }



            }

        }

        this.lblNombreUsu.Text = Convert.ToString(this.Session["nombreusu"]); ;
    }

    private string GenerarBodyMail(List<Legajos> legs)
    {

        string tbl = "<table>";
        tbl += "<tr>";
        tbl += "    <td>";
        tbl += "Personal de Asistencia Conosud:";
        tbl += "</td>";
        tbl += "</tr>";
        tbl += "<tr>";
        tbl += "<td>";
        tbl += "El <span style='font-weight:bold' id='sSolitante' runat='server'>Sistema de soporte automático</span>";
        tbl += " informa que se ha realizado una actualización correcta de legajos, con motivo del vencimiento de la fecha del examen médico. ";
        tbl += "<span style='font-weight:bold' id='Span1' runat='server'>A continuación se lista los legajos actualizados:</span>";
        tbl += "</td>";
        tbl += "</tr>";
        tbl += "<tr>";
        tbl += "   <td>";
        tbl += "        &nbsp;";
        tbl += "   </td>";
        tbl += "</tr>";
        tbl += "</table>";


        tbl += "<table border='1' width='650px'>";
        tbl += "<tr>";
        tbl += "    <td style='background-color: #E0D6BE;font-family: Sans-Serif; font-size: 12px;'>";
        tbl += "Legajo";
        tbl += "    </td>";
        tbl += "    <td style='background-color: #E0D6BE;font-family: Sans-Serif; font-size: 12px;'>";
        tbl += "DNI";
        tbl += "    </td>";
        tbl += "    <td style='background-color: #E0D6BE;font-family: Sans-Serif; font-size: 12px;'>";
        tbl += "Fecha Examen Médico";
        tbl += "    </td>";
        tbl += "    <td style='background-color: #E0D6BE;font-family: Sans-Serif; font-size: 12px;'>";
        tbl += "Días Transcurridos";
        tbl += "    </td>";
        tbl += "    <td style='background-color: #E0D6BE;font-family: Sans-Serif; font-size: 12px;'>";
        tbl += "Estudios Básicos";
        tbl += "    </td>";
        tbl += "    <td style='background-color: #E0D6BE;font-family: Sans-Serif; font-size: 12px;'>";
        tbl += "Complementarios Racs";
        tbl += "    </td>";
        tbl += "    <td style='background-color: #E0D6BE;font-family: Sans-Serif; font-size: 12px;'>";
        tbl += "Adicional Químicos";
        tbl += "    </td>";
        tbl += "</tr>";

        foreach (Legajos item in legs)
        {
            decimal dias = new TimeSpan(DateTime.Now.Ticks - item.FechaUltimoExamen.Value.Ticks).Days;
            string estBasicos = item.EstudiosBasicos.HasValue ? item.EstudiosBasicos.Value ? "Si" : "No" : "No";
            string estComplementarios = item.ComplementarioRacs.HasValue ? item.ComplementarioRacs.Value ? "Si" : "No" : "No";
            string estQuimicos = item.AdicionalQuimicos.HasValue ? item.AdicionalQuimicos.Value ? "Si" : "No" : "No";


            tbl += "<tr>";
            tbl += "    <td>";
            tbl += Helper.ToCapitalize(item.Apellido + ", " + item.Nombre);
            tbl += "    </td>";
            tbl += "    <td>";
            tbl += item.NroDoc;
            tbl += "    </td>";
            tbl += "    <td>";
            tbl += item.FechaUltimoExamen.Value.ToShortDateString();
            tbl += "    </td>";
            tbl += "    <td>";
            tbl += dias.ToString() + string.Format(" ({0:0.0} años aprox.)", dias / 365).Replace(",", ".");
            tbl += "    </td>";
            tbl += "    <td>";
            tbl += estBasicos;
            tbl += "    </td>";
            tbl += "    <td>";
            tbl += estComplementarios;
            tbl += "    </td>";
            tbl += "    <td>";
            tbl += estQuimicos;
            tbl += "    </td>";
            tbl += "</tr>";
        }
        
        tbl += "</table>";

        return tbl;
    }
}
