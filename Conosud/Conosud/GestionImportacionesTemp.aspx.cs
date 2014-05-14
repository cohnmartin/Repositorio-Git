using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;

public partial class GestionImportacionesTemp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnActualizarVehiculos_Click(object sender, EventArgs e)
    {

        EntidadesConosud dc = new EntidadesConosud();

        var allVehiculos = (from v in dc.VehiculosYPF
                            select v).ToList();

        foreach (var item in allVehiculos)
        {

            VahiculosyEquipos obj = (from v in dc.VahiculosyEquipos
                                     where v.Patente.ToUpper() == item.Patente.ToUpper()
                                     select v).FirstOrDefault();

            if (obj != null)
            {
                obj.NroMotor = item.Motor;
                obj.NroChasis = item.Chasis;

                if (item.Año.Trim() != "")
                    obj.FechaFabricacion = Convert.ToDateTime("01/01/" + item.Año.Trim());

                obj.Modelo += item.Modelo;

            }
        }


        dc.SaveChanges();

        ResultActualizacion.InnerHtml = "Actualizacion Correcta";

    }

    protected void btnActualizar_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();

        var allVehiculos = (from v in dc.VahiculosyEquipos
                            select v).ToList();

        foreach (var item in allVehiculos)
        {
            item.FechaCreacion = item.FechaCreacion == null ? DateTime.Parse("16/11/2012") : item.FechaCreacion;
            item.FechaUltimaActualizacion = item.FechaUltimaActualizacion == null ? DateTime.Parse("16/11/2012") : item.FechaUltimaActualizacion;
        }


        dc.SaveChanges();



        var modifacion = (from vm in dc.ModificacionVehiculos
                          join v in dc.VahiculosyEquipos on vm.Patente.ToUpper().Trim() equals v.Patente.ToUpper().Trim()
                          select new
                          {
                              vehiculo = v,
                              vModifcacion = vm
                          }).ToList();


        foreach (var item in modifacion)
        {
            item.vehiculo.VencimientoCredencial = DateTime.Parse(item.vModifcacion.FechaVencimientoCredencial);
            item.vehiculo.FechaUltimaActualizacion = DateTime.Now;
        }

        dc.SaveChanges();


        ResultActualizacion.InnerHtml = "Resultado Actualizacion: " + modifacion.Count.ToString() + " Vehiculos y Equipos actualizados</br>";



    }

    protected void btnImpVehiculos_Click(object sender, EventArgs e)
    {

        EntidadesConosud dc = new EntidadesConosud();

        List<string> empresas = (from emp in dc.VehiculosEquiposImp
                                 where emp.Empresa != null
                                 select emp.Empresa).Distinct().ToList();


        var empresasEncontradas = (from emp in dc.Empresa
                                   where empresas.Contains(emp.RazonSocial)
                                   select new
                                   {
                                       emp.RazonSocial,
                                       emp.IdEmpresa
                                   }).ToList();


        var empresasNoEncontradas = (from emp in empresas
                                     where !empresasEncontradas.Select(w => w.RazonSocial).Contains(emp)
                                     select emp).ToList();


        //*-------------------------------------------------------------------------------------


        List<string> contratos = (from c in dc.VehiculosEquiposImp
                                  where c.Contrato != null
                                  select c.Contrato).Distinct().ToList();


        var contratosEncontradas = (from c in dc.Contrato
                                    where contratos.Contains(c.Codigo)
                                    select new
                                    {
                                        c.Codigo,
                                        c.IdContrato
                                    }).ToList();


        var contratosNoEncontradas = (from c in contratos
                                      where !contratosEncontradas.Select(w => w.Codigo).Contains(c)
                                      select c).ToList();


        //*-------------------------------------------------------------------------------------



        List<string> ciaSeguros = (from c in dc.VehiculosEquiposImp
                                   where c.Contrato != null
                                   select c.CiaSeguro.ToUpper().Trim()).Distinct().ToList();


        var ciaSegurosEncontradas = (from c in dc.Clasificacion
                                     where c.Tipo == "Compañias Seguro" && ciaSeguros.Contains(c.Descripcion.ToUpper().Trim())
                                     select new
                                     {
                                         c.Descripcion,
                                         c.IdClasificacion
                                     }).ToList();


        var ciaSegurosNoEncontradas = (from c in ciaSeguros
                                       where !ciaSegurosEncontradas.Select(w => w.Descripcion).Contains(c.ToUpper().Trim())
                                       select c).ToList();



        //*-------------------------------------------------------------------------------------



        List<string> tipoUnidades = (from c in dc.VehiculosEquiposImp
                                     where c.TipoUnidad != null
                                     select c.TipoUnidad.ToUpper().Trim()).Distinct().ToList();


        var tipoUnidadesEncontradas = (from c in dc.Clasificacion
                                       where (c.Tipo == "Tipo Vehiculos" || c.Tipo == "Tipo Equipos") && tipoUnidades.Contains(c.Descripcion.ToUpper().Trim())
                                       select new
                                       {
                                           c.Descripcion,
                                           c.IdClasificacion
                                       }).ToList();


        var tipoUnidadesNoEncontradas = (from c in tipoUnidades
                                         where !tipoUnidadesEncontradas.Select(w => w.Descripcion).Contains(c.ToUpper().Trim())
                                         select c).ToList();



        ResultImportacion.InnerHtml = "Empresas no encontradas: " + empresasNoEncontradas.Count.ToString() + "</br>";

        ResultImportacion.InnerHtml += "<ul>";
        foreach (var item in empresasNoEncontradas)
        {

            ResultImportacion.InnerHtml += "<li>" + item + "</li>";

        }
        ResultImportacion.InnerHtml += "</ul>";

        ResultImportacion.InnerHtml += "Contratos no encontrados: " + contratosNoEncontradas.Count.ToString() + "</br>";
        ResultImportacion.InnerHtml += "<ul>";
        foreach (var item in contratosNoEncontradas)
        {

            ResultImportacion.InnerHtml += "<li>" + item + "</li>";

        }
        ResultImportacion.InnerHtml += "</ul>";


        ResultImportacion.InnerHtml += "Compañias de seguro no encontrados: " + ciaSegurosNoEncontradas.Count.ToString();
        ResultImportacion.InnerHtml += "<ul>";
        foreach (var item in ciaSegurosNoEncontradas)
        {

            ResultImportacion.InnerHtml += "<li>" + item + "</li>";

        }
        ResultImportacion.InnerHtml += "</ul>";



        ResultImportacion.InnerHtml += "Tipo de Unidades no encontrados: " + tipoUnidadesNoEncontradas.Count.ToString();
        ResultImportacion.InnerHtml += "<ul>";
        foreach (var item in tipoUnidadesNoEncontradas)
        {

            ResultImportacion.InnerHtml += "<li>" + item + "</li>";

        }
        ResultImportacion.InnerHtml += "</ul>";









        List<VehiculosEquiposImp> vehiculos = (from v in dc.VehiculosEquiposImp
                                               where v.Empresa != null && v.Tipo == "VEH"
                                               select v).ToList();

        ResultImportacion.InnerHtml += "Total de Vehiculos a Importar: " + vehiculos.Count.ToString() + "</br>";

        int cant = 0;
        foreach (var item in vehiculos.Where(v => v.Contrato != null))
        {
            if (empresasEncontradas.Any(w => w.RazonSocial == item.Empresa) &&
                contratosEncontradas.Any(w => w.Codigo == item.Contrato) &&
                ciaSegurosEncontradas.Any(w => w.Descripcion == item.CiaSeguro) &&
                tipoUnidadesEncontradas.Any(w => w.Descripcion == item.TipoUnidad))
            {
                cant++;
                VahiculosyEquipos v = new VahiculosyEquipos();
                v.Tipo = "Vehículo";
                v.Empresa = empresasEncontradas.Where(w => w.RazonSocial == item.Empresa).First().IdEmpresa;
                v.Patente = item.Patente;

                if (item.FechaFabricacion != null)
                    v.FechaFabricacion = DateTime.Parse("01/01/" + item.FechaFabricacion.ToString());

                v.TipoUnidad = tipoUnidadesEncontradas.Where(w => w.Descripcion == item.TipoUnidad).First().IdClasificacion;
                v.Marca = item.Marca;
                v.NombreTitular = item.Titular;
                v.ContratoAfectado = contratosEncontradas.Where(w => w.Codigo == item.Contrato).First().IdContrato;
                v.NroPolizaSeguro = item.NroPoliza;
                v.CompañiaSeguro = ciaSegurosEncontradas.Where(w => w.Descripcion == item.CiaSeguro).First().IdClasificacion;
                v.FechaVencimientoHabilitacion = item.FechaHabilitacionCENT;

                if (item.FechaPoliza.Trim() != "")
                    v.FechaVencimientoSeguro = Convert.ToDateTime(item.FechaPoliza);

                v.VencimientoCredencial = item.FechaVencimientoCredencial;
                v.HabilitarCredencial = item.FechaVencimientoCredencial > DateTime.Now ? true : false;
                v.FechaUltimaActualizacion = DateTime.Now;
                v.FechaCreacion = DateTime.Now;
                dc.AddToVahiculosyEquipos(v);
            }

        }

        ResultImportacion.InnerHtml += "Total de Vehiculos Importados: " + cant.ToString() + "</br>";
        ResultImportacion.InnerHtml += "Posible Causas: No tiene contrato asignado </br>";



        List<VehiculosEquiposImp> equipos = (from v in dc.VehiculosEquiposImp
                                             where v.Empresa != null && v.Tipo != "VEH"
                                             select v).ToList();

        ResultImportacion.InnerHtml += "Total de equipos a Importar: " + equipos.Count.ToString() + "</br>";

        cant = 0;
        foreach (var item in equipos.Where(v => v.Contrato != null))
        {
            if (empresasEncontradas.Any(w => w.RazonSocial == item.Empresa) &&
                contratosEncontradas.Any(w => w.Codigo == item.Contrato) &&
                ciaSegurosEncontradas.Any(w => w.Descripcion == item.CiaSeguro) &&
                tipoUnidadesEncontradas.Any(w => w.Descripcion == item.TipoUnidad))
            {
                cant++;
                VahiculosyEquipos v = new VahiculosyEquipos();
                v.Tipo = "Equipo";
                v.Empresa = empresasEncontradas.Where(w => w.RazonSocial == item.Empresa).First().IdEmpresa;
                v.Patente = item.Patente;

                if (item.FechaFabricacion != null)
                    v.FechaFabricacion = DateTime.Parse("01/01/" + item.FechaFabricacion.ToString());

                v.TipoUnidad = tipoUnidadesEncontradas.Where(w => w.Descripcion == item.TipoUnidad).First().IdClasificacion;
                v.Marca = item.Marca;
                v.NombreTitular = item.Titular;
                v.ContratoAfectado = contratosEncontradas.Where(w => w.Codigo == item.Contrato).First().IdContrato;
                v.NroPolizaSeguro = item.NroPoliza;
                v.CompañiaSeguro = ciaSegurosEncontradas.Where(w => w.Descripcion == item.CiaSeguro).First().IdClasificacion;
                v.FechaVencimientoHabilitacion = item.FechaHabilitacionCENT;

                if (item.FechaPoliza.Trim() != "")
                    v.FechaVencimientoSeguro = Convert.ToDateTime(item.FechaPoliza);

                v.VencimientoCredencial = item.FechaVencimientoCredencial;
                v.HabilitarCredencial = item.FechaVencimientoCredencial > DateTime.Now ? true : false;
                v.FechaUltimaActualizacion = DateTime.Now;
                v.FechaCreacion = DateTime.Now;
                dc.AddToVahiculosyEquipos(v);


            }

        }

        ResultImportacion.InnerHtml += "Total de equipos Importados: " + cant.ToString() + "</br>";
        ResultImportacion.InnerHtml += "Posible Causas: No tiene contrato asignado </br>";

        dc.SaveChanges();
    }
}