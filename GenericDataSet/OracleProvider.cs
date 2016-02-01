using System;
using System.Data;
using System.Diagnostics;

using System.Data.OracleClient;

namespace Database.GenericDataSet
{
	/// <summary>
	/// Zusammenfassung für OracleProvider.
	/// </summary>
	public class OracleProvider : IDatabaseProvider
	{
		private DataSet dataSet;

		//Oracle
		private string				myOracleConnectionString = String.Empty;
		private OracleConnection	myOracleConnection;
		private OracleDataAdapter	myOracleDataAdapter;

		private string	database;
		private string	user;
		private string	password;
		private string	table;
		private bool	isConnected;

		public OracleProvider(DataSet ds)
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
            lock (myOracleConnection)
            {
                //check Oracle Connection
                if (myOracleConnection == null)
                    throw new Exception("No connection established.");

                //Oracle Connection
                try
                {
                    //open initialized connection to check parameters
                    //(pw, user, database)
                    myOracleConnection.Open();

                    myOracleConnection.Close();

                    isConnected = false;
                    return String.Empty;
                }
                catch (OracleException e)
                {
                    //return error
                    isConnected = false;

                    myOracleConnection.Close();
                    return e.Message;
                }
            }
		}

		public virtual void SetupConnection()
		{
			//Oracle Connection
			//Connection string for Oracle 3.51
			/*myOracleConnectionString = "DRIVER={MySQL Oracle 3.51 Driver};" + 
														"SERVER=localhost;" +
														"DATABASE=" + database + ";" +
														"UID=" + user + ";" +
														"PASSWORD=" + password + ";" +
														"OPTION=3";
					*/

			//Connection string for Oracle and configured DSN
			myOracleConnectionString = "DSN=" + database + ";" +
				"UID=" + user + ";" +
				"PASSWORD=" + password + ";" +
				"OPTION=3";
				
			//Connect to database using Oracle
			myOracleConnection = new OracleConnection(myOracleConnectionString);
		}

		public virtual void OpenConnection()
		{
            lock (myOracleConnection)
            {
                try
                {
                    //open Connection
                    isConnected = false;
                    if (myOracleConnection != null && myOracleConnection.State != ConnectionState.Open)
                    {
                        myOracleConnection.Open();
                        isConnected = true;
                    }
                }
                catch (OracleException e)
                {
                    Trace.WriteLine("Oracle-Error: ", "Error");
                    Trace.WriteLine("Code: " + e.Code, "Error");
                    Trace.WriteLine("Message: " + e.Message, "Error");
                    Trace.WriteLine("Source: " + e.Source, "Error");
                }
            }
		}

