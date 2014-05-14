using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
//using Entidades;

public partial class PruebaBusquedaXml : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //EntidadesConosud dc = new EntidadesConosud();
        //List<Entidades.Empresa> legajos = (from L in dc.Empresa
        //                                    select L).ToList<Entidades.Empresa>();

        //XDocument xDoc = new XDocument(
        //        new XElement("searchable_index", from pt in legajos select new XElement("item", pt.RazonSocial + "  " + pt.CUIT)));
        ////new XElement("searchable_index", from pt in legajos select new XElement("item", new XElement("x", pt.CUIL), new XElement("y", pt.Apellido))));

        //xDoc.Save(@"c:\legajos.xml");
    }
}
