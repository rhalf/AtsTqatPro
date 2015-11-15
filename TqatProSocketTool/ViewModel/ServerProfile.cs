using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProSocketTool.ViewModel {
    public class ServerProfile  : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged (String propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public String Brand {
            get;
            set;
        }

        public String Model {
            get;
            set;
        }

        Boolean isEnabled = false;
        public Boolean IsEnabled {
            get {
                return this.isEnabled;
            }
            set {
                this.isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        String ipAddress = "";
        public String IpAddress {
            get {
                return this.ipAddress;
            }
            set {
                ipAddress = value;
                OnPropertyChanged("IpAddress");
            }
        }

        public List<String> IpAddresses {
            get;
            set;
        }

        Int32 port = 0;
        public Int32 Port {
            get { return this.port; }
            set {
                this.port = value;
                OnPropertyChanged("Port");
            }
        }


        Int32 tcpClient = 0;
        public Int32 TcpClient {
            get {
                return tcpClient;
            }
            set {
                this.tcpClient = value;
                OnPropertyChanged("TcpClient");
            }
        }
        Int32 tcpClientReceivePacket = 0;
        public Int32 TcpClientReceivePacket {
            get {
                return this.tcpClientReceivePacket;
            }
            set {
                this.tcpClientReceivePacket = value;
                OnPropertyChanged("TcpClientReceivePacket");
            }
        }

        Int32 tcpClientSentPacket = 0;
        public Int32 TcpClientSentPacket {
            get {
                return this.tcpClientSentPacket;
            }
            set {
                this.tcpClientSentPacket = value;
                OnPropertyChanged("TcpClientSentPacket");
            }
        }

    }
}
