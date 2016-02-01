using System;
using System.Data;
using System.Diagnostics;

using System.Data.OleDb;

namespace Database.GenericDataSet
{
	/// <summary>
	/// Zusammenfassung für OledbProvider.
	/// </summary>
	public class OleDbProvider : IDatabaseProvider
	{
		private DataSet dataSet;

		//OleDb
		private string				myOleDbConnectionString = String.Empty;
		private OleDbConnection		myOleDbConnection;
		private OleDbDataAdapter	myOleDbDataAdapter;

		private string	database;
		private string	user;
		private string	password;
		private string	table;
		private bool	isConnected;

		public OleDbProvider(DataSet ds)
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
            lock (myOleDbConnection)
            {
                //check OleDb Connection
                if (myOleDbConnection == null)
                    throw new Exception("No connection established.");

                //OleDb Connection
                try
                {
                    //open initialized connection to check parameters
                    //(pw, user, database)
                    myOleDbConnection.Open();

                    myOleDbConnection.Close();

                    isConnected = false;
                    return String.Empty;
                }
                catch (OleDbException e)
                {
                    //return error					
                    isConnected = false;

                    myOleDbConnection.Close();
                    return e.Message;
                }
            }
		}

		public virtual void SetupConnection()
		{
			//OleDb Connection
			//Connection string for Database file (Jet)
			myOleDbConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + 
				"Data Source=" + database + ";" +
				//"Persist Security=true;" +				//retain passwords
				//"System Database=" + database + ";" +		//.mdw workgroupinfo file
				//"System Database=D:\\Private\\Software Engeneering\\Visual .Net\\HomeER\\HomeER\\bin\\Release\\data\\datenbestand.mdw;" +
				"User ID=" + user + ";" +
				"Password=" + password;

			//Connect to database using OleDb
			myOleDbConnection = new OleDbConnection(myOleDbConnectionString);
		}

