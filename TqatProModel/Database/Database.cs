using System;
using System.Net;

namespace TqatProModel.Database {
    public class Database {
        public String IpAddress {
            get;
            set;
        }

        public Int32 Port {
            get;
            set;
        }
        public String DatabaseName {
            get;
            set;
        }
        public String Username {
            get;
            set;
        }
        public String Password {
            get;
            set;
        }

        public Int32 CommandTimeOut {
            get;
            set;
        }

      
        public string getConnectionString () {
            String connectionString;
            //Standard
            //Server = myServerAddress; Database = myDataBase; Uid = myUsername; Pwd = myPassword;
            //Specifying TCP port
            //Server=myServerAddress;Port=1234;Database=myDataBase;Uid=myUsername; Pwd = myPassword;
            connectionString = "SERVER=" + this.IpAddress + ";" + "PORT=" + this.Port.ToString() + ";" + "DATABASE=" + this.DatabaseName + ";" + "UID=" + this.Username + ";" + "PASSWORD=" + this.Password + ";";
            return connectionString;
        }


    }
}
