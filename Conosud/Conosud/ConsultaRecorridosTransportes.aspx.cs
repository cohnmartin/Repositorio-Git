using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using System.Web.Services;

public partial class ConsultaRecorridosTransportes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            using (EntidadesConosud dc = new EntidadesConosud())
            {
                cboRecorridos.DataTextField = "Descripcion";
                cboRecorridos.DataValueField = "Id";
                cboRecorridos.DataSource = (from c in dc.CabeceraRutasTransportes
                                            select new
                                                {
                                                    Id = c.Id,
                                                    Descripcion = c.Empresa + " - LINEA: " + c.Linea + " Turno:" + c.Turno + " - " +c.TipoTurno
                                                }).ToList();
                cboRecorridos.DataBind();    
            }

            
        }
    }

    [WebMethod(EnableSession = true)]
    public static object GetRecorrido(long idcab)
    {
        Dictionary<string, object> datos = new Dictionary<string, object>();

        using (EntidadesConosud dc = new EntidadesConosud())
        {
            var recorrido = (from c in dc.CabeceraRutasTransportes
                             where c.Id == idcab
                             select new
                             {
                                 recorrido = c.RutasTransportes.Select(w => new { w.Latitud, w.Longitud }),
                                 c.Empresa,
                                 Horario = c.HorariosSalida + " - " + c.HorariosLlegada,
                                 TipoRecorrido = "IDA"

                             }).FirstOrDefault();

            datos.Add("InfoRecorrido", recorrido);
          
        }
        

       return datos;
    }
}