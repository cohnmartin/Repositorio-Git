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


public partial class ABMLegajos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod(EnableSession = true)]
    public static List<dynamic> GetLegajos(string filtroApellido, string FiltroDNI, string TipoUsuario, string IdEmpresa, int take = 15)
    {
        using (EntidadesConosud Contexto = new EntidadesConosud())
        {

            List<ContEmpLegajos> Todos = new List<ContEmpLegajos>();
            List<Legajos> DatosLegajosFiltrados;

            if (TipoUsuario == "Cliente")
            {
                long idEmpresa = long.Parse(IdEmpresa);
                if (filtroApellido.Trim().ToLower() != "")
                {
                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             where emp.Apellido.ToLower().StartsWith(filtroApellido.ToLower())
                                             && emp.EmpresaLegajo == idEmpresa
                                             orderby emp.Apellido
                                             select emp).Take(take).ToList();
                }
                else if (FiltroDNI.Trim().ToLower() != "")
                {

                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             where emp.NroDoc.StartsWith(FiltroDNI.Trim())
                                             && emp.EmpresaLegajo == idEmpresa
                                             orderby emp.Apellido
                                             select emp).Take(take).ToList();
                }
                else
                {
                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             where emp.EmpresaLegajo == idEmpresa
                                             orderby emp.Apellido
                                             select emp).Take(take).ToList();

                }


            }
            else
            {

                if (filtroApellido.Trim().ToLower() != "")
                {
                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             where emp.Apellido.ToLower().StartsWith(filtroApellido.ToLower())
                                             orderby emp.Apellido
                                             select emp).Take(take).ToList();
                }
                else if (FiltroDNI.Trim().ToLower() != "")
                {

                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             where emp.NroDoc.StartsWith(FiltroDNI.Trim())
                                             orderby emp.Apellido
                                             select emp).Take(take).ToList();
                }
                else
                {
                    DatosLegajosFiltrados = (from emp in Contexto.Legajos
                                             orderby emp.Apellido
                                             select emp).Take(take).ToList();

                }

            }



            List<long> idsLegajos = DatosLegajosFiltrados.Select(w => w.IdLegajos).Distinct().ToList();


            var contEmpLegajos = (from e in Contexto.ContEmpLegajos
                                  where idsLegajos.Contains(e.IdLegajos.Value)
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
                List<ContEmpLegajos> TotalContratosLegajo = null;
                var ContratosExistentes = contEmpLegajos.Where(w => w.Key.IdLegajos == leg.IdLegajos && w.Key.IdEmpresa == leg.EmpresaLegajo.Value).Select(w => w.contratos).ToList().FirstOrDefault();
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
                    else
                    {

                        bool encontrado = false;
                        foreach (ContEmpLegajos item in TotalContratosLegajo)
                        {
                            if (encontrado)
                            {
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
                             //d = d,
                             Apellido = d.Apellido,
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

    }

}