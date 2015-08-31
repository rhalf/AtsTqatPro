using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProModel {
    public class Server {

        public int Id {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public string Ip {
            get;
            set;
        }

        public int PortHttp {
            get;
            set;
        }

        public int PortCommand {
            get;
            set;
        }

    }
}
