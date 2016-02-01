using System;
using System.Data;

namespace Database.GenericDataSet
{
	/// <summary>
	/// Zusammenfassung für IDatabaseProvider.
	/// </summary>
	public interface IDatabaseProvider
	{
		string	Database {get; set;}
		string	User {get; set;}
		string	Password {get; set;}
		/// <summary>
		/// Sets or gets the Table we are working in.
		/// </summary>
		string	Table {get; set;}
		/// <summary>
		/// Returns true when Database Connection was established without errors.
		/// </summary>
		bool	Connected {get;}

		/// <summary>
		/// Log into Database (establish connection = check if database exists,
		/// check username and password -> close connection) and return any error.
		/// </summary>
		/// <returns></returns>
		string	Login();
		/// <summary>
		/// Instantiates new Connection with ConnectionString,
		/// requires paramters 'database', 'user', 'password'.
		/// </summary>
		void	SetupConnection();
		/// <summary>
		/// Establishes connection with database.
		/// </summary>
		void	OpenConnection();
		/// <summary>
		/// Closes connection to database.
		/// </summary>
		void	CloseConnection();
		/// <summary>
		/// Writes Database Connection Information to Trace Listeners.
		/// </summary>
		void	ConnectionInfo();
		/// <summary>
		/// Creates new Table 'table' with 'fields' in Database.
		/// </summary>
		/// <param name="table"></param>
		/// <param name="fields"></param>
		/// <returns></returns>
		bool	CreateTable(string table, string fields);
		/// <summary>
		/// Sends a specified sqlCommand
		/// to an already established database connection.
		/// </summary>
		/// <param name="sqlCommand"></param>
		void	Select(string sqlCommand);
		/// <summary>
		/// Sends the SQL-Command 'SELECT * FROM table'
		/// to an already established database connection.
		/// </summary>
		void	SelectAll();
		/// <summary>
		/// Sends SQL-Command 'SELECT * FROM table ORDER BY orderby'
		/// to an already established database connection.
		/// </summary>
		/// <param name="orderBy"></param>
		void	SelectAll(string orderBy);
		/// <summary>
		/// Inserts one Row into DB without using CommandBuilder and DataAdapter.
		/// </summary>
		/// <param name="sqlCommand">Parameters in SQL</param>
		/// <param name="autoOpenClose"></param>
		/// <returns>Status</returns>
		bool	InsertRow(string sqlCommand, bool autoOpenClose);
		/// <summary>
		/// Updates Row in DB without using CommandBuilder and DataAdapter
		/// </summary>
		/// <param name="sqlCommand">SQL UPDATE parameters</param>
		/// <param name="autoOpenClose">if true: DB-Connection is opened and closed each time UpdateRow(..) is automatically called</param>
		/// <returns>status</returns>
		bool	UpdateRow(string sqlCommand, bool autoOpenClose);
		/// <summary>
		/// Deletes one Row from DB without using CommandBuilder and DataAdapter
		/// </summary>
		/// <param name="id">Row Id</param>
		/// <param name="autoOpenClose"></param>
		/// <returns>Status</returns>
		bool	DeleteRow(int id, bool autoOpenClose);
		/// <summary>
		/// Returns last inserted DataRow-ID (AutoIncrement Column).
		/// </summary>
		/// <param name="autoOpenClose"></param>
		/// <returns>id</returns>
		int		GetAutoID(bool autoOpenClose);
		/// <summary>
		/// Updates DataSet using DataAdapter's Update-Method and CommandBuilder.
		/// This method programatically chooses whether to update,
		/// insert or delete for each Row individually.
		/// </summary>
		/// <returns></returns>
		bool	WriteChanges();
		/// <summary>
		/// Updates DataSet using DataAdapter's Update-Method, CommandBuilder
		/// by comparing the underlying DataSet with ds.
		/// This method programatically chooses whether to update,
		/// insert or delete for each Row individually.
		/// </summary>
		/// <param name="ds"></param>
		/// <returns></returns>
		bool	WriteChanges(DataSet ds);
		byte[]	GetBytes(int id, string column);
		byte[]	GetBytes(int id, string column, bool autoOpenClose);
		bool	SaveBytes(int id, byte[] bytes, string column);
		bool	SaveBytes(int id, byte[] bytes, string column, bool autoOpenClose);
	}
}
