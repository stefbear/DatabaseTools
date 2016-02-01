using System;
using System.Data;
using System.Diagnostics;

using MySql.Data.MySqlClient;

namespace Database.GenericDataSet
{
	/// <summary>
	/// Custom MySql database provider implementing IDatabaseProvider Protocol.
	/// </summary>
	public class MySqlProvider : IDatabaseProvider
	{
		private DataSet dataSet;

		//MySql
		private string				mySqlConnectionString = String.Empty;
		private MySqlConnection		mySqlConnection;
		private MySqlDataAdapter	mySqlDataAdapter;

		private string	database;
		private string	user;
		private string	password;
		private string	table;
		private bool	isConnected;

		public MySqlProvider(DataSet ds)
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
            lock (mySqlConnection)
            {
                //check MySql Connection
                if (mySqlConnection == null)
                    throw new Exception("No connection established.");

                //MySql Connection
                try
                {
                    //open initialized connection to check parameters
                    //(pw, user, database)
                    mySqlConnection.Open();

                    mySqlConnection.Close();

                    isConnected = false;
                    return String.Empty;
                }
                catch (MySqlException ex)
                {
                    //return error
                    isConnected = false;

                    mySqlConnection.Close();
                    return ex.Message;
                }
            }
		}

		public virtual void SetupConnection()
		{
			//MySQL Connection
			//Connection string for MySQL
			mySqlConnectionString = "Database=" + database + ";" +
				"Data Source=localhost;" + 
				"User Id=" + user + ";" +
				"Password=" + password;
				
			//Connect to database using MySQL
			mySqlConnection = new MySqlConnection(mySqlConnectionString);
		}

		public virtual void OpenConnection()
		{
            lock (mySqlConnection)
            {
                try
                {
                    //open Connection
                    isConnected = false;
                    if (mySqlConnection != null && mySqlConnection.State != ConnectionState.Open)
                    {
                        mySqlConnection.Open();
                        isConnected = true;
                    }
                }
                catch (MySqlException ex)
                {
                    Trace.WriteLine("MySql-Error:", "Error");
                    Trace.WriteLine("Code: " + ex.Number, "Error");
                    Trace.WriteLine("Message: " + ex.Message, "Error");
                    Trace.WriteLine("Source: " + ex.Source, "Error");
                }
            }
		}

		public virtual void CloseConnection()
		{
            lock (mySqlConnection)
            {
                try
                {
                    //close Connection
                    if (mySqlConnection.State != ConnectionState.Closed)
                    {
                        mySqlConnection.Close();
                        isConnected = false;
                    }
                }
                catch (MySqlException ex)
                {
                    Trace.WriteLine("MySql-Error:", "Error");
                    Trace.WriteLine("Code: " + ex.Number, "Error");
                    Trace.WriteLine("Message: " + ex.Message, "Error");
                    Trace.WriteLine("Source: " + ex.Source, "Error");
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

            lock (mySqlConnection)
            {
                mySqlConnection.Open();

                //Display connection information
                Trace.WriteLine("Connection Information: ", "Information");
                Trace.WriteLine("\tConnection String: " + mySqlConnection.ConnectionString, "Information");
                Trace.WriteLine("\tConnection Timeout: " + mySqlConnection.ConnectionTimeout, "Information");
                Trace.WriteLine("\tDatabase: " + mySqlConnection.Database, "Information");
                Trace.WriteLine("\tDataSource: " + mySqlConnection.DataSource, "Information");
                //Trace.WriteLine("\tDriver: " + mySqlConnection.Driver, "Information");
                Trace.WriteLine("\tServerVersion: " + mySqlConnection.ServerVersion, "Information");

                mySqlConnection.Close();
            }
		}

		public virtual bool CreateTable(string table, string fields)
		{
            lock (mySqlConnection)
            {
                try
                {
                    //open Connection
                    mySqlConnection.Open();

                    //Delete manually
                    string sqlCommand = "CREATE TABLE IF NOT EXISTS " + table + " " + fields;
                    MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);
                    mySqlCommand.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    Trace.WriteLine("MySql-Error:", "Error");
                    Trace.WriteLine("Code: " + ex.Number, "Error");
                    Trace.WriteLine("Message: " + ex.Message, "Error");
                    Trace.WriteLine("Source: " + ex.Source, "Error");

                    mySqlConnection.Close();

                    //return with error			
                    return false;
                }
                mySqlConnection.Close();

                //everything went fine
                return true;
            }
		}

		public virtual void Select(string sqlCommand)
		{
			//check MySql Connection
			if (mySqlConnection == null)
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
				MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);
				mySqlDataAdapter = new MySqlDataAdapter();
				mySqlDataAdapter.SelectCommand = mySqlCommand;
				
				//Create CommandBuilder to automatically generate Update/Delete/Insert-Commands
				//for DataAdapter used by method writeToDb()
				MySqlCommandBuilder mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);

