using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using AjaxControlToolkit;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Entidades;


/// <summary>
/// Summary description for ServiceResumen
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class ServiceResumen : System.Web.Services.WebService {

    public static string _PeriodoDefaul = "";

    public ServiceResumen () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetEmpresas(string knownCategoryValues,string category)
    {
        long IdUsuario = long.Parse(this.Application["idusuario"].ToString());

        EntidadesConosud db = new EntidadesConosud();
        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

        List<Entidades.SegUsuario> usuario = (from U in db.SegUsuario.Include("Empresa")
                       where U.IdSegUsuario == IdUsuario
                            && U.Empresa != null
                            select U).ToList<Entidades.SegUsuario>();


        if (usuario.Count == 1)
        {

            string make = usuario[0].Empresa.RazonSocial;
            long makeId = usuario[0].Empresa.IdEmpresa;
            values.Add(new CascadingDropDownNameValue(
              make, makeId.ToString()));

        }
        else
        {
            var varempresas = (from C in db.ContratoEmpresas.Include("Empresa")
                               where C.EsContratista == true
                               select C.Empresa).Distinct<Entidades.Empresa>();

            var Empresas = (from Emp in varempresas
                        orderby Emp.RazonSocial
                        select Emp).ToList<Entidades.Empresa>();



            foreach (Entidades.Empresa E in Empresas)
            {
                string make = E.RazonSocial;
                long makeId = E.IdEmpresa;
                values.Add(new CascadingDropDownNameValue(
                  make, makeId.ToString()));
            }
        }

        return values.ToArray();

    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetContratos(string knownCategoryValues, string category)
    {
        string[] Empresasvalues = knownCategoryValues.Split(':', ';');

        // Convert the element at index 1 in the string[] to get the CarId
        int _empresaID = Convert.ToInt32(Empresasvalues[1]);

        DSConosudTableAdapters.ContratoTableAdapter adapter = new DSConosudTableAdapters.ContratoTableAdapter();
        DSConosud.ContratoDataTable contratos = adapter.GetDataByIdEmpresa(_empresaID);

        Helpers._Contratos = new Hashtable();

        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
        foreach (DSConosud.ContratoRow dr in contratos)
        {
            string make = (string)dr.Codigo;
            long makeId = dr.IdContrato;
            values.Add(new CascadingDropDownNameValue(
              make, makeId.ToString()));

            Helpers._Contratos.Add(make, dr);
        }
        return values.ToArray();


    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetSubContratistas(string knownCategoryValues, string category)
    {
        string[] Contratosvalues = knownCategoryValues.Split(':', ';');
        long _ContratoID = 0;

        _ContratoID = Convert.ToInt64(Contratosvalues[3]);

        DSConosudTableAdapters.ContratoEmpresasTableAdapter adapter = new DSConosudTableAdapters.ContratoEmpresasTableAdapter();
        DSConosud.ContratoEmpresasDataTable contratoEmpresa = adapter.GetContEmpresaExtend(Convert.ToInt32(_ContratoID));

        Helpers._SubContratistas = new Hashtable();

        string IdContratista = "";
        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

        foreach (DSConosud.ContratoEmpresasRow dr in contratoEmpresa)
        {
            string make = (string)dr["RazonSocial"];
            long makeId = dr.IdContratoEmpresas;
            values.Add(new CascadingDropDownNameValue(
              make, makeId.ToString()));

            Helpers._SubContratistas.Add(make,dr);

            if (bool.Parse(dr["EsContratista"].ToString()))
            {
                IdContratista = dr.IdContratoEmpresas.ToString();
            }
        }

        values.Add(new CascadingDropDownNameValue(
              "Todas", IdContratista));


        return values.ToArray();


    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetPeriodos(string knownCategoryValues, string category)
    {
        string[] Empresasvalues = knownCategoryValues.Split(':', ';');

        int _empresaID = 0;
        _empresaID = Convert.ToInt32(Empresasvalues[5]);


        DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter adapter = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
        DSConosud.CabeceraHojasDeRutaDataTable contratos = adapter.GetDataByPeriodosDispXContEmp(_empresaID);

        Helpers._Cabeceras = new Hashtable();

        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
        string make = "";
        long makeId = 0;
        foreach (DSConosud.CabeceraHojasDeRutaRow dr in contratos)
        {
            if ( (dr.Periodo.Year < DateTime.Now.Year) || ( dr.Periodo.Year == DateTime.Now.Year && dr.Periodo.Month < DateTime.Now.Month) )
            {
                make = string.Format("{0:yyyy/MM}" , dr.Periodo);
                makeId = dr.IdCabeceraHojasDeRuta;

                if (_PeriodoDefaul == make)
                {
                    values.Add(new CascadingDropDownNameValue(
                      make, makeId.ToString(), true));
                }
                else
                {
                    values.Add(new CascadingDropDownNameValue(
                    make, makeId.ToString(), false));
                }

                Helpers._Cabeceras.Add(make, dr);
            }
        }
        

        Helpers._UltimoPeriodo = makeId;
        return values.ToArray();


    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetAllPeriodos(string knownCategoryValues, string category)
    {
        string[] Empresasvalues = knownCategoryValues.Split(':', ';');

        int _empresaID = 0;
        _empresaID = Convert.ToInt32(Empresasvalues[5]);


        DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter adapter = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
        DSConosud.CabeceraHojasDeRutaDataTable contratos = adapter.GetDataByPeriodosDispXContEmp(_empresaID);

        Helpers._Cabeceras = new Hashtable();

        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
        string make = "";
        long makeId = 0;
        foreach (DSConosud.CabeceraHojasDeRutaRow dr in contratos)
        {
                make = string.Format("{0:yyyy/MM}", dr.Periodo);
                makeId = dr.IdCabeceraHojasDeRuta;

                if (_PeriodoDefaul != "" && _PeriodoDefaul == make)
                {
                    values.Add(new CascadingDropDownNameValue(
                      make, makeId.ToString(), true));
                }
                else if (_PeriodoDefaul == "")
                {
                    values.Add(new CascadingDropDownNameValue(
                    make, makeId.ToString(), false));
                }

                Helpers._Cabeceras.Add(make, dr);
         }


        Helpers._UltimoPeriodo = makeId;
        return values.ToArray();


    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetPeriodosSinLegajos(string knownCategoryValues, string category)
    {
        string[] Empresasvalues = knownCategoryValues.Split(':', ';');

        int _empresaID = 0;
        _empresaID = Convert.ToInt32(Empresasvalues[5]);


        DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter adapter = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
        DSConosud.CabeceraHojasDeRutaDataTable contratos = adapter.GetDataByPeriodosSinLegajos(_empresaID);

        Helpers._Cabeceras = new Hashtable();

        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
        string make = "";
        long makeId = 0;

        foreach (DSConosud.CabeceraHojasDeRutaRow dr in contratos)
        {

            if ((dr.Periodo.Year < DateTime.Now.Year) || (dr.Periodo.Year == DateTime.Now.Year && dr.Periodo.Month < DateTime.Now.Month))
            {
                make = string.Format("{0:yyyy/MM}", dr.Periodo);
                makeId = dr.IdCabeceraHojasDeRuta;
                values.Add(new CascadingDropDownNameValue(
                  make, makeId.ToString()));

                Helpers._Cabeceras.Add(make, dr);
            }
        }
        Helpers._UltimoPeriodo = makeId;
        return values.ToArray();


    }
    
    [WebMethod]
    public CascadingDropDownNameValue[] GetPeriodosxContratoEmpresa(string knownCategoryValues, string category)
    {
        string[] Empresasvalues = knownCategoryValues.Split(':', ';');

        int _IdContratoEmpresa = 0;
        _IdContratoEmpresa = Convert.ToInt32(Empresasvalues[5]);


        DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter adapter = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
        DSConosud.CabeceraHojasDeRutaDataTable contratos = adapter.GetDataByIdContratoEmpresa(_IdContratoEmpresa);

        Helpers._Cabeceras = new Hashtable();

        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
        string make = "";
        long makeId = 0;
        foreach (DSConosud.CabeceraHojasDeRutaRow dr in contratos)
        {
            make = string.Format("{0:yyyy/MM}", dr.Periodo);
            makeId = dr.IdCabeceraHojasDeRuta;
            values.Add(new CascadingDropDownNameValue(
              make, makeId.ToString()));

            Helpers._Cabeceras.Add(make, dr);
        }

        Helpers._UltimoPeriodo = makeId;
        return values.ToArray();


    }


}

