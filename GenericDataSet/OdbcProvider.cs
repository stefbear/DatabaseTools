using System;
using System.Data;
using System.Diagnostics;

using System.Data.Odbc;

namespace Database.GenericDataSet
{
	/// <summary>
	/// Zusammenfassung für OdbcProvider.
	/// </summary>
	public class OdbcProvider : IDatabaseProvider
	{
		private DataSet dataSet;

		//Odbc
		private string			myOdbcConnectionString = String.Empty;
		private OdbcConnection	myOdbcConnection;
		private OdbcDataAdapter	myOdbcDataAdapter;

		private string	database;
		private string	user;
		private string	password;
		private string	table;
		private bool	isConnected;

		public OdbcProvider(DataSet ds)
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
            lock (myOdbcConnection)
            {
                //check Odbc Connection
                if (myOdbcConnection == null)
                    throw new Exception("No connection established.");

                //Odbc Connection
                try
                {
                    //open initialized connection to check parameters
                    //(pw, user, database)
                    myOdbcConnection.Open();

                    myOdbcConnection.Close();

                    isConnected = false;
                    return String.Empty;
                }
                catch (OdbcException e)
                {
                    //return error
                    isConnected = false;

                    myOdbcConnection.Close();
                    return e.Message;
                }
            }
		}

		public virtual void SetupConnection()
		{
			//ODBC Connection
			//Connection string for MyODBC 3.51
			/*myOdbcConnectionString = "DRIVER={MySQL ODBC 3.51 Driver};" + 
														"SERVER=localhost;" +
														"DATABASE=" + database + ";" +
														"UID=" + user + ";" +
														"PASSWORD=" + password + ";" +
														"OPTION=3";
					*/

			//Connection string for MyODBC and configured DSN
			myOdbcConnectionString = "DSN=" + database + ";" +
				"UID=" + user + ";" +
				"PASSWORD=" + password + ";" +
				"OPTION=3";
				
			//Connect to database using MyODBC
			myOdbcConnection = new OdbcConnection(myOdbcConnectionString);
		}

		public virtual void OpenConnection()
		{
            lock (myOdbcConnection)
            {
                try
                {
                    //open Connection
                    isConnected = false;
                    if (myOdbcConnection != null && myOdbcConnection.State != ConnectionState.Open)
                    {
                        myOdbcConnection.Open();
                        isConnected = true;
                    }
                }
                catch (OdbcException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                }
            }
		}

