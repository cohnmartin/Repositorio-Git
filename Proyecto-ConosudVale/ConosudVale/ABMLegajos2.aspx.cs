using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using Telerik.Web.UI.Upload;

public partial class ABMLegajos2 : System.Web.UI.Page
{
    private EntidadesConosud _Contexto;

    [Serializable]
    public class TempCursos
    {
        public string Curso { get; set; }
        public DateTime FechaCurso { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Dictado { get; set; }
        public long IdCursoLegajo { get; set; }
        public bool Aprobado { get; set; }
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

    public bool PoseePermisoSoloLectura
    { get; set; }

    public EntidadesConosud Contexto
    {
        //get
        //{
        //    if (Session["Contexto"] != null)
        //        return (EntidadesConosud)Session["Contexto"];
        //    else
        //    {
        //        Session["Contexto"] = new EntidadesConosud();
        //        return (EntidadesConosud)Session["Contexto"];
        //    }
        //}

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



    protected void Page_Load(object sender, EventArgs e)
    {
        RadGrid1.DataBound += new EventHandler(RadGrid1_DataBound);
        //gvLegajos.RowCreated += new GridViewRowEventHandler(gvLegajos_RowCreated);

        if (!IsPostBack)
        {
            //Session["Contexto"] = new EntidadesConosud();
            this.HiddenTipoUsuario.Value = this.Session["TipoUsuario"].ToString();

            string DescRol = Helpers.RolesEspeciales.Administrador.ToString();
            long idUsuario = long.Parse(Session["idusu"].ToString());
            int RolesAdministrador = (from U in Contexto.SegUsuario
                                      from UR in U.SegUsuarioRol
                                      where U.IdSegUsuario == idUsuario
                                      && UR.SegRol.Descripcion == DescRol
                                      select UR).Count();
            if (RolesAdministrador > 0)
            {
                this.HiddenEsAdministrador.Value = true.ToString();
            }
            else
            {
                this.HiddenEsAdministrador.Value = false.ToString();
            }


            if (this.Session["TipoUsuario"].ToString() == "Cliente")
            {
                this.PnlCredencial.Visible = false;

                ////Datos de auditoria
                this.lblEstVer.Visible = false;
                this.cboEstadoVerificacion.Visible = false;
                this.lblUltVer.Visible = true;
                this.txtFechaVerificacion.Enabled = false;
                this.lblUltMod.Visible = false;
                this.txtFechaUltimaModificacion.Visible = false;
                this.txtObservacion.ReadOnly = true;

                this.gvRAC.Columns.FindByUniqueName("AprobadoColumn").Visible = false;
                this.HiddenIdEmpresa.Value = ((SegUsuario)this.Session["usuario"]).Empresa.IdEmpresa.ToString();
                this.rowEmpresaLegajo.Style.Add(HtmlTextWriterStyle.Display, "none");
                this.rowContratoLegajo.Style.Add(HtmlTextWriterStyle.Display, "none");
                this.rowPeriodoLegajo.Style.Add(HtmlTextWriterStyle.Display, "none");
                this.HDCredencial.Style.Add(HtmlTextWriterStyle.Display, "none");

            }
            else
            {
                ////Datos de auditoria
                this.lblEstVer.Visible = true;
                this.cboEstadoVerificacion.Visible = true;
                this.lblUltVer.Visible = true;
                this.txtFechaVerificacion.Enabled = true;
                this.lblUltMod.Visible = true;
                this.txtFechaUltimaModificacion.Visible = true;

                this.PnlCredencial.Visible = true;
                this.gvRAC.Columns.FindByUniqueName("AprobadoColumn").Visible = true;

            }

            //CargarSessionLegajos();

            this.RadGrid1.DataSource = FiltrarLegajos("", "", RadGrid1.PageSize);
            this.RadGrid1.DataBind();
            this.UpdPnlGrilla.Update();

            List<Clasificacion> AllClasif = (from c in Contexto.Clasificacion
                                             where c.Tipo == "Estado Civil" ||
                                             c.Tipo == "Estado Verificacion" ||
                                             c.Tipo == "Nacionalidad" ||
                                             c.Tipo == "Provincia" ||
                                             c.Tipo == "Tipo Documento" ||
                                             c.Tipo == "Convenio" ||
                                             c.Tipo == "Compañias Seguro"
                                             select c).ToList();

            cboEstadoCivil.DataSource = (from c in AllClasif where c.Tipo == "Estado Civil" select c).ToList();
            cboEstadoCivil.DataBind();

            cboEstadoVerificacion.DataSource = (from c in AllClasif where c.Tipo == "Estado Verificacion" select c).ToList();
            cboEstadoVerificacion.DataBind();

            cboNacionalidad.DataSource = (from c in AllClasif where c.Tipo == "Nacionalidad" select c).ToList();
            cboNacionalidad.DataBind();

            cboProvincia.DataSource = (from c in AllClasif where c.Tipo == "Provincia" select c).ToList();
            cboProvincia.DataBind();

            cboTipoDoc.DataSource = (from c in AllClasif where c.Tipo == "Tipo Documento" select c).ToList();
            cboTipoDoc.DataBind();

            cboCompañia.DataSource = (from c in AllClasif where c.Tipo == "Compañias Seguro" select c).ToList();
            cboCompañia.DataBind();



            cboConvenio.DataSource = (from c in AllClasif
                                      where c.Tipo == "Convenio"
                                      select new
                                      {

                                          IdClasificacion = c.IdClasificacion,
                                          Descripcion = c.Descripcion.Trim()

                                      }).ToList();
            cboConvenio.DataBind();

            cboEmpresaLegajo.DataSource = Helpers.GetEmpresas(long.Parse(Session["idusu"].ToString())); ; // (from emp in Contexto.Empresa orderby emp.RazonSocial select new { emp.IdEmpresa, emp.RazonSocial }).ToList();
            cboEmpresaLegajo.DataBind();


            chkCompRacs.Attributes.Add("onclick", "ControlCheckRacs(this,'" + chkEstudosBasicos.ClientID + "');");

            //Do not display SelectedFilesCount progress indicator.
            RadProgressArea1.ProgressIndicators &= ~ProgressIndicators.SelectedFilesCount;
        }
        RadProgressArea1.DisplayCancelButton = true;
        RadProgressArea1.Localization.Uploaded = "Total Progreso";
        RadProgressArea1.Localization.UploadedFiles = "Progreso";
        RadProgressArea1.Localization.TransferSpeed = "Velocidad: ";
        RadProgressArea1.Localization.EstimatedTime = "Tiempo estimado: ";
        RadProgressArea1.Localization.ElapsedTime = "Tiempo de enlace: ";
        RadProgressArea1.Localization.Cancel = "Cancelar: ";

        string[] allowedFileExtensions = new string[4] { ".jpg", ".jpeg", ".png", ".gif" };
        RadUpload1.AllowedFileExtensions = allowedFileExtensions;

    }



    public List<dynamic> FiltrarLegajos(string filtroApellido, string FiltroDNI, int take)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        List<ContEmpLegajos> Todos = new List<ContEmpLegajos>();
        List<Legajos> DatosLegajosFiltrados;
        var datosC = (from c in Contexto.SegContextos
                      where c.SegUsuario == idUsuario
                      && c.Contexto == "CONTRATO"
                      select c).ToList();


        var contratosSegContexto = (from c in datosC
                                    join c1 in Contexto.Contrato on long.Parse(c.Valor) equals c1.IdContrato
                                    where c.SegUsuario == idUsuario
                                    select new
                                    {
                                        Empresa = c1.ContratoEmpresas.Where(w => w.EsContratista.Value).FirstOrDefault().Empresa.IdEmpresa,
                                        IdContrato = c1.ContratoEmpresas.Where(w => w.EsContratista.Value).FirstOrDefault().Contrato.IdContrato,
                                    }).ToList();

        List<long> empresasHabilitadas = contratosSegContexto.Select(w => w.Empresa).ToList();
        List<long> contratosHabilitados = contratosSegContexto.Select(w => w.IdContrato).ToList();

        if (this.Session["TipoUsuario"].ToString() == "Cliente")
        {
            long idEmpresa = long.Parse(HiddenIdEmpresa.Value);
            if (filtroApellido.Trim().ToLower() != "")
            {
                if (empresasHabilitadas.Count > 0)
                {
                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             join c in Contexto.ContEmpLegajos on emp.IdLegajos equals c.IdLegajos
                                             where empresasHabilitadas.Contains(emp.EmpresaLegajo.Value) && emp.Apellido.ToLower().StartsWith(filtroApellido.ToLower()) && emp.EmpresaLegajo == idEmpresa
                                             select emp).Distinct().OrderBy(w => w.Apellido).Take(take).ToList();
                }
                else
                {

                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             where emp.Apellido.ToLower().StartsWith(filtroApellido.ToLower()) && emp.EmpresaLegajo == idEmpresa
                                             select emp).OrderBy(w => w.Apellido).Take(take).ToList();
                }

                //DatosLegajosFiltrados = (from emp in Contexto.Legajos
                //                         where emp.Apellido.ToLower().StartsWith(filtroApellido.ToLower()) && emp.EmpresaLegajo == idEmpresa
                //                         orderby emp.Apellido
                //                         select emp).Take(take).ToList();
            }
            else if (FiltroDNI.Trim().ToLower() != "")
            {
                if (empresasHabilitadas.Count > 0)
                {
                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             join c in Contexto.ContEmpLegajos on emp.IdLegajos equals c.IdLegajos
                                             where empresasHabilitadas.Contains(emp.EmpresaLegajo.Value) && emp.NroDoc.StartsWith(FiltroDNI.Trim()) && emp.EmpresaLegajo == idEmpresa
                                             select emp).Distinct().OrderBy(w => w.Apellido).Take(take).ToList();
                }
                else
                {

                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             where emp.NroDoc.StartsWith(FiltroDNI.Trim()) && emp.EmpresaLegajo == idEmpresa
                                             select emp).OrderBy(w => w.Apellido).Take(take).ToList();
                }


