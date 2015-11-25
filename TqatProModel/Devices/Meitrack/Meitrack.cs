using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace TqatProModel.Devices.Meitrack {

    public enum TransmissionDirection {
        SERVER_TO_DEVICE,   //@@
        DEVICE_TO_SERVER    //$$
    };

    public enum GpsSignalStatus {
        POSITIONED,
        NOT_POSITIONED
    }

    public enum EventCode {
        NONE = 0,                   //0
        INPUT1_ACTIVE = 1,          //SOS PRESSED
        INPUT2_ACTIVE = 2,
        INPUT3_ACTIVE = 3,
        INPUT4_ACTIVE = 4,
        INPUT5_ACTIVE = 5,
        INPUT1_INACTIVE = 9,        //SOS RELEASED
        INPUT2_INACTIVE = 10,
        INPUT3_INACTIVE = 11,
        INPUT4_INACTIVE = 12,
        INPUT5_INACTIVE = 13,
        LOW_BATTERY = 17,
        LOW_EXTERNAL_BATTERY = 18,
        SPEEDING = 19,
        ENTER_GEOFENCE = 20,
        EXIT_GEOFENCE = 21,
        EXTERNAL_BATTERY_ON = 22,
        EXTERNAL_BATTERY_CUT = 23,
        LOST_GPS_SIGNAL = 24,
        GPS_SIGNAL_RECOVERY = 25,
        ENTER_SLEEP = 26,
        EXIT_SLEEP = 27,
        GPS_ANTENNA_CUT = 28,
        DEVICE_REBOOT = 29,
        IMPACT_OR_FALL = 30,
        HEART_BEAT = 31,
        DIRECTION_CHANGE = 32,
        TRACK_BY_DISTANCE = 33,
        REPLY_CURRENT = 34,
        TRACK_BY_TIME_INTERVAL = 35,
        TOW = 36,
        RFID = 37,
        PHOTO = 39,
        TEMPERATURE_HIGH = 50,
        TEMPERATURE_LOW = 51,
        FUEL_FULL = 52,
        FUEL_EMPTY = 53,
        ARMED = 56,
        DISARMED = 57,
        PRESS_INPUT1_TO_CALL = 65,
        PRESS_INPUT2_TO_CALL = 66,
        PRESS_INPUT3_TO_CALL = 67,
        PRESS_INPUT4_TO_CALL = 68,
        PRESS_INPUT5_TO_CALL = 69,
        REJECT_INCOMING_CALL = 70,
        GET_LOCATION_BY_CALL = 71,
        AUTO_ANSWER_INCOMING_CALL = 72,
        LISTEN_IN = 73,
        FAST_DECELERATE = 129,
        FAST_ACCELERATE = 130,
        RPM_HIGH = 131,
        RPM_RECOVERY_TO_NORMAL = 132,
        IDLE_OVERTIME = 133,
        IDLE_RECOVERY = 134,
        FATIGUE_DRIVING = 135,
        ENOUGH_REST = 136,
        ENGINE_TEMPERATURE_OVERHEAT = 137,
        SPEED_RECOVERY = 138,
        MAINTENANCE_NOTICE = 139,
        ENGINE_FAULT = 140,
        EXHAUST_EMISSION_FAULT = 141,
        HEALTH_ABNORMAL = 142,
        FUEL_LOW = 143,
        IGNITION_ON = 144,
        IGNITION_OFF = 145,
        HALT_TO_START = 146,
        START_TO_HALT = 147
    }
    class Meitrack {

        public static Boolean CheckSum (String raw) {
            try {
                String[] tempData = raw.Split('*');

                String checkSumData = raw.Substring(0, raw.Length - 4);
                Byte[] checkSumBytes = ASCIIEncoding.UTF8.GetBytes(checkSumData);
                Int32 result = 0;
                foreach (Byte byte1 in checkSumBytes) {
                    result += (Int32)byte1;
                }


                String checkCode = tempData[1].Trim();
                String checkSum = result.ToString("X2").Substring((result.ToString().Length - 2), 2);
                if (checkCode.Equals(checkSum)) {
                    return true;
                }
                return false;
            } catch {
                return false;
            }
        }

        public static Boolean CheckDataLength (String raw) {
            try {
                Int32 dataLength = Int32.Parse(raw.Substring(3, 3));
                String data = raw.Substring(6);
                Int32 actualLength = data.Length;
                if (dataLength == actualLength) {
                    return true;
                } else {
                    return false;
                }
            } catch {
                return false;
            }
        }

        public static Mvt100 parseMvt100 (string rawData) {
            //$$d140,865734029500608,AAA,35,0.000000,0.000999,000101030330,V,0,31,0,0,0.0,0,0,11003,427|2|008E|53C1,0000,0000|0000|0000|0A51|0393,00000031,*BA
            //$$d140,865734029500608,AAA,35,0.000000,0.000999,000101030330,V,0,31,0,0,(12)0.0,0,0,11003,427|2|008E|53C1,(17)0000,0000|0000|0000|0A51|0393,00000031,*BA
            Mvt100 mvt100 = new Mvt100();

            mvt100.rawData = rawData;

            List<string> listData = mvt100.rawData.Split(',').ToList();


            //Validate
            if (String.IsNullOrEmpty(rawData))
                throw new MeitrackException(1, "rawData is null or empty.");

            if (listData.Count != 21)
                throw new MeitrackException(1, "rawData does not have enough comma.");


            if (!(listData[0].Substring(0, 2) == "@@" || listData[0].Substring(0, 2) == "$$"))
                throw new MeitrackException(1, "rawData has wrong format.");




            //Parse
            if (listData[0].Substring(0, 2) == "@@") {
                mvt100.transmissionDirection = TransmissionDirection.SERVER_TO_DEVICE;
            }
            if (listData[0].Substring(0, 2) == "$$") {
                mvt100.transmissionDirection = TransmissionDirection.DEVICE_TO_SERVER;
            }


            mvt100.dataIdentifier = (int)char.Parse(listData[0].Substring(2, 1));
            mvt100.dataLength = int.Parse(listData[0].Substring(4));
            mvt100.imei = Int64.Parse(listData[1]);
            mvt100.commandType = listData[2];
            mvt100.eventCode = int.Parse(listData[3]);
            mvt100.latitude = double.Parse(listData[4]);
            mvt100.longitude = double.Parse(listData[5]);
            mvt100.dateTimeSent = DateTime.ParseExact(listData[6], "yyMMddhhmmss", System.Globalization.CultureInfo.InvariantCulture);
            mvt100.dateTimeReceived = DateTime.Now;
            mvt100.gpsSignalStatus = listData[7] == "A" ? GpsSignalStatus.POSITIONED : GpsSignalStatus.NOT_POSITIONED;
            mvt100.gpsSatellites = int.Parse(listData[8]);
            mvt100.gsmSignalStrength = int.Parse(listData[9]);
            mvt100.speed = int.Parse(listData[10]);
            mvt100.direction = int.Parse(listData[11]);
            mvt100.horizontalPositioningAccuracy = double.Parse(listData[12]);
            mvt100.altitude = int.Parse(listData[13]);
            mvt100.mileage = int.Parse(listData[14]);
            mvt100.runTime = int.Parse(listData[15]);
            mvt100.baseStation = listData[16];
            mvt100.ioPort = int.Parse(listData[17], System.Globalization.NumberStyles.AllowHexSpecifier);

            mvt100.analogValue = listData[18];

            List<string> analogValues = mvt100.analogValue.Split('|').ToList();
            mvt100.analogValueAd1 = int.Parse(analogValues[0], System.Globalization.NumberStyles.AllowHexSpecifier);
            mvt100.analogValueAd2 = int.Parse(analogValues[1], System.Globalization.NumberStyles.AllowHexSpecifier);
            mvt100.analogValueAd3 = int.Parse(analogValues[2], System.Globalization.NumberStyles.AllowHexSpecifier);
            mvt100.analogValueAd4 = int.Parse(analogValues[3], System.Globalization.NumberStyles.AllowHexSpecifier);
            mvt100.analogValueAd5 = int.Parse(analogValues[4], System.Globalization.NumberStyles.AllowHexSpecifier);

            return mvt100;


        }
    }


}
