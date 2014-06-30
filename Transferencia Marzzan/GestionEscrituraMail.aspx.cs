using System;
using System.Collections;
using System.Collections.Generic;
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
using Telerik.Web.UI;
using CommonMarzzan;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Script.Services;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

public partial class GestionEscrituraMail : BasePage
{
    public class destinos
    {

        public long Mail { get; set; }
        public long Usuario { get; set; }
        public string Estado { get; set; }
        public string FechaCambioEstado { get; set; }
        public long Ubicacion { get; set; }
    }

    public class TempLibretaContactos
    {
        public string Id { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }

    }

    public bool ReadOnlyEditor
    {
        get;
        set;
    }

    public long IdMail
    {
        get;
        set;
    }

    public string Accion
    {
        get;
        set;
    }

    private Marzzan_InfolegacyDataContext Contexto
    {
        get
        {

            if (Session["Context"] == null)
            {
                Session.Add("Context", new Marzzan_InfolegacyDataContext());
            }

            return (Marzzan_InfolegacyDataContext)Session["Context"];
        }

    }

    protected override void PageLoad()
    {
        if (!this.IsPostBack)
        {
            Session.Add("ListaContactos", new List<TempLibretaContactos>());


            IdMail = long.Parse(Request.QueryString["id"]);

            var mail = (from m in Contexto.MailsCabeceras
                        where m.IdMailCabecera == IdMail
                        select m).FirstOrDefault();

            Accion = Request.QueryString["Accion"];

            #region Visualizacion de Pagina
            MailsFirma FirmaActual = Contexto.MailsFirmas.Where(w => w.Usuario == long.Parse(Session["IdUsuario"].ToString())).FirstOrDefault();

            if (Accion == "Responder")
            {

                txtPara.Text = mail.Cliente.Nombre + ";";
                hdClientes.Value = mail.Cliente.IdCliente.ToString() + ";";

                txtAsunto.Text = "RE: " + mail.Subject;

                if (FirmaActual != null)
                    elm1.InnerText = "<p></p><p style='padding-top:20px;text-align:center'><img src='ImagenesFirma/" + FirmaActual.NombeArchivo + "'  /></p>";

                elm1.InnerText += "<p ><p></p><p></p><p><b>Mensaje enviado originalmente por: " + mail.Cliente.Nombre +
                                ", el día: " + string.Format("{0:dd.MM.yyyy HH:mm}", mail.Fecha) + "</p></b>" +
                             mail.Cuerpo.Replace("<p>", "<p style='color:Gray'>");

                ReadOnlyEditor = false;

                trLecturaMail.Visible = false;
                divLectura.Visible = false;

                trNuevoMail.Visible = true;
                trNuevoMail.Visible = true;
            }
            else if (Accion == "Nuevo")
            {

                txtPara.Text = "";
                txtAsunto.Text = "";

                if (FirmaActual != null)
                    elm1.InnerText = "<p></p><p style='padding-top:20px;text-align:center'><img src='ImagenesFirma/" + FirmaActual.NombeArchivo + "'  /></p>";
                else
                    elm1.InnerText = "";

                ReadOnlyEditor = false;

                trLecturaMail.Visible = false;
                divLectura.Visible = false;

                trNuevoMail.Visible = true;
                trNuevoMail.Visible = true;
            }
            else if (Accion == "Leer")
            {
                ReadOnlyEditor = true;

                MailsDestino estadoMail = mail.MailsDestinos.FirstOrDefault(w => w.Usuario == long.Parse(Session["IdUsuario"].ToString()));
                elm1.Value = mail.Cuerpo;
                lblAsunto.Text = "ASUNTO DEL MENSAJE: " + mail.Subject;

                /// Si hay estado de mail es porque el usuario logeado es el destino del mail
                /// por lo que se esta leyendo un mail recivido.
                if (estadoMail != null)
                {
                    lblOrigen.Text = mail.Cliente.Nombre;
                    lblFechaHora.Text = string.Format("{0:dd.MM.yyyy HH:mm}", mail.Fecha);
                    lblTelefono.Text = "Tel: " + mail.Cliente.Telefono;

                    if (estadoMail.Estado == EstadosMails.SINLEER)
                    {
                        /// Cambio el estado del mail a leido
                        estadoMail.Estado = EstadosMails.LEIDO;
                        estadoMail.FechaCambioEstado = DateTime.Now;
                        Contexto.SubmitChanges();
                    }
                }
                /// Se trata de la lectura de un mail enviado por el usuario logeado.
                else
                {
                    lblOrigen.Text = string.Join(";", mail.MailsDestinos.Select(w => w.Cliente.Nombre).ToArray());
                    lblFechaHora.Text = string.Format("{0:dd.MM.yyyy HH:mm}", mail.Fecha);
                    lblTelefono.Visible = false;
                    btnResponder.Visible = false;
                    lblResponder.Visible = false;
                }

                trLecturaMail.Visible = true;
                divLectura.Visible = true;

                trNuevoMail.Visible = false;
                divEnvio.Visible = false;
            }
            #endregion

            #region Carga de link para adjuntos

            if (Accion == "Leer")
            {
                /// 1. Recuperar y mostrar los link de los adjuntos
                /// 2. reemplazar y mover las imagenes insertadas en el cuerpo del mail.

                string ruta = "ArchivosMail\\@" + mail.Cliente.CodigoExterno + "-" + mail.IdMailCabecera.ToString() + "\\";
                string ImagenArchivo = "";
                foreach (MailsAttach attach in mail.MailsAttaches)
                {
                    string nombreAbreviado = attach.Archivo.Length > 12 ? attach.Archivo.Substring(0, 12) + "..." : attach.Archivo;
                    ImagenArchivo = ResolverImagen(attach.Extension);
                    divAdjuntos.InnerHtml += "<div ID='btnAttach' " + attach.IdMailAttach.ToString() + @" onclick='AbrirArchivo(""" + ruta + "\\" + attach.Archivo + @"""); return false;' class='Adjunto' >" +
                                             "<img width='24px' height='24px' style='border:0px;vertical-align:middle;' alt='' src='Imagenes/" + ImagenArchivo + "' />" + nombreAbreviado +
                                             "</div>";
                }
            }
            #endregion

            #region Carga de Contactos
            
            Session.Add("UsuarioInvalidos", new List<long>());

            if (Accion == "Nuevo" || Accion == "Responder")
            {
                Cliente cliente = (from C in Contexto.Clientes
                                   where C.IdCliente == long.Parse(Session["IdUsuario"].ToString())
                                   select C).Single<Cliente>();

                /// Segun el tipo de cliente es con lo que carga la libreta de contactos
                if (cliente.TipoCliente.ToUpper() != TipoClientes.Consultor.ToString().ToUpper()
                    && !cliente.Clasif1.ToUpper().StartsWith("WEB"))
                {
                    /// Cuando se tranta de un cliente que no es interno cargo los grupos validos para el usaurio en los cuales
                    /// se buscaran los clientes a los que se les puede enviar mail.
                    
                    /// 1. Busco  los grupos partiendo del grupo del cliente logeado.
                    var GruposValidos = Helper.ObtenerGruposSubordinados(Session["GrupoCliente"].ToString());
                    
                    ///2. Agrego el grupo de asistente, que representa el asistente que lo atiende segun la conf de mail.
                    GruposValidos.Add("ASISTENTE");

                    
                    ///3. Esto es para que los lidesres puedan ver a usuarios internos especificos. Es decir que todos los clietes internos
                    /// que sean del grupo "WEB-LIDERES" se mostraran en la lista de clientes.
                    GruposValidos.Add("WEB-LIDERES");

                    ///4. Cargo la variable de session con la informacion.
                    Session.Add("GruposValidos", GruposValidos);

                    /// 5. Esta variable es para excluir usuario especificos, por ejemplo 35026 es el usuario
                    /// de gerencia, para los casos de consultores y lideres no debe aparecer en la lista de
                    /// contactos.
                    Session.Add("UsuarioInvalidos", new List<long>() { 35026 });


                    /// 6. Cargo la variable de session con los grupos para mostrar en la parte de GRUPOS.
                    Session.Add("DatosGruposAshx", GruposValidos.Where(w => !w.Contains("WEB-")).Select(w => new View_Grupo { Grupo = w }).ToList());

                }
                else if (cliente.Clasif1.ToUpper().StartsWith("WEB"))
                {
                    /// AL DEJAR LAS DOS VARIABLES VACIAS, NO SE APLICA NINGUN FILTRO POR EL QUE EL USUARIO VE TODOS LOS CLIENTES Y TODOS GRUPO.
                    /// ADEMAS SE AGREGAN DOS GRUPOS MAS LIDERES Y REVENDEDORES PARA PODER HACER ENVIO MASIVO (ESTO SE HACE EN EL ASCX DE GRUPOS)

                    Session.Add("GruposValidos", null);
                    Session.Add("DatosGruposAshx", null);

                }
                else
                {
                    if (Accion == "Nuevo")
                    {
                        Cliente Lider = (from C in Contexto.Clientes
                                         where C.Clasif1 == cliente.Clasif1
                                         && C.TipoCliente != TipoClientes.Consultor.ToString()
                                         select C).FirstOrDefault<Cliente>();

                        btnPara.Visible = false;
                        txtPara.Text = "Lider: " + Lider.Nombre;
                        hdClientes.Value = Lider.IdCliente.ToString() + ";";
                    }
                    else if (Accion == "Responder")
                    {
                        btnPara.Visible = false;
                    }

                }
            }
            #endregion

        }
    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {

        /// Cambio el estado del mail original a respondido
        long IdMailOrigen = long.Parse(Request.QueryString["id"]);

        var mailOrigen = (from m in Contexto.MailsCabeceras
                          where m.IdMailCabecera == IdMailOrigen
                          select m).FirstOrDefault();

        if (mailOrigen != null)
        {
            MailsDestino estadoMail = mailOrigen.MailsDestinos.FirstOrDefault(w => w.Usuario == long.Parse(Session["IdUsuario"].ToString()));
            estadoMail.Estado = EstadosMails.RESPONDIDO;
        }


        ////// genero el nuevo mail
        string cuerpo = e.Argument.Split('|')[0];
        string[] archivosAdjuntos = e.Argument.Split('|')[1].Split(',');
        string[] archivosInsertados = e.Argument.Split('|')[2].Split(',');

        string asunto = txtAsunto.Text;
        List<string> grupos = hdGrupos.Value.Split(';').ToList();
        List<string> clientes = hdClientes.Value.Split(';').ToList();
        bool generarMailMasivoRevendedores = false;


        MailsCabecera mail = new MailsCabecera();
        mail.Cuerpo = cuerpo;
        mail.Fecha = DateTime.Now;
        mail.Subject = asunto;
        mail.Usuario = long.Parse(Session["IdUsuario"].ToString());

        /// Si IdMailOrigen es mayor a 0 es porque se esta respondiendo un mail,
        /// entoces al nuevo mail que estoy generando le indico cual fue su origen.
        if (IdMailOrigen > 0)
        {
            mail.MailOrigen = IdMailOrigen;
        }


        /// Agrego cada uno de los clientes destinos seleccionados
        foreach (var item in clientes.Distinct())
        {
            if (item != "")
            {
                MailsDestino mDestino = new MailsDestino();
                mDestino.MailsCabecera = mail;
                mDestino.Usuario = long.Parse(item);
                mDestino.Estado = EstadosMails.SINLEER;
                mDestino.FechaCambioEstado = DateTime.Now;
                mail.MailsDestinos.Add(mDestino);
            }
        }

        foreach (string grupo in grupos)
        {
            if (grupo != "" && grupo != "ASISTENTE" && grupo != "LIDERES" && grupo != "REVENDEDORES")
            {
                List<Cliente> subordinados = Helper.ObtenerConsultoresSubordinadosDirectos(grupo);
                foreach (Cliente cliente in subordinados.Distinct().Where(w => w.IdCliente != long.Parse(Session["IdUsuario"].ToString())))
                {
                    // Verifico que no se haya agregado el cliente por medio del grupo e individualmente
                    if (!mail.MailsDestinos.Any(w => w.Usuario.Value == cliente.IdCliente))
                    {
                        MailsDestino mDestino = new MailsDestino();
                        mDestino.MailsCabecera = mail;
                        mDestino.Usuario = cliente.IdCliente;
                        mDestino.Estado = EstadosMails.SINLEER;
                        mDestino.FechaCambioEstado = DateTime.Now;
                        mail.MailsDestinos.Add(mDestino);
                    }
                }
            }
            else if (grupo == "ASISTENTE")
            {
                Cliente asistente = ObtenerAsistenteAsignado();
                if (asistente != null)
                {
                    MailsDestino mDestino = new MailsDestino();
                    mDestino.MailsCabecera = mail;
                    mDestino.Usuario = asistente.IdCliente;
                    mDestino.Estado = EstadosMails.SINLEER;
                    mDestino.FechaCambioEstado = DateTime.Now;
                    mail.MailsDestinos.Add(mDestino);
                }
            }
            else if (grupo == "LIDERES")
            {
                List<long> lideres = ObtenerLideres();
                if (lideres.Count> 0)
                {
                    foreach (var id in lideres)
                    {

                        MailsDestino mDestino = new MailsDestino();
                        mDestino.MailsCabecera = mail;
                        mDestino.Usuario = id;
                        mDestino.Estado = EstadosMails.SINLEER;
                        mDestino.FechaCambioEstado = DateTime.Now;
                        mail.MailsDestinos.Add(mDestino);
                    }
                }
            }
            else if (grupo == "REVENDEDORES")
            {
                generarMailMasivoRevendedores = true;
                
                //if (lideres.Count> 0)
                //{
                //    foreach (var id in lideres)
                //    {

                //        MailsDestino mDestino = new MailsDestino();
                //        mDestino.MailsCabecera = mail;
                //        mDestino.Usuario = id;
                //        mDestino.Estado = EstadosMails.SINLEER;
                //        mDestino.FechaCambioEstado = DateTime.Now;
                //        mail.MailsDestinos.Add(mDestino);
                //    }
                //}
            }

            

        }


        /// Guardo en la base de datos los arvhivos adjuntos al mail.
        foreach (string adjunto in archivosAdjuntos)
        {
            if (adjunto != "")
            {
                MailsAttach mAttach = new MailsAttach();
                mAttach.MailsCabecera = mail;
                mAttach.Extension = adjunto.Substring(adjunto.LastIndexOf("."));
                mAttach.Archivo = adjunto;
                mail.MailsAttaches.Add(mAttach);
            }
        }


        Contexto.MailsCabeceras.InsertOnSubmit(mail);
        Contexto.SubmitChanges();

        /// Esto es para el caso del mail masivos
        if (generarMailMasivoRevendedores)
        {
            string xmlRevendedores = ObtenerXmlRevendedores(mail.IdMailCabecera);
            Contexto.xmlToMailsDestinos(xmlRevendedores);
        }

        /// Despues de grabar deberia mover los archivos de la carpeta temporal
        /// a la carpeta final, que esta compuesta por:
        /// nro interno de mail + Nro cliente de quien envia el mail 


        /// Realizo un retarde de 3 segundo para que todos los datos se guarden y se 
        /// pueda recuperar los valores necesarios.
        //System.Threading.Thread.Sleep(3000);

        string nombreCarpeta = mail.Cliente.CodigoExterno + "-" + mail.IdMailCabecera.ToString();
        string rutaFinal = Server.MapPath("ArchivosMail") + "\\" + nombreCarpeta;

        System.IO.DirectoryInfo dirDestino = new System.IO.DirectoryInfo(Server.MapPath("ArchivosMail"));
        System.IO.DirectoryInfo dirTemp = new System.IO.DirectoryInfo(Server.MapPath("TempImgMail"));

        /// Creo el directorio donde se van a colocar los archivos
        dirDestino.CreateSubdirectory(nombreCarpeta);

        foreach (string adjunto in archivosAdjuntos)
        {
            /// Recupero el archivo que se va a mover
            System.IO.FileInfo arcAdjunto = dirTemp.GetFiles().Where(w => w.Name == adjunto).FirstOrDefault();

            if (arcAdjunto != null)
            {
                /// Muevo el archivo
                arcAdjunto.MoveTo(rutaFinal + "/" + adjunto);
            }
        }


        /// Igual para las imagenes insertadas en el cuerpo del mail.
        /// Muevo los archivos a la carpeta destino y reemplazo los link
        /// en el mail.
        foreach (string insert in archivosInsertados)
        {
            string nombreArchivo = insert.Substring(insert.LastIndexOf('/') + 1);
            /// Recupero el archivo que se va a mover
            System.IO.FileInfo arcInsert = dirTemp.GetFiles().Where(w => w.Name == nombreArchivo).FirstOrDefault();

            if (arcInsert != null)
            {
                /// Muevo el archivo
                arcInsert.MoveTo(rutaFinal + "/" + nombreArchivo);
            }

            if (insert != "")
            {
                /// Distintas alternativas de como aparece el nombre de la imagen en el text box
                mail.Cuerpo = mail.Cuerpo.Replace(".." + insert, @"ArchivosMail/" + nombreCarpeta + @"/" + nombreArchivo);
                mail.Cuerpo = mail.Cuerpo.Replace(insert, @"ArchivosMail/" + nombreCarpeta + @"/" + nombreArchivo);
                mail.Cuerpo = mail.Cuerpo.Replace(@"TempImgMail/" + nombreArchivo, @"ArchivosMail/" + nombreCarpeta + @"/" + nombreArchivo);

            }

        }

        Contexto.SubmitChanges();

        HelperReglasMails.GestionarReglaMoverMailSegunOrigen(mail.Usuario.Value, mail.MailsDestinos.ToList(), Contexto);

    }


    private string ResolverImagen(string ext)
    {
        switch (ext)
        {
            case ".pdf":
                return "pdf_24x24.png";
            case ".xls":
            case ".xlsx":
                return "Excel_24x24.gif";
            case ".doc":
            case ".docx":
                return "Word_24x24.png";
            case ".rar":
                return "Rar_24x24.png";
            default:
                return "Image_24x24.png";
        }

    }

    private Cliente ObtenerAsistenteAsignado()
    {
        string mailAsistente = (from C in Contexto.ConfMails
                                where C.Consultor.Value == long.Parse(Session["IdUsuario"].ToString())
                                select C.EmailDestino).Single<string>();

        Cliente asistente = (from C in Contexto.Clientes
                             where C.Email == mailAsistente
                             && C.TipoCliente == "INTERNO"
                             select C).FirstOrDefault<Cliente>();
        return asistente;
    }

    private string ObtenerXmlRevendedores(long idMailCabecera)
    {

        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {
            // Generacion del xml a partir de una lista de clientes, los cuales deben haber hecho por lo menos un pedido
            // en los ultimas 6 meses.
            DateTime fechaDesde = DateTime.Now.AddMonths(-6);
            
            List<destinos> allDestino = (from c in dc.CabeceraPedidos
                                         where c.FechaPedido >= fechaDesde && c.objCliente.Habilitado.Value && c.objCliente.CodTipoCliente == "1"
                                         select new destinos
                                         {
                                             Mail = idMailCabecera,
                                             Usuario = c.objCliente.IdCliente,
                                             Estado = EstadosMails.SINLEER,
                                             FechaCambioEstado = string.Format("{0:yyyy-MM-dd}", DateTime.Now)
                                         }).Distinct().ToList();

            //// Generacion del xml a partir de una lista de destinos.
            //List<destinos> allDestino = (from c in dc.Clientes
            //                             where c.Habilitado.Value && c.CodTipoCliente == "1"
            //                             select new destinos
            //                             {
            //                                 Mail = idMailCabecera,
            //                                 Usuario = c.IdCliente,
            //                                 Estado = EstadosMails.SINLEER,
            //                                 FechaCambioEstado = DateTime.Now.ToShortDateString()
            //                             }).ToList();


            XmlDocument xmlDoc = new XmlDocument();
            XPathNavigator nav = xmlDoc.CreateNavigator();
            using (XmlWriter writer = nav.AppendChild())
            {
                XmlSerializer ser = new XmlSerializer(allDestino.GetType(), new XmlRootAttribute("root"));
                ser.Serialize(writer, allDestino);
            }

            return xmlDoc.InnerXml;

        }
    
    }

    private List<long> ObtenerLideres()
    {
        string tipoConsultor = TipoClientes.Consultor.ToString();
        string tipoPotencial = TipoClientes.PotencialBolso.ToString();

        return (from C in Contexto.Clientes
                where C.TipoCliente != tipoConsultor &&
                C.TipoCliente != tipoPotencial  &&
                !C.Clasif1.Contains("WEB") &&
                !C.Nombre.Contains("POTENCIAL") && 
                C.Habilitado.Value
                select C.IdCliente).ToList<long>();

    }

    [WebMethod(EnableSession = true)]
    public static object AgregarContacto(string id, string Nombre, string tipo)
    {
        List<TempLibretaContactos> contactosSeleccionados = (List<TempLibretaContactos>)HttpContext.Current.Session["ListaContactos"];

        TempLibretaContactos cont = new TempLibretaContactos();
        cont.Id = id;
        cont.Tipo = tipo;
        cont.Nombre = Nombre;
        contactosSeleccionados.Add(cont);


        return contactosSeleccionados.ToList();

    }

    [WebMethod(EnableSession = true)]
    public static object EliminarContacto(string id)
    {
        List<TempLibretaContactos> contactosSeleccionados = (List<TempLibretaContactos>)HttpContext.Current.Session["ListaContactos"];
        contactosSeleccionados.RemoveAll(w => w.Id == id);

        return contactosSeleccionados.ToList();

    }
}