		public virtual void CloseConnection()
		{
            lock (myOdbcConnection)
            {
                try
                {
                    //close Connection
                    if (myOdbcConnection.State != ConnectionState.Closed)
                    {
                        myOdbcConnection.Close();
                        isConnected = false;
                    }
                }
                catch (OdbcException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
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

            lock (myOdbcConnection)
            {
                myOdbcConnection.Open();

                //Display connection information
                Trace.WriteLine("Connection Information: ", "Information");
                Trace.WriteLine("\tConnection String: " + myOdbcConnection.ConnectionString, "Information");
                Trace.WriteLine("\tConnection Timeout: " + myOdbcConnection.ConnectionTimeout, "Information");
                Trace.WriteLine("\tDatabase: " + myOdbcConnection.Database, "Information");
                Trace.WriteLine("\tDataSource: " + myOdbcConnection.DataSource, "Information");
                Trace.WriteLine("\tDriver: " + myOdbcConnection.Driver, "Information");
                Trace.WriteLine("\tServerVersion: " + myOdbcConnection.ServerVersion, "Information");

                myOdbcConnection.Close();
            }
		}

		public virtual bool CreateTable(string table, string fields)
		{
            lock (myOdbcConnection)
            {
                try
                {
                    //open Connection
                    myOdbcConnection.Open();

                    //Delete manually
                    string sqlCommand = "CREATE TABLE IF NOT EXISTS " + table + " " + fields;
                    OdbcCommand myOdbcCommand = new OdbcCommand(sqlCommand, myOdbcConnection);
                    myOdbcCommand.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    myOdbcConnection.Close();
                    //return with error			
                    return false;
                }
                myOdbcConnection.Close();
                //everything went fine
                return true;
            }
		}

		public virtual void Select(string sqlCommand)
		{
			//Connection string for MyODBC 3.51
			/*myOdbcConnectionString = "DRIVER={MySQL ODBC 3.51 Driver};" + 
														"SERVER=localhost;" +
														"DATABASE=" + database + ";" +
														"UID=" + user + ";" +
														"PASSWORD=" + password + ";" +
														"OPTION=3";
					*/

			//check Odbc Connection
			if (myOdbcConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//myOdbcConnection.Open();

				//read all available datasets (rows, from value parameter strSqlCommand)
				OdbcCommand myOdbcCommand = new OdbcCommand(sqlCommand, myOdbcConnection);
				myOdbcDataAdapter = new OdbcDataAdapter();
				myOdbcDataAdapter.SelectCommand = myOdbcCommand;
				
				//Create CommandBuilder to automatically generate Update/Delete/Insert-Commands
				//for DataAdapter used by method writeToDb()
				OdbcCommandBuilder myOdbcCommandBuilder = new OdbcCommandBuilder(myOdbcDataAdapter);

				//Add Tablenames to Dataset	
				myOdbcDataAdapter.TableMappings.Clear();
				myOdbcDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				myOdbcDataAdapter.Fill(dataSet, table);
	
				//connection established
				isConnected = true;
			}
			catch (OdbcException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
				}
				//return with error
				isConnected = false;
			}
			finally
			{
				//myOdbcConnection.Close();
			}
		}

		public virtual void SelectAll()
		{
			//Connection string for MyODBC 3.51
			/*myOdbcConnectionString = "DRIVER={MySQL ODBC 3.51 Driver};" + 
														"SERVER=localhost;" +
														"DATABASE=" + database + ";" +
														"UID=" + user + ";" +
														"PASSWORD=" + password + ";" +
														"OPTION=3";
					*/

			//check Odbc Connection
			if (myOdbcConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//myOdbcConnection.Open();
													
				//read all Datasets from table
				OdbcCommand myOdbcCommand = new OdbcCommand("SELECT * FROM " + table, myOdbcConnection);
				myOdbcDataAdapter = new OdbcDataAdapter();
				myOdbcDataAdapter.SelectCommand = myOdbcCommand;					
				
				//Create CommandBuilder to generate Update/Delete/Insert-Commands for writeToDb()
				OdbcCommandBuilder myOdbcCommandBuilder = new OdbcCommandBuilder(myOdbcDataAdapter);
					
				//Add Tablenames to Dataset
				myOdbcDataAdapter.TableMappings.Clear();
				myOdbcDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				myOdbcDataAdapter.Fill(dataSet, table);

				//connection established
				isConnected = true;		
			}
			catch (OdbcException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
				}

				//return with error
				isConnected=false;
			}
			finally
			{
				//myOdbcConnection.Close();
			}
		}

