using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using TqatProModel;

namespace TqatProViewModel {
    public class TrackerDatabaseSizeItem {
        TrackerDatabaseSize trackerDatabaseSize;

        public TrackerDatabaseSizeItem(TrackerDatabaseSize trackerDatabaseSize) {
            this.trackerDatabaseSize = trackerDatabaseSize;
        }

        public TrackerDatabaseSize TrackerDatabaseSize {
            set {
                this.trackerDatabaseSize = value;
            }
        }
        public string Name {
            get {
                return this.trackerDatabaseSize.Name;
            }
            set {
                this.trackerDatabaseSize.Name = value;
            }
        }

        public string Size {
            get {
                return Math.Round(this.trackerDatabaseSize.DatabaseSize, 2).ToString("0.00");
            }
        }

        public string FreeSpace {
            get {
                return Math.Round(this.trackerDatabaseSize.DatabaseFreeSpace, 2).ToString("0.00");
            }
        }

        public DateTime DateTimeUpdated {
            get;
            set;
        }

        public MaintenanceServerStatus Status {
            get;
            set;
        }

    }
}
