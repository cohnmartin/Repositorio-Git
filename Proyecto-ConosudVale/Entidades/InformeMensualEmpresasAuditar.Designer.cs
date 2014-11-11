namespace Entidades
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    partial class InformeMensualEmpresasAuditar
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformeMensualEmpresasAuditar));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.txtPeriodo = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox20 = new Telerik.Reporting.TextBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Height = new Telerik.Reporting.Drawing.Unit(3D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pictureBox1,
            this.titleTextBox,
            this.txtPeriodo,
            this.textBox2,
            this.textBox9,
            this.textBox8,
            this.textBox7,
            this.textBox6,
            this.textBox5,
            this.textBox4,
            this.textBox3,
            this.textBox17,
            this.textBox19});
            this.pageHeader.Name = "pageHeader";
            // 
            // pictureBox1
            // 
            this.pictureBox1.MimeType = "image/png";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(105.8267822265625D, Telerik.Reporting.Drawing.UnitType.Pixel), new Telerik.Reporting.Drawing.Unit(75.5905532836914D, Telerik.Reporting.Drawing.UnitType.Pixel));
            this.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            this.pictureBox1.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(2.8002004623413086D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(9.9921220680698752E-05D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(13.699803352355957D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.88561445474624634D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.Style.BackgroundColor = System.Drawing.Color.White;
            this.titleTextBox.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(8D, Telerik.Reporting.Drawing.UnitType.Mm);
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "Informe Empresas a Auditar";
            // 
            // txtPeriodo
            // 
            this.txtPeriodo.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(2.8002004623413086D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.88591408729553223D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtPeriodo.Name = "txtPeriodo";
            this.txtPeriodo.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(13.699803352355957D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.88561445474624634D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtPeriodo.Style.BackgroundColor = System.Drawing.Color.White;
            this.txtPeriodo.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(4D, Telerik.Reporting.Drawing.UnitType.Mm);
            this.txtPeriodo.StyleName = "Title";
            this.txtPeriodo.Value = "= Parameters.Periodo";
            // 
            // textBox2
            // 
            this.textBox2.Format = "{0:D}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(21.500001907348633D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(9.9921220680698752E-05D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(4.4997010231018066D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox2.StyleName = "PageInfo";
            this.textBox2.Value = "= Now()";
            // 
            // textBox9
            // 
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(26.000001907348633D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.2000000476837158D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.1998987197875977D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.799799919128418D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox9.Style.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.textBox9.Style.BorderColor.Bottom = System.Drawing.Color.SkyBlue;
            this.textBox9.Style.BorderColor.Default = System.Drawing.Color.SkyBlue;
            this.textBox9.Style.BorderColor.Left = System.Drawing.Color.SkyBlue;
            this.textBox9.Style.BorderColor.Right = System.Drawing.Color.SkyBlue;
            this.textBox9.Style.BorderColor.Top = System.Drawing.Color.SkyBlue;
            this.textBox9.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox9.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox9.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox9.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox9.Style.Color = System.Drawing.Color.White;
            this.textBox9.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox9.StyleName = "PageInfo";
            this.textBox9.Value = "Public.";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(24.000200271606445D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.2000000476837158D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(1.9994994401931763D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.800000011920929D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox8.Style.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.textBox8.Style.BorderColor.Default = System.Drawing.Color.SkyBlue;
            this.textBox8.Style.BorderColor.Right = System.Drawing.Color.SkyBlue;
            this.textBox8.Style.BorderColor.Top = System.Drawing.Color.SkyBlue;
            this.textBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox8.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox8.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox8.Style.Color = System.Drawing.Color.White;
            this.textBox8.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox8.StyleName = "PageInfo";
            this.textBox8.Value = "Estado";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(21.500001907348633D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.2000000476837158D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.5D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.800000011920929D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox7.Style.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.textBox7.Style.BorderColor.Default = System.Drawing.Color.SkyBlue;
            this.textBox7.Style.BorderColor.Right = System.Drawing.Color.SkyBlue;
            this.textBox7.Style.BorderColor.Top = System.Drawing.Color.SkyBlue;
            this.textBox7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox7.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox7.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox7.Style.Color = System.Drawing.Color.White;
            this.textBox7.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.StyleName = "PageInfo";
            this.textBox7.Value = "Fecha Aud.";
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(19D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.2000000476837158D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.5D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.800000011920929D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox6.Style.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.textBox6.Style.BorderColor.Default = System.Drawing.Color.SkyBlue;
            this.textBox6.Style.BorderColor.Right = System.Drawing.Color.SkyBlue;
            this.textBox6.Style.BorderColor.Top = System.Drawing.Color.SkyBlue;
            this.textBox6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox6.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox6.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox6.Style.Color = System.Drawing.Color.White;
            this.textBox6.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.StyleName = "PageInfo";
            this.textBox6.Value = "Fecha PS";
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(16.5D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.2000000476837158D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.499798059463501D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.800000011920929D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox5.Style.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.textBox5.Style.BorderColor.Default = System.Drawing.Color.SkyBlue;
            this.textBox5.Style.BorderColor.Right = System.Drawing.Color.SkyBlue;
            this.textBox5.Style.BorderColor.Top = System.Drawing.Color.SkyBlue;
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox5.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox5.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox5.Style.Color = System.Drawing.Color.White;
            this.textBox5.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.StyleName = "PageInfo";
            this.textBox5.Value = "Fecha PP";
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.600001335144043D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.1999998092651367D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.8997998237609863D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.800000011920929D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox4.Style.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.textBox4.Style.BorderColor.Default = System.Drawing.Color.SkyBlue;
            this.textBox4.Style.BorderColor.Right = System.Drawing.Color.SkyBlue;
            this.textBox4.Style.BorderColor.Top = System.Drawing.Color.SkyBlue;
            this.textBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.Color = System.Drawing.Color.White;
            this.textBox4.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.StyleName = "PageInfo";
            this.textBox4.Value = "Contrato";
            // 
            // textBox3
            // 
            this.textBox3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(9.9921220680698752E-05D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.2000000476837158D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5.899899959564209D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.799799919128418D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox3.Style.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.textBox3.Style.BorderColor.Default = System.Drawing.Color.SkyBlue;
            this.textBox3.Style.BorderColor.Right = System.Drawing.Color.SkyBlue;
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox3.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox3.Style.Color = System.Drawing.Color.White;
            this.textBox3.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.StyleName = "PageInfo";
            this.textBox3.Value = "Empresa";
            // 
            // textBox17
            // 
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(10.600000381469727D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.2000002861022949D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.9995999336242676D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.800000011920929D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox17.Style.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.textBox17.Style.BorderColor.Default = System.Drawing.Color.SkyBlue;
            this.textBox17.Style.BorderColor.Right = System.Drawing.Color.SkyBlue;
            this.textBox17.Style.BorderColor.Top = System.Drawing.Color.SkyBlue;
            this.textBox17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox17.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox17.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox17.Style.Color = System.Drawing.Color.White;
            this.textBox17.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
            this.textBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox17.StyleName = "PageInfo";
            this.textBox17.Value = "Período";
            // 
            // textBox19
            // 
            this.textBox19.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(5.8999996185302734D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.1999998092651367D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(4.6993999481201172D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.799799919128418D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox19.Style.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.textBox19.Style.BorderColor.Default = System.Drawing.Color.SkyBlue;
            this.textBox19.Style.BorderColor.Right = System.Drawing.Color.SkyBlue;
            this.textBox19.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox19.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox19.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox19.Style.Color = System.Drawing.Color.White;
            this.textBox19.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
            this.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox19.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox19.StyleName = "PageInfo";
            this.textBox19.Value = "Sub Contratista";
            // 
            // detail
            // 
            this.detail.Height = new Telerik.Reporting.Drawing.Unit(0.60019993782043457D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox10,
            this.textBox11,
            this.textBox12,
            this.textBox13,
            this.textBox14,
            this.textBox15,
            this.textBox16,
            this.textBox18,
            this.textBox20});
            this.detail.Name = "detail";
            this.detail.Style.BackgroundColor = System.Drawing.Color.Empty;
            // 
            // textBox10
            // 
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5.8999996185302734D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox10.TextWrap = false;
            this.textBox10.Value = "= Fields.DescContratista";
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.600103378295898D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.00020024616969749332D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.89989972114563D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox11.StyleName = "PageInfo";
            this.textBox11.TextWrap = false;
            this.textBox11.Value = "= Fields.CodigoContrato";
            // 
            // textBox12
            // 
            this.textBox12.Format = "{0:dd/MM/yyyy}";
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(16.500001907348633D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.5D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox12.StyleName = "PageInfo";
            this.textBox12.Value = "=Fields.FechaUltimaPresentacionPrevisional";
            // 
            // textBox13
            // 
            this.textBox13.Format = "{0:dd/MM/yyyy}";
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(19D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.5D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox13.StyleName = "PageInfo";
            this.textBox13.Value = "=Fields.FechaUltimaPresentacionSueldos";
            // 
            // textBox14
            // 
            this.textBox14.Format = "{0:dd/MM/yyyy}";
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(21.500001907348633D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.5D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox14.StyleName = "PageInfo";
            this.textBox14.Value = "=Fields.FechaUltimaAuditoria";
            // 
            // textBox15
            // 
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(24D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(1.999699592590332D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox15.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox15.StyleName = "PageInfo";
            this.textBox15.TextWrap = false;
            this.textBox15.Value = "=Fields.Aprobada";
            // 
            // textBox16
            // 
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(26.000001907348633D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.1998987197875977D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox16.StyleName = "PageInfo";
            this.textBox16.Value = "=Fields.Publicada";
            // 
            // textBox18
            // 
            this.textBox18.Format = "{0:yyyy/MM}";
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(10.600000381469727D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.00020024616969749332D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.9998006820678711D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox18.StyleName = "PageInfo";
            this.textBox18.Value = "= Fields.Periodo";
            // 
            // textBox20
            // 
            this.textBox20.Culture = new System.Globalization.CultureInfo("");
            this.textBox20.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(5.9001998901367188D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(4.6991996765136719D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox20.TextWrap = false;
            this.textBox20.Value = "= Fields.DescSubContratista";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = new Telerik.Reporting.Drawing.Unit(0.39980036020278931D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.pageFooter.Name = "pageFooter";
            // 
            // InformeMensualEmpresasAuditar
            // 
            this.DocumentName = "EmpresasAuditar";
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeader,
            this.detail,
            this.pageFooter});
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins.Bottom = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Left = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Right = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Top = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "Periodo";
            this.ReportParameters.Add(reportParameter1);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            this.Width = new Telerik.Reporting.Drawing.Unit(281.99899291992188D, Telerik.Reporting.Drawing.UnitType.Mm);
            this.NeedDataSource += new System.EventHandler(this.InformeMensualEmpresasAuditar_NeedDataSource);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.PictureBox pictureBox1;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.TextBox txtPeriodo;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox7;
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
    }
}