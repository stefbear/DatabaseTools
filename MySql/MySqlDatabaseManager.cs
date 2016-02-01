using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Specialized;
using System.IO;

using MySql.Data.MySqlClient;

namespace Database.MySql
{
    public static class MySqlDatabaseManager
    {
        private static String username;
        private static String password;
        private static String template;

        private static bool credentials = false;

        #region properties
        public static String Username
        {
            get { return MySqlDatabaseManager.username; }
            set
            {
                MySqlDatabaseManager.username = value;
                credentials = true;
            }
        }

        public static String Password
        {
            get { return MySqlDatabaseManager.password; }
            set
            {
                MySqlDatabaseManager.password = value;
                credentials = true;
            }
        }

        public static String Template
        {
            get { return MySqlDatabaseManager.template; }
            set { MySqlDatabaseManager.template = value; }
        }

        public static StringCollection Databases
        {
            get { return GetDatabases(); }
        }
        #endregion

        public static void ProvideCredentials(String username, String password)
        {
            MySqlDatabaseManager.username = username;
            MySqlDatabaseManager.password = password;
            credentials = true;
        }

        private static StringCollection GetDatabases()
        {
            MySqlConnection mySqlConnection = null;
            try
            {
                //Connection string for MySQL
                //credentials provided?
                string mySqlConnectionString;
                if (!credentials)
                {
                    mySqlConnectionString = "Data Source=localhost;Persist Security Info=yes";

                    //inform user about missing credentials
                    MessageBox.Show("No credentials provided. There will only be databases listed which grant anonymous access.", "MySql Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Trace.WriteLine("No credentials provided. There will only be databases listed which grant anonymous access.", "Information");
                }
                else
                {
                    mySqlConnectionString = /*"Database=" + database + ";" +*/
                    "Data Source=localhost;" +
                    "User Id=" + username + ";" +
                    "Password=" + password;
                }

                //Connect to database using MySQL
                mySqlConnection = new MySqlConnection(mySqlConnectionString);
                mySqlConnection.Open();

                //create SQL command
                string sqlCommand = "SHOW DATABASES";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);

                //fetch
                StringCollection sc = new StringCollection();
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                    sc.Add(mySqlDataReader.GetString(0));
                mySqlDataReader.Close();

                return sc;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                MessageBox.Show("Couldn't establish connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("Socket-Error:" + ex.Message, "Error");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error during connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("MySql-Error:", "Error");
                Trace.WriteLine("Code: " + ex.Number, "Error");
                Trace.WriteLine("Message: " + ex.Message, "Error");
                Trace.WriteLine("Source: " + ex.Source, "Error");
            }
            finally
            {
                //close possibly open connection
                if (mySqlConnection != null)
                    mySqlConnection.Close();
            }

            return null;
        }

        public static bool SetupDatabase(string name, string username, string password)
        {
            MySqlConnection mySqlConnection = null;
            try
            {
                //Connection string for MySQL
                //credentials provided?
                string mySqlConnectionString;
                if (username == null || username.Length == 0)
                {
                    mySqlConnectionString = "Data Source=localhost;Persist Security Info=yes";

                    //inform user about missing credentials
                    //MessageBox.Show("No credentials provided. There will only be databases listed which grant anonymous access.", "MySql Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Trace.WriteLine("No credentials provided. There will only be databases listed which grant anonymous access.", "Information");
                }
                else
                {
                    mySqlConnectionString = /*"Database=" + database + ";" +*/
                    "Data Source=localhost;" +
                    "User Id=" + username + ";" +
                    "Password=" + password;
                }

                //Connect to database using MySQL
                mySqlConnection = new MySqlConnection(mySqlConnectionString);
                mySqlConnection.Open();

                //create Database
                string sqlCommand = "CREATE DATABASE " + name + ";";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();

                //grant privileges for MySqlDatabaseManager.Username
                sqlCommand = "GRANT Select, Insert, Update, Delete, Create ON " + name + ".* TO '" + MySqlDatabaseManager.username + "';";
                mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                MessageBox.Show("Couldn't establish connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("Socket-Error:" + ex.Message, "Error");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error during connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("MySql-Error:", "Error");
                Trace.WriteLine("Code: " + ex.Number, "Error");
                Trace.WriteLine("Message: " + ex.Message, "Error");
                Trace.WriteLine("Source: " + ex.Source, "Error");
            }
            finally
            {
                //close possibly open connection
                if (mySqlConnection != null)
                    mySqlConnection.Close();
            }

            return false;
        }

        public static bool DeleteDatabase(string name, string username, string password)
        {
            MySqlConnection mySqlConnection = null;
            try
            {
                //Connection string for MySQL
                //credentials provided?
                string mySqlConnectionString;
                if (username == null || username.Length == 0)
                {
                    mySqlConnectionString = "Data Source=localhost;Persist Security Info=yes";

                    //inform user about missing credentials
                    //MessageBox.Show("No credentials provided. There will only be databases listed which grant anonymous access.", "MySql Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Trace.WriteLine("No credentials provided. There will only be databases listed which grant anonymous access.", "Information");
                }
                else
                {
                    mySqlConnectionString = /*"Database=" + database + ";" +*/
                    "Data Source=localhost;" +
                    "User Id=" + username + ";" +
                    "Password=" + password;
                }

                //Connect to database using MySQL
                mySqlConnection = new MySqlConnection(mySqlConnectionString);
                mySqlConnection.Open();

                //delete Database
                string sqlCommand = "DROP DATABASE " + name + ";";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                MessageBox.Show("Couldn't establish connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("Socket-Error:" + ex.Message, "Error");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error during connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("MySql-Error:", "Error");
                Trace.WriteLine("Code: " + ex.Number, "Error");
                Trace.WriteLine("Message: " + ex.Message, "Error");
                Trace.WriteLine("Source: " + ex.Source, "Error");
            }
            finally
            {
                //close possibly open connection
                if (mySqlConnection != null)
                    mySqlConnection.Close();
            }

            return false;
        }

        public static bool RenameDatabase(string name, string username, string password)
        {
            MySqlConnection mySqlConnection = null;
            try
            {
                //Connection string for MySQL
                //credentials provided?
                string mySqlConnectionString;
                if (username == null || username.Length == 0)
                {
                    mySqlConnectionString = "Data Source=localhost;Persist Security Info=yes";

                    //inform user about missing credentials
                    //MessageBox.Show("No credentials provided. There will only be databases listed which grant anonymous access.", "MySql Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Trace.WriteLine("No credentials provided. There will only be databases listed which grant anonymous access.", "Information");
                }
                else
                {
                    mySqlConnectionString = /*"Database=" + database + ";" +*/
                    "Data Source=localhost;" +
                    "User Id=" + username + ";" +
                    "Password=" + password;
                }

                //Connect to database using MySQL
                mySqlConnection = new MySqlConnection(mySqlConnectionString);
                mySqlConnection.Open();

                //rename Database
                string sqlCommand = "RENAME DATABASE " + name + ";";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                MessageBox.Show("Couldn't establish connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("Socket-Error:" + ex.Message, "Error");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error during connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("MySql-Error:", "Error");
                Trace.WriteLine("Code: " + ex.Number, "Error");
                Trace.WriteLine("Message: " + ex.Message, "Error");
                Trace.WriteLine("Source: " + ex.Source, "Error");
            }
            finally
            {
                //close possibly open connection
                if (mySqlConnection != null)
                    mySqlConnection.Close();
            }

            return false;
        }

        public static bool CreateTables(string database, string username, string password, string templateString)
        {
            MySqlConnection mySqlConnection = null;
            try
            {
                //Connection string for MySQL
                //credentials provided?
                string mySqlConnectionString;
                if (!credentials)
                {
                    mySqlConnectionString = "Data Source=localhost;Persist Security Info=yes";

                    //inform user about missing credentials
                    //MessageBox.Show("No credentials provided. There will only be databases listed which grant anonymous access.", "MySql Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Trace.WriteLine("No credentials provided. There will only be databases listed which grant anonymous access.", "Information");
                }
                else
                {
                    mySqlConnectionString = "Database=" + database + ";" +
                    "Data Source=localhost;" +
                    "User Id=" + username + ";" +
                    "Password=" + password;
                }

                //Connect to database using MySQL
                mySqlConnection = new MySqlConnection(mySqlConnectionString);
                mySqlConnection.Open();

                //read template schema
                string sqlCommand = templateString;
                MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                MessageBox.Show("Couldn't establish connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("Socket-Error:" + ex.Message, "Error");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error during connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("MySql-Error:", "Error");
                Trace.WriteLine("Code: " + ex.Number, "Error");
                Trace.WriteLine("Message: " + ex.Message, "Error");
                Trace.WriteLine("Source: " + ex.Source, "Error");
            }
            finally
            {
                //close possibly open connection
                if (mySqlConnection != null)
                    mySqlConnection.Close();
            }

            return false;
        }

        public static bool CreateTables(string database, string username, string password, FileInfo template)
        {
            MySqlConnection mySqlConnection = null;
            try
            {
                //Connection string for MySQL
                //credentials provided?
                string mySqlConnectionString;
                if (!credentials)
                {
                    mySqlConnectionString = "Data Source=localhost;Persist Security Info=yes";

                    //inform user about missing credentials
                    //MessageBox.Show("No credentials provided. There will only be databases listed which grant anonymous access.", "MySql Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Trace.WriteLine("No credentials provided. There will only be databases listed which grant anonymous access.", "Information");
                }
                else
                {
                    mySqlConnectionString = "Database=" + database + ";" +
                    "Data Source=localhost;" +
                    "User Id=" + username + ";" +
                    "Password=" + password;
                }

                //Connect to database using MySQL
                mySqlConnection = new MySqlConnection(mySqlConnectionString);
                mySqlConnection.Open();

                //read template schema from file
                string sqlCommand = LoadTemplate(template);
                MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                MessageBox.Show("Couldn't establish connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("Socket-Error:" + ex.Message, "Error");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error during connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("MySql-Error:", "Error");
                Trace.WriteLine("Code: " + ex.Number, "Error");
                Trace.WriteLine("Message: " + ex.Message, "Error");
                Trace.WriteLine("Source: " + ex.Source, "Error");
            }
            finally
            {
                //close possibly open connection
                if (mySqlConnection != null)
                    mySqlConnection.Close();
            }

            return false;
        }

        internal static string LoadTemplate(FileInfo template)
        {
            if (template == null)
                return null;

            FileStream fileStream = null;
            StreamReader streamReader = null;
            try
            {
                //open file
                fileStream = new FileStream(template.FullName, FileMode.Open);
                streamReader = new StreamReader(fileStream);

                //get lines
                StringBuilder sb = new StringBuilder();
                string line = null;
                while ((line = streamReader.ReadLine()) != null)
                    sb.AppendLine(line);

                return sb.ToString();
            }
            catch (IOException error)
            {
                Trace.WriteLine("Datei '" + template + "' nicht gefunden, fehlerhaft oder falsches Format. Dateifehler: " + error.Message, "Error");
                MessageBox.Show("Datei '" + template + "' nicht gefunden, fehlerhaft oder falsches Format. Prüfen Sie ob Pfad und Dateiname noch stimmen.\n" + error.Message,
                    "Dateifehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //close	
                if (streamReader != null)
                    streamReader.Close();

                if (fileStream != null)
                    fileStream.Close();
            }

            return null;
        }

        public static string ShowGrants(string username, string password)
        {
            MySqlConnection mySqlConnection = null;
            try
            {
                //Connection string for MySQL
                //credentials provided?
                string mySqlConnectionString;
                if (username == null || username.Length == 0)
                {
                    mySqlConnectionString = "Data Source=localhost;Persist Security Info=yes";

                    //inform user about missing credentials
                    //MessageBox.Show("No credentials provided. There will only be databases listed which grant anonymous access.", "MySql Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Trace.WriteLine("No credentials provided. There will only be databases listed which grant anonymous access.", "Information");
                }
                else
                {
                    mySqlConnectionString = /*"Database=" + database + ";" +*/
                    "Data Source=localhost;" +
                    "User Id=" + username + ";" +
                    "Password=" + password;
                }

                //Connect to database using MySQL
                mySqlConnection = new MySqlConnection(mySqlConnectionString);
                mySqlConnection.Open();

                //create SQL command
                string sqlCommand = "SHOW GRANTS;";// FOR " + username + "@localhost;";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlCommand, mySqlConnection);

                //fetch
                StringBuilder sb = new StringBuilder();
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                    sb.AppendLine(mySqlDataReader.GetString(0));
                mySqlDataReader.Close();
                
                return sb.ToString();
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                MessageBox.Show("Couldn't establish connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("Socket-Error:" + ex.Message, "Error");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error during connection with MySql-Server: " + ex.Message, "MySql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log
                Trace.WriteLine("MySql-Error:", "Error");
                Trace.WriteLine("Code: " + ex.Number, "Error");
                Trace.WriteLine("Message: " + ex.Message, "Error");
                Trace.WriteLine("Source: " + ex.Source, "Error");
            }
            finally
            {
                //close possibly open connection
                if (mySqlConnection != null)
                    mySqlConnection.Close();
            }

            return null;
        }

       
    }
}
