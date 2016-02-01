using System;
using System.Collections;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Database.Odbc
{
	//public enum ODBC_DRIVERS {SQL_SERVER=0};

	/// <summary>
	/// OdbcManager is the class that provides static methods, which provide
	/// access to the various ODBC components such as Drivers List, DSNs List
	/// etc. 
	/// </summary>
	public sealed class OdbcManager
	{
		private const string ODBC_LOC_IN_REGISTRY = "SOFTWARE\\ODBC\\";
		private const string ODBC_INI_LOC_IN_REGISTRY = ODBC_LOC_IN_REGISTRY +
			"ODBC.INI\\";
 
		private const string DSN_LOC_IN_REGISTRY = ODBC_INI_LOC_IN_REGISTRY +
			"ODBC Data Sources\\";

		private const string ODBCINST_INI_LOC_IN_REGISTRY = ODBC_LOC_IN_REGISTRY +
			"ODBCINST.INI\\";

		private const string ODBC_DRIVERS_LOC_IN_REGISTRY = ODBCINST_INI_LOC_IN_REGISTRY +
			"ODBC Drivers\\";

		// Win32 API
		[DllImport("ODBCCP32.dll")]
		public static extern bool SQLConfigDataSource(IntPtr parent, int request, string driver, string attributes);
		[DllImport("ODBCCP32.dll")]
		public static extern bool SQLManageDataSources(IntPtr parent);
		[DllImport("ODBCCP32.dll")]
		public static extern bool SQLCreateDataSource(IntPtr parent, string lpszDS);

		public enum SqlRequest
		{
			ODBC_ADD_DSN = 1,
			ODBC_CONFIG_DSN,
			ODBC_REMOVE_DSN,
			ODBC_ADD_SYS_DSN,
			ODBC_CONFIG_SYS_DSN,
			ODBC_REMOVE_SYS_DSN
		}

		private OdbcManager()
		{}

		/// <summary>
		/// Returns the ODBC Drivers installed in the local machine.
		/// </summary>
		/// <returns></returns>
		public static OdbcDriver[] GetOdbcDrivers()
		{
			ArrayList driversList = new ArrayList();
			OdbcDriver[] odbcDrivers = null;

			// Get the key for
			// "KHEY_LOCAL_MACHINE\\SOFTWARE\\ODBC\\ODBCINST.INI\\ODBC Drivers\\"
			// (ODBC_DRIVERS_LOC_IN_REGISTRY) that contains all the drivers
			// that are installed in the local machine.
			RegistryKey odbcDrvLocKey = OpenComplexSubKey(Registry.LocalMachine, 
				ODBC_DRIVERS_LOC_IN_REGISTRY, false);

			if (odbcDrvLocKey != null)
			{
				// Get all Driver entries defined in ODBC_DRIVERS_LOC_IN_REGISTRY.
				string [] driverNames =  odbcDrvLocKey.GetValueNames();
				odbcDrvLocKey.Close();
				if (driverNames != null)
				{
					// Foreach Driver entry in the ODBC_DRIVERS_LOC_IN_REGISTRY,
					// goto the Key ODBCINST_INI_LOC_IN_REGISTRY+driver and get
					// elements of the DSN entry to create ODBCDSN objects.
					foreach (string driverName in driverNames)
					{
						OdbcDriver odbcDriver = GetOdbcDriver(driverName);
						if (odbcDriver != null)
							driversList.Add(odbcDriver);
					}

					if (driversList.Count>0)
					{
						// Create ODBCDriver objects equal to number of valid objects
						// in the ODBC Drivers ArrayList.
						odbcDrivers = new OdbcDriver[driversList.Count];
						driversList.CopyTo(odbcDrivers,0);
					}
				}
			}
			return odbcDrivers;
		}

		/// <summary>
		/// Returns driver object based on the driver name.
		/// </summary>
		/// <param name="driverName"></param>
		/// <returns>ODBCDriver object</returns>
		public static OdbcDriver GetOdbcDriver(string driverName)
		{
			int j=0;
			OdbcDriver odbcDriver = null;
			string[] driverElements = null;
			string[] driverElmVals = null;
			RegistryKey driverNameKey = null;

			// Get the key for ODBCINST_INI_LOC_IN_REGISTRY+dsnName.
			driverNameKey = OpenComplexSubKey(Registry.LocalMachine,
				ODBCINST_INI_LOC_IN_REGISTRY + driverName, false);

			if (driverNameKey != null)
			{
				// Get all elements defined in the above key
				driverElements = driverNameKey.GetValueNames();

				// Create Driver Element values array.
				driverElmVals = new string[driverElements.Length];

				// For each element defined for a typical Driver get
				// its value.
				foreach (string driverElement in driverElements)
				{
					driverElmVals[j] = driverNameKey.GetValue(driverElement).ToString();
					j++;
				}

				// Create ODBCDriver Object.
				odbcDriver = OdbcDriver.ParseForDriver(driverName,
					driverElements, driverElmVals);

				driverNameKey.Close();
			}
			return odbcDriver;
		}
		/// <summary>
		/// Returns the System Data Source Name (DSN) entries as
		/// array of ODBCDSN objects.
		/// </summary>
		/// <returns>Array of System DSNs</returns>
		public static OdbcDsn[] GetSystemDsnList()
		{
			return GetDsnList(Registry.LocalMachine);
		}

		/// <summary>
		/// Returns one System ODBCDSN Object.
		/// </summary>
		/// <param name="dsnName"></param>
		/// <returns></returns>
		public static OdbcDsn GetSystemDsn(string dsnName)
		{
			return GetDsn(Registry.LocalMachine, dsnName);
		}

		/// <summary>
		/// Returns the User Data Source Name (DSN) entries as
		/// array of ODBCDSN objects.
		/// </summary>
		/// <returns>Array of User DSNs</returns>
		public static OdbcDsn[] GetUserDsnList()
		{
			return GetDsnList(Registry.CurrentUser);
		}

		/// <summary>
		/// Returns one User OdbcDsn Object.
		/// </summary>
		/// <param name="dsnName"></param>
		/// <returns></returns>
		public static OdbcDsn GetUserDsn(string dsnName)
		{
			return GetDsn(Registry.CurrentUser, dsnName);
		}

		/// <summary>
		/// Returns the Data Source Name (DSN) entries as array of
		/// ODBCDSN objects.
		/// </summary>
		/// <returns>Array of DSNs based on the baseKey parameter</returns>
		private static OdbcDsn[] GetDsnList(RegistryKey baseKey)
		{
			ArrayList dsnList = new ArrayList();
			OdbcDsn[] odbcDSNs = null;

			if (baseKey == null)
				return null;

			// Get the key for (using the baseKey parameter passed in)
			// "\\SOFTWARE\\ODBC\\ODBC.INI\\ODBC Data Sources\\" (DSN_LOC_IN_REGISTRY)
			// that contains all the configured Data Source Name (DSN) entries.
			RegistryKey dsnNamesKey = OpenComplexSubKey(baseKey, 
				DSN_LOC_IN_REGISTRY, false);

			if (dsnNamesKey != null)
			{
				// Get all DSN entries defined in DSN_LOC_IN_REGISTRY.
				string [] dsnNames =  dsnNamesKey.GetValueNames();
				if (dsnNames != null)
				{
					// Foreach DSN entry in the DSN_LOC_IN_REGISTRY, goto the
					// Key ODBC_INI_LOC_IN_REGISTRY+dsnName and get elements of
					// the DSN entry to create ODBCDSN objects.
					foreach (string dsnName in dsnNames)
					{
						// Get ODBC DSN object.
						OdbcDsn odbcDSN = GetDsn(baseKey, dsnName);
						if(odbcDSN != null)
							dsnList.Add(odbcDSN);
					}

					if (dsnList.Count>0)
					{
						// Create ODBCDSN objects equal to number of valid objects
						// in the DSN ArrayList.
						odbcDSNs = new OdbcDsn[dsnList.Count];
						dsnList.CopyTo(odbcDSNs,0);
					}
				}

				dsnNamesKey.Close();
			}
			return odbcDSNs;
		}

		/// <summary>
		/// Returns one ODBC DSN object
		/// </summary>
		/// <param name="baseKey"></param>
		/// <param name="dsnName"></param>
		/// <returns>ODBC DSN object</returns>
		private static OdbcDsn GetDsn(RegistryKey baseKey, string dsnName)
		{
			int j=0;
			string dsnDriverName = null;
			RegistryKey dsnNamesKey = null;
			RegistryKey dsnNameKey = null;
			string [] dsnElements = null;
			string [] dsnElmVals = null;
			OdbcDsn odbcDSN = null;

			// Get the key for (using the baseKey parmetre passed in)
			// "\\SOFTWARE\\ODBC\\ODBC.INI\\ODBC Data Sources\\" (DSN_LOC_IN_REGISTRY)
			// that contains all the configured Data Source Name (DSN) entries.
			dsnNamesKey = OpenComplexSubKey(baseKey, DSN_LOC_IN_REGISTRY, false);
			if (dsnNamesKey != null)
			{
				// Get the name of the driver for which the DSN is 
				// defined.
				try
				{
					dsnDriverName = dsnNamesKey.GetValue(dsnName).ToString();
				}
				catch (NullReferenceException)
				{
					return null;
				}
				finally
				{
					dsnNamesKey.Close();
				}
			}

			// Get the key for ODBC_INI_LOC_IN_REGISTRY+dsnName.
			dsnNameKey = OpenComplexSubKey(baseKey, ODBC_INI_LOC_IN_REGISTRY + dsnName, false);
			if (dsnNameKey != null)
			{
				// Get all elements defined in the above key
				dsnElements = dsnNameKey.GetValueNames();

				// Create DSN Element values array.
				dsnElmVals = new string[dsnElements.Length];

				// For each element defined for a typical DSN get
				// its value.
				foreach (string dsnElement in dsnElements)
				{
					dsnElmVals[j] = dsnNameKey.GetValue(dsnElement).ToString();
					j++;
				}

				// Create ODBCDSN Object.
				odbcDSN = OdbcDsn.ParseForOdbcDsn(dsnName, dsnDriverName,
					dsnElements, dsnElmVals);

				dsnNamesKey.Close();
			}
			return odbcDSN;
		}

		/// <summary>
		/// Returns the registry key for a complex string that
		/// represents the key that is to be returned. The 'baseKey' parameter
		/// passed to this method is the root registry key on which the
		/// complex sub key has to be created. The 'complexKeyStr' is the 
		/// stirng representation of the key to be created over the 'baseKey'.
		/// The syntax of the 'complexKeyStr' is "KEY1//KEY2//KEY3//...//".
		/// The "..." in the above syntax represents the repetetion. This
		/// method parses the 'compleKeyStr' parameter value and keep building
		/// the keys following the path of the keys listed in the string. Each
		/// key is built upon its previous key. First Key (KEY1) is built based
		/// on the 'basKey' parameter. Second key (KEY2) is based on the first
		/// key (Key creatd for KEY1) and so on... . The writable parameter 
		/// represents whether final key has to be writable or not.
		/// </summary>
		/// <param name="baseKey"></param>
		/// <param name="complexKeyStr"></param>
		/// <param name="writable"></param>
		/// <returns>RegistryKey For the complex Subkey. </returns>
		public static RegistryKey OpenComplexSubKey(RegistryKey baseKey,
			string complexKeyStr, bool writable)
		{
			int prevLoc=0,currLoc = 0;
			string subKeyStr=complexKeyStr;
			RegistryKey finalKey = baseKey;

			if (baseKey == null)
				return null;

			if (complexKeyStr == null)
				return finalKey;

			// Search for the occurence of "\\" character in the complex string
			// and get all the characters upto "\\" from the start of search
			// point (prevLoc) as the keyString. Open a key out of string 
			// keyString.
			do
			{
				currLoc = complexKeyStr.IndexOf("\\", prevLoc);
				if (currLoc != -1)
				{
					subKeyStr = complexKeyStr.Substring(prevLoc, currLoc-prevLoc);
					prevLoc = currLoc+1;
				}
				else
				{
					subKeyStr = complexKeyStr.Substring(prevLoc);
				}

				if (subKeyStr.Length > 0)
					finalKey = finalKey.OpenSubKey(subKeyStr, writable);
			}
			while(currLoc != -1);

			return finalKey;
		}
	}
}
