namespace Entidades
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    partial class InformeEncuadreGremial
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup4 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup5 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup6 = new Telerik.Reporting.TableGroup();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformeEncuadreGremial));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule5 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule6 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector1 = new Telerik.Reporting.Drawing.DescendantSelector();
            Telerik.Reporting.Drawing.StyleRule styleRule7 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector2 = new Telerik.Reporting.Drawing.DescendantSelector();
            Telerik.Reporting.Drawing.StyleRule styleRule8 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector3 = new Telerik.Reporting.Drawing.DescendantSelector();
            this.textBox12 = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.crosstab2 = new Telerik.Reporting.Crosstab();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.pictureBox3 = new Telerik.Reporting.PictureBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.htmlTextBox2 = new Telerik.Reporting.HtmlTextBox();
            this.pictureBox2 = new Telerik.Reporting.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // textBox12
            // 
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.49999997019767761D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox12.Style.Font.Bold = true;
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox12.StyleName = "Solstice.TableBody";
            this.textBox12.Value = "= Sum(Fields.Cantidad)";
            // 
            // textBox1
            // 
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.5D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.StyleName = "Solstice.TableGroup";
            this.textBox1.Value = "=Fields.Convenio";
            // 
            // textBox3
            // 
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99999994039535522D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox3.Style.BackgroundColor = System.Drawing.Color.White;
            this.textBox3.StyleName = "Solstice.TableGroup";
            // 
            // textBox6
            // 
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.5D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox6.StyleName = "Solstice.TableGroup";
            this.textBox6.Value = "=Fields.Contrato";
            // 
            // textBox5
            // 
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(4D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.5D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox5.StyleName = "Solstice.TableGroup";
            this.textBox5.Value = "=Fields.SubContratista";
            // 
            // textBox4
            // 
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(4D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.5D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox4.StyleName = "Solstice.TableGroup";
            this.textBox4.Value = "=Fields.Empresa";
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
            this.reportHeader.Height = new Telerik.Reporting.Drawing.Unit(0.800000011920929D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.reportHeader.Name = "reportHeader";
            this.reportHeader.Style.Visible = false;
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(3.0343575477600098D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(9.80000114440918D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.89980012178421021D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "Informe Encuadre Gremial";
            // 
            // detail
            // 
            this.detail.Height = new Telerik.Reporting.Drawing.Unit(6.1677079200744629D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox,
            this.crosstab2,
            this.pictureBox3,
            this.textBox2});
            this.detail.Name = "detail";
            // 
            // crosstab2
            // 
            this.crosstab2.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm)));
            this.crosstab2.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm)));
            this.crosstab2.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(new Telerik.Reporting.Drawing.Unit(0.49999997019767761D, Telerik.Reporting.Drawing.UnitType.Cm)));
            this.crosstab2.Body.SetCellContent(0, 0, this.textBox11);
            this.crosstab2.Body.SetCellContent(0, 1, this.textBox8);
            tableGroup2.Name = "Group2";
            tableGroup2.ReportItem = this.textBox12;
            tableGroup1.ChildGroups.Add(tableGroup2);
            tableGroup1.Grouping.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.Convenio")});
            tableGroup1.Name = "Convenio";
            tableGroup1.ReportItem = this.textBox1;
            tableGroup1.Sorting.AddRange(new Telerik.Reporting.Data.Sorting[] {
            new Telerik.Reporting.Data.Sorting("=Fields.Convenio", Telerik.Reporting.Data.SortDirection.Asc)});
            tableGroup3.Name = "Group1";
            tableGroup3.ReportItem = this.textBox3;
            this.crosstab2.ColumnGroups.Add(tableGroup1);
            this.crosstab2.ColumnGroups.Add(tableGroup3);
            this.crosstab2.Corner.SetCellContent(0, 2, this.textBox10, 2, 1);
            this.crosstab2.Corner.SetCellContent(0, 1, this.textBox9, 2, 1);
            this.crosstab2.Corner.SetCellContent(0, 0, this.textBox7, 2, 1);
            this.crosstab2.DataMember = "";
            this.crosstab2.DataSource = typeof(Entidades.InformeEncuadreGremialTemp);
            this.crosstab2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox11,
            this.textBox8,
            this.textBox1,
            this.textBox12,
            this.textBox3,
            this.textBox10,
            this.textBox9,
            this.textBox7,
            this.textBox4,
            this.textBox5,
            this.textBox6});
            this.crosstab2.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(0.099999949336051941D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(2.2677080631256104D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.crosstab2.Name = "crosstab2";
            tableGroup6.Grouping.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.Contrato")});
            tableGroup6.Name = "Contrato";
            tableGroup6.ReportItem = this.textBox6;
            tableGroup6.Sorting.AddRange(new Telerik.Reporting.Data.Sorting[] {
            new Telerik.Reporting.Data.Sorting("=Fields.Contrato", Telerik.Reporting.Data.SortDirection.Asc)});
            tableGroup5.ChildGroups.Add(tableGroup6);
            tableGroup5.Grouping.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.SubContratista")});
            tableGroup5.Name = "SubContratista";
            tableGroup5.ReportItem = this.textBox5;
            tableGroup5.Sorting.AddRange(new Telerik.Reporting.Data.Sorting[] {
            new Telerik.Reporting.Data.Sorting("=Fields.SubContratista", Telerik.Reporting.Data.SortDirection.Asc)});
            tableGroup4.ChildGroups.Add(tableGroup5);
            tableGroup4.Grouping.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.Empresa")});
            tableGroup4.Name = "Empresa";
            tableGroup4.ReportItem = this.textBox4;
            tableGroup4.Sorting.AddRange(new Telerik.Reporting.Data.Sorting[] {
            new Telerik.Reporting.Data.Sorting("=Fields.Empresa", Telerik.Reporting.Data.SortDirection.Asc)});
            this.crosstab2.RowGroups.Add(tableGroup4);
            this.crosstab2.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(14D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.5D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.crosstab2.StyleName = "Solstice.TableNormal";
            // 
            // textBox11
            // 
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.5D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox11.StyleName = "Solstice.TableBody";
            this.textBox11.Value = "=Sum(Fields.Cantidad)";
            // 
            // textBox8
            // 
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.5D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox8.Style.Font.Bold = true;
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox8.StyleName = "Solstice.TableBody";
            this.textBox8.Value = "= Sum(Fields.Cantidad)";
            // 
            // textBox10
            // 
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(1.9999998807907105D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99999988079071045D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox10.StyleName = "Solstice.TableHeader";
            this.textBox10.Value = "Contrato";
            // 
            // textBox9
            // 
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(4D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99999988079071045D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox9.StyleName = "Solstice.TableHeader";
            this.textBox9.Value = "Sub Contratista";
            // 
            // textBox7
            // 
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(4D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.99999988079071045D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox7.StyleName = "Solstice.TableHeader";
            this.textBox7.Value = "Empresa";
            // 
            // pictureBox3
            // 
            this.pictureBox3.MimeType = "image/png";
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.0340573787689209D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.7788580656051636D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pictureBox3.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            this.pictureBox3.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Value = ((object)(resources.GetObject("pictureBox3.Value")));
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(3.0343575477600098D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.89999997615814209D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(5.9657425880432129D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.88561445474624634D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.textBox2.Style.BackgroundColor = System.Drawing.Color.White;
            this.textBox2.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(4D, Telerik.Reporting.Drawing.UnitType.Mm);
            this.textBox2.StyleName = "Title";
            this.textBox2.Value = "= Parameters.Periodo";
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
            // pictureBox2
            // 
            this.pictureBox2.MimeType = "image/png";
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(3.0340573787689209D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(1.7788580656051636D, Telerik.Reporting.Drawing.UnitType.Cm));
            this.pictureBox2.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            this.pictureBox2.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Value = ((object)(resources.GetObject("pictureBox2.Value")));
            // 
            // InformeEncuadreGremial
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageFooter,
            this.reportHeader,
            this.detail});
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins.Bottom = new Telerik.Reporting.Drawing.Unit(2.5399999618530273D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Left = new Telerik.Reporting.Drawing.Unit(2.5399999618530273D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Right = new Telerik.Reporting.Drawing.Unit(2.5399999618530273D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.Margins.Top = new Telerik.Reporting.Drawing.Unit(2.5399999618530273D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
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
            styleRule5.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.Table), "Solstice.TableNormal")});
            styleRule5.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule5.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule5.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            styleRule5.Style.Color = System.Drawing.Color.Black;
            styleRule5.Style.Font.Name = "Gill Sans MT";
            styleRule5.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(8D, Telerik.Reporting.Drawing.UnitType.Point);
            descendantSelector1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Solstice.TableGroup")});
            styleRule6.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector1});
            styleRule6.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(238)))), ((int)(((byte)(228)))));
            styleRule6.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule6.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            descendantSelector2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Solstice.TableHeader")});
            styleRule7.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector2});
            styleRule7.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(39)))), ((int)(((byte)(28)))));
            styleRule7.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule7.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            styleRule7.Style.Color = System.Drawing.Color.White;
            styleRule7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            descendantSelector3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Solstice.TableBody")});
            styleRule8.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector3});
            styleRule8.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule8.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4,
            styleRule5,
            styleRule6,
            styleRule7,
            styleRule8});
            this.Width = new Telerik.Reporting.Drawing.Unit(16.351249694824219D, Telerik.Reporting.Drawing.UnitType.Cm);
            this.NeedDataSource += new System.EventHandler(this.InformeEncuadreGremial_NeedDataSource);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private ReportHeaderSection reportHeader;
        private Telerik.Reporting.TextBox titleTextBox;
        private DetailSection detail;
        private HtmlTextBox htmlTextBox2;
        private Telerik.Reporting.PictureBox pictureBox2;
        private Crosstab crosstab2;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.PictureBox pictureBox3;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox12;

    }
}