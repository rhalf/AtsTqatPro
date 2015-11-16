using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProSocketTool.Model {
    public class Gm {

        public Gm (byte[] raw) {
            if (raw[0] == '$' && raw[1] == '$') {
                //Device    1           2   3       4       5           6       7  8 9 10 11 12  13  14         15          16         17           18                19    20
                //@@Q25,353358017784062,A10,35,24.963245,51.598091,151111064417,A,10,13,0,28,0.9,0,20164489,15062657,427|1|238E|6375,0200,0013|0000|0000|0A8B|0842,00000039,*6A


                String[] datas = ASCIIEncoding.UTF8.GetString(raw).Split(',');

                this.Unit = datas[1];
                this.TimeStamp = datas[6];
                this.Latitude = datas[4];
                this.Longitude = datas[5];
                this.Speed = datas[4];
                this.Orientation = datas[5];
                this.Mileage = datas[6];

                //1,			            //(0)
                //35,			            //Event code(Decimal)
                //11,			            //Number of satellites(Decimal)
                //26,			            //GSM signal status(Decimal)
                //17160691, 		        //Mileage(Decimal)unit: meter
                //0.7, 			            //hpos accuracy(Decimal)
                //18, 			            //Altitude(Decimal)unit: meter
                //18661240, 		        //Run time(Decimal)unit: second
                //427|2|0078|283F, 	        //Base station information(binary|binary|hex|hex)           (8)
                //==============================================0200
                //0,0,0,0,0,0,0,0,          //Io port lowbyte (low bit start from left)                 (9)
                //0,1,0,0,0,0,0,0,          //Io port lowbyte (low bit start from left)                 (17)
                //==============================================
                //000B,0000,0000,0A6E,0434, //Analog input value                                        (25)
                //00000001 		            //System mark    

                //gm_id = 10682
                //gm_time = 1428992007(1)
                //gm_lat = 25.891435(2)
                //gm_lng = 51.508528(3)
                //gm_speed = 0(4)
                //gm_ori = 117(5)
                //gm_mileage = 3627.813(6)
                //gm_data = 1,35,11,26,6596078,1.0,4,2156297,427|1|2582|5C1E,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0004,0000,0000,0847,0000,00000001(7)
                StringBuilder sb = new StringBuilder();
                sb.Append("1,");
                sb.Append(datas[3] + ","); //Event code(Decimal)
                sb.Append(datas[8] + ","); //Number of satellites(Decimal)
                sb.Append(datas[9] + ","); //GSM signal status(Decimal)
                sb.Append(datas[14] + ","); //Mileage(Decimal)unit: meter
                sb.Append(datas[12] + ","); //hpos accuracy(Decimal)
                sb.Append(datas[13] + ","); //Altitude(Decimal)unit: meter
                sb.Append(datas[15] + ","); //Run time(Decimal)unit: second
                sb.Append(datas[16] + ","); //Base station information(binary|binary|hex|hex)           (8)

                Int32 io = Int32.Parse(datas[17], System.Globalization.NumberStyles.HexNumber);

                for (int index = 0; index < 16; index++) {
                    if (((io >> (index)) & 1) == 1) {
                        sb.Append("1,");
                    } else {
                        sb.Append("0,");
                    }
                }

                String[] analog = datas[18].Split('|');
                sb.Append(datas[0] + ",");
                sb.Append(datas[1] + ",");
                sb.Append(datas[2] + ",");
                sb.Append(datas[3] + ",");

                sb.Append("00000001");



                return;
            }



            //throw new Exception("Protocol not recognized");
        }
        public String Id {
            get;
            set;
        }
        public String Unit {
            get;
            set;
        }
        public String TimeStamp {
            get;
            set;
        }
        public String Latitude {
            get;
            set;
        }
        public String Longitude {
            get;
            set;
        }

        public String Speed {
            get;
            set;
        }
        public String Orientation {
            get;
            set;
        }
        public String Mileage {
            get;
            set;
        }
        public String Data {
            get;
            set;
        }
        public String LastTime {
            get;
            set;
        }
    }
}