		public virtual void CloseConnection()
		{
            lock (myOracleConnection)
            {
                try
                {
                    //close Connection
                    if (myOracleConnection.State != ConnectionState.Closed)
                    {
                        myOracleConnection.Close();
                        isConnected = false;
                    }
                }
                catch (OracleException e)
                {
                    Trace.WriteLine("Oracle-Error: ", "Error");
                    Trace.WriteLine("Code: " + e.Code, "Error");
                    Trace.WriteLine("Message: " + e.Message, "Error");
                    Trace.WriteLine("Source: " + e.Source, "Error");
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

            lock (myOracleConnection)
            {
                myOracleConnection.Open();

                //Display connection information
                Trace.WriteLine("Connection Information: ", "Information");
                Trace.WriteLine("\tConnection String: " + myOracleConnection.ConnectionString, "Information");
                //Trace.WriteLine("\tConnection Timeout: " + myOracleConnection.ConnectionTimeout, "Information");    
                //Trace.WriteLine("\tDatabase: " + myOracleConnection.Database, "Information");   
                Trace.WriteLine("\tDataSource: " + myOracleConnection.DataSource, "Information");
                //Trace.WriteLine("\tDriver: " + myOracleConnection.Driver, "Information");
                Trace.WriteLine("\tServerVersion: " + myOracleConnection.ServerVersion, "Information");

                myOracleConnection.Close();
            }
		}

		public virtual bool CreateTable(string table, string fields)
		{
            lock (myOracleConnection)
            {
                try
                {
                    //open Connection
                    myOracleConnection.Open();

                    //Delete manually
                    string sqlCommand = "CREATE TABLE IF NOT EXISTS " + table + " " + fields;
                    OracleCommand myOracleCommand = new OracleCommand(sqlCommand, myOracleConnection);
                    myOracleCommand.ExecuteNonQuery();
                }
                catch (OracleException e)
                {
                    Trace.WriteLine("Oracle-Error: ", "Error");
                    Trace.WriteLine("Code: " + e.Code, "Error");
                    Trace.WriteLine("Message: " + e.Message, "Error");
                    Trace.WriteLine("Source: " + e.Source, "Error");

                    myOracleConnection.Close();
                    //return with error			
                    return false;
                }
                myOracleConnection.Close();
                //everything went fine
                return true;
            }
		}

		public virtual void Select(string sqlCommand)
		{
			//Connection string for Oracle 3.51
			/*myOracleConnectionString = "DRIVER={MySQL Oracle 3.51 Driver};" + 
														"SERVER=localhost;" +
														"DATABASE=" + database + ";" +
														"UID=" + user + ";" +
														"PASSWORD=" + password + ";" +
														"OPTION=3";
					*/

			//check Oracle Connection
			if (myOracleConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//myOracleConnection.Open();

				//read all available datasets (rows, from value parameter strSqlCommand)
				OracleCommand myOracleCommand = new OracleCommand(sqlCommand, myOracleConnection);
				myOracleDataAdapter = new OracleDataAdapter();
				myOracleDataAdapter.SelectCommand = myOracleCommand;
				
				//Create CommandBuilder to automatically generate Update/Delete/Insert-Commands
				//for DataAdapter used by method writeToDb()
				OracleCommandBuilder myOracleCommandBuilder = new OracleCommandBuilder(myOracleDataAdapter);

				//Add Tablenames to Dataset	
				myOracleDataAdapter.TableMappings.Clear();
				myOracleDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				myOracleDataAdapter.Fill(dataSet, table);
	
				//connection established
				isConnected = true;
			}
			catch (OracleException e)
			{
				Trace.WriteLine("Oracle-Error: ", "Error");
				Trace.WriteLine("Code: " + e.Code, "Error");
				Trace.WriteLine("Message: " + e.Message, "Error");
				Trace.WriteLine("Source: " + e.Source, "Error");

				//return with error
				isConnected = false;
			}
			finally
			{
				//myOracleConnection.Close();
			}
		}

		public virtual void SelectAll()
		{
			//Connection string for Oracle 3.51
			/*myOracleConnectionString = "DRIVER={MySQL Oracle 3.51 Driver};" + 
														"SERVER=localhost;" +
														"DATABASE=" + database + ";" +
														"UID=" + user + ";" +
														"PASSWORD=" + password + ";" +
														"OPTION=3";
					*/

			//check Oracle Connection
			if (myOracleConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//myOracleConnection.Open();
													
				//read all Datasets from table
				OracleCommand myOracleCommand = new OracleCommand("SELECT * FROM " + table, myOracleConnection);
				myOracleDataAdapter = new OracleDataAdapter();
				myOracleDataAdapter.SelectCommand = myOracleCommand;					
				
				//Create CommandBuilder to generate Update/Delete/Insert-Commands for writeToDb()
				OracleCommandBuilder myOracleCommandBuilder = new OracleCommandBuilder(myOracleDataAdapter);
					
				//Add Tablenames to Dataset
				myOracleDataAdapter.TableMappings.Clear();
				myOracleDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				myOracleDataAdapter.Fill(dataSet, table);

				//connection established
				isConnected = true;		
			}
			catch (OracleException e)
			{
				Trace.WriteLine("Oracle-Error: ", "Error");
				Trace.WriteLine("Code: " + e.Code, "Error");
				Trace.WriteLine("Message: " + e.Message, "Error");
				Trace.WriteLine("Source: " + e.Source, "Error");

				//return with error
				isConnected=false;
			}
			finally
			{
				//myOracleConnection.Close();
			}
		}

		public virtual void SelectAll(string orderBy)
		{
			//check Oracle Connection
			if (myOracleConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//myOracleConnection.Open();
													
				//read all datasets (rows) from table
				OracleCommand myOracleCommand = new OracleCommand("SELECT * FROM " + table + " ORDER BY " + orderBy, myOracleConnection);
				myOracleDataAdapter = new OracleDataAdapter();
				myOracleDataAdapter.SelectCommand = myOracleCommand;					
				
				//Create CommandBuilder to automatically generate Update/Delete/Insert-Commands
				//for DataAdapter used by method writeToDb()
				OracleCommandBuilder myOracleCommandBuilder = new OracleCommandBuilder(myOracleDataAdapter);
					
				//Add Tablenames to Dataset
				myOracleDataAdapter.TableMappings.Clear();
				myOracleDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				myOracleDataAdapter.Fill(dataSet, table);

				//connection established
				isConnected = true;
			}
			catch (OracleException e)
			{
				Trace.WriteLine("Oracle-Error: ", "Error");
				Trace.WriteLine("Code: " + e.Code, "Error");
				Trace.WriteLine("Message: " + e.Message, "Error");
				Trace.WriteLine("Source: " + e.Source, "Error");

				//connection not established
				isConnected = false;
			}
			finally
			{
				//myOracleConnection.Close();
			}
		}

		public virtual bool InsertRow(string sqlCommand, bool autoOpenClose)
		{
            lock (myOracleConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOracleConnection.Open();

                    //Insert manually				
                    string sqlInsertCommand = "INSERT INTO " + table + sqlCommand;
                    OracleCommand myOracleCommand = new OracleCommand(sqlInsertCommand, myOracleConnection);
                    myOracleCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (OracleException e)
                {
                    Trace.WriteLine("Oracle-Error: ", "Error");
                    Trace.WriteLine("Code: " + e.Code, "Error");
                    Trace.WriteLine("Message: " + e.Message, "Error");
                    Trace.WriteLine("Source: " + e.Source, "Error");

                    //close Connection
                    if (autoOpenClose)
                        myOracleConnection.Close();

                    //return with error
                    return false;
                }

                //close Connection
                if (autoOpenClose)
                    myOracleConnection.Close();

                //everything went fine
                return true;
            }
		}

		public virtual bool UpdateRow(string sqlCommand, bool autoOpenClose)
		{
            lock (myOracleConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOracleConnection.Open();

                    //Update manually
                    string sqlUpdateCommand = "UPDATE " + table + " SET " + sqlCommand;
                    OracleCommand myOracleCommand = new OracleCommand(sqlUpdateCommand, myOracleConnection);
                    myOracleCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (OracleException e)
                {
                    Trace.WriteLine("Oracle-Error: ", "Error");
                    Trace.WriteLine("Code: " + e.Code, "Error");
                    Trace.WriteLine("Message: " + e.Message, "Error");
                    Trace.WriteLine("Source: " + e.Source, "Error");

                    if (autoOpenClose)
                        myOracleConnection.Close();

                    //return with error
                    return false;
                }

                if (autoOpenClose)
                    myOracleConnection.Close();

                //everything went fine
                return true;
            }
		}

		public virtual bool DeleteRow(int id, bool autoOpenClose)
		{
            lock (myOracleConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOracleConnection.Open();

                    //Delete manually
                    string sqlDeleteCommand = "DELETE FROM " + table + " WHERE id='" + id + "'";
                    OracleCommand myOracleCommand = new OracleCommand(sqlDeleteCommand, myOracleConnection);
                    myOracleCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (OracleException e)
                {
                    Trace.WriteLine("Oracle-Error: ", "Error");
                    Trace.WriteLine("Code: " + e.Code, "Error");
                    Trace.WriteLine("Message: " + e.Message, "Error");
                    Trace.WriteLine("Source: " + e.Source, "Error");

                    if (autoOpenClose)
                        myOracleConnection.Close();

                    //return with error			
                    return false;
                }

                if (autoOpenClose)
                    myOracleConnection.Close();

                //everything went fine
                return true;
            }
		}

		public virtual int GetAutoID(bool autoOpenClose)
		{
            lock (myOracleConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOracleConnection.Open();

                    //retrieve id
                    //stored procedure: Last_insert_id() doesn't work ?
                    string sqlCommand = "SELECT max(id) FROM " + table;
                    OracleCommand myOracleCommand = new OracleCommand(sqlCommand, myOracleConnection);

                    int autoIncrement = -1;

                    //fetch
                    OracleDataReader myOracleDataReader = myOracleCommand.ExecuteReader();
                    while (myOracleDataReader.Read())
                        autoIncrement = myOracleDataReader.GetInt32(0);
                    myOracleDataReader.Close();

                    //close Connection
                    if (autoOpenClose)
                        myOracleConnection.Close();

                    return autoIncrement;
                }
                catch (OracleException e)
                {
                    Trace.WriteLine("Oracle-Error: ", "Error");
                    Trace.WriteLine("Code: " + e.Code, "Error");
                    Trace.WriteLine("Message: " + e.Message, "Error");
                    Trace.WriteLine("Source: " + e.Source, "Error");

                    //close Connection
                    if (autoOpenClose)
                        myOracleConnection.Close();

                    //return with error
                    return -1;
                }
            }
		}

		public virtual bool WriteChanges()
		{
			//check Oracle DataAdapter
			if (myOracleDataAdapter == null)
				throw new Exception("DataAdapter not initialized.");

			//Oracle
			try
			{
				//Connect to database using Oracle
				//no need to open the connection manually, DataAdapter will open and close
				//it automatically
				//myOracleConnection.Open();
				
				//Update DataBase
				myOracleDataAdapter.Update(dataSet, dataSet.Tables[0].TableName);

				//finalize changes to DataSet
				//do not use this Method in conjunction with DataAdpater.Update()
				//Update() performs the necessary AcceptChanges automatically on each
				//row successfully updated
				//this.AcceptChanges();
			}
			catch (OracleException e)
			{
				Trace.WriteLine("Oracle-Error: ", "Error");
				Trace.WriteLine("Code: " + e.Code, "Error");
				Trace.WriteLine("Message: " + e.Message, "Error");
				Trace.WriteLine("Source: " + e.Source, "Error");

				//myOracleConnection.Close();
				//return with error
				return false;
			}
			//myOracleConnection.Close();
			//everything went fine
			return true;
		}

		public virtual bool WriteChanges(DataSet ds)
		{
			//check Oracle DataAdapter
			if (myOracleDataAdapter == null)
				throw new Exception("DataAdapter not initialized.");

			//Oracle
			try
			{
				//Update DataBase
				myOracleDataAdapter.Update(ds);
			}
			catch (OracleException e)
			{
				Trace.WriteLine("Oracle-Error: ", "Error");
				Trace.WriteLine("Code: " + e.Code, "Error");
				Trace.WriteLine("Message: " + e.Message, "Error");
				Trace.WriteLine("Source: " + e.Source, "Error");

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
            lock (myOracleConnection)
            {
                byte[] result;
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOracleConnection.Open();

                    //select
                    string sqlSelectCommand = "SELECT `" + column + "` FROM " + table + " WHERE id=" + id;
                    OracleCommand myOracleCommand = new OracleCommand(sqlSelectCommand, myOracleConnection);

                    //read all at once
                    result = (byte[])myOracleCommand.ExecuteScalar();
                }
                catch (OracleException e)
                {
                    Trace.WriteLine("Oracle-Error: ", "Error");
                    Trace.WriteLine("Code: " + e.Code, "Error");
                    Trace.WriteLine("Message: " + e.Message, "Error");
                    Trace.WriteLine("Source: " + e.Source, "Error");

                    //return with error
                    if (autoOpenClose)
                        myOracleConnection.Close();

                    return null;
                }

                if (autoOpenClose)
                    myOracleConnection.Close();

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
            lock (myOracleConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOracleConnection.Open();

                    //update binary as OracleCommand parameter
                    //string sqlUpdateCommand = "UPDATE " + table + " SET `" + column + "`=? WHERE id=" + id;
                    string sqlUpdateCommand = "UPDATE " + table + " SET `" + column + "`=:" + column + " WHERE id=" + id;
                    OracleCommand myOracleCommand = new OracleCommand(sqlUpdateCommand, myOracleConnection);
                    if (bytes != null)
                        myOracleCommand.Parameters.Add(column, OracleType.Blob, bytes.Length).Value = bytes;
                    else
                        myOracleCommand.Parameters.Add(column, OracleType.Blob, 0).Value = System.DBNull.Value;

                    myOracleCommand.ExecuteNonQuery();
                }
                catch (OracleException e)
                {
                    Trace.WriteLine("Oracle-Error: ", "Error");
                    Trace.WriteLine("Code: " + e.Code, "Error");
                    Trace.WriteLine("Message: " + e.Message, "Error");
                    Trace.WriteLine("Source: " + e.Source, "Error");

                    if (autoOpenClose)
                        myOracleConnection.Close();

                    //return with error
                    return false;
                }

                if (autoOpenClose)
                    myOracleConnection.Close();

                //everything went fine
                return true;
            }
		}

	}
}
