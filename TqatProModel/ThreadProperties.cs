
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProModel {
    public class ThreadProperties {

        public int MaxThreadCount {
            get;
            set;
        }
        public int CurrentThreadCount {
            get;
            set;
        }

        public override string ToString() {
            return CurrentThreadCount.ToString() + "/" + MaxThreadCount.ToString();
        }
    }
}
