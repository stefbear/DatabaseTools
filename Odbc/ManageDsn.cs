using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using Framework;
using CustomControls.ManagedListView;

namespace Database.Odbc
{
	/// <summary>
	/// Description of dsn.	
	/// </summary>
	public class ManageDsn : System.Windows.Forms.Form
	{
		private OdbcDsn _preSelectedDsn;

		//Designer
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button btOK;
		private System.Windows.Forms.Button btCancel;
		private CustomControls.ManagedListView.ContainerListView containerListView1;
        private CustomControls.CoolButton button1;
        private CustomControls.CoolButton button2;
		private CustomControls.ManagedListView.ContainerListViewColumnHeader containerListViewColumnHeader1;
		private CustomControls.ManagedListView.ContainerListViewColumnHeader containerListViewColumnHeader2;
		private CustomControls.ManagedListView.ContainerListViewColumnHeader containerListViewColumnHeader3;
		private CustomControls.ManagedListView.ContainerListViewColumnHeader containerListViewColumnHeader4;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.GroupBox groupBox1;
		private CustomControls.ManagedListView.ContainerListViewColumnHeader containerListViewColumnHeader5;
        private CustomControls.CoolButton button3;
		private CustomControls.ManagedListView.ContainerListViewColumnHeader containerListViewColumnHeader6;
		private System.Windows.Forms.ImageList imageList;

