using System;

namespace Database.Odbc
{
	/// <summary>
	/// Summary description for OdbcDriver.
	/// </summary>
	public class OdbcDriver
	{
		/// <summary>
		/// ODBC Driver Name.
		/// </summary>
		private string m_ODBCDriverName = null;

		private string m_APILevel = null;
		private string m_ConnectFunctions = null;
		private string m_Driver = null;
		private string m_DriverODBCVer = null;
		private string m_FileExtns = null;
		private string m_FileUsage = null;
		private string m_SetUp = null;
		private string m_SQLLevel = null;
		private string m_UsageCount = null;

		// Additional atttributes for INTERSOLV 3.00 32-BIT ParadoxFile (*.db).
		// Additional atttributes for INTERSOLV 3.11 32-BIT ParadoxFile (*.db).
		// Microsoft ODBC for Oracle has 'm_CPTimeOut' extra but doesn't have
		// 'm_FileExtns' atttribute.
		private string m_CPTimeOut = null;
		private string m_PdxUnInstall = null;

		private OdbcDriver(string drivername, string apilevel,
			string connectfunctions, string driver, string driverodbcver,
			string fileextns, string fileusage, string setup, string sqllevel,
			string usagecount, string cptimeout, string pdxuninstall)
		{
			m_ODBCDriverName = drivername;
			m_APILevel = apilevel;
			m_ConnectFunctions = connectfunctions;
			m_Driver = driver;
			m_DriverODBCVer = driverodbcver;
			m_FileExtns = fileextns;
			m_FileUsage = fileusage;
			m_SetUp = setup;
			m_SQLLevel = sqllevel;
			m_UsageCount = usagecount;
			m_CPTimeOut = cptimeout;
			m_PdxUnInstall = pdxuninstall;
		}

		#region properties
		public string OdbcDriverName
		{
			get {return m_ODBCDriverName;}
		}

		public string ApiLevel
		{
			get {return m_APILevel;}
		}

		public string ConnectFunctions
		{
			get {return m_ConnectFunctions;}
		}

		public string DriverDll
		{
			get {return m_Driver;}
		}

		public string DriverOdbcVer
		{
			get {return m_DriverODBCVer;}
		}

		public string FileExtensions
		{
			get {return m_FileExtns;}
		}

		public string FileUsage
		{
			get {return m_FileUsage;}
		}

		public string SetUp
		{
			get {return m_SetUp;}
		}

		public string SqlLevel
		{
			get {return m_SQLLevel;}
		}

		public string UsageCount
		{
			get {return m_UsageCount;}
		}
		#endregion

		public override string ToString()
		{
			return m_ODBCDriverName;
		}

		public static OdbcDriver ParseForDriver(string driverName, 
			string[] driverElements, string[] driverElmVals)
		{
			OdbcDriver odbcdriver = null;
			if (driverElements != null && driverElmVals != null)
			{
				string apilevel=null;
				string connectfunctions=null;
				string driver=null;
				string driverodbcver=null;
				string fileextns=null;
				string fileusage=null;
				string setup=null;
				string sqllevel=null;
				string usagecount=null;
				string cptimeout=null;
				string pdxuninstall=null;

				// For each element defined for a typical Driver get
				// its value.
				int i=0;
				foreach (string driverElement in driverElements)
				{
					switch (driverElement.ToLower())
					{
						case "apilevel":
							apilevel = driverElmVals[i].ToString();
							break;
						case "connectfunctions":
							connectfunctions = driverElmVals[i].ToString();
							break;
						case "driver":
							driver = driverElmVals[i].ToString();
							break;
						case "driverodbcver":
							driverodbcver = driverElmVals[i].ToString();
							break;
						case "fileextns":
							fileextns = driverElmVals[i].ToString();
							break;
						case "fileusage":
							fileusage = driverElmVals[i].ToString();
							break;
						case "setup":
							setup = driverElmVals[i].ToString();
							break;
						case "sqllevel":
							sqllevel = driverElmVals[i].ToString();
							break;
						case "usagecount":
							usagecount = driverElmVals[i].ToString();
							break;
						case "cptimeout":
							cptimeout = driverElmVals[i].ToString();
							break;
						case "pdxuninstall":
							pdxuninstall = driverElmVals[i].ToString();
							break;
					}
					i++;
				}
				odbcdriver = new OdbcDriver(
					driverName,
					apilevel,
					connectfunctions,
					driver,
					driverodbcver,
					fileextns,
					fileusage,
					setup,
					sqllevel,
					usagecount,
					cptimeout,
					pdxuninstall); 
			}

			return odbcdriver;
		}

	}
}
