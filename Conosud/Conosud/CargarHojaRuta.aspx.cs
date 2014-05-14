using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Subgurim.Controles;
using System.Linq;

public partial class CargarHojaRuta : System.Web.UI.Page
{
    private static DSConosud.CabeceraHojasDeRutaDataTable _DTCabecera;
    private DSConosud.HojasDeRutaRow _CurrentItem = null;
    private bool _esContratista = false;
    private static int _idCabecera = 0;
    private static int _idContratoEmpresa = 0;
    private bool _esPenultimaHoja = false;
    private bool _esUltimaHoja = false;
    private const int ULTIMA=1;
    private const int PENULTIMA=0;
    private bool _puedeAprobarPenultimaHoja = false;
    public string FechaActual = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
    public static DSConosud.ArchivosAdjuntosDataTable _DTArchivos;
    public string _DescripcionItemSel = "";


    protected void Page_Load(object sender, EventArgs e)
    {

        btnAprobar.Attributes.Add("onmouseover", "this.src='images/HojaAprobadaUp.gif'");
        btnAprobar.Attributes.Add("onmouseout", "this.src='images/HojaAprobada4.gif'");
        
        btnComentario.Attributes.Add("onmouseover", "this.src='images/ComentarioUp.gif'");
        btnComentario.Attributes.Add("onmouseout", "this.src='images/Comentario.gif'");

        btnEstimacion.Attributes.Add("onmouseover", "this.src='images/EstimacionUp4.gif'");
        btnEstimacion.Attributes.Add("onmouseout", "this.src='images/Estimacion4.gif'");

        //DetailsView1.Attributes.Add("onkeydown", "NoSubmit();");

        //if (FileUploaderAJAX1.IsDeleting)
        //{
        //}
        //else if (FileUploaderAJAX1.IsPosting)
        //    this.managePost();
        if (1==1)
        {
            
            //FileUploaderAJAX1.showDeletedFilesOnPostBack = false;


            gdEncezado.SelectedIndex = 0;
            imgUltimoCertificado.Visible = false;
            _idCabecera = Convert.ToInt32(Request.QueryString["id"]);
            _esContratista = Convert.ToBoolean(((System.Web.UI.WebControls.DataControlFieldCell)gdEncezado.SelectedRow.Controls[9]).Text);
            _idContratoEmpresa = Convert.ToInt32(((System.Web.UI.WebControls.DataControlFieldCell)gdEncezado.SelectedRow.Controls[10]).Text);

            

            if (!IsPostBack)
            {
                GestionComentarios(_idContratoEmpresa);

                ///recupero las ultimas dos hojas de ruta
                ///para hacer controles posteriores.
                DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter TACabecera = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
                _DTCabecera = TACabecera.GetUltimasCabecerasByContrato(_idContratoEmpresa);



                lblContratista.Text = ((System.Web.UI.WebControls.DataControlFieldCell)gdEncezado.SelectedRow.Controls[0]).Text;
                lblSubCon.Text = ((System.Web.UI.WebControls.DataControlFieldCell)gdEncezado.SelectedRow.Controls[1]).Text;
                lblNroCarpeta.Text = "Nº " + ((System.Web.UI.WebControls.DataControlFieldCell)gdEncezado.SelectedRow.Controls[5]).Text;
                lblDescContrato.Text = ((System.Web.UI.WebControls.DataControlFieldCell)gdEncezado.SelectedRow.Controls[2]).Text + " - " + ((System.Web.UI.WebControls.DataControlFieldCell)gdEncezado.SelectedRow.Controls[3]).Text; ;
                lblEstadoDoc.Text = ((System.Web.UI.WebControls.DataControlFieldCell)gdEncezado.SelectedRow.Controls[4]).Text.ToUpper();
                lblTituloComentario.Text += ((System.Web.UI.WebControls.DataControlFieldCell)gdEncezado.SelectedRow.Controls[2]).Text.ToUpper() + " - " + ((System.Web.UI.WebControls.DataControlFieldCell)gdEncezado.SelectedRow.Controls[3]).Text.ToUpper() ;

                DateTime Periodo = Convert.ToDateTime(((System.Web.UI.WebControls.DataControlFieldCell)gdEncezado.SelectedRow.Controls[6]).Text);
                lblFechaHojaRuta.Text = string.Format("{0:yyyy/MM}", Periodo);

                lblUsuario.Text = this.Session["nombreusu"].ToString();

            }


            /// Controlo si es la penultima hoja de ruta
            if (((DSConosud.CabeceraHojasDeRutaRow)_DTCabecera.Rows[PENULTIMA]).IdCabeceraHojasDeRuta == _idCabecera)
            {
                imgUltimoCertificado.Visible = true;
                _esPenultimaHoja = true;
            }
            else if (((DSConosud.CabeceraHojasDeRutaRow)_DTCabecera.Rows[ULTIMA]).IdCabeceraHojasDeRuta == _idCabecera)
            {
                _esUltimaHoja = true;

                if (((DSConosud.CabeceraHojasDeRutaRow)_DTCabecera.Rows[PENULTIMA]).IdEstado == (long)Helpers.EstadosHoja.NoAprobada)
                {
                    _puedeAprobarPenultimaHoja = true;
                    DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter TACabecera = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
                    if (Convert.ToInt32(TACabecera.EstadosItemsPenultimaHoja(_idContratoEmpresa)) == 0)
                    {
                        _puedeAprobarPenultimaHoja = true;
                        ConfirmButtonExtender1.ConfirmText = "La Hoja de Ruta anterior correspondiente al periodo: " + string.Format("{0:yyyy/MM}", ((DSConosud.CabeceraHojasDeRutaRow)_DTCabecera.Rows[PENULTIMA]).Periodo) + "\n" +
                                                             " se aprobará, junto con la actual, ya que esta en condiciones de ser aprobada.";
                    }
                }


            }
        }

        if (EsAprobador())
        {
            btnAprobar.Visible = true;
            btnComentario.Visible = true;
            btnEstimacion.Visible = true;
        }
        else
        {
            btnAprobar.Visible = false;
            btnComentario.Visible = false;
            btnEstimacion.Visible = false;
        }

        if (EsPublicador())
        {
            chkPublicar.Enabled = true;
        }
        else
        {
            chkPublicar.Enabled = false;
        }


        btnAprobarItems.Visible = true; 

//        FileUploaderAJAX1.CssClass = "fua";
//        FileUploaderAJAX1.Style = @"
//        body{font-size: smaller; 
//            color: white;
//            background-color: #F1DDDD;}
//        a{font-weight:#800000}";

        if (!IsPostBack)
        {
            ConosudDataContext dc = new ConosudDataContext();
            var itemsHojaRuta = from H in dc.HojasDeRutas
                                where H.IdCabeceraHojaDeRuta == int.Parse(Request.QueryString["Id"])
                                select H;

            //gdItemHoja.DataSource = itemsHojaRuta;
            //gdItemHoja.DataBind();
        }

    }
    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        this.gdItemHoja.DataBind();
        HabilitarComentarios();
    }
    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        this.gdItemHoja.DataBind();
    }
    protected void ODSContLegajos_DataBinding(object sender, EventArgs e)
    {
        lblTotalLegajos.Text = gvLegajos.Rows.Count.ToString();

    }
    protected void ODSContLegajos_Init(object sender, EventArgs e)
    {

    }
    protected void ODSContLegajos_Load(object sender, EventArgs e)
    {
    }
    protected void gdItemHoja_SelectedIndexChanged(object sender, EventArgs e)
    {
        HabilitarComentarios();

        if (gdItemHoja.SelectedRow != null)
        {
            Control ctr = gdItemHoja.SelectedRow.Cells[3].FindControl("lblComentDoc");
            txtComentDoc.Text = ((Label)ctr).Text;
            _DescripcionItemSel = gdItemHoja.SelectedRow.Cells[1].Text;
            string auditadoPor = (gdItemHoja.SelectedRow.FindControl("lblAuditadoPor") as Label).Text;

            if ((auditadoPor == "" || auditadoPor == Application["idusuario"].ToString()) && PoseeRol(long.Parse((gdItemHoja.SelectedRow.FindControl("lblIdPlanilla") as Label).Text)))
            {
                DetailsView1.Fields[8].Visible = true;
                DetailsView1.Fields[10].Visible = false;
            }
            else
            {
                DetailsView1.Fields[8].Visible = false;
                DetailsView1.Fields[10].Visible = true;
            }


        }
    }
    protected void gdItemHoja_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
       

        if (gdItemHoja.SelectedIndex != -1)
        {
            foreach (Control ctr in gdItemHoja.Rows[gdItemHoja.SelectedIndex].Cells[gdItemHoja.Columns.Count - 1].Controls)
            {
                if (ctr.ID == "btnEditComentHoja")
                {
                    ((ImageButton)ctr).Enabled = false;
                    break;
                }
            }

            foreach (Control ctr in gdItemHoja.Rows[gdItemHoja.SelectedIndex].Cells[3].Controls)
            {
                if (ctr.ID == "btnEditComentDoc")
                {
                    ((ImageButton)ctr).Enabled = false;
                    break;
                }
            }



        }
    }
    protected void btnAprobar_Click(object sender, ImageClickEventArgs e)
    {
        bool EsApta = true;
        string itemControlar = "";

        foreach (GridViewRow ItemPla in gdItemHoja.Rows)
        {
            foreach (Control ctr in ItemPla.Cells[4].Controls)
            {
                if (ctr.ID == "chkAprobo" && !((CheckBox)ctr).Checked)
                {
                    EsApta = false;
                    itemControlar += "\\t" + gdItemHoja.DataKeys[ItemPla.RowIndex].Values["Titulo"].ToString() + "\\n";
                }
            }
        }

        if (!EsApta)
        {
            string alert = "alert('No todos los ítems estan aprobados, debe aprobar todos los ítems para aprobar la Hoja de Ruta.\\n" +
                            "Items por Controlar:\\n" + itemControlar + "')";

            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);

            return;
        }


        // La empresa como los subcontratistas tiene que tener la hoja de ruta 
        // aprobada para que la contratista pueda aprobar la misma.
        if (_esContratista)
        {

            DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter TACabecera = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
            int? cant = TACabecera.GetCantHojasNoAprobada(_idCabecera);

            if (cant > 0)
            {
                string alert = "alert('Algunos de los subcontratistras no posee la hoja de ruta aprobada para este período.\\nNo se puede aprobar esta hoja de ruta.')";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
                return;
            }
        }

        //No Aprobar la Hoja Actual si las anteriores no están Aprobadas
        if (!_esUltimaHoja)
        {
            DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter TACabecera = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
            int Cantidad = Convert.ToInt32(TACabecera.CantHojasAntNoAprobadas(_idContratoEmpresa,_idCabecera));
            if (Cantidad > 0)
            {
                string alert = "alert('No se puede Aprobar dicha hoja de ruta hasta no aprobar las anteriores.')";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
                return;
            }
        }


        //  No aprobar la penúltima hoja si la ultima no esta aprobada.  
        if (_esPenultimaHoja)
        {
            if (((DSConosud.CabeceraHojasDeRutaRow)_DTCabecera.Rows[ULTIMA]).IdEstado == (long)Helpers.EstadosHoja.NoAprobada)
            {
                string alert = "alert('No se puede Aprobar dicha hoja de ruta hasta no aprobar la Ultima hoja de ruta.')";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
                return;
            }

        }



        // Si llego a este punto significa que se puede aprobar la hoja
        //actual.
        if (_esUltimaHoja)
        {
            if (_puedeAprobarPenultimaHoja)
            {
                // Aprueba la Cabecera de la Hoja Actual.
                DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter TACabecera = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
                _DTCabecera = TACabecera.GetUltimasCabecerasByContrato(_idContratoEmpresa);
                ((DSConosud.CabeceraHojasDeRutaRow)_DTCabecera.Rows[PENULTIMA]).IdEstado = (long)Helpers.EstadosHoja.Aprobada;
                ((DSConosud.CabeceraHojasDeRutaRow)_DTCabecera.Rows[ULTIMA]).IdEstado = (long)Helpers.EstadosHoja.Aprobada;
                TACabecera.Update(_DTCabecera);
            }
            else
            {
                string alert = "alert('No se puede Aprobar la ultima hoja de ruta hasta aprobar la hoja anterior.')";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
                return;
            }
        }
        else
        {
            // Aprueba la Cabecera de la Hoja Actual.
            DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter TACabecera = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
            _DTCabecera = TACabecera.GetById(_idCabecera);
            ((DSConosud.CabeceraHojasDeRutaRow)_DTCabecera.Rows[0]).IdEstado = (long)Helpers.EstadosHoja.Aprobada;
            TACabecera.Update(_DTCabecera);
        }

        

    }
    protected void gdItemHoja_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdItemHoja.SelectedIndex = -1;
    }
    protected void ObjectDataSource1_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        e.InputParameters["AuditadoPor"] = this.Session["idusu"].ToString();
    }
 
    #region Metodos Privados
    private void managePost()
    {
        //HttpPostedFileAJAX pf = FileUploaderAJAX1.PostedFile;
        //FileUploaderAJAX1.SaveAs("~/Documentos", pf.FileName);

        //_DTArchivos.Rows.Add(new object[] { null, pf.FileName, _idContratoEmpresa, DateTime.Now ,false});
        //gvArchivos.DataSource = _DTArchivos;
        //gvArchivos.DataBind();

    }
    private bool PoseeRol(long IdPlanilla)
    {
        ConosudDataContext dc = new ConosudDataContext();

        long IdUsuario = long.Parse(this.Application["idusuario"].ToString());
        List<long> RolesUsuario = (from U in dc.SegUsuarios
                                 from UR in U.ColSegUsuarioRols
                                    where U.IdSegUsuario == IdUsuario
                                    select UR.Rol).ToList<long>();

        List<SegRolMenu> Roles = (from M in dc.SegMenus
                    from RM in M.ColSegRolMenus
                    where M.Url == Request.AppRelativeCurrentExecutionFilePath
                    && RolesUsuario.Contains<long>(RM.Rol)
                    select RM).ToList<SegRolMenu>();

        bool PuedeModificar = false;
        /// si al menos uno de los roles 
        /// asignados tiene el permiso de modificacion
        /// no hace falta controlar seguir controlando.
        foreach (SegRolMenu rolMenu in Roles)
        {
            if (rolMenu.Modificacion)
            {
                PuedeModificar = true;
                break;
            }
        }


        if ((from rp in dc.RolesPlanillas
             where rp.IdRol == 1 && rp.IdPlanilla == IdPlanilla
             select rp).Count() == 0)
        {
            PuedeModificar = false;
        }

        return PuedeModificar;
    }
    private bool EsAprobador()
    {
        ConosudDataContext dc = new ConosudDataContext();

        long IdUsuario = long.Parse(this.Application["idusuario"].ToString());
        List<long> RolesUsuario = (from U in dc.SegUsuarios
                                   from UR in U.ColSegUsuarioRols
                                   where U.IdSegUsuario == IdUsuario
                                   && UR.ObjSegRol.Descripcion == Helpers.RolesEspeciales.Aprobador.ToString()
                                   select UR.Rol).ToList<long>();

        if (RolesUsuario.Count > 0)
            return true;
        else
            return false;
        
    }
    private bool EsPublicador()
    {
        ConosudDataContext dc = new ConosudDataContext();

        long IdUsuario = long.Parse(this.Application["idusuario"].ToString());
        List<long> RolesUsuario = (from U in dc.SegUsuarios
                                   from UR in U.ColSegUsuarioRols
                                   where U.IdSegUsuario == IdUsuario
                                   && UR.ObjSegRol.Descripcion == Helpers.RolesEspeciales.Publicador.ToString()
                                   select UR.Rol).ToList<long>();

        if (RolesUsuario.Count > 0)
            return true;
        else
            return false;
        
    }
   
    private void HabilitarComentarios()
    {
        if (gdItemHoja.SelectedRow != null)
        {
            Control ctr = gdItemHoja.SelectedRow.Cells[5].FindControl("btnEditComentHoja");
            ((ImageButton)ctr).Enabled = true;
            AnimationExtender1.TargetControlID = ctr.UniqueID;


            //ctr = gdItemHoja.SelectedRow.Cells[3].FindControl("btnEditComentDoc");
            //((ImageButton)ctr).Enabled = true;
            //AnimationExtender2.TargetControlID = ctr.UniqueID;

        }
    }
    private void GestionComentarios(int idContEmpresa)
    {
        //if (!FileUploaderAJAX1.IsRequesting)
        //{
            DSConosudTableAdapters.ArchivosAdjuntosTableAdapter DAArchivos = new DSConosudTableAdapters.ArchivosAdjuntosTableAdapter();
            _DTArchivos = DAArchivos.GetDataByContEmp(idContEmpresa);

            gvArchivos.DataSource = _DTArchivos;
            gvArchivos.DataBind();

           // FileUploaderAJAX1.Reset();
        //}

    }

    #endregion 
    
    protected void ODSContLegajos_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.ReturnValue != null)
            lblTotalLegajos.Text = ((DSConosud.ConsultaCabeceraLegajosDataTable)e.ReturnValue).Rows.Count.ToString();
        else
            lblTotalLegajos.Text = "No posee legajos asocidos";
        

    }
    protected void ODSContLegajos_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        object a= e.InputParameters[0];
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        DSConosudTableAdapters.ArchivosAdjuntosTableAdapter DAArchivos = new DSConosudTableAdapters.ArchivosAdjuntosTableAdapter();

        bool EliminacionCompleta = false;
        while (! EliminacionCompleta)
        {
            foreach (DSConosud.ArchivosAdjuntosRow archivo in _DTArchivos)
            {
                if (!System.IO.File.Exists(Server.MapPath(Request.ApplicationPath) + "\\Documentos\\" + archivo.NombreArchivo)
                    || archivo.Eliminar)
                {
                        _DTArchivos.RemoveArchivosAdjuntosRow(archivo);
                        EliminacionCompleta = false;
                        break;
                }
            }
            EliminacionCompleta = true;

            
        }
        
        int index = 0;
        foreach (GridViewRow row in gvArchivos.Rows)
        {
            foreach (Control ctr in row.Cells[0].Controls)
            {
                if (ctr.ID == "CheckBox2")
                {
                    ((DSConosud.ArchivosAdjuntosRow)_DTArchivos.Rows[index]).Eliminar = ((CheckBox)ctr).Checked;

                    if (((CheckBox)ctr).Checked)
                        _DTArchivos.Rows[index].Delete();
                    
                    index++;
                }
            }
        }


        DAArchivos.Update(_DTArchivos);
        gvArchivos.DataSource = _DTArchivos;
        gvArchivos.DataBind();

       // FileUploaderAJAX1.Reset();
         
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
       // FileUploaderAJAX1.Reset();
    }
    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        if (DetailsView1.FindControl("lblDetalleDetailView") != null)
        {
            ((Label)DetailsView1.FindControl("lblDetalleDetailView")).Visible = true;
            ((Label)DetailsView1.FindControl("lblDetalleDetailView")).Text = _DescripcionItemSel.ToUpper();
        }
    }
    protected void SqlDataSource2_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        int a = 0;
    }
    protected void btnAprobarItems_Click(object sender, EventArgs e)
    {
        try
        {
            DSConosud.HojasDeRutaDataTable dtHojaRutaTot = new DSConosud.HojasDeRutaDataTable();
            DSConosudTableAdapters.HojasDeRutaTableAdapter TAHojaRuta = new DSConosudTableAdapters.HojasDeRutaTableAdapter();
            foreach (GridViewRow row in gdItemHoja.Rows)
            {
                CheckBox check = (CheckBox) row.Cells[6].FindControl("chkAprobo");
                TextBox ctr = (TextBox)row.Cells[6].FindControl("txtIdHojaDeRuta");
                DSConosud.HojasDeRutaDataTable dtHojaRuta = TAHojaRuta.GetDataById(Convert.ToInt32(ctr.Text));
                if (check.Checked && ! dtHojaRuta[0].HojaAprobado)
                {
                    dtHojaRuta[0].HojaAprobado = true;
                    dtHojaRuta[0].HojaFechaAprobacion = DateTime.Now;
                    dtHojaRuta[0].HojaFechaControlado = DateTime.Now;
                    dtHojaRutaTot.Merge(dtHojaRuta);
                }
                else if (! check.Checked && dtHojaRuta[0].HojaAprobado)
                {
                    dtHojaRuta[0].HojaAprobado = false;
                    dtHojaRuta[0].SetHojaFechaAprobacionNull(); 
                    dtHojaRuta[0].SetHojaFechaControladoNull();
                    dtHojaRutaTot.Merge(dtHojaRuta);
                }
            }

            TAHojaRuta.Update(dtHojaRutaTot);
            gdItemHoja.DataBind();
        }
        catch (Exception ex)
        {
            
            throw;
        }

    }
    protected void btnEditComentDoc_Click(object sender, ImageClickEventArgs e)
    {
         Control ctr = ((GridViewRow)((Control)sender).NamingContainer).Cells[3].FindControl("lblComentDoc");
         txtComentDoc.Text = ((Label)ctr).Text;
        _DescripcionItemSel = ((GridViewRow)((Control)sender).NamingContainer).Cells[1].Text;
    }
    protected void chkPublicar_CheckedChanged(object sender, EventArgs e)
    {
        ConosudDataContext dc = new ConosudDataContext();

        CabeceraHojasDeRuta cab = (from C in dc.CabeceraHojasDeRutas
                                   where C.IdCabeceraHojasDeRuta == _idCabecera
                                   select C).Single<CabeceraHojasDeRuta>();

        cab.Publicar = chkPublicar.Checked;

        dc.SubmitChanges();
    }
}
