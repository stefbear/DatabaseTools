using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.ComponentModel;

using Framework;
using CustomControls.ManagedListView;

namespace Database.MySql
{
    public class DatabaseBrowser : System.Windows.Forms.Form
	{
		private string _selectedDatabase;

		//Designer
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button btOK;
		private System.Windows.Forms.Button btCancel;
		private CustomControls.ManagedListView.ContainerListView containerListView1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
        private CustomControls.ManagedListView.ContainerListViewColumnHeader containerListViewColumnHeader1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private Label label2;
        private Label label3;
		private System.Windows.Forms.ImageList imageList;

		public DatabaseBrowser()
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
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // containerListView1
            // 
            this.containerListView1.AlternatingRowColor = System.Drawing.Color.WhiteSmoke;
            this.containerListView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.containerListView1.CheckBoxes = true;
            this.containerListView1.Columns.AddRange(new CustomControls.ManagedListView.ContainerListViewColumnHeader[] {
            this.containerListViewColumnHeader1});
            this.containerListView1.ColumnSortColor = System.Drawing.SystemColors.Window;
            this.containerListView1.ColumnTracking = true;
            this.containerListView1.DataSource = null;
            this.containerListView1.DisplayMember = "";
            this.containerListView1.Location = new System.Drawing.Point(8, 64);
            this.containerListView1.Name = "containerListView1";
            this.containerListView1.NoItemsMessage = "No Databases found.";
            this.containerListView1.SelectedItem = null;
            this.containerListView1.Size = new System.Drawing.Size(448, 272);
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
            this.containerListViewColumnHeader1.Width = 360;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(471, 344);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 24);
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "Cancel";
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btOK.Location = new System.Drawing.Point(364, 344);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(92, 24);
            this.btOK.TabIndex = 8;
            this.btOK.Text = "OK";
            // 
            // button2
            // 
            this.button2.Image = global::Database.Properties.Resources.mDeleteHS;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(468, 96);
            this.button2.Name = "button2";
            this.button2.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.button2.Size = new System.Drawing.Size(92, 24);
            this.button2.TabIndex = 3;
            this.button2.Text = "&Remove";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(457, 39);
            this.label1.TabIndex = 10;
            this.label1.Text = "Below you can see a list of MySql databases installed on your MySql server.\r\nNote" +
                " that this list only contains databases that was granted privilege to read for t" +
                "he current user.\r\n\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(5, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Current User:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(110, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "label3";
            // 
            // button1
            // 
            this.button1.Image = global::Database.Properties.Resources.mAddTableHS;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(468, 64);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.button1.Size = new System.Drawing.Size(92, 24);
            this.button1.TabIndex = 3;
            this.button1.Text = "&Add";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Image = global::Database.Properties.Resources.mDataSet_TableView;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(468, 128);
            this.button3.Name = "button3";
            this.button3.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.button3.Size = new System.Drawing.Size(92, 24);
            this.button3.TabIndex = 3;
            this.button3.Text = "&Edit";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // DatabaseBrowser
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(573, 382);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.containerListView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "DatabaseBrowser";
            this.Text = "MySql Database Browser";
            this.Load += new System.EventHandler(this.DatabaseBrowser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region properties
		public string SelectedDatabase
		{
			get {return containerListView1.CheckedItems.Count > 0 ? (string)containerListView1.CheckedItems[0].Tag : null;}
			set {_selectedDatabase = value;}
		}
		#endregion

        private void DatabaseBrowser_Load(object sender, System.EventArgs e)
		{
			GetDatabaseList();
		}

        private void InitializeResources()
        {
            //imageList
            ImageListHelper.AddFromImage(Properties.Resources.mDatabase, imageList);
            //strings
            this.Text = Properties.Resources.MySql_DatabaseBrowserTitle;
            button3.Text = Properties.Resources.MySql_EditButton;
            button1.Text = Properties.Resources.MySql_AddButton;
            label2.Text = Properties.Resources.MySql_CurrentUserLabel;
            label1.Text = Properties.Resources.MySql_DatabaseBrowserLabel;
            button2.Text = Properties.Resources.MySql_RemoveButton;
            btOK.Text = Properties.Resources.Ok;
            btCancel.Text = Properties.Resources.Cancel;
            containerListView1.NoItemsMessage = Properties.Resources.MySql_NoItemsMessage;
        }
		
		private void GetDatabaseList()
		{
			//get all installed mysql databases and list them in containerlistview
            StringCollection dbs = MySqlDatabaseManager.Databases;
            if (dbs != null)
            {
                ContainerListViewItem[] clvis = new ContainerListViewItem[dbs.Count];
                ContainerListViewItem clvi = null;
                int i = 0;
                foreach (string db in dbs)
                {
                    clvi = new ContainerListViewItem(db);
                    clvi.Tag = db;
                    clvi.ImageIndex = 0;
                    if (db == _selectedDatabase)
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

            this.label3.Text = MySqlDatabaseManager.Username;
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

		private void button1_Click(object sender, System.EventArgs e)
		{
			//add
			AddDatabase();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			//remove
			RemoveDatabase();		
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			//edit
			EditDatabase();
		}

        private void AddDatabase()
		{
			//add new database
            DatabaseEditor de = new DatabaseEditor();
            de.Username = MySqlDatabaseManager.Username;
            de.Password = MySqlDatabaseManager.Password;
            de.Template = MySqlDatabaseManager.Template;
            if (this.containerListView1.CheckedItems.Count > 0)
                de.Database = containerListView1.CheckedItems[0].Text;

			if (de.ShowDialog() == DialogResult.OK)
            {
                bool ok = MySqlDatabaseManager.SetupDatabase(de.Database, de.Username, de.Password);
                
                if (ok && de.UseEditedTemplate)
                    ok = MySqlDatabaseManager.CreateTables(de.Database, de.Username, de.Password, de.EditedTemplate);
                else if (ok)
                    ok = MySqlDatabaseManager.CreateTables(de.Database, de.Username, de.Password, new System.IO.FileInfo(de.Template));
                
                if (ok)
				    //refresh listView
				    GetDatabaseList();
            }
		}

        private void RemoveDatabase()
		{
			//remove selected database
            if (containerListView1.CheckedItems.Count <= 0)
				return;

            DatabaseEditor de = new DatabaseEditor();
            de.Username = MySqlDatabaseManager.Username;
            de.Password = MySqlDatabaseManager.Password;
            de.Template = MySqlDatabaseManager.Template;
            de.Database = containerListView1.CheckedItems[0].Text;
            de.EditorMode = DatabaseEditor.Mode.Remove;

            if (de.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show(string.Format(Properties.Resources.MB_AskDelete, de.Database), Properties.Resources.MB_AskDeleteTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes && 
                    MySqlDatabaseManager.DeleteDatabase(de.Database, de.Username, de.Password))
                    //refresh listView
                    //GetDatabaseList();
                    //remove database and if successful clvi from listview
                    containerListView1.CheckedItems[0].Delete();
            }
		}

        private void EditDatabase()
		{
            //edit selected database
            if (containerListView1.CheckedItems.Count <= 0)
                return;

            DatabaseEditor de = new DatabaseEditor();
            de.Username = MySqlDatabaseManager.Username;
            de.Password = MySqlDatabaseManager.Password;
            de.Template = MySqlDatabaseManager.Template;
            de.Database = containerListView1.CheckedItems[0].Text;
            de.EditorMode = DatabaseEditor.Mode.Edit;

            if (de.ShowDialog() == DialogResult.OK)
            {
                if (MySqlDatabaseManager.RenameDatabase(de.Database, de.Username, de.Password))
                    //refresh listView
                    GetDatabaseList();
            }
		}
		
	}
}