		public virtual void SelectAll(string orderBy)
		{
			//check Odbc Connection
			if (myOdbcConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//myOdbcConnection.Open();
													
				//read all datasets (rows) from table
				OdbcCommand myOdbcCommand = new OdbcCommand("SELECT * FROM " + table + " ORDER BY " + orderBy, myOdbcConnection);
				myOdbcDataAdapter = new OdbcDataAdapter();
				myOdbcDataAdapter.SelectCommand = myOdbcCommand;					
				
				//Create CommandBuilder to automatically generate Update/Delete/Insert-Commands
				//for DataAdapter used by method writeToDb()
				OdbcCommandBuilder myOdbcCommandBuilder = new OdbcCommandBuilder(myOdbcDataAdapter);
					
				//Add Tablenames to Dataset
				myOdbcDataAdapter.TableMappings.Clear();
				myOdbcDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				myOdbcDataAdapter.Fill(dataSet, table);

				//connection established
				isConnected = true;
			}
			catch (OdbcException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
				}

				//connection not established
				isConnected = false;
			}
			finally
			{
				//myOdbcConnection.Close();
			}
		}

		public virtual bool InsertRow(string sqlCommand, bool autoOpenClose)
		{
            lock (myOdbcConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOdbcConnection.Open();

                    //Insert manually				
                    string sqlInsertCommand = "INSERT INTO " + table + sqlCommand;
                    OdbcCommand myOdbcCommand = new OdbcCommand(sqlInsertCommand, myOdbcConnection);
                    myOdbcCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (OdbcException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    //close Connection
                    if (autoOpenClose)
                        myOdbcConnection.Close();
                    //return with error
                    return false;
                }
                //close Connection
                if (autoOpenClose)
                    myOdbcConnection.Close();
                //everything went fine
                return true;
            }
		}

		public virtual bool UpdateRow(string sqlCommand, bool autoOpenClose)
		{
            lock (myOdbcConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOdbcConnection.Open();

                    //Update manually
                    string sqlUpdateCommand = "UPDATE " + table + " SET " + sqlCommand;
                    OdbcCommand myOdbcCommand = new OdbcCommand(sqlUpdateCommand, myOdbcConnection);
                    myOdbcCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (OdbcException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    if (autoOpenClose)
                        myOdbcConnection.Close();
                    //return with error
                    return false;
                }
                if (autoOpenClose)
                    myOdbcConnection.Close();
                //everything went fine
                return true;
            }
		}

		public virtual bool DeleteRow(int id, bool autoOpenClose)
		{
            lock (myOdbcConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOdbcConnection.Open();

                    //Delete manually
                    string sqlDeleteCommand = "DELETE FROM " + table + " WHERE id='" + id + "'";
                    OdbcCommand myOdbcCommand = new OdbcCommand(sqlDeleteCommand, myOdbcConnection);
                    myOdbcCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (OdbcException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    if (autoOpenClose)
                        myOdbcConnection.Close();
                    //return with error			
                    return false;
                }
                if (autoOpenClose)
                    myOdbcConnection.Close();
                //everything went fine
                return true;
            }
		}

		public virtual int GetAutoID(bool autoOpenClose)
		{
            lock (myOdbcConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOdbcConnection.Open();

                    //retrieve id
                    //stored procedure: Last_insert_id() doesn't work ?
                    string sqlCommand = "SELECT max(id) FROM " + table;
                    OdbcCommand myOdbcCommand = new OdbcCommand(sqlCommand, myOdbcConnection);

                    int autoIncrement = -1;

                    //fetch
                    OdbcDataReader myOdbcDataReader = myOdbcCommand.ExecuteReader();
                    while (myOdbcDataReader.Read())
                        autoIncrement = myOdbcDataReader.GetInt32(0);
                    myOdbcDataReader.Close();

                    //close Connection
                    if (autoOpenClose)
                        myOdbcConnection.Close();

                    return autoIncrement;
                }
                catch (OdbcException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    //close Connection
                    if (autoOpenClose)
                        myOdbcConnection.Close();

                    //return with error
                    return -1;
                }
            }
		}

		public virtual bool WriteChanges()
		{
			//check ODBC DataAdapter
			if (myOdbcDataAdapter == null)
				throw new Exception("DataAdapter not initialized.");

			//ODBC
			try
			{
				//Connect to database using ODBC
				//no need to open the connection manually, DataAdapter will open and close
				//it automatically
				//myOdbcConnection.Open();
				
				//Update DataBase
				myOdbcDataAdapter.Update(dataSet, dataSet.Tables[0].TableName);

				//finalize changes to DataSet
				//do not use this Method in conjunction with DataAdpater.Update()
				//Update() performs the necessary AcceptChanges automatically on each
				//row successfully updated
				//this.AcceptChanges();
			}
			catch (OdbcException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
				}
				//myOdbcConnection.Close();
				//return with error
				return false;
			}
			//myOdbcConnection.Close();
			//everything went fine
			return true;
		}

		public virtual bool WriteChanges(DataSet ds)
		{
			//check ODBC DataAdapter
			if (myOdbcDataAdapter == null)
				throw new Exception("DataAdapter not initialized.");

			//ODBC
			try
			{
				//Update DataBase
				myOdbcDataAdapter.Update(ds);
			}
			catch (OdbcException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
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
            lock (myOdbcConnection)
            {
                byte[] result;
                try
                {
                    //open Connection
                    //if (myOdbcConnection.State == ConnectionState.Closed)
                    if (autoOpenClose)
                        myOdbcConnection.Open();

                    //select
                    string sqlSelectCommand = "SELECT `" + column + "` FROM " + table + " WHERE id=" + id;
                    OdbcCommand myOdbcCommand = new OdbcCommand(sqlSelectCommand, myOdbcConnection);

                    //read all at once
                    result = (byte[])myOdbcCommand.ExecuteScalar();
                }
                catch (OdbcException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }

                    //return with error
                    if (autoOpenClose)
                        myOdbcConnection.Close();
                    return null;
                }

                if (autoOpenClose)
                    myOdbcConnection.Close();
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
            lock (myOdbcConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOdbcConnection.Open();

                    //update binary as OdbcCommand parameter
                    string sqlUpdateCommand = "UPDATE " + table + " SET `" + column + "`=? WHERE id=" + id;
                    OdbcCommand myOdbcCommand = new OdbcCommand(sqlUpdateCommand, myOdbcConnection);
                    if (bytes != null)
                        myOdbcCommand.Parameters.Add("@" + column, OdbcType.Binary, bytes.Length).Value = bytes;
                    else
                        myOdbcCommand.Parameters.Add("@" + column, OdbcType.Binary, 0).Value = System.DBNull.Value;

                    myOdbcCommand.ExecuteNonQuery();
                }
                catch (OdbcException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("Odbc-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }

                    if (autoOpenClose)
                        myOdbcConnection.Close();
                    //return with error
                    return false;
                }

                if (autoOpenClose)
                    myOdbcConnection.Close();
                //everything went fine
                return true;
            }
		}

	}
}
