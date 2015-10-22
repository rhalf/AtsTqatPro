using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqatProModel;

namespace TqatProModel {

    public class Tracker {
        public int Id {
            get;
            set;
        }
        public string CompanyDatabaseName {
            get;
            set;
        }
        public string VehicleRegistration {
            get;
            set;
        }
        public string DriverName {
            get;
            set;
        }
        public string OwnerName {
            get;
            set;
        }
        public string VehicleModel {
            get;
            set;
        }
        public DateTime VehicleRegistrationExpiry {
            get;
            set;
        }
        public string SimNumber {
            get;
            set;
        }
        public string SimImei {
            get;
            set;
        }
        public string TrackerImei {
            get;
            set;
        }
        public int MobileDataProvider {
            get;
            set;
        }
        public int DeviceType {
            get;
            set;
        }
        public string DevicePassword {
            get;
            set;
        }
        public int ImageNumber {
            get;
            set;
        }
        public int DatabaseHost {
            get;
            set;
        }
        public int HttpHost {
            get;
            set;
        }
        public string DatabaseName {
            get;
            set;
        }
        public ConcurrentQueue<User> Users {
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
        public string Emails {
            get;
            set;
        }
        public int MileageInitial {
            get;
            set;
        }
        public int MileageLimit {
            get;
            set;
        }
        public int SpeedLimit {
            get;
            set;
        }
        public string Inputs {
            get;
            set;
        }
        public int IdlingTime {
            get;
            set;
        }
        public string Note {
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