		public ManageDsn()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
            InitializeResources();
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() 
		{
            this.components = new System.ComponentModel.Container();
            this.containerListView1 = new CustomControls.ManagedListView.ContainerListView();
            this.containerListViewColumnHeader1 = new CustomControls.ManagedListView.ContainerListViewColumnHeader();
            this.containerListViewColumnHeader2 = new CustomControls.ManagedListView.ContainerListViewColumnHeader();
            this.containerListViewColumnHeader3 = new CustomControls.ManagedListView.ContainerListViewColumnHeader();
            this.containerListViewColumnHeader4 = new CustomControls.ManagedListView.ContainerListViewColumnHeader();
            this.containerListViewColumnHeader5 = new CustomControls.ManagedListView.ContainerListViewColumnHeader();
            this.containerListViewColumnHeader6 = new CustomControls.ManagedListView.ContainerListViewColumnHeader();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.button1 = new CustomControls.CoolButton();
            this.button2 = new CustomControls.CoolButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new CustomControls.CoolButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // containerListView1
            // 
            this.containerListView1.AlternatingRowColor = System.Drawing.Color.WhiteSmoke;
            this.containerListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.containerListView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.containerListView1.CheckBoxes = true;
            this.containerListView1.Columns.AddRange(new CustomControls.ManagedListView.ContainerListViewColumnHeader[] {
            this.containerListViewColumnHeader1,
            this.containerListViewColumnHeader2,
            this.containerListViewColumnHeader3,
            this.containerListViewColumnHeader4,
            this.containerListViewColumnHeader5,
            this.containerListViewColumnHeader6});
            this.containerListView1.ColumnSortColor = System.Drawing.SystemColors.Window;
            this.containerListView1.ColumnTracking = true;
            this.containerListView1.DataSource = null;
            this.containerListView1.DisplayMember = "";
            this.containerListView1.Location = new System.Drawing.Point(8, 64);
            this.containerListView1.Name = "containerListView1";
            this.containerListView1.NoItemsMessage = "No Data Sources found.";
            this.containerListView1.SelectedItem = null;
            this.containerListView1.Size = new System.Drawing.Size(433, 256);
            this.containerListView1.SmallImageList = this.imageList;
            this.containerListView1.TabIndex = 1;
            this.containerListView1.ItemCheck += new CustomControls.ManagedListView.ContainerListViewEventHandler(this.containerListView1_ItemCheck);
            // 
            // containerListViewColumnHeader1
            // 
            this.containerListViewColumnHeader1.MinimumWidth = 60;
            this.containerListViewColumnHeader1.Reorderable = true;
            this.containerListViewColumnHeader1.SortDataType = CustomControls.ManagedListView.SortDataType.TextCaseSensitive;
            this.containerListViewColumnHeader1.Text = "Name";
            this.containerListViewColumnHeader1.Width = 205;
            // 
            // containerListViewColumnHeader2
            // 
            this.containerListViewColumnHeader2.DisplayIndex = 1;
            this.containerListViewColumnHeader2.MinimumWidth = 60;
            this.containerListViewColumnHeader2.Reorderable = true;
            this.containerListViewColumnHeader2.SortDataType = CustomControls.ManagedListView.SortDataType.TextCaseSensitive;
            this.containerListViewColumnHeader2.Text = "Driver";
            this.containerListViewColumnHeader2.Width = 215;
            // 
            // containerListViewColumnHeader3
            // 
            this.containerListViewColumnHeader3.DisplayIndex = 2;
            this.containerListViewColumnHeader3.MinimumWidth = 60;
            this.containerListViewColumnHeader3.Reorderable = true;
            this.containerListViewColumnHeader3.SortDataType = CustomControls.ManagedListView.SortDataType.TextCaseSensitive;
            this.containerListViewColumnHeader3.Text = "Server";
            this.containerListViewColumnHeader3.Visible = false;
            this.containerListViewColumnHeader3.Width = 120;
            // 
            // containerListViewColumnHeader4
            // 
            this.containerListViewColumnHeader4.DisplayIndex = 3;
            this.containerListViewColumnHeader4.MinimumWidth = 60;
            this.containerListViewColumnHeader4.Reorderable = true;
            this.containerListViewColumnHeader4.SortDataType = CustomControls.ManagedListView.SortDataType.TextCaseSensitive;
            this.containerListViewColumnHeader4.Text = "Description";
            this.containerListViewColumnHeader4.Visible = false;
            this.containerListViewColumnHeader4.Width = 160;
            // 
            // containerListViewColumnHeader5
            // 
            this.containerListViewColumnHeader5.DisplayIndex = 4;
            this.containerListViewColumnHeader5.MinimumWidth = 60;
            this.containerListViewColumnHeader5.Reorderable = true;
            this.containerListViewColumnHeader5.SortDataType = CustomControls.ManagedListView.SortDataType.TextCaseSensitive;
            this.containerListViewColumnHeader5.Text = "Path";
            this.containerListViewColumnHeader5.Visible = false;
            this.containerListViewColumnHeader5.Width = 200;
            // 
            // containerListViewColumnHeader6
            // 
            this.containerListViewColumnHeader6.DisplayIndex = 5;
            this.containerListViewColumnHeader6.MinimumWidth = 60;
            this.containerListViewColumnHeader6.Reorderable = true;
            this.containerListViewColumnHeader6.SortDataType = CustomControls.ManagedListView.SortDataType.TextCaseSensitive;
            this.containerListViewColumnHeader6.Text = "Database";
            this.containerListViewColumnHeader6.Width = 200;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(452, 334);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 24);
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "Cancel";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btOK.Location = new System.Drawing.Point(349, 334);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(92, 24);
            this.btOK.TabIndex = 8;
            this.btOK.Text = "OK";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Image = global::Database.Properties.Resources.mAddTableHS;
            this.button1.Location = new System.Drawing.Point(452, 64);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(6, 4, 10, 4);
            this.button1.Size = new System.Drawing.Size(92, 24);
            this.button1.TabIndex = 3;
            this.button1.Text = "&Add";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Image = global::Database.Properties.Resources.mDeleteHS;
            this.button2.Location = new System.Drawing.Point(452, 94);
            this.button2.Name = "button2";
            this.button2.Padding = new System.Windows.Forms.Padding(6, 4, 10, 4);
            this.button2.Size = new System.Drawing.Size(92, 24);
            this.button2.TabIndex = 3;
            this.button2.Text = "&Remove";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButton1.Location = new System.Drawing.Point(8, 24);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(91, 18);
            this.radioButton1.TabIndex = 10;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "System DSN";
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButton2.Location = new System.Drawing.Point(128, 24);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(79, 18);
            this.radioButton2.TabIndex = 10;
            this.radioButton2.Text = "User DSN";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(433, 48);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Show only System Data Sources or User Data Sources:";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button3.Image = global::Database.Properties.Resources.mDataSet_TableView;
            this.button3.Location = new System.Drawing.Point(452, 124);
            this.button3.Name = "button3";
            this.button3.Padding = new System.Windows.Forms.Padding(6, 4, 10, 4);
            this.button3.Size = new System.Drawing.Size(92, 24);
            this.button3.TabIndex = 3;
            this.button3.Text = "&Edit";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ManageDsn
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(553, 369);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.containerListView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ManageDsn";
            this.Text = "Manage Data Sources";
            this.Load += new System.EventHandler(this.ManageDsn_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region properties
		public OdbcDsn SelectedDsn
		{
			get {return containerListView1.CheckedItems.Count > 0 ? (OdbcDsn)containerListView1.CheckedItems[0].Tag : null;}
			set {_preSelectedDsn = value;}
		}
		#endregion

		private void ManageDsn_Load(object sender, System.EventArgs e)
		{
			GetDsnList();
		}

        private void InitializeResources()
        {
            //imageList
            ImageListHelper.AddFromImage(Properties.Resources.mDatabase, imageList);
            ImageListHelper.AddFromImage(Properties.Resources.mDatabase_readonly, imageList);
            //strings
            containerListView1.NoItemsMessage = Properties.Resources.Odbc_NoItemsMesage;
            containerListViewColumnHeader1.Text = Properties.Resources.Odbc_NameColumn;
            containerListViewColumnHeader2.Text = Properties.Resources.Odbc_DriverColumn;
            containerListViewColumnHeader3.Text = Properties.Resources.Odbc_ServerColumn;
            containerListViewColumnHeader4.Text = Properties.Resources.Odbc_DescriptionColumn;
            containerListViewColumnHeader5.Text = Properties.Resources.Odbc_PathColumn;
            containerListViewColumnHeader6.Text = Properties.Resources.Odbc_DatabaseColumn;
            btCancel.Text = Properties.Resources.Cancel;
            btOK.Text = Properties.Resources.Ok;
            button1.Text = Properties.Resources.MySql_AddButton;
            button2.Text = Properties.Resources.MySql_RemoveButton;
            button3.Text = Properties.Resources.MySql_EditButton;
            radioButton1.Text = Properties.Resources.Odbc_SystemDsnRadioText;
            radioButton2.Text = Properties.Resources.Odbc_UserDsnRadioText;
            groupBox1.Text = Properties.Resources.Odbc_DsnSelectionGroup;
            this.Text = Properties.Resources.Odbc_ManageDsnTitle;
        }

		private void GetDsnList()
		{
			//get system|user dsn list
			OdbcDsn[] sDsns = radioButton1.Checked ? OdbcManager.GetSystemDsnList() : OdbcManager.GetUserDsnList();
            if (sDsns != null)
            {
                ContainerListViewItem[] clvis = new ContainerListViewItem[sDsns.Length];
                ContainerListViewItem clvi = null;
                int i = 0;
                foreach (OdbcDsn dsn in sDsns)
                {
                    clvi = new ContainerListViewItem(dsn.DsnName);
                    clvi.SubItems.AddRange(new string[]{
													   dsn.DsnDriverName,
													   dsn.DsnServerName,
													   dsn.DsnDescription,
													   dsn.DsnDriverPath,
													   dsn.DsnDatabase});
                    clvi.Tag = dsn;
                    clvi.ImageIndex = clvi.SubItems[1].Text.ToLower().StartsWith("microsoft access") ? 0 : 1;
                    if (dsn == _preSelectedDsn)
                        clvi.Checked = true;
                    clvis[i++] = clvi;
                }
                containerListView1.BeginUpdate();
                containerListView1.Items.Clear();
                containerListView1.Sort(0, SortOrder.Ascending, true);
                containerListView1.Items.AddRange(clvis);
                containerListView1.EndUpdate();
            }
            else
                containerListView1.Items.Clear();
		}

		private void containerListView1_ItemCheck(object sender, CustomControls.ManagedListView.ContainerListViewEventArgs e)
		{
			bool isChecked = e.Item.Checked;
			//only one item must be selected
			//uncheck all other items
			containerListView1.BeginUpdate();
			for (int i=0; i<containerListView1.Items.Count; i++)
				containerListView1.Items[i].Checked = false;

			e.Item.Checked = isChecked;
			containerListView1.EndUpdate();
		}

		private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
		{
			GetDsnList();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			//add
			AddDsn();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			//remove
			RemoveDsn();		
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			//edit
			EditDsn();
		}

		public void AddDsn()
		{
			//add new system or user dsn
			if (OdbcManager.SQLCreateDataSource(this.Handle, "New DSN"))
				//refresh listView
				GetDsnList();
		}

		public void RemoveDsn()
		{
			//remove selected OdbcDsn
			//get selected clvi
			if (containerListView1.SelectedItems.Count <= 0)
				return;

			ContainerListViewItem clvi = containerListView1.SelectedItems[0];
			
			if (MessageBox.Show("Remove selected DSN: '" + clvi.Text + "' ?", "Remove selected DSN?",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				return;

			//system or user dsn?
			bool systemDsn = radioButton1.Checked;
			OdbcDsn dsn = (OdbcDsn)clvi.Tag;

			//remove dsn and if successful clvi from listview
			if (OdbcManager.SQLConfigDataSource(
				this.Handle,
				systemDsn ? (int)OdbcManager.SqlRequest.ODBC_REMOVE_SYS_DSN : (int)OdbcManager.SqlRequest.ODBC_REMOVE_DSN,
				OdbcManager.GetOdbcDriver(dsn.DsnDriverName).OdbcDriverName,
				"DSN=" + dsn.DsnName))
				clvi.Delete();
		}

		public void EditDsn()
		{
			//edit selected OdbcDsn
			//get selected clvi
			if (containerListView1.SelectedItems.Count <= 0)
				return;

			ContainerListViewItem clvi = containerListView1.SelectedItems[0];

			//system or user dsn?
			bool systemDsn = radioButton1.Checked;
			OdbcDsn dsn = (OdbcDsn)clvi.Tag;

			//open config window
			if (OdbcManager.SQLConfigDataSource(
				this.Handle,
				systemDsn ? (int)OdbcManager.SqlRequest.ODBC_CONFIG_SYS_DSN : (int)OdbcManager.SqlRequest.ODBC_CONFIG_DSN,
				OdbcManager.GetOdbcDriver(dsn.DsnDriverName).OdbcDriverName,
				"DSN=" + dsn.DsnName)) //"DSN=Hallo\0Uid=admin\0pwd=\0DBQ=C:\\Temp\\hallo.mdb\0");
			{
				//refresh listView
				GetDsnList();
			}
		}
		
	}
}
