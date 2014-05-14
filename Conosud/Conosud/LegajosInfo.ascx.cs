using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;

public partial class LegajosInfo : System.Web.UI.UserControl
{
    public Entidades.Legajos Data;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        //txtApellidoss.Text = Data.Apellido;
        //txtNombre.Text = Data.Nombre;
        //txtNroDocumento.Text = Data.NroDoc;
        //txtFechaNacimiento.SelectedDate = Data.FechaNacimiento;
        //txtCuil.Text = Data.CUIL;
        //txtDireccion.Text = Data.Direccion;
        //txtCodigoPostal.Text = Data.CodigoPostal;
        //txtTelefono.Text = Data.TelefonoFijo;
        //txtEmail.Text = Data.CorreoElectronico;

        //if (Data.objEstadoCivil != null)
        //    cboEstadoCivil.Items.FindItemByValue(Data.objEstadoCivil.IdClasificacion.ToString()).Selected = true;

        //if (Data.objTipoDocumento != null)
        //    cboTipoDoc.Items.FindItemByValue(Data.objTipoDocumento.IdClasificacion.ToString()).Selected = true;

        //if (Data.objProvincia != null)
        //    cboProvincia.Items.FindItemByValue(Data.objProvincia.IdClasificacion.ToString()).Selected = true;

        //if (Data.objNacionalidad != null)
        //    cboNacionalidad.Items.FindItemByValue(Data.objNacionalidad.IdClasificacion.ToString()).Selected = true;

    }
}
