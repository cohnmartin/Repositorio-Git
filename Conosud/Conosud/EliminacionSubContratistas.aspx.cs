using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using System.Data.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using Telerik.Web.UI;

public partial class EliminacionSubContratistas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int año = DateTime.Now.AddMonths(1).Year;
            int mes = DateTime.Now.AddMonths(1).Month;
            Entidades.EntidadesConosud dcAux = new Entidades.EntidadesConosud();
            long idContrado = long.Parse(Request.QueryString["IdContrato"].ToString());


            gvSubContratistas.DataSource = from CE in dcAux.ContratoEmpresas.Include("Empresa")
                                           where CE.Contrato.IdContrato == idContrado
                                           && CE.EsContratista == false
                                           // Antes solo se podian eliminar sub contratistas que tuvieran contrato
                                           // en el periodo seguiente al mes actual, ahora se muestran todas las subs ya que
                                           // se puede eliminar para atras. Bug ID:  223
                                           //&& CE.CabeceraHojasDeRuta.Any(w => w.Periodo.Month >= mes && w.Periodo.Year >= año)
                                           select CE;

            gvSubContratistas.DataBind();
            txtFechaBaja.SelectedDate = DateTime.Now;

        }
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            /// Logica:
            /// Dar la posibilidad de asignar y desadignar las empresas a un contrato haciendo que las hojas de ruta 
            /// respondan a las fechas de ejecución de la accion: 
            /// Asignacion: solo se dejan las hojas de ruta desde la fecha de sistema hasta la finalización del contratl. 
            /// Desasignacion: se deben eliminar las hojas de ruta y sus relaciones desde el mes siguiente a la ejecución de dicha acción. 

            Entidades.EntidadesConosud dcAux = new Entidades.EntidadesConosud();

            foreach (GridDataItem item in gvSubContratistas.Items)
            {
                if ((item.FindControl("chkSeleccion") as CheckBox).Checked)
                {
                    int año = txtFechaBaja.SelectedDate.Value.AddMonths(1).Year;
                    int mes = txtFechaBaja.SelectedDate.Value.AddMonths(1).Month;
                    DateTime fechaEliminacion = new DateTime(año, mes, 1, 0, 0, 0);

                    long IdContratoEmpresas = long.Parse(gvSubContratistas.Items[item.DataSetIndex].GetDataKeyValue("IdContratoEmpresas").ToString());

                    List<Entidades.CabeceraHojasDeRuta> cabEliminar = (from C in dcAux.CabeceraHojasDeRuta
                                                                       where (C.Periodo >= fechaEliminacion)
                                                             && C.ContratoEmpresas.IdContratoEmpresas == IdContratoEmpresas
                                                                       select C).ToList<Entidades.CabeceraHojasDeRuta>();



                    foreach (Entidades.CabeceraHojasDeRuta cab in cabEliminar)
                    {

                        long IdCabeceraEliminar = cab.IdCabeceraHojasDeRuta;

                        /// Eliminacion de los legajos asociados
                        var contratoLegajos = (from CL in dcAux.ContEmpLegajos
                                               where CL.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == IdCabeceraEliminar
                                               select CL);

                        foreach (ContEmpLegajos itemContLeg in contratoLegajos)
                        {
                            dcAux.DeleteObject(itemContLeg);
                        }

                        /// YA NO SE DEBERIAN BORRAR LOS COMENTARIO GENERALES
                        /// YA QUE ES POSIBLE QUE QUEDEN HOJAS DE RUTA ANTERIORES 
                        /// AL MES ACTUAL.
                        ///// Eliminacion de los comentarios generales asociados a la hoja de ruta
                        //var ComentariosGrales = (from CL in dcAux.ComentariosGral
                        //                         where CL.ContratoEmpresas.IdContratoEmpresas == IdContratoEmpresas
                        //                         select CL);

                        //foreach (ComentariosGral itemComent in ComentariosGrales)
                        //{
                        //    dcAux.DeleteObject(itemComent);
                        //}


                        // Eliminacion del contrato empresa
                        dcAux.DeleteObject(cab);
                    }

                    dcAux.SaveChanges();


                    /// Si despues de eliminar las cabeceras seleccionadas, la empresa subcontratista
                    /// se queda sin cabeceras, entoces borro el contrato de la misma, ya que no tiene
                    /// sentido que siga asignado.
                    List<Entidades.CabeceraHojasDeRuta> CabEliminar = (from C in dcAux.CabeceraHojasDeRuta
                                                                       where C.ContratoEmpresas.IdContratoEmpresas == IdContratoEmpresas
                                                                       select C).ToList<Entidades.CabeceraHojasDeRuta>();

                    if (CabEliminar.Count == 0)
                    {



                        // Eliminacion de los comentarios Generales
                        var ComentariosGrales = (from CL in dcAux.ComentariosGral
                                                 where CL.ContratoEmpresas.IdContratoEmpresas == IdContratoEmpresas
                                                 select CL);

                        foreach (ComentariosGral itemComent in ComentariosGrales)
                        {
                            dcAux.DeleteObject(itemComent);
                        }


                        // Eliminacion del contrato empresa
                        Entidades.ContratoEmpresas ContratoEliminar = (from C in dcAux.ContratoEmpresas
                                                                       where C.IdContratoEmpresas == IdContratoEmpresas
                                                                       select C).FirstOrDefault();
                        dcAux.DeleteObject(ContratoEliminar);


                        dcAux.SaveChanges();
                    }

                }

            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "ocultar", "CloseWindows();", true);
        }
        catch
        { }
    }
}
