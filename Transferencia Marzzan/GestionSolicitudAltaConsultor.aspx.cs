using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonMarzzan;
using Telerik.Web.UI;

public partial class GestionSolicitudAltaConsultor : BasePage
{
    private bool _clienteExistente = false;
    private long? _idAsistenteResponsable;
    private long _idLogistica = 35357;

    private Dictionary<string, string> _mensajeClienteExistente;

    private static CommonMarzzan.Parametro confMail = null;


    protected override void PageLoad()
    {
        Session.Timeout = 30;
        _clienteExistente = false;
        _mensajeClienteExistente = new Dictionary<string, string>();

        if (!IsPostBack)
        {
            Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();

            confMail = (from C in dc.Parametros
                        where C.Tipo == "ConfMail"
                        select C).First<Parametro>();

            gvDirEntrega.DataSource = new List<Direccione>();
            gvDirEntrega.DataBind();

            lblCoordinadora.Text = Session["NombreUsuario"].ToString();
            Session.Add("DireccionesxSolicitud", new List<Direccione>());

            if (GetDireccionDestino() == "")
                tblPrincipal.Disabled = true;
            else
            {
                tblPrincipal.Disabled = false;
                ToolTipDireccion.VisibleOnPageLoad = false;
            }


            /// Carga de los grupos según el usuario logeado
            Cliente cliente = (from C in dc.Clientes
                               where C.IdCliente == long.Parse(Session["IdUsuario"].ToString())
                               select C).FirstOrDefault<Cliente>();

            foreach (string item in Helper.ObtenerGruposSubordinados(cliente))
            {
                if (item != null && !item.Contains("INACTIVO"))
                    cboGrupos.Items.Add(new RadComboBoxItem(item, item));
            }

            Session["DivisionesPoliticas"] = (from d in dc.DivisionesPoliticas
                                              select d).ToList();



            cboConsultores.AppendDataBoundItems = true;
            //cboConsultores.Items.Add(new RadComboBoxItem("Seleccione un referente", "0"));
            cboConsultores.DataTextField = "Nombre";
            cboConsultores.DataValueField = "CodigoExterno";
            cboConsultores.DataSource = Helper.ObtenerConsultoresSubordinados((Cliente)cliente); ;
            cboConsultores.DataBind();
            //cboConsultores.SelectedIndex = 0;

        }

    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {

        if (e.Argument != "undefined")
        {
            gvDirEntrega.DataSource = (Session["DireccionesxSolicitud"] as List<Direccione>);
            gvDirEntrega.DataBind();
            upDirecciones.Update();
        }
    }

    private void EnviarMailSolicitud(Marzzan_InfolegacyDataContext dc, long idCliente, string grupo, string asunto, string cuerpo)
    {

        if (grupo != "")
        {
            if (Helper.ObtenerLider(idCliente, grupo) > 0)
            {
                MailsCabecera mail = new MailsCabecera();
                mail.Cuerpo = cuerpo;
                mail.Fecha = DateTime.Now;
                mail.Subject = asunto;
                mail.Usuario = long.Parse(Session["IdUsuario"].ToString());



                MailsDestino mDestino = new MailsDestino();
                mDestino.MailsCabecera = mail;
                mDestino.Usuario = Helper.ObtenerLider(idCliente, grupo);
                mDestino.Estado = EstadosMails.SINLEER;
                mDestino.FechaCambioEstado = DateTime.Now;
                mail.MailsDestinos.Add(mDestino);


                dc.MailsCabeceras.InsertOnSubmit(mail);
            }
        }
        else
        {
            MailsCabecera mail = new MailsCabecera();
            mail.Cuerpo = cuerpo;
            mail.Fecha = DateTime.Now;
            mail.Subject = asunto;
            mail.Usuario = long.Parse(Session["IdUsuario"].ToString());



            MailsDestino mDestino = new MailsDestino();
            mDestino.MailsCabecera = mail;
            mDestino.Usuario = idCliente;
            mDestino.Estado = EstadosMails.SINLEER;
            mDestino.FechaCambioEstado = DateTime.Now;
            mail.MailsDestinos.Add(mDestino);


            dc.MailsCabeceras.InsertOnSubmit(mail);
        }

    }

    protected void btnAlta_Click(object sender, EventArgs e)
    {
        string body = "";
        string Subject = "";
        SolicitudesAlta newSolicitud = null;



        try
        {
            /// Para que se puedan hacer pruebas con el usuario DEMO
            if (long.Parse(Session["IdUsuario"].ToString()) != 0)
            {
                /// Genero el cuerpo y el nuevos cliente en la web
                body = GetHTMLNuevoConsultor(out newSolicitud);

                using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
                {
                    #region Recupero el asistente que atiende al lider logeado

                    var mailAsistente = (from d in dc.ConfMails
                                         where d.Consultor == long.Parse(Session["IdUsuario"].ToString())
                                         select d.EmailDestino).FirstOrDefault();

                    if (mailAsistente == null)
                    {
                        _idAsistenteResponsable = (from d in dc.Clientes
                                                   where d.Email.ToLower() == "asistente1@sandramarzzan.com.ar"
                                                   && d.TipoCliente == "INTERNO"
                                                   select d.IdCliente).FirstOrDefault();
                    }
                    else
                    {
                        _idAsistenteResponsable = (from d in dc.Clientes
                                                   where d.Email.ToLower() == mailAsistente
                                                    && d.TipoCliente == "INTERNO"
                                                   select d.IdCliente).FirstOrDefault();
                    }

                    #endregion

                    using (Marzzan_BejermanDataContext dcB = new Marzzan_BejermanDataContext())
                    {
                        /// Genero el nuevo cliente en bejerman y verifico si el mismo existe o no 
                        /// en la base de datos.
                        Clientes cliBejerman = GenerarClienteBejerman(dcB);


                        /// Si el cliente es existente solo se da de alta la solicitud en la web
                        /// pero no en bejerman.
                        if (_clienteExistente)
                        {
                            newSolicitud.EsRepetido = true;
                            newSolicitud.MensajeRepetido = _mensajeClienteExistente["Mensaje"];
                            newSolicitud.CodigosExistentes = _mensajeClienteExistente["Codigos"];

                            if (!_mensajeClienteExistente["Mensaje"].Contains("solicitudes"))
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnvioCorrecto", "AvisoClienteExistente();", true);
                                EnviarMailSolicitud(dc, long.Parse(Session["IdUsuario"].ToString()), cboGrupos.SelectedValue, "Solicitud Alta - Cliente Existente: " + txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper(), "La solicitud no se completo, ya que el revendedor existe en la base de datos de clientes. Por favor comuníquese con Asistencia Comercial.");

                                if (_idAsistenteResponsable != null)
                                    EnviarMailSolicitud(dc, _idAsistenteResponsable.Value, cboGrupos.SelectedValue, "Solicitud Alta - Cliente Existente: " + txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper(), "La solicitud no se completo, ya que el revendedor existe en la base de datos de clientes. Por favor comuníquese con Asistencia Comercial.");

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnvioCorrecto", "AvisoClienteExistenteSolicitud();", true);
                                EnviarMailSolicitud(dc, long.Parse(Session["IdUsuario"].ToString()), cboGrupos.SelectedValue, "Solicitud Alta - Solicitud Repetida: " + txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper(), "La solicitud no se completo, ya que el revendedor posee una SOLICITUD DE ALTA PREVIA. Por favor comuníquese con Asistencia Comercial.");

                                if (_idAsistenteResponsable != null)
                                    EnviarMailSolicitud(dc, _idAsistenteResponsable.Value, cboGrupos.SelectedValue, "Solicitud Alta - Solicitud Repetida: " + txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper(), "La solicitud no se completo, ya que el revendedor posee una SOLICITUD DE ALTA PREVIA. Por favor comuníquese con Asistencia Comercial.");
                            }
                        }
                        else
                        {
                            newSolicitud.CodigoBejerman = cliBejerman.cli_Cod;
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnvioCorrecto", "ConfirmacionEnvio();", true);

                            /// 1. Evio el mail a la consola del usuario LOGEADO para informar la confirmación del alta.
                            EnviarMailSolicitud(dc, long.Parse(Session["IdUsuario"].ToString()), cboGrupos.SelectedValue, "Solicitud Alta - Solicitud Exitosa: " + txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper(), "La solicitud se completo en forma exitosa, a la brevedad el revendedor estará disponible para ser utilizado.");

                            //2. Evio de mail a la consola del usuario de LOGISTICA para informar el alta SIN TRASNPORTE.
                            if (cliBejerman.clitrn_Cod == null && _idLogistica != null)
                            {
                                EnviarMailSolicitud(dc, _idLogistica, "", "Alta Sin Transporte", "El Líder " + lblCoordinadora.Text + " ha realizado un alta satisfactoria para: <b>" + cliBejerman.cli_Cod + " - " + txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper() + "</b>, pero la misma no posee un TRANSPORTE para la localidad seleccionada: '<b>" + txtNuevo_Depart_loc.Text + "'</b>, se solicita que verifique los datos de LOCALIDAD de la misma.");
                            }

                            /// 3. Evio de mail a la consola del ASISTENTE para informar el alta correcta.
                            if (_idAsistenteResponsable != null)
                                EnviarMailSolicitud(dc, _idAsistenteResponsable.Value, cboGrupos.SelectedValue, "Solicitud Alta - Solicitud Satisfactoria: " + txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper(), "La solicitud se completo correctamente - Documento nuevo revendedor:" + txtDni.Text);


                        }


                        dc.SolicitudesAltas.InsertOnSubmit(newSolicitud);
                        dc.SubmitChanges();

                    }

                    // Envio de mail fisico al asistente que asiste al lider logeado.
                    Subject = "Solicitud Alta Nuevo Cosnultor";
                    string DireccionDestino = GetDireccionDestino();
                    Helper.EnvioMailSolicitudAlta(body, DireccionDestino, confMail.ColHijos[0].Valor, Subject, confMail.ColHijos[3].Valor, confMail.ColHijos[1].Valor, confMail.ColHijos[2].Valor);

                }

                Session.Remove("DireccionesxSolicitud");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "AvisoOk", "AvisoOk();", true);
            }

        }
        catch (Exception err)
        {
            StreamWriter _sw = null;
            _sw = new StreamWriter(Server.MapPath("") + "\\logAltaClientes.txt", true);

            _sw.WriteLine("----------------------------------  " + DateTime.Now.ToString() + "  -------------------------------------------");
            _sw.WriteLine(" Líder que solicita el alta: " + lblCoordinadora.Text);
            _sw.WriteLine("--------- Datos Ingresados --------------");
            _sw.WriteLine(body);
            _sw.WriteLine("--------- Detalle Error --------------");
            _sw.WriteLine(err.Message);

            _sw.WriteLine("--------- Detalle Inner --------------");
            if (err.InnerException != null)
                _sw.WriteLine(err.InnerException.Message);

            _sw.WriteLine("--------- Detalle Stack --------------");
            if (err.StackTrace != null)
                _sw.WriteLine(err.StackTrace);

            _sw.WriteLine("-----------------------------------------------------------------------------");
            _sw.Close();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "AvisoErrorss", "AvisoError();", true);
        }
    }

    private Clientes GenerarClienteBejerman(Marzzan_BejermanDataContext dcB)
    {
        /// Busco primero en la tabla clientes CRM de Bejerman
        _mensajeClienteExistente = Helper.GetCoincidenciasClientesCRM(txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper(), txtDni.Text, txtEmail.Text, txtTelFijo.Text, txtTelCel.Text);

        if (_mensajeClienteExistente.Count == 0)
        {
            /// Busco primero en la tabla clientes de la web
            _mensajeClienteExistente = Helper.GetCoincidenciasCliente(txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper(), txtDni.Text, txtEmail.Text, txtTelFijo.Text, txtTelCel.Text);
        }

        if (_mensajeClienteExistente.Count == 0)
        {
            // Busco en la tabla de solicitudes de alta de la web
            _mensajeClienteExistente = Helper.GetCoincidenciasClienteSolicitudesAltas(txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper(), txtDni.Text, txtEmail.Text, txtTelFijo.Text, txtTelCel.Text);
        }

        if (_mensajeClienteExistente.Count == 0)
        {
            #region Genero todos los datos principales de Clientes y ClientesCRM
            Clientes newClienteB = new Clientes();
            ClientesCRM newClienteBCRM = new ClientesCRM();


            string maxIdCrm = dcB.ClientesCRMs.Max(w => w.clr_Cod);
            string maxIdCli = dcB.Clientes.Max(w => w.cli_Cod);
            long maxId = long.Parse(maxIdCrm) > long.Parse(maxIdCli) ? long.Parse(maxIdCrm) : long.Parse(maxIdCli);

            newClienteB.cli_Cod = string.Format("{0:000000}", maxId + 1);
            //newClienteBCRM.clr_Cod = string.Format("{0:000000}", long.Parse(maxIdCrm) + 1);
            newClienteBCRM.clr_Cod = string.Format("{0:000000}", maxId + 1);


            newClienteB.cli_RazSoc = txtApellido.Text.ToUpper().Trim() + " " + txtNombres.Text.ToUpper().Trim();
            if (newClienteB.cli_RazSoc.Length <= 35)
                newClienteB.cli_RazSoc += "/ALTA";
            else if (newClienteB.cli_RazSoc.Length <= 38)
                newClienteB.cli_RazSoc += "/A";


            newClienteBCRM.clr_RazSoc = txtApellido.Text.ToUpper().Trim() + " " + txtNombres.Text.ToUpper().Trim();
            if (newClienteBCRM.clr_RazSoc.Length <= 35)
                newClienteBCRM.clr_RazSoc += "/ALTA";
            else if (newClienteBCRM.clr_RazSoc.Length <= 38)
                newClienteBCRM.clr_RazSoc += "/A";




            newClienteB.cli_NomFantasia = "";
            newClienteB.cli_Tel = txtTelFijo.Text;
            newClienteB.cli_Fax = txtTelCel.Text;
            newClienteB.cli_EMail = txtEmail.Text;
            newClienteB.cli_Modem = "";
            newClienteB.cli_CodPos = txtCodigoPostal.Text;

            newClienteBCRM.clr_NomFantasia = "";
            newClienteBCRM.clr_Tel = txtTelFijo.Text;
            newClienteBCRM.clr_Fax = txtTelCel.Text;
            newClienteBCRM.clr_EMail = txtEmail.Text;
            newClienteBCRM.clr_Modem = "";
            newClienteBCRM.clr_CodPos = txtCodigoPostal.Text;


            /// Utilizo la localidad que se cargo.
            string localidadDepartamento = txtNuevo_Depart_loc.Text.Trim().ToUpper() == "" ? txtdepartamento.Text.Trim().ToUpper() : txtNuevo_Depart_loc.Text.Trim().ToUpper();
            newClienteB.cli_Direc = txtDireccion.Text.ToUpper();
            if (localidadDepartamento.Length > 25)
                newClienteB.cli_Loc = localidadDepartamento.Substring(0, 25);
            else
                newClienteB.cli_Loc = localidadDepartamento;

            newClienteB.cliprv_Codigo = dcB.prvs.Where(w => w.prv_descrip.ToLower() == cboProvincias.SelectedItem.Text.ToLower()).FirstOrDefault().prv_codigo;


            newClienteBCRM.clr_Direc = txtDireccion.Text.ToUpper();
            if (localidadDepartamento.Length > 25)
                newClienteBCRM.clr_Loc = localidadDepartamento.Substring(0, 25);
            else
                newClienteBCRM.clr_Loc = localidadDepartamento;

            newClienteBCRM.clrprv_Codigo = dcB.prvs.Where(w => w.prv_descrip.ToLower() == cboProvincias.SelectedItem.Text.ToLower()).FirstOrDefault().prv_codigo;



            if (cboCondicionIVA.SelectedValue == "ConsumidorFinal")
            {
                newClienteB.clisiv_Cod = Convert.ToChar("3");
                newClienteBCRM.clrsiv_Cod = Convert.ToChar("3");

                newClienteB.clisig_Cod = Convert.ToChar(((int)TipoGanacia.NoAlcanzado).ToString());
                newClienteBCRM.clrsig_Cod = Convert.ToChar(((int)TipoGanacia.NoAlcanzado).ToString());
            }
            else
            {
                TipoIVA tipoIva = (TipoIVA)Enum.Parse(typeof(TipoIVA), cboCondicionIVA.SelectedValue);
                newClienteB.clisiv_Cod = Convert.ToChar(((int)tipoIva).ToString());
                newClienteBCRM.clrsiv_Cod = Convert.ToChar(((int)tipoIva).ToString());

                switch (tipoIva)
                {
                    case TipoIVA.Inscripto:
                        newClienteB.clisig_Cod = Convert.ToChar(((int)TipoGanacia.Inscripto).ToString());
                        newClienteBCRM.clrsig_Cod = Convert.ToChar(((int)TipoGanacia.Inscripto).ToString());
                        break;
                    case TipoIVA.Monotributista:
                        newClienteB.clisig_Cod = Convert.ToChar(((int)TipoGanacia.NoAlcanzado).ToString());
                        newClienteBCRM.clrsig_Cod = Convert.ToChar(((int)TipoGanacia.NoAlcanzado).ToString());
                        break;
                    case TipoIVA.NoCategorizado:
                        newClienteB.clisig_Cod = Convert.ToChar(((int)TipoGanacia.NoInscripto).ToString());
                        newClienteBCRM.clrsig_Cod = Convert.ToChar(((int)TipoGanacia.NoInscripto).ToString());
                        break;
                    default:
                        break;
                }
            }



            newClienteB.cli_CUIT = txtCuit.Text.Replace("-", "");
            newClienteB.clisib_Cod = char.Parse(cboIB.SelectedValue);
            newClienteB.cli_NroIB = txtNroIb.Text.Trim();
            newClienteB.cli_Habilitado = false;
            newClienteB.clidlp_Cod = null;
            newClienteB.clidco_Cod = cboProvincias.Text.ToLower().Contains("tierra del fuego") ? "03" : null;
            newClienteB.clizon_Cod = null;
            newClienteB.cliven_Cod = Session["CodigoVendedor"].ToString();
            newClienteB.clicvt_Cod = "CON";


            newClienteBCRM.clr_CUIT = txtCuit.Text.Replace("-", "");
            newClienteBCRM.clrsib_Cod = char.Parse(cboIB.SelectedValue);
            newClienteBCRM.clr_NroIB = txtNroIb.Text.Trim();
            newClienteBCRM.clr_Habilitado = false;
            newClienteBCRM.clrdlp_Cod = null;
            newClienteBCRM.clrdco_Cod = cboProvincias.Text.ToLower().Contains("tierra del fuego") ? "03" : null;
            newClienteBCRM.clrzon_Cod = null;
            newClienteBCRM.clrven_Cod = Session["CodigoVendedor"].ToString();
            newClienteBCRM.clrcvt_Cod = "CON";



            newClienteB.clidc1_Cod = dcB.Defi1Clis.Where(w => w.dc1_Desc.ToLower() == cboGrupos.Text.ToLower()).First().dc1_Cod;
            newClienteB.clidc2_Cod = null;

            //if ((cboConsultores.Text + " - " + txtPremioPorPresentacion.Text).Length > 40)
            //    newClienteB.cli_Contacto = (cboConsultores.Text + " - " + txtPremioPorPresentacion.Text).Substring(0, 40);
            //else
            //    newClienteB.cli_Contacto = cboConsultores.Text + " - " + txtPremioPorPresentacion.Text;

            if ((cboConsultores.Text).Length > 40)
                newClienteB.cli_Contacto = (cboConsultores.Text).Substring(0, 40);
            else
                newClienteB.cli_Contacto = cboConsultores.Text;

            newClienteB.cli_RespPago = "";
            newClienteB.cli_LugarPago = "";
            newClienteB.cli_HorarioPago = "";
            newClienteB.clidep_Cod = null;
            newClienteB.clitdc_Cod = 1;


            if (dcB.Defi1CliCRMs.Where(w => w.dr1_Desc.ToLower() == cboGrupos.Text.ToLower()).FirstOrDefault() != null)
                newClienteBCRM.clrdr1_Cod = dcB.Defi1CliCRMs.Where(w => w.dr1_Desc.ToLower() == cboGrupos.Text.ToLower()).FirstOrDefault().dr1_Cod;
            else
                newClienteBCRM.clrdr1_Cod = null;

            newClienteBCRM.clrdr2_Cod = null;

            //if ((cboConsultores.Text + " - " + txtPremioPorPresentacion.Text).Length > 40)
            //    newClienteBCRM.clr_Contacto = (cboConsultores.Text + " - " + txtPremioPorPresentacion.Text).Substring(0, 40);
            //else
            //    newClienteBCRM.clr_Contacto = cboConsultores.Text + " - " + txtPremioPorPresentacion.Text;

            if ((cboConsultores.Text).Length > 40)
                newClienteBCRM.clr_Contacto = (cboConsultores.Text).Substring(0, 40);
            else
                newClienteBCRM.clr_Contacto = cboConsultores.Text;

            newClienteBCRM.clr_RespPago = "";
            newClienteBCRM.clr_LugarPago = "";
            newClienteBCRM.clr_HorarioPago = "";
            newClienteBCRM.clrdep_Cod = null;
            newClienteBCRM.clrtdc_Cod = 1;



            Transportes trans = dcB.Transportes.Where(w => w.trn_RazSoc.ToLower() == GetTransporte(cboProvincias.Text, txtNuevo_Depart_loc.Text, txtdepartamento.Text).ToLower()).FirstOrDefault();
            newClienteB.clitrn_Cod = trans != null ? trans.trn_Cod : null;
            newClienteBCRM.clrtrn_Cod = trans != null ? trans.trn_Cod : null;


            newClienteB.cli_Nota = "";
            //newClienteB.clitic_Cod = cboTipoPresentador.SelectedValue == "Patrocinador" ? ((int)TipoAltaCliente.Patocinador).ToString() : ((int)TipoAltaCliente.Revendedor).ToString();
            // El cliente es siempre del tipo Consultor
            newClienteB.clitic_Cod = ((int)TipoAltaCliente.Revendedor).ToString();

            newClienteB.cli_Alic = 0;
            newClienteB.cli_ControlaCred = false;
            newClienteB.cli_PagWeb = "";
            newClienteB.cli_Password = "";
            newClienteB.cli_Activo = 'N';
            newClienteB.cli_FecMod = DateTime.Now;
            newClienteB.cliusu_Codigo = "WEB";
            newClienteB.cli_FotocopiaCUIT = true;
            newClienteB.clipai_Cod = "054";
            newClienteB.cliape_Cod = null;
            newClienteB.cliidm_Cod = null;
            newClienteB.clipro_cod = null;
            newClienteB.cli_ControlaCredAutoriz = 'N';
            newClienteB.cli_Calle = null;
            newClienteB.cli_Numero = null;
            newClienteB.cliccn_ID = null;
            newClienteB.cli_Piso = null;
            newClienteB.cli_Depto = null;
            newClienteB.cli_Barrio = null;
            newClienteB.clicpl_CodLoc = null;
            newClienteB.clicpl_CodPos = null;
            newClienteB.cli_NotaIngVta = "DIRECCION:" + txtDireccion.Text.ToUpper() + " " + txtdepartamento.Text + " " + txtNuevo_Depart_loc.Text + " ENTRE:" + txtCalleEntre.Text.ToUpper() + " Y " + txtCalleFinal.Text.ToUpper();
            dcB.Clientes.InsertOnSubmit(newClienteB);

            newClienteBCRM.clr_Nota = "";
            //newClienteBCRM.clrtic_Cod = cboTipoPresentador.SelectedValue == "Patrocinador" ? ((int)TipoAltaCliente.Patocinador).ToString() : ((int)TipoAltaCliente.Revendedor).ToString();
            // El cliente es siempre del tipo Consultor
            newClienteB.clitic_Cod = ((int)TipoAltaCliente.Revendedor).ToString();

            newClienteBCRM.clr_Alic = 0;
            newClienteBCRM.clr_ControlaCred = false;
            newClienteBCRM.clr_PagWeb = "";
            newClienteBCRM.clr_Password = "";
            newClienteBCRM.clr_Activo = 'N';
            newClienteBCRM.clr_FecMod = DateTime.Now;
            newClienteBCRM.clrusu_Codigo = "WEB";
            newClienteBCRM.clr_FotocopiaCUIT = true;
            newClienteBCRM.clrpai_Cod = "054";
            newClienteBCRM.clr_ControlaCred = false;

            //newClienteBCRM.clrape_Cod = null;
            //newClienteBCRM.clridm_Cod = null;
            //newClienteBCRM.clrpro_cod = null;
            //newClienteBCRM.clr_Calle = null;
            //newClienteBCRM.clr_Numero = null;
            //newClienteBCRM.clrccn_ID = null;
            //newClienteBCRM.clr_Piso = null;
            //newClienteBCRM.clr_Depto = null;
            //newClienteBCRM.clr_Barrio = null;
            //newClienteBCRM.clrcpl_CodLoc = null;
            //newClienteBCRM.clrcpl_CodPos = null;
            //newClienteBCRM.clr_NotaIngVta = "DIRECCION:" + txtDireccion.Text.ToUpper() + " " + txtdepartamento.Text + " " + txtNuevo_Depart_loc.Text + " ENTRE:" + txtCalleEntre.Text.ToUpper() + " Y " + txtCalleFinal.Text.ToUpper();
            dcB.ClientesCRMs.InsertOnSubmit(newClienteBCRM);

            #endregion

            #region Genero los datos adicionales para el cliente
            DtsCliente newDts = new DtsCliente();
            newDts.cli_Cod = newClienteB.cli_Cod;
            newDts.Dcli_000FchNac = DpFechaNacimiento.SelectedDate;
            newDts.Dcli_001Profesion = txtPrefesion.Text;
            newDts.Dcli_002Falta = DateTime.Now;
            newDts.Dcli_DNI = txtDni.Text;
            newDts.Dcli_conyuge = txtApellidoConyuge.Text.ToUpper() + " " + txtNombresConyuge.Text.ToUpper();
            newDts.Dcli_codigoInc = cboConsultores.SelectedValue;
            newDts.Dcli_EsPatrocinador = cboTipoPresentador.Text == "Referente" ? "N" : "S";

            dcB.DtsClientes.InsertOnSubmit(newDts);
            dcB.SubmitChanges();
            #endregion

            #region Actualizao los datos adicionales del presentador
            /// Busco los datos adicionales del cliente presentador
            DtsCliente dtsCliPresentador = (from dts in dcB.DtsClientes
                                            where dts.cli_Cod == cboConsultores.SelectedValue
                                            select dts).FirstOrDefault();

            /// Si el cliente que estoy dando de alta dice que lo presento un patrocinador
            /// entonces al cliente patrocinador lo marco como tal.
            if (newDts.Dcli_EsPatrocinador == "S")
            {
                dtsCliPresentador.Dcli_EsPatrocinador = "S";
            }
            else
            {
                /// Si el presentador ya es patrocinador se deja el mismo estado, 
                /// caso contrario indico que no es patrocinador.
                dtsCliPresentador.Dcli_EsPatrocinador = dtsCliPresentador.Dcli_EsPatrocinador == "" ? "N" : dtsCliPresentador.Dcli_EsPatrocinador;
            }


            #endregion

            #region Genero las direcciones de entrega, la primer direccion de entrega es la dirección principal.
            LugarEnt newDir = new LugarEnt();
            newDir.lencli_Cod = newClienteB.cli_Cod;
            newDir.len_ID = (newClienteB.cli_Cod + "1").PadRight(9, ' ');
            newDir.len_Cod = "1";


            if (txtDireccion.Text.Length < 25)
            {
                newDir.len_Desc = txtDireccion.Text.ToUpper();
                newDir.len_Lugar = "";
            }
            else
            {
                newDir.len_Desc = txtDireccion.Text.ToUpper().Substring(0, 25);

                if (txtDireccion.Text.Length > 50)
                    newDir.len_Lugar = txtDireccion.Text.ToUpper().Substring(25, 49);
                else
                    newDir.len_Lugar = txtDireccion.Text.ToUpper().Substring(25);
            }

            if (txtdepartamento.Text.Length > 15)
                newDir.len_Loc = txtdepartamento.Text.Substring(0, 15);
            else
                newDir.len_Loc = txtdepartamento.Text;

            newDir.lenprv_Codigo = dcB.prvs.Where(w => w.prv_descrip.ToLower() == cboProvincias.SelectedItem.Text.ToLower()).First().prv_codigo;
            newDir.len_CodPos = txtCodigoPostal.Text;
            newDir.len_EsDefault = true;
            newDir.lenemp_Codigo = "LCD3";
            newDir.len_Horario = "";
            newDir.len_GuiaCalles = "";
            newDir.lensuc_Cod = "";
            newDir.lenpai_Cod = "054";


            dcB.LugarEnts.InsertOnSubmit(newDir);

            dcB.SubmitChanges();

            int contDir = 2;
            if (Session["DireccionesxSolicitud"] != null)
            {
                foreach (Direccione dir in Session["DireccionesxSolicitud"] as List<Direccione>)
                {
                    newDir = new LugarEnt();
                    newDir.lencli_Cod = newClienteB.cli_Cod;
                    newDir.len_ID = (newClienteB.cli_Cod + contDir.ToString()).PadRight(9, ' ');
                    newDir.len_Cod = contDir.ToString();

                    if (dir.Calle.Length < 25)
                    {
                        newDir.len_Desc = dir.Calle.ToUpper();
                        newDir.len_Lugar = "";
                    }
                    else
                    {
                        newDir.len_Desc = dir.Calle.ToUpper().Substring(0, 25);

                        if (dir.Calle.Length > 49)
                            newDir.len_Lugar = dir.Calle.ToUpper().Substring(25, 49);
                        else
                            newDir.len_Lugar = dir.Calle.ToUpper().Substring(25);
                    }

                    if (dir.Departamento.Length > 15)
                        newDir.len_Loc = dir.Departamento.Substring(0, 15);
                    else
                        newDir.len_Loc = dir.Departamento;

                    newDir.lenprv_Codigo = dcB.prvs.Where(w => w.prv_descrip.ToLower() == dir.Provincia.ToLower()).First().prv_codigo;
                    newDir.len_CodPos = dir.CodigoPostal;
                    newDir.len_EsDefault = false;
                    newDir.lenemp_Codigo = "LCD3";
                    newDir.len_Horario = "";
                    newDir.len_GuiaCalles = "";
                    newDir.lensuc_Cod = "";
                    newDir.lenpai_Cod = "054";
                    dcB.LugarEnts.InsertOnSubmit(newDir);
                    contDir++;
                }

            }

            #endregion

            dcB.SubmitChanges();
            _clienteExistente = false;
            return newClienteB;
        }
        else
        {
            _clienteExistente = true;
            return null;
        }



    }

    private string GetHTMLNuevoConsultor(out SolicitudesAlta newSolicitud)
    {
        newSolicitud = new SolicitudesAlta();

        string tbl = "<table>";
        tbl += "<tr>";
        tbl += "    <td>";
        tbl += "Personal de Asistencia:";
        tbl += "</td>";
        tbl += "</tr>";
        tbl += "<tr>";
        tbl += "<td>";
        tbl += "El Líder <span style='font-weight:bold' id='sSolitante' runat='server'>" + Session["NombreUsuario"].ToString() + "</span>";
        tbl += " ha solicitado que se agregue un nuevo revendedor a la base de clientes de Sandra Marzzan, con los siguientes datos: ";
        tbl += "</td>";
        tbl += "</tr>";
        tbl += "<tr>";
        tbl += "   <td>";
        tbl += "        &nbsp;";
        tbl += "   </td>";
        tbl += "</tr>";

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 45px'>";
        tbl += "        <span style='font-weight:bold' id='Span2' runat='server'>Datos Personales:</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Nombre Completo:<span style='font-weight:bold' id='Span2' runat='server'>" + txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper() + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        tbl += "<tr>";
        newSolicitud.Nombre = txtApellido.Text.ToUpper() + " " + txtNombres.Text.ToUpper();

        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           DNI:<span style='font-weight:bold' id='Span2' runat='server'>" + txtDni.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.DNI = txtDni.Text;


        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Nombre Completo Conyuge:<span style='font-weight:bold' id='Span2' runat='server'>" + txtApellidoConyuge.Text.ToUpper() + " " + txtNombresConyuge.Text.ToUpper() + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        tbl += "<tr>";
        newSolicitud.NombreConyugue = txtApellidoConyuge.Text.ToUpper() + " " + txtNombresConyuge.Text.ToUpper();


        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Fecha Nacimiento:<span style='font-weight:bold' id='Span2' runat='server'>" + DpFechaNacimiento.SelectedDate.Value.ToShortDateString() + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.FechaNacimiento = DpFechaNacimiento.SelectedDate;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Teléfono Fijo:<span style='font-weight:bold' id='Span2' runat='server'>" + txtTelFijo.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.TelFijo = txtTelFijo.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Teléfono Celular:<span style='font-weight:bold' id='Span2' runat='server'>" + txtTelCel.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.TelCelular = txtTelCel.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Correo Electrónico:<span style='font-weight:bold' id='Span2' runat='server'>" + txtEmail.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.Email = txtEmail.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "          Nacionalidad:<span style='font-weight:bold' id='Span2' runat='server'>" + txtNacionalidad.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.Nacionalidad = txtNacionalidad.Text;


        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Sexo:<span style='font-weight:bold' id='Span2' runat='server'>" + cboSexo.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.Sexo = cboSexo.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Tipo Pesona:<span style='font-weight:bold' id='Span2' runat='server'>" + cboTipoPersona.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.TipoPersona = cboTipoPersona.Text;


        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Estado Civil:<span style='font-weight:bold' id='Span2' runat='server'>" + cboEstadoCivil.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.EstadoCivil = cboEstadoCivil.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Profesión:<span style='font-weight:bold' id='Span2' runat='server'>" + txtPrefesion.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.Profesion = txtPrefesion.Text;

        tbl += "<tr>";
        tbl += "   <td>";
        tbl += "        Fotocopia DNI:<span style='font-weight:bold' id='Span2' runat='server'>" + txtArchivoDNI.Text + "</span>";
        tbl += "   </td>";
        tbl += "</tr>";
        newSolicitud.ArchivoDNI = txtArchivoDNI.Text;



        tbl += "<tr>";
        tbl += "    <td style='padding-left: 45px'>";
        tbl += "        <span style='font-weight:bold' id='Span2' runat='server'>Dirección Principal:</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Provincia:<span style='font-weight:bold' id='Span2' runat='server'>" + cboProvincias.SelectedItem.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.Provincia = cboProvincias.SelectedItem.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Departamento/Localidad:<span style='font-weight:bold' id='Span2' runat='server'>" + txtdepartamento.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.Departamento = txtdepartamento.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Nuevo Departamento/Localidad:<span style='font-weight:bold' id='Span2' runat='server'>" + txtNuevo_Depart_loc.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.Localidad = txtNuevo_Depart_loc.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Código Postal:<span style='font-weight:bold' id='Span2' runat='server'>" + txtCodigoPostal.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.CodigoPostal = txtCodigoPostal.Text;


        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Dirección:<span style='font-weight:bold' id='Span2' runat='server'>" + txtDireccion.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.CalleyNro = txtDireccion.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           entre calles:<span style='font-weight:bold' id='Span2' runat='server'>" + txtCalleEntre.Text + " y " + txtCalleFinal.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.EntreCalle = txtCalleEntre.Text;
        newSolicitud.YCalle = txtCalleFinal.Text;

        tbl += "<tr>";
        tbl += "   <td>";
        tbl += "        &nbsp;";
        tbl += "   </td>";
        tbl += "</tr>";


        tbl += "<tr>";
        tbl += "    <td style='padding-left: 45px'>";
        tbl += "        <span style='font-weight:bold' id='Span2' runat='server'>Direcciones de Entrega:</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        tbl += "<tr>";
        tbl += "   <td>";
        tbl += "        &nbsp;";
        tbl += "   </td>";
        tbl += "</tr>";

        if (Session["DireccionesxSolicitud"] != null)
        {
            int contDir = 1;
            foreach (Direccione dir in Session["DireccionesxSolicitud"] as List<Direccione>)
            {
                DireccionesSolicitudAlta newDir = new DireccionesSolicitudAlta();
                tbl += "<tr>";
                tbl += "    <td style='padding-left: 65px'>";
                tbl += "           <span style='font-weight:bold' id='Span2' runat='server'>Dirección " + contDir.ToString() + "</span>";
                tbl += "    </td>";
                tbl += "</tr>";
                tbl += "<tr>";
                tbl += "    <td style='padding-left: 65px'>";
                tbl += "           Provincia:<span style='font-weight:bold' id='Span2' runat='server'>" + dir.Provincia + "</span>";
                tbl += "    </td>";
                tbl += "</tr>";
                newDir.Provincia = dir.Provincia;

                tbl += "<tr>";
                tbl += "    <td style='padding-left: 65px'>";
                tbl += "           Dep/Loc:<span style='font-weight:bold' id='Span2' runat='server'>" + dir.Departamento + "</span>";
                tbl += "    </td>";
                tbl += "</tr>";
                newDir.Departamento = dir.Departamento;

                tbl += "<tr>";
                tbl += "    <td style='padding-left: 65px'>";
                tbl += "           Nuevo Dep/Loc:<span style='font-weight:bold' id='Span2' runat='server'>" + dir.Localidad + "</span>";
                tbl += "    </td>";
                tbl += "</tr>";
                newDir.Localidad = dir.Localidad;

                tbl += "<tr>";
                tbl += "    <td style='padding-left: 65px'>";
                tbl += "           Dirección:<span style='font-weight:bold' id='Span2' runat='server'>" + dir.Calle + "</span>";
                tbl += "    </td>";
                tbl += "</tr>";
                newDir.CalleyNro = dir.Calle;

                tbl += "<tr>";
                tbl += "    <td style='padding-left: 65px'>";
                tbl += "           Código Postal:<span style='font-weight:bold' id='Span2' runat='server'>" + dir.CodigoPostal + "</span>";
                tbl += "    </td>";
                tbl += "</tr>";
                newDir.CodigoPostal = dir.CodigoPostal;

                tbl += "<tr>";
                tbl += "   <td>";
                tbl += "        &nbsp;";
                tbl += "   </td>";
                tbl += "</tr>";
                newDir.objSolicitudesAlta = newSolicitud;


                contDir++;
            }

        }


        tbl += "<tr>";
        tbl += "    <td style='padding-left: 45px'>";
        tbl += "        <span style='font-weight:bold' id='Span2' runat='server'>Contacto Entregas:</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Coordinadora:<span style='font-weight:bold' id='Span2' runat='server'>" + lblCoordinadora.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.IdCoordinadorBejerman = Session["CodigoBejerman"].ToString();

        //tbl += "<tr>";
        //tbl += "    <td style='padding-left: 65px'>";
        //tbl += "           Premio por Presentació:<span style='font-weight:bold' id='Span2' runat='server'>" + txtPremioPorPresentacion.Text + "</span>";
        //tbl += "    </td>";
        //tbl += "</tr>";
        //newSolicitud.Premio = txtPremioPorPresentacion.Text;
        newSolicitud.Premio = "";

        newSolicitud.TipoAlta = TipoAltaCliente.Revendedor.ToString();

        //if (cboTipoPresentador.SelectedValue == "Potencial")
        //{
        //    tbl += "<tr>";
        //    tbl += "    <td style='padding-left: 65px'>";
        //    tbl += "           Tipo Incorporación:<span style='font-weight:bold' id='Span2' runat='server'>" + cboTipoPresentador.SelectedValue + "</span>";
        //    tbl += "    </td>";
        //    tbl += "</tr>";
        //}
        //newSolicitud.TipoAlta = cboTipoPresentador.SelectedValue;


        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Como Nos Comocio?<span style='font-weight:bold' id='Span2' runat='server'>" + cboComo.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.Como = cboComo.Text;


        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Tipo Presentador:<span style='font-weight:bold' id='Span2' runat='server'>" + cboTipoPresentador.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.TipoPresentador = cboTipoPresentador.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Nombre Presentador:<span style='font-weight:bold' id='Span2' runat='server'>" + cboConsultores.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.Quien = cboConsultores.Text;


        /// Carga del Grupo Seleccionado dentro del mail
        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Grupo Alta:<span style='font-weight:bold' id='Span2' runat='server'>" + cboGrupos.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.GrupoAlta = cboGrupos.Text;


        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Comentario Solicitud:<span style='font-weight:bold' id='Span2' runat='server'>" + txtComentario.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.Comentario = txtComentario.Text;

        tbl += "<tr>";
        tbl += "   <td>";
        tbl += "        &nbsp;";
        tbl += "   </td>";
        tbl += "</tr>";




        tbl += "<tr>";
        tbl += "    <td style='padding-left: 45px'>";
        tbl += "        <span style='font-weight:bold' id='Span2' runat='server'>Datos Complementarios:</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Condición IVA:<span style='font-weight:bold' id='Span2' runat='server'>" + cboCondicionIVA.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.IVA = cboCondicionIVA.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           CUIT/CUIL:<span style='font-weight:bold' id='Span2' runat='server'>" + txtCuit.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.CUITL = txtCuit.Text;


        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Condición IIBB:<span style='font-weight:bold' id='Span2' runat='server'>" + cboIB.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.IB = cboIB.Text;

        tbl += "<tr>";
        tbl += "    <td style='padding-left: 65px'>";
        tbl += "           Nro IIBB:<span style='font-weight:bold' id='Span2' runat='server'>" + txtNroIb.Text + "</span>";
        tbl += "    </td>";
        tbl += "</tr>";
        newSolicitud.NroIB = txtNroIb.Text;



        newSolicitud.FechaSolicitud = DateTime.Now;

        tbl += "</table>";

        return tbl;


    }

    private string GetDireccionDestino()
    {
        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();
        ConfMail dir = null;

        try
        {
            dir = (from C in dc.ConfMails
                   where C.objCoordinador.IdCliente == long.Parse(Session["IdUsuario"].ToString())
                   select C).FirstOrDefault<ConfMail>();

            if (dir != null)
                return dir.EmailDestino;
            else
                return "";

        }
        catch
        {
            return "";
        }




    }

    private string GetTransporte(string provincia, string NuevoDepLoc, string depart_local)
    {
        /// Esto indica que no se encontro el departameto/localidad y se ha soliciticado una nueva
        if (NuevoDepLoc.Trim() != "")
        {
            return "";
        }
        else
        {
            Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();

            List<ConfTransporte> configuraciones = (from C in dc.ConfTransportes
                                                    where C.Provincia.ToUpper() == provincia.ToUpper()
                                                     && C.Localidad.ToUpper() == depart_local.ToUpper()
                                                    select C).ToList<ConfTransporte>();


            ConfTransporte conf_ContraReembolso = (from c in configuraciones
                                                   where c.FormaDePago == "Contra Reembolso"
                                                   select c).FirstOrDefault();


            /// Busca para una provincia, localidad y forma de pago contra reembolso, si no encuentra ahi
            /// busca por las otras dos formas de pago.
            if (conf_ContraReembolso != null)
                return conf_ContraReembolso.Transporte;
            else
            {

                ConfTransporte conf_PagoFacil = (from c in configuraciones
                                                 where c.FormaDePago == "Pago Fácil"
                                                 select c).FirstOrDefault();

                if (conf_PagoFacil != null)
                    return conf_PagoFacil.Transporte;
                else
                {

                    ConfTransporte conf_RapiPago = (from c in configuraciones
                                                    where c.FormaDePago == "Rapi Pago"
                                                    select c).FirstOrDefault();

                    if (conf_RapiPago != null)
                        return conf_RapiPago.Transporte;
                    else
                        return "";
                }
            }
        }
    }

    //<td class="style2">
    //    <asp:Label ID="label29" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
    //        font-size: 11px">Premio por Presentaci&oacute;n:</asp:Label>
    //</td>
    //<td align="left">
    //    <telerik:RadTextBox ID="txtPremioPorPresentacion" runat="server" EmptyMessage="Ingrese tipo de premio "
    //        InvalidStyleDuration="100" MaxLength="100" Skin="WebBlue" Width="256px">
    //    </telerik:RadTextBox>
    //    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtPremioPorPresentacion"
    //        Text="*" ErrorMessage="Debe Ingresar el premio por presentaci&oacute;n"></asp:RequiredFieldValidator>
    //</td>


    //<tr>
    //    <td class="style2">
    //        <asp:Label ID="label35" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
    //            font-size: 11px">Grupo Alta:</asp:Label>
    //    </td>
    //    <td align="left" colspan="3">
    //        <telerik:RadComboBox ID="cboGruposOriginal" runat="server" EmptyMessage="Seleccione el grupo para el alta de la solicitud"
    //            Skin="WebBlue" Width="356px">
    //            <CollapseAnimation Duration="200" Type="OutQuint" />
    //        </telerik:RadComboBox>
    //        <asp:RequiredFieldValidator ID="RequiredFieldValidator19Original" runat="server" ControlToValidate="cboGruposOriginal"
    //            Text="*" ErrorMessage="Debe Ingresar el grupo para el alta"></asp:RequiredFieldValidator>
    //    </td>
    //</tr>
}

