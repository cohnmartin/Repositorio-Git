using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using System.Drawing.Imaging;
using System.Drawing;

public partial class ViewerCredenciales : System.Web.UI.Page
{
    public EntidadesConosud Contexto
    {
        get
        {
            if (Session["Contexto"] != null)
                return (EntidadesConosud)Session["Contexto"];
            else
            {
                Session["Contexto"] = new EntidadesConosud();
                return (EntidadesConosud)Session["Contexto"];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (Request.QueryString["IdVehiculoEquipo"] != null)
                {
                    long id = long.Parse(Request.QueryString["IdVehiculoEquipo"].ToString());
                    List<VahiculosyEquipos> VahiyEqui = (from U in Contexto.VahiculosyEquipos where U.IdVehiculoEquipo == id select U).ToList();

                    
                    CredencialVehiculos rep = new CredencialVehiculos();
                    string Tipo = VahiyEqui.First().objEmpresa.RazonSocial.Contains("YPF") ? "OFICIAL" : "CONTRATISTA";
                    string PuestoIngrso = VahiyEqui.First().PuestoIngreso == null ? "" : VahiyEqui.First().PuestoIngreso;
                    string modelo = VahiyEqui.First().Modelo == null ? "" : VahiyEqui.First().Modelo;
                    string tipoUnidad = VahiyEqui.First().TipoUnidad == null ? "" : VahiyEqui.First().objTipoUnidad.Descripcion;
                    rep.InitReport(Tipo, VahiyEqui.First().Marca, modelo,
                        VahiyEqui.First().Patente, string.Format("{0:dd/MM/yy}", VahiyEqui.First().VencimientoCredencial),
                        VahiyEqui.First().IdVehiculoEquipo.ToString().PadLeft(8, '0'), PuestoIngrso, tipoUnidad);

                    rep.DataSource = null;
                    this.ReportViewer1.Report = rep;
                }
                else
                {

                    CredencialVisita rep = new CredencialVisita();
                    this.ReportViewer1.Report = rep;
                }


            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.StackTrace.ToString());
            }
        }
    }
}