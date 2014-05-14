using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Legajos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (! this.IsPostBack)
        {
            if (this.gvLegajos.Rows.Count != 0)
            {
                this.gvLegajos.SelectedIndex = 0;
            }
        }


        GridViewRow pagerRow = gvLegajos.BottomPagerRow;
        ImageButton igbExcel = (ImageButton)pagerRow.FindControl("igbExcel");
        if (igbExcel != null)
        {
            igbExcel.Click += new ImageClickEventHandler(igbExcel_Click);
        }
    }
    protected void dvLegajos_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        if (e.Exception == null)
        {
            this.gvLegajos.DataBind();
        }
        else
        {
            string alert = "alert('El legajo se esta utilizando en algun contrato, el mismo no puede ser eliminado.')";
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);

            e.ExceptionHandled = true;
        }

    }
    protected void dvLegajos_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        this.gvLegajos.DataBind();
    }
    protected void dvLegajos_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        this.gvLegajos.DataBind();
    }

    protected void igbExcel_Click(object sender, ImageClickEventArgs e)
    {
        DSConosudTableAdapters.LegajosTableAdapter TALeg = new DSConosudTableAdapters.LegajosTableAdapter();
        DSConosud.LegajosDataTable dtLeg = TALeg.GetLegajosCompleto();
        Helpers.GenExcell c = new Helpers.GenExcell();
        c.DoExcell(Server.MapPath(Request.ApplicationPath) + @"\ReporteExcel.xls", dtLeg, this.gvLegajos.Columns);

        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", "window.open('ReporteExcel.xls')", true);
    }

    protected void dvLegajos_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        DSConosudTableAdapters.LegajosTableAdapter TALeg = new DSConosudTableAdapters.LegajosTableAdapter();

        if ((int)TALeg.ExisteNroDoc(e.Values["NroDoc"].ToString()) > 0)
        {
            string alert = "alert('El Nro. de Documento que intenta ingresar ya existe')";
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
            e.Cancel = true;
        }

    }
    protected void dvLegajos_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        DSConosudTableAdapters.LegajosTableAdapter TALeg = new DSConosudTableAdapters.LegajosTableAdapter();

        if (e.OldValues["NroDoc"].ToString() != e.NewValues["NroDoc"].ToString())
        {
            if ((int)TALeg.ExisteNroDoc(e.NewValues["NroDoc"].ToString()) > 0)
            {
                string alert = "alert('El Nro. de Documento que intenta ingresar ya existe')";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
                e.Cancel = true;
            }
        }

    }
}
