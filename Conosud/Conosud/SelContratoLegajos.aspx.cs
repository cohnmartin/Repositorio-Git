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

public partial class SelContratoLegajos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ibCancelar_Click(object sender, ImageClickEventArgs e)
    {
        string alert = "javascript:window.close();";

        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
    }

    protected void ibOk_Click(object sender, ImageClickEventArgs e)
    {
        string alert = string.Empty;
        if (this.txtSeleccionado.Value == string.Empty)
        {
            alert = "alert('Debe seleccionar un Contrato');";
        }
        else if (this.txtPeriodo.Value == string.Empty)
        {
            alert = "alert('Debe seleccionar un Periodo');";
        }
        else
        {
            alert = "DevolverValor();javascript:window.close();";
        }

        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
    }

    protected void DDLPeriodosDesde_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.txtPeriodo.Value = this.DDLPeriodosDesde.SelectedValue;
        ODSContEmpLeg.DataBind();
    }

    protected void ODSPeriodos_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        
    }

    protected void gvContratos_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.txtSeleccionado.Value = this.gvContratos.SelectedValue.ToString();
    }
    protected void DDLPeriodosDesde_DataBound(object sender, EventArgs e)
    {
        if (this.DDLPeriodosDesde.Items.Count > 0)
        {
            this.txtPeriodo.Value = this.DDLPeriodosDesde.SelectedValue;
        }
    }
}
