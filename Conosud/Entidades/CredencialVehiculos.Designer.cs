namespace Entidades
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    partial class CredencialVehiculos
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.detail = new Telerik.Reporting.DetailSection();
            this.pnlPrincipalReverso = new Telerik.Reporting.Panel();
            this.txtTituloR = new Telerik.Reporting.TextBox();
            this.txtTarjetR = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.panel5 = new Telerik.Reporting.Panel();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.panel6 = new Telerik.Reporting.Panel();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.barcode1 = new Telerik.Reporting.Barcode();
            this.pnlPrincipalF = new Telerik.Reporting.Panel();
            this.txtTituloF = new Telerik.Reporting.TextBox();
            this.txtTarjetF = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.panel1 = new Telerik.Reporting.Panel();
            this.txtMarca = new Telerik.Reporting.TextBox();
            this.txtModelo = new Telerik.Reporting.TextBox();
            this.txtTituloVehiculo = new Telerik.Reporting.TextBox();
            this.panel2 = new Telerik.Reporting.Panel();
            this.txtTituloDominio = new Telerik.Reporting.TextBox();
            this.txtPatente = new Telerik.Reporting.TextBox();
            this.panel3 = new Telerik.Reporting.Panel();
            this.txtTituloVigencia = new Telerik.Reporting.TextBox();
            this.txtVigencia = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.txtPuestoIngreso = new Telerik.Reporting.TextBox();
            this.panel4 = new Telerik.Reporting.Panel();
            this.txtTipo = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageFooter
            // 
            this.pageFooter.Height = new Telerik.Reporting.Drawing.Unit(1.0000988245010376D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox6});
            this.pageFooter.Name = "pageFooter";
            this.pageFooter.Style.Visible = true;
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.10000024735927582D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(9.9315635452512652E-05D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(28.519899368286133D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99999988079071045D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox6.Style.Color = System.Drawing.Color.Black;
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(13D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.Value = "LA PRESENTE CREDENCIAL DE HABILITACION CARECE DE VALOR SIN LA FIRMA DEL RESPONSAB" +
    "LE DE SyS DE YPF";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = new Telerik.Reporting.Drawing.Unit(0.60000008344650269D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.reportHeader.Name = "reportHeader";
            this.reportHeader.Style.Visible = false;
            // 
            // detail
            // 
            this.detail.Height = new Telerik.Reporting.Drawing.Unit(194D, Telerik.Reporting.Drawing.UnitType.Mm);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pnlPrincipalReverso,
            this.pnlPrincipalF});
            this.detail.Name = "detail";
            // 
            // pnlPrincipalReverso
            // 
            this.pnlPrincipalReverso.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtTituloR,
            this.txtTarjetR,
            this.textBox11,
            this.panel5,
            this.panel6});
            this.pnlPrincipalReverso.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(14.499999046325684D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.099999949336051941D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pnlPrincipalReverso.Name = "pnlPrincipalReverso";
            this.pnlPrincipalReverso.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(14.119901657104492D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(19.299901962280273D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pnlPrincipalReverso.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(192)))));
            // 
            // txtTituloR
            // 
            this.txtTituloR.CanGrow = false;
            this.txtTituloR.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTituloR.Name = "txtTituloR";
            this.txtTituloR.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(14.119901657104492D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.7999999523162842D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTituloR.Style.Color = System.Drawing.Color.White;
            this.txtTituloR.Style.Font.Bold = true;
            this.txtTituloR.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(40D, Telerik.Reporting.Drawing.UnitType.Point);
            this.txtTituloR.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtTituloR.Value = "OFICIAL";
            // 
            // txtTarjetR
            // 
            this.txtTarjetR.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(6.0002002716064453D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.8001999855041504D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTarjetR.Name = "txtTarjetR";
            this.txtTarjetR.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(6.4998002052307129D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99980032444000244D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTarjetR.Style.BackgroundColor = System.Drawing.Color.White;
            this.txtTarjetR.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(25D, Telerik.Reporting.Drawing.UnitType.Point);
            this.txtTarjetR.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtTarjetR.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtTarjetR.Value = "xxxxxxx";
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(1.0000007152557373D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.8001999855041504D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99980026483535767D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox11.Style.Color = System.Drawing.Color.White;
            this.textBox11.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(25D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox11.Value = "Tarjeta Nº:";
            // 
            // panel5
            // 
            this.panel5.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox12});
            this.panel5.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.4999997615814209D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(3D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel5.Name = "panel5";
            this.panel5.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(13.000001907348633D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(4.5D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel5.Style.BackgroundColor = System.Drawing.Color.White;
            // 
            // textBox12
            // 
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.50000095367431641D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(11.999799728393555D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(4D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox12.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.textBox12.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(18D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox12.Style.Padding.Top = new Telerik.Reporting.Drawing.Unit(0.20000000298023224D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.textBox12.Value = "Esta tarjeta deberá permanecer colgada del espejo retrovisor o en su defecto cerc" +
    "ana al parabrisas durante la permanencia del vehículo dentro del Complejo Indust" +
    "rial Lujan de Cuyo (CILC)\r\n";
            // 
            // panel6
            // 
            this.panel6.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox13,
            this.textBox14,
            this.textBox4,
            this.textBox5,
            this.barcode1});
            this.panel6.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.4999997615814209D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(7.7999997138977051D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel6.Name = "panel6";
            this.panel6.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(12.999801635742188D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(11.000000953674316D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel6.Style.BackgroundColor = System.Drawing.Color.White;
            // 
            // textBox13
            // 
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.19999989867210388D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.19999989867210388D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(12.299798965454102D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.79999959468841553D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox13.Style.Color = System.Drawing.Color.Red;
            this.textBox13.Style.Font.Bold = true;
            this.textBox13.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(18D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox13.Style.Font.Underline = true;
            this.textBox13.Value = "No olvide";
            // 
            // textBox14
            // 
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.20000110566616058D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.9999995231628418D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(12.299799919128418D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(3.9999992847442627D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox14.Style.Color = System.Drawing.Color.Red;
            this.textBox14.Style.Font.Bold = true;
            this.textBox14.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(14D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox14.Style.LineWidth = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox14.Style.Padding.Left = new Telerik.Reporting.Drawing.Unit(0.30000001192092896D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.textBox14.Style.Padding.Right = new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.textBox14.Style.Padding.Top = new Telerik.Reporting.Drawing.Unit(0.30000001192092896D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.textBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.textBox14.Value = "- Usar cinturón de seguridad\r\n- Respetar velocidad máxima\r\n- No utilizar Celular " +
    "o Radio mientras conduce\r\n- Prioridad al peatón\r\n- Circular solo por lugares aut" +
    "orizados\r\n- Luces bajas encendidas";
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.20000070333480835D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(5.0270833969116211D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(12.299798965454102D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99999988079071045D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox4.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.textBox4.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(25D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.Value = "Aprobación S&S";
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.20000110566616058D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(6.0272831916809082D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(12.299798965454102D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.9727168083190918D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.Color = System.Drawing.Color.Black;
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(60D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            // 
            // barcode1
            // 
            this.barcode1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.20000110566616058D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(8.4999990463256836D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.barcode1.Name = "barcode1";
            this.barcode1.ShowText = false;
            this.barcode1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(12.299798965454102D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.0648629665374756D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.barcode1.Stretch = true;
            // 
            // pnlPrincipalF
            // 
            this.pnlPrincipalF.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtTituloF,
            this.txtTarjetF,
            this.textBox1,
            this.panel1,
            this.panel2,
            this.panel3,
            this.textBox2,
            this.textBox3,
            this.textBox7,
            this.txtPuestoIngreso});
            this.pnlPrincipalF.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.099999949336051941D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.099999949336051941D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pnlPrincipalF.Name = "pnlPrincipalF";
            this.pnlPrincipalF.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(14.119901657104492D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(19.30000114440918D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pnlPrincipalF.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(192)))));
            // 
            // txtTituloF
            // 
            this.txtTituloF.CanGrow = false;
            this.txtTituloF.CanShrink = true;
            this.txtTituloF.Name = "txtTituloF";
            this.txtTituloF.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(14.119901657104492D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.7999999523162842D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTituloF.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.txtTituloF.Style.Color = System.Drawing.Color.White;
            this.txtTituloF.Style.Font.Bold = true;
            this.txtTituloF.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(40D, Telerik.Reporting.Drawing.UnitType.Point);
            this.txtTituloF.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtTituloF.TextWrap = false;
            this.txtTituloF.Value = "OFICIAL";
            // 
            // txtTarjetF
            // 
            this.txtTarjetF.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(5.4001994132995605D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.8001999855041504D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTarjetF.Name = "txtTarjetF";
            this.txtTarjetF.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(6.4998002052307129D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99980032444000244D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTarjetF.Style.BackgroundColor = System.Drawing.Color.White;
            this.txtTarjetF.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(25D, Telerik.Reporting.Drawing.UnitType.Point);
            this.txtTarjetF.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtTarjetF.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtTarjetF.Value = "xxxxxxx";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.40000003576278687D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.8001999855041504D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99980026483535767D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox1.Style.Color = System.Drawing.Color.White;
            this.textBox1.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(25D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox1.Value = "Tarjeta Nº:";
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtMarca,
            this.txtModelo,
            this.txtTituloVehiculo,
            this.txtTipo});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(1.2649255990982056D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(3D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(11.59005069732666D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(4.5D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel1.Style.BackgroundColor = System.Drawing.Color.White;
            // 
            // txtMarca
            // 
            this.txtMarca.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.54512536525726318D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.1500999927520752D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtMarca.Name = "txtMarca";
            this.txtMarca.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(10.499799728393555D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.0499999523162842D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtMarca.Style.Color = System.Drawing.Color.Black;
            this.txtMarca.Style.Font.Bold = true;
            this.txtMarca.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(35D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.txtMarca.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtMarca.Value = "Renualt";
            // 
            // txtModelo
            // 
            this.txtModelo.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.54507529735565186D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(3.3000001907348633D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtModelo.Name = "txtModelo";
            this.txtModelo.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(10.499899864196777D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.0499999523162842D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtModelo.Style.Color = System.Drawing.Color.Black;
            this.txtModelo.Style.Font.Bold = true;
            this.txtModelo.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(35D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.txtModelo.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtModelo.Value = "Trafic";
            // 
            // txtTituloVehiculo
            // 
            this.txtTituloVehiculo.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.54507613182067871D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTituloVehiculo.Name = "txtTituloVehiculo";
            this.txtTituloVehiculo.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(10.499898910522461D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99999988079071045D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTituloVehiculo.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.txtTituloVehiculo.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(25D, Telerik.Reporting.Drawing.UnitType.Point);
            this.txtTituloVehiculo.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtTituloVehiculo.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtTituloVehiculo.Value = "Tipo Vehículo";
            // 
            // panel2
            // 
            this.panel2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtTituloDominio,
            this.txtPatente});
            this.panel2.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(1.2649255990982056D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(7.7999997138977051D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel2.Name = "panel2";
            this.panel2.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(11.59005069732666D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(3.4999995231628418D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel2.Style.BackgroundColor = System.Drawing.Color.White;
            // 
            // txtTituloDominio
            // 
            this.txtTituloDominio.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.54512536525726318D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTituloDominio.Name = "txtTituloDominio";
            this.txtTituloDominio.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(10.499799728393555D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99999988079071045D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTituloDominio.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.txtTituloDominio.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(25D, Telerik.Reporting.Drawing.UnitType.Point);
            this.txtTituloDominio.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtTituloDominio.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtTituloDominio.Value = "Dominio";
            // 
            // txtPatente
            // 
            this.txtPatente.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.54512536525726318D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.4999991655349731D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtPatente.Name = "txtPatente";
            this.txtPatente.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(10.499799728393555D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.3486040830612183D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtPatente.Style.Color = System.Drawing.Color.Black;
            this.txtPatente.Style.Font.Bold = true;
            this.txtPatente.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(48D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.txtPatente.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtPatente.Value = "ABC 123";
            // 
            // panel3
            // 
            this.panel3.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtTituloVigencia,
            this.txtVigencia});
            this.panel3.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(1.2649255990982056D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(11.800000190734863D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel3.Name = "panel3";
            this.panel3.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(11.59005069732666D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(3.5000007152557373D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.panel3.Style.BackgroundColor = System.Drawing.Color.White;
            // 
            // txtTituloVigencia
            // 
            this.txtTituloVigencia.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.54507613182067871D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTituloVigencia.Name = "txtTituloVigencia";
            this.txtTituloVigencia.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(10.499898910522461D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99999988079071045D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTituloVigencia.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.txtTituloVigencia.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(25D, Telerik.Reporting.Drawing.UnitType.Point);
            this.txtTituloVigencia.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtTituloVigencia.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtTituloVigencia.Value = "Vigencia Hasta";
            // 
            // txtVigencia
            // 
            this.txtVigencia.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.54512536525726318D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.5000004768371582D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtVigencia.Name = "txtVigencia";
            this.txtVigencia.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(10.499799728393555D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.5000004768371582D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtVigencia.Style.Color = System.Drawing.Color.Black;
            this.txtVigencia.Style.Font.Bold = true;
            this.txtVigencia.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(60D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.txtVigencia.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtVigencia.Value = "10/10/12";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(7.0099496841430664D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(15.5D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5.3000006675720215D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.10020112991333D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Style.BorderColor.Default = System.Drawing.Color.White;
            this.textBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox2.Style.Color = System.Drawing.Color.White;
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(42D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.Value = "CILC";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(1.8100498914718628D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(15.5D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(4.2897500991821289D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.10020112991333D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox3.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(128)))));
            this.textBox3.Style.BorderColor.Default = System.Drawing.Color.White;
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.Style.Color = System.Drawing.Color.White;
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.Font.Name = "Tahoma";
            this.textBox3.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(42D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.Value = "YPF";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(1.264925479888916D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(17.80000114440918D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(7.1350750923156738D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.2999985218048096D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox7.Style.Color = System.Drawing.Color.White;
            this.textBox7.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(18D, Telerik.Reporting.Drawing.UnitType.Point);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.Value = "PUESTO INGRESO Nº:";
            // 
            // txtPuestoIngreso
            // 
            this.txtPuestoIngreso.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(8.4001998901367188D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(17.80000114440918D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtPuestoIngreso.Name = "txtPuestoIngreso";
            this.txtPuestoIngreso.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.90964937210083D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.2999999523162842D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtPuestoIngreso.Style.BackgroundColor = System.Drawing.Color.White;
            this.txtPuestoIngreso.Style.Font.Bold = true;
            this.txtPuestoIngreso.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(35D, Telerik.Reporting.Drawing.UnitType.Point);
            this.txtPuestoIngreso.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtPuestoIngreso.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtPuestoIngreso.Value = "5";
            // 
            // panel4
            // 
            this.panel4.Name = "panel4";
            this.panel4.Style.BackgroundColor = System.Drawing.Color.White;
            // 
            // txtTipo
            // 
            this.txtTipo.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.54522591829299927D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.0001999139785767D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTipo.Name = "txtTipo";
            this.txtTipo.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(10.499799728393555D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.0499999523162842D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtTipo.Style.Color = System.Drawing.Color.Black;
            this.txtTipo.Style.Font.Bold = true;
            this.txtTipo.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(35D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.txtTipo.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtTipo.Value = "EESTSESSSSSDS";
            // 
            // CredencialVehiculos
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageFooter,
            this.reportHeader,
            this.detail});
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins.Bottom = new Telerik.Reporting.Drawing.Unit(0.10000000149011612D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Left = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Right = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Top = new Telerik.Reporting.Drawing.Unit(0.10000000149011612D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
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
            this.Width = new Telerik.Reporting.Drawing.Unit(286.20004272460938D, Telerik.Reporting.Drawing.UnitType.Mm);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private PageFooterSection pageFooter;
        private ReportHeaderSection reportHeader;
        private DetailSection detail;
        private Telerik.Reporting.TextBox txtTituloF;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox txtTarjetF;
        private Telerik.Reporting.Panel panel1;
        private Telerik.Reporting.TextBox txtTituloVehiculo;
        private Telerik.Reporting.TextBox txtMarca;
        private Telerik.Reporting.TextBox txtModelo;
        private Telerik.Reporting.Panel panel2;
        private Telerik.Reporting.TextBox txtTituloDominio;
        private Telerik.Reporting.TextBox txtPatente;
        private Telerik.Reporting.Panel panel3;
        private Telerik.Reporting.TextBox txtTituloVigencia;
        private Telerik.Reporting.TextBox txtVigencia;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.Panel pnlPrincipalReverso;
        private Telerik.Reporting.TextBox txtTituloR;
        private Telerik.Reporting.TextBox txtTarjetR;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.Panel panel5;
        private Telerik.Reporting.TextBox textBox12;
        private Telerik.Reporting.Panel panel6;
        private Telerik.Reporting.TextBox textBox13;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.Panel panel4;
        private Telerik.Reporting.Panel pnlPrincipalF;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Barcode barcode1;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox txtPuestoIngreso;
        private Telerik.Reporting.TextBox txtTipo;

    }
}