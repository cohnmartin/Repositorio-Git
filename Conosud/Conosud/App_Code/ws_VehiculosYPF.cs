using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Entidades;

/// <summary>
/// Summary description for ws_VehiculosYPF
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ws_VehiculosYPF : System.Web.Services.WebService
{

    public ws_VehiculosYPF()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    public bool GrabarVehiculo(IDictionary<string, object> vehiculo)
    {
        DateTime? fechaNula = null;

        using (EntidadesConosud dc = new EntidadesConosud())
        {
            VehiculosYPF current = null;

            if (vehiculo.ContainsKey("Id"))
            {
                long id = long.Parse(vehiculo["Id"].ToString());
                current = (from v in dc.VehiculosYPF
                           where v.Id == id
                           select v).FirstOrDefault();
            }
            else
            {
                current = new VehiculosYPF();
                dc.AddToVehiculosYPF(current);
            }

            current.Patente = vehiculo["Patente"].ToString();
            current.Modelo = vehiculo["Modelo"].ToString();

            if (vehiculo.ContainsKey("IdDepartamento") && vehiculo["IdDepartamento"] != null)
                current.Departamento = long.Parse(vehiculo["IdDepartamento"].ToString());

            if (vehiculo.ContainsKey("IdSector") && vehiculo["IdSector"] != null)
                current.Sector = long.Parse(vehiculo["IdSector"].ToString());


            current.Titular = vehiculo["Titular"].ToString();
            current.Combustible = long.Parse(vehiculo["IdTipoCombustible"].ToString());

            if (vehiculo.ContainsKey("IdTipoAsignacion") && vehiculo["IdTipoAsignacion"] != null)
                current.TipoAsignacion = long.Parse(vehiculo["IdTipoAsignacion"].ToString());

            if (vehiculo.ContainsKey("CentroCosto") && vehiculo["CentroCosto"] != null)
                current.CentroCosto = vehiculo["CentroCosto"].ToString();

            current.FechaVtoTarjVerde = vehiculo["VtoTarjVerde"].ToString() != null ? Convert.ToDateTime(vehiculo["VtoTarjVerde"].ToString()) : fechaNula;
            current.FechaVtoRevTecnica = vehiculo.ContainsKey("VtoRevTecnica") && vehiculo["VtoRevTecnica"] != null ? Convert.ToDateTime(vehiculo["VtoRevTecnica"].ToString()) : fechaNula;
            current.VelocimetroFecha = vehiculo.ContainsKey("VelocimetroFecha") && vehiculo["VelocimetroFecha"] != null ? Convert.ToDateTime(vehiculo["VelocimetroFecha"].ToString()) : fechaNula;

            if (vehiculo.ContainsKey("Contrato") && vehiculo["Contrato"] != null)
                current.Contrato = vehiculo["Contrato"].ToString();

            if (vehiculo.ContainsKey("NroTarjeta") && vehiculo["NroTarjeta"] != null)
                current.NroTarjeta = vehiculo["NroTarjeta"].ToString();

            if (vehiculo.ContainsKey("VelocimetroOdometro") && vehiculo["VelocimetroOdometro"] != null)
                current.VelocimetroOdometro = vehiculo["VelocimetroOdometro"].ToString();

            current.Año = vehiculo["Anio"].ToString();

            if (vehiculo.ContainsKey("Observacion") && vehiculo["Observacion"] != null)
                current.Observacion = vehiculo["Observacion"].ToString();

            dc.SaveChanges();
        }
        return true;
    }

    [WebMethod]
    public bool BajaVehiculo(long Id)
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {

            var vehiculo = (from v in dc.VehiculosYPF
                            where v.Id == Id
                            select v).FirstOrDefault();


            if (vehiculo != null)
            {

                vehiculo.FechaBaja = DateTime.Now;
                dc.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }



    }

    [WebMethod]
    public object filtrarVehiculos(string nroPatente)
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {
            long? nullValue = null;

            var datos = (from v in dc.VehiculosYPF
                         where v.Patente.Contains(nroPatente)
                         select new
                         {
                             v.Id,
                             v.Patente,
                             v.Modelo,
                             Departamento = v.objDepartamento,
                             Sector = v.objSector,
                             TipoCombustible = v.objTipoCombustible,
                             TipoAsignacion = v.objTipoAsignacion,

                             v.Titular,
                             v.FechaBaja,
                             CentroCosto = v.CentroCosto,
                             VtoTarjVerde = v.FechaVtoTarjVerde,
                             VtoRevTecnica = v.FechaVtoRevTecnica,
                             VelocimetroFecha = v.VelocimetroFecha,
                             v.Contrato,
                             v.NroTarjeta,
                             v.VelocimetroOdometro,
                             v.Año,
                             v.Observacion

                         }).Take(25).ToList();


            return (from v in datos
                    select new
                    {
                        v.Id,
                        v.Patente,
                        v.Modelo,

                        IdDepartamento = v.Departamento != null ? v.Departamento.IdClasificacion : nullValue,
                        Departamento = v.Departamento != null ? v.Departamento.Descripcion : "",

                        IdSector = v.Sector != null ? v.Sector.IdClasificacion : nullValue,
                        Sector = v.Sector != null ? v.Sector.Descripcion : "",

                        IdTipoAsignacion = v.TipoAsignacion != null ? v.TipoAsignacion.IdClasificacion : nullValue,
                        TipoAsignacion = v.TipoAsignacion != null ? v.TipoAsignacion.Descripcion : "",

                        IdTipoCombustible = v.TipoCombustible != null ? v.TipoCombustible.IdClasificacion : nullValue,
                        TipoCombustible = v.TipoCombustible != null ? v.TipoCombustible.Descripcion : "",


                        v.Titular,
                        FechaBaja = string.Format("{0:dd/MM/yyyy}", v.FechaBaja),
                        v.CentroCosto,
                        VtoTarjVerde = string.Format("{0:dd/MM/yyyy}", v.VtoTarjVerde),
                        VtoRevTecnica = string.Format("{0:dd/MM/yyyy}", v.VtoRevTecnica),
                        VelocimetroFecha = string.Format("{0:dd/MM/yyyy}", v.VelocimetroFecha),
                        v.Contrato,
                        v.NroTarjeta,
                        v.VelocimetroOdometro,
                        Anio = v.Año,
                        v.Observacion

                    }).ToList();



        }

    }

    [WebMethod]
    public object getVehiculos()
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {
            long? nullValue = null;

            var datos = (from v in dc.VehiculosYPF
                         select new
                         {
                             v.Id,
                             v.Patente,
                             v.Modelo,
                             Departamento = v.objDepartamento,
                             Sector = v.objSector,
                             TipoCombustible = v.objTipoCombustible,
                             TipoAsignacion = v.objTipoAsignacion,

                             v.Titular,
                             v.FechaBaja,
                             CentroCosto = v.CentroCosto,
                             VtoTarjVerde = v.FechaVtoTarjVerde,
                             VtoRevTecnica = v.FechaVtoRevTecnica,
                             VelocimetroFecha = v.VelocimetroFecha,
                             v.Contrato,
                             v.NroTarjeta,
                             v.VelocimetroOdometro,
                             v.Año,
                             v.Observacion

                         }).Take(10).ToList();


            return (from v in datos
                    select new
                    {
                        v.Id,
                        v.Patente,
                        v.Modelo,

                        IdDepartamento = v.Departamento != null ? v.Departamento.IdClasificacion : nullValue,
                        Departamento = v.Departamento != null ? v.Departamento.Descripcion : "",

                        IdSector = v.Sector != null ? v.Sector.IdClasificacion : nullValue,
                        Sector = v.Sector != null ? v.Sector.Descripcion : "",

                        IdTipoAsignacion = v.TipoAsignacion != null ? v.TipoAsignacion.IdClasificacion : nullValue,
                        TipoAsignacion = v.TipoAsignacion != null ? v.TipoAsignacion.Descripcion : "",

                        IdTipoCombustible = v.TipoCombustible != null ? v.TipoCombustible.IdClasificacion : nullValue,
                        TipoCombustible = v.TipoCombustible != null ? v.TipoCombustible.Descripcion : "",


                        v.Titular,
                        FechaBaja = string.Format("{0:dd/MM/yyyy}", v.FechaBaja),
                        v.CentroCosto,
                        VtoTarjVerde = string.Format("{0:dd/MM/yyyy}", v.VtoTarjVerde),
                        VtoRevTecnica = string.Format("{0:dd/MM/yyyy}", v.VtoRevTecnica),
                        VelocimetroFecha = string.Format("{0:dd/MM/yyyy}", v.VelocimetroFecha),
                        v.Contrato,
                        v.NroTarjeta,
                        v.VelocimetroOdometro,
                        Anio = v.Año,
                        v.Observacion

                    }).ToList();

        }

    }

    [WebMethod]
    public object getContextoClasificaciones()
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {

            return (from c in dc.Clasificacion
                    where c.Tipo == Helpers.Constantes.ContextoVehiculosYPF
                    select c).FirstOrDefault().Hijos.Select(c => new
                    {
                        Id = c.IdClasificacion,
                        Descripcion = c.Descripcion,
                        Tipo = c.Tipo
                    }).ToList();
        }

    }


}
