using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;

public partial class ABMSeguros : System.Web.UI.Page
{
    public EntidadesConosud Contexto
    {
        get
        {
            if (Session["Contexto"] != null)
                return (EntidadesConosud)Session["Contexto"];
            else
            {
                Session["Contexto"] = new EntidadesConosud();
                return (EntidadesConosud)Session["Contexto"];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            CargarCombos();
            CargarGrilla("");

            if (this.Session["TipoUsuario"].ToString() == "Cliente")
            {
                trFiltro.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            else
            {
                trFiltro.Style.Add(HtmlTextWriterStyle.Display, "block");
                

            }
            
        }

    }

    private void CargarCombos()
    {
        cboTipoSeguro.DataTextField = "Descripcion";
        cboTipoSeguro.DataValueField = "IdClasificacion";
        cboTipoSeguro.DataSource = (from c in Contexto.Clasificacion
                                    where c.Tipo == "Tipos Seguros" &&
                                    (c.Descripcion.Contains("Vida") || c.Descripcion.Contains("ART"))
                                    select c).ToList();
        cboTipoSeguro.DataBind();


        cboCompañia.DataTextField = "Descripcion";
        cboCompañia.DataValueField = "IdClasificacion";
        cboCompañia.DataSource = (from c in Contexto.Clasificacion
                                  where c.Tipo == "Compañias Seguro"
                                  select c).ToList();
        cboCompañia.DataBind();

    }


    protected void cboEmpresa_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {

        long idEmpresaContratista = 0;
        List<MaestroGenerico> Empresas = new List<MaestroGenerico>();
        if (this.Session["TipoUsuario"].ToString() == "Cliente")
        {
            idEmpresaContratista = long.Parse(Session["IdEmpresaContratista"].ToString());
            Empresas = (from l in Contexto.Empresa
                        where l.RazonSocial.StartsWith(e.Text)
                        && l.IdEmpresa == idEmpresaContratista
                        select new MaestroGenerico
                        {
                            Denominacion = l.RazonSocial,
                            Id = l.IdEmpresa
                        }).Take(10).ToList();
        }
        else
        {
            Empresas = (from l in Contexto.Empresa
                        where l.RazonSocial.StartsWith(e.Text)
                        select new MaestroGenerico
                        {
                            Denominacion = l.RazonSocial,
                            Id = l.IdEmpresa
                        }).Take(10).ToList();
        }

        

        if (((Telerik.Web.UI.ControlItemContainer)(o)).ID != "cboEmpresaContratista")
        {

            cboEmpresa.Items.Clear();
            if (Empresas.Count() > 0)
            {
                foreach (var item in Empresas)
                {
                    cboEmpresa.Items.Add(new RadComboBoxItem(item.Denominacion, item.Id.ToString()));
                }
            }
            else
            {
                cboEmpresa.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
            }
        }
        else
        {

            cboEmpresaContratista.Items.Clear();
            if (Empresas.Count() > 0)
            {
                foreach (var item in Empresas)
                {
                    cboEmpresaContratista.Items.Add(new RadComboBoxItem(item.Denominacion, item.Id.ToString()));
                }
            }
            else
            {
                cboEmpresaContratista.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
            }
        }

    }

    protected void cboContratos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        //long idEmpresa = long.Parse(e.Text);


        ///// Cargo los contratos para la empresa seleccionada
        //var Contratos = (from c in Contexto.ContratoEmpresas
        //                 where c.IdEmpresa == idEmpresa
        //                 select new
        //                 {
        //                     c.Contrato.Codigo,
        //                     c.Contrato.IdContrato,
        //                 }).Distinct();

        //cboContrato.Items.Clear();
        //if (Contratos.Count() > 0)
        //{
        //    foreach (var item in Contratos)
        //    {
        //        cboContrato.Items.Add(new RadComboBoxItem(item.Codigo, item.IdContrato.ToString()));
        //    }
        //}
        //else
        //{
        //    cboContrato.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
        //}



        ///// Cargo los seguros dados de alta para la empresa seleccionada
        //var Seguros = (from s in Contexto.Seguros
        //               where s.EmpresaContratista == idEmpresa
        //               select new
        //               {
        //                   Descripcion = s.objCompañia.Descripcion + " " + s.NroPoliza + " " + s.objTipoSeguro.Descripcion,
        //                   s.IdSeguro
        //               }).Distinct();

        //cboSeguro.Items.Clear();
        //if (Seguros.Count() > 0)
        //{
        //    foreach (var item in Seguros)
        //    {
        //        cboSeguro.Items.Add(new RadComboBoxItem(item.Descripcion, item.IdSeguro.ToString()));
        //    }
        //}
        //else
        //{
        //    cboSeguro.Items.Add(new RadComboBoxItem("No se encontraron resultados", "-1"));
        //}


    }


    protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
    {
        CargarGrilla(cboEmpresa.SelectedValue);
        UpdPnlGrilla.Update();
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {

        long idSeguro = long.Parse(gvVahiculos.SelectedValue.ToString());
        Contexto.DeleteObject((from l in Contexto.Seguros where l.IdSeguro == idSeguro select l).FirstOrDefault());
        Contexto.SaveChanges();
        CargarGrilla(cboEmpresa.SelectedValue);
    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument == "ActualizarGrilla")
        {
            CargarGrilla(cboEmpresa.SelectedValue);
            UpdPnlGrilla.Update();
        }
    }


    private void CargarGrilla(string empresa)
    {
        long IdEmpresa = 0;
        if (this.Session["TipoUsuario"].ToString() == "Cliente")
        {
            IdEmpresa = long.Parse(Session["IdEmpresaContratista"].ToString());
        }
        else
        {
            if (empresa != "")
            {
                IdEmpresa = long.Parse(empresa);
            }
        
        }

        if (IdEmpresa > 0)
        {

            var seguros = (from s in Contexto.Seguros
                           where s.EmpresaContratista == IdEmpresa
                           select new
                           {
                               DescEmpresa = s.objEmpresa.RazonSocial,
                               DescTipoSeguro = s.objTipoSeguro.Descripcion,
                               DescCompañia = s.objCompañia.Descripcion,
                               s.NroPoliza,
                               s.IdSeguro,
                               s.Compañia,
                               s.Descripcion,
                               s.EmpresaContratista,
                               s.FechaInicial,
                               s.FechaUltimoPago,
                               s.FechaVencimiento,
                               s.TipoSeguro

                           }).Take(20);


            gvVahiculos.DataSource = seguros;
            gvVahiculos.DataBind();
        }
        else
        {
            var seguros = (from s in Contexto.Seguros
                           select new
                           {
                               DescEmpresa = s.objEmpresa.RazonSocial,
                               DescTipoSeguro = s.objTipoSeguro.Descripcion,
                               DescCompañia = s.objCompañia.Descripcion,
                               s.NroPoliza,
                               s.IdSeguro,
                               s.Compañia,
                               s.Descripcion,
                               s.EmpresaContratista,
                               s.FechaInicial,
                               s.FechaUltimoPago,
                               s.FechaVencimiento,
                               s.TipoSeguro

                           }).Take(20);


            gvVahiculos.DataSource = seguros;
            gvVahiculos.DataBind();
        }


    }


    [WebMethod(EnableSession = true)]
    public static void Aplicar(
        string IdCopañia, string IdTipoSeguro, string NroPoliza, string
                    Descipcion, string FechaInicial, string FechaVencimiento, string FechaUltimoPago, string
                    IdEmpresa, string IdSeguro)
    {

        if (IdSeguro == "-1")
        {

            Seguros newSeguro = new Seguros();
            newSeguro.Descripcion = Descipcion;
            newSeguro.NroPoliza = NroPoliza;


            if (IdCopañia != "")
                newSeguro.Compañia = long.Parse(IdCopañia);

            if (IdEmpresa != "")
                newSeguro.EmpresaContratista = long.Parse(IdEmpresa);

            if (IdTipoSeguro != "")
                newSeguro.TipoSeguro = long.Parse(IdTipoSeguro);




            if (FechaInicial != "")
                newSeguro.FechaInicial = Convert.ToDateTime(FechaInicial);

            if (FechaUltimoPago != "")
                newSeguro.FechaUltimoPago = Convert.ToDateTime(FechaUltimoPago);

            if (FechaVencimiento != "")
                newSeguro.FechaVencimiento = Convert.ToDateTime(FechaVencimiento);


            (HttpContext.Current.Session["Contexto"] as EntidadesConosud).AddToSeguros(newSeguro);
        }
        else
        {
            long Id = long.Parse(IdSeguro);
            Seguros newSeguro = (from v in (HttpContext.Current.Session["Contexto"] as EntidadesConosud).Seguros
                                 where v.IdSeguro == Id
                                 select v).First();


            newSeguro.Descripcion = Descipcion;
            newSeguro.NroPoliza = NroPoliza;


            if (IdCopañia.Trim() != "")
                newSeguro.Compañia = long.Parse(IdCopañia);
            else
                newSeguro.Compañia = null;


            if (IdTipoSeguro != "")
                newSeguro.TipoSeguro = long.Parse(IdTipoSeguro);
            else
                newSeguro.TipoSeguro = null;


            if (IdEmpresa != "")
                newSeguro.EmpresaContratista = long.Parse(IdEmpresa);
            else
                newSeguro.EmpresaContratista = null;



            if (FechaInicial != "")
                newSeguro.FechaInicial = Convert.ToDateTime(FechaInicial);
            else
                newSeguro.FechaInicial = null;


            if (FechaVencimiento != "")
                newSeguro.FechaVencimiento = Convert.ToDateTime(FechaVencimiento);
            else
                newSeguro.FechaVencimiento = null;


            if (FechaUltimoPago != "")
                newSeguro.FechaUltimoPago = Convert.ToDateTime(FechaUltimoPago);
            else
                newSeguro.FechaUltimoPago = null;

        }

        (HttpContext.Current.Session["Contexto"] as EntidadesConosud).SaveChanges();

    }

}
