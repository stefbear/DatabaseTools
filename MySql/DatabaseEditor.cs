using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Database.MySql
{
    public partial class DatabaseEditor : Form
    {
        public enum Mode
        {
            Add,
            Remove,
            Edit
        }
        private Mode mode = Mode.Add;

        private string database;
        private string username;
        private string password;
        private string template;

        public DatabaseEditor()
        {
            InitializeComponent();
            InitializeResources();
        }

        #region properties
        public string Database
        {
            get { return this.textBox1.Text; }
            set { database = value; }
        }

        public string Username
        {
            get { return this.textBox2.Text; }
            set { username = value; }
        }

        public string Password
        {
            get { return this.textBox3.Text; }
            set { password = value; }
        }

        public string Template
        {
            get { return template; }
            set { template = value; }
        }

        public bool UseEditedTemplate
        {
            get { return (this.checkBox1.CheckState == CheckState.Checked ? true : false); }
        }

        public string EditedTemplate
        {
            get { return this.richTextBox1.Text; }
        }

        public Mode EditorMode
        {
            get { return mode; }
            set { mode = value; }
        }
        #endregion

        private void InitializeResources()
        {
            //Strings
            groupBox1.Text = Properties.Resources.MySql_EditorDatabaseNameGroup;
            label1.Text = Properties.Resources.MySql_EditorDatabaseNameLabel;
            button1.Text = Properties.Resources.Ok;
            button2.Text = Properties.Resources.Cancel;
            groupBox3.Text = Properties.Resources.MySql_CurrentUserLabel;
            label4.Text = Properties.Resources.MySql_PasswordLabel;
            label3.Text = Properties.Resources.MySql_UsernameLabel;
            label2.Text = Properties.Resources.MySql_EditorPrivilegesLabel;
            button4.Text = Properties.Resources.MySql_RefreshButton;
            groupBox4.Text = Properties.Resources.MySql_EditorDatabaseSchemaGroup;
            checkBox1.Text = Properties.Resources.MySql_EditorDatabaseSchemaCheckBox;
            label5.Text = Properties.Resources.MySql_EditorDatabaseSchemaLabel;
            openFileDialog1.Title = Properties.Resources.MySql_OpenFileDialogTitle;
            this.Text = Properties.Resources.MySql_EditorTitle;
        }

        private void DatabaseEditor_Load(object sender, EventArgs e)
        {
            this.label8.Text = MySqlDatabaseManager.ShowGrants(username, password);
            this.textBox1.Text = database;
            this.textBox2.Text = username;
            this.textBox3.Text = password;
            this.textBox4.Text = template;
            if (template != null)
                this.richTextBox1.Text = MySqlDatabaseManager.LoadTemplate(new FileInfo(template));
            this.textBox2.Focus();
            this.textBox2.SelectAll();
            if (mode == Mode.Edit || mode == Mode.Remove)
                this.groupBox4.Enabled = false;
            if (mode == Mode.Remove)
                this.textBox1.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Refresh
            this.label8.Text = MySqlDatabaseManager.ShowGrants(this.textBox2.Text, this.textBox3.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Durchsuchen
            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            if (this.textBox4.Text != null && this.textBox4.Text.Length > 0)
                this.openFileDialog1.FileName = this.textBox4.Text;
            else
                this.openFileDialog1.FileName = String.Empty;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox4.Text = openFileDialog1.FileName;
                template = this.textBox4.Text;
                this.richTextBox1.Text = MySqlDatabaseManager.LoadTemplate(new FileInfo(template));
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Use edited Schema
            this.richTextBox1.Enabled = checkBox1.Checked;
        }

    }
}