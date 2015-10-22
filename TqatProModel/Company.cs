using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqatProModel;

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

        public ConcurrentQueue<Geofence> Geofences {
            get;
            set;
        }

        public ConcurrentQueue<Tracker> Trackers {
            get;
            set;
        }

        public ConcurrentQueue<User> Users {
            get;
            set;
        }
    }
}
