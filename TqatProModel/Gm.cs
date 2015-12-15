using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqatProModel.Devices.Meitrack;
using TqatProModel.Parser;

namespace TqatProModel {
    //public class Gm {

    //    public Gm (byte[] raw) {
    //        if (raw[0] == '$' && raw[1] == '$') {
    //            #region Validate
    //            //Device    1           2   3       4       5           6       7  8 9 10 11 12  13  14         15          16         17           18                19    20
    //            //@@Q25,353358017784062,A10,35,24.963245,51.598091,151111064417,A,10,13,0,28,0.9,0,20164489,15062657,427|1|238E|6375,0200,0013|0000|0000|0A8B|0842,00000039,*6A

    //            //1,35,9,24,1450984,1.1,8,484707,427 | 2 | 008F | 2DE9,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0003,0000,0000,0A68,0432,00000001
    //            String stringPacket = ASCIIEncoding.UTF8.GetString(raw);

    //            String[] bytesPacket = stringPacket.Split(',');

    //            stringPacket.Substring(0, stringPacket.Length - 2);

    //            if (bytesPacket[7].Equals("V")) {
    //                throw new Exception(bytesPacket[1] + " gps data is invalid.");
    //            }

    //            if (!Meitrack.CheckDataLength(stringPacket)) {
    //                throw new Exception(bytesPacket[1] + " dataLength is wrong.");
    //            }

    //            if (!Meitrack.CheckSum(stringPacket)) {
    //                throw new Exception(bytesPacket[1] + " checksum is wrong.");
    //            }
    //            #endregion

    //            try {

    //                this.Unit = bytesPacket[1];
    //                this.TimeStamp = UnixTime.toUnixTimestamp(bytesPacket[6]).ToString();
    //                this.Latitude = bytesPacket[4];
    //                this.Longitude = bytesPacket[5];
    //                this.Speed = bytesPacket[10];
    //                this.Orientation = bytesPacket[11];
    //                this.Mileage = Math.Round((Double.Parse(bytesPacket[14]) / 1000d), 2).ToString();

    //                //1,			            //(0)
    //                //35,			            //Event code(Decimal)
    //                //11,			            //Number of satellites(Decimal)
    //                //26,			            //GSM signal status(Decimal)
    //                //17160691, 		        //Mileage(Decimal)unit: meter
    //                //0.7, 			            //hpos accuracy(Decimal)
    //                //18, 			            //Altitude(Decimal)unit: meter
    //                //18661240, 		        //Run time(Decimal)unit: second
    //                //427|2|0078|283F, 	        //Base station information(binary|binary|hex|hex)           (8)
    //                //==============================================0200
    //                //0,0,0,0,0,0,0,0,          //Io port lowbyte (low bit start from left)                 (9)
    //                //0,1,0,0,0,0,0,0,          //Io port lowbyte (low bit start from left)                 (17)
    //                //==============================================
    //                //000B,0000,0000,0A6E,0434, //Analog input value                                        (25)
    //                //00000001 		            //System mark    

    //                //gm_id = 10682
    //                //gm_time = 1428992007(1)
    //                //gm_lat = 25.891435(2)
    //                //gm_lng = 51.508528(3)
    //                //gm_speed = 0(4)
    //                //gm_ori = 117(5)
    //                //gm_mileage = 3627.813(6)
    //                //gm_data = 1,35,11,26,6596078,1.0,4,2156297,427|1|2582|5C1E,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0004,0000,0000,0847,0000,00000001(7)

    //                StringBuilder sb = new StringBuilder();
    //                sb.Append("1,");
    //                sb.Append(bytesPacket[3] + ","); //Event code(Decimal)
    //                sb.Append(bytesPacket[8] + ","); //Number of satellites(Decimal)
    //                sb.Append(bytesPacket[9] + ","); //GSM signal status(Decimal)
    //                sb.Append(bytesPacket[14] + ","); //Mileage(Decimal)unit: meter
    //                sb.Append(bytesPacket[12] + ","); //hpos accuracy(Decimal)
    //                sb.Append(bytesPacket[13] + ","); //Altitude(Decimal)unit: meter
    //                sb.Append(bytesPacket[15] + ","); //Run time(Decimal)unit: second
    //                sb.Append(bytesPacket[16] + ","); //Base station information(binary|binary|hex|hex)           (8)

    //                Int32 io = Int32.Parse(bytesPacket[17], System.Globalization.NumberStyles.HexNumber);

    //                for (int index = 0; index < 16; index++) {
    //                    if (((io >> (index)) & 1) == 1) {
    //                        sb.Append("1,");
    //                    } else {
    //                        sb.Append("0,");
    //                    }
    //                }

    //                String[] analog = bytesPacket[18].Split('|');
    //                sb.Append(analog[0] + ",");
    //                sb.Append(analog[1] + ",");
    //                sb.Append(analog[2] + ",");
    //                sb.Append(analog[3] + ",");
    //                sb.Append(analog[4] + ",");
    //                sb.Append("00000001");


    //                this.Data = sb.ToString();
    //                this.LastTime = "0";

    //                return;
    //            } catch {
    //                throw new Exception(bytesPacket[1] + " parsing failed.");
    //            }
    //        }

    //        throw new Exception("Protocol not recognized");
    //    }
    //    public String Id {
    //        get;
    //        set;
    //    }
    //    public String Unit {
    //        get;
    //        set;
    //    }
    //    public String TimeStamp {
    //        get;
    //        set;
    //    }
    //    public String Latitude {
    //        get;
    //        set;
    //    }
    //    public String Longitude {
    //        get;
    //        set;
    //    }

    //    public String Speed {
    //        get;
    //        set;
    //    }
    //    public String Orientation {
    //        get;
    //        set;
    //    }
    //    public String Mileage {
    //        get;
    //        set;
    //    }
    //    public String Data {
    //        get;
    //        set;
    //    }
    //    public String LastTime {
    //        get;
    //        set;
    //    }
    //}
}
