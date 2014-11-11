namespace Entidades
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    partial class InformeAltaBajaLegajo
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformeAltaBajaLegajo));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox20 = new Telerik.Reporting.TextBox();
            this.textBox21 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageFooter
            // 
            this.pageFooter.Height = new Telerik.Reporting.Drawing.Unit(0.71437495946884155D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageInfoTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(19.866981506347656D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = new Telerik.Reporting.Drawing.Unit(3.3141751289367676D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox,
            this.pictureBox1,
            this.textBox2,
            this.textBox6,
            this.textBox7,
            this.textBox13,
            this.textBox14,
            this.textBox15,
            this.textBox16,
            this.textBox17,
            this.textBox18,
            this.textBox19,
            this.textBox20,
            this.textBox21});
            this.reportHeader.Name = "reportHeader";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(2.8531172275543213D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(9.9921220680698752E-05D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(11.605630874633789D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.98552507162094116D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.Style.BackgroundColor = System.Drawing.Color.Empty;
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "Informe Altas Bajas Legajos";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pictureBox1.MimeType = "image/png";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.7999999523162842D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.1859145164489746D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            this.pictureBox1.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // textBox2
            // 
            this.textBox2.Format = "{0:D}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(21.899999618530273D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.5858148336410523D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(11.699999809265137D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Style.Color = System.Drawing.Color.Black;
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox2.StyleName = "PageInfo";
            this.textBox2.Value = "= Now()";
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(2.8531172275543213D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.9858248233795166D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.5468826293945312D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.69980001449584961D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox6.Style.BackgroundColor = System.Drawing.Color.Empty;
            this.textBox6.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(13D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox6.StyleName = "Title";
            this.textBox6.Value = "Período:";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(5.4001998901367188D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.9858248233795166D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(6.69980001449585D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.69980001449584961D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox7.Style.BackgroundColor = System.Drawing.Color.Empty;
            this.textBox7.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(13D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox7.StyleName = "Title";
            this.textBox7.Value = "= Parameters.Periodo";
            // 
            // textBox13
            // 
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.7141752243041992D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(1.7470831871032715D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox13.Style.BackgroundColor = System.Drawing.Color.Orange;
            this.textBox13.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox13.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox13.Style.Color = System.Drawing.Color.Black;
            this.textBox13.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox13.StyleName = "PageInfo";
            this.textBox13.Value = "Mov";
            // 
            // textBox14
            // 
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(1.7470831871032715D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.7141752243041992D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5.9468832015991211D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox14.Style.BackgroundColor = System.Drawing.Color.Orange;
            this.textBox14.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox14.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox14.Style.Color = System.Drawing.Color.Black;
            this.textBox14.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox14.StyleName = "PageInfo";
            this.textBox14.Value = "Contratista";
            // 
            // textBox15
            // 
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(7.6943669319152832D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.7141752243041992D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5.45271635055542D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox15.Style.BackgroundColor = System.Drawing.Color.Orange;
            this.textBox15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox15.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox15.Style.Color = System.Drawing.Color.Black;
            this.textBox15.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox15.StyleName = "PageInfo";
            this.textBox15.Value = "Sub Contratista";
            // 
            // textBox16
            // 
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.147283554077148D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.7141752243041992D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.8998000621795654D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox16.Style.BackgroundColor = System.Drawing.Color.Orange;
            this.textBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox16.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox16.Style.Color = System.Drawing.Color.Black;
            this.textBox16.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox16.StyleName = "PageInfo";
            this.textBox16.Value = "Nº Contrato";
            // 
            // textBox17
            // 
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(16.047283172607422D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.7141752243041992D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5.4523153305053711D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox17.Style.BackgroundColor = System.Drawing.Color.Orange;
            this.textBox17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox17.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox17.Style.Color = System.Drawing.Color.Black;
            this.textBox17.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox17.StyleName = "PageInfo";
            this.textBox17.Value = "Apellido y Nombre";
            // 
            // textBox18
            // 
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(21.499799728393555D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.7141752243041992D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.2002015113830566D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox18.Style.BackgroundColor = System.Drawing.Color.Orange;
            this.textBox18.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox18.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox18.Style.Color = System.Drawing.Color.Black;
            this.textBox18.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox18.StyleName = "PageInfo";
            this.textBox18.Value = "DNI";
            // 
            // textBox19
            // 
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(23.7002010345459D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.7141752243041992D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.5998027324676514D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox19.Style.BackgroundColor = System.Drawing.Color.Orange;
            this.textBox19.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox19.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox19.Style.Color = System.Drawing.Color.Black;
            this.textBox19.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox19.StyleName = "PageInfo";
            this.textBox19.Value = "Encuadre";
            // 
            // textBox20
            // 
            this.textBox20.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(27.300203323364258D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.7141752243041992D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.5998027324676514D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox20.Style.BackgroundColor = System.Drawing.Color.Orange;
            this.textBox20.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox20.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox20.Style.Color = System.Drawing.Color.Black;
            this.textBox20.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox20.StyleName = "PageInfo";
            this.textBox20.Value = "Fecha Trámite";
            // 
            // textBox21
            // 
            this.textBox21.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(30.900205612182617D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.7141752243041992D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.6995928287506104D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox21.Style.BackgroundColor = System.Drawing.Color.Orange;
            this.textBox21.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox21.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox21.Style.Color = System.Drawing.Color.Black;
            this.textBox21.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox21.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox21.StyleName = "PageInfo";
            this.textBox21.Value = "Mes Baja";
            // 
            // detail
            // 
            this.detail.Height = new Telerik.Reporting.Drawing.Unit(0.599999725818634D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.textBox4,
            this.textBox3,
            this.textBox5,
            this.textBox8,
            this.textBox9,
            this.textBox10,
            this.textBox11,
            this.textBox12});
            this.detail.Name = "detail";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(1.7472834587097168D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5.9468832015991211D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox1.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox1.Style.Color = System.Drawing.Color.Black;
            this.textBox1.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox1.StyleName = "PageInfo";
            this.textBox1.TextWrap = false;
            this.textBox1.Value = "=Fields.NombreEmpresaContratista";
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(7.6943669319152832D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5.4527158737182617D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox4.Style.Color = System.Drawing.Color.Black;
            this.textBox4.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox4.StyleName = "PageInfo";
            this.textBox4.TextWrap = false;
            this.textBox4.Value = "=Fields.NombreEmpresaSubContratista";
            // 
            // textBox3
            // 
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(1.7470831871032715D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox3.Style.Color = System.Drawing.Color.Black;
            this.textBox3.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox3.StyleName = "PageInfo";
            this.textBox3.Value = "=Fields.Accion";
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.147283554077148D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.8997998237609863D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox5.Style.Color = System.Drawing.Color.Black;
            this.textBox5.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox5.StyleName = "PageInfo";
            this.textBox5.TextWrap = false;
            this.textBox5.Value = "=Fields.CodigoContrato";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(16.047283172607422D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5.4523153305053711D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox8.Style.Color = System.Drawing.Color.Black;
            this.textBox8.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox8.StyleName = "PageInfo";
            this.textBox8.TextWrap = false;
            this.textBox8.Value = "=Fields.NombreCompleto";
            // 
            // textBox9
            // 
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(21.499799728393555D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.2002012729644775D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox9.Style.Color = System.Drawing.Color.Black;
            this.textBox9.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox9.StyleName = "PageInfo";
            this.textBox9.Value = "=Fields.Nrodoc";
            // 
            // textBox10
            // 
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(23.7002010345459D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.5998024940490723D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox10.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox10.Style.Color = System.Drawing.Color.Black;
            this.textBox10.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox10.StyleName = "PageInfo";
            this.textBox10.TextWrap = false;
            this.textBox10.Value = "=Fields.Encuadre";
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(27.300203323364258D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.5998024940490723D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox11.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox11.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox11.Style.Color = System.Drawing.Color.Black;
            this.textBox11.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox11.StyleName = "PageInfo";
            this.textBox11.TextWrap = false;
            this.textBox11.Value = "=Fields.FechaTramite";
            // 
            // textBox12
            // 
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(30.900205612182617D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.6995925903320312D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox12.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox12.Style.Color = System.Drawing.Color.Black;
            this.textBox12.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox12.StyleName = "PageInfo";
            this.textBox12.Value = "=Fields.FechaBaja";
            // 
            // InformeAltaBajaLegajo
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageFooter,
            this.reportHeader,
            this.detail});
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins.Bottom = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Left = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Right = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Top = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Legal;
            reportParameter1.Name = "Periodo";
            this.ReportParameters.Add(reportParameter1);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule1.Style.Color = System.Drawing.Color.Black;
            styleRule1.Style.Font.Bold = true;
            styleRule1.Style.Font.Italic = false;
            styleRule1.Style.Font.Name = "Tahoma";
            styleRule1.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(20D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule1.Style.Font.Strikeout = false;
            styleRule1.Style.Font.Underline = false;
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule2.Style.Color = System.Drawing.Color.Black;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(11D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = new Telerik.Reporting.Drawing.Unit(33.599998474121094D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.NeedDataSource += new System.EventHandler(this.InformeAltaBajaLegajo_NeedDataSource);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private ReportHeaderSection reportHeader;
        private DetailSection detail;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.PictureBox pictureBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.TextBox textBox13;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.TextBox textBox15;
        private Telerik.Reporting.TextBox textBox16;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox textBox18;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox20;
        private Telerik.Reporting.TextBox textBox21;

    }
}