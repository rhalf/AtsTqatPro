using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProModel {

    public class Coordinate {

        public double latitude {
            get;
            set;
        }

        public double longitude {
            get;
            set;
        }

        public Coordinate(double latitude, double longitude) {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public override string ToString() {
            return "{" + this.latitude.ToString() + "," + this.longitude.ToString() + "}";
        }
    }

}
