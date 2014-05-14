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
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using AjaxControlToolkit;
using System.Web.Services;





public partial class ReporteComentariosHojas : System.Web.UI.Page
{
    public static string _nombreArchivo="";
    public static string _rutaArchivo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tblDetalleResumen.Visible = false;
            tblDetalleEnAuditoria.Visible = false;
        }
    }
    protected void btnBusquedaPrincipal_Click(object sender, ImageClickEventArgs e)
    {
        #region Codigo Anterior
        //bool CargoElementos = false;
        //lblEmpresa.Text = cboEmpresasAjax.SelectedItem.Text;
        //lblNroCont.Text = cboContratosAjax.SelectedItem.Text;
        //lblPeriodo.Text = cboPriodosAjax.SelectedItem.Text;


        //_nombreArchivo = @"ComentariosHoja.pdf";
        //_rutaArchivo = Server.MapPath(Request.ApplicationPath) + "\\Documentos\\";
        
        //imgReporte.Attributes["onClick"] = "AbrirDocumento('" + _nombreArchivo + "')";

        //Document document = new Document(PageSize.A4);

        //try
        //{
        //    Font font;

        //    PdfWriter.GetInstance(document, new FileStream(_rutaArchivo + _nombreArchivo, FileMode.Create));
        //    document.Open();

        //    string rutaImagen = Server.MapPath(Request.ApplicationPath) + @"\images\Encabezdo.png";
        //    iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(rutaImagen);
        //    png.SetAbsolutePosition(20, 770);
        //    png.ScalePercent(20);
        //    document.Add(png);


        //    // PAGE 1
        //    Paragraph par = new Paragraph("Lujan de Cuyo");
        //    par.Alignment = 2;
        //    document.Add(par);


        //    par = new Paragraph(DateTime.Now.ToLongDateString());
        //    par.Alignment = 2;
        //    document.Add(par);


        //    string encabezado = "            A Continuación se detallan los comentarios para la hoja de ruta correspondiente al período " + cboPriodosAjax.SelectedItem.Text + ".";


        //    par = new Paragraph(encabezado);
        //    par.SpacingBefore= 15;
        //    par.SpacingAfter = 15;
        //    document.Add(par);


        //    List listInicial = new List(false, 20);


        //    List Empresaslist = new List(false, 20);
        //    Empresaslist.Add(new iTextSharp.text.ListItem("Empresa: " + cboEmpresasAjax.SelectedItem.Text));

        //    List ContratosList = new List(false, 20);
        //    ContratosList.Add(new iTextSharp.text.ListItem("Nro. Contrato:" + cboContratosAjax.SelectedItem.Text));


        //    List ContratistasList = new List(false, 20);
        //    ContratistasList.Add(new iTextSharp.text.ListItem("Contratista: " + cboContratistasAjax.SelectedItem.Text));


        //    DateTime Fecha = Convert.ToDateTime("01/" + cboPriodosAjax.SelectedItem.Text.Substring(5, 2) + "/" + cboPriodosAjax.SelectedItem.Text.Substring(0, 4));
        //    List ConObsList = new List(false, 50);
        //    font = new Font();
        //    font = FontFactory.GetFont("Verdana", 12, Font.UNDERLINE);
        //    font.Color = Color.BLUE;
        //    ConObsList.ListSymbol = new Chunk("");
        //    ConObsList.Add(new iTextSharp.text.ListItem(string.Format("COMENTARIOS {0:MMMM} del {0:yyyy}", Fecha).ToUpper(), font));



        //    GenerarMesesConComentarios(ConObsList, Convert.ToInt64(cboContratistasAjax.SelectedItem.Value), out CargoElementos);
        //    if (CargoElementos)
        //        ContratistasList.Add(ConObsList);



        //    listInicial.Add(Empresaslist);
        //    Empresaslist.Add(ContratosList);
        //    ContratosList.Add(ContratistasList);
        //    document.Add(listInicial);

        //}
        //catch (DocumentException de)
        //{
        //    Console.Error.WriteLine(de.Message);
        //}
        //catch (IOException ioe)
        //{
        //    Console.Error.WriteLine(ioe.Message);
        //}

        //// step 5: we close the document
        //document.Close();
        //tblDetalleResumen.Visible = true;
        #endregion


        ConosudDataContext db = new ConosudDataContext();
        CabeceraHojasDeRuta cab = (from C in db.CabeceraHojasDeRutas
                    where C.IdCabeceraHojasDeRuta == int.Parse(cboPriodosAjax.SelectedItem.Value)
                    select C).Single<CabeceraHojasDeRuta>();

        if (!cab.Publicar.Value && this.Session["TipoUsuario"] != null)
        {
            tblDetalleResumen.Visible = false;
            tblDetalleEnAuditoria.Visible = true;
        }
        else
        {


            bool CargoElementos = false;
            lblEmpresa.Text = cboEmpresasAjax.SelectedItem.Text;
            lblNroCont.Text = cboContratosAjax.SelectedItem.Text;
            lblPeriodo.Text = cboPriodosAjax.SelectedItem.Text;


            _nombreArchivo = @"ComentariosHoja.pdf";
            _rutaArchivo = Server.MapPath(Request.ApplicationPath) + "\\Documentos\\";

            imgReporte.Attributes["onClick"] = "AbrirDocumento('" + _nombreArchivo + "')";

            Document document = new Document(PageSize.A4);

            try
            {
                Font font;
                PdfWriter.GetInstance(document, new FileStream(_rutaArchivo + _nombreArchivo, FileMode.Create));
                document.Open();

                string rutaImagen = Server.MapPath(Request.ApplicationPath) + @"\images\Encabezdo.png";
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(rutaImagen);
                png.SetAbsolutePosition(20, 770);
                png.ScalePercent(20);
                document.Add(png);


                // PAGE 1
                Paragraph par = new Paragraph("Lujan de Cuyo");
                par.Alignment = 2;
                document.Add(par);


                par = new Paragraph(DateTime.Now.ToLongDateString());
                par.Alignment = 2;
                document.Add(par);


                string encabezado = "            A Continuación se detallan los comentarios para la hoja de ruta correspondiente al período " + cboPriodosAjax.SelectedItem.Text + ".";


                par = new Paragraph(encabezado);
                par.SpacingBefore = 15;
                par.SpacingAfter = 15;
                document.Add(par);


                List listInicial = new List(false, 20);

                List ContratosList = new List(false, 20);
                ContratosList.Add(new iTextSharp.text.ListItem("Contrato:" + cboContratosAjax.SelectedItem.Text));

                DateTime Fecha = Convert.ToDateTime("01/" + cboPriodosAjax.SelectedItem.Text.Substring(5, 2) + "/" + cboPriodosAjax.SelectedItem.Text.Substring(0, 4));
                List DetalleItems = new List(false, 20);
                DetalleItems = GenerarDetalleItem(out CargoElementos, Fecha);

                listInicial.Add(ContratosList);
                ContratosList.Add(DetalleItems);
                document.Add(listInicial);

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
            tblDetalleResumen.Visible = true;
            tblDetalleEnAuditoria.Visible = false;
        }

    }
    
    #region Metodos Privados
    private List GenerarDetalleItem(out bool CargoElementos, DateTime Periodo)
    {
        List ItemListFinal = new List(false, 20);
        CargoElementos = false;
        ConosudDataContext db = new ConosudDataContext();
        int IdContrato = int.Parse(cboContratosAjax.SelectedItem.Value);
        List<int> Empresas = new List<int>();

        if (cboContratistasAjax.SelectedItem.Text.IndexOf("Todas") >= 0)
        {
            // Todas las empresas a partir del contrato
            Empresas = (from C in db.ContratoEmpresas 
                                 where C.IdContrato == IdContrato
                                 select C.IdEmpresa.Value).ToList<int>();
        }
        else
        {
            // Solo la empresa Seleccionada a partir del contrato empresa
            Empresas = (from C in db.ContratoEmpresas 
                                 where C.IdContratoEmpresas == int.Parse(cboContratistasAjax.SelectedItem.Value)
                                 select C.IdEmpresa.Value).ToList<int>();
        }


        var GrupoDatos = from C in db.ContratoEmpresas
                         from Ca in C.ColCabeceraHojasDeRutas
                         where C.IdContrato == IdContrato && Ca.Periodo.Date == Periodo.Date
                         && Empresas.Contains<int>(C.IdEmpresa.Value) 
                         group Ca by Ca.Periodo.Date into p
                         select new { Periodos = p.Key, Cabeceras = p };


        foreach (var Periodos in GrupoDatos)
        {
            /// Mes correspondiente
            List MesesList = new List(false, 20);
            MesesList.Add(new iTextSharp.text.ListItem(string.Format("Mes: {0:MMMM}", Periodos.Periodos)));


            foreach (CabeceraHojasDeRuta cab in Periodos.Cabeceras)
            {
                string descripEmpresa = "Contratista: ";
                if (!cab.ObjContratoEmpresa.EsContratista.Value)
                {
                    descripEmpresa = "SubContratista: ";
                }

                //// Empresa en Cuestion
                List Empresaslist = new List(false, 20);
                Empresaslist.Add(new iTextSharp.text.ListItem(descripEmpresa + cab.ObjContratoEmpresa.ObjEmpresa.RazonSocial));

                /// Lista de Item
                List ItemList = new List(false, 20);
                List<HojasDeRuta> Hojas = (from H in cab.ColHojasDeRutas
                                           where H.HojaAprobado == false
                                           orderby H.HojaComentario
                                           select H).ToList<HojasDeRuta>();


                foreach (HojasDeRuta hoja in Hojas)
                {
                    string cadenaDescriptiva = "";

                    if (hoja.HojaComentario != null && hoja.HojaComentario.Trim() != "")
                    {
                        cadenaDescriptiva = hoja.HojaComentario;

                        List ComentarioList = new List(false, 10);
                        Font font = new Font();
                        font = FontFactory.GetFont("Verdana", 8);
                        font.Color = BaseColor.LIGHT_GRAY;
                        ComentarioList.ListSymbol = new Chunk("");
                        ComentarioList.Add(new iTextSharp.text.ListItem(cadenaDescriptiva, font));

                        ItemList.Add(hoja.ObjPlantilla.Descripcion);
                        ItemList.Add(ComentarioList);

                    }
                    else if ((hoja.HojaComentario == null || hoja.HojaComentario.Trim() == "") && !hoja.DocFechaEntrega.HasValue)
                    {
                        ItemList.Add(hoja.ObjPlantilla.Descripcion + " (Sin Documentacion) ");
                    }

                }

                Empresaslist.Add(ItemList);
                MesesList.Add(Empresaslist);

            }
            
            ItemListFinal.Add(MesesList);

        }

        return ItemListFinal;
        

    }
    
    private List GenerarMesesConComentarios(List CurrentList, Int64 id, out bool CargoElementos)
    {
        Hashtable MesesConComentarios = new Hashtable();
        Hashtable MesesSinComentarios = new Hashtable();

        List ItemComentarioList = new List(false, 20);
        List ItemSinComentarioList = new List(false, 20);


        DateTime Fecha = Convert.ToDateTime("01/" + cboPriodosAjax.SelectedItem.Text.Substring(5, 2) + "/" + cboPriodosAjax.SelectedItem.Text.Substring(0, 4));

        DSConosudTableAdapters.ConsultaItemConDocTableAdapter TAHoja = new DSConosudTableAdapters.ConsultaItemConDocTableAdapter();
        DSConosud.ConsultaItemConDocDataTable DTHoja = TAHoja.GetDataCurrentItemConDoc(Fecha,id);


        if (DTHoja.Rows.Count > 0)
        {
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
                    string cadenaDescriptiva = row.Descripcion;
                    cadenaDescriptiva += "- Ultima Presentación: " + row.DocFechaEntrega.ToShortDateString();

                    ((List)MesesConComentarios[row.Periodo]).Add(new iTextSharp.text.ListItem(cadenaDescriptiva));

                    List ComentarioList = new List(false, 10);
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
                Meses.Add(new iTextSharp.text.ListItem("ITEMS CON COMENTARIOS"));
                Meses.Add((IElement)MesesConComentarios[mesActual]);
                CurrentList.Add(Meses);
            }

            foreach (DateTime mesActual in MesesConComentarios.Keys)
            {
                List Meses = new List(false, 20);
                Meses.Add(new iTextSharp.text.ListItem("ITEMS SIN COMENTARIOS"));
                Meses.Add((IElement)MesesSinComentarios[mesActual]);
                CurrentList.Add(Meses);
            }

            CargoElementos = true;
        }
        else 
        {
            CargoElementos = false;
        }

        return CurrentList;

    }
    #endregion 
    protected void cbo_SelectedIndexChanged(object sender, EventArgs e)
    {
        tblDetalleResumen.Visible = false;
        tblDetalleEnAuditoria.Visible = false;
       
    }

}
