using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace TqatProModel.Database {
    public class Database {
        private string databaseName = null;
        public string Host = null;
        private string username = null;
        private string password = null;
        private int port = 0;
        private int commandTimeOut = 0;

        public Database(string sHost, string username, string password, int port, string databaseName, int commandTimeout) {
            if (String.IsNullOrEmpty(sHost)) {
                throw new DatabaseException(1, "Host is Null or Empty!");
            }
            if (String.IsNullOrEmpty(username)) {
                throw new DatabaseException(1, "Username is Null or Empty!");
            }
            if (String.IsNullOrEmpty(password)) {
                throw new DatabaseException(1, "Password is Null or Empty!");
            }
            if (String.IsNullOrEmpty(databaseName)) {
                throw new DatabaseException(1, "DatabaseName is Null or Empty!");
            }
            if (port < 1 || port > 65536) {
                throw new DatabaseException(1, "Port should be from 1-65536!");
            }

            this.Host = sHost;
            this.username = username;
            this.password = password;
            this.port = port;
            this.databaseName = databaseName;
            this.commandTimeOut = commandTimeout;

        }

        public Database(string host, string username, string password, int port, string databaseName) {
            if (String.IsNullOrEmpty(host)) {
                throw new DatabaseException(1, "Host is Null or Empty!");
            }
            if (String.IsNullOrEmpty(username)) {
                throw new DatabaseException(1, "Username is Null or Empty!");
            }
            if (String.IsNullOrEmpty(password)) {
                throw new DatabaseException(1, "Password is Null or Empty!");
            }
            if (String.IsNullOrEmpty(databaseName)) {
                throw new DatabaseException(1, "DatabaseName is Null or Empty!");
            }
            if (port < 1 || port > 65536) {
                throw new DatabaseException(1, "Port should be from 1-65536!");
            }

            this.Host = host;
            this.username = username;
            this.password = password;
            this.port = port;
            this.databaseName = databaseName;
            this.commandTimeOut = 30;

        }
        public Database(string sHost, string username, string password,int commandTimeout) {
            if (String.IsNullOrEmpty(sHost)) {
                throw new DatabaseException(1, "Host is Null or Empty!");
            }
            if (String.IsNullOrEmpty(username)) {
                throw new DatabaseException(1, "Username is Null or Empty!");
            }
            if (String.IsNullOrEmpty(password)) {
                throw new DatabaseException(1, "Password is Null or Empty!");
            }

            this.Host = sHost;
            this.username = username;
            this.password = password;
            this.port = 3306;
            this.databaseName = null;
            this.commandTimeOut = commandTimeout;
        }
        public Database(string sHost, string username, string password) {
            if (String.IsNullOrEmpty(sHost)) {
                throw new DatabaseException(1, "Host is Null or Empty!");
            }
            if (String.IsNullOrEmpty(username)) {
                throw new DatabaseException(1, "Username is Null or Empty!");
            }
            if (String.IsNullOrEmpty(password)) {
                throw new DatabaseException(1, "Password is Null or Empty!");
            }

            this.Host = sHost;
            this.username = username;
            this.password = password;
            this.port = 3306;
            this.databaseName = null;
            this.commandTimeOut = 30;
        }

        public string getConnectionString() {
            string sConnectionString;
            if (databaseName != null) {
                sConnectionString = "SERVER=" + this.Host + ";" + "DATABASE=" + this.databaseName + ";" + "UID=" + this.username + ";" + "PASSWORD=" + this.password + ";";
            } else {
                sConnectionString = "SERVER=" + this.Host + ";" + "DATABASE=;" + "UID=" + this.username + ";" + "PASSWORD=" + this.password + "; default command timeout="+this.commandTimeOut;
            }

            return sConnectionString;
        }

        public void checkConnection() {
            if (String.IsNullOrEmpty(Host)) {
                throw new DatabaseException(1, "Host is Null or Empty!");
            }
            if (String.IsNullOrEmpty(username)) {
                throw new DatabaseException(1, "Username is Null or Empty!");
            }
            if (String.IsNullOrEmpty(password)) {
                throw new DatabaseException(1, "Password is Null or Empty!");
            }
            if (port < 1 || port > 65536) {
                throw new DatabaseException(1, "Port should be from 1-65536!");
            }

            MySqlConnection mysqlConnection = new MySqlConnection(this.getConnectionString());

            try {
                mysqlConnection.Open();
            } catch (MySqlException mySqlException) {
                throw new DatabaseException(1, mySqlException.Message);
            } catch (Exception exception) {
                throw new DatabaseException(1, exception.Message);
            } finally {
                mysqlConnection.Close();
            }
        }

    }
}
