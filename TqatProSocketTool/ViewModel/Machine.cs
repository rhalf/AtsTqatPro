﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TqatProSocketTool.ViewModel {
    public class Machine : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged (String propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public Machine() {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick (object sender, EventArgs e) {
            this.LocalDateTime = System.DateTime.Now.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            this.UtcDateTime = System.DateTime.Now.ToUniversalTime().ToString("yyyy/MM/dd HH:mm:ss");
            OnPropertyChanged("UtcDateTime");
            OnPropertyChanged("LocalDateTime");
        }
        public String OperatingSystem {
            get {
                //string bit = "";
                //if (System.Environment.Is64BitOperatingSystem) {
                //    bit = " 64 bit";
                //} else {
                //    bit = " 32 bit";
                //}
                return System.Environment.OSVersion.ToString();
            }
        }
        public String Name {
            get {
                return System.Environment.MachineName;
            }
        }
        public List<String> IpAddresses {
            get {
                IPHostEntry host;
                host = Dns.GetHostEntry(Dns.GetHostName());


                List<String> listIp = new List<String>();
                foreach (IPAddress ip in host.AddressList) {
                    if (ip.AddressFamily.ToString() == "InterNetwork") {
                        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                            listIp.Add(ip.ToString());
                        }
                    }
                }
                return listIp;
            }
        }
        public string LocalDateTime {
            get;
            set;
        }
        public string UtcDateTime {
            get;
            set;
        }
        public string Processor {
            get {
                return System.Environment.ProcessorCount.ToString();
            }
        }

    }
}
