using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Entidades;


/// <summary>
/// Summary description for ws_HojaRuta
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ws_HojaRuta : System.Web.Services.WebService
{
    private class SegPagina
    {
        public long idPlantilla { get; set; }
        public bool permitirAprobarItem {get;set;}
        public bool permitirEditarItem { get; set; }
        public bool permitirTerminarAuditoria { get; set; }
        public bool permitirVerAuditadoPor { get; set; }
        public bool permitirVerComentarioGral { get; set; }
        public bool permitirVerAprobadoPor { get; set; }
        public bool TerminoAudotoria { get; set; }
    
    }


    public ws_HojaRuta()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(EnableSession = true)]
    public object getItemsHojaRuta(long IdCab)
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {
            var items = (from h in dc.HojasDeRuta
                         where h.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == IdCab
                         orderby h.Plantilla.Codigo
                         select new
                         {
                             AuditadoPor = h.AuditadoPor,
                             DocFechaEntrega = h.DocFechaEntrega,
                             HojaComentario = h.HojaComentario,
                             IdCabecera = IdCab,
                             IdHoja = h.IdHojaDeRuta,
                             IdPlantilla = h.Plantilla.IdPlantilla,
                             Descripcion = h.Plantilla.Descripcion,
                             DocComentario = h.DocComentario,
                             HojaFechaAprobacion = h.HojaFechaAprobacion,
                             AuditoriaTerminada = h.AuditoriaTerminada,
                             IdContratoEmpresa = h.CabeceraHojasDeRuta.IdContratoEmpresa
                         }).ToList();

            List<long> idContratosEmpresa = items.Select(w => w.IdContratoEmpresa).ToList();
            var ComentariosGrales = (from c in dc.ComentariosGral
                                     where idContratosEmpresa.Contains(c.ContratoEmpresas.IdContratoEmpresas)
                                     select new
                                     {
                                         IdContratoEmpresa = c.ContratoEmpresa,
                                         Comentario = c.Comentario,
                                         IdPlantilla = c.Plantilla.Value,
                                         IdComentarioGel = c.IdComentarioGral

                                     }).ToList();


            #region  Consultas de seguridad de la pagina
            Entidades.SegUsuario usuario = (Entidades.SegUsuario) HttpContext.Current.Session["usuario"];
            List<long> idsPlantilla = items.Select(w => w.IdPlantilla).ToList();
            List<SegPagina> seguridadPagina = new List<SegPagina>();

           

            foreach (var item in items)
            {
                SegPagina sg = new SegPagina();
                sg.idPlantilla = item.IdPlantilla;

                long[] rolesAsignados = (from R in dc.RolesPlanilla
                                         where R.IdPlanilla == item.IdPlantilla
                                         select R.SegRol.IdSegRol).Distinct<long>().ToArray<long>();

                int PoseeRol = (from R in usuario.SegUsuarioRol
                                where rolesAsignados.Contains(R.SegRol.IdSegRol)
                                select R).Count();

                if (PoseeRol == 0)
                {
                    //(e.Item.FindControl("chkAprobo") as CheckBox).Enabled = false;
                    //(e.Item.FindControl("imgEdit") as ImageButton).Visible = false;
                    //(e.Item.FindControl("chkTerminoAuditoria") as CheckBox).Enabled = false;

                    sg.permitirAprobarItem = false;
                    sg.permitirEditarItem = false;
                    sg.permitirTerminarAuditoria = false;
                }
                else
                {
                    sg.permitirAprobarItem = true;
                    sg.permitirEditarItem = true;
                    sg.permitirTerminarAuditoria = true;
                }

                seguridadPagina.Add(sg);

                if (item.AuditadoPor != null && item.AuditadoPor.Trim() != "" && item.AuditadoPor != usuario.Login)
                {
                    //(e.Item.FindControl("chkAprobo") as CheckBox).Visible = false;
                    //(e.Item.FindControl("imagenAuditadoPor") as System.Web.UI.WebControls.Image).Visible = true;
                    //(e.Item.FindControl("chkTerminoAuditoria") as CheckBox).Enabled = false;

                    sg.permitirAprobarItem = false;
                    sg.permitirVerAuditadoPor = true;
                    sg.permitirTerminarAuditoria = false;
                }
                else
                {
                    sg.permitirAprobarItem = true;
                    sg.permitirVerAuditadoPor = false;
                    sg.permitirTerminarAuditoria = true;
                }


                if (item.AuditadoPor != null && item.AuditadoPor.Trim() != "" && item.AuditadoPor == usuario.Login && item.HojaFechaAprobacion == null)
                {
                    sg.permitirAprobarItem = true;
                    sg.permitirTerminarAuditoria = true;
                }
                else {
                    sg.permitirAprobarItem = false;
                    sg.permitirTerminarAuditoria = false;
                }


                if (item.AuditoriaTerminada.HasValue && item.AuditoriaTerminada.Value)
                {
                    // resuelto en la interface
                    //(e.Item.FindControl("chkTerminoAuditoria") as CheckBox).Checked = true;
                    

                    if (item.HojaFechaAprobacion.HasValue)
                    {
                        //(e.Item.FindControl("chkTerminoAuditoria") as CheckBox).Enabled = false;
                        sg.permitirTerminarAuditoria = false;
                    }
                    else
                    {
                        //(e.Item.FindControl("chkTerminoAuditoria") as CheckBox).Enabled = true;
                        sg.permitirTerminarAuditoria = true;
                    }
                }






                ////TempComentarioGrales CurrentComentario = (from C in ComentariosGrales
                ////                                          where C.IdPlantilla == hoja.IdPlantilla
                ////                                          select C).FirstOrDefault<TempComentarioGrales>();


                ////if (CurrentComentario != null && CurrentComentario.Comentario != "")
                ////    (e.Item.FindControl("imgcomentariogral") as System.Web.UI.WebControls.Image).Attributes.Add("coment", CurrentComentario.Comentario);
                ////else
                ////    (e.Item.FindControl("imgcomentariogral") as System.Web.UI.WebControls.Image).Visible = false;


                ///// Si el usuario es de una empresa, entonces oculto los comentarios generales
                //if (!usuario.EmpresaReference.IsLoaded) { usuario.EmpresaReference.Load(); }
                //if (usuario.Empresa != null)
                //{
                //    gvItemHoja.Columns.FindByUniqueName("imgComentarioGral").Visible = false;
                //    gvItemHoja.Columns.FindByUniqueName("chkAproboColumn").Visible = false;

                //}


            }
            

            #endregion



            return (from h in items
                    join s in seguridadPagina on h.IdPlantilla equals s.idPlantilla
                    select new {

                        h.AuditadoPor,
                        h.DocFechaEntrega,
                        h.HojaComentario,
                        h.IdCabecera,
                        h.IdHoja ,
                        h.IdPlantilla ,
                        h.Descripcion ,
                        h.DocComentario ,
                        HojaFechaAprobacion  = h.HojaFechaAprobacion!= null ? h.HojaFechaAprobacion.Value.ToShortDateString() : ""  ,
                        h.AuditoriaTerminada ,
                        ComentarioGral = ComentariosGrales.Any(w=> w.IdContratoEmpresa == h.IdContratoEmpresa && w.IdPlantilla == h.IdPlantilla)  ? ComentariosGrales.Where(w=> w.IdContratoEmpresa == h.IdContratoEmpresa && w.IdPlantilla == h.IdPlantilla).FirstOrDefault().Comentario:"",
                        s.permitirAprobarItem,
                        s.permitirEditarItem,
                        s.permitirTerminarAuditoria,
                        s.permitirVerAuditadoPor

                    }).ToList();

            //string.Format("{0:dd/MM/yyyy}", h.HojaFechaAprobacion),



        }
    }


}