				//Add Tablenames to Dataset	
				mySqlDataAdapter.TableMappings.Clear();
				mySqlDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				mySqlDataAdapter.Fill(dataSet, table);
	
				//connection established
				isConnected = true;
			}
			catch (MySqlException ex)
			{
				Trace.WriteLine("MySql-Error:", "Error");
				Trace.WriteLine("Code: " + ex.Number, "Error");
				Trace.WriteLine("Message: " + ex.Message, "Error");
				Trace.WriteLine("Source: " + ex.Source, "Error");

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
			//check MySql Connection
			if (mySqlConnection == null)
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
				MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM " + table, mySqlConnection);
				mySqlDataAdapter = new MySqlDataAdapter();
				mySqlDataAdapter.SelectCommand = mySqlCommand;					
				
				//Create CommandBuilder to generate Update/Delete/Insert-Commands for writeToDb()
				MySqlCommandBuilder mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);
					
				//Add Tablenames to Dataset
				mySqlDataAdapter.TableMappings.Clear();
				mySqlDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				mySqlDataAdapter.Fill(dataSet, table);

				//connection established
				isConnected = true;		
			}
			catch (MySqlException ex)
			{
				Trace.WriteLine("MySql-Error:", "Error");
				Trace.WriteLine("Code: " + ex.Number, "Error");
				Trace.WriteLine("Message: " + ex.Message, "Error");
				Trace.WriteLine("Source: " + ex.Source, "Error");

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
			//check MySql Connection
			if (mySqlConnection == null)
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
				MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM " + table + " ORDER BY " + orderBy, mySqlConnection);
				mySqlDataAdapter = new MySqlDataAdapter();
				mySqlDataAdapter.SelectCommand = mySqlCommand;					
				
				//Create CommandBuilder to automatically generate Update/Delete/Insert-Commands
				//for DataAdapter used by method writeToDb()
				MySqlCommandBuilder mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);
					
				//Add Tablenames to Dataset
				mySqlDataAdapter.TableMappings.Clear();
				mySqlDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				mySqlDataAdapter.Fill(dataSet, table);

				//connection established
				isConnected = true;
			}
			catch (MySqlException ex)
			{
				Trace.WriteLine("MySql-Error:", "Error");
				Trace.WriteLine("Code: " + ex.Number, "Error");
				Trace.WriteLine("Message: " + ex.Message, "Error");
				Trace.WriteLine("Source: " + ex.Source, "Error");

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
            lock (mySqlConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        mySqlConnection.Open();

                    //Insert manually				
                    string sqlInsertCommand = "INSERT INTO " + table + sqlCommand;
                    MySqlCommand mySqlCommand = new MySqlCommand(sqlInsertCommand, mySqlConnection);
                    mySqlCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (MySqlException ex)
                {
                    Trace.WriteLine("MySql-Error:", "Error");
                    Trace.WriteLine("Code: " + ex.Number, "Error");
                    Trace.WriteLine("Message: " + ex.Message, "Error");
                    Trace.WriteLine("Source: " + ex.Source, "Error");

                    //close Connection
                    if (autoOpenClose)
                        mySqlConnection.Close();

                    //return with error
                    return false;
                }

                //close Connection
                if (autoOpenClose)
                    mySqlConnection.Close();

                //everything went fine
                return true;
            }
		}

		public virtual bool UpdateRow(string sqlCommand, bool autoOpenClose)
		{
            lock (mySqlConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        mySqlConnection.Open();

                    //Update manually
                    string sqlUpdateCommand = "UPDATE " + table + " SET " + sqlCommand;
                    MySqlCommand mySqlCommand = new MySqlCommand(sqlUpdateCommand, mySqlConnection);
                    mySqlCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (MySqlException ex)
                {
                    Trace.WriteLine("MySql-Error:", "Error");
                    Trace.WriteLine("Code: " + ex.Number, "Error");
                    Trace.WriteLine("Message: " + ex.Message, "Error");
                    Trace.WriteLine("Source: " + ex.Source, "Error");

                    if (autoOpenClose)
                        mySqlConnection.Close();

                    //return with error
                    return false;
                }

                if (autoOpenClose)
                    mySqlConnection.Close();

                //everything went fine
                return true;
            }
		}

		public virtual bool DeleteRow(int id, bool autoOpenClose)
		{
            lock (mySqlConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        mySqlConnection.Open();

                    //Delete manually
                    string sqlDeleteCommand = "DELETE FROM " + table + " WHERE id='" + id + "'";
                    MySqlCommand mySqlCommand = new MySqlCommand(sqlDeleteCommand, mySqlConnection);
                    mySqlCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (MySqlException ex)
                {
                    Trace.WriteLine("MySql-Error:", "Error");
                    Trace.WriteLine("Code: " + ex.Number, "Error");
                    Trace.WriteLine("Message: " + ex.Message, "Error");
                    Trace.WriteLine("Source: " + ex.Source, "Error");

                    if (autoOpenClose)
                        mySqlConnection.Close();

                    //return with error			
                    return false;
                }

                if (autoOpenClose)
                    mySqlConnection.Close();

                //everything went fine
                return true;
            }
		}

		public virtual int GetAutoID(bool autoOpenClose)
		{
            lock (mySqlConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        mySqlConnection.Open();

                    //retrieve id
                    //stored procedure: Last_insert_id() doesn't work ?
                    string sqlCommand = "SELECT max(id) FROM " + table;
                    MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);

                    int autoIncrement = -1;

                    //fetch
                    MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                    while (mySqlDataReader.Read())
                        autoIncrement = mySqlDataReader.GetInt32(0);
                    mySqlDataReader.Close();

                    //close Connection
                    if (autoOpenClose)
                        mySqlConnection.Close();

                    return autoIncrement;
                }
                catch (MySqlException ex)
                {
                    Trace.WriteLine("MySql-Error:", "Error");
                    Trace.WriteLine("Code: " + ex.Number, "Error");
                    Trace.WriteLine("Message: " + ex.Message, "Error");
                    Trace.WriteLine("Source: " + ex.Source, "Error");

                    //close Connection
                    if (autoOpenClose)
                        mySqlConnection.Close();

                    //return with error
                    return -1;
                }
            }
		}

		public virtual bool WriteChanges()
		{
            lock (mySqlConnection)
            {
                //check MySQL DataAdapter
                if (mySqlDataAdapter == null)
                    throw new Exception("DataAdapter not initialized.");

                //MySQL
                try
                {
                    //Connect to database using MySQL
                    //no need to open the connection manually, DataAdapter will open and close
                    //it automatically

                    //^^MySQL Connector 1.0.4 does not support this^ feature, open and
                    //close manually
                    mySqlConnection.Open();

                    //Update DataBase
                    mySqlDataAdapter.Update(dataSet, dataSet.Tables[0].TableName);

                    //finalize changes to DataSet
                    //do not use this Method in conjunction with DataAdpater.Update()
                    //Update() performs the necessary AcceptChanges automatically on each
                    //row successfully updated
                    //this.AcceptChanges();
                }
                catch (MySqlException ex)
                {
                    Trace.WriteLine("MySql-Error:", "Error");
                    Trace.WriteLine("Code: " + ex.Number, "Error");
                    Trace.WriteLine("Message: " + ex.Message, "Error");
                    Trace.WriteLine("Source: " + ex.Source, "Error");

                    mySqlConnection.Close();

                    //return with error
                    return false;
                }

                mySqlConnection.Close();

                //everything went fine
                return true;
            }
		}

		public virtual bool WriteChanges(DataSet ds)
		{
            lock (mySqlConnection)
            {
                //check MySQL DataAdapter
                if (mySqlDataAdapter == null)
                    throw new Exception("DataAdapter not initialized.");

                //MySQL
                try
                {
                    //Connect to database using MySQL
                    //no need to open the connection manually, DataAdapter will open and close
                    //it automatically

                    //^^MySQL Connector 1.0.4 does not support this^ feature, open and
                    //close manually
                    mySqlConnection.Open();

                    //Update DataBase
                    mySqlDataAdapter.Update(ds);
                }
                catch (MySqlException ex)
                {
                    Trace.WriteLine("MySql-Error:", "Error");
                    Trace.WriteLine("Code: " + ex.Number, "Error");
                    Trace.WriteLine("Message: " + ex.Message, "Error");
                    Trace.WriteLine("Source: " + ex.Source, "Error");

                    mySqlConnection.Close();

                    //return with error
                    return false;
                }
                mySqlConnection.Close();

                //everything went fine
                return true;
            }
		}

		public virtual byte[] GetBytes(int id, string column)
		{
			return GetBytes(id, column, true);
		}

		public virtual byte[] GetBytes(int id, string column, bool autoOpenClose)
		{
            lock (mySqlConnection)
            {
                byte[] result;
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        mySqlConnection.Open();

                    //select
                    string sqlSelectCommand = "SELECT `" + column + "` FROM " + table + " WHERE id=" + id;
                    MySqlCommand mySqlCommand = new MySqlCommand(sqlSelectCommand, mySqlConnection);

                    //read all at once
                    result = (byte[])mySqlCommand.ExecuteScalar();
                }
                catch (MySqlException ex)
                {
                    Trace.WriteLine("MySql-Error:", "Error");
                    Trace.WriteLine("Code: " + ex.Number, "Error");
                    Trace.WriteLine("Message: " + ex.Message, "Error");
                    Trace.WriteLine("Source: " + ex.Source, "Error");

                    //return with error
                    if (autoOpenClose)
                        mySqlConnection.Close();

                    return null;
                }

                if (autoOpenClose)
                    mySqlConnection.Close();

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
            lock (mySqlConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        mySqlConnection.Open();

                    //update binary as MySqlCommand parameter
                    string sqlUpdateCommand = "UPDATE " + table + " SET `" + column + "`=?" + column + " WHERE id=" + id;
                    MySqlCommand mySqlCommand = new MySqlCommand(sqlUpdateCommand, mySqlConnection);
                    if (bytes != null)
                        mySqlCommand.Parameters.Add("?" + column, MySqlDbType.LongBlob, bytes.Length).Value = bytes;
                    else
                        mySqlCommand.Parameters.Add("?" + column, MySqlDbType.LongBlob, 0).Value = System.DBNull.Value;

                    mySqlCommand.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    Trace.WriteLine("MySql-Error:", "Error");
                    Trace.WriteLine("Code: " + ex.Number, "Error");
                    Trace.WriteLine("Message: " + ex.Message, "Error");
                    Trace.WriteLine("Source: " + ex.Source, "Error");

                    if (autoOpenClose)
                        mySqlConnection.Close();

                    //return with error
                    return false;
                }

                if (autoOpenClose)
                    mySqlConnection.Close();

                //everything went fine
                return true;
            }
		}

	}
}
