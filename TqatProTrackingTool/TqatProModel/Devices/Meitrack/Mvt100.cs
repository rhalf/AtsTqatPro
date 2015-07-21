using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;


namespace TqatProModel.Devices.Meitrack {


    class Mvt100 {

        public string rawData {
            get;
            set;
        }

        public TransmissionDirection transmissionDirection {
            get;
            set;
        }

        public int dataIdentifier {
            get;
            set;
        }

        public int dataLength {
            get;
            set;
        }
        public Int64 imei {
            get;
            set;
        }
        public string commandType {
            get;
            set;
        }
        public int eventCode {
            get;
            set;
        }
        public double latitude {
            get;
            set;
        }
        public double longitude {
            get;
            set;
        }
        public DateTime dateTimeSent {
            get;
            set;
        }
        public DateTime dateTimeReceived {
            get;
            set;
        }
        public GpsSignalStatus gpsSignalStatus {
            get;
            set;
        }
        public int gpsSatellites {
            get;
            set;
        }
        public int gsmSignalStrength {
            get;
            set;
        }
        public int speed {
            get;
            set;
        }
        public int direction {
            get;
            set;
        }
        public double horizontalPositioningAccuracy {
            get;
            set;
        }
        public int altitude {
            get;
            set;
        }
        public int mileage {
            get;
            set;
        }
        public int runTime {
            get;
            set;
        }
        public string baseStation {
            get;
            set;
        }
        public int ioPort {
            get;
            set;
        }
        public string analogValue {
            get;
            set;
        }
        public int analogValueAd1 {
            get;
            set;
        }
        public int analogValueAd2 {
            get;
            set;
        }
        public int analogValueAd3 {
            get;
            set;
        }
        public int analogValueAd4 {
            get;
            set;
        }
        public int analogValueAd5 {
            get;
            set;
        }



        public DataTable toDataTable() {
            DataTable dataTable = new DataTable();

            //dataTable.Columns.Add("communicationDirection", typeof(string));
            //dataTable.Columns.Add("dataIdentifier", typeof(int));
            //dataTable.Columns.Add("dataLength", typeof(int));
            //dataTable.Columns.Add("imei", typeof(Int64));
            //dataTable.Columns.Add("commandType", typeof(string));
            //dataTable.Columns.Add("eventCode", typeof(int));
            //dataTable.Columns.Add("latitude", typeof(double));
            //dataTable.Columns.Add("longitude", typeof(double));
            //dataTable.Columns.Add("dateTimeSent", typeof(DateTime));
            //dataTable.Columns.Add("dateTimeReceived", typeof(DateTime));
            //dataTable.Columns.Add("gpsSignalStatus", typeof(string));
            //dataTable.Columns.Add("gpsSatellites", typeof(int));
            //dataTable.Columns.Add("gsmSignalStrength", typeof(int));
            //dataTable.Columns.Add("speed", typeof(int));
            //dataTable.Columns.Add("direction", typeof(int));
            //dataTable.Columns.Add("horizontalPositioningAccuracy", typeof(double));
            //dataTable.Columns.Add("altitude", typeof(int));
            //dataTable.Columns.Add("mileage", typeof(int));
            //dataTable.Columns.Add("runTime", typeof(int));
            //dataTable.Columns.Add("baseStation", typeof(string));
            //dataTable.Columns.Add("ioPort", typeof(int));
            //dataTable.Columns.Add("analogValueAd1", typeof(int));
            //dataTable.Columns.Add("analogValueAd2", typeof(int));
            //dataTable.Columns.Add("analogValueAd3", typeof(int));
            //dataTable.Columns.Add("analogValueAd4", typeof(int));
            //dataTable.Columns.Add("analogValueAd5", typeof(int));


            //DataRow dataRow = dataTable.NewRow();
            //if (parseStatus) {
            //    dataRow["communicationDirection"] = this.transmissionDirection == TransmissionDirection.DEVICE_TO_SERVER ? "DEVICE TO SERVER" : "SERVER TO DEVICE";
            //    dataRow["dataIdentifier"] = this.dataIdentifier;
            //    dataRow["dataLength"] = this.dataLength;
            //    dataRow["imei"] = this.imei;
            //    dataRow["commandType"] = this.commandType;
            //    dataRow["eventCode"] = this.eventCode;
            //    dataRow["latitude"] = this.latitude;
            //    dataRow["longitude"] = this.longitude;
            //    dataRow["dateTimeSent"] = this.dateTimeSent;
            //    dataRow["dateTimeReceived"] = this.dateTimeReceived;
            //    dataRow["gpsSignalStatus"] = this.gpsSignalStatus;
            //    dataRow["gpsSatellites"] = this.gpsSatellites;
            //    dataRow["gsmSignalStrength"] = this.gsmSignalStrength;
            //    dataRow["speed"] = this.speed;
            //    dataRow["direction"] = this.direction;
            //    dataRow["horizontalPositioningAccuracy"] = this.horizontalPositioningAccuracy;
            //    dataRow["altitude"] = this.altitude;
            //    dataRow["mileage"] = this.mileage;
            //    dataRow["runTime"] = this.runTime;
            //    dataRow["baseStation"] = this.baseStation;
            //    dataRow["ioPort"] = this.ioPort;
            //    dataRow["analogValueAd1"] = this.analogValueAd1;
            //    dataRow["analogValueAd2"] = this.analogValueAd2;
            //    dataRow["analogValueAd3"] = this.analogValueAd3;
            //    dataRow["analogValueAd4"] = this.analogValueAd4;
            //    dataRow["analogValueAd5"] = this.analogValueAd5;
            //}
            //this.dataTable.Rows.Add(dataRow);
            return dataTable;
        }


    }
}
