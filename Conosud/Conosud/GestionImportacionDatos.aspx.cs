using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using Telerik.Web.UI.Upload;
using System.IO;
using System.Data.OleDb;
using System.ComponentModel;

public partial class GestionImportacionDatos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    #region Importacion Desde Tablas

    protected void btnContratos_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();
        List<ContratosImp> contratos = (from C in dc.ContratosImp
                                        select C).ToList<ContratosImp>();



        foreach (ContratosImp cont in contratos)
        {

            GenerarContratos(cont, dc);

        }

        dc.SaveChanges();

    }
    private void GenerarContratos(ContratosImp ContImp, EntidadesConosud dc)
    {


        int HayEmpresa = (from E in dc.Empresa
                          where E.CUIT.Replace("-", "").Contains(ContImp.CUIT_FK.Replace("-", ""))
                          select E).Count();


        if (HayEmpresa > 0)
        {
            /// Creo el contrato
            Entidades.Contrato newContrato = new Entidades.Contrato();
            newContrato.Codigo = ContImp.Codigo;
            newContrato.FechaInicio = DateTime.Parse(ContImp.FechaInicio);
            newContrato.FechaVencimiento = DateTime.Parse(ContImp.FechaVencimiento);
            newContrato.Prorroga = ContImp.Prorroga;
            newContrato.Servicio = ContImp.Servicio.ToLower();

            newContrato.TipoContrato = (from T in dc.Clasificacion
                                        where T.Tipo == "Tipo Contrato" && T.Descripcion.Contains(ContImp.TipoContrato)
                                        select T).FirstOrDefault<Entidades.Clasificacion>();

            newContrato.Contratadopor = (from T in dc.Clasificacion
                                         where T.Tipo == "Contratado  por" && T.Descripcion.Contains(ContImp.ContratadoPor)
                                         select T).FirstOrDefault<Entidades.Clasificacion>();


            /// Creo el Contrato Empresa
            Entidades.ContratoEmpresas _ContEmp = new Entidades.ContratoEmpresas();

            _ContEmp.EsContratista = true;
            _ContEmp.Contrato = newContrato;
            _ContEmp.Empresa = (from E in dc.Empresa
                                where E.CUIT.Replace("-", "").Contains(ContImp.CUIT_FK.Replace("-", ""))
                                select E).First<Entidades.Empresa>();


            /// Creo las Hojas de Ruta
            DateTime FFin = DateTime.Now;
            if (newContrato.Prorroga.HasValue) { FFin = newContrato.Prorroga.Value; }
            else { FFin = newContrato.FechaVencimiento.Value; }
            DateTime FechaInicio = new DateTime(newContrato.FechaInicio.Value.Year, newContrato.FechaInicio.Value.Month, 1);

            Helpers.GenerarHojadeRuta(dc, FechaInicio, FFin, _ContEmp);


            /// Creo los contrato empresa para las subcontratista
            List<ContratoEmpresasImp> contratosSubContratistas = (from C in dc.ContratoEmpresasImp
                                                                  where C.Contratista.Replace("-", "") == _ContEmp.Empresa.CUIT.Replace("-", "")
                                                                  && C.NroContrato == _ContEmp.Contrato.Codigo
                                                                  select C).ToList<ContratoEmpresasImp>();

            foreach (ContratoEmpresasImp item in contratosSubContratistas)
            {

                Entidades.Empresa SubContratista = (from E in dc.Empresa
                                                    where E.CUIT.Replace("-", "").Contains(item.SubContratista.Replace("-", ""))
                                                    select E).FirstOrDefault<Entidades.Empresa>();
                if (SubContratista != null)
                {

                    /// creo el contrato empresa para el subcontratista
                    Entidades.ContratoEmpresas _ContEmpSub = new Entidades.ContratoEmpresas();
                    _ContEmpSub.EsContratista = false;
                    _ContEmpSub.Empresa = SubContratista;
                    newContrato.ContratoEmpresas.Add(_ContEmpSub);

                    /// Creo las Hojas de Ruta
                    FFin = DateTime.Now;
                    if (newContrato.Prorroga.HasValue) { FFin = newContrato.Prorroga.Value; }
                    else { FFin = newContrato.FechaVencimiento.Value; }
                    FechaInicio = new DateTime(newContrato.FechaInicio.Value.Year, newContrato.FechaInicio.Value.Month, 1);
                    Helpers.GenerarHojadeRuta(dc, FechaInicio, FFin, _ContEmpSub);

                }
            }


            dc.AddToContrato(newContrato);
        }


    }

    protected void btnLegajo_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();

        List<LegajosContratosImp> legajos = (from L in dc.LegajosContratosImp
                                             select L).ToList<LegajosContratosImp>();

        foreach (LegajosContratosImp item in legajos)
        {
            try
            {

                long idLegajo = GetLegajo(item);
                DateTime periodo = item.FechaIngreso.Value;
                long idEstado = 15;

                long idContratoEmpresa = (from CE in dc.ContratoEmpresas
                                          where CE.Contrato.Codigo == item.NroContrato
                                          && CE.Empresa.RazonSocial == item.Contratista
                                          select CE.IdContratoEmpresas).FirstOrDefault<long>();

                if (idContratoEmpresa > 0)
                {

                    List<Entidades.CabeceraHojasDeRuta> cabeceras = (from C in dc.CabeceraHojasDeRuta.Include("ContratoEmpresas")
                                                                     where C.Periodo >= periodo
                                                                     && C.ContratoEmpresas.IdContratoEmpresas == idContratoEmpresa
                                                                     && C.Estado.IdClasificacion == idEstado
                                                                     select C).ToList<Entidades.CabeceraHojasDeRuta>();



                    foreach (Entidades.CabeceraHojasDeRuta cab in cabeceras)
                    {
                        if (!cab.ContratoEmpresas.EsContratista.Value)
                        {
                            int a = 100;
                        }

                        Entidades.ContEmpLegajos ContEmpLeg = new Entidades.ContEmpLegajos();
                        ContEmpLeg.Legajos = (from L in dc.Legajos
                                              where L.IdLegajos == idLegajo
                                              select L).First<Entidades.Legajos>();
                        ContEmpLeg.ContratoEmpresas = cab.ContratoEmpresas;
                        ContEmpLeg.CabeceraHojasDeRuta = cab;
                        dc.AddToContEmpLegajos(ContEmpLeg);
                    }
                }

            }
            catch { }

        }

        dc.SaveChanges();



    }
    private long GetLegajo(LegajosContratosImp leg)
    {
        EntidadesConosud dc = new EntidadesConosud();
        string NroDoc = leg.NroDoc.Value.ToString();
        List<Entidades.Legajos> HayLegajo = (from L in dc.Legajos
                                             where L.NroDoc == NroDoc
                                             select L).ToList<Entidades.Legajos>();

        if (HayLegajo.Count == 1)
        {

            return HayLegajo[0].IdLegajos;
        }
        else
        {
            Entidades.Legajos newLegajo = new Entidades.Legajos();
            newLegajo.Apellido = leg.Apellido.Replace(",", "");
            newLegajo.Nombre = leg.Nombre.Replace("-", "");
            newLegajo.CodigoPostal = leg.CodigoPostal;
            newLegajo.CorreoElectronico = leg.CorreoElectronico;
            newLegajo.CUIL = leg.CUIL;
            newLegajo.Direccion = leg.Direccion;

            if (leg.FechaNacimiento != null)
                newLegajo.FechaNacimiento = DateTime.Parse(leg.FechaNacimiento);

            newLegajo.NroDoc = leg.NroDoc.ToString();
            newLegajo.TelefonoFijo = leg.TelefonoFijo;


            newLegajo.objConvenio = (from C in dc.Clasificacion
                                     where C.Tipo == "Convenio" &&
                                     C.Descripcion.Contains(leg.Convenio)
                                     select C).FirstOrDefault<Entidades.Clasificacion>();


            dc.AddToLegajos(newLegajo);
            dc.SaveChanges();
            return newLegajo.IdLegajos;
        }


    }

    protected void btnEmpresas_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();

        List<Entidades.EmpresasImp> empresass = (from E in dc.EmpresasImp
                                                 select E).ToList<Entidades.EmpresasImp>();


        foreach (Entidades.EmpresasImp emp in empresass)
        {
            string cuitEmpresa = emp.CUIT.Value.ToString().Replace("-", "");
            int HayEmpresa = (from E in dc.Empresa
                              where E.CUIT.Replace("-", "").Contains(cuitEmpresa)
                              select E).Count();


            if (HayEmpresa == 0)
            {

                Entidades.Empresa newemp = new Entidades.Empresa();
                newemp.CUIT = emp.CUIT.ToString();
                newemp.RazonSocial = emp.RazonSocial;
                newemp.Telefono = emp.Telefono;
                newemp.FechaAlta = emp.FechaAlta;
                newemp.Direccion = emp.Direccion;
                newemp.RepresentanteTecnico = emp.RepresentanteTecnico;
                newemp.Emergencia = emp.Emergencias;
                newemp.PrestacionEmergencia = emp.PrestacionEmergencia;
                newemp.CorreoElectronico = emp.CorreoElectronico;
                newemp.objART = (from Art in dc.Clasificacion
                                 where Art.Tipo == "ART"
                                 && Art.Descripcion == emp.ART
                                 select Art).FirstOrDefault();


                dc.AddToEmpresa(newemp);

            }


        }

        dc.SaveChanges();


    }
    #endregion
    protected void btnContProrr_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();

        List<Entidades.Contrato> contratos = (from C in dc.Contrato
                                              where C.Prorroga != null
                                              select C).ToList<Entidades.Contrato>();


        foreach (Entidades.Contrato itemContratos in contratos)
        {
            if (!itemContratos.ContratoEmpresas.IsLoaded) { itemContratos.ContratoEmpresas.Load(); }
            /// Para cada contrato empresa busco los legajos de la ultima hoja de ruta
            /// segun la fecha de vencimiento.
            foreach (Entidades.ContratoEmpresas itemContratosEmp in itemContratos.ContratoEmpresas)
            {
                if (!itemContratosEmp.CabeceraHojasDeRuta.IsLoaded) { itemContratosEmp.CabeceraHojasDeRuta.Load(); }
                Entidades.CabeceraHojasDeRuta CabFechaVencimiento = itemContratosEmp.CabeceraHojasDeRuta.Where(C => C.Periodo.Month == itemContratos.FechaVencimiento.Value.Month && C.Periodo.Year == itemContratos.FechaVencimiento.Value.Year).FirstOrDefault();

                /// Legajos que se tienen que copiar
                if (CabFechaVencimiento != null)
                {

                    if (!CabFechaVencimiento.ContEmpLegajos.IsLoaded) { CabFechaVencimiento.ContEmpLegajos.Load(); }

                    /// cabecera pertenecientes a la prorroga
                    List<Entidades.CabeceraHojasDeRuta> CabProrrogadas = itemContratosEmp.CabeceraHojasDeRuta.Where(C => (C.Periodo.Month > itemContratos.FechaVencimiento.Value.Month && C.Periodo.Year == itemContratos.FechaVencimiento.Value.Year) || C.Periodo.Year > itemContratos.FechaVencimiento.Value.Year).ToList();
                    foreach (Entidades.CabeceraHojasDeRuta itemCabPro in CabProrrogadas)
                    {
                        /// si la cabecera prorrogada no tiene legajos asociados
                        /// entonces le copio los legados de la cabecera segun la
                        /// fecha de vencimiento
                        if (!itemCabPro.ContEmpLegajos.IsLoaded) { itemCabPro.ContEmpLegajos.Load(); }
                        if (itemCabPro.ContEmpLegajos != null && itemCabPro.ContEmpLegajos.Count == 0)
                        {
                            foreach (Entidades.ContEmpLegajos itemLeg in CabFechaVencimiento.ContEmpLegajos)
                            {
                                if (!itemLeg.LegajosReference.IsLoaded) { itemLeg.LegajosReference.Load(); }
                                Entidades.ContEmpLegajos newContLeg = new Entidades.ContEmpLegajos();
                                newContLeg.ContratoEmpresas = itemContratosEmp;
                                newContLeg.CabeceraHojasDeRuta = itemCabPro;
                                newContLeg.Legajos = itemLeg.Legajos;
                                dc.AddToContEmpLegajos(newContLeg);
                            }


                        }

                    }
                }
            }

            dc.SaveChanges();

        }



    }


    protected void btnMesNoviembre_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();
        
        //select * from cabecerahojasderuta where idcontratoempresa in (2128, 
        //2129 ,2130 ,2132 ,2133 ,2139 ,2151 ,2152 ,2153 ,2156 ) and
        //month(Periodo) = 12 and year(Periodo) = 2010

        
        try
        {
            List<Entidades.CabeceraHojasDeRuta> cabeceras = (from C in dc.CabeceraHojasDeRuta
                                                             where (C.ContratoEmpresas.IdContratoEmpresas == 2128
                                                                    || C.ContratoEmpresas.IdContratoEmpresas == 2129
                                                                    || C.ContratoEmpresas.IdContratoEmpresas == 2130
                                                                    || C.ContratoEmpresas.IdContratoEmpresas == 2132
                                                                    || C.ContratoEmpresas.IdContratoEmpresas == 2139
                                                                    || C.ContratoEmpresas.IdContratoEmpresas == 2151
                                                                    || C.ContratoEmpresas.IdContratoEmpresas == 2152
                                                                    || C.ContratoEmpresas.IdContratoEmpresas == 2153
                                                                    || C.ContratoEmpresas.IdContratoEmpresas == 2156)
                                                             && C.Periodo.Month == 12 && C.Periodo.Year == 2010
                                                             select C).ToList<Entidades.CabeceraHojasDeRuta>();



            foreach (Entidades.CabeceraHojasDeRuta itemCab in cabeceras)
            {
                if (!itemCab.ContratoEmpresasReference.IsLoaded) { itemCab.ContratoEmpresasReference.Load(); }
                if (!itemCab.EstadoReference.IsLoaded) { itemCab.EstadoReference.Load(); }

                // 1. Creo la cabecera para el periodo 11
                Entidades.CabeceraHojasDeRuta newCab = new Entidades.CabeceraHojasDeRuta();
                newCab.Periodo = DateTime.Parse("01/11/2010");
                newCab.ContratoEmpresas = itemCab.ContratoEmpresas;
                newCab.Estado = itemCab.Estado;
                newCab.NroCarpeta = itemCab.NroCarpeta;
                dc.AddToCabeceraHojasDeRuta(newCab);

                //2. Creo los items de la hoja de ruta
                if (!itemCab.HojasDeRuta.IsLoaded) { itemCab.HojasDeRuta.Load(); }
                foreach (var item in itemCab.HojasDeRuta)
                {
                    if (!item.PlantillaReference.IsLoaded) { item.PlantillaReference.Load(); }
                    Entidades.HojasDeRuta newHoja = new Entidades.HojasDeRuta();
                    newHoja.CabeceraHojasDeRuta = newCab;
                    newHoja.Plantilla = item.Plantilla;
                }

                //3. Creo los legajos asociados
                if (!itemCab.ContEmpLegajos.IsLoaded) { itemCab.ContEmpLegajos.Load(); }
                foreach (var item in itemCab.ContEmpLegajos)
                {
                    if (!item.LegajosReference.IsLoaded) { item.LegajosReference.Load(); }
                    Entidades.ContEmpLegajos newLeg = new Entidades.ContEmpLegajos();
                    newLeg.CabeceraHojasDeRuta = newCab;
                    newLeg.ContratoEmpresas = itemCab.ContratoEmpresas;
                    newLeg.Legajos = item.Legajos;
                }


                dc.SaveChanges();

            }
        }
        catch (Exception er){
            throw er;
        }


    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();
        int Mes_AGenerar = int.Parse(txtMesAño.Text.Split('/')[0]);
        int Año_AGenerar = int.Parse(txtMesAño.Text.Split('/')[1]);

        int Mes_Refrencia = Mes_AGenerar - 1;
        int Año_Refrencia = Año_AGenerar;


        Entidades.CabeceraHojasDeRuta cabeceraAnterior = (from C in dc.CabeceraHojasDeRuta
                                                         where (C.ContratoEmpresas.IdContratoEmpresas == 2073)
                                                         && C.Periodo.Month == Mes_Refrencia && C.Periodo.Year == Año_Refrencia
                                                         select C).FirstOrDefault();

        if (cabeceraAnterior != null)
        {

            if (!cabeceraAnterior.ContratoEmpresasReference.IsLoaded) { cabeceraAnterior.ContratoEmpresasReference.Load(); }
            if (!cabeceraAnterior.EstadoReference.IsLoaded) { cabeceraAnterior.EstadoReference.Load(); }

            // 1. Creo la cabecera para el periodo solicitado
            Entidades.CabeceraHojasDeRuta newCab = new Entidades.CabeceraHojasDeRuta();
            newCab.Periodo = DateTime.Parse("01/" + Mes_AGenerar.ToString() + "/" + Año_AGenerar.ToString());
            newCab.ContratoEmpresas = cabeceraAnterior.ContratoEmpresas;
            newCab.Estado = cabeceraAnterior.Estado;
            newCab.NroCarpeta = cabeceraAnterior.NroCarpeta;
            newCab.Publicar = false;
            newCab.EsFueraTermino = false;
            newCab.Estimacion = string.Empty;
            dc.AddToCabeceraHojasDeRuta(newCab);

            //2. Creo los items de la hoja de ruta
            if (!cabeceraAnterior.HojasDeRuta.IsLoaded) { cabeceraAnterior.HojasDeRuta.Load(); }
            foreach (var item in cabeceraAnterior.HojasDeRuta)
            {
                if (!item.PlantillaReference.IsLoaded) { item.PlantillaReference.Load(); }
                Entidades.HojasDeRuta newHoja = new Entidades.HojasDeRuta();
                newHoja.CabeceraHojasDeRuta = newCab;
                newHoja.Plantilla = item.Plantilla;
                newHoja.HojaComentario = string.Empty;
                newHoja.DocComentario = string.Empty;
                newHoja.HojaAprobado = false;
            }

            //3. Creo los legajos asociados
            if (!cabeceraAnterior.ContEmpLegajos.IsLoaded) { cabeceraAnterior.ContEmpLegajos.Load(); }
            foreach (var item in cabeceraAnterior.ContEmpLegajos)
            {
                if (!item.LegajosReference.IsLoaded) { item.LegajosReference.Load(); }
                Entidades.ContEmpLegajos newLeg = new Entidades.ContEmpLegajos();
                newLeg.CabeceraHojasDeRuta = newCab;
                newLeg.ContratoEmpresas = cabeceraAnterior.ContratoEmpresas;
                newLeg.Legajos = item.Legajos;
            }


            dc.SaveChanges();

        }


    }
}
