using System;
using System.Data;
using System.Diagnostics;

using System.Data.SqlClient;

namespace Database.GenericDataSet
{
	/// <summary>
	/// Zusammenfassung für MsSqlProvider.
	/// </summary>
	public class MsSqlProvider : IDatabaseProvider
	{
		private DataSet dataSet;

		//MsSql
		private string			msSqlConnectionString = String.Empty;
		private SqlConnection	msSqlConnection;
		private SqlDataAdapter	msSqlDataAdapter;

		private string	database;
		private string	user;
		private string	password;
		private string	table;
		private bool	isConnected;

		public MsSqlProvider(DataSet ds)
		{
			dataSet = ds;
		}

		#region properties
		public string Table
		{
			get {return table;}
			set {table = value;}
		}

		public string Database
		{
			get {return database;}
			set 
			{
				database = value;
			}
		}

		public string User
		{
			get {return user;}
			set 
			{
				user = value;
			}
		}

		public string Password
		{
			get {return password;}
			set 
			{
				password = value;
			}
		}

		public bool Connected
		{
			get	{return isConnected;}
		}

		protected DataSet DataSet
		{
			get {return dataSet;}
		}
		#endregion

		public virtual string Login()
		{
            lock (msSqlConnection)
            {
                //check MsSql Connection
                if (msSqlConnection == null)
                    throw new Exception("No connection established.");

                //MsSql Connection
                try
                {
                    //open initialized connection to check parameters
                    //(pw, user, database)
                    msSqlConnection.Open();

                    msSqlConnection.Close();

                    isConnected = false;
                    return String.Empty;
                }
                catch (SqlException e)
                {
                    //return error
                    isConnected = false;

                    msSqlConnection.Close();
                    return e.Message;
                }
            }
		}

		public virtual void SetupConnection()
		{
			//MsSql Connection
			//Connection string for MySql 3.51
			/*mySqlConnectionString = "DRIVER={MySQL MsSql 3.51 Driver};" + 
														"SERVER=localhost;" +
														"DATABASE=" + database + ";" +
														"UID=" + user + ";" +
														"PASSWORD=" + password + ";" +
														"OPTION=3";
					*/

			//Connection string for MySql and configured DSN
			msSqlConnectionString = "DSN=" + database + ";" +
				"UID=" + user + ";" +
				"PASSWORD=" + password + ";" +
				"OPTION=3";
				
			//Connect to database using MySql
			msSqlConnection = new SqlConnection(msSqlConnectionString);
		}

		public virtual void OpenConnection()
		{
            lock (msSqlConnection)
            {
                try
                {
                    //open Connection
                    isConnected = false;
                    if (msSqlConnection != null && msSqlConnection.State != ConnectionState.Open)
                    {
                        msSqlConnection.Open();
                        isConnected = true;
                    }
                }
                catch (SqlException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
                    }
                }
            }
		}

