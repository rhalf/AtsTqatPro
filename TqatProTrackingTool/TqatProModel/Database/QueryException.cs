using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProModel.Database {
    public class QueryException : Exception {
        private int number = 0;
        private string message = null;


        public QueryException(int number, string message) {
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
