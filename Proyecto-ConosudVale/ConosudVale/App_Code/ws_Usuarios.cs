using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Entidades;

/// <summary>
/// Summary description for ws_Usuarios
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ws_Usuarios : System.Web.Services.WebService
{

    public ws_Usuarios()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    [WebMethod]
    public object getUsuarios()
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {
            var aa =  (from u in dc.SegUsuario
                    select new
                    {
                        Login = u.Login,
                        Password = u.Password,
                        Empresa =  u.Empresa != null ? u.Empresa.RazonSocial : "",
                        IdSegUsuario = u.IdSegUsuario
                    }).ToList().OrderBy(w => w.Login);

            return aa.ToList();
        }
    }

    [WebMethod]
    public object agregarContrato(long idUsuario, long idContrato)
    {
        using (EntidadesConosud dc = new EntidadesConosud())
        {

            SegContextos newContexto = new SegContextos();
            newContexto.SegUsuario = idUsuario;
            newContexto.Valor = idContrato.ToString();
            newContexto.Contexto = "CONTRATO";
            dc.AddToSegContextos(newContexto);
            dc.SaveChanges();

            var datosC = (from c in dc.SegContextos
                          where c.SegUsuario == idUsuario
                          select c).ToList();


            var datos = (from c in datosC
                         join c1 in dc.Contrato on long.Parse(c.Valor) equals c1.IdContrato
                         where c.SegUsuario == idUsuario
                         select new
                         {
                             NroContrato = c1.Codigo,
                             Vencimiento = c1.FechaInicio.Value.ToShortDateString(),
                             Empresa = c1.ContratoEmpresas.Where(w => w.EsContratista.Value).FirstOrDefault().Empresa.RazonSocial,
                             idSegContexto = c.IdSegContexto
                         }).Take(50).ToList();

            return datos.ToList();
        }


    }

    [WebMethod]
    public object eliminarContrato(long idSegUsuario)
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {

            var segContexto = (from c in dc.SegContextos
                               where c.IdSegContexto == idSegUsuario
                               select c).FirstOrDefault();

            long idUsuario = segContexto.SegUsuario.Value;

            dc.DeleteObject(segContexto);
            dc.SaveChanges();


            return getContratosAsignados(idUsuario);
        }

    }

    [WebMethod]
    public object getContratosAsignados(long idUsuario)
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {

            var datosC = (from c in dc.SegContextos
                          where c.SegUsuario == idUsuario
                          select c).ToList();


            var datos = (from c in datosC
                         join c1 in dc.Contrato on long.Parse(c.Valor) equals c1.IdContrato
                         where c.SegUsuario == idUsuario
                         select new
                         {
                             NroContrato = c1.Codigo,
                             Vencimiento = c1.FechaInicio.Value.ToShortDateString(),
                             Empresa = c1.ContratoEmpresas.Where(w => w.EsContratista.Value).FirstOrDefault().Empresa.RazonSocial,
                             idSegContexto = c.IdSegContexto


                         }).Take(50).ToList();

            return datos.ToList();
        }

    }


    [WebMethod]
    public object getContratosDisponibles(long idUsuario)
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {

            var contratosAsigandos = (from c in dc.SegContextos
                                      where c.SegUsuario == idUsuario
                                      select c.Valor).ToList().Select(w => long.Parse(w)).ToList();


            var datos = (from c in dc.Contrato
                         where !contratosAsigandos.Contains(c.IdContrato)
                         select new
                         {
                             IdContrato = c.IdContrato,
                             Contrato = c.Codigo,
                             Empresa = c.ContratoEmpresas.Where(w => w.EsContratista.Value).FirstOrDefault().Empresa.RazonSocial
                         }).OrderBy(w => w.Contrato).ToList();

            return datos.ToList();
        }

    }


}
