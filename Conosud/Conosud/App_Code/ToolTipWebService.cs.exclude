using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Linq;
using System.Data.Linq;
using Entidades;

[ScriptService]
public class ToolTipWebService : System.Web.Services.WebService
{
    [WebMethod]
    public string GetMailMessagesCount(object context)
    {
        IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;
        string elementID = ((string)contextDictionary["Value"]);

        if (elementID == string.Empty)
        {
            throw new Exception("No Value argument is provided to the webservice!");
        }

        //Return a fake message with messages count based on a random number that gets increased each time
        string messages = DateTime.Now.Second.ToString();
        return "Welcome, user. <br/> <b>You have " + messages + " new email messages";
    }



    [WebMethod]
    public string GetToolTipData(object context)
    {
        IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;
        string elementID = ((string)contextDictionary["Value"]);
        long id = long.Parse(elementID);

        if (elementID == string.Empty)
        {
            throw new Exception("No Value argument is provided to the webservice!");
        }

        EntidadesConosud dc = new EntidadesConosud();
        Entidades.Legajos leg = (from E in dc.Legajos
                                 where E.IdLegajos == id
                                 select E).First<Entidades.Legajos>();


        //DataTable information = new DataTable();

        //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString);
        //try
        //{
        //    conn.Open();

        //    SqlDataAdapter adapter = new SqlDataAdapter();

        //    try
        //    {
        //        adapter.SelectCommand = new SqlCommand("SELECT * FROM [Customers] WHERE CustomerID=@id", conn);
        //        adapter.SelectCommand.Parameters.AddWithValue("@id", elementID);
        //        adapter.Fill(information);
        //    }
        //    finally
        //    {
        //        if (!Object.Equals(adapter.SelectCommand, null)) adapter.SelectCommand.Dispose();
        //    }
        //}
        //finally
        //{
        //    if (conn.State == ConnectionState.Open)
        //    {
        //        conn.Close();
        //    }
        //}

        //DataRow row = information.Rows[0];

        return ViewManager.RenderView("LegajosInfo.ascx", leg);
    }

    [WebMethod]
    public string GetCustomersByCountry(object context)
    {
        return this.GetCustomersByCountry(context, "~/Tooltip/Examples/ImageMapToolTipManager/UserControls/");
    }

    [WebMethod]
    public string GetCustomersByCountryClientAPIExample(object context)
    {
        return this.GetCustomersByCountry(context, "~/Tooltip/Examples/RadToolTipManagerClientAPI/UserControls/");
    }

    public string GetCustomersByCountry(object context, string path)
    {
        IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;
        string country = ((string)contextDictionary["Value"]);

        DataTable customers = ToolTipWebService.DB_GetCustomersByCountry(country);
        if (customers.Rows.Count > 0)
            return ViewManager.RenderView(path + "Customers.ascx", customers);
        else
            return ViewManager.RenderView(path + "NoCustomers.ascx", null);
    }





    public static DataTable DB_GetCustomersByCountry(string Country)
    {
        return ToolTipWebService.DB_GetDataTable("SELECT * FROM [Customers] WHERE Country=@id", Country);
    }

    public static DataTable DB_GetOrdersByCustomer(string customerID)
    {
        return ToolTipWebService.DB_GetDataTable("SELECT * FROM [Orders] WHERE CustomerID=@id", customerID);
    }


    static DataTable DB_GetDataTable(string query, string key)
    {
        DataTable information = new DataTable();

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString);
        try
        {
            conn.Open();

            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                adapter.SelectCommand = new SqlCommand(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@id", key);
                adapter.Fill(information);
            }
            finally
            {
                if (!Object.Equals(adapter.SelectCommand, null)) adapter.SelectCommand.Dispose();
            }
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
        return information;
    }



    [WebMethod]
    public string GetFullSizeImage(object context)
    {
        IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;
        string imageID = ((string)contextDictionary["Value"]);

        if (imageID == string.Empty)
        {
            throw new Exception("No Value argument is provided to the webservice!");
        }
        Image img = new Image();
        img.ImageUrl = String.Format("~/Tooltip/Img/Flowers/{0}.jpg", imageID);
        img.Attributes["onload"] = "centerTooltip(this);this.onload = null;";
        StringWriter sw = new StringWriter();
        HtmlTextWriter writer = new HtmlTextWriter(sw);
        img.RenderControl(writer);
        writer.Flush();
        return sw.ToString();
    }
    [WebMethod]
    public string GetToolTipDataTimeout(object context)
    {
        return GetToolTipData(context);
    }

}


public class ViewManager
{
    public static string RenderView(string path)
    {
        return RenderView(path, null);
    }

    public static string RenderView(string path, object data)
    {
        Page pageHolder = new Page();
        UserControl viewControl = (UserControl)pageHolder.LoadControl(path);

        if (data != null)
        {
            Type viewControlType = viewControl.GetType();
            FieldInfo field = viewControlType.GetField("Data");

            if (field != null)
            {
                field.SetValue(viewControl, data);
            }
            else
            {
                throw new Exception("View file: " + path + " does not have a public Data property");
            }
        }

        pageHolder.Controls.Add(viewControl);

        StringWriter output = new StringWriter();
        HttpContext.Current.Server.Execute(pageHolder, output, false);

        return output.ToString();
    }
}


