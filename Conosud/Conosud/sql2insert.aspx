<%@ Page Language="C#" enableViewState="False" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<script runat="server">
		
		//Maxima cantidad de registros a exportar
		protected int MaxRows2Export=90000;
		
		protected SqlConnection oConn;

		void Page_Load(object sender, System.EventArgs e)
		{
			btnExport.Visible = false;
			lblTables.Visible = false;
		}

		void CreateSqlConnection() {
			try {
				oConn = new SqlConnection(fieldConnString.Text);
			} catch (Exception ex) {
				WriteErrorMessage(ex.Message);
			}
		}
		
		void ExportData(string[] tables) {
			string export = "";
			for (int a=0;a<tables.Length;a++) {
				export += Table2SqlInsert(tables[a]);
			}
			CreateExportFile(oConn.Database,export);
		}

		DataTable getDataTable(string queryString) {
			SqlCommand dbCommand = new SqlCommand(queryString,oConn);
			SqlDataAdapter da = new SqlDataAdapter();
			da.SelectCommand = dbCommand;
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}

		void CreateExportFile(string reportName, string reportData) {
			Response.Clear();
			Response.Buffer= true;
			Response.ContentType = "application/vnd.text";
			Response.Charset = "iso-8859-1";
			Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
			Response.AddHeader("content-disposition","attachment;filename=" + reportName + ".txt");
			Response.Charset = "";
			this.EnableViewState = false;
    
			System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
    
			oHtmlTextWriter.Write(reportData);
			Response.Write(oStringWriter.ToString());
			Response.End();
		}
		string Table2SqlInsert(string tableName) {
			int a,b;
			string insertText = "";
			string fieldText = "";
			string columns = "";
			string returnText = "-- " + tableName + "\r\n";

			DataTable dt = getDataTable("select * from " + tableName);

			if (dt.Rows.Count>MaxRows2Export) return returnText + "-- Max rows exceeded (" + dt.Rows.Count.ToString() + " rows / " + MaxRows2Export.ToString() + " max)\r\n\r\n";
			if (dt.Rows.Count==0) return returnText + "-- No rows in this table\r\n\r\n";

			returnText += "SET IDENTITY_INSERT " + tableName + " ON \r\nGO\r\n";

			for (a=0; a<dt.Columns.Count; a++) {
				columns += "," + dt.Columns[a].ColumnName;
			}
			columns = columns.Substring(1);

			for (b=0; b<dt.Rows.Count; b++){
				fieldText = "";
				for (a=0; a<dt.Columns.Count; a++) {
					fieldText += ",";
					if (dt.Rows[b].IsNull(dt.Columns[a].ColumnName)) {
						fieldText += "null";
					} else {
						switch (dt.Columns[a].DataType.ToString()) {
							case ("System.Char"):
							case ("System.String"):
							case ("System.Guid"):
								fieldText += "'" + dt.Rows[b][a].ToString().Replace("'","''") + "'"; 
								break;
							case ("System.DateTime"):
								fieldText += "'" + ((DateTime)dt.Rows[b][a]).ToString("s") + "'"; 
								break;
							case ("System.Boolean"):
								fieldText += ((bool)dt.Rows[b][a])?1:0; 
								break;
							default:
								fieldText += dt.Rows[b][a].ToString();
								break;
						}
					}
				}
				insertText += "INSERT INTO " + tableName + "(" + columns + ")" + " VALUES (" + fieldText.Substring(1) + ") \r\n";
			}
			returnText += insertText  + "GO\r\n" + "SET IDENTITY_INSERT " + tableName + " OFF \r\nGO\r\n\r\n";
			return returnText;
		}

		void GetDatabaseTables() {
			string query = "select [name] [TableName],  ( select top 1 rows from dbo.sysindexes where id=so.id ) [aprox_rowcount] " +
							"from dbo.sysobjects so " +
							"where xtype = 'U' " +
							"and name != 'dtproperties'";
			try {
				dgSqlTables.DataSource = getDataTable(query);
				dgSqlTables.DataBind();
			}
			catch (Exception ex) {
				WriteErrorMessage(ex.Message);
			}
		}

		void WriteErrorMessage(string errorString) {
			lblError.Text = "<p>" + errorString + "</p>";
		}
		
		
		void btnGetTables_Click(object sender, System.EventArgs e)
		{
			CreateSqlConnection();
			GetDatabaseTables();
			btnExport.Visible = true;
			lblTables.Visible = true;
			lblMaxRows.Text = MaxRows2Export.ToString();
		}

		void btnExport_Click(object sender, System.EventArgs e)
		{
			if (Request.Form["tList"]!=null && Request.Form["tList"]!=string.Empty){
				string[] tables = Request.Form["tList"].Split(',');
				CreateSqlConnection();
				ExportData(tables);
			} else {
				WriteErrorMessage("You must select at least one table.");
			}
		}
</script>
<HTML>
	<HEAD>
		<title>Sql2Insert</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style>
		A { COLOR: white; TEXT-DECORATION: none }
		A:hover { COLOR: gray }
		</style>
		<SCRIPT language="JavaScript">
function checkChangeStatus() {
 var el = document.forms[0].elements;
 for(var i = 0 ; i < el.length ; ++i) {
  if(el[i].type == "checkbox") {
   el[i].checked = !el[i].checked;
  }
 }
} ver=;database=;uid=;
		</SCRIPT>
	</HEAD>
	<body style="FONT-FAMILY: arial">
		<form id="Form1" method="post" runat="server">
			<h2>Export SQL data in Insert statements</h2>
			<P>
				<hr SIZE="1">
				<asp:label id="Label1" runat="server">Connection String</asp:label><BR>
				<asp:textbox id="fieldConnString" runat="server" style="width:100%;">server=;database=;uid=;pwd=</asp:textbox><br>
				<asp:button id="btnGetTables" runat="server" onclick="btnGetTables_Click" Text="Get Database Tables"></asp:button><asp:label id="lblError" runat="server" ForeColor="Red"></asp:label>
			</P>
			<P>
				<asp:label id="lblTables" runat="server">Database Tables / max rows:</asp:label>
				<asp:label id="lblMaxRows" runat="server" Font-Bold="True"></asp:label><BR>
				<asp:datagrid id="dgSqlTables" runat="server" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
					BackColor="White" CellPadding="3" Font-Size="X-Small" AutoGenerateColumns="False">
					<FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
					<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedItemStyle>
					<ItemStyle ForeColor="#000066"></ItemStyle>
					<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#006699"></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="TableName" HeaderText="TableName"></asp:BoundColumn>
						<asp:BoundColumn DataField="aprox_rowcount" HeaderText="RCount (aprox)">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:TemplateColumn HeaderText="&lt;a href=&quot;javascript:checkChangeStatus()&quot;&gt;sel/unsel&lt;/a&gt;">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<input type="checkbox" name="tList" value='<%# DataBinder.Eval(Container, "DataItem.TableName") %>'>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
					<PagerStyle HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
				<asp:Button id="btnExport" runat="server" onclick="btnExport_Click" Text="Export data"></asp:Button>
			</P>
		</form>
	</body>
</HTML>
