namespace Entidades
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    partial class InformexPeriodoHistorico
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformexPeriodoHistorico));
            Telerik.Reporting.Drawing.FormattingRule formattingRule1 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.labelsGroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.labelsGroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.Empresa = new Telerik.Reporting.Group();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.reportNameTextBox = new Telerik.Reporting.TextBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.txtLeyendaEncabezado = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.Periodo = new Telerik.Reporting.Group();
            this.groupFooterSection2 = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection2 = new Telerik.Reporting.GroupHeaderSection();
            this.textBox8 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // labelsGroupHeader
            // 
            this.labelsGroupHeader.Height = new Telerik.Reporting.Drawing.Unit(0.60010033845901489D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.labelsGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox5});
            this.labelsGroupHeader.Name = "labelsGroupHeader";
            this.labelsGroupHeader.PrintOnEveryPage = true;
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.7822834849357605D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.00010011057747760788D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(7.199800968170166D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox5.Style.Color = System.Drawing.Color.Black;
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox5.StyleName = "PageInfo";
            this.textBox5.Value = "= IIf(Fields.EsContratista, \"Contratista: \" + Fields.RazonSocial, \"Sub Contratist" +
    "a: \" + Fields.RazonSocial)";
            // 
            // labelsGroupFooter
            // 
            this.labelsGroupFooter.Height = new Telerik.Reporting.Drawing.Unit(0.19999994337558746D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.labelsGroupFooter.Name = "labelsGroupFooter";
            this.labelsGroupFooter.Style.Visible = false;
            // 
            // Empresa
            // 
            this.Empresa.GroupFooter = this.labelsGroupFooter;
            this.Empresa.GroupHeader = this.labelsGroupHeader;
            this.Empresa.Grouping.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.RazonSocial")});
            this.Empresa.Sorting.AddRange(new Telerik.Reporting.Data.Sorting[] {
            new Telerik.Reporting.Data.Sorting("=Fields.OrdenEmpresas", Telerik.Reporting.Data.SortDirection.Asc),
            new Telerik.Reporting.Data.Sorting("=Fields.OrdenDetalle", Telerik.Reporting.Data.SortDirection.Asc)});
            // 
            // pageHeader
            // 
            this.pageHeader.Height = new Telerik.Reporting.Drawing.Unit(0.71428573131561279D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.reportNameTextBox});
            this.pageHeader.Name = "pageHeader";
            // 
            // reportNameTextBox
            // 
            this.reportNameTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.052910052239894867D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052910052239894867D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.reportNameTextBox.Name = "reportNameTextBox";
            this.reportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(15.867624282836914D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.reportNameTextBox.Style.Visible = false;
            this.reportNameTextBox.StyleName = "PageInfo";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = new Telerik.Reporting.Drawing.Unit(0.71428573131561279D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageInfoTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(11.960190773010254D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052910052239894867D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(7.8280425071716309D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pageInfoTextBox.Style.Color = System.Drawing.Color.Black;
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = new Telerik.Reporting.Drawing.Unit(4.86074686050415D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pictureBox1,
            this.textBox1,
            this.textBox2,
            this.txtLeyendaEncabezado});
            this.reportHeader.Name = "reportHeader";
            this.reportHeader.Style.BackgroundColor = System.Drawing.Color.Empty;
            // 
            // pictureBox1
            // 
            this.pictureBox1.MimeType = "image/png";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.7999999523162842D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.1859145164489746D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            this.pictureBox1.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(15.403640747070313D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.932797908782959D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(4.3581328392028809D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox1.Style.Color = System.Drawing.Color.Black;
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox1.StyleName = "PageInfo";
            this.textBox1.Value = "Mendoza ";
            // 
            // textBox2
            // 
            this.textBox2.Format = "{0:D}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(9.0289583206176758D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.5594562292099D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(10.759275436401367D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Style.Color = System.Drawing.Color.Black;
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox2.StyleName = "PageInfo";
            this.textBox2.Value = "=Now()";
            // 
            // txtLeyendaEncabezado
            // 
            this.txtLeyendaEncabezado.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(1.2962963581085205D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(3.41269850730896D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtLeyendaEncabezado.Name = "txtLeyendaEncabezado";
            this.txtLeyendaEncabezado.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(17.673017501831055D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.2994712591171265D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.txtLeyendaEncabezado.Style.Color = System.Drawing.Color.Black;
            this.txtLeyendaEncabezado.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.txtLeyendaEncabezado.StyleName = "PageInfo";
            this.txtLeyendaEncabezado.Value = "Señor Jefe";
            // 
            // detail
            // 
            this.detail.Height = new Telerik.Reporting.Drawing.Unit(0.57363986968994141D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox6});
            this.detail.Name = "detail";
            this.detail.ItemDataBound += new System.EventHandler(this.detail_ItemDataBound);
            // 
            // textBox6
            // 
            formattingRule1.Filters.AddRange(new Telerik.Reporting.Data.Filter[] {
            new Telerik.Reporting.Data.Filter("=Fields.Comentario", Telerik.Reporting.Data.FilterOperator.Equal, "")});
            formattingRule1.Style.Visible = false;
            this.textBox6.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.textBox6.Format = "{0}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(1.2908763885498047D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(9.970690734917298E-05D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(18.412082672119141D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.49416637420654297D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox6.Value = "=Fields.Comentario";
            // 
            // Periodo
            // 
            this.Periodo.GroupFooter = this.groupFooterSection2;
            this.Periodo.GroupHeader = this.groupHeaderSection2;
            this.Periodo.Grouping.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.PeriodoDesc")});
            this.Periodo.Sorting.AddRange(new Telerik.Reporting.Data.Sorting[] {
            new Telerik.Reporting.Data.Sorting("=Fields.Periodo", Telerik.Reporting.Data.SortDirection.Desc)});
            // 
            // groupFooterSection2
            // 
            this.groupFooterSection2.Height = new Telerik.Reporting.Drawing.Unit(0.27513211965560913D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.groupFooterSection2.Name = "groupFooterSection2";
            this.groupFooterSection2.Style.Visible = false;
            // 
            // groupHeaderSection2
            // 
            this.groupHeaderSection2.Height = new Telerik.Reporting.Drawing.Unit(0.62645483016967773D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.groupHeaderSection2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox8});
            this.groupHeaderSection2.Name = "groupHeaderSection2";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.23791344463825226D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.026455026119947433D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(6.9396820068359375D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.59999984502792358D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox8.Style.Color = System.Drawing.Color.Black;
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox8.StyleName = "PageInfo";
            this.textBox8.Value = "=Fields.PeriodoDesc";
            // 
            // InformexPeriodoHistorico
            // 
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            this.Periodo,
            this.Empresa});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection2,
            this.groupFooterSection2,
            this.labelsGroupHeader,
            this.labelsGroupFooter,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.detail});
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins.Bottom = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Left = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Right = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Top = new Telerik.Reporting.Drawing.Unit(0.54000002145767212D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(222)))), ((int)(((byte)(201)))));
            styleRule1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(39)))), ((int)(((byte)(28)))));
            styleRule1.Style.Font.Name = "Gill Sans MT";
            styleRule1.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(20D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule2.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(222)))), ((int)(((byte)(201)))));
            styleRule2.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(39)))), ((int)(((byte)(28)))));
            styleRule2.Style.Font.Name = "Gill Sans MT";
            styleRule2.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule3.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(39)))), ((int)(((byte)(28)))));
            styleRule3.Style.Font.Name = "Gill Sans MT";
            styleRule3.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(10D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule4.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(141)))), ((int)(((byte)(105)))));
            styleRule4.Style.Font.Name = "Gill Sans MT";
            styleRule4.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(9D, Telerik.Reporting.Drawing.UnitType.Point);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = new Telerik.Reporting.Drawing.Unit(19.841150283813477D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.NeedDataSource += new System.EventHandler(this.InformexPeriodoHistorico_NeedDataSource);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GroupHeaderSection labelsGroupHeader;
        private GroupFooterSection labelsGroupFooter;
        private Group Empresa;
        private PageHeaderSection pageHeader;
        private Telerik.Reporting.TextBox reportNameTextBox;
        private PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private ReportHeaderSection reportHeader;
        private Telerik.Reporting.PictureBox pictureBox1;
        private DetailSection detail;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox txtLeyendaEncabezado;
        private Group Periodo;
        private GroupFooterSection groupFooterSection2;
        private GroupHeaderSection groupHeaderSection2;
        private Telerik.Reporting.TextBox textBox8;

    }
}