                //DatosLegajosFiltrados = (from emp in Contexto.Legajos
                //                         where emp.NroDoc.StartsWith(FiltroDNI.Trim()) && emp.EmpresaLegajo == idEmpresa
                //                         orderby emp.Apellido
                //                         select emp).Take(take).ToList();
            }
            else
            {

                if (empresasHabilitadas.Count > 0)
                {
                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             join c in Contexto.ContEmpLegajos on emp.IdLegajos equals c.IdLegajos
                                             where empresasHabilitadas.Contains(emp.EmpresaLegajo.Value) && emp.EmpresaLegajo == idEmpresa
                                             select emp).Distinct().OrderBy(w => w.Apellido).Take(take).ToList();
                }
                else
                {

                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             where  emp.EmpresaLegajo == idEmpresa
                                             select emp).OrderBy(w => w.Apellido).Take(take).ToList();
                }


                //DatosLegajosFiltrados = (from emp in Contexto.Legajos
                //                         where emp.EmpresaLegajo == idEmpresa
                //                         orderby emp.Apellido
                //                         select emp).Take(take).ToList();



            }


        }
        else
        {

            if (filtroApellido.Trim().ToLower() != "")
            {
                if (empresasHabilitadas.Count > 0)
                {
                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             join c in Contexto.ContEmpLegajos on emp.IdLegajos equals c.IdLegajos
                                             where empresasHabilitadas.Contains(emp.EmpresaLegajo.Value) && emp.Apellido.ToLower().StartsWith(filtroApellido.ToLower())
                                             select emp).Distinct().OrderBy(w => w.Apellido).Take(take).ToList();
                }
                else
                {

                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             where emp.Apellido.ToLower().StartsWith(filtroApellido.ToLower())
                                             select emp).OrderBy(w => w.Apellido).Take(take).ToList();
                }


                //DatosLegajosFiltrados = (from emp in Contexto.Legajos
                //                         where emp.Apellido.ToLower().StartsWith(filtroApellido.ToLower())
                //                         orderby emp.Apellido
                //                         select emp).Take(take).ToList();
            }
            else if (FiltroDNI.Trim().ToLower() != "")
            {
                if (empresasHabilitadas.Count > 0)
                {
                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             join c in Contexto.ContEmpLegajos on emp.IdLegajos equals c.IdLegajos
                                             where empresasHabilitadas.Contains(emp.EmpresaLegajo.Value) && emp.NroDoc.StartsWith(FiltroDNI.Trim())
                                             select emp).Distinct().OrderBy(w => w.Apellido).Take(take).ToList();
                }
                else
                {

                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             where emp.NroDoc.StartsWith(FiltroDNI.Trim())
                                             select emp).OrderBy(w => w.Apellido).Take(take).ToList();
                }


                //DatosLegajosFiltrados = (from emp in Contexto.Legajos
                //                         where emp.NroDoc.StartsWith(FiltroDNI.Trim())
                //                         orderby emp.Apellido
                //                         select emp).Take(take).ToList();
            }
            else
            {

                if (empresasHabilitadas.Count > 0)
                {
                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             join c in Contexto.ContEmpLegajos on emp.IdLegajos equals c.IdLegajos
                                             where empresasHabilitadas.Contains(emp.EmpresaLegajo.Value)
                                             select emp).Distinct().OrderBy(w => w.Apellido).Take(take).ToList();
                }
                else
                {

                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             select emp).OrderBy(w => w.Apellido).Take(take).ToList();
                }

            }

        }



        List<long> idsLegajos = DatosLegajosFiltrados.Select(w => w.IdLegajos).Distinct().ToList();

        var contEmpLegajos = (from e in Contexto.ContEmpLegajos
                              where idsLegajos.Contains(e.IdLegajos.Value) && (contratosHabilitados.Count == 0 || contratosHabilitados.Contains(e.ContratoEmpresas.Contrato.IdContrato))
                              group e by new { e.IdLegajos, e.ContratoEmpresas.IdEmpresa } into g
                              select new
                              {
                                  g.Key,
                                  contratos = g,
                                  cab = g.Select(w => w.CabeceraHojasDeRuta)
                              }).ToList();




        /// Esta lógica es para determinar el contrato actual de cada legajo y si no esta asignado
        /// a ningun contrato se bueca el ultimo en el que estuvo
        foreach (Legajos leg in DatosLegajosFiltrados)
        {
            /// ORIGINAL
            //List<ContEmpLegajos> TotalContratosLegajo = contEmpLegajos.Where(w => w.IdLegajos == leg.IdLegajos && w.ContratoEmpresas.IdEmpresa == leg.objEmpresaLegajo.IdEmpresa).ToList();
            //List<ContEmpLegajos> TotalContratosLegajo = contEmpLegajos.Where(w => w.e.IdLegajos == leg.IdLegajos && w.IdEmpresa == leg.EmpresaLegajo.Value).Select(w => w.e).ToList();

            List<ContEmpLegajos> TotalContratosLegajo = null;
            var ContratosExistentes = contEmpLegajos.Where(w => leg.EmpresaLegajo.HasValue && w.Key.IdLegajos == leg.IdLegajos && w.Key.IdEmpresa == leg.EmpresaLegajo.Value).Select(w => w.contratos).ToList().FirstOrDefault();
            if (ContratosExistentes != null)
            {
                TotalContratosLegajo = ContratosExistentes.ToList();


                ContEmpLegajos contFinal = null;
                ContEmpLegajos Ultimo = TotalContratosLegajo.Where(w => w.FechaTramiteBaja.HasValue).OrderBy(w => w.FechaTramiteBaja).LastOrDefault();

                // Si ultimo es null indica que el legajo esta asociado a un contrato actualmente.
                if (Ultimo == null)
                {
                    var ultimoContrato = TotalContratosLegajo.OrderBy(w => w.CabeceraHojasDeRuta.Periodo).LastOrDefault();
                    contFinal = TotalContratosLegajo.Where(w => w.IdContratoEmpresas == ultimoContrato.IdContratoEmpresas).OrderBy(w => w.CabeceraHojasDeRuta.Periodo).FirstOrDefault();
                }
                else if (Ultimo != null && TotalContratosLegajo.Any(w => !w.FechaTramiteBaja.HasValue && w.CabeceraHojasDeRuta.Periodo >= Ultimo.CabeceraHojasDeRuta.Periodo && Ultimo.IdContEmpLegajos != w.IdContEmpLegajos))
                {
                    /// busco el primer periodo luego de la ultima baja que tuvo en el contrato, es decir que el legajo
                    /// trabajo un periodo, se lo dio de baja y luego se lo volvio a asignar.
                    contFinal = TotalContratosLegajo.Where(w => !w.FechaTramiteBaja.HasValue && w.CabeceraHojasDeRuta.Periodo.Month >= Ultimo.CabeceraHojasDeRuta.Periodo.Month && w.CabeceraHojasDeRuta.Periodo.Year >= Ultimo.CabeceraHojasDeRuta.Periodo.Year && Ultimo.IdContEmpLegajos != w.IdContEmpLegajos).FirstOrDefault();

                }
                else
                {

                    bool encontrado = false;
                    foreach (ContEmpLegajos item in TotalContratosLegajo.OrderBy(w => w.CabeceraHojasDeRuta.Periodo))
                    {
                        if (encontrado)
                        {
                            if (item.CabeceraHojasDeRuta.Periodo.Date >= DateTime.Now.Date)
                                contFinal = item;

                            break;
                        }
                        else if (item.IdContEmpLegajos == Ultimo.IdContEmpLegajos)
                            encontrado = true;
                    }

                }

                Todos.Add(contFinal);
            }

        }



        var datos = (from d in DatosLegajosFiltrados
                     select new
                     {
                         d = d,
                         DesEstudiosBasicos = !d.EstudiosBasicos.HasValue ? "No Apto" : !d.EstudiosBasicos.Value ? "No Apto" : "Apto",
                         DesComplementarioRacs = !d.ComplementarioRacs.HasValue ? "No Apto" : !d.ComplementarioRacs.Value ? "No Apto" : "Apto",
                         DesAdicionalQuimicos = !d.AdicionalQuimicos.HasValue ? "No Apto" : !d.AdicionalQuimicos.Value ? "No Apto" : "Apto",
                         dc = Todos.Where(w => w != null && w.IdLegajos == d.IdLegajos).Select(w => new
                         {
                             w.ContratoEmpresas.Contrato.Codigo,
                             Periodo = string.Format("{0:MM/yyyy}", w.CabeceraHojasDeRuta.Periodo),
                             w.Legajos.IdLegajos,
                             FechaVencimiento = w.ContratoEmpresas.Contrato.Prorroga.HasValue && w.ContratoEmpresas.Contrato.Prorroga.Value > w.ContratoEmpresas.Contrato.FechaVencimiento ? w.ContratoEmpresas.Contrato.Prorroga.Value.ToShortDateString() : w.ContratoEmpresas.Contrato.FechaVencimiento.Value.ToShortDateString(),
                             CategoriaContrato = "Contrato: " + w.ContratoEmpresas.Contrato.objCategoria.Descripcion,
                             Contratista = w.ContratoEmpresas.EsContratista.Value ? w.ContratoEmpresas.Empresa.RazonSocial : w.ContratoEmpresas.Contrato.ContratoEmpresas.Where(c => c.EsContratista.Value).FirstOrDefault().Empresa.RazonSocial,
                             SubContratista = !w.ContratoEmpresas.EsContratista.Value ? w.ContratoEmpresas.Empresa.RazonSocial : "",
                         }).FirstOrDefault()
                     }).ToList();

        return datos.ToList<dynamic>();

    }


    private void BindResults()
    {
        if (RadUpload1.UploadedFiles.Count > 0)
        {
            string nombreArchivo = RadUpload1.UploadedFiles[0].GetName();

            long idLegajo = long.Parse(RadGrid1.SelectedValue.ToString());

            Legajos leg = (from l in Contexto.Legajos
                           where l.IdLegajos == idLegajo
                           select l).FirstOrDefault();

            leg.RutaFoto = nombreArchivo;
            Contexto.SaveChanges();

            this.RadGrid1.DataSource = FiltrarLegajos(txtApellidoLegajo.Text, txtNroDoc.Text, RadGrid1.PageSize);
            this.RadGrid1.DataBind();
            this.UpdPnlGrilla.Update();
        }
    }

    protected void buttonSubmit_Click(object sender, System.EventArgs e)
    {

        UpdateProgressContext();

        BindResults();
    }

    private void UpdateProgressContext()
    {
        const int total = 100;

        RadProgressContext progress = RadProgressContext.Current;
        progress.Speed = "N/A";

        for (int i = 0; i < total; i++)
        {
            progress.PrimaryTotal = 1;
            progress.PrimaryValue = 1;
            progress.PrimaryPercent = 100;

            progress.SecondaryTotal = total;
            progress.SecondaryValue = i;
            progress.SecondaryPercent = i;

            progress.CurrentOperationText = "Step " + i.ToString();

            if (!Response.IsClientConnected)
            {
                //Cancel button was clicked or the browser was closed, so stop processing
                break;
            }

            progress.TimeEstimated = (total - i) * 100;
            //Stall the current thread for 0.1 seconds
            System.Threading.Thread.Sleep(100);
        }
    }

    void RadGrid1_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Legajos, idUsuario);
        //Legajos a; a.objEmpresaLegajo.IdEmpresa

        /// Siempre se muestra el boton de edición pero la seguridad se aplica al boton guardar de la ventana de edición
        /// desde JavaScript

        LinkButton btnAccionI = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
        btnAccionI.Visible = PermisosPagina.Creacion;

        LinkButton btnAccionD = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDelete");
        btnAccionD.Visible = PermisosPagina.Creacion;

        /////Por regla de negocio el cliente no puede eliminar legajos
        if (this.Session["TipoUsuario"].ToString() == "Cliente")
        {
            btnAccionD.Visible = false;
            ((LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnCargarForo")).Visible = false;
            ((LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnSueldos")).Visible = false;
            ((System.Web.UI.HtmlControls.HtmlGenericControl)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblEditar")).InnerText = "Ver Datos";
            PoseePermisoSoloLectura = false;
        }
        else if (this.Session["TipoUsuario"].ToString() != "Cliente" && (!PermisosPagina.Creacion && !PermisosPagina.Modificacion && PermisosPagina.Lectura))
        {
            btnAccionD.Visible = false;
            ((LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnCargarForo")).Visible = false;
            ((LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnSueldos")).Visible = false;
            ((LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ExportExcel")).Visible = false;
            ((System.Web.UI.HtmlControls.HtmlGenericControl)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblEditar")).InnerText = "Ver Datos";
            PoseePermisoSoloLectura = true;
        }
        else if (this.Session["TipoUsuario"].ToString() != "Cliente" && (!PermisosPagina.Creacion && !PermisosPagina.Modificacion && !PermisosPagina.Lectura))
        {
            btnAccionD.Visible = false;
            ((LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnCargarForo")).Visible = false;
            ((LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnSueldos")).Visible = false;
            ((LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("ExportExcel")).Visible = false;
            ((LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEdit")).Visible = false;
            PoseePermisoSoloLectura = true;
        }
        else
        {
            PoseePermisoSoloLectura = false;
        }
    }

    public void ConfigureExportAndExport()
    {
        /// Oculto las columnas que no tiene un valor en la propiedad HeaderText
        foreach (Telerik.Web.UI.GridColumn column in RadGrid1.MasterTableView.Columns)
        {
            if ((!column.Visible || !column.Display) && column.HeaderText != "")
            {
                if (this.Session["TipoUsuario"].ToString() == "Cliente" && column.UniqueName == "ObservacionBloqueoColumn")
                {
                    column.Visible = false;
                    column.Display = false;
                }
                else
                {
                    column.Visible = true;
                    column.Display = true;
                }
            }
            else if (column.HeaderText == "")
            {
                column.Visible = false;
                column.Display = false;
            }
        }

        RadGrid1.ExportSettings.ExportOnlyData = true;
        RadGrid1.ExportSettings.IgnorePaging = true;
        RadGrid1.ExportSettings.FileName = "Legajos";
        RadGrid1.MasterTableView.ExportToExcel();



    }

    void gvLegajos_RowCreated(object sender, GridViewRowEventArgs e)
    {
        /*Create header row above generated header row*/
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //create row    
            GridViewRow row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);


            Image logo = new Image();
            string ruta = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("ABMLegajos2.aspx") - 1);
            logo.ImageUrl = ruta + "/images/LogoReportes.png"; ;
            logo.Width = Unit.Pixel(142);
            logo.Height = Unit.Pixel(92);




            TableCell left = new TableHeaderCell();
            left.ColumnSpan = 2;
            left.RowSpan = 4;
            left.Controls.Add(logo);
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "White");
            left.Style.Add(HtmlTextWriterStyle.PaddingTop, "10px");
            row.Cells.Add(left);

            //spanned cell that will span the columns I don't want to give the additional header 
            left = new TableHeaderCell();
            left.ColumnSpan = 6;
            left.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C5BE97");
            left.Style.Add(HtmlTextWriterStyle.Color, "Black");
            left.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
            left.Style.Add(HtmlTextWriterStyle.FontFamily, "Calibri");
            left.Style.Add(HtmlTextWriterStyle.FontSize, "11");
            left.Text = "Base de Datos de Legajos";
            row.Cells.Add(left);

            //Add the new row to the gridview as the master header row
            //A table is the only Control (index[0]) in a GridView
            ((Table)(sender as GridView).Controls[0]).Rows.AddAt(0, row);


            //create row    
            row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            //spanned cell that will span the columns I don't want to give the additional header 
            left = new TableHeaderCell();
            left.ColumnSpan = 6;
            left.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C5BE97");
            left.Style.Add(HtmlTextWriterStyle.Color, "Black");
            left.Style.Add(HtmlTextWriterStyle.FontFamily, "Calibri");
            left.Style.Add(HtmlTextWriterStyle.FontSize, "11");
            left.Style.Add(HtmlTextWriterStyle.FontWeight, "Normal");
            left.Text = "Fecha y Hora Emisi&oacute;n:" + DateTime.Now;
            row.Cells.Add(left);

            //Add the new row to the gridview as the master header row
            //A table is the only Control (index[0]) in a GridView
            ((Table)(sender as GridView).Controls[0]).Rows.AddAt(1, row);

            //create row    
            row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            //spanned cell that will span the columns I don't want to give the additional header 
            left = new TableHeaderCell();
            left.ColumnSpan = 6;
            left.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C5BE97");
            left.Style.Add(HtmlTextWriterStyle.Color, "Black");
            left.Style.Add(HtmlTextWriterStyle.FontFamily, "Calibri");
            left.Style.Add(HtmlTextWriterStyle.FontSize, "11");
            left.Style.Add(HtmlTextWriterStyle.FontWeight, "Normal");
            left.Text = "Incluye a todo el personal cargado en el SCS, se encuentre afectado/no afectado, habilitado/no habilitado.";
            row.Cells.Add(left);

            //Add the new row to the gridview as the master header row
            //A table is the only Control (index[0]) in a GridView
            ((Table)(sender as GridView).Controls[0]).Rows.AddAt(2, row);

            //create row    
            row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            //spanned cell that will span the columns I don't want to give the additional header 
            left = new TableHeaderCell();
            left.ColumnSpan = 6;
            left.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C5BE97");
            left.Style.Add(HtmlTextWriterStyle.Color, "Black");
            left.Style.Add(HtmlTextWriterStyle.FontFamily, "Calibri");
            left.Style.Add(HtmlTextWriterStyle.FontSize, "11");
            left.Style.Add(HtmlTextWriterStyle.FontWeight, "Normal");
            left.Text = "";
            row.Cells.Add(left);

            //Add the new row to the gridview as the master header row
            //A table is the only Control (index[0]) in a GridView
            ((Table)(sender as GridView).Controls[0]).Rows.AddAt(3, row);


            //create row    
            row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            //spanned cell that will span the columns I don't want to give the additional header 
            left = new TableHeaderCell();
            left.ColumnSpan = 8;
            left.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "White");
            left.Text = "&nbsp;";
            row.Cells.Add(left);

            //Add the new row to the gridview as the master header row
            //A table is the only Control (index[0]) in a GridView
            ((Table)(sender as GridView).Controls[0]).Rows.AddAt(4, row);

            /*fin*/


            e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C5BE97");
            e.Row.Style.Add(HtmlTextWriterStyle.Color, "Black");
            e.Row.Style.Add(HtmlTextWriterStyle.FontFamily, "Calibri");
            e.Row.Style.Add(HtmlTextWriterStyle.FontSize, "11");
            e.Row.Style.Add(HtmlTextWriterStyle.FontWeight, "Normal");

        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Style.Add(HtmlTextWriterStyle.Color, "Black");
            e.Row.Height = Unit.Pixel(18);
        }

    }

    public void btnExportar_click(object sender, EventArgs e)
    {
        List<dynamic> datos = FiltrarLegajos("", "", 10000);
        DateTime fechaVencimientoBaja = DateTime.Parse("01/01/1910");
        var datosExportar = (from d in datos
                             select new
                             {

                                 Apellido = d.d.Apellido.ToUpper(),
                                 Nombre = d.d.Nombre.ToUpper(),
                                 //TipoDocumento = d.d.objTipoDocumento != null ? d.d.objTipoDocumento.Descripcion : "",
                                 d.d.NroDoc,//" HeaderText="NroDoc" 
                                 d.d.CUIL,//" HeaderText="CUIL" 
                                 //fechaVencimientoCalculada = (d.dc != null && d.dc.FechaVencimiento != null) ? d.dc.FechaVencimiento : fechaVencimientoBaja,
                                 CredVencimiento = d.d.CredVencimiento == null ? "NO" : (DateTime.Now < d.d.CredVencimiento && d.d.CredVencimiento <= (d.dc != null && d.dc.FechaVencimiento != null ? DateTime.Parse(d.dc.FechaVencimiento) : fechaVencimientoBaja)) ? "SÍ" : "NO",
                                 Contratista = d.dc == null ? "" : d.dc.Contratista,//"  HeaderText="Contratista" 
                                 SubContratista = d.dc == null ? "" : d.dc.SubContratista,//" HeaderText="Sub Contratista" 
                                 Codigo = d.dc == null ? "" : d.dc.Codigo,//"  HeaderText="Contrato"
                                 FechaVencimientoContrato = d.dc == null ? "" : d.dc.FechaVencimiento,//"  HeaderText="Vencimiento Contrato" >
                                 CategoriaContrato = d.dc == null ? "" : d.dc.CategoriaContrato, //"  HeaderText="Categoria" 
                                 Periodo = d.dc == null ? "" : d.dc.Periodo,//" HeaderText="Periodo" 
                                 DesEstudiosBasicos = d.DesAdicionalQuimicos,  //  HeaderText="Estudios Básicos"
                                 DesComplementarioRacs = d.DesComplementarioRacs, //  HeaderText="Comp. Racs"
                                 DesAdicionalQuimicos = d.DesAdicionalQuimicos,//   HeaderText="Adicional Químicos" 
                                 d.d.FechaUltimoExamen, //" HeaderText="Fecha Ult. Exámen" 
                                 objConvenio = d.d.objConvenio != null ? d.d.objConvenio.Descripcion : "",//HeaderText="Convenio" 

                                 d.d.FechaUltimaVerificacion,//"   HeaderText="Fecha Verificación"
                                 d.d.FechaUltmaModificacion,//  HeaderText="Fecha Ultima Modificación" 
                                 d.d.ObservacionBloqueo,//" HeaderText="Obs. Bloqueo" 
                                 d.d.Direccion,//" HeaderText="Dirección"
                                 d.d.CodigoPostal,//" HeaderText="Codigo Postal" 
                                 d.d.TelefonoFijo,//" HeaderText="Telefono Fijo"
                                 d.d.CorreoElectronico,//" HeaderText="Correo Electrónico"
                                 d.d.FechaNacimiento,//"   HeaderText="Fecha Nacimiento"
                                 objEstadoCivil = d.d.objEstadoCivil != null ? d.d.objEstadoCivil.Descripcion : "",//HeaderText="Estado Civil" 
                                 objNacionalidad = d.d.objNacionalidad != null ? d.d.objNacionalidad.Descripcion : "",//  HeaderText="Nacionalidad" 
                                 objProvincia = d.d.objProvincia != null ? d.d.objProvincia.Descripcion : "",// HeaderText="Provincia" 
                                 objEstadoVerificacion = d.d.objEstadoVerificacion != null ? d.d.objEstadoVerificacion.Descripcion : "",//HeaderText="Estado Verificacion" 
                                 d.d.Observacion, //"  HeaderText="Observacion Auditoria"
                                 d.d.FechaIngreos, //" HeaderText="Ingreso" 
                                 d.d.GrupoSangre, //" HeaderText="Grupo Sangre" 
                                 d.d.Funcion, //"  HeaderText="Función"
                                 d.d.NroPoliza,//" HeaderText="Nro Poliza" 
                                 objCompañiaSeguro = d.d.objCompañiaSeguro != null ? d.d.objCompañiaSeguro.Descripcion : "",//HeaderText="Compañia Seguro" 
                                 d.d.FechaInicial,//" HeaderText="Inicio Seguro" 
                                 d.d.FechaUltimoPago,//"  HeaderText="Ultimo Pago Seg." 
                                 d.d.FechaVencimiento,//"  HeaderText="Vencimiento Seguro" 
                                 d.d.Descripcion,//" HeaderText="Descripción Seguro" 


                             }).OrderBy(w => w.Contratista).ToList();



        List<string> camposExcluir = new List<string>(); ;
        Dictionary<string, string> alias = new Dictionary<string, string>() {
        { "TipoDocumento", "Tipo Documento" } ,
        { "FechaNacimiento", "Fecha Nacimiento" } ,
        { "FechaUltimaVerificacion", "Fecha Verificación"} ,
        { "FechaUltmaModificacion", "Fecha Ultima Modificación" } ,
        { "CredVencimiento", "Hab. Cred." } ,
        { "ObservacionBloqueo", "Obs. Bloqueo" } ,
        { "Direccion", "Dirección"} ,
        { "CodigoPostal", "Codigo Postal" } ,
        { "TelefonoFijo", "Telefono Fijo"} ,
        { "CorreoElectronico", "Correo Electrónico"} ,
        { "objEstadoCivil", "Estado Civil" } ,
        { "objNacionalidad", "Nacionalidad" } ,
        { "objProvincia", "Provincia" } ,
        { "objEstadoVerificacion", "Estado Verificacion" } ,
        { "Observacion", "Observacion Auditoria"} ,
        { "objConvenio", "Convenio" } ,
        { "FechaIngreos", "Ingreso" } ,
        { "GrupoSangre", "Grupo Sangre"} , 
        { "Funcion", "Función"} ,
        { "DesEstudiosBasicos", "Estudios Básicos"} ,
        { "DesComplementarioRacs", "Comp. Racs"} ,
        { "DesAdicionalQuimicos", "Adicional Químicos" } ,
        { "FechaUltimoExamen", "Fecha Ult. Exámen" } ,
        { "Contratista", "Contratista"  } ,
        { "SubContratista", "Sub Contratista"  } ,
        { "NroPoliza", "Nro Poliza"  } ,
        { "objCompañiaSeguro", "Compañia Seguro"  } ,
        { "FechaInicial,", "Inicio Seguro"  } ,
        { "FechaUltimoPago", "Ultimo Pago Seg." } , 
        { "FechaVencimiento", "Vencimiento Seguro"  } ,
        { "Descripcion", "Descripción Seguro"  } ,
        { "Codigo", "Contrato" } ,
        { "FechaVencimientoContrato", "Vencimiento Contrato"  } ,
        { "Periodo", "Periodo"  } ,
        { "CategoriaContrato", "Categoria"  }         };

        List<string> DatosReporte = new List<string>();
        DatosReporte.Add("Base de Datos de Legajos");
        DatosReporte.Add("Fecha y Hora emisión:" + DateTime.Now.ToString());
        DatosReporte.Add("Incluye a todo el personal cargado en el SCS, se encuentre afectado/no afectado, habilitado/no habilitado");


        GridView gv = Helpers.GenerarExportExcel(datosExportar.ToList<dynamic>(), alias, camposExcluir, DatosReporte);

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gv.RenderControl(htmlWrite);

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Legajos" + "_" + DateTime.Now.ToString("M_dd_yyyy_H_M_s") + ".xls");
        HttpContext.Current.Response.ContentType = "application/xls";
        HttpContext.Current.Response.Write(stringWrite.ToString());
        HttpContext.Current.Response.End();

    }

    protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "ExportLegajos")
        {
            btnExportar_click(null, null);
        }
    }

    protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
    {
        this.RadGrid1.DataSource = FiltrarLegajos(txtApellidoLegajo.Text, txtNroDoc.Text, RadGrid1.PageSize);
        this.RadGrid1.DataBind();
        this.UpdPnlGrilla.Update();
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            long id = long.Parse(RadGrid1.SelectedValue.ToString());
            Entidades.Legajos LegEliminar = (from L in Contexto.Legajos
                                             where L.IdLegajos == id
                                             select L).FirstOrDefault<Entidades.Legajos>();

            Contexto.DeleteObject(LegEliminar);
            Contexto.SaveChanges();
            //CargarSessionLegajos();
            this.RadGrid1.DataSource = FiltrarLegajos(txtApellidoLegajo.Text, txtNroDoc.Text, RadGrid1.PageSize);
            this.RadGrid1.DataBind();
            this.UpdPnlGrilla.Update();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "eliminacino", "alert('El legajo no puede ser eliminado ya que es parte de algún contrato.');", true);
        }

    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        /// IMPORTANTE 1: Si el legajo que se esta editando tiene cargado los valores para contrato y periodo y antes (guardados) no los tenia indica que se esta
        /// asignado el legajo a un contrato, entonces se tiene que ejecutar la misma funcionalidad que esta en la pantalla de AsignacionLegajosContratos
        /// para el caso de asignación.
        /// Si el legajo se esta creado y posee la información de contrato y periodo entonces se tiene que ejecutar la misma funcionalidad que esta en la pantalla de AsignacionLegajosContratos
        /// para el caso de asignación.


        ///IMPORTANTE 2: Si es administrador puede asignar y desasiganar en el mesa actual y hacia atras.
        ///Si no es administrador puede desasignar en el mes actual y hacia atras, 
        ///PERO NO puede asignar hacia atras solo en el mes actual.

        /// Ejmplo para determinar si es administrador el usuario logeado:
        /// EntidadesConosud db = new EntidadesConosud();
        //string DescRol = Helpers.RolesEspeciales.Administrador.ToString();
        //int RolesAdministrador = (from U in db.SegUsuario
        //                          from UR in U.SegUsuarioRol
        //                          where U.IdSegUsuario == idUsuario
        //                          && UR.SegRol.Descripcion == DescRol
        //                          select UR).Count();

        if (e.Argument == "Edit" && RadGrid1.SelectedValue != null)
        {
            long IdLegajo = long.Parse(RadGrid1.SelectedValue.ToString());

            List<TempCursos> cursosLegajos = (from c in Contexto.CursosLegajos
                                              where c.Legajo == IdLegajo
                                              select new TempCursos
                                              {
                                                  IdCursoLegajo = c.IdCursoLegajo,
                                                  Curso = c.objCurso.Descripcion,
                                                  FechaCurso = c.FechaCurso.Value,
                                                  FechaVencimiento = c.FechaVencimiento.Value,
                                                  Dictado = c.objInstituto.Descripcion,
                                                  Aprobado = c.Aprobado
                                              }).ToList();

            this.gvRAC.DataSource = cursosLegajos;
            this.gvRAC.DataBind();
            this.UpdPnlCursos.Update();
        }
        else if (e.Argument == "Update")
        {
            #region Actualizacion Legajo

            using (EntidadesConosud dc = new EntidadesConosud())
            {

                long id = long.Parse(RadGrid1.SelectedValue.ToString());
                bool ExisteModificacion = false;
                string CodigoContratoOriginal = "";

                if (RadGrid1.Items[RadGrid1.SelectedItems[0].DataSetIndex].GetDataKeyValue("dc.Codigo") != null)
                    CodigoContratoOriginal = RadGrid1.Items[RadGrid1.SelectedItems[0].DataSetIndex].GetDataKeyValue("dc.Codigo").ToString();


                Entidades.Legajos LegUpdate = (from L in dc.Legajos
                                               where L.IdLegajos == id
                                               select L).FirstOrDefault<Entidades.Legajos>();

                int cant = (from l in dc.Legajos
                            where l.NroDoc == txtNroDocEdit.Text.Trim()
                            && l.IdLegajos != id
                            select l).Count();

                if (cant > 0)
                {
                    txtNroDocEdit.Attributes.Add("NroExistente", true.ToString());
                    upNroDoc.Update();
                    return;
                }
                else
                {
                    txtNroDocEdit.Attributes.Add("NroExistente", false.ToString());
                    upNroDoc.Update();
                }

                ///Actualizo los cursos
                foreach (GridDataItem gditem in this.gvRAC.Items)
                {
                    //long IdCursoLegajo = long.Parse(this.gvRAC.Items[gditem.DataSetIndex].GetDataKeyValue("IdCursoLegajo").ToString());

                    if (((TextBox)gditem.FindControl("hiddenId")).Text != "")
                    {
                        long IdCursoLegajo = long.Parse(((TextBox)gditem.FindControl("hiddenId")).Text);
                        CheckBox chk = (CheckBox)gditem.FindControl("chkAprobado");

                        CursosLegajos curleg = LegUpdate.CursosLegajos.First(c => c.IdCursoLegajo == IdCursoLegajo);
                        if (curleg.Aprobado != chk.Checked)
                        {
                            ExisteModificacion = true;
                        }
                        curleg.Aprobado = chk.Checked;
                    }
                }

                if (LegUpdate != null)
                {
                    if (LegUpdate.Apellido.Trim() != txtApellido.Text.Trim() ||
                        LegUpdate.Nombre.Trim() != txtNombre.Text.Trim() ||
                        LegUpdate.NroDoc.Trim() != txtNroDocEdit.Text.Trim() ||
                        LegUpdate.Direccion.Trim() != txtDireccion.Text.Trim() ||
                        LegUpdate.CodigoPostal.Trim() != txtCodigoPostal.Text.Trim() ||
                        LegUpdate.TelefonoFijo.Trim() != txtTelFijo.Text.Trim() ||
                        LegUpdate.CorreoElectronico.Trim() != txtEmail.Text.Trim() ||
                        LegUpdate.CUIL.Trim() != txtCUIL.Text.Trim() ||
                        LegUpdate.FechaNacimiento != txtFechaNacimiento.SelectedDate ||
                        LegUpdate.FechaIngreos != txtFechaIngreso.SelectedDate ||
                        (LegUpdate.Observacion != null && LegUpdate.Observacion != txtObservacion.Text.Trim()) ||
                        (LegUpdate.ObservacionBloqueo != null && LegUpdate.ObservacionBloqueo != txtObsBloqueo.Text.Trim()) ||
                        (LegUpdate.GrupoSangre != null && LegUpdate.GrupoSangre != txtGrupoSangre.Text.Trim()) ||
                        LegUpdate.AutorizadoCond != chkAutorizadoConducir.Checked ||
                        LegUpdate.EstudiosBasicos != chkEstudosBasicos.Checked ||
                        LegUpdate.ComplementarioRacs != chkCompRacs.Checked ||
                        LegUpdate.AdicionalQuimicos != chkAdicionalQuimicos.Checked ||
                        LegUpdate.NroPoliza != txtNroPoliza.Text ||
                        (LegUpdate.Funcion != null && LegUpdate.Funcion != txtFuncion.Text.Trim()))
                    {
                        ExisteModificacion = true;
                    }

                    /// Controles Tipo TextBox
                    LegUpdate.Apellido = txtApellido.Text.Trim();
                    LegUpdate.Nombre = txtNombre.Text.Trim();
                    LegUpdate.NroDoc = txtNroDocEdit.Text.Trim();
                    LegUpdate.Direccion = txtDireccion.Text.Trim();
                    LegUpdate.CodigoPostal = txtCodigoPostal.Text.Trim();
                    LegUpdate.TelefonoFijo = txtTelFijo.Text.Trim();
                    LegUpdate.CorreoElectronico = txtEmail.Text.Trim();

                    /// Controles Tipo Telerik
                    LegUpdate.CUIL = txtCUIL.Text.Trim();

                    /// Controles Tipo Fecha
                    LegUpdate.FechaNacimiento = txtFechaNacimiento.SelectedDate;
                    LegUpdate.FechaIngreos = txtFechaIngreso.SelectedDate;

                    /// Controles Tipo Combos
                    long idCombo = 0;

                    //if (cboEstadoVerificacion.SelectedItem != null)
                    //{
                    //    idCombo = long.Parse(cboEstadoVerificacion.SelectedValue);
                    //    if (txtFechaVerificacion.SelectedDate != null)
                    //    {
                    //        if (LegUpdate.FechaUltimaVerificacion != txtFechaVerificacion.SelectedDate.Value)
                    //        {
                    //            LegUpdate.FechaUltimaVerificacion = txtFechaVerificacion.SelectedDate.Value;
                    //        }
                    //    }
                    //    else if (LegUpdate.FechaUltimaVerificacion == null && (LegUpdate.objEstadoVerificacion == null || (LegUpdate.objEstadoVerificacion != null && LegUpdate.objEstadoVerificacion.IdClasificacion != idCombo)))
                    //    {
                    //        LegUpdate.FechaUltimaVerificacion = DateTime.Now;
                    //    }

                    //    LegUpdate.objEstadoVerificacion = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                    //}

                    /// La fecha FechaUltimaVerificacion solo se debe actualizar si existe algun cambio en la
                    /// observación de la verificacion, caso contrario se deja como esta. (Martin : 01/04/2014)
                    if (LegUpdate.Observacion != txtObservacion.Text)
                        LegUpdate.FechaUltimaVerificacion = DateTime.Now;

                    /// El estado de verificación se deja de utilizar por el momento. (Martin : 01/04/2014)
                    LegUpdate.objEstadoVerificacion = null;


                    if (cboTipoDoc.SelectedItem != null)
                    {
                        idCombo = long.Parse(cboTipoDoc.SelectedValue);
                        if ((LegUpdate.objTipoDocumento != null && LegUpdate.objTipoDocumento.IdClasificacion != idCombo) || LegUpdate.objTipoDocumento == null)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.objTipoDocumento = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                    }

                    if (cboEstadoCivil.SelectedItem != null)
                    {
                        idCombo = long.Parse(cboEstadoCivil.SelectedValue);
                        if ((LegUpdate.objEstadoCivil != null && LegUpdate.objEstadoCivil.IdClasificacion != idCombo) || LegUpdate.objEstadoCivil == null)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.objEstadoCivil = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                    }

                    if (cboNacionalidad.SelectedItem != null)
                    {
                        idCombo = long.Parse(cboNacionalidad.SelectedValue);
                        if ((LegUpdate.objNacionalidad != null && LegUpdate.objNacionalidad.IdClasificacion != idCombo) || LegUpdate.objNacionalidad == null)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.objNacionalidad = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                    }

                    if (cboConvenio.SelectedItem != null)
                    {
                        idCombo = long.Parse(cboConvenio.SelectedValue);
                        if ((LegUpdate.objConvenio != null && LegUpdate.objConvenio.IdClasificacion != idCombo) || LegUpdate.objConvenio == null)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.objConvenio = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                    }

                    if (cboProvincia.SelectedItem != null)
                    {
                        idCombo = long.Parse(cboProvincia.SelectedValue);
                        if ((LegUpdate.objProvincia != null && LegUpdate.objProvincia.IdClasificacion != idCombo) || LegUpdate.objProvincia == null)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.objProvincia = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                    }

                    if (cboEmpresaLegajo.SelectedItem != null)
                    {
                        idCombo = long.Parse(cboEmpresaLegajo.SelectedValue);
                        if ((LegUpdate.objEmpresaLegajo != null && LegUpdate.objEmpresaLegajo.IdEmpresa != idCombo) || LegUpdate.objEmpresaLegajo == null)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.objEmpresaLegajo = dc.Empresa.Where(w => w.IdEmpresa == idCombo).FirstOrDefault();
                    }

                    LegUpdate.Observacion = txtObservacion.Text;
                    LegUpdate.ObservacionBloqueo = txtObsBloqueo.Text;
                    LegUpdate.GrupoSangre = txtGrupoSangre.Text;
                    LegUpdate.AutorizadoCond = chkAutorizadoConducir.Checked;
                    LegUpdate.EstudiosBasicos = chkEstudosBasicos.Checked;
                    LegUpdate.ComplementarioRacs = chkCompRacs.Checked;
                    LegUpdate.AdicionalQuimicos = chkAdicionalQuimicos.Checked;


                    LegUpdate.Funcion = txtFuncion.Text;

                    if (txtFechaUltExa.SelectedDate != null)
                    {
                        if (LegUpdate.FechaUltimoExamen != txtFechaUltExa.SelectedDate.Value)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.FechaUltimoExamen = txtFechaUltExa.SelectedDate.Value;
                    }
                    else
                    {
                        LegUpdate.FechaUltimoExamen = null;
                    }

                    if (txtFechaVenCredencial.SelectedDate != null)
                    {
                        if (LegUpdate.CredVencimiento != txtFechaVenCredencial.SelectedDate.Value)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.CredVencimiento = txtFechaVenCredencial.SelectedDate.Value;
                    }
                    else
                    {
                        LegUpdate.CredVencimiento = null;
                    }


                    ////////// Datos Seguro
                    LegUpdate.NroPoliza = txtNroPoliza.Text;
                    LegUpdate.Descripcion = txtDescripcion.Text;

                    if (cboCompañia.SelectedItem != null)
                    {
                        idCombo = long.Parse(cboCompañia.SelectedValue);
                        if ((LegUpdate.objCompañiaSeguro != null && LegUpdate.objCompañiaSeguro.IdClasificacion != idCombo) || LegUpdate.objCompañiaSeguro == null)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.objCompañiaSeguro = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                    }


                    if (txtFechaInicio.SelectedDate != null)
                    {
                        if (LegUpdate.FechaInicial != txtFechaInicio.SelectedDate.Value)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.FechaInicial = txtFechaInicio.SelectedDate.Value;
                    }
                    else
                        LegUpdate.FechaInicial = null;



                    if (txtFechaUltPago.SelectedDate != null)
                    {
                        if (LegUpdate.FechaUltimoPago != txtFechaUltPago.SelectedDate.Value)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.FechaUltimoPago = txtFechaUltPago.SelectedDate.Value;
                    }
                    else
                        LegUpdate.FechaUltimoPago = null;



                    if (txtFechaVenicimiento.SelectedDate != null)
                    {
                        if (LegUpdate.FechaVencimiento != txtFechaVenicimiento.SelectedDate.Value)
                        {
                            ExisteModificacion = true;
                        }
                        LegUpdate.FechaVencimiento = txtFechaVenicimiento.SelectedDate.Value;
                    }
                    else
                        LegUpdate.FechaVencimiento = null;
                    ///////////////////////////////////////////


                    /////////////////////////////////////////
                    ////PROCESO DE ASIGNACION A CONTRATO DEL LEGAJO
                    if (cboContratoLegajo.SelectedValue != "" && cboContratoLegajo.SelectedValue != "-1" && CodigoContratoOriginal != cboContratoLegajo.Text)
                    {
                        GenerarAsignacionLegajoContratos(dc, LegUpdate, LegUpdate.CredVencimiento);
                    }

                    if (ExisteModificacion)
                    {
                        LegUpdate.FechaUltmaModificacion = DateTime.Now;
                    }

                    dc.SaveChanges();
                }
            }

            //CargarSessionLegajos();
            this.RadGrid1.DataSource = FiltrarLegajos(txtApellidoLegajo.Text, txtNroDoc.Text, RadGrid1.PageSize);
            this.RadGrid1.DataBind();
            this.UpdPnlGrilla.Update();

            #endregion
        }
        else if (e.Argument == "Insert")
        {
            #region Nuevo Legajo
            using (EntidadesConosud dc = new EntidadesConosud())
            {

                Entidades.Legajos LegInsert = new Entidades.Legajos();

                int cant = (from l in dc.Legajos
                            where l.NroDoc == txtNroDocEdit.Text.Trim()
                            select l).Count();


                if (cant > 0)
                {
                    txtNroDocEdit.Attributes.Add("NroExistente", true.ToString());
                    upNroDoc.Update();
                    return;
                }
                else
                {
                    txtNroDocEdit.Attributes.Add("NroExistente", false.ToString());
                    upNroDoc.Update();
                }


                /// Controles Tipo TextBox
                LegInsert.Apellido = txtApellido.Text.Trim();
                LegInsert.Nombre = txtNombre.Text.Trim();
                LegInsert.NroDoc = txtNroDocEdit.Text.Trim();
                LegInsert.Direccion = txtDireccion.Text.Trim();
                LegInsert.CodigoPostal = txtCodigoPostal.Text.Trim();
                LegInsert.TelefonoFijo = txtTelFijo.Text.Trim();
                LegInsert.CorreoElectronico = txtEmail.Text.Trim();

                /// Controles Tipo Telerik
                LegInsert.CUIL = txtCUIL.Text.Trim();
                if (LegInsert.CUIL == "")
                {
                    LegInsert.CUIL = "1111111111";
                }

                /// Controles Tipo Fecha
                LegInsert.FechaNacimiento = txtFechaNacimiento.SelectedDate;
                LegInsert.FechaIngreos = txtFechaIngreso.SelectedDate;

                /// Controles Tipo Combos
                long idCombo = 0;

                //if (cboEstadoVerificacion.SelectedItem != null)
                //{
                //    idCombo = long.Parse(cboEstadoVerificacion.SelectedValue);
                //    LegInsert.objEstadoVerificacion = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                //}

                /// El estado de verificación se deja de utilizar por el momento. (Martin : 01/04/2014)
                LegInsert.objEstadoVerificacion = null;


                if (cboTipoDoc.SelectedItem != null)
                {
                    idCombo = long.Parse(cboTipoDoc.SelectedValue);
                    LegInsert.objTipoDocumento = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }

                if (cboEstadoCivil.SelectedItem != null)
                {
                    idCombo = long.Parse(cboEstadoCivil.SelectedValue);
                    LegInsert.objEstadoCivil = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }

                if (cboNacionalidad.SelectedItem != null)
                {
                    idCombo = long.Parse(cboNacionalidad.SelectedValue);
                    LegInsert.objNacionalidad = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }

                if (cboConvenio.SelectedItem != null)
                {
                    idCombo = long.Parse(cboConvenio.SelectedValue);
                    LegInsert.objConvenio = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }

                if (cboProvincia.SelectedItem != null)
                {
                    idCombo = long.Parse(cboProvincia.SelectedValue);
                    LegInsert.objProvincia = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }

                if (this.Session["TipoUsuario"].ToString() == "Cliente")
                {
                    long idEmpresa = long.Parse(this.Session["IdEmpresaContratista"].ToString());
                    LegInsert.objEmpresaLegajo = dc.Empresa.Where(w => w.IdEmpresa == idEmpresa).FirstOrDefault();
                }
                else
                {
                    if (cboEmpresaLegajo.SelectedItem != null)
                    {
                        idCombo = long.Parse(cboEmpresaLegajo.SelectedValue);
                        LegInsert.objEmpresaLegajo = dc.Empresa.Where(w => w.IdEmpresa == idCombo).FirstOrDefault();
                    }
                }

                //if (txtFechaVerificacion.SelectedDate != null)
                //    LegInsert.FechaUltimaVerificacion = txtFechaVerificacion.SelectedDate.Value;
                //else
                //    LegInsert.FechaUltimaVerificacion = null;

                /// La fecha FechaUltimaVerificacion solo se debe actualizar si existe algun comentario en la
                /// observación de la verificacion, caso contrario se deja en blanco. (Martin : 01/04/2014)
                if ("" != txtObservacion.Text)
                    LegInsert.FechaUltimaVerificacion = DateTime.Now;
                else
                    LegInsert.FechaUltimaVerificacion = null;


                LegInsert.Observacion = txtObservacion.Text;
                LegInsert.ObservacionBloqueo = txtObsBloqueo.Text;
                LegInsert.GrupoSangre = txtGrupoSangre.Text;
                LegInsert.AutorizadoCond = chkAutorizadoConducir.Checked;
                LegInsert.EstudiosBasicos = chkEstudosBasicos.Checked;
                LegInsert.ComplementarioRacs = chkCompRacs.Checked;
                LegInsert.AdicionalQuimicos = chkAdicionalQuimicos.Checked;

                LegInsert.Funcion = txtFuncion.Text;
                LegInsert.FechaUltmaModificacion = DateTime.Now;



                if (txtFechaUltExa.SelectedDate != null)
                    LegInsert.FechaUltimoExamen = txtFechaUltExa.SelectedDate.Value;
                else
                    LegInsert.FechaUltimoExamen = null;

                if (txtFechaVenCredencial.SelectedDate != null)
                    LegInsert.CredVencimiento = txtFechaVenCredencial.SelectedDate.Value;
                else
                    LegInsert.CredVencimiento = null;



                ////////// Datos Seguro

                LegInsert.NroPoliza = txtNroPoliza.Text;
                LegInsert.Descripcion = txtDescripcion.Text;

                if (cboCompañia.SelectedItem != null)
                {
                    idCombo = long.Parse(cboCompañia.SelectedValue);
                    LegInsert.objCompañiaSeguro = dc.Clasificacion.Where(w => w.IdClasificacion == idCombo).FirstOrDefault();
                }

                if (txtFechaInicio.SelectedDate != null)
                {
                    LegInsert.FechaInicial = txtFechaInicio.SelectedDate.Value;
                }

                if (txtFechaUltPago.SelectedDate != null)
                {
                    LegInsert.FechaUltimoPago = txtFechaUltPago.SelectedDate.Value;
                }

                if (txtFechaVenicimiento.SelectedDate != null)
                {
                    LegInsert.FechaVencimiento = txtFechaVenicimiento.SelectedDate.Value;
                }

                ///////////////////////////////////////////
                ////PROCESO DE ASIGNACION A CONTRATO DEL LEGAJO
                if (cboContratoLegajo.SelectedValue != "-1" && cboContratoLegajo.SelectedValue != "" && cboPeriodoLegajo.Text != string.Empty && cboContratoLegajo.Text != string.Empty && cboEmpresaLegajo.Text != string.Empty)
                {
                    GenerarAsignacionLegajoContratos(dc, LegInsert, LegInsert.CredVencimiento);
                }

                dc.AddToLegajos(LegInsert);
                dc.SaveChanges();
                this.RadGrid1.DataSource = FiltrarLegajos(txtApellidoLegajo.Text, txtNroDoc.Text, RadGrid1.PageSize);
                this.RadGrid1.DataBind();
                this.UpdPnlGrilla.Update();
            }

            #endregion
        }
        else if (e.Argument == "DesAsignar")
        {
            long idLegajo = long.Parse(RadGrid1.SelectedValue.ToString());
            // Se tiene que ejecutar la misma funcionalidad que esta en la pantalla de AsignacionLegajosContratos

            ///PROCESO DE DESASIGNACION DE LEGAJOS EN CONTRATO

            if (cboPeriodoLegajo.Text != string.Empty && cboContratoLegajo.Text != string.Empty && cboEmpresaLegajo.Text != string.Empty)
            {
                DateTime periodo = DateTime.Parse(cboPeriodoLegajo.Text + "/01");
                string NroContrato = cboContratoLegajo.Text;
                long idEmpLeg = long.Parse(cboEmpresaLegajo.SelectedValue);

                long idContratoEmpresa = (from C in Contexto.ContratoEmpresas
                                          where C.Contrato.Codigo == NroContrato && C.IdEmpresa == idEmpLeg
                                          select C.IdContratoEmpresas).FirstOrDefault();

                Legajos Legajo = (from l in Contexto.Legajos
                                  where l.IdLegajos == idLegajo
                                  select l).FirstOrDefault();

                var contEmpLegajos = from C in Contexto.ContEmpLegajos.Include("CabeceraHojasDeRuta")
                                     where C.ContratoEmpresas.IdContratoEmpresas == idContratoEmpresa
                                     && C.Legajos.IdLegajos == idLegajo
                                     && C.CabeceraHojasDeRuta.Periodo >= periodo
                                     select C;

                foreach (Entidades.ContEmpLegajos cont in contEmpLegajos)
                {
                    if (!cont.CabeceraHojasDeRutaReference.IsLoaded) { cont.CabeceraHojasDeRutaReference.Load(); }

                    if (cont.CabeceraHojasDeRuta != null && cont.CabeceraHojasDeRuta.Periodo.Year == periodo.Year
                        && cont.CabeceraHojasDeRuta.Periodo.Month == periodo.Month)
                    {
                        cont.FechaTramiteBaja = DateTime.Now;
                    }
                    else
                    {
                        Contexto.DeleteObject(cont);
                    }
                }

                ////DesHabilito la Credencial
                Legajo.CredVencimiento = null;
                Contexto.SaveChanges();

                //CargarSessionLegajos();
                this.RadGrid1.DataSource = FiltrarLegajos(txtApellidoLegajo.Text, txtNroDoc.Text, RadGrid1.PageSize);
                this.RadGrid1.DataBind();
                this.UpdPnlGrilla.Update();
            }
        }
    }

    private void GenerarAsignacionLegajoContratos(EntidadesConosud dc, Entidades.Legajos LegUpdate, DateTime? FechaVencCredencial)
    {
        if (cboPeriodoLegajo.Text != "")
        {
            DateTime periodo = DateTime.Parse(cboPeriodoLegajo.Text + "/01");
            string NroContrato = cboContratoLegajo.Text;
            long IdContrato = long.Parse(cboContratoLegajo.SelectedValue);
            long idEmpLeg = long.Parse(cboEmpresaLegajo.SelectedValue);
            //long idEstado = 15;

            ////Supongo que el contrato es unico y no corresponde a mas de 2 empresas
            long idContratoEmpresa = (from C in dc.ContratoEmpresas
                                      where C.Contrato.IdContrato == IdContrato && C.IdEmpresa == idEmpLeg
                                      select C.IdContratoEmpresas).FirstOrDefault();

            var cabeceras = (from C in dc.CabeceraHojasDeRuta.Include("ContratoEmpresas")
                             where C.Periodo >= periodo
                             && C.ContratoEmpresas.IdContratoEmpresas == idContratoEmpresa
                             // && C.Estado.IdClasificacion == idEstado
                             select C).ToList();

            foreach (Entidades.CabeceraHojasDeRuta cab in cabeceras)
            {
                List<ContEmpLegajos> legajosExistentes = cab.ContEmpLegajos.ToList();
                ContEmpLegajos legAsigndoExistente = legajosExistentes.Where(w => w.IdLegajos == LegUpdate.IdLegajos).FirstOrDefault();
                if (legAsigndoExistente == null)
                {
                    Entidades.ContEmpLegajos ContEmpLeg = new Entidades.ContEmpLegajos();
                    ContEmpLeg.Legajos = LegUpdate;
                    ContEmpLeg.ContratoEmpresas = cab.ContratoEmpresas;
                    ContEmpLeg.CabeceraHojasDeRuta = cab;
                    dc.AddToContEmpLegajos(ContEmpLeg);
                }
                else
                {
                    legAsigndoExistente.FechaTramiteBaja = null;
                }
            }

            Contrato Contrato = (from C in dc.Contrato
                                 where C.IdContrato == IdContrato
                                 select C).FirstOrDefault();

            if (Contrato != null)
            {
                if (FechaVencCredencial != null)
                {
                    /// 91: Categoria del contrato Auditable al ingreso
                    /// Solo se tiene que actualizar los datos de la credencial si la cateria
                    /// del contrato es distinta a esta.
                    if (Contrato.objCategoria.IdClasificacion != 91)
                    {
                        if (Contrato.Prorroga.HasValue)
                        {
                            LegUpdate.CredVencimiento = Contrato.Prorroga;
                        }
                        else
                        {
                            LegUpdate.CredVencimiento = Contrato.FechaVencimiento;
                        }
                    }
                }
            }
        }
    }

    protected void cboPolAccPer_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        //long IdEmpresa = long.Parse(e.Context["IdEmpresa"].ToString());
        //string nropoliza = "";
        //if (e.Text.IndexOf(" ") > 0)
        //    nropoliza = e.Text.Substring(0, e.Text.IndexOf(" "));
        //else
        //    nropoliza = e.Text;

        ///// Cargo los seguros dados de alta para la empresa seleccionada
        //var Seguros = (from s in Contexto.Seguros
        //               where s.NroPoliza.StartsWith(nropoliza)
        //               && s.objEmpresa.IdEmpresa == IdEmpresa
        //               select new
        //               {
        //                   Descripcion = s.NroPoliza + " - " + s.objCompañia.Descripcion + " - " + s.objTipoSeguro.Descripcion,
        //                   s.IdSeguro
        //               }).Distinct().Take(10);

        //cboPolAccPer.Items.Clear();
        //if (Seguros.Count() > 0)
        //{
        //    foreach (var item in Seguros)
        //    {
        //        cboPolAccPer.Items.Add(new RadComboBoxItem(item.Descripcion, item.IdSeguro.ToString()));
        //    }
        //}
        //else
        //{
        //    cboPolAccPer.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
        //}
    }

    protected void CargarSessionLegajos()
    {
        //if (this.Session["TipoUsuario"] == "Cliente")
        //{
        //    long idEmpresa = long.Parse(HiddenIdEmpresa.Value);
        //    DatosLegajos = (from leg in Contexto.Legajos
        //                    where leg.EmpresaLegajo == idEmpresa
        //                    orderby leg.Apellido
        //                    select leg).ToList();

        //}
        //else
        //{

        //    DatosLegajos = (from leg in Contexto.Legajos
        //                    orderby leg.Apellido
        //                    select leg).ToList();
        //}
    }

    protected void cboContratos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        //////Probar bindeandolos asi esta en Asignacion legajos contratos
        long idEmpresa = long.Parse(e.Text);
        long idUsuario = long.Parse(Session["idusu"].ToString());

        /// Busco si el usuario tiene restricciones para los contratos.
        var datos = (from c in Contexto.SegContextos
                     where c.SegUsuario == idUsuario 
                     && c.Contexto == "CONTRATO"
                     select c.Valor).ToList();

        List<long> contratosHabilitados = datos.Select(w => long.Parse(w)).ToList();



        /// Cargo los contratos para la empresa seleccionada
        var Contratos = (from c in Contexto.ContratoEmpresas
                         where c.IdEmpresa == idEmpresa && (contratosHabilitados.Count == 0 || contratosHabilitados.Contains(c.IdContrato.Value))
                         select new
                         {
                             c.Contrato.Codigo,
                             c.Contrato.IdContrato,
                             c.Contrato.FechaVencimiento,
                             c.Contrato.Prorroga,
                             Categoria = c.Contrato.objCategoria.Descripcion,
                             IdCategoria = c.Contrato.Categoria,
                             FechaInicio = c.Contrato.FechaInicio,

                         }).Distinct();

        cboContratoLegajo.Items.Clear();
        if (Contratos.Count() > 0)
        {
            RadComboBoxItem vacio = new RadComboBoxItem("Sin Afectación", "-1");
            vacio.Attributes.Add("Tipo", "");
            cboContratoLegajo.Items.Add(vacio);


            foreach (var item in Contratos)
            {
                string Codigo = item.Codigo + " (" + string.Format("{0:MM/yyyy}", item.FechaInicio.Value) + " - " + string.Format("{0:MM/yyyy}", (item.Prorroga.HasValue ? item.Prorroga.Value : item.FechaVencimiento.Value)) + ")";
                RadComboBoxItem cont = new RadComboBoxItem(Codigo, item.IdContrato.ToString());

                if (item.IdCategoria.HasValue && item.IdCategoria == 91)
                {
                    DateTime fechaFin;
                    if (item.Prorroga.HasValue && item.Prorroga > item.FechaVencimiento)
                        fechaFin = item.Prorroga.Value;
                    else
                        fechaFin = item.FechaVencimiento.Value;

                    if (DateTime.Now.AddMonths(1) > fechaFin)
                        cont.Attributes.Add("FechaVencimiento", fechaFin.ToShortDateString());
                    else
                        cont.Attributes.Add("FechaVencimiento", DateTime.Now.AddMonths(1).ToShortDateString());
                }
                else
                {
                    if (item.Prorroga.HasValue && item.Prorroga > item.FechaVencimiento)
                        cont.Attributes.Add("FechaVencimiento", item.Prorroga.Value.ToShortDateString());
                    else
                        cont.Attributes.Add("FechaVencimiento", item.FechaVencimiento.Value.ToShortDateString());
                }

                cont.Attributes.Add("Tipo", item.Categoria);
                cboContratoLegajo.Items.Add(cont);
            }
        }
        else
        {
            cboContratoLegajo.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
        }
    }

    protected void cboPeriodos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        long idEmpresa = long.Parse(e.Text.Split('|')[0]);
        long idContrato = 0;
        string codContrato = string.Empty;
        DateTime fechaCondulta = Convert.ToDateTime("01/" + DateTime.Now.Date.Month + "/" + DateTime.Now.Date.Year).AddMonths(1).AddDays(-1).Date;

        if (e.Text.Split('|').Count() > 2)
        {
            codContrato = e.Text.Split('|')[2];
        }
        else
        {
            if (e.Text.Split('|')[1] != "")
            {
                idContrato = long.Parse(e.Text.Split('|')[1]);
            }
            else
            {
                cboPeriodoLegajo.Items.Clear();
                return;
            }
        }

        if (codContrato != string.Empty)
        {
            ////Aca entraria en el caso de estar editando un contraro
            var periodos = (from c in Contexto.CabeceraHojasDeRuta
                            where c.ContratoEmpresas.IdEmpresa == idEmpresa
                            && c.ContratoEmpresas.Contrato.Codigo == codContrato
                            && c.Periodo <= fechaCondulta
                            select new
                            {
                                c.Periodo,
                                c.IdCabeceraHojasDeRuta
                            }).ToList();

            cboPeriodoLegajo.Items.Clear();
            if (periodos.Count() > 0)
            {

                DateTime periodoSel = DateTime.Parse(e.Text.Split('|')[3].ToString() + "/01");
                foreach (var item in periodos)
                {
                    string p = string.Format("{0:MM/yyyy}", item.Periodo);
                    RadComboBoxItem itemCbo = new RadComboBoxItem(p, item.IdCabeceraHojasDeRuta.ToString());
                    if (periodoSel == item.Periodo)
                    {
                        itemCbo.Selected = true;
                    }
                    cboPeriodoLegajo.Items.Add(itemCbo);
                }
            }
            else
            {
                cboPeriodoLegajo.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
            }

            ////Pruebas para la Edicion que no funcionaron (cerrar ek combo)
            //cboPeriodoLegajo.OpenDropDownOnLoad = false;
            //UpdPnlComboPeriodos.Update();
            //this.cboPeriodoLegajo.Enabled = true;
            //ScriptManager.RegisterStartupScript(this.UpdPnlComboPeriodos, typeof(UpdatePanel), "Error Grabacion", "CerrarCombo();", true);
            //ScriptManager.RegisterStartupScript(Page, typeof(Page), "Error", "CerrarCombo();", true);
            //cboPeriodoLegajo.OpenDropDownOnLoad = false;
        }
        else
        {
            ///// Cargo los contratos para la empresa seleccionada
            var periodos = (from c in Contexto.CabeceraHojasDeRuta
                            where c.ContratoEmpresas.IdEmpresa == idEmpresa && c.ContratoEmpresas.Contrato.IdContrato == idContrato
                             && c.Periodo <= fechaCondulta
                            select new
                            {
                                c.Periodo,
                                c.IdCabeceraHojasDeRuta
                            }).ToList();


            cboPeriodoLegajo.Items.Clear();
            if (periodos.Count() > 0)
            {

                foreach (var item in periodos)
                {
                    string p = string.Format("{0:MM/yyyy}", item.Periodo);
                    cboPeriodoLegajo.Items.Add(new RadComboBoxItem(p, item.IdCabeceraHojasDeRuta.ToString()));
                }
            }
            else
            {
                cboPeriodoLegajo.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
            }
        }
    }



    [WebMethod(EnableSession = true)]
    public static List<TempCursos> ObtenerCursos(long IdLegajo)
    {
        using (EntidadesConosud dc = new EntidadesConosud())
        {
            var cursosLegajos = from c in dc.CursosLegajos
                                where c.Legajo == IdLegajo
                                select new TempCursos
                                {
                                    IdCursoLegajo = c.IdCursoLegajo,
                                    Curso = c.objCurso.Descripcion,
                                    FechaCurso = c.FechaCurso.Value,
                                    FechaVencimiento = c.FechaVencimiento.Value,
                                    Dictado = c.objInstituto.Descripcion,
                                    Aprobado = c.Aprobado
                                };

            return cursosLegajos.ToList();
        }

    }


}
