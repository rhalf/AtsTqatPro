using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProViewModel {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;



    public enum MaintenanceServerStatus {
        SUCCESS,
        RUNNING,
        WAITING,
        ERROR,
        WARNING
    }

    public class MaintenanceServerLog {
        static int index = 0;

        public MaintenanceServerLog() {
            this.DateTime = DateTime.Now;
            Sequence = ++index;
        }

        public int Sequence { get; set; }

        public DateTime DateTime { get; set; }

        public string Description { get; set; }

        public MaintenanceServerStatus Status { get; set; }
    }
}


