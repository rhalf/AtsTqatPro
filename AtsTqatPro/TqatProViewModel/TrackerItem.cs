using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TqatProModel;

namespace TqatProViewModel {
    public class TrackerItem {

        Tracker tracker;

        public TrackerItem(Tracker tracker) {
            this.tracker = tracker;
        }
        public string VehicleRegistration {
            get {
                return tracker.VehicleRegistration;
            }
            set {
                tracker.VehicleRegistration = value;
            }
        }
        public string DriverName {
            get {
                return tracker.DriverName;
            }
            set {
                tracker.VehicleRegistration = value;
            }
        }
        public string OwnerName {
            get {
                return tracker.OwnerName;
            }
            set {
                tracker.OwnerName = value;
            }
        }
        public string VehicleModel {
            get {
                return tracker.VehicleModel;
            }
            set {
                tracker.VehicleModel = value;
            }
        }
        public string TrackerImei {
            get {
                return tracker.TrackerImei;
            }
            set {
                tracker.VehicleRegistration = value;
            }
        }
        public string SimImei {
            get {
                return tracker.SimImei;
            }
            set {
                tracker.SimImei = value;
            }
        }
        public string SimNumber {
            get {
                return tracker.SimNumber;
            }
            set {
                tracker.SimImei = value;
            }
        }
        public bool IsChecked {
            get;
            set;
        }
        public Tracker getTracker () {
            return this.tracker;
        }
    }
}
