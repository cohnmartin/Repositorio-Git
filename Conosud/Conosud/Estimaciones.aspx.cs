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


using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


public partial class Estimaciones : System.Web.UI.Page
{
    private static DSConosud.ContratoEmpresasDataTable _CurrentContEmp = null;
    private static DSConosud.ContratoDataTable _contratos = null;
    private static DSConosud.CabeceraHojasDeRutaDataTable _cabecera = null;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnBusquedaPrincipal_Click(object sender, ImageClickEventArgs e)
    {

        // step 1: creation of a document-object
        Document document = new Document(PageSize.A4);

        try
        {
            Font font;
            Chunk chunk;


            // step 2: we create a writer that listens to the document
            PdfWriter.GetInstance(document, new FileStream("c:\\Pruebas Pdf\\Informe.pdf", FileMode.Create));

            //// we Add a Footer that will show up on PAGE 1
            //HeaderFooter footer = new HeaderFooter(new Phrase("This is page: "), true);
            //footer.Border = Rectangle.NO_BORDER;
            //document.Footer = footer;

            //// we Add a Header that will show up on PAGE 2
            //HeaderFooter header = new HeaderFooter( new Phrase("This is a header"), false);
            //header.Alignment = 1;
            //document.Header = header;

            // step 3: we open the document
            document.Open();

            iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(@"G:\InfoLegacy\Conosud\Analisis\Encabezdo.png");
            png.SetAbsolutePosition(20, 770);
            png.ScalePercent(20);
            document.Add(png);

            // step 4: we Add content to the document

            // PAGE 1
            Paragraph par = new Paragraph("Lujan de Cuyo");
            par.Alignment = 2;
            document.Add(par);


            par = new Paragraph(DateTime.Now.ToLongDateString());
            par.Alignment = 2;
            document.Add(par);

            par = new Paragraph("Señor Jefe");
            par.Alignment = 0;
            document.Add(par);

            font = new Font();
            font = FontFactory.GetFont("Verdana", 12, Font.UNDERLINE);
            chunk = new Chunk("CONTRATOS Y.P.F.", font);
            par = new Paragraph(chunk);
            document.Add(par);

            
            par = new Paragraph("           A continuación se informa la Nómina de Empresas Contratistas que deben documentación mensual, o que no han presentado la documentación correspondiente al mes de mayo/07, de acuerdo a la siguiente clasificación:");
            par.SpacingBefore= 15;
            par.SpacingAfter = 15;
            document.Add(par);



            List listInicial = new List(false, 20);


            List Empresaslist = new List(false, 20);
            Empresaslist.Add(new iTextSharp.text.ListItem("Empresa: " + cboEmpresas.SelectedItem.Text));

            List ContratosList = new List(false, 20);
            ContratosList.Add(new iTextSharp.text.ListItem("Nro. Contrato:" + cboContratos.SelectedItem.Text));

                        
            List ContratistasList = new List(false, 20);
            ContratistasList.Add(new iTextSharp.text.ListItem("Contratista: " + cboContratistas.SelectedItem.Text));


            List SinDocList = new List(false,50);
            font = new Font();
            font = FontFactory.GetFont("Verdana", 12, Font.UNDERLINE);
            font.Color = BaseColor.RED;
            SinDocList.ListSymbol = new Chunk("");
            SinDocList.Add(new iTextSharp.text.ListItem("MESES SIN DOCUMENTACION PRESENTADA", font));

            List ConObsList = new List(false, 50);
            font = new Font();
            font = FontFactory.GetFont("Verdana", 12, Font.UNDERLINE);
            font.Color = BaseColor.BLUE;
            ConObsList.ListSymbol = new Chunk("");
            ConObsList.Add(new iTextSharp.text.ListItem("MESES CON OBSERVACIONES", font));



            ConObsList = GenerarMesesConComentarios(ConObsList);
            SinDocList = GenerarMeseSinDocumentacion(SinDocList);

            listInicial.Add(Empresaslist);
            Empresaslist.Add(ContratosList);
            ContratosList.Add(ContratistasList);

            ContratistasList.Add(SinDocList);
            ContratistasList.Add(ConObsList);


            document.Add(listInicial);



            //par = new Paragraph(cboEmpresas.SelectedItem.Text);
            //par.SpacingBefore = 15;
            //document.Add(par);

            //par = new Paragraph("       " + cboContratos.SelectedItem.Text);
            //document.Add(par);

            //par = new Paragraph("              " + cboContratistas.SelectedItem.Text);
            //document.Add(par);


            //font = new Font();
            //font = FontFactory.GetFont("Verdana", 12, Font.UNDERLINE);
            //font.Color = Color.RED;
            //chunk = new Chunk("Meses Sin Documentacion Presentada", font);
            //par = new Paragraph(chunk);
            //document.Add(par);


            //// we trigger a page break
            //document.NewPage();
            //// PAGE 2
            //// we Add some more content
            //document.Add(new Paragraph("Texto Segunda PAGINA"));
            //// we remove the header starting from PAGE 3
            //document.ResetHeader();
            //// we trigger a page break
            //document.NewPage();

            //// PAGE 3
            //// we Add some more content
            //document.Add(new Paragraph("Hello Sun"));
            //document.Add(new Paragraph("Remark: the header has vanished!"));
            //// we reset the page numbering
            //document.ResetPageCount();
            //// we trigger a page break
            //document.NewPage();

            //// PAGE 4
            //// we Add some more content
            //document.Add(new Paragraph("Hello Moon"));
            //document.Add(new Paragraph("Remark: the pagenumber has been reset!"));

        }
        catch (DocumentException de)
        {
            Console.Error.WriteLine(de.Message);
        }
        catch (IOException ioe)
        {
            Console.Error.WriteLine(ioe.Message);
        }

        // step 5: we close the document
        document.Close();

    }
    protected void cboContratos_SelectedIndexChanged(object sender, EventArgs e)
    {

        //string descip = "";
        //descip += _contratos.Rows[cboContratos.SelectedIndex]["Servicio"].ToString() + " - ";
        //descip += Convert.ToDateTime(_contratos.Rows[cboContratos.SelectedIndex]["FechaInicio"]).ToShortDateString() + " - ";
        //descip += Convert.ToDateTime(_contratos.Rows[cboContratos.SelectedIndex]["FechaVencimiento"]).ToShortDateString() + " - ";
        

    }
    protected void cboContratistas_DataBound(object sender, EventArgs e)
    {
        cboPeriodos.DataBind();
    }
    protected void cboContratos_DataBound(object sender, EventArgs e)
    {
        cboContratistas.DataBind();
    }
    protected void cboPeriodos_DataBound(object sender, EventArgs e)
    {

    }
    protected void cboPeriodos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_cabecera.Rows.Count > 0) { }
    
    }
    protected void ODSPeriodos_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        _cabecera = (DSConosud.CabeceraHojasDeRutaDataTable)e.ReturnValue;
        if (_cabecera.Rows.Count > 0) { }
         
    }
    protected void ODSContratos_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        _contratos = (DSConosud.ContratoDataTable)e.ReturnValue;
        if (_contratos != null)
        {
            if (_contratos.Rows.Count > 0)
            {
                //string descip = "";
                //descip += _contratos.Rows[0]["Servicio"].ToString() + " - ";
                //descip += Convert.ToDateTime(_contratos.Rows[0]["FechaInicio"]).ToShortDateString() + " - ";
                //descip += Convert.ToDateTime(_contratos.Rows[0]["FechaVencimiento"]).ToShortDateString();
                
            }
        }
    }

    #region Metodos Privados
    private List GenerarMeseSinDocumentacion(List SinDocList)
    {
        List MesesList = new List(false, 20);
        DateTime Fecha = Convert.ToDateTime("01/" + cboPeriodos.SelectedItem.Text.Substring(5, 2) + "/" + cboPeriodos.SelectedItem.Text.Substring(0, 4));
        string Meses = "";

        DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter TACabecera = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
        DSConosud.CabeceraHojasDeRutaDataTable DTCabecera = TACabecera.GetHojasSinDoc(Fecha, Convert.ToInt64(cboContratistas.SelectedItem.Value));


        foreach (DSConosud.CabeceraHojasDeRutaRow row in DTCabecera.Rows)
        {
            Meses += string.Format("{0:MMMM} del {0:yyyy} - ", row.Periodo);
        }

        MesesList.Add(new iTextSharp.text.ListItem(Meses));
        SinDocList.Add(MesesList);
        return SinDocList;

    }

    private List GenerarMesesConComentarios(List CurrentList)
    {
        Hashtable MesesConComentarios = new Hashtable();
        Hashtable MesesSinComentarios = new Hashtable();

        List ItemComentarioList = new List(false, 20);
        List ItemSinComentarioList = new List(false, 20);


        DateTime Fecha = Convert.ToDateTime("01/" + cboPeriodos.SelectedItem.Text.Substring(5, 2) + "/" + cboPeriodos.SelectedItem.Text.Substring(0, 4));

        DSConosudTableAdapters.ConsultaItemConDocTableAdapter TAHoja = new DSConosudTableAdapters.ConsultaItemConDocTableAdapter();
        DSConosud.ConsultaItemConDocDataTable DTHoja = TAHoja.GetDataItemConDoc(Fecha, Convert.ToInt64(cboContratistas.SelectedItem.Value));



        foreach (DSConosud.ConsultaItemConDocRow row in DTHoja.Rows)
        {
            if (!MesesConComentarios.ContainsKey(row.Periodo))
            {
                MesesConComentarios.Add(row.Periodo, new List(false, 20));
                MesesSinComentarios.Add(row.Periodo, new List(false, 20));
            }

        }


        foreach (DSConosud.ConsultaItemConDocRow row in DTHoja.Rows)
        {
            if (row.Comentario == "")
            {
                ((List)MesesSinComentarios[row.Periodo]).Add(new iTextSharp.text.ListItem(row.Descripcion));



            }
            else
            {
                ((List)MesesConComentarios[row.Periodo]).Add(new iTextSharp.text.ListItem(row.Descripcion));


                List ComentarioList = new List(false,10);
                Font font = new Font();
                font = FontFactory.GetFont("Verdana", 8);
                font.Color = BaseColor.LIGHT_GRAY;
                ComentarioList.ListSymbol = new Chunk("");
                ComentarioList.Add(new iTextSharp.text.ListItem(row.Comentario, font));

                ((List)MesesConComentarios[row.Periodo]).Add(ComentarioList);
            }

        }



       
        foreach (DateTime mesActual in MesesConComentarios.Keys)
        {
            List Meses = new List(false, 20);
            Meses.Add(new iTextSharp.text.ListItem(string.Format("{0:MMMM} del {0:yyyy} - Con Comentarios", mesActual)));
            Meses.Add((IElement)MesesConComentarios[mesActual]);
            CurrentList.Add(Meses);
        }

        foreach (DateTime mesActual in MesesConComentarios.Keys)
        {
            List Meses = new List(false, 20);
            Meses.Add(new iTextSharp.text.ListItem(string.Format("{0:MMMM} del {0:yyyy} - Sin Documentación", mesActual)));
            Meses.Add((IElement)MesesSinComentarios[mesActual]);
            CurrentList.Add(Meses);
        }


        return CurrentList;

    }
    #endregion 

}
