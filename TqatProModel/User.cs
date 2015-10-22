using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqatProModel;


namespace TqatProModel {
    public class User {
        public int Id {
            get;
            set;
        }
        public string Username {
            get;
            set;
        }
        public string Password {
            get;
            set;
        }
        public string Email {
            get;
            set;
        }
        public string Main {
            get;
            set;
        }
        public string Timezone {
            get;
            set;
        }
        public int AccessLevel {
            get;
            set;
        }
        public DateTime DateTimeExpired {
            get;
            set;
        }
        public DateTime DateTimeCreated {
            get;
            set;
        }
        public bool IsActive {
            get;
            set;
        }
        public string DatabaseName {
            get;
            set;
        }
        public bool? RememberMe {
            get;
            set;
        }
        public ConcurrentQueue<Poi> Pois {
            get;
            set;
        }
        public ConcurrentQueue<Collection> Collections {
            get;
            set;
        }
        //public bool? IsChecked {
        //    get;
        //    set;
        //}

    }
}
