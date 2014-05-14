using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class PruebaConex : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnConectar_Click(object sender, EventArgs e)
    {
        try
        {

            //Se define el objeto conexión
            System.Data.SqlClient.SqlConnection conn;
            System.Data.SqlClient.SqlDataReader reader;
            System.Data.SqlClient.SqlCommand sql;

            //Se especifica el string de conexión
            conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = lblCadena.Text;

            //Se abre la conexión y se ejecuta la consulta
            conn.Open();

            sql = new System.Data.SqlClient.SqlCommand();
            sql.CommandText = "SELECT * FROM Plantilla";
            sql.Connection = conn;

            reader = sql.ExecuteReader();
            do
            {
                Response.Write(reader.FieldCount + "<BR>");
            } while (reader.Read());

        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
}
