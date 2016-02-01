using System;
using System.Data;

namespace Database.GenericDataSet
{
	/// <summary>
	/// Zusammenfassung für SqlToDataSet.
	/// </summary>
	public class GenericDataSet : DataSet
	{
		private IDatabaseProvider genericDatabase;

        public GenericDataSet()
            : base()
        { }

        public GenericDataSet(string dataSetName)
            : base(dataSetName)
        { }

		public GenericDataSet(ProviderType dbProvider) : this()
		{
			InitializeGenericDatabase(dbProvider);
		}

		public GenericDataSet(
			string strDatabase,
			string strUser,
			string strPassword,
			string strTable,
			ProviderType dbProvider) : this (dbProvider)
		{
			//initialize connection (not opened yet)
			genericDatabase.Database = strDatabase;
			genericDatabase.User = strUser;
			genericDatabase.Password = strPassword;
			genericDatabase.Table = strTable;
			genericDatabase.SetupConnection();
		}

		#region properties
		public string Table
		{
			get
			{
				return genericDatabase.Table;
			}
			set
			{
				genericDatabase.Table = value;
			}
		}

		public string Database
		{
			get
			{		
				return genericDatabase.Database;
			}
			set
			{
				genericDatabase.Database = value;
				genericDatabase.SetupConnection();
			}
		}

		public string User
		{
			get
			{
				return genericDatabase.User;
			}
			set
			{
				genericDatabase.User = value;
				genericDatabase.SetupConnection();
			}
		}

		public string Password
		{
			get
			{
				return genericDatabase.Password;
			}
			set
			{
				genericDatabase.Password = value;
				genericDatabase.SetupConnection();
			}
		}

		public bool Connected
		{
			get
			{
				return genericDatabase.Connected;
			}
		}

		protected IDatabaseProvider GenericDatabase
		{
			get	{return genericDatabase;}
			set	{genericDatabase = value;}
		}
		#endregion

		protected virtual void InitializeGenericDatabase(ProviderType dbProvider)
		{
			switch (dbProvider)
			{
				case ProviderType.OleDB:
					genericDatabase = new OleDbProvider(this);
					break;
				case ProviderType.Odbc:
					genericDatabase = new OdbcProvider(this);
					break;
				case ProviderType.MySql:
					genericDatabase = new MySqlProvider(this);
					break;
				case ProviderType.MSSql:
					genericDatabase = new MsSqlProvider(this);
					break;
				case ProviderType.Oracle:
					genericDatabase = new OracleProvider(this);
					break;
				default:
					throw new NotSupportedException("Database provider not supported.");
			}
		}

		public virtual string Login()
		{
			return genericDatabase.Login();
		}

		public virtual void SetupConnection()
		{
			genericDatabase.SetupConnection();
		}

		public virtual void OpenConnection()
		{
			genericDatabase.OpenConnection();
		}

		public virtual void CloseConnection()
		{
			genericDatabase.CloseConnection();
		}

		public virtual void ConnectionInfo()
		{
			genericDatabase.ConnectionInfo();
		}

		public virtual bool CreateTable(string table, string fields)
		{	
			return genericDatabase.CreateTable(table, fields);
		}

		public virtual void Select(string sqlCommand)
		{
			genericDatabase.Select(sqlCommand);
		}

		public virtual void SelectAll()
		{
			genericDatabase.SelectAll();
		}

		public virtual void SelectAll(string orderBy)
		{
			genericDatabase.SelectAll(orderBy);
		}

		public virtual bool InsertRow(string sqlCommand, bool autoOpenClose)
		{
			return genericDatabase.InsertRow(sqlCommand, autoOpenClose);
		}

		public virtual bool UpdateRow(string sqlCommand, bool autoOpenClose)
		{
			return genericDatabase.UpdateRow(sqlCommand, autoOpenClose);
		}

		public virtual bool DeleteRow(int id, bool autoOpenClose)
		{
			return genericDatabase.DeleteRow(id, autoOpenClose);
		}

		public virtual int GetAutoID(bool autoOpenClose)
		{
			return genericDatabase.GetAutoID(autoOpenClose);
		}

		public virtual bool WriteChanges()
		{
			return genericDatabase.WriteChanges();
		}

		public virtual bool WriteChanges(DataSet ds)
		{
			return genericDatabase.WriteChanges(ds);
		}

		public virtual byte[] GetBytes(int id, string column)
		{
			return genericDatabase.GetBytes(id, column);
		}

		public virtual byte[] GetBytes(int id, string column, bool autoOpenClose)
		{
			return genericDatabase.GetBytes(id, column, autoOpenClose);
		}

		public virtual bool SaveBytes(int id, byte[] bytes, string column)
		{
			return genericDatabase.SaveBytes(id, bytes, column);
		}

		public virtual bool SaveBytes(int id, byte[] bytes, string column, bool autoOpenClose)
		{
			return genericDatabase.SaveBytes(id, bytes, column, autoOpenClose);
		}
	}
}
