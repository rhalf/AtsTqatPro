using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqatProModel;

namespace TqatProModel {

    public class Tracker : IDisposable {
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose (bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects).
                    this.Collections = null;
                    this.CompanyDatabaseName = null;
                    this.Note = null;
                    this.Emails = null;
                    this.DatabaseName = null;
                    this.VehicleModel = null;
                    this.VehicleRegistration = null;
                    this.Inputs = null;
                    this.DevicePassword= null;
                    this.DriverName = null;
                    this.OwnerName = null;
                    this.SimImei = null;
                    GC.SuppressFinalize(this);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Tracker() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose () {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
        //public bool? IsChecked {
        //    get;
        //    set;
        //}

    }
}

