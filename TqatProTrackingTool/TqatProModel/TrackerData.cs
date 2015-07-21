using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TqatProModel.Devices.Meitrack;

namespace TqatProModel {

    public class TrackerData {
        public int Id {
            get;
            set;
        }
        public int TrackerId {
            get;
            set;
        }
        public DateTime DateTime {
            get;
            set;
        }
        public Coordinate Coordinate {
            get;
            set;
        }
        public int Speed {
            get;
            set;
        }
        public double Mileage {
            get;
            set;
        }
        public int Altitude {
            get;
            set;
        }
        public int Degrees {
            get;
            set;
        }
        public string Direction {
            get;
            set;
        }
        public int GpsSatellites {
            get;
            set;
        }
        public int GsmSignal {
            get;
            set;
        }
        public EventCode EventCode {
            get;
            set;
        }
        public bool ACC {
            get;
            set;
        }
        public bool SOS {
            get;
            set;
        }
        public bool OverSpeed {
            get;
            set;
        }
        public double Battery {
            get;
            set;
        }
        public double BatteryVoltage {
            get;
            set;
        }
        public double ExternalVoltage {
            get;
            set;
        }
        public Geofence Geofence {
            get;
            set;
        }
    }
}