		public virtual void OpenConnection()
		{
            lock (myOleDbConnection)
            {
                //OleDb
                try
                {
                    //open Connection
                    isConnected = false;
                    if (myOleDbConnection != null && myOleDbConnection.State != ConnectionState.Open)
                    {
                        myOleDbConnection.Open();
                        isConnected = true;
                    }
                }
                catch (OleDbException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
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
            lock (myOleDbConnection)
            {
                //OleDb
                try
                {
                    //close Connection
                    if (myOleDbConnection.State != ConnectionState.Closed)
                    {
                        myOleDbConnection.Close();
                        isConnected = false;
                    }
                }
                catch (OleDbException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
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

            lock (myOleDbConnection)
            {
                //OleDb
                //Open Connection
                myOleDbConnection.Open();

                //Display connection information
                Trace.WriteLine("Connection Information: ", "Information");
                Trace.WriteLine("\tConnection String: " + myOleDbConnection.ConnectionString, "Information");
                Trace.WriteLine("\tConnection Timeout: " + myOleDbConnection.ConnectionTimeout, "Information");
                Trace.WriteLine("\tDatabase: " + myOleDbConnection.Database, "Information");
                Trace.WriteLine("\tDataSource: " + myOleDbConnection.DataSource, "Information");
                Trace.WriteLine("\tDriver: " + myOleDbConnection.Provider, "Information");
                Trace.WriteLine("\tServerVersion: " + myOleDbConnection.ServerVersion, "Information");

                myOleDbConnection.Close();
            }
		}

		public virtual bool CreateTable(string table, string fields)
		{
            lock (myOleDbConnection)
            {
                try
                {
                    //open Connection
                    myOleDbConnection.Open();

                    //--------------------------------------->
                    //string sqlCommand = "CREATE TABLE IF NOT EXISTS " + table + " " + fields;
                    //----------------------------------^^^^^^^^^^^^^ is unknown to MS Access!
                    string sqlCommand = "CREATE TABLE " + table + " " + fields;
                    OleDbCommand myOleDbCommand = new OleDbCommand(sqlCommand, myOleDbConnection);
                    myOleDbCommand.ExecuteNonQuery();
                }
                catch (OleDbException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        //Display connection information						
                        Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    myOleDbConnection.Close();
                    //return with error			
                    return false;
                }
                myOleDbConnection.Close();
                //everything went fine				
                return true;
            }
		}

		public virtual void Select(string sqlCommand)
		{
			//check OleDb Connection
			if (myOleDbConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//myOleDbConnection.Open();

				//read all available datasets (rows, from value parameter strSqlCommand)
				OleDbCommand myOleDbCommand = new OleDbCommand(sqlCommand, myOleDbConnection);
				myOleDbDataAdapter = new OleDbDataAdapter();
				myOleDbDataAdapter.SelectCommand = myOleDbCommand;
				
				//Create CommandBuilder to automatically generate Update/Delete/Insert-Commands
				//for DataAdapter used by method writeToDb()
				OleDbCommandBuilder myOleDbCommandBuilder = new OleDbCommandBuilder(myOleDbDataAdapter);

				//Add tablename to Dataset
				myOleDbDataAdapter.TableMappings.Clear();
				myOleDbDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				myOleDbDataAdapter.Fill(dataSet, table);
	
				//connection established
				isConnected = true;
			}
			catch (OleDbException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
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
				//myOleDbConnection.Close();
			}
		}

		public virtual void SelectAll()
		{
			//check OleDb Connection
			if (myOleDbConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//myOleDbConnection.Open();

				//read all Datasets from table					
				OleDbCommand myOleDbCommand = new OleDbCommand("SELECT * FROM " + table, myOleDbConnection);
				myOleDbDataAdapter = new OleDbDataAdapter();
				myOleDbDataAdapter.SelectCommand = myOleDbCommand;
				
				//Create CommandBuilder to generate Update/Delete/Insert-Commands for writeToDb()					
				OleDbCommandBuilder myOleDbCommandBuilder = new OleDbCommandBuilder(myOleDbDataAdapter);

				//Add Tablenames to Dataset
				myOleDbDataAdapter.TableMappings.Clear();
				myOleDbDataAdapter.TableMappings.Add("Table", table);
					
				//fill DataSet
				myOleDbDataAdapter.Fill(dataSet, table);

				//connection established
				isConnected = true;		
			}
			catch (OleDbException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
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
				//myOleDbConnection.Close();
			}
		}

		public virtual void SelectAll(string orderBy)
		{
			//check OleDb Connection
			if (myOleDbConnection == null)
				throw new Exception("No connection established.");

			try
			{
				//no need to open the connection manually, DataAdapter will open
				//and close it automatically, in fact it is recommended not to 
				//explicity open the connection: in case of an error, raised by 
				//the DataAdapter.Fill() or DataAdapter.Update() method, the connection
				//will stay open
				//myOleDbConnection.Open();

				//read all datasets (rows) from table
				OleDbCommand myOleDbCommand = new OleDbCommand("SELECT * FROM " + table + " ORDER BY " + orderBy, myOleDbConnection);
				myOleDbDataAdapter = new OleDbDataAdapter();
				myOleDbDataAdapter.SelectCommand = myOleDbCommand;
				
				//Create CommandBuilder to automatically generate Update/Delete/Insert-Commands
				//for DataAdapter used by method writeToDb()
				OleDbCommandBuilder myOleDbCommandBuilder = new OleDbCommandBuilder(myOleDbDataAdapter);
					
				//Create tablemappings (and columnmappings) for each table
				myOleDbDataAdapter.TableMappings.Clear();
				myOleDbDataAdapter.TableMappings.Add("Table", table);

				//fill DataSet
				myOleDbDataAdapter.Fill(dataSet, table);

				//connection established
				isConnected = true;
			}
			catch (OleDbException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
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
				//myOleDbConnection.Close();
			}
		}

		public virtual bool InsertRow(string sqlCommand, bool autoOpenClose)
		{
            lock (myOleDbConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOleDbConnection.Open();

                    //Insert manually				
                    string sqlInsertCommand = "INSERT INTO " + table + sqlCommand;
                    OleDbCommand myOleDbCommand = new OleDbCommand(sqlInsertCommand, myOleDbConnection);
                    myOleDbCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (OleDbException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    //close connection
                    if (autoOpenClose)
                        myOleDbConnection.Close();
                    //return with error
                    return false;
                }
                //close connection
                if (autoOpenClose)
                    myOleDbConnection.Close();
                //everything went fine
                return true;
            }
		}

		public virtual bool UpdateRow(string sqlCommand, bool autoOpenClose)
		{
            lock (myOleDbConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOleDbConnection.Open();

                    //Update manually
                    string sqlUpdateCommand = "UPDATE " + table + " SET " + sqlCommand;
                    OleDbCommand myOleDbCommand = new OleDbCommand(sqlUpdateCommand, myOleDbConnection);
                    myOleDbCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (OleDbException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    if (autoOpenClose)
                        myOleDbConnection.Close();
                    //return with error
                    return false;
                }
                if (autoOpenClose)
                    myOleDbConnection.Close();
                //everything went fine
                return true;
            }
		}

		public virtual bool DeleteRow(int id, bool autoOpenClose)
		{
            lock (myOleDbConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOleDbConnection.Open();

                    //Delete manually
                    string sqlDeleteCommand = "DELETE FROM " + table + " WHERE id=" + id;
                    OleDbCommand myOleDbCommand = new OleDbCommand(sqlDeleteCommand, myOleDbConnection);
                    myOleDbCommand.ExecuteNonQuery();

                    dataSet.AcceptChanges();
                }
                catch (OleDbException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    if (autoOpenClose)
                        myOleDbConnection.Close();
                    //return with error			
                    return false;
                }
                if (autoOpenClose)
                    myOleDbConnection.Close();
                //everything went fine
                return true;
            }
		}

		public virtual int GetAutoID(bool autoOpenClose)
		{
            lock (myOleDbConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOleDbConnection.Open();

                    //retrieve id
                    //Last_insert_id() doesn't work ?
                    string sqlCommand = "SELECT max(id) FROM " + table;
                    OleDbCommand myOleDbCommand = new OleDbCommand(sqlCommand, myOleDbConnection);

                    int autoIncrement = -1;

                    //fetch
                    OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();
                    while (myOleDbDataReader.Read())
                        autoIncrement = myOleDbDataReader.GetInt32(0);
                    myOleDbDataReader.Close();

                    //close Connection
                    if (autoOpenClose)
                        myOleDbConnection.Close();

                    return autoIncrement;
                }
                catch (OleDbException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    //close Connection
                    if (autoOpenClose)
                        myOleDbConnection.Close();

                    //return with error
                    return -1;
                }
            }
		}

		public virtual bool WriteChanges()
		{
			//check OleDb DataAdapter
			if (myOleDbDataAdapter == null)
				throw new Exception("DataAdapter not initialized.");

			//OleDb
			try
			{
				//Connect to database using Oledb
				//no need to open the connection manually, DataAdapter will open and close
				//it automatically
				//myOleDbConnection.Open();
				
				/*
						string sqlUpdateCommand = "UPDATE " + table + " SET " + strSqlCommand;
						string sqlInsertCommand = "INSERT INTO " + table + " VALUES " + strSqlCommand;
						string sqlDeleteCommand = "DELETE FROM " + table + " " + strSqlCommand;
					
						OleDbCommand myUpdateCommand = new OleDbCommand(sqlUpdateCommand, myOleDbConnection);
						OleDbCommand myInsertCommand = new OleDbCommand(sqlInsertCommand, myOleDbConnection);
						OleDbCommand myDeleteCommand = new OleDbCommand(sqlDeleteCommand, myOleDbConnection);
												
						//assign UpdateCommand, InsertCommand from SQL-String
						myOleDbDataAdapter.UpdateCommand = myUpdateCommand;
						myOleDbDataAdapter.InsertCommand = myInsertCommand;
						myOleDbDataAdapter.DeleteCommand = myDeleteCommand;
					
						//using CommandBuilder to automatically create Update/Insert/Delete-Commands for
						//DataAdapter; CommandBuilder is initialized when SetupCommand is declared
						*/
					
				//Update DataBase
				myOleDbDataAdapter.Update(dataSet, dataSet.Tables[0].TableName);

				//finalize changes to DataSet
				//do not use this Method in conjunction with DataAdpater.Update()
				//Update() performs the necessary AcceptChanges automatically on each
				//row successfully updated
				//this.AcceptChanges();
			}
			catch (OleDbException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
					Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
					Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
					Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
					Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
				}
				//myOleDbConnection.Close();
				//return with error
				return false;
			}
			//myOleDbConnection.Close();
			//everything went fine
			return true;
		}

		public virtual bool WriteChanges(DataSet ds)
		{
			//check OleDb DataAdapter
			if (myOleDbDataAdapter == null)
				throw new Exception("DataAdapter not initialized.");

			//OleDb
			try
			{
				//Update DataBase
				myOleDbDataAdapter.Update(ds);
			}
			catch (OleDbException e)
			{
				for (int i=0; i < e.Errors.Count; i++)
				{
					Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
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
            lock (myOleDbConnection)
            {
                byte[] result;
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOleDbConnection.Open();

                    //select
                    string sqlSelectCommand = "SELECT `" + column + "` FROM " + table + " WHERE id=" + id;
                    OleDbCommand myOleDbCommand = new OleDbCommand(sqlSelectCommand, myOleDbConnection);

                    //read all at once
                    result = (byte[])myOleDbCommand.ExecuteScalar();

                    /*
                            //read from stream (for very large binary data)
                            OleDbDataReader dataReader = myOleDbCommand.ExecuteReader(CommandBehavior.SequentialAccess);

                            long startIndex = 0;
                            int bufferSize = 4096;
                            byte[] outbyte = new byte[bufferSize];
					
                            MemoryStream ms = new MemoryStream();
                            BinaryWriter bw = new BinaryWriter(ms);

                            // Read the bytes into outbyte[] and retain the number of bytes returned.
                            long retval = dataReader.GetBytes(0, startIndex, outbyte, 0, bufferSize);

                            // Continue reading and writing while there are bytes beyond the size of the buffer.
                            while (retval == bufferSize)
                            {
                                bw.Write(outbyte);
                                bw.Flush();

                                // Reposition the start index to the end of the last buffer and fill the buffer.
                                startIndex += bufferSize;
                                retval = dataReader.GetBytes(0, startIndex, outbyte, 0, bufferSize);
                            }

                            // Write the remaining buffer.
                            bw.Write(outbyte, 0, (int)retval - 1);
                            bw.Flush();

                            bw.Close();					
                            ms.Close();
					
                            return ms.GetBytes();
                            */
                }
                catch (OleDbException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    if (autoOpenClose)
                        myOleDbConnection.Close();
                    //return with error			
                    return null;
                }

                if (autoOpenClose)
                    myOleDbConnection.Close();
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
            lock (myOleDbConnection)
            {
                try
                {
                    //open Connection
                    if (autoOpenClose)
                        myOleDbConnection.Open();

                    //update binary as OleDbCommand parameter
                    //string sqlUpdateCommand = "UPDATE " + table + " SET `" + column + "`=@" + column + " WHERE id=" + id;
                    string sqlUpdateCommand = "UPDATE " + table + " SET `" + column + "`=? WHERE id=" + id;
                    OleDbCommand myOleDbCommand = new OleDbCommand(sqlUpdateCommand, myOleDbConnection);
                    if (bytes != null)
                        myOleDbCommand.Parameters.Add("@" + column, OleDbType.Binary, bytes.Length).Value = bytes;
                    else
                        myOleDbCommand.Parameters.Add("@" + column, OleDbType.Binary, 0).Value = System.DBNull.Value;

                    myOleDbCommand.ExecuteNonQuery();
                }
                catch (OleDbException e)
                {
                    for (int i = 0; i < e.Errors.Count; i++)
                    {
                        Trace.WriteLine("OleDb-Error #" + i + ":", "Error");
                        Trace.WriteLine("Message: " + e.Errors[i].Message, "Error");
                        Trace.WriteLine("Native: " + e.Errors[i].NativeError.ToString(), "Error");
                        Trace.WriteLine("Source: " + e.Errors[i].Source, "Error");
                        Trace.WriteLine("SQL: " + e.Errors[i].SQLState, "Error");
                    }
                    if (autoOpenClose)
                        myOleDbConnection.Close();
                    //return with error			
                    return false;
                }

                if (autoOpenClose)
                    myOleDbConnection.Close();
                //everything went fine
                return true;
            }
		}
		
	}
}
