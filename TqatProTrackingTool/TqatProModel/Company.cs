using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TqatProModel {
    public class Company {

        

        public int Id {
            get;
            set;
        }

        public string Username {
            get;
            set;
        }

        public string DisplayName {
            get;
            set;
        }

        public int Host {
            get;
            set;
        }

        public string Email {
            get;
            set;
        }

        public string PhoneNo {
            get;
            set;
        }

        public string MobileNo {
            get;
            set;
        }

        public string Address {
            get;
            set;
        }
        public string DatabaseName {
            get;
            set;
        }
        public bool IsActive {
            get;
            set;
        }

        public DateTime DateTimeCreated {
            get;
            set;
        }
        public DateTime DateTimeExpired {
            get;
            set;
        }

        public List<Geofence> Geofences {
            get;
            set;
        }

        public List<Tracker> Trackers {
            get;
            set;
        }

    }
}
