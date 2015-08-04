using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProModel.Devices.Meitrack {
    class MeitrackException : Exception {
        private int number = 0;
        private string message = null;

        public MeitrackException(int number, string message) {
            this.number = number;
            this.message = message;
        }

        public override string Message {
            get {
                return this.message;
            }
        }
    }
}
