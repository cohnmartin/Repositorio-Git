namespace Entidades
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    partial class InformeMapaHojaDeRuta
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformeMapaHojaDeRuta));
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup4 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup5 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup6 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.Drawing.FormattingRule formattingRule1 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.pictureBox2 = new Telerik.Reporting.PictureBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.crosstab1 = new Telerik.Reporting.Crosstab();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.htmlTextBox1 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox3 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox2 = new Telerik.Reporting.HtmlTextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // textBox8
            // 
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.657987117767334D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.7788580656051636D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox8.Value = "=Fields.Empresa";
            // 
            // textBox1
            // 
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.6394209861755371D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.7788580656051636D, Telerik.Reporting.Drawing.UnitType.Cm));
            // 
            // textBox9
            // 
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.8025918006896973D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.40000015497207642D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.textBox9.Value = "=Fields.Periodo";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = new Telerik.Reporting.Drawing.Unit(0.71437495946884155D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageInfoTextBox,
            this.currentTimeTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(13.199999809265137D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.052916664630174637D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.1512496471405029D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5.3331265449523926D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.60000002384185791D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = new Telerik.Reporting.Drawing.Unit(0.13229186832904816D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.reportHeader.Name = "reportHeader";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.70000004768371582D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(13.603965759277344D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.89980012178421021D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "Informe Mapa Hoja De Ruta";
            // 
            // pictureBox2
            // 
            this.pictureBox2.MimeType = "image/png";
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.8025918006896973D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.7788580656051636D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pictureBox2.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            this.pictureBox2.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Value = ((object)(resources.GetObject("pictureBox2.Value")));
            // 
            // detail
            // 
            this.detail.Height = new Telerik.Reporting.Drawing.Unit(3.2677083015441895D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.crosstab1,
            this.titleTextBox});
            this.detail.Name = "detail";
            // 
            // crosstab1
            // 
            this.crosstab1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(new Telerik.Reporting.Drawing.Unit(3.6579873561859131D, Telerik.Reporting.Drawing.UnitType.Cm)));
            this.crosstab1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm)));
            this.crosstab1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(new Telerik.Reporting.Drawing.Unit(3.639420747756958D, Telerik.Reporting.Drawing.UnitType.Cm)));
            this.crosstab1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(new Telerik.Reporting.Drawing.Unit(0.40000015497207642D, Telerik.Reporting.Drawing.UnitType.Cm)));
            this.crosstab1.Body.SetCellContent(0, 1, this.textBox3);
            this.crosstab1.Body.SetCellContent(0, 2, this.htmlTextBox1);
            this.crosstab1.Body.SetCellContent(0, 0, this.htmlTextBox3);
            tableGroup2.Name = "Group1";
            tableGroup3.Name = "Group4";
            tableGroup1.ChildGroups.Add(tableGroup2);
            tableGroup1.ChildGroups.Add(tableGroup3);
            tableGroup1.Grouping.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.Empresa")});
            tableGroup1.Name = "Empresa";
            tableGroup1.ReportItem = this.textBox8;
            tableGroup1.Sorting.AddRange(new Telerik.Reporting.Data.Sorting[] {
            new Telerik.Reporting.Data.Sorting("=Fields.OrdenEmpresas", Telerik.Reporting.Data.SortDirection.Asc)});
            tableGroup5.Name = "Group3";
            tableGroup4.ChildGroups.Add(tableGroup5);
            tableGroup4.Name = "Group2";
            tableGroup4.ReportItem = this.textBox1;
            this.crosstab1.ColumnGroups.Add(tableGroup1);
            this.crosstab1.ColumnGroups.Add(tableGroup4);
            this.crosstab1.Corner.SetCellContent(0, 0, this.pictureBox2);
            this.crosstab1.DataMember = "";
            this.crosstab1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox3,
            this.htmlTextBox1,
            this.htmlTextBox3,
            this.textBox8,
            this.textBox1,
            this.pictureBox2,
            this.textBox9});
            this.crosstab1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.70000004768371582D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.89999997615814209D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.crosstab1.Name = "crosstab1";
            tableGroup6.Grouping.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.Periodo")});
            tableGroup6.GroupKeepTogether = true;
            tableGroup6.Name = "Periodo";
            tableGroup6.ReportItem = this.textBox9;
            this.crosstab1.RowGroups.Add(tableGroup6);
            this.crosstab1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(10.100000381469727D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.1788582801818848D, Telerik.Reporting.Drawing.UnitType.Cm));
            // 
            // textBox3
            // 
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.40000012516975403D, Telerik.Reporting.Drawing.UnitType.Cm));
            // 
            // htmlTextBox1
            // 
            formattingRule1.Filters.AddRange(new Telerik.Reporting.Data.Filter[] {
            new Telerik.Reporting.Data.Filter("= Sum(Fields.EstadoValor)", Telerik.Reporting.Data.FilterOperator.Equal, "0")});
            formattingRule1.Style.BackgroundColor = System.Drawing.Color.Empty;
            formattingRule1.Style.Visible = false;
            this.htmlTextBox1.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.htmlTextBox1.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(11.200000762939453D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.31408530473709106D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.htmlTextBox1.Name = "htmlTextBox1";
            this.htmlTextBox1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.6394209861755371D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.40000015497207642D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.htmlTextBox1.Style.BorderColor.Default = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(166)))), ((int)(((byte)(181)))));
            this.htmlTextBox1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox1.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.htmlTextBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.htmlTextBox1.Style.Visible = true;
            this.htmlTextBox1.Value = "<a href=\"ReporteViewerPeriodoHistorico.aspx?Id={Fields.IdCabecera}&amp;EsHistoric" +
    "o=false\" target=\"_blank\">Reporte Mensual</a>";
            // 
            // htmlTextBox3
            // 
            this.htmlTextBox3.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(11.399999618530273D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.2140852212905884D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.htmlTextBox3.Name = "htmlTextBox3";
            this.htmlTextBox3.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.6579873561859131D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.40000015497207642D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.htmlTextBox3.Style.BorderColor.Default = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(166)))), ((int)(((byte)(181)))));
            this.htmlTextBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox3.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.htmlTextBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.htmlTextBox3.Value = "<a target=\"_blank\" href=\"GestionHojadeRuta.aspx?IdCabecera={Fields.IdCabecera}\">{" +
    "Fields.Estado}</a>";
            // 
            // htmlTextBox2
            // 
            this.htmlTextBox2.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(11.299999237060547D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.2140852212905884D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.htmlTextBox2.Name = "htmlTextBox2";
            this.htmlTextBox2.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2.6400654315948486D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.7788581848144531D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.htmlTextBox2.Style.BorderColor.Default = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(166)))), ((int)(((byte)(181)))));
            this.htmlTextBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox2.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.htmlTextBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.htmlTextBox2.Value = "htmlTextBox2";
            // 
            // InformeMapaHojaDeRuta
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
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
            this.Width = new Telerik.Reporting.Drawing.Unit(19.920000076293945D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.NeedDataSource += new System.EventHandler(this.InformeMapaHojaDeRuta_NeedDataSource);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private ReportHeaderSection reportHeader;
        private Telerik.Reporting.TextBox titleTextBox;
        private DetailSection detail;
        private Crosstab crosstab1;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.PictureBox pictureBox2;
        private HtmlTextBox htmlTextBox1;
        private Telerik.Reporting.TextBox textBox1;
        private HtmlTextBox htmlTextBox2;
        private HtmlTextBox htmlTextBox3;

    }
}