		public virtual void CloseConnection()
		{
            lock (msSqlConnection)
            {
                try
                {
                    //close Connection
                    if (msSqlConnection.State != ConnectionState.Closed)
                    {
                        msSqlConnection.Close();
                        isConnected = false;
                    }
                }
                catch (SqlException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
                    }
                }
            }
		}

		public virtual void ConnectionInfo()
		{
			if (!isConnected)
			{
				Trace.WriteLine("Keine Verbindung hergestellt.", "Error");
				return;
			}

            lock (msSqlConnection)
            {
                msSqlConnection.Open();

                //Display connection information
                Trace.WriteLine("Connection Information: ", "Information");
                Trace.WriteLine("\tConnection String: " + msSqlConnection.ConnectionString, "Information");
                Trace.WriteLine("\tConnection Timeout: " + msSqlConnection.ConnectionTimeout, "Information");
                Trace.WriteLine("\tDatabase: " + msSqlConnection.Database, "Information");
                Trace.WriteLine("\tDataSource: " + msSqlConnection.DataSource, "Information");
                //Trace.WriteLine("\tDriver: " + mySqlConnection.Driver, "Information");
                Trace.WriteLine("\tServerVersion: " + msSqlConnection.ServerVersion, "Information");

                msSqlConnection.Close();
            }
		}

		public virtual bool CreateTable(string table, string fields)
		{
			//MsSql
			try
			{
				//open Connection
				msSqlConnection.Open();
				
				//Delete manually
				string sqlCommand = "CREATE TABLE IF NOT EXISTS " + table + " " + fields;
				SqlCommand mySqlCommand = new SqlCommand(sqlCommand, msSqlConnection);
				mySqlCommand.ExecuteNonQuery();
			}
			catch (SqlException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
				}
				msSqlConnection.Close();
				//return with error			
				return false;
			}
			msSqlConnection.Close();
			//everything went fine
			return true;
		}

		public virtual void Select(string sqlCommand)
		{
			//Connection string for MySql 3.51
			/*mySqlConnectionString = "DRIVER={MySQL MsSql 3.51 Driver};" + 
														"SERVER=localhost;" +
														"DATABASE=" + database + ";" +
														"UID=" + user + ";" +
														"PASSWORD=" + password + ";" +
														"OPTION=3";
					*/

			//check MsSql Connection
			if (msSqlConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//mySqlConnection.Open();

				//read all available datasets (rows, from value parameter strSqlCommand)
				SqlCommand mySqlCommand = new SqlCommand(sqlCommand, msSqlConnection);
				msSqlDataAdapter = new SqlDataAdapter();
				msSqlDataAdapter.SelectCommand = mySqlCommand;
				
				//Create CommandBuilder to automatically generate Update/Delete/Insert-Commands
				//for DataAdapter used by method writeToDb()
				SqlCommandBuilder mySqlCommandBuilder = new SqlCommandBuilder(msSqlDataAdapter);

				//Add Tablenames to Dataset	
				msSqlDataAdapter.TableMappings.Clear();
				msSqlDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				msSqlDataAdapter.Fill(dataSet, table);
	
				//connection established
				isConnected = true;
			}
			catch (SqlException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
				}
				//return with error
				isConnected = false;
			}
			finally
			{
				//mySqlConnection.Close();
			}
		}

		public virtual void SelectAll()
		{
			//Connection string for MySql 3.51
			/*mySqlConnectionString = "DRIVER={MySQL MsSql 3.51 Driver};" + 
														"SERVER=localhost;" +
														"DATABASE=" + database + ";" +
														"UID=" + user + ";" +
														"PASSWORD=" + password + ";" +
														"OPTION=3";
					*/

			//check MsSql Connection
			if (msSqlConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//mySqlConnection.Open();
													
				//read all Datasets from table
				SqlCommand mySqlCommand = new SqlCommand("SELECT * FROM " + table, msSqlConnection);
				msSqlDataAdapter = new SqlDataAdapter();
				msSqlDataAdapter.SelectCommand = mySqlCommand;					
				
				//Create CommandBuilder to generate Update/Delete/Insert-Commands for writeToDb()
				SqlCommandBuilder mySqlCommandBuilder = new SqlCommandBuilder(msSqlDataAdapter);
					
				//Add Tablenames to Dataset
				msSqlDataAdapter.TableMappings.Clear();
				msSqlDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				msSqlDataAdapter.Fill(dataSet, table);

				//connection established
				isConnected = true;		
			}
			catch (SqlException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
				}

				//return with error
				isConnected=false;
			}
			finally
			{
				//mySqlConnection.Close();
			}
		}

		public virtual void SelectAll(string orderBy)
		{
			//check MsSql Connection
			if (msSqlConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//mySqlConnection.Open();
													
				//read all datasets (rows) from table
				SqlCommand mySqlCommand = new SqlCommand("SELECT * FROM " + table + " ORDER BY " + orderBy, msSqlConnection);
				msSqlDataAdapter = new SqlDataAdapter();
				msSqlDataAdapter.SelectCommand = mySqlCommand;					
				
				//Create CommandBuilder to automatically generate Update/Delete/Insert-Commands
				//for DataAdapter used by method writeToDb()
				SqlCommandBuilder mySqlCommandBuilder = new SqlCommandBuilder(msSqlDataAdapter);
					
				//Add Tablenames to Dataset
				msSqlDataAdapter.TableMappings.Clear();
				msSqlDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				msSqlDataAdapter.Fill(dataSet, table);

				//connection established
				isConnected = true;
			}
			catch (SqlException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
				}

				//connection not established
				isConnected = false;
			}
			finally
			{
				//mySqlConnection.Close();
			}
		}

		public virtual bool InsertRow(string sqlCommand, bool autoOpenClose)
		{
            lock (msSqlConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        msSqlConnection.Open();

                    //Insert manually				
                    string sqlInsertCommand = "INSERT INTO " + table + sqlCommand;
                    SqlCommand mySqlCommand = new SqlCommand(sqlInsertCommand, msSqlConnection);
                    mySqlCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (SqlException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
                    }
                    //close Connection
                    if (autoOpenClose)
                        msSqlConnection.Close();
                    //return with error
                    return false;
                }
                //close Connection
                if (autoOpenClose)
                    msSqlConnection.Close();
                //everything went fine
                return true;
            }
		}

		public virtual bool UpdateRow(string sqlCommand, bool autoOpenClose)
		{
            lock (msSqlConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        msSqlConnection.Open();

                    //Update manually
                    string sqlUpdateCommand = "UPDATE " + table + " SET " + sqlCommand;
                    SqlCommand mySqlCommand = new SqlCommand(sqlUpdateCommand, msSqlConnection);
                    mySqlCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (SqlException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
                    }
                    if (autoOpenClose)
                        msSqlConnection.Close();
                    //return with error
                    return false;
                }
                if (autoOpenClose)
                    msSqlConnection.Close();
                //everything went fine
                return true;
            }
		}

		public virtual bool DeleteRow(int id, bool autoOpenClose)
		{
            lock (msSqlConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        msSqlConnection.Open();

                    //Delete manually
                    string sqlDeleteCommand = "DELETE FROM " + table + " WHERE id='" + id + "'";
                    SqlCommand mySqlCommand = new SqlCommand(sqlDeleteCommand, msSqlConnection);
                    mySqlCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (SqlException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
                    }
                    if (autoOpenClose)
                        msSqlConnection.Close();
                    //return with error			
                    return false;
                }
                if (autoOpenClose)
                    msSqlConnection.Close();
                //everything went fine
                return true;
            }
		}

		public virtual int GetAutoID(bool autoOpenClose)
		{
            lock (msSqlConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        msSqlConnection.Open();

                    //retrieve id
                    //stored procedure: Last_insert_id() doesn't work ?
                    string sqlCommand = "SELECT max(id) FROM " + table;
                    SqlCommand mySqlCommand = new SqlCommand(sqlCommand, msSqlConnection);

                    int autoIncrement = -1;

                    //fetch
                    SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                    while (mySqlDataReader.Read())
                        autoIncrement = mySqlDataReader.GetInt32(0);
                    mySqlDataReader.Close();

                    //close Connection
                    if (autoOpenClose)
                        msSqlConnection.Close();

                    return autoIncrement;
                }
                catch (SqlException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
                    }
                    //close Connection
                    if (autoOpenClose)
                        msSqlConnection.Close();

                    //return with error
                    return -1;
                }
            }
		}

		public virtual bool WriteChanges()
		{
			//check MsSql DataAdapter
			if (msSqlDataAdapter == null)
				throw new Exception("DataAdapter not initialized.");

			//MsSql
			try
			{
				//Connect to database using MsSql
				//no need to open the connection manually, DataAdapter will open and close
				//it automatically
				//mySqlConnection.Open();
				
				//Update DataBase
				msSqlDataAdapter.Update(dataSet, dataSet.Tables[0].TableName);

				//finalize changes to DataSet
				//do not use this Method in conjunction with DataAdpater.Update()
				//Update() performs the necessary AcceptChanges automatically on each
				//row successfully updated
				//this.AcceptChanges();
			}
			catch (SqlException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
				}
				//mySqlConnection.Close();
				//return with error
				return false;
			}
			//mySqlConnection.Close();
			//everything went fine
			return true;
		}

		public virtual bool WriteChanges(DataSet ds)
		{
			//check MsSql DataAdapter
			if (msSqlDataAdapter == null)
				throw new Exception("DataAdapter not initialized.");

			//MsSql
			try
			{
				//Update DataBase
				msSqlDataAdapter.Update(ds);
			}
			catch (SqlException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
				}
				//return with error
				return false;
			}
			//everything went fine
			return true;
		}

		public virtual byte[] GetBytes(int id, string column)
		{
			return GetBytes(id, column, true);
		}

		public virtual byte[] GetBytes(int id, string column, bool autoOpenClose)
		{
            lock (msSqlConnection)
            {
                byte[] result;
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        msSqlConnection.Open();

                    //select
                    string sqlSelectCommand = "SELECT `" + column + "` FROM " + table + " WHERE id=" + id;
                    SqlCommand mySqlCommand = new SqlCommand(sqlSelectCommand, msSqlConnection);

                    //read all at once
                    result = (byte[])mySqlCommand.ExecuteScalar();
                }
                catch (SqlException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
                    }

                    //return with error
                    if (autoOpenClose)
                        msSqlConnection.Close();
                    return null;
                }

                if (autoOpenClose)
                    msSqlConnection.Close();
                //everything went fine
                return result;
            }
		}

		public virtual bool	SaveBytes(int id, byte[] bytes, string column)
		{
			return SaveBytes(id, bytes, column, true);
		}

		public virtual bool	SaveBytes(int id, byte[] bytes, string column, bool autoOpenClose)
		{
            lock (msSqlConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        msSqlConnection.Open();

                    //update binary as SqlCommand parameter
                    string sqlUpdateCommand = "UPDATE " + table + " SET `" + column + "`=@" + column + " WHERE id=" + id;
                    SqlCommand mySqlCommand = new SqlCommand(sqlUpdateCommand, msSqlConnection);
                    if (bytes != null)
                        mySqlCommand.Parameters.Add("@" + column, SqlDbType.Binary, bytes.Length).Value = bytes;
                    else
                        mySqlCommand.Parameters.Add("@" + column, SqlDbType.Binary, 0).Value = System.DBNull.Value;

                    mySqlCommand.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("MsSql-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("MsSql: " + e.Errors[i].State, "Error");
                    }

                    if (autoOpenClose)
                        msSqlConnection.Close();
                    //return with error
                    return false;
                }

                if (autoOpenClose)
                    msSqlConnection.Close();
                //everything went fine
                return true;
            }
		}

	}
}
