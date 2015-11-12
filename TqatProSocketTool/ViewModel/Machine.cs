using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProSocketTool.ViewModel {
    public class ViewDashBoard {
        public ViewDashBoard () {

        }

        public string MachineOperatingSystem {
            get {
                return System.Environment.OSVersion.ToString();
            }
        }

        public string MachineName {
            get;
            set;
        }
        public string Ip {
            get;
            set;
        }
    }
}
