using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Globalization;

namespace TqatProModel.Parser {
    public class Cryptography {
        public static string md5(string input) {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] aInputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] aHash = md5.ComputeHash(aInputBytes);

            StringBuilder sbOutput = new StringBuilder();
            for (int i = 0; i < aHash.Length; i++) {
                sbOutput.Append(aHash[i].ToString("X2"));
            }

            return sbOutput.ToString();
        }
    }

    public class UnixTime {
        public static DateTime toDateTime(double unixTimeStamp) {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp);
            return dateTime.ToLocalTime();
        }

        public static double toUnixTimestamp(DateTime dateTime) {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }
    }

    public class Direction {
        public static string degreesToCardinal(double degrees) {
            string[] caridnals = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
            return caridnals[(int)Math.Round(((double)degrees % 360) / 45)];
        }

        public static string degreesToCardinalDetailed(double degrees) {
            degrees *= 10;

            string[] caridnals = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
            return caridnals[(int)Math.Round(((double)degrees % 3600) / 225)];
        }

    }

    public class Bits {
        public static uint getBit(uint value, int index) {
            return (value >> index) & 1;
        }

        public static uint setBit(uint value, int index, bool bit) {
            uint flag = (uint)(bit ? 1 : 0);
            uint constBit = 1;
            if (bit) {
                value |= (constBit << index);
            } else {
                value &= ~(constBit << index);
            }

            return value;
        }
    }

    public class SubStandard {
        public static DateTime dateTime(string dateTime) {
            DateTime parsedDateTime;

            if (!String.IsNullOrEmpty(dateTime)) {
                int dateTimeLength = dateTime.Length;

                if (dateTimeLength == 10) {
                    parsedDateTime = DateTime.ParseExact(dateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                } else if (dateTimeLength == 8) {
                    parsedDateTime = DateTime.ParseExact(dateTime, "d/M/yyyy", CultureInfo.InvariantCulture);
                } else if (dateTimeLength == 9) {
                    if (dateTime.IndexOf('/') == 1) {
                        parsedDateTime = DateTime.ParseExact(dateTime, "d/MM/yyyy", CultureInfo.InvariantCulture);
                    } else {//if (dateTime.IndexOf('/') == 2) {
                        parsedDateTime = DateTime.ParseExact(dateTime, "dd/M/yyyy", CultureInfo.InvariantCulture);
                    }

                } else if (dateTimeLength == 18) {
                    if (dateTime.IndexOf('/') == 1) {
                        parsedDateTime = DateTime.ParseExact(dateTime, "d/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    } else { //if (dateTime.IndexOf('/') == 2) {
                        parsedDateTime = DateTime.ParseExact(dateTime, "dd/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                } else { //(dateTimeLength == 19) {
                    parsedDateTime = DateTime.ParseExact(dateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
            } else {
                parsedDateTime = new DateTime(2000, 01, 01);
            }

            return parsedDateTime;
        }
    }
}
