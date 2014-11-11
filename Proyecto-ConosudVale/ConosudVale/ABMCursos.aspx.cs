using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;


public partial class ABMCursos : System.Web.UI.Page
{

    private EntidadesConosud _Contexto;

    public EntidadesConosud Contexto
    {
        get
        {

            if (_Contexto == null)
            {
                _Contexto = new EntidadesConosud();
                return _Contexto;
            }
            else
                return _Contexto;
        }
    }

    public bool EsContratista
    {
        get
        {
            if (this.Session["TipoUsuario"].ToString() == "Cliente")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public class Vehiculo
    {
        public string Patente { get; set; }
        public string NroInterno { get; set; }
        public string TipoUnidad { get; set; }
        public string Marca { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        //List<Vehiculo> vahiculos = new List<Vehiculo>();

        //Vehiculo v = new Vehiculo();
        //v.Patente = "Juan Fernandez Miño";
        //v.NroInterno = "Inducción en Seguridad";
        //v.Marca = "10/12/2010";
        //v.TipoUnidad = "SEGPRO";
        //vahiculos.Add(v);

        //v = new Vehiculo();
        //v.Patente = "Martín Fernando Lupez";
        //v.NroInterno = "Manejo Defesivo";
        //v.Marca = "10/07/2008";
        //v.TipoUnidad = "SEGPRO";
        //vahiculos.Add(v);


        //v = new Vehiculo();
        //v.Patente = "Diego Andrés Torrente";
        //v.NroInterno = "Rol de Emergencia";
        //v.Marca = "30/04/1998";
        //v.TipoUnidad = "";
        //vahiculos.Add(v);

        //v = new Vehiculo();
        //v.Patente = "Armando Julio Quiroga";
        //v.NroInterno = "Inducción en Seguridad";
        //v.Marca = "10/12/2010";
        //v.TipoUnidad = "";
        //vahiculos.Add(v);

        //v = new Vehiculo();
        //v.Patente = "Mauricio Malanca";
        //v.NroInterno = "Uso Correcto de Maquinas y Herramientas";
        //v.Marca = "22/06/2010";
        //v.TipoUnidad = "";
        //vahiculos.Add(v);

        //v = new Vehiculo();
        //v.Patente = "Sergio Maya";
        //v.NroInterno = "Uso Correcto de Maquinas y Herramientas";
        //v.Marca = "22/06/2010";
        //v.TipoUnidad = "";
        //vahiculos.Add(v);

        if (!IsPostBack)
        {
            CargarGrilla("");

            cboCursos.DataTextField = "Descripcion";
            cboCursos.DataValueField = "IdClasificacion";
            cboCursos.DataSource = (from c in Contexto.Clasificacion
                                    where c.Tipo == "Cursos"
                                    select c).ToList();

            cboCursos.DataBind();

            cboInstitutos.DataTextField = "Descripcion";
            cboInstitutos.DataValueField = "IdClasificacion";
            cboInstitutos.DataSource = (from c in Contexto.Clasificacion
                                        where c.Tipo == "Instituto Cursos"
                                        select c).ToList();

            cboInstitutos.DataBind();
        }
    }

    protected void cboLegajos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {

        if (this.Session["TipoUsuario"].ToString() == "Cliente")
        {
            long idEmpresaContratista = long.Parse(Session["IdEmpresaContratista"].ToString());
            var LegajosEncontrados = (from l in Contexto.Legajos
                                      where l.Apellido.StartsWith(e.Text)
                                      && l.EmpresaLegajo == idEmpresaContratista
                                      select new
                                      {
                                          Descripcion = l.Apellido + ", " + l.Nombre + " - " + l.NroDoc,
                                          l.IdLegajos
                                      }).Take(15);

            cboLegajos.Items.Clear();
            if (LegajosEncontrados.Count() > 0)
            {
                foreach (var item in LegajosEncontrados)
                {
                    cboLegajos.Items.Add(new RadComboBoxItem(item.Descripcion, item.IdLegajos.ToString()));
                }
            }
            else
            {
                cboLegajos.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
            }

        }
        else
        {

            var LegajosEncontrados = (from l in Contexto.Legajos
                                      where l.Apellido.StartsWith(e.Text)
                                      select new
                                      {
                                          Descripcion = l.Apellido + ", " + l.Nombre + " - " + l.NroDoc,
                                          l.IdLegajos
                                      }).Take(15);

            cboLegajos.Items.Clear();
            if (LegajosEncontrados.Count() > 0)
            {
                foreach (var item in LegajosEncontrados)
                {
                    cboLegajos.Items.Add(new RadComboBoxItem(item.Descripcion, item.IdLegajos.ToString()));
                }
            }
            else
            {
                cboLegajos.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
            }
        }

    }

    public void ConfigureExportAndExport()
    {



    }

    protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
    {
        CargarGrilla(txApellido.Text);
        UpdPnlGrilla.Update();
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        long idCurso = long.Parse(gvVahiculos.SelectedValue.ToString());
        Contexto.DeleteObject((from l in Contexto.CursosLegajos where l.IdCursoLegajo == idCurso select l).FirstOrDefault());
        Contexto.SaveChanges();
        CargarGrilla("");
    }

    private void CargarGrilla(string Apellido)
    {

        if (this.Session["TipoUsuario"].ToString() == "Cliente")
        {
            long idEmp = long.Parse(Session["IdEmpresaContratista"].ToString());
            var LegajosCursos = (from l in Contexto.CursosLegajos
                                 where l.objLegajo.Apellido.StartsWith(Apellido)
                                 && l.objLegajo.objEmpresaLegajo.IdEmpresa == idEmp
                                 select new
                                 {
                                     DesCompletaLegajo = l.objLegajo.Apellido + ", " + l.objLegajo.Nombre + " - " + l.objLegajo.NroDoc,
                                     Legajo = l.objLegajo.Apellido + "," + l.objLegajo.Nombre,
                                     Curso = l.objCurso.Descripcion.Trim(),
                                     Instituto = l.objInstituto.Descripcion,
                                     FechaCurso = l.FechaVencimiento.Value,
                                     FechaVen = l.FechaVencimiento,
                                     Obs = l.Observacion,
                                     l.IdCursoLegajo,
                                     IdCurso = l.objCurso.IdClasificacion,
                                     EsAprobado  = l.Aprobado
                                 }).Take(20);


            gvVahiculos.DataSource = LegajosCursos;
        }
        else
        {
            var LegajosCursos = (from l in Contexto.CursosLegajos
                                 where l.objLegajo.Apellido.StartsWith(Apellido)
                                 select new
                                 {
                                     DesCompletaLegajo = l.objLegajo.Apellido + ", " + l.objLegajo.Nombre + " - " + l.objLegajo.NroDoc,
                                     Legajo = l.objLegajo.Apellido + "," + l.objLegajo.Nombre,
                                     Curso = l.objCurso.Descripcion,
                                     Instituto = l.objInstituto.Descripcion,
                                     FechaCurso = l.FechaVencimiento.Value,
                                     FechaVen = l.FechaVencimiento,
                                     Obs = l.Observacion,
                                     l.IdCursoLegajo,
                                     IdCurso = l.objCurso.IdClasificacion,
                                     EsAprobado = l.Aprobado
                                 }).Take(20);


            gvVahiculos.DataSource = LegajosCursos;
        }

        gvVahiculos.DataBind();

    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument == "ActualizarGrilla")
        {
            CargarGrilla("");
            UpdPnlGrilla.Update();
        }
    }

    [WebMethod(EnableSession = true)]
    public static void Aplicar(string IdLegajo, string IdCurso, string IdInstituto, string FechaCurso, string FechaVen, string Obs, string IdCursoLegajo)
    {
        if (IdCursoLegajo == "-1")
        {
            CursosLegajos newcurso = new CursosLegajos();
            newcurso.Legajo = long.Parse(IdLegajo);
            newcurso.Curso = long.Parse(IdCurso);
            newcurso.InstitutoDictador = long.Parse(IdInstituto);
            newcurso.FechaCurso = DateTime.Parse(FechaCurso);
            newcurso.FechaVencimiento = DateTime.Parse(FechaVen);
            newcurso.Observacion = Obs;
            (HttpContext.Current.Session["Contexto"] as EntidadesConosud).AddToCursosLegajos(newcurso);

        }
        else
        {
            long idCursoLeg = long.Parse(IdCursoLegajo);
            CursosLegajos current = (from c in (HttpContext.Current.Session["Contexto"] as EntidadesConosud).CursosLegajos
                                     where c.IdCursoLegajo == idCursoLeg
                                     select c).First();


            if (current.Curso != long.Parse(IdCurso) ||
                current.InstitutoDictador != long.Parse(IdInstituto) ||
                current.FechaCurso != DateTime.Parse(FechaCurso) ||
                current.FechaVencimiento != DateTime.Parse(FechaVen) ||
                current.Observacion != Obs)
            {
                current.objLegajo.FechaUltmaModificacion = DateTime.Now;
            }


            current.Curso = long.Parse(IdCurso);
            current.InstitutoDictador = long.Parse(IdInstituto);
            current.FechaCurso = DateTime.Parse(FechaCurso);
            current.FechaVencimiento = DateTime.Parse(FechaVen);
            current.Observacion = Obs;
        }

        (HttpContext.Current.Session["Contexto"] as EntidadesConosud).SaveChanges();

    }
}
