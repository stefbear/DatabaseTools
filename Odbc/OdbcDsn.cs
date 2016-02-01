using System;

namespace Database.Odbc
{
	/// <summary>
	/// Summary description for OdbcDsn.
	/// </summary>
	public class OdbcDsn
	{
		private string m_DSNName = null;
		private string m_DSNDriverName = null;
		private string m_DSNDescription = null;
		private string m_DSNServerName = null;
		private string m_DSNDriver = null;
		private string m_DSNDatabase = null;

		private OdbcDsn(string dsnName, string dsnDriverName,
			string description, string server, string driver, string database)
		{
			m_DSNName = dsnName;
			m_DSNDriverName = dsnDriverName;
			m_DSNDescription = description;
			m_DSNServerName = server;
			m_DSNDriver = driver;
			m_DSNDatabase = database;
		}

		#region properties
		public string DsnName
		{
			get {return m_DSNName;}
		}

		public string DsnDriverName
		{
			get {return m_DSNDriverName;}
		}

		public string DsnDescription
		{
			get {return m_DSNDescription;}
		}

		public string DsnServerName
		{
			get {return m_DSNServerName;}
		}

		public string DsnDriverPath
		{
			get {return m_DSNDriver;}
		}

		public string DsnDatabase
		{
			get {return m_DSNDatabase;}
		}
		#endregion

		public override string ToString()
		{
			return m_DSNName;
		}

		public static OdbcDsn ParseForOdbcDsn(string dsnName,
			string dsnDriverName, string[] dsnElements, string[] dsnElmVals)
		{
			OdbcDsn odbcdsn = null;

			if (dsnElements != null && dsnElmVals != null)
			{
				int i=0;
				string description = null;
				string server = null;
				string driver = null;
				string database = null;

				// For each element defined for a typical DSN get
				// its value.
				foreach (string dsnElement in dsnElements)
				{
					switch (dsnElement.ToLower())
					{
						case "description":
							description = dsnElmVals[i];
							break;
						case "server":
							server = dsnElmVals[i];
							break;
						case "driver":
							driver = dsnElmVals[i];
							break;
//						case "driverid":
//						case "safetransactions":
//						case "uid":
						case "database":
						case "dbq":
							database = dsnElmVals[i];
							break;
					}
					i++;
				}
				odbcdsn = new OdbcDsn(dsnName, dsnDriverName, 
					description, server, driver, database); 
			}
			return odbcdsn;
		}

	}
}
