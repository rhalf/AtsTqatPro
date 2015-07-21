using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProModel {
    public class Geofence {

        List<Coordinate> points = new List<Coordinate>();


        //Constructor
        public Geofence() {
        }


        public Geofence(List<Coordinate> points) {
            this.points = points;
        }

       

        //Properties
        public int Id {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public string Tracks {
            get;
            set;
        }

        public int Count {
            get {
                return this.points.Count;
            }
        }

        //Methods
        public void addPoint(Coordinate location) {
            this.points.Add(location);
        }

        public List<Coordinate> getPoints() {
            return this.points;
        }

        public static bool isPointInPolygon(Geofence geoFence, Coordinate point) {
            List<Coordinate> location = geoFence.getPoints();

            int indexX, indexJ;
            bool status = false;
            for (indexX = 0, indexJ = location.Count - 1; indexX < location.Count; indexJ = indexX++) {
                if ((((location[indexX].latitude <= point.latitude) && (point.latitude < location[indexJ].latitude))
                        || ((location[indexJ].latitude <= point.latitude) && (point.latitude < location[indexX].latitude)))
                        && (point.longitude < (location[indexJ].longitude - location[indexX].longitude) * (point.latitude - location[indexX].latitude)
                            / (location[indexJ].latitude - location[indexX].latitude) + location[indexX].longitude)) {
                    status = !status;
                }
            }
            return status;
        }

        public static bool isPointInPolygon(List<Coordinate> location, Coordinate point) {

            int indexX, indexJ;
            bool status = false;
            for (indexX = 0, indexJ = location.Count - 1; indexX < location.Count; indexJ = indexX++) {
                if ((((location[indexX].latitude <= point.latitude) && (point.latitude < location[indexJ].latitude))
                        || ((location[indexJ].latitude <= point.latitude) && (point.latitude < location[indexX].latitude)))
                        && (point.longitude < (location[indexJ].longitude - location[indexX].longitude) * (point.latitude - location[indexX].latitude)
                            / (location[indexJ].latitude - location[indexX].latitude) + location[indexX].longitude)) {
                    status = !status;
                }
            }
            return status;
        }
    }
}
