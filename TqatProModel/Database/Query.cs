﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Globalization;

using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TqatProModel;
using TqatProModel.Database;
using TqatProModel.Devices.Meitrack;
using TqatProModel.Parser;
using System.Collections.Concurrent;
using System.Net;
using System.IO;
using System.Diagnostics;
using AtsGps.Ats;

namespace TqatProModel.Database {

    public class Query {

        MySqlConnection mysqlConnection = null;
        Database database;


        public Query (Database database) {
            if (database == null) {
                throw new QueryException(1, "Database is null.");
            }

            this.database = database;

            mysqlConnection = new MySqlConnection(this.database.getConnectionString());
        }


    
        public void checkConnection () {
            MySqlConnection mysqlConnection = new MySqlConnection(database.getConnectionString());

            try {
                mysqlConnection.Open();
                mysqlConnection.Close();
            } catch (Exception exception) {
                throw exception;
            }
        }

        #region Socket
        public Tracker getTracker (String imei) {
            try {
                mysqlConnection.Open();

                string sql =
                    "SELECT * " +
                    "FROM dbt_tracking_master.trks " +
                    "WHERE dbt_tracking_master.trks.tunit = @imei;";

                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);
                mySqlCommand.Parameters.AddWithValue("@imei", imei);

                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (!mySqlDataReader.HasRows) {
                    mySqlDataReader.Dispose();
                    throw new QueryException(1, "Tracker " + imei + " is not registered.");
                } else {
                    mySqlDataReader.Read();
                    Tracker tracker = new Tracker();
                    tracker.CompanyDatabaseName = (string)mySqlDataReader["tcmp"];
                    tracker.DatabaseHost = int.Parse((string)mySqlDataReader["tdbhost"]);
                    tracker.DatabaseName = (string)mySqlDataReader["tdbs"];

                    String dateTime = (string)mySqlDataReader["tcreatedate"];
                    tracker.DateTimeCreated = SubStandard.dateTime(dateTime);
                    dateTime = String.Empty;

                    dateTime = (string)mySqlDataReader["ttrackerexpiry"];
                    tracker.DateTimeExpired = SubStandard.dateTime(dateTime);
                    dateTime = String.Empty;

                    tracker.TrackerImei = (string)mySqlDataReader["tunit"];
                    tracker.DevicePassword = (string)mySqlDataReader["tunitpassword"];
                    tracker.DeviceType = int.Parse((string)mySqlDataReader["ttype"]);
                    tracker.DriverName = (string)mySqlDataReader["tdrivername"];
                    tracker.Emails = (string)mySqlDataReader["temails"];
                    tracker.HttpHost = int.Parse((string)mySqlDataReader["thttphost"]);
                    tracker.Id = (int)mySqlDataReader["tid"];
                    tracker.IdlingTime = int.Parse((string)mySqlDataReader["tidlingtime"]);
                    tracker.ImageNumber = int.Parse((string)mySqlDataReader["timg"]);
                    tracker.Inputs = (string)mySqlDataReader["tinputs"];
                    tracker.MileageInitial = int.Parse((string)mySqlDataReader["tmileageInit"]);
                    tracker.MileageLimit = int.Parse((string)mySqlDataReader["tmileagelimit"]);
                    tracker.MobileDataProvider = int.Parse((string)mySqlDataReader["tprovider"]);
                    tracker.Note = (string)mySqlDataReader["tnote"];
                    tracker.OwnerName = (string)mySqlDataReader["townername"];
                    tracker.SimImei = (string)mySqlDataReader["tsimsr"];
                    tracker.SimNumber = (string)mySqlDataReader["tsimno"];

                    tracker.SpeedLimit = int.Parse((string)mySqlDataReader["tSpeedLimit"]);
                    tracker.VehicleModel = (string)mySqlDataReader["tvehiclemodel"];
                    tracker.VehicleRegistration = (string)mySqlDataReader["tvehiclereg"];

                    mySqlDataReader.Dispose();

                    return tracker;
                }
            } catch (Exception exception) {
                throw exception;
            } finally {
                mysqlConnection.Close();
            }
        }
        public void insertTrackerData (Tracker tracker, Gm gm) {
            try {
                mysqlConnection.Open();

                string sql =
                    "INSERT INTO " +
                    "trk_" + tracker.DatabaseName + ".gps_" + tracker.DatabaseName +
                    "(gm_time,gm_lat,gm_lng,gm_speed,gm_ori,gm_mileage,gm_data,gm_lasttime) " +
                    "VALUES(@TimeStamp,@Latitude,@Longitude,@Speed,@Orientation,@Mileage,@Data,@LastTime)";

                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);
                mySqlCommand.Parameters.AddWithValue("@TimeStamp", gm.TimeStamp);
                mySqlCommand.Parameters.AddWithValue("@Latitude", gm.Latitude);
                mySqlCommand.Parameters.AddWithValue("@Longitude", gm.Longitude);
                mySqlCommand.Parameters.AddWithValue("@Speed", gm.Speed);
                mySqlCommand.Parameters.AddWithValue("@Orientation", gm.Orientation);
                mySqlCommand.Parameters.AddWithValue("@Mileage", gm.Mileage);
                mySqlCommand.Parameters.AddWithValue("@Data", gm.Data);
                mySqlCommand.Parameters.AddWithValue("@LastTime", gm.LastTime);

                if (mySqlCommand.ExecuteNonQuery() != 1) {
                    throw new Exception("Error on Insert..");
                }

            } catch (MySqlException mySqlException) {
                throw new QueryException(1, mySqlException.Message);
            } catch (QueryException queryException) {
                throw queryException;
            } catch (Exception exception) {
                throw new QueryException(1, exception.Message);
            } finally {
                mysqlConnection.Close();
            }
        }

        #endregion
        #region Tracking and Reporting
        public void getCompany (Company company) {
            try {
                mysqlConnection.Open();

                string sql =
                    "SELECT * " +
                    "FROM dbt_tracking_master.cmps " +
                    "WHERE dbt_tracking_master.cmps.cmpname = @sCompanyName;";

                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);
                mySqlCommand.Parameters.AddWithValue("@sCompanyName", company.Username);

                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();



                if (!mySqlDataReader.HasRows) {
                    mySqlDataReader.Dispose();
                    throw new QueryException(1, "Company does not exist.");
                } else {
                    mySqlDataReader.Read();
                    company.Id = mySqlDataReader.GetInt32("cmpid");
                    company.Username = mySqlDataReader.GetString("cmpname");
                    company.DisplayName = mySqlDataReader.GetString("cmpdisplayname");
                    company.Host = int.Parse(mySqlDataReader.GetString("cmphost"));
                    company.Email = mySqlDataReader.GetString("cmpemail");
                    company.PhoneNo = mySqlDataReader.GetString("cmpphoneno");
                    company.MobileNo = mySqlDataReader.GetString("cmpmobileno");
                    company.Address = mySqlDataReader.GetString("cmpaddress");
                    company.IsActive = (mySqlDataReader.GetString("cmpactive") == "1") ? true : false;
                    company.DateTimeCreated = SubStandard.dateTime(mySqlDataReader.GetString("cmpcreatedate"));
                    company.DateTimeExpired = SubStandard.dateTime(mySqlDataReader.GetString("cmpexpiredate"));
                    company.DatabaseName = mySqlDataReader.GetString("cmpdbname");

                    mySqlDataReader.Dispose();
                }
            } catch (MySqlException mySqlException) {
                throw new QueryException(1, mySqlException.Message);
            } catch (QueryException queryException) {
                throw queryException;
            } catch (Exception exception) {
                throw new QueryException(1, exception.Message);
            } finally {
                mysqlConnection.Close();
            }

        }

        public List<Company> getCompanies () {
            List<Company> companies = new List<Company>();

            try {
                mysqlConnection.Open();

                string sql =
                    "SELECT * " +
                    "FROM dbt_tracking_master.cmps;";

                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);

                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();



                if (!mySqlDataReader.HasRows) {
                    mySqlDataReader.Dispose();
                    throw new QueryException(1, "Company list is empty.");
                } else {
                    while (mySqlDataReader.Read()) {
                        Company company = new Company();
                        company.Id = mySqlDataReader.GetInt32("cmpid");
                        company.Username = mySqlDataReader.GetString("cmpname");
                        company.DisplayName = mySqlDataReader.GetString("cmpdisplayname");
                        company.Host = int.Parse(mySqlDataReader.GetString("cmphost"));
                        company.Email = mySqlDataReader.GetString("cmpemail");
                        company.PhoneNo = mySqlDataReader.GetString("cmpphoneno");
                        company.MobileNo = mySqlDataReader.GetString("cmpmobileno");
                        company.Address = mySqlDataReader.GetString("cmpaddress");
                        company.IsActive = (mySqlDataReader.GetString("cmpactive") == "1") ? true : false;
                        company.DateTimeCreated = SubStandard.dateTime(mySqlDataReader.GetString("cmpcreatedate"));
                        company.DateTimeExpired = SubStandard.dateTime(mySqlDataReader.GetString("cmpexpiredate"));
                        company.DatabaseName = mySqlDataReader.GetString("cmpdbname");

                        companies.Add(company);
                    }

                    mySqlDataReader.Dispose();
                }
            } catch (MySqlException mySqlException) {
                throw new QueryException(1, mySqlException.Message);
            } catch (QueryException queryException) {
                throw queryException;
            } catch (Exception exception) {
                throw new QueryException(1, exception.Message);
            } finally {
                mysqlConnection.Close();
            }
            return companies;
        }

        public void fillGeofences (Company company) {
            ConcurrentQueue<Geofence> geofences = new ConcurrentQueue<Geofence>();

            try {
                mysqlConnection = new MySqlConnection(database.getConnectionString());
                mysqlConnection.Open();

                string sql =
                     "SELECT * " +
                     "FROM cmp_" + company.DatabaseName + ".gf";

                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (!mySqlDataReader.HasRows) {
                    mySqlDataReader.Dispose();
                } else {
                    while (mySqlDataReader.Read()) {
                        Geofence geofence = new Geofence();
                        geofence.Id = mySqlDataReader.GetInt32("gf_id");
                        geofence.Name = mySqlDataReader.GetString("gf_name");
                        geofence.Tracks = mySqlDataReader.GetString("gf_trks");

                        string geofenceData = (string)mySqlDataReader["gf_data"];
                        geofenceData = geofenceData.Replace("),( ", "|");
                        geofenceData = geofenceData.Replace(")", string.Empty);
                        geofenceData = geofenceData.Replace("(", string.Empty);
                        geofenceData = geofenceData.Replace(" ", string.Empty);


                        List<string> points = geofenceData.Split('|').ToList();
                        foreach (string point in points) {
                            string[] coordinates = point.Split(',');
                            double latitude = double.Parse(coordinates[0]);
                            double longitude = double.Parse(coordinates[1]);
                            Coordinate location = new Coordinate(latitude, longitude);
                            geofence.addPoint(location);
                        }
                        geofences.Enqueue(geofence);
                    }
                    company.Geofences = geofences;
                    mySqlDataReader.Dispose();
                }
            } catch (MySqlException mySqlException) {
                throw new QueryException(1, mySqlException.Message);
            } catch (QueryException queryException) {
                throw queryException;
            } catch (Exception exception) {
                throw new QueryException(1, exception.Message);
            } finally {
                mysqlConnection.Close();
            }
        }

        public void getUser (Company company, User user) {
            try {
                mysqlConnection = new MySqlConnection(database.getConnectionString());

                mysqlConnection.Open();

                string sql =
                    "SELECT * " +
                    "FROM cmp_" + company.DatabaseName + ".usrs " +
                    "WHERE " +
                    "cmp_" + company.DatabaseName + ".usrs.uname = @sUsername AND " +
                    "cmp_" + company.DatabaseName + ".usrs.upass = @sPassword;";

                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);
                mySqlCommand.Parameters.AddWithValue("@sUsername", user.Username);
                mySqlCommand.Parameters.AddWithValue("@sPassword", user.Password);

                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (!mySqlDataReader.HasRows) {
                    mySqlDataReader.Dispose();
                    throw new QueryException(1, "Username or Password does not exist.");
                } else {
                    mySqlDataReader.Read();

                    user.Id = mySqlDataReader.GetInt32("uid");
                    user.Username = mySqlDataReader.GetString("uname");
                    user.Password = mySqlDataReader.GetString("upass");
                    user.Email = mySqlDataReader.GetString("uemail");
                    user.Main = mySqlDataReader.GetString("umain");
                    user.AccessLevel = int.Parse(mySqlDataReader.GetString("upriv"));
                    user.Timezone = mySqlDataReader.GetString("utimezone");
                    user.IsActive = mySqlDataReader.GetString("uactive").Equals("1");
                    user.DatabaseName = mySqlDataReader.GetString("udbs");

                    if (!String.IsNullOrEmpty(mySqlDataReader.GetString("uexpiredate"))) {
                        string dateTime = (mySqlDataReader.GetString("uexpiredate"));
                        if (!String.IsNullOrEmpty(dateTime)) {
                            DateTime parsedDate = SubStandard.dateTime(dateTime);
                            user.DateTimeExpired = parsedDate;
                        }
                    } else {
                        user.DateTimeExpired = new DateTime(2050, 01, 01);
                    }

                    if (!String.IsNullOrEmpty(mySqlDataReader.GetString("ucreatedate"))) {
                        string dateTime = mySqlDataReader.GetString("ucreatedate");
                        if (!String.IsNullOrEmpty(dateTime)) {
                            DateTime parsedDate = SubStandard.dateTime(dateTime);
                            user.DateTimeCreated = parsedDate;
                        }
                    } else {
                        user.DateTimeCreated = new DateTime(2010, 01, 01);
                    }
                    mySqlDataReader.Dispose();
                }

            } catch (MySqlException mySqlException) {
                throw new QueryException(1, mySqlException.Message);
            } catch (QueryException queryException) {
                throw queryException;
            } catch (Exception exception) {
                throw new QueryException(1, exception.Message);
            } finally {
                mysqlConnection.Close();
            }
        }

        //public void fillPois(Company company, User user) {
        //    ConcurrentQueue<Poi> pois = new ConcurrentQueue<Poi>();
        //    try {
        //        mysqlConnection = new MySqlConnection(database.getConnectionString());

        //        mysqlConnection.Open();

        //        string sql =
        //            "SELECT * " +
        //            "FROM cmp_" + company.DatabaseName + ".poi_" + user.DatabaseName + ";";

        //        MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);

        //        MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

        //        if (!mySqlDataReader.HasRows) {
        //            mySqlDataReader.Dispose();
        //        } else {
        //            while (mySqlDataReader.Read()) {
        //                Poi poi = new Poi();
        //                poi.Id = mySqlDataReader.GetInt32("poi_id");
        //                poi.Name = mySqlDataReader.GetString("poi_name");
        //                poi.Description = mySqlDataReader.GetString("poi_desc");
        //                poi.Image = mySqlDataReader.GetString("poi_img");


        //                string latitude = mySqlDataReader.GetString("poi_lat");
        //                string longitude = mySqlDataReader.GetString("poi_lon");

        //                if(String.IsNullOrEmpty(latitude) || String.IsNullOrEmpty(longitude)) {
        //                    poi.Coordinate = new Coordinate(0.0f, 0.0f);
        //                } else {
        //                    poi.Coordinate = new Coordinate(double.Parse(latitude), double.Parse(longitude));
        //                }

        //                pois.Enqueue(poi);
        //            }

        //            user.Pois = pois;
        //            mySqlDataReader.Dispose();
        //        }
        //    } catch (MySqlException mySqlException) {
        //        throw new QueryException(1, mySqlException.Message);
        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (Exception exception) {
        //        throw new QueryException(1, exception.Message);
        //    } finally {
        //        mysqlConnection.Close();
        //    }
        //}

        public void fillUsers (Company company, User user) {
            ConcurrentQueue<User> users = new ConcurrentQueue<User>();
            try {
                mysqlConnection = new MySqlConnection(database.getConnectionString());

                mysqlConnection.Open();

                string sql = "";

                if (user.AccessLevel == 1 || user.AccessLevel == 2) {
                    sql =
                    "SELECT * " +
                    "FROM cmp_" + company.DatabaseName + ".usrs " +
                    "WHERE cmp_" + company.DatabaseName + ".usrs.upriv >= " + user.AccessLevel.ToString() + ";";
                } else {
                    sql =
                    "SELECT * " +
                    "FROM cmp_" + company.DatabaseName + ".usrs " +
                    "WHERE cmp_" + company.DatabaseName + ".usrs.upriv = " + user.AccessLevel.ToString() + " and " +
                    "cmp_" + company.DatabaseName + ".usrs.uname = '" + user.Username + "';";
                }

                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);

                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (!mySqlDataReader.HasRows) {
                    mySqlDataReader.Dispose();
                    throw new QueryException(1, "Users table is empty.");
                } else {
                    while (mySqlDataReader.Read()) {
                        User userSubordinate = new User();
                        userSubordinate.Id = mySqlDataReader.GetInt32("uid");
                        userSubordinate.Username = mySqlDataReader.GetString("uname");
                        userSubordinate.Password = mySqlDataReader.GetString("upass");
                        userSubordinate.Email = mySqlDataReader.GetString("uemail");
                        userSubordinate.Main = mySqlDataReader.GetString("umain");
                        userSubordinate.AccessLevel = int.Parse(mySqlDataReader.GetString("upriv"));
                        userSubordinate.Timezone = mySqlDataReader.GetString("utimezone");
                        userSubordinate.IsActive = mySqlDataReader.GetString("uactive").Equals("1");
                        userSubordinate.DatabaseName = mySqlDataReader.GetString("udbs");

                        if (!String.IsNullOrEmpty(mySqlDataReader.GetString("uexpiredate"))) {
                            string dateTime = (mySqlDataReader.GetString("uexpiredate"));
                            if (!String.IsNullOrEmpty(dateTime)) {
                                DateTime parsedDate = SubStandard.dateTime(dateTime);
                                userSubordinate.DateTimeExpired = parsedDate;
                            }
                        } else {
                            userSubordinate.DateTimeExpired = new DateTime(2050, 01, 01);
                        }

                        if (!String.IsNullOrEmpty(mySqlDataReader.GetString("ucreatedate"))) {
                            string dateTime = mySqlDataReader.GetString("ucreatedate");
                            if (!String.IsNullOrEmpty(dateTime)) {
                                DateTime parsedDate = SubStandard.dateTime(dateTime);
                                userSubordinate.DateTimeCreated = parsedDate;
                            }
                        } else {
                            userSubordinate.DateTimeCreated = new DateTime(2010, 01, 01);
                        }
                        users.Enqueue(userSubordinate);
                    }
                    mySqlDataReader.Dispose();
                }

            } catch (MySqlException mySqlException) {
                throw new QueryException(1, mySqlException.Message);
            } catch (QueryException queryException) {
                throw queryException;
            } catch (Exception exception) {
                throw new QueryException(1, exception.Message);
            } finally {
                mysqlConnection.Close();
            }
            company.Users = users;
        }
        public void fillCollection (Company company) {
            foreach (User user in company.Users) {
                ConcurrentQueue<Collection> collections = new ConcurrentQueue<Collection>();
                try {
                    mysqlConnection = new MySqlConnection(database.getConnectionString());

                    mysqlConnection.Open();

                    string sql =
                        "SELECT * " +
                        "FROM cmp_" + company.DatabaseName + ".coll_" + user.DatabaseName + ";";

                    MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);

                    MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                    if (!mySqlDataReader.HasRows) {
                        Collection collection = new Collection();
                        collection.Id = 0;
                        collection.Name = "All";
                        collection.Description = "All";
                        collections.Enqueue(collection);
                        user.Collections = collections;
                        mySqlDataReader.Dispose();
                    } else {
                        Collection collection = new Collection();
                        collection.Id = 0;
                        collection.Name = "All";
                        collection.Description = "All";
                        collections.Enqueue(collection);

                        while (mySqlDataReader.Read()) {
                            Collection collectionItem = new Collection();
                            collectionItem.Id = mySqlDataReader.GetInt32("collid");
                            collectionItem.Name = mySqlDataReader.GetString("collname");
                            collectionItem.Description = mySqlDataReader.GetString("colldesc");
                            collections.Enqueue(collectionItem);
                        }

                        user.Collections = collections;
                        mySqlDataReader.Dispose();
                    }
                } catch (MySqlException mySqlException) {
                    throw new QueryException(1, mySqlException.Message);
                } catch (QueryException queryException) {
                    throw queryException;
                } catch (Exception exception) {
                    throw new QueryException(1, exception.Message);
                } finally {
                    mysqlConnection.Close();
                }
            }
        }
        public void fillPois (Company company) {
            foreach (User user in company.Users) {
                ConcurrentQueue<Poi> pois = new ConcurrentQueue<Poi>();
                try {
                    mysqlConnection = new MySqlConnection(database.getConnectionString());

                    mysqlConnection.Open();

                    string sql =
                        "SELECT * " +
                        "FROM cmp_" + company.DatabaseName + ".poi_" + user.DatabaseName + ";";

                    MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);

                    MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                    if (!mySqlDataReader.HasRows) {
                        mySqlDataReader.Dispose();
                    } else {
                        while (mySqlDataReader.Read()) {
                            Poi poi = new Poi();
                            poi.Id = mySqlDataReader.GetInt32("poi_id");
                            poi.Name = mySqlDataReader.GetString("poi_name");
                            poi.Description = mySqlDataReader.GetString("poi_desc");
                            poi.Image = mySqlDataReader.GetString("poi_img");

                            string poiLatitude = mySqlDataReader.GetString("poi_lat");
                            string potLongitude = mySqlDataReader.GetString("poi_lon");


                            if (String.IsNullOrEmpty(poiLatitude) || String.IsNullOrEmpty(potLongitude)) {
                                poi.Coordinate = new Coordinate(0.0f, 0.0f);
                            } else {
                                poi.Coordinate = new Coordinate(double.Parse(poiLatitude), double.Parse(potLongitude));
                            }

                            //if (!String.IsNullOrEmpty(poiLatitude) || !String.IsNullOrEmpty(potLongitude)) {
                            //    poi.Coordinate = new Coordinate(
                            //            double.Parse(poiLatitude),
                            //            double.Parse(potLongitude)
                            //            );
                            //}


                            pois.Enqueue(poi);
                        }

                        user.Pois = pois;
                        mySqlDataReader.Dispose();
                    }
                } catch (MySqlException mySqlException) {
                    throw new QueryException(1, mySqlException.Message);
                } catch (QueryException queryException) {
                    throw queryException;
                } catch (Exception exception) {
                    throw new QueryException(1, exception.Message);
                } finally {
                    mysqlConnection.Close();
                }
            }
        }
        public void fillTrackers (Company company) {
            ConcurrentQueue<Tracker> trackers = new ConcurrentQueue<Tracker>();

            try {
                mysqlConnection.Open();

                string sql =
                    "SELECT * " +
                    "FROM dbt_tracking_master.trks " +
                    "WHERE dbt_tracking_master.trks.tcmp = @CompanyDatabaseName";

                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);
                mySqlCommand.Parameters.AddWithValue("@CompanyDatabaseName", company.DatabaseName);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();


                if (!mySqlDataReader.HasRows) {
                    //throw new QueryException(1, "Tracker's Collection is empty.");
                    mySqlDataReader.Dispose();
                    mysqlConnection.Close();
                } else {


                    string dateTime;

                    while (mySqlDataReader.Read()) {
                        string[] trackerUser = mySqlDataReader.GetString("tusers").Split(',');
                        ConcurrentQueue<User> trackerUsers = new ConcurrentQueue<User>();
                        foreach (User user in company.Users) {
                            for (int index = 0; index < trackerUser.ToList().Count; index++) {
                                if (Int32.Parse(trackerUser[index]) == user.Id) {
                                    trackerUsers.Enqueue(user);
                                }
                            }
                        }

                        Tracker tracker = new Tracker();

                        dynamic dynamicCollection = JsonConvert.DeserializeObject<dynamic>(mySqlDataReader.GetString("tcollections"));
                        JArray arrayCollection = (JArray)dynamicCollection;
                        ConcurrentQueue<Collection> collections = new ConcurrentQueue<Collection>();
                        Collection collectionItem = new Collection();
                        collectionItem.Id = 0;
                        collectionItem.Name = "All";
                        collectionItem.Description = "All";
                        collections.Enqueue(collectionItem);

                        foreach (User user in company.Users) {
                            if (user.Collections == null) {
                                continue;
                            }

                            foreach (Collection collection in user.Collections) {

                                for (int index = 0; index < arrayCollection.Count; index++) {
                                    if (dynamicCollection[index].value == collection.Id) {
                                        collections.Enqueue(collection);
                                    }
                                }
                            }
                        }
                        tracker.Collections = collections;


                        //tracker.Collections = mySqlDataReader.GetString("tcollections");
                        tracker.CompanyDatabaseName = (string)mySqlDataReader["tcmp"];
                        tracker.DatabaseHost = int.Parse((string)mySqlDataReader["tdbhost"]);
                        tracker.DatabaseName = (string)mySqlDataReader["tdbs"];

                        dateTime = (string)mySqlDataReader["tcreatedate"];
                        tracker.DateTimeCreated = SubStandard.dateTime(dateTime);
                        dateTime = String.Empty;

                        dateTime = (string)mySqlDataReader["ttrackerexpiry"];
                        tracker.DateTimeExpired = SubStandard.dateTime(dateTime);
                        dateTime = String.Empty;

                        tracker.TrackerImei = (string)mySqlDataReader["tunit"];
                        tracker.DevicePassword = (string)mySqlDataReader["tunitpassword"];
                        tracker.DeviceType = int.Parse((string)mySqlDataReader["ttype"]);
                        tracker.DriverName = (string)mySqlDataReader["tdrivername"];
                        tracker.Emails = (string)mySqlDataReader["temails"];
                        tracker.HttpHost = int.Parse((string)mySqlDataReader["thttphost"]);
                        tracker.Id = (int)mySqlDataReader["tid"];
                        tracker.IdlingTime = int.Parse((string)mySqlDataReader["tidlingtime"]);
                        tracker.ImageNumber = int.Parse((string)mySqlDataReader["timg"]);
                        tracker.Inputs = (string)mySqlDataReader["tinputs"];
                        tracker.MileageInitial = int.Parse((string)mySqlDataReader["tmileageInit"]);
                        tracker.MileageLimit = int.Parse((string)mySqlDataReader["tmileagelimit"]);
                        tracker.MobileDataProvider = int.Parse((string)mySqlDataReader["tprovider"]);
                        tracker.Note = (string)mySqlDataReader["tnote"];
                        tracker.OwnerName = (string)mySqlDataReader["townername"];
                        tracker.SimImei = (string)mySqlDataReader["tsimsr"];
                        tracker.SimNumber = (string)mySqlDataReader["tsimno"];
                        tracker.Users = trackerUsers;
                        tracker.SpeedLimit = int.Parse((string)mySqlDataReader["tSpeedLimit"]);
                        tracker.VehicleModel = (string)mySqlDataReader["tvehiclemodel"];
                        tracker.VehicleRegistration = (string)mySqlDataReader["tvehiclereg"];

                        trackers.Enqueue(tracker);
                    }
                    company.Trackers = trackers;
                    mySqlDataReader.Dispose();
                }
            } catch (QueryException queryException) {
                throw queryException;
            } catch (MySqlException mySqlException) {
                throw new QueryException(1, mySqlException.Message);
            } catch (Exception exception) {
                throw new QueryException(1, exception.Message);
            } finally {
                mysqlConnection.Close();
            }
        }


        //public DataTable getAllCompanies() {
        //    DataTable dataTable = new DataTable();
        //    try {
        //        mysqlConnection.Open();

        //        string sql =
        //        "SELECT * " +
        //        "FROM dbt_tracking_master.cmps;";

        //        MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);

        //        MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

        //        if (!mySqlDataReader.HasRows) {
        //            throw new QueryException(1, "Companies's Collection is empty.");
        //        } else {
        //            dataTable.Load(mySqlDataReader);
        //        }

        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (MySqlException mySqlException) {
        //        throw new QueryException(1, mySqlException.Message);
        //    } catch (Exception exception) {
        //        throw new QueryException(1, exception.Message);
        //    } finally {
        //        mysqlConnection.Close();
        //    }
        //    return dataTable;
        //}

        //public TrackerData getTrackerLatestData (Company company, Tracker tracker) {
        //    TrackerData trackerData = new TrackerData();
        //    trackerData.Tracker = tracker;
        //    try {
        //        mysqlConnection.Open();

        //        string sql =
        //             "SELECT * " +
        //             "FROM trk_" + tracker.DatabaseName + ".gps_" + tracker.DatabaseName + " " +
        //             "ORDER BY " +
        //             "trk_" + tracker.DatabaseName + ".gps_" + tracker.DatabaseName + ".gm_time " +
        //             "DESC limit 1;";
        //        //"SELECT * " +
        //        // "FROM trk_" + tracker.DatabaseName + ".gps_" + tracker.DatabaseName + " " +
        //        // "ORDER BY " +
        //        // "trk_" + tracker.DatabaseName + ".gps_" + tracker.DatabaseName + ".gm_id " +
        //        // "DESC limit 1;";

        //        MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);
        //        MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

        //        if (!mySqlDataReader.HasRows) {
        //            trackerData.IsDataEmpty = true;
        //            return trackerData;
        //        } else {
        //            mySqlDataReader.Read();
        //            trackerData.IsDataEmpty = false;
        //            trackerData.Id = mySqlDataReader.GetInt32("gm_id");
        //            double dateTime = double.Parse(mySqlDataReader.GetString("gm_time"));
        //            trackerData.DateTime = Parser.UnixTime.toDateTime(dateTime);
        //            double latitude = double.Parse(mySqlDataReader.GetString("gm_lat"));
        //            double longitude = double.Parse(mySqlDataReader.GetString("gm_lng"));
        //            trackerData.Coordinate = new Coordinate(latitude, longitude);

        //            trackerData.Speed = int.Parse(mySqlDataReader.GetString("gm_speed"));
        //            trackerData.Degrees = int.Parse(mySqlDataReader.GetString("gm_ori"));
        //            trackerData.Direction = Direction.degreesToCardinalDetailed(double.Parse(mySqlDataReader.GetString("gm_ori")));
        //            //trackerData.Mileage = double.Parse(mySqlDataReader.GetString("gm_mileage"));

        //            double deviceMileage = double.Parse(mySqlDataReader.GetString("gm_mileage"));
        //            double carMileage = (tracker.MileageInitial);
        //            trackerData.Mileage = deviceMileage + carMileage;

        //            //1,			            //                                                          (0)
        //            //35,			            //Event code(Decimal)
        //            //11,			            //Number of satellites(Decimal)
        //            //26,			            //GSM signal status(Decimal)
        //            //17160691, 		        //Mileage(Decimal)unit: meter
        //            //0.7, 			            //hpos accuracy(Decimal)
        //            //18, 			            //Altitude(Decimal)unit: meter
        //            //18661240, 		        //Run time(Decimal)unit: second
        //            //427|2|0078|283F, 	        //Base station information(binary|binary|hex|hex)           (8)
        //            //==============================================0200
        //            //0,0,0,0,0,0,0,0,          //Io port lowbyte (low bit start from left)                 (9)
        //            //0,1,0,0,0,0,0,0,          //Io port lowbyte (low bit start from left)                 (17)
        //            //==============================================
        //            //000B,0000,0000,0A6E,0434, //Analog input value                                        (25)
        //            //00000001 		            //System mark

        //            string gmData = (string)mySqlDataReader["gm_data"];
        //            string[] data = gmData.Split(',');
        //            trackerData.EventCode = (EventCode)int.Parse(data[1]);

        //            trackerData.GpsSatellites = int.Parse(data[2]);
        //            trackerData.GsmSignal = int.Parse(data[3]);
        //            trackerData.Altitude = int.Parse(data[6]);

        //            trackerData.ACC = (int.Parse(data[18]) == 1) ? true : false;
        //            trackerData.SOS = (int.Parse(data[17]) == 1) ? true : false;
        //            trackerData.OverSpeed = ((int)trackerData.Speed > tracker.SpeedLimit) ? true : false;
        //            //Geofence
        //            Coordinate coordinate = new Coordinate(latitude, longitude);

        //            foreach (Geofence geofence in company.Geofences) {
        //                if (Geofence.isPointInPolygon(geofence, coordinate)) {
        //                    trackerData.Geofence = geofence;
        //                }
        //            };

        //            double batteryStrength = (double)int.Parse(data[28], System.Globalization.NumberStyles.AllowHexSpecifier);
        //            batteryStrength = ((batteryStrength - 2114f) * (100f / 492f));//*100.0;
        //            batteryStrength = Math.Round(batteryStrength, 2);
        //            if (batteryStrength > 100) {
        //                batteryStrength = 100f;
        //            } else if (batteryStrength < 0) {
        //                batteryStrength = 0;
        //            }

        //            double batteryVoltage = (double)int.Parse(data[28], System.Globalization.NumberStyles.AllowHexSpecifier);
        //            batteryVoltage = (batteryVoltage * 3 * 2) / 1024;
        //            batteryVoltage = Math.Round(batteryVoltage, 2);
        //            double externalVoltage = (double)int.Parse(data[29], System.Globalization.NumberStyles.AllowHexSpecifier);
        //            externalVoltage = (externalVoltage * 3 * 16) / 1024;
        //            externalVoltage = Math.Round(externalVoltage, 2);

        //            trackerData.Battery = batteryStrength;
        //            trackerData.BatteryVoltage = batteryVoltage;
        //            trackerData.ExternalVoltage = externalVoltage;

        //            return trackerData;
        //        }

        //        //} catch (QueryException queryException) {
        //        //throw queryException;
        //        //} catch (MySqlException mySqlException) {
        //        //throw new QueryException(1, mySqlException.Message);
        //    } catch (Exception exception) {
        //        Debug.Write(exception);
        //        //throw new QueryException(1, exception.Message);
        //        return trackerData;
        //    } finally {
        //        mysqlConnection.Close();
        //    }

        //}

        public TrackerData getTrackerLatestData (Company company, Tracker tracker, Server server) {
            TrackerData trackerData = new TrackerData();
            trackerData.Tracker = tracker;

            try {
                WebRequest request = WebRequest.Create("http://" + server.Ip + "/connect/get_realtime.php?url=http://" + server.Ip + ":" + server.PortHttp + "/?id=" + tracker.TrackerImei);

                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);
                // Clean up the streams and the response.
                reader.Close();
                response.Close();
                //===========================================

                //===========================================
                responseFromServer = responseFromServer.Trim();
                if (responseFromServer == "1") {
                    trackerData.IsDataEmpty = true;
                    return trackerData;
                } else {
                    dynamic jsonData = (dynamic)JsonConvert.DeserializeObject<dynamic>(responseFromServer);

                    trackerData.IsDataEmpty = false;
                    trackerData.Tracker.TrackerImei = (string)jsonData.gm_unit;

                    double dateTime = double.Parse((string)jsonData.gm_time);

                    trackerData.DateTime = Parser.UnixTime.toDateTime(dateTime);
                    //DateTime dateTime1 = Parser.UnixTime.toDateTime(dateTime);
                    //TimeSpan timeSpan = dateTime1.Subtract(DateTime.Now);

                    //if (timeSpan.Hours > 6) {
                    //    trackerData.DateTime = dateTime1;
                    //} else {
                    //    trackerData.DateTime = DateTime.Now;
                    //}


                    double latitude = 0;
                    double.TryParse((string)jsonData.gm_lat, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out latitude);
                    double longitude = 0;
                    double.TryParse((string)jsonData.gm_lng, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out longitude);

                    trackerData.Coordinate = new Coordinate(latitude, longitude);

                    trackerData.Speed = int.Parse((string)jsonData.gm_speed);
                    trackerData.Degrees = int.Parse((string)jsonData.gm_ori);
                    trackerData.Direction = Direction.degreesToCardinalDetailed(double.Parse((string)jsonData.gm_ori));
                    //trackerData.Mileage = double.Parse((string)jsonData.gm_mileage);

                    double deviceMileage = 0;
                    double.TryParse((string)jsonData.gm_mileage, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out deviceMileage);

                    double carMileage = (tracker.MileageInitial);
                    trackerData.Mileage = deviceMileage + carMileage;

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

                    string gmData = (string)jsonData.gm_data;
                    string[] data = gmData.Split(',');
                    trackerData.EventCode = (TqatProModel.Devices.Meitrack.EventCode)int.Parse(data[1]);

                    trackerData.GpsSatellites = int.Parse(data[2]);
                    trackerData.GsmSignal = int.Parse(data[3]);
                    trackerData.Altitude = int.Parse(data[6]);

                    trackerData.ACC = (int.Parse(data[18]) == 1) ? true : false;
                    trackerData.SOS = (int.Parse(data[17]) == 1) ? true : false;
                    trackerData.OverSpeed = ((int)trackerData.Speed > tracker.SpeedLimit) ? true : false;
                    //Geofence
                    Coordinate coordinate = new Coordinate(latitude, longitude);

                    if (company.Geofences != null) {
                        foreach (Geofence geofence in company.Geofences) {
                            if (Geofence.isPointInPolygon(geofence, coordinate)) {
                                trackerData.Geofence = geofence;
                            }
                        };
                    }


                    double batteryVoltage = (double)int.Parse(data[28], System.Globalization.NumberStyles.AllowHexSpecifier);
                    batteryVoltage = (batteryVoltage * 3.3 * 2) / 4096;

                    double externalVoltage = (double)int.Parse(data[29], System.Globalization.NumberStyles.AllowHexSpecifier);
                    externalVoltage = (externalVoltage * 3.3 * 16) / 4096;

                    double batteryStrength = batteryVoltage - 3.5;
                    batteryStrength = (batteryStrength / 0.8) * 100;

                    if (batteryStrength < 0) {
                        batteryStrength = 0;
                    } else if (batteryStrength > 100) {
                        batteryStrength = 100;
                    }

                    trackerData.EPC = (externalVoltage > 3) ? false : true;

                    trackerData.Battery = Math.Round(batteryStrength, 2);
                    trackerData.BatteryVoltage = Math.Round(batteryVoltage, 2);
                    trackerData.ExternalVoltage = Math.Round(externalVoltage, 2);

                    return trackerData;
                }

            } catch (QueryException queryException) {
                throw queryException;
            } catch (MySqlException mySqlException) {
                throw new QueryException(1, mySqlException.Message);
            } catch (Exception exception) {
                Debug.Write(exception);
                mysqlConnection.Close();
                throw new QueryException(1, exception.Message);
            } finally {
                mysqlConnection.Close();
            }

        }
        #endregion

        public ConcurrentQueue<TrackerDatabaseSize> getDatabasesSize () {

            ConcurrentQueue<TrackerDatabaseSize> trackerDatabaseSizes = new ConcurrentQueue<TrackerDatabaseSize>();


            try {
                mysqlConnection = new MySqlConnection(database.getConnectionString());

                mysqlConnection.Open();

                string sql =
                    "SELECT table_schema \"databaseName\", " +
                    "sum( data_length + index_length ) / 1024 / 1024 \"databaseSize\", " +
                    "sum( data_free )/ 1024 / 1024 \"databaseFreeSpace\" " +

                    "FROM information_schema.TABLES " +
                    "WHERE table_schema LIKE \"trk_%\" OR table_schema LIKE \"dbt_%\" " +
                     "GROUP BY table_schema ;";

                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);

                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (!mySqlDataReader.HasRows) {
                    mySqlDataReader.Dispose();
                    throw new QueryException(1, "No database.");
                } else {
                    while (mySqlDataReader.Read()) {
                        TrackerDatabaseSize trackerDatabaseSize = new TrackerDatabaseSize();

                        trackerDatabaseSize.Name = mySqlDataReader.GetString("databaseName");
                        trackerDatabaseSize.DatabaseSize = mySqlDataReader.GetDouble("databaseSize");
                        trackerDatabaseSize.DatabaseFreeSpace = mySqlDataReader.GetDouble("databaseFreeSpace");

                        trackerDatabaseSizes.Enqueue(trackerDatabaseSize);
                    }
                }

            } catch (MySqlException mySqlException) {
                throw new QueryException(1, mySqlException.Message);
            } catch (QueryException queryException) {
                throw queryException;
            } catch (Exception exception) {
                throw new QueryException(1, exception.Message);
            } finally {
                mysqlConnection.Close();
            }
            return trackerDatabaseSizes;
        }

        public TrackerDatabaseSize getDatabaseSize (string databaseName) {

            TrackerDatabaseSize trackerDatabaseSize = new TrackerDatabaseSize();


            try {
                mysqlConnection = new MySqlConnection(database.getConnectionString());

                mysqlConnection.Open();

                string sql =
                    "SELECT table_schema \"databaseName\", " +
                    "sum( data_length + index_length ) / 1024 / 1024 \"databaseSize\", " +
                    "sum( data_free )/ 1024 / 1024 \"databaseFreeSpace\" " +

                    "FROM information_schema.TABLES " +
                    "WHERE table_schema LIKE @databaseName;";

                MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);
                mySqlCommand.Parameters.AddWithValue("@databaseName", databaseName);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (!mySqlDataReader.HasRows) {
                    mySqlDataReader.Dispose();
                    throw new QueryException(1, "No database.");
                } else {
                    mySqlDataReader.Read();
                    trackerDatabaseSize.Name = mySqlDataReader.GetString("databaseName");
                    trackerDatabaseSize.DatabaseSize = mySqlDataReader.GetDouble("databaseSize");
                    trackerDatabaseSize.DatabaseFreeSpace = mySqlDataReader.GetDouble("databaseFreeSpace");
                }

            } catch (MySqlException mySqlException) {
                throw new QueryException(1, mySqlException.Message);
            } catch (QueryException queryException) {
                throw queryException;
            } catch (Exception exception) {
                throw new QueryException(1, exception.Message);
            } finally {
                mysqlConnection.Close();
            }

            return trackerDatabaseSize;
        }



        //        MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

        //        if (!mySqlDataReader.HasRows) {
        //            throw new QueryException(1, "Tracker's Collection is empty.");
        //        } else {
        //            dataTable.Columns.Add("id", typeof(int));

        //            dataTable.Columns.Add("vehicleRegistration", typeof(string));
        //            dataTable.Columns.Add("vehicleModel", typeof(string));
        //            dataTable.Columns.Add("ownerName", typeof(string));
        //            dataTable.Columns.Add("driverName", typeof(string));

        //            dataTable.Columns.Add("simImei", typeof(string));
        //            dataTable.Columns.Add("simNumber", typeof(string));
        //            dataTable.Columns.Add("mobileDataProvider", typeof(int));

        //            dataTable.Columns.Add("deviceImei", typeof(string));
        //            dataTable.Columns.Add("devicePassword", typeof(string));
        //            dataTable.Columns.Add("deviceType", typeof(int));

        //            dataTable.Columns.Add("emails", typeof(string));
        //            dataTable.Columns.Add("users", typeof(string));

        //            dataTable.Columns.Add("mileageInitial", typeof(int));
        //            dataTable.Columns.Add("mileageLimit", typeof(int));
        //            dataTable.Columns.Add("speedLimit", typeof(int));

        //            dataTable.Columns.Add("idlingTime", typeof(int));
        //            dataTable.Columns.Add("inputs", typeof(string));
        //            dataTable.Columns.Add("imageNumber", typeof(int));
        //            dataTable.Columns.Add("note", typeof(string));

        //            dataTable.Columns.Add("collections", typeof(string));
        //            dataTable.Columns.Add("companyDatabaseName", typeof(string));
        //            dataTable.Columns.Add("databaseHost", typeof(int));
        //            dataTable.Columns.Add("dataDatabaseName", typeof(string));
        //            dataTable.Columns.Add("httpHost", typeof(int));


        //            dataTable.Columns.Add("dateCreated", typeof(DateTime));
        //            dataTable.Columns.Add("dateExpired", typeof(DateTime));

        //            Tracker tracker = new Tracker();
        //            string dateTime;

        //            while (mySqlDataReader.Read()) {

        //                tracker.Collections = (string)mySqlDataReader["tcollections"];
        //                tracker.CompanyDatabaseName = (string)mySqlDataReader["tcmp"];
        //                tracker.DatabaseHost = int.Parse((string)mySqlDataReader["tdbhost"]);
        //                tracker.DatabaseName = (string)mySqlDataReader["tdbs"];

        //                dateTime = (string)mySqlDataReader["tcreatedate"];
        //                tracker.DateTimeCreated = SubStandard.dateTime(dateTime);
        //                dateTime = String.Empty;

        //                dateTime = (string)mySqlDataReader["ttrackerexpiry"];
        //                tracker.DateTimeExpired = SubStandard.dateTime(dateTime);
        //                dateTime = String.Empty;

        //                tracker.DeviceImei = (string)mySqlDataReader["tunit"];
        //                tracker.DevicePassword = (string)mySqlDataReader["tunitpassword"];
        //                tracker.DeviceType = int.Parse((string)mySqlDataReader["ttype"]);
        //                tracker.DriverName = (string)mySqlDataReader["tdrivername"];
        //                tracker.Emails = (string)mySqlDataReader["temails"];
        //                tracker.HttpHost = int.Parse((string)mySqlDataReader["thttphost"]);
        //                tracker.Id = (int)mySqlDataReader["tid"];
        //                tracker.IdlingTime = int.Parse((string)mySqlDataReader["tidlingtime"]);
        //                tracker.ImageNumber = int.Parse((string)mySqlDataReader["timg"]);
        //                tracker.Inputs = (string)mySqlDataReader["tinputs"];
        //                tracker.MileageInitial = int.Parse((string)mySqlDataReader["tmileageInit"]);
        //                tracker.MileageLimit = int.Parse((string)mySqlDataReader["tmileagelimit"]);
        //                tracker.MobileDataProvider = int.Parse((string)mySqlDataReader["tprovider"]);
        //                tracker.Note = (string)mySqlDataReader["tnote"];
        //                tracker.OwnerName = (string)mySqlDataReader["townername"];
        //                tracker.SimImei = (string)mySqlDataReader["tsimsr"];
        //                tracker.SimNumber = (string)mySqlDataReader["tsimno"];
        //                tracker.Users = (string)mySqlDataReader["tusers"];
        //                tracker.SpeedLimit = int.Parse((string)mySqlDataReader["tspeedlimit"]);
        //                tracker.VehicleModel = (string)mySqlDataReader["tvehiclemodel"];
        //                tracker.VehicleRegistration = (string)mySqlDataReader["tvehiclereg"];


        //                DataRow dataRow = dataTable.NewRow();
        //                dataRow["id"] = tracker.Id;
        //                dataRow["vehicleRegistration"] = tracker.VehicleRegistration;
        //                dataRow["vehicleModel"] = tracker.VehicleModel;
        //                dataRow["ownerName"] = tracker.OwnerName;
        //                dataRow["driverName"] = tracker.DriverName;

        //                dataRow["simImei"] = tracker.SimImei;
        //                dataRow["simNumber"] = tracker.SimNumber;
        //                dataRow["mobileDataProvider"] = tracker.MobileDataProvider;

        //                dataRow["deviceImei"] = tracker.DeviceImei;
        //                dataRow["devicePassword"] = tracker.DevicePassword;
        //                dataRow["deviceType"] = tracker.DeviceType;

        //                dataRow["emails"] = tracker.Emails;
        //                dataRow["users"] = tracker.Users;

        //                dataRow["mileageInitial"] = tracker.MileageInitial;
        //                dataRow["mileageLimit"] = tracker.MileageLimit;
        //                dataRow["speedLimit"] = tracker.SpeedLimit;

        //                dataRow["idlingTime"] = tracker.IdlingTime;
        //                dataRow["inputs"] = tracker.Inputs;
        //                dataRow["imageNumber"] = tracker.ImageNumber;
        //                dataRow["note"] = tracker.Note;

        //                dataRow["collections"] = tracker.Collections;
        //                dataRow["companyDatabaseName"] = tracker.CompanyDatabaseName;
        //                dataRow["databaseHost"] = tracker.DatabaseHost;
        //                dataRow["dataDatabaseName"] = tracker.DatabaseName;
        //                dataRow["httpHost"] = tracker.HttpHost;
        //                dataRow["dateCreated"] = tracker.DateTimeCreated;
        //                dataRow["dateExpired"] = tracker.DateTimeExpired;

        //                dataTable.Rows.Add(dataRow);
        //            }


        //            return dataTable;
        //        }
        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (MySqlException mySqlException) {
        //        throw new QueryException(1, mySqlException.Message);
        //    } catch (Exception exception) {
        //        throw new QueryException(1, exception.Message);
        //    } finally {
        //        mysqlConnection.Close();
        //    }
        //}

        //public DataTable getUsers(User User) {
        //    DataTable dataTable = new DataTable();
        //    try {
        //        mysqlConnection.Open();
        //        string sql;
        //        if (User.AccessLevel == 1 || User.AccessLevel == 2) {
        //            sql =
        //                "SELECT  * " +
        //                "FROM " + User.DatabaseName + ".usrs " +
        //                "WHERE " + User.DatabaseName + ".usrs.upriv >= " + User.AccessLevel.ToString() + ";";
        //        } else {
        //            sql =
        //               "SELECT  * " +
        //               "FROM " + User.DatabaseName + ".usrs " +
        //               "WHERE " + User.DatabaseName + ".usrs.upriv = " + User.AccessLevel.ToString() + " " +
        //               "AND " + User.DatabaseName + ".usrs.uname = '" + User.Username + "';";
        //        }

        //        MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);

        //        MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

        //        if (!mySqlDataReader.HasRows) {
        //            throw new QueryException(1, "User's Collection is empty.");
        //        } else {
        //            dataTable.Load(mySqlDataReader);
        //            return dataTable;
        //        }
        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (MySqlException mySqlException) {
        //        throw new QueryException(1, mySqlException.Message);
        //    } catch (Exception exception) {
        //        throw new QueryException(1, exception.Message);
        //    } finally {
        //        mysqlConnection.Close();
        //    }
        //}

        //public DataTable getTrackerHistoricalData(User User, DateTime dateTimeDateFrom, DateTime dateTimeDateTo, ReportType reportType, int limit, int offset, Tracker tracker) {
        //    DataTable dataTable = new DataTable();
        //    try {
        //        mysqlConnection.Open();

        //        double timestampFrom = UnixTime.toUnixTimestamp(dateTimeDateFrom);
        //        double timestampTo = UnixTime.toUnixTimestamp(dateTimeDateTo);

        //        string sql = "";
        //        if (timestampFrom > timestampTo) {
        //            sql =
        //               "SELECT " +

        //               "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_id, " +
        //               "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time, " +
        //               "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_lat, " +
        //               "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_lng, " +
        //               "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_ori, " +
        //               "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_speed, " +
        //               "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_mileage, " +
        //               "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_data " +


        //               "FROM trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + " " +
        //               "WHERE trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time >= " + timestampTo.ToString() + " " +
        //               "AND trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time <= " + timestampFrom.ToString() + " " +
        //               "ORDER BY trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time ASC LIMIT " + limit.ToString() + " OFFSET " + offset.ToString() + ";";
        //        } else {
        //            sql =
        //                "SELECT " +

        //                "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_id, " +
        //                "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time, " +
        //                "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_lat, " +
        //                "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_lng, " +
        //                "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_ori, " +
        //                "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_speed, " +
        //                "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_mileage, " +
        //                "trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_data " +

        //                "FROM trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + " " +
        //                "WHERE trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time <= " + timestampTo.ToString() + " " +
        //                "AND trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time >= " + timestampFrom.ToString() + " " +
        //                "ORDER BY trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time ASC LIMIT " + limit.ToString() + " OFFSET " + offset.ToString() + ";";
        //        }
        //        MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);

        //        MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

        //        dataTable.TableName = tracker.dataDatabaseName;
        //        dataTable.Columns.Add("No", typeof(int));
        //        dataTable.Columns.Add("DateTime", typeof(DateTime));
        //        dataTable.Columns.Add("Latitude", typeof(double));
        //        dataTable.Columns.Add("Longitude", typeof(double));
        //        dataTable.Columns.Add("Speed", typeof(int));
        //        dataTable.Columns.Add("Mileage", typeof(double));
        //        //dataTable.Columns.Add("Fuel", typeof(double));
        //        //dataTable.Columns.Add("Cost", typeof(double));
        //        dataTable.Columns.Add("Altitude", typeof(int));
        //        dataTable.Columns.Add("Degrees", typeof(int));
        //        dataTable.Columns.Add("Direction", typeof(string));
        //        dataTable.Columns.Add("GpsSatellites", typeof(int));
        //        dataTable.Columns.Add("GsmSignal", typeof(int));
        //        dataTable.Columns.Add("EventCode", typeof(string));
        //        dataTable.Columns.Add("Geofence", typeof(string));
        //        dataTable.Columns.Add("ACC", typeof(bool));
        //        dataTable.Columns.Add("SOS", typeof(bool));
        //        dataTable.Columns.Add("OverSpeed", typeof(bool));
        //        dataTable.Columns.Add("Battery", typeof(double));
        //        dataTable.Columns.Add("BatteryVoltage", typeof(double));
        //        dataTable.Columns.Add("ExternalVoltage", typeof(double));

        //        if (!mySqlDataReader.HasRows) {
        //            //throw new QueryException(1, "Tracker data is empty.");
        //            return dataTable;
        //        } else {
        //            int index = offset;
        //            while (mySqlDataReader.Read()) {
        //                index++;

        //                DataRow dataRow = dataTable.NewRow();
        //                dataRow["No"] = index;
        //                dataRow["DateTime"] = UnixTime.toDateTime(double.Parse((string)mySqlDataReader["gm_time"]));

        //                double latitude = double.Parse((string)mySqlDataReader["gm_lat"]);
        //                double longitude = double.Parse((string)mySqlDataReader["gm_lng"]);

        //                dataRow["Latitude"] = latitude;
        //                dataRow["Longitude"] = longitude;
        //                dataRow["Speed"] = int.Parse((string)mySqlDataReader["gm_speed"]);
        //                dataRow["Mileage"] = Math.Round(double.Parse((string)mySqlDataReader["gm_mileage"]), 2);

        //                if ((double)dataRow["Mileage"] < 0) {
        //                    dataRow["Mileage"] = ((double)dataRow["Mileage"]) * -1;
        //                }

        //                //dataRow["Fuel"] = Math.Round(double.Parse((string)mySqlDataReader["gm_mileage"]) / Settings.Default.fuelLiterToKilometer, 2);
        //                //dataRow["Cost"] = Math.Round(((double)dataRow["Fuel"]) * Settings.Default.fuelLiterToCost, 2);
        //                dataRow["Degrees"] = int.Parse((string)mySqlDataReader["gm_ori"]);
        //                dataRow["Direction"] = Direction.degreesToCardinalDetailed(double.Parse((string)mySqlDataReader["gm_ori"]));

        //                //1,			            //                                                          (0)
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

        //                string gmData = (string)mySqlDataReader["gm_data"];
        //                string[] data = gmData.Split(',');
        //                dataRow["EventCode"] = Enum.GetName(typeof(EventCode), (EventCode)int.Parse(data[1]));
        //                dataRow["GpsSatellites"] = int.Parse(data[2]);
        //                dataRow["GsmSignal"] = int.Parse(data[3]);
        //                dataRow["Altitude"] = int.Parse(data[6]);

        //                dataRow["ACC"] = (int.Parse(data[18]) == 1) ? true : false;
        //                dataRow["SOS"] = (int.Parse(data[17]) == 1) ? true : false;
        //                dataRow["OverSpeed"] = ((int)dataRow["Speed"] > tracker.speedLimit) ? true : false;
        //                //Geofence
        //                Location location = new Location(latitude, longitude);

        //                foreach (Geofence geofence in User.geofenceCollection) {
        //                    if (Geofence.IsPointInPolygon(geofence, location)) {
        //                        if (String.IsNullOrEmpty(geofence.Name)) {
        //                            dataRow["Geofence"] = "";
        //                        } else {
        //                            dataRow["Geofence"] = geofence.Name;
        //                        }
        //                    }
        //                };

        //                double batteryStrength = (double)int.Parse(data[28], System.Globalization.NumberStyles.AllowHexSpecifier);
        //                batteryStrength = ((batteryStrength - 2114f) * (100f / 492f));//*100.0;
        //                batteryStrength = Math.Round(batteryStrength, 2);
        //                if (batteryStrength > 100) {
        //                    batteryStrength = 100f;
        //                } else if (batteryStrength < 0) {
        //                    batteryStrength = 0;
        //                }

        //                double batteryVoltage = (double)int.Parse(data[28], System.Globalization.NumberStyles.AllowHexSpecifier);
        //                batteryVoltage = (batteryVoltage * 3 * 2) / 1024;
        //                batteryVoltage = Math.Round(batteryVoltage, 2);
        //                double externalVoltage = (double)int.Parse(data[29], System.Globalization.NumberStyles.AllowHexSpecifier);
        //                externalVoltage = (externalVoltage * 3 * 16) / 1024;
        //                externalVoltage = Math.Round(externalVoltage, 2);

        //                dataRow["Battery"] = batteryStrength;
        //                dataRow["BatteryVoltage"] = batteryVoltage;
        //                dataRow["ExternalVoltage"] = externalVoltage;

        //                dataTable.Rows.Add(dataRow);
        //            }
        //            mySqlDataReader.Close();
        //            return dataTable;
        //        }
        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (MySqlException mySqlException) {
        //        throw mySqlException;
        //    } catch (Exception exception) {
        //        throw exception;
        //    } finally {
        //        mysqlConnection.Close();
        //    }


        //}

        //public int getTrackerHistoricalDataCount(User User, DateTime dateTimeDateFrom, DateTime dateTimeDateTo, ReportType reportType, Tracker tracker) {
        //    DataTable dataTable = new DataTable();
        //    try {
        //        mysqlConnection.Open();

        //        double timestampFrom = TqatProModel.Helper.Converter.dateTimeToUnixTimestamp(dateTimeDateFrom);
        //        double timestampTo = TqatProModel.Helper.Converter.dateTimeToUnixTimestamp(dateTimeDateTo);

        //        string sql = "";
        //        if (timestampFrom > timestampTo) {
        //            sql =
        //               "SELECT  COUNT(*) " +
        //               "FROM trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + " " +
        //               "WHERE trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time >= " + timestampTo.ToString() + " " +
        //               "AND trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time <= " + timestampFrom.ToString() + " " +
        //               "ORDER BY trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_id ASC;";


        //        } else {
        //            sql =
        //                  "SELECT  COUNT(*) " +
        //                  "FROM trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + " " +
        //                  "WHERE trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time <= " + timestampTo.ToString() + " " +
        //                  "AND trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_time >= " + timestampFrom.ToString() + " " +
        //                  "ORDER BY trk_" + tracker.dataDatabaseName + ".gps_" + tracker.dataDatabaseName + ".gm_id ASC;";

        //        }
        //        MySqlCommand mySqlCommand = new MySqlCommand(sql, mysqlConnection);

        //        int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
        //        return count;

        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (MySqlException mySqlException) {
        //        throw mySqlException;
        //    } catch (Exception exception) {
        //        throw exception;
        //    } finally {
        //        mysqlConnection.Close();
        //    }


        //}

        //public DataTable getTrackerGeofence(User User, DateTime dateTimeDateFrom, DateTime dateTimeDateTo, ReportType reportType, int limit, int offset, Tracker tracker) {
        //    DataTable dataTableHistoricalData = this.getTrackerHistoricalData(User, dateTimeDateFrom, dateTimeDateTo, reportType, limit, offset, tracker);
        //    DataTable dataTableGeofenceData = new DataTable();
        //    dataTableGeofenceData.Columns.Add("No", typeof(int));
        //    dataTableGeofenceData.Columns.Add("Status", typeof(bool));
        //    dataTableGeofenceData.Columns.Add("DateTimeFrom", typeof(DateTime));
        //    dataTableGeofenceData.Columns.Add("DateTimeTo", typeof(DateTime));
        //    dataTableGeofenceData.Columns.Add("Time", typeof(TimeSpan));
        //    dataTableGeofenceData.Columns.Add("SpeedMax", typeof(double));
        //    dataTableGeofenceData.Columns.Add("SpeedAve", typeof(double));
        //    dataTableGeofenceData.Columns.Add("Distance", typeof(double));
        //    dataTableGeofenceData.Columns.Add("Fuel", typeof(double));
        //    dataTableGeofenceData.Columns.Add("Cost", typeof(double));
        //    dataTableGeofenceData.Columns.Add("Geofence", typeof(string));

        //    try {

        //        string geofenceStatusNow = "";
        //        string geofenceStatusBefore = "";

        //        bool acc = false;

        //        int index = 0;

        //        double speed = 0;
        //        double speedDivisor = 0;
        //        double speedAccumulator = 0;
        //        double speedMax = 0;
        //        double speedAverage = 0;


        //        double distanceBefore = 0;
        //        double distance = 0;

        //        DateTime dateTimeGeofenceFrom = new DateTime();
        //        DateTime dateTimeGeofenceTo = new DateTime();

        //        TimeSpan timeSpan;

        //        for (int no = 0; no < dataTableHistoricalData.Rows.Count; no++) {
        //            if ((int)dataTableHistoricalData.Rows[no]["No"] == 1) {
        //                dateTimeGeofenceFrom = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];
        //                distanceBefore = (double)dataTableHistoricalData.Rows[no]["Mileage"];
        //            }

        //            acc = (bool)dataTableHistoricalData.Rows[no]["ACC"];

        //            speed = (int)dataTableHistoricalData.Rows[no]["Speed"];
        //            speedDivisor++;
        //            speedAccumulator += speed;
        //            if (speed > speedMax) {
        //                speedMax = speed;
        //            }


        //            if (!(dataTableHistoricalData.Rows[no]["Geofence"] == System.DBNull.Value)) {
        //                geofenceStatusNow = (string)dataTableHistoricalData.Rows[no]["Geofence"];
        //            } else {
        //                geofenceStatusNow = "";
        //            }


        //            if (geofenceStatusBefore != geofenceStatusNow) {
        //                index++;


        //                dateTimeGeofenceTo = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];

        //                timeSpan = dateTimeGeofenceTo - dateTimeGeofenceFrom;
        //                distance = (double)dataTableHistoricalData.Rows[no]["Mileage"] - distanceBefore;
        //                distance = Math.Round(distance, 2);

        //                speedAverage = speedAccumulator / speedDivisor;
        //                speedAverage = Math.Round(speedAverage, 2);
        //                speedAccumulator = 0;
        //                speedDivisor = 0;


        //                DataRow dataRowGeofenceData = dataTableGeofenceData.NewRow();
        //                dataRowGeofenceData["No"] = index;

        //                dataRowGeofenceData["DateTimeFrom"] = dateTimeGeofenceFrom;
        //                dataRowGeofenceData["DateTimeTo"] = dateTimeGeofenceTo;
        //                dataRowGeofenceData["Time"] = timeSpan;
        //                dataRowGeofenceData["Distance"] = distance;


        //                dataRowGeofenceData["Fuel"] = Math.Round(((double)dataRowGeofenceData["Distance"]) / Settings.Default.fuelLiterToKilometer, 2);
        //                dataRowGeofenceData["Cost"] = Math.Round(((double)dataRowGeofenceData["Fuel"]) / Settings.Default.fuelLiterToCost, 2);


        //                dataRowGeofenceData["Geofence"] = geofenceStatusBefore;
        //                dataRowGeofenceData["SpeedMax"] = speedMax;
        //                dataRowGeofenceData["SpeedAve"] = speedAverage;
        //                dataRowGeofenceData["Status"] = geofenceStatusBefore == "" ? false : true;

        //                speedMax = 0;
        //                distanceBefore = distance + distanceBefore;
        //                dateTimeGeofenceFrom = dateTimeGeofenceTo;
        //                geofenceStatusBefore = geofenceStatusNow;
        //                dataTableGeofenceData.Rows.Add(dataRowGeofenceData);
        //            }

        //            if (no == dataTableHistoricalData.Rows.Count - 1) {
        //                index++;


        //                dateTimeGeofenceTo = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];

        //                timeSpan = dateTimeGeofenceTo - dateTimeGeofenceFrom;
        //                distance = (double)dataTableHistoricalData.Rows[no]["Mileage"] - distanceBefore;
        //                distance = Math.Round(distance, 2);

        //                speedAverage = speedAccumulator / speedDivisor;
        //                speedAverage = Math.Round(speedAverage, 2);
        //                speedAccumulator = 0;
        //                speedDivisor = 0;


        //                DataRow dataRowGeofenceData = dataTableGeofenceData.NewRow();
        //                dataRowGeofenceData["No"] = index;

        //                dataRowGeofenceData["DateTimeFrom"] = dateTimeGeofenceFrom;
        //                dataRowGeofenceData["DateTimeTo"] = dateTimeGeofenceTo;
        //                dataRowGeofenceData["Time"] = timeSpan;
        //                dataRowGeofenceData["Distance"] = distance;


        //                dataRowGeofenceData["Fuel"] = Math.Round(((double)dataRowGeofenceData["Distance"]) / Settings.Default.fuelLiterToKilometer, 2);
        //                dataRowGeofenceData["Cost"] = Math.Round(((double)dataRowGeofenceData["Fuel"]) / Settings.Default.fuelLiterToCost, 2);


        //                dataRowGeofenceData["Geofence"] = geofenceStatusBefore;
        //                dataRowGeofenceData["SpeedMax"] = speedMax;
        //                dataRowGeofenceData["SpeedAve"] = speedAverage;
        //                dataRowGeofenceData["Status"] = geofenceStatusBefore == "" ? false : true;

        //                speedMax = 0;
        //                distanceBefore = distance + distanceBefore;
        //                dateTimeGeofenceFrom = dateTimeGeofenceTo;
        //                geofenceStatusBefore = geofenceStatusNow;
        //                dataTableGeofenceData.Rows.Add(dataRowGeofenceData);
        //            }
        //        }

        //        return dataTableGeofenceData;

        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (Exception exception) {
        //        throw new QueryException(1, exception.Message);
        //    }
        //}

        //public DataTable getTrackerIdlingData(User User, DateTime dateTimeDateFrom, DateTime dateTimeDateTo, ReportType reportType, int limit, int offset, Tracker tracker) {
        //    DataTable dataTableHistoricalData = this.getTrackerHistoricalData(User, dateTimeDateFrom, dateTimeDateTo, reportType, limit, offset, tracker);
        //    DataTable dataTableIdleData = new DataTable();
        //    dataTableIdleData.Columns.Add("No", typeof(int));
        //    dataTableIdleData.Columns.Add("Status", typeof(bool));
        //    dataTableIdleData.Columns.Add("DateTimeFrom", typeof(DateTime));
        //    dataTableIdleData.Columns.Add("DateTimeTo", typeof(DateTime));
        //    dataTableIdleData.Columns.Add("Time", typeof(TimeSpan));
        //    dataTableIdleData.Columns.Add("SpeedMax", typeof(double));
        //    dataTableIdleData.Columns.Add("SpeedAve", typeof(double));
        //    dataTableIdleData.Columns.Add("Distance", typeof(double));
        //    dataTableIdleData.Columns.Add("Fuel", typeof(double));
        //    dataTableIdleData.Columns.Add("Cost", typeof(double));
        //    dataTableIdleData.Columns.Add("Geofence", typeof(string));

        //    try {

        //        bool idleStatusNow = false;
        //        bool idleStatusBefore = false;

        //        EventCode eventCode = EventCode.TRACK_BY_TIME_INTERVAL;
        //        bool acc = false;

        //        int index = 0;

        //        double speed = 0;
        //        double speedDivisor = 0;
        //        double speedAccumulator = 0;
        //        double speedMax = 0;
        //        double speedAverage = 0;


        //        double distanceBefore = 0;
        //        double distance = 0;

        //        StringBuilder geofence = new StringBuilder();
        //        DateTime dateTimeIdleFrom = new DateTime();
        //        DateTime dateTimeIdleTo = new DateTime();

        //        TimeSpan timeSpan;

        //        for (int no = 0; no < dataTableHistoricalData.Rows.Count; no++) {
        //            if ((int)dataTableHistoricalData.Rows[no]["No"] == 1) {
        //                dateTimeIdleFrom = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];
        //                distanceBefore = (double)dataTableHistoricalData.Rows[no]["Mileage"];
        //            }

        //            eventCode = (dataTableHistoricalData.Rows[no]["EventCode"] == System.DBNull.Value) ?
        //               EventCode.TRACK_BY_TIME_INTERVAL : (EventCode)Enum.Parse(typeof(EventCode), (string)dataTableHistoricalData.Rows[no]["EventCode"], true);

        //            geofence.Append((dataTableHistoricalData.Rows[no]["Geofence"] == System.DBNull.Value) ? "" : (string)dataTableHistoricalData.Rows[no]["Geofence"] + " | ");

        //            acc = (bool)dataTableHistoricalData.Rows[no]["ACC"];

        //            speed = (int)dataTableHistoricalData.Rows[no]["Speed"];
        //            speedDivisor++;
        //            speedAccumulator += speed;
        //            if (speed > speedMax) {
        //                speedMax = speed;
        //            }


        //            if (speed == 0 && acc == true) {
        //                idleStatusNow = true;
        //            } else {
        //                idleStatusNow = false;
        //            }


        //            if (idleStatusBefore != idleStatusNow) {
        //                index++;
        //                idleStatusBefore = idleStatusNow;
        //                dateTimeIdleTo = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];

        //                timeSpan = dateTimeIdleTo - dateTimeIdleFrom;
        //                distance = (double)dataTableHistoricalData.Rows[no]["Mileage"] - distanceBefore;
        //                distance = Math.Round(distance, 2);

        //                speedAverage = speedAccumulator / speedDivisor;
        //                speedAverage = Math.Round(speedAverage, 2);
        //                speedAccumulator = 0;
        //                speedDivisor = 0;


        //                DataRow dataRowIdleData = dataTableIdleData.NewRow();
        //                dataRowIdleData["No"] = index;
        //                dataRowIdleData["Status"] = !idleStatusNow;
        //                dataRowIdleData["DateTimeFrom"] = dateTimeIdleFrom;
        //                dataRowIdleData["DateTimeTo"] = dateTimeIdleTo;
        //                dataRowIdleData["Time"] = timeSpan;
        //                dataRowIdleData["Distance"] = distance;
        //                dataRowIdleData["Geofence"] = geofence.ToString();
        //                geofence.Clear();

        //                dataRowIdleData["Fuel"] = Math.Round(((double)dataRowIdleData["Distance"]) / Settings.Default.fuelLiterToKilometer, 2);
        //                dataRowIdleData["Cost"] = Math.Round(((double)dataRowIdleData["Fuel"]) / Settings.Default.fuelLiterToCost, 2);

        //                dataRowIdleData["SpeedMax"] = speedMax;
        //                dataRowIdleData["SpeedAve"] = speedAverage;

        //                speedMax = 0;
        //                distanceBefore = distance + distanceBefore;
        //                dateTimeIdleFrom = dateTimeIdleTo;
        //                dataTableIdleData.Rows.Add(dataRowIdleData);
        //            }
        //            if (no == dataTableHistoricalData.Rows.Count - 1) {
        //                index++;
        //                idleStatusBefore = idleStatusNow;
        //                dateTimeIdleTo = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];

        //                timeSpan = dateTimeIdleTo - dateTimeIdleFrom;
        //                distance = (double)dataTableHistoricalData.Rows[no]["Mileage"] - distanceBefore;
        //                distance = Math.Round(distance, 2);

        //                speedAverage = speedAccumulator / speedDivisor;
        //                speedAverage = Math.Round(speedAverage, 2);
        //                speedAccumulator = 0;
        //                speedDivisor = 0;


        //                DataRow dataRowIdleData = dataTableIdleData.NewRow();
        //                dataRowIdleData["No"] = index;
        //                dataRowIdleData["Status"] = idleStatusNow;
        //                dataRowIdleData["DateTimeFrom"] = dateTimeIdleFrom;
        //                dataRowIdleData["DateTimeTo"] = dateTimeIdleTo;
        //                dataRowIdleData["Time"] = timeSpan;
        //                dataRowIdleData["Distance"] = distance;
        //                dataRowIdleData["Geofence"] = geofence.ToString();
        //                geofence.Clear();

        //                dataRowIdleData["Fuel"] = Math.Round(((double)dataRowIdleData["Distance"]) / Settings.Default.fuelLiterToKilometer, 2);
        //                dataRowIdleData["Cost"] = Math.Round(((double)dataRowIdleData["Fuel"]) / Settings.Default.fuelLiterToCost, 2);

        //                dataRowIdleData["SpeedMax"] = speedMax;
        //                dataRowIdleData["SpeedAve"] = speedAverage;

        //                speedMax = 0;
        //                distanceBefore = distance + distanceBefore;
        //                dateTimeIdleFrom = dateTimeIdleTo;
        //                dataTableIdleData.Rows.Add(dataRowIdleData);
        //            }
        //        }


        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (Exception exception) {
        //        throw new QueryException(1, exception.Message);
        //    } finally {

        //    }
        //    return dataTableIdleData;
        //}

        //public DataTable getTrackerRunningData(User User, DateTime dateTimeDateFrom, DateTime dateTimeDateTo, ReportType reportType, int limit, int offset, Tracker tracker) {
        //    DataTable dataTableHistoricalData = this.getTrackerHistoricalData(User, dateTimeDateFrom, dateTimeDateTo, reportType, limit, offset, tracker);
        //    DataTable dataTableIdleData = new DataTable();
        //    dataTableIdleData.Columns.Add("No", typeof(int));
        //    dataTableIdleData.Columns.Add("Status", typeof(bool));
        //    dataTableIdleData.Columns.Add("DateTimeFrom", typeof(DateTime));
        //    dataTableIdleData.Columns.Add("DateTimeTo", typeof(DateTime));
        //    dataTableIdleData.Columns.Add("Time", typeof(TimeSpan));
        //    dataTableIdleData.Columns.Add("SpeedMax", typeof(double));
        //    dataTableIdleData.Columns.Add("SpeedAve", typeof(double));
        //    dataTableIdleData.Columns.Add("Distance", typeof(double));
        //    dataTableIdleData.Columns.Add("Fuel", typeof(double));
        //    dataTableIdleData.Columns.Add("Cost", typeof(double));
        //    dataTableIdleData.Columns.Add("Geofence", typeof(string));

        //    try {

        //        bool runningStatusNow = false;
        //        bool runningStatusBefore = false;
        //        EventCode eventCode = EventCode.TRACK_BY_TIME_INTERVAL;
        //        bool acc = false;

        //        int index = 0;

        //        double speed = 0;
        //        double speedDivisor = 0;
        //        double speedAccumulator = 0;
        //        double speedMax = 0;
        //        double speedAverage = 0;


        //        double distanceBefore = 0;
        //        double distance = 0;

        //        StringBuilder geofence = new StringBuilder();
        //        DateTime dateTimeRunningFrom = new DateTime();
        //        DateTime dateTimeRunningTo = new DateTime();

        //        TimeSpan timeSpan;

        //        for (int no = 0; no < dataTableHistoricalData.Rows.Count; no++) {
        //            if ((int)dataTableHistoricalData.Rows[no]["No"] == 1) {
        //                dateTimeRunningFrom = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];
        //                distanceBefore = (double)dataTableHistoricalData.Rows[no]["Mileage"];
        //            }

        //            eventCode = (dataTableHistoricalData.Rows[no]["EventCode"] == System.DBNull.Value) ?
        //               EventCode.TRACK_BY_TIME_INTERVAL : (EventCode)Enum.Parse(typeof(EventCode), (string)dataTableHistoricalData.Rows[no]["EventCode"], true);

        //            geofence.Append((dataTableHistoricalData.Rows[no]["Geofence"] == System.DBNull.Value) ? "" : (string)dataTableHistoricalData.Rows[no]["Geofence"] + " | ");

        //            acc = (bool)dataTableHistoricalData.Rows[no]["ACC"];

        //            speed = (int)dataTableHistoricalData.Rows[no]["Speed"];
        //            speedDivisor++;
        //            speedAccumulator += speed;
        //            if (speed > speedMax) {
        //                speedMax = speed;
        //            }


        //            if (acc == true && speed > 0) {
        //                runningStatusNow = true;
        //            }

        //            if (acc = true && speed <= 0) {
        //                runningStatusNow = false;
        //            }

        //            if (acc = false && speed <= 0) {
        //                runningStatusNow = false;
        //            }
        //            if (runningStatusBefore != runningStatusNow) {
        //                runningStatusBefore = runningStatusNow;
        //                index++;
        //                dateTimeRunningTo = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];

        //                timeSpan = dateTimeRunningTo - dateTimeRunningFrom;
        //                distance = (double)dataTableHistoricalData.Rows[no]["Mileage"] - distanceBefore;
        //                distance = Math.Round(distance, 2);


        //                speedAverage = speedAccumulator / speedDivisor;
        //                speedAverage = Math.Round(speedAverage, 2);
        //                speedAccumulator = 0;
        //                speedDivisor = 0;


        //                DataRow dataRowIdleData = dataTableIdleData.NewRow();
        //                dataRowIdleData["No"] = index;
        //                dataRowIdleData["Status"] = !runningStatusNow;
        //                dataRowIdleData["DateTimeFrom"] = dateTimeRunningFrom;
        //                dataRowIdleData["DateTimeTo"] = dateTimeRunningTo;
        //                dataRowIdleData["Time"] = timeSpan;
        //                dataRowIdleData["Distance"] = distance;

        //                dataRowIdleData["Fuel"] = Math.Round(((double)dataRowIdleData["Distance"]) / Settings.Default.fuelLiterToKilometer, 2);
        //                dataRowIdleData["Cost"] = Math.Round(((double)dataRowIdleData["Fuel"]) / Settings.Default.fuelLiterToCost, 2);

        //                dataRowIdleData["Geofence"] = geofence.ToString();
        //                geofence.Clear();
        //                dataRowIdleData["SpeedMax"] = speedMax;
        //                dataRowIdleData["SpeedAve"] = speedAverage;

        //                speedMax = 0;
        //                distanceBefore = distance + distanceBefore;
        //                dateTimeRunningFrom = dateTimeRunningTo;
        //                dataTableIdleData.Rows.Add(dataRowIdleData);
        //            }
        //            if (no == dataTableHistoricalData.Rows.Count - 1) {
        //                runningStatusBefore = runningStatusNow;
        //                index++;
        //                dateTimeRunningTo = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];

        //                timeSpan = dateTimeRunningTo - dateTimeRunningFrom;
        //                distance = (double)dataTableHistoricalData.Rows[no]["Mileage"] - distanceBefore;
        //                distance = Math.Round(distance, 2);


        //                speedAverage = speedAccumulator / speedDivisor;
        //                speedAverage = Math.Round(speedAverage, 2);
        //                speedAccumulator = 0;
        //                speedDivisor = 0;


        //                DataRow dataRowIdleData = dataTableIdleData.NewRow();
        //                dataRowIdleData["No"] = index;
        //                dataRowIdleData["Status"] = runningStatusNow;
        //                dataRowIdleData["DateTimeFrom"] = dateTimeRunningFrom;
        //                dataRowIdleData["DateTimeTo"] = dateTimeRunningTo;
        //                dataRowIdleData["Time"] = timeSpan;
        //                dataRowIdleData["Distance"] = distance;

        //                dataRowIdleData["Fuel"] = Math.Round(((double)dataRowIdleData["Distance"]) / Settings.Default.fuelLiterToKilometer, 2);
        //                dataRowIdleData["Cost"] = Math.Round(((double)dataRowIdleData["Fuel"]) / Settings.Default.fuelLiterToCost, 2);

        //                dataRowIdleData["Geofence"] = geofence.ToString();
        //                geofence.Clear();
        //                dataRowIdleData["SpeedMax"] = speedMax;
        //                dataRowIdleData["SpeedAve"] = speedAverage;

        //                speedMax = 0;
        //                distanceBefore = distance + distanceBefore;
        //                dateTimeRunningFrom = dateTimeRunningTo;
        //                dataTableIdleData.Rows.Add(dataRowIdleData);
        //            }
        //        }
        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (Exception exception) {
        //        throw new QueryException(1, exception.Message);
        //    } finally {

        //    }
        //    return dataTableIdleData;
        //}

        //public DataTable getTrackerAccData(User User, DateTime dateTimeDateFrom, DateTime dateTimeDateTo, ReportType reportType, int limit, int offset, Tracker tracker) {
        //    DataTable dataTableHistoricalData = this.getTrackerHistoricalData(User, dateTimeDateFrom, dateTimeDateTo, reportType, limit, offset, tracker);
        //    DataTable dataTableIdleData = new DataTable();
        //    dataTableIdleData.Columns.Add("No", typeof(int));
        //    dataTableIdleData.Columns.Add("Status", typeof(bool));
        //    dataTableIdleData.Columns.Add("DateTimeFrom", typeof(DateTime));
        //    dataTableIdleData.Columns.Add("DateTimeTo", typeof(DateTime));
        //    dataTableIdleData.Columns.Add("Time", typeof(TimeSpan));
        //    dataTableIdleData.Columns.Add("SpeedMax", typeof(double));
        //    dataTableIdleData.Columns.Add("SpeedAve", typeof(double));
        //    dataTableIdleData.Columns.Add("Distance", typeof(double));
        //    dataTableIdleData.Columns.Add("Fuel", typeof(double));
        //    dataTableIdleData.Columns.Add("Cost", typeof(double));
        //    dataTableIdleData.Columns.Add("Geofence", typeof(string));

        //    try {

        //        bool accStatusNow = false;
        //        bool accStatusBefore = false;


        //        int index = 0;

        //        double speed = 0;
        //        double speedDivisor = 0;
        //        double speedAccumulator = 0;
        //        double speedMax = 0;
        //        double speedAverage = 0;


        //        double distanceBefore = 0;
        //        double distance = 0;

        //        StringBuilder geofence = new StringBuilder();
        //        DateTime dateTimeRunningFrom = new DateTime();
        //        DateTime dateTimeRunningTo = new DateTime();

        //        TimeSpan timeSpan;


        //        for (int no = 0; no < dataTableHistoricalData.Rows.Count; no++) {
        //            if ((int)dataTableHistoricalData.Rows[no]["No"] == 1) {
        //                dateTimeRunningFrom = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];
        //                distanceBefore = (double)dataTableHistoricalData.Rows[no]["Mileage"];
        //            }

        //            geofence.Append((dataTableHistoricalData.Rows[no]["Geofence"] == System.DBNull.Value) ? "" : (string)dataTableHistoricalData.Rows[no]["Geofence"] + " | ");

        //            accStatusNow = (bool)dataTableHistoricalData.Rows[no]["ACC"];

        //            speed = (int)dataTableHistoricalData.Rows[no]["Speed"];
        //            speedDivisor++;
        //            speedAccumulator += speed;
        //            if (speed > speedMax) {
        //                speedMax = speed;
        //            }



        //            if (accStatusBefore != accStatusNow) {
        //                accStatusBefore = accStatusNow;
        //                index++;
        //                dateTimeRunningTo = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];

        //                timeSpan = dateTimeRunningTo - dateTimeRunningFrom;
        //                distance = (double)dataTableHistoricalData.Rows[no]["Mileage"] - distanceBefore;
        //                distance = Math.Round(distance, 2);


        //                speedAverage = speedAccumulator / speedDivisor;
        //                speedAverage = Math.Round(speedAverage, 2);
        //                speedAccumulator = 0;
        //                speedDivisor = 0;


        //                DataRow dataRowIdleData = dataTableIdleData.NewRow();
        //                dataRowIdleData["No"] = index;
        //                dataRowIdleData["Status"] = !accStatusNow;
        //                dataRowIdleData["DateTimeFrom"] = dateTimeRunningFrom;
        //                dataRowIdleData["DateTimeTo"] = dateTimeRunningTo;
        //                dataRowIdleData["Time"] = timeSpan;
        //                dataRowIdleData["Distance"] = distance;

        //                dataRowIdleData["Fuel"] = Math.Round(((double)dataRowIdleData["Distance"]) / Settings.Default.fuelLiterToKilometer, 2);
        //                dataRowIdleData["Cost"] = Math.Round(((double)dataRowIdleData["Fuel"]) / Settings.Default.fuelLiterToCost, 2);

        //                dataRowIdleData["Geofence"] = geofence;
        //                geofence.Clear();
        //                dataRowIdleData["SpeedMax"] = speedMax;
        //                dataRowIdleData["SpeedAve"] = speedAverage;

        //                speedMax = 0;
        //                distanceBefore = distance + distanceBefore;
        //                dateTimeRunningFrom = dateTimeRunningTo;
        //                dataTableIdleData.Rows.Add(dataRowIdleData);
        //            }
        //            if (no == dataTableHistoricalData.Rows.Count - 1) {
        //                accStatusBefore = accStatusNow;
        //                index++;
        //                dateTimeRunningTo = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];

        //                timeSpan = dateTimeRunningTo - dateTimeRunningFrom;
        //                distance = (double)dataTableHistoricalData.Rows[no]["Mileage"] - distanceBefore;
        //                distance = Math.Round(distance, 2);


        //                speedAverage = speedAccumulator / speedDivisor;
        //                speedAverage = Math.Round(speedAverage, 2);
        //                speedAccumulator = 0;
        //                speedDivisor = 0;


        //                DataRow dataRowIdleData = dataTableIdleData.NewRow();
        //                dataRowIdleData["No"] = index;
        //                dataRowIdleData["Status"] = accStatusNow;
        //                dataRowIdleData["DateTimeFrom"] = dateTimeRunningFrom;
        //                dataRowIdleData["DateTimeTo"] = dateTimeRunningTo;
        //                dataRowIdleData["Time"] = timeSpan;
        //                dataRowIdleData["Distance"] = distance;

        //                dataRowIdleData["Fuel"] = Math.Round(((double)dataRowIdleData["Distance"]) / Settings.Default.fuelLiterToKilometer, 2);
        //                dataRowIdleData["Cost"] = Math.Round(((double)dataRowIdleData["Fuel"]) / Settings.Default.fuelLiterToCost, 2);

        //                dataRowIdleData["Geofence"] = geofence;
        //                geofence.Clear();
        //                dataRowIdleData["SpeedMax"] = speedMax;
        //                dataRowIdleData["SpeedAve"] = speedAverage;

        //                speedMax = 0;
        //                distanceBefore = distance + distanceBefore;
        //                dateTimeRunningFrom = dateTimeRunningTo;
        //                dataTableIdleData.Rows.Add(dataRowIdleData);
        //            }
        //        }
        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (Exception exception) {
        //        throw new QueryException(1, exception.Message);
        //    } finally {

        //    }
        //    return dataTableIdleData;
        //}

        //public DataTable getTrackerExternalPowerCutData(User User, DateTime dateTimeDateFrom, DateTime dateTimeDateTo, ReportType reportType, int limit, int offset, Tracker tracker) {
        //    DataTable dataTableHistoricalData = this.getTrackerHistoricalData(User, dateTimeDateFrom, dateTimeDateTo, reportType, limit, offset, tracker);
        //    DataTable dataTableIdleData = new DataTable();
        //    dataTableIdleData.Columns.Add("No", typeof(int));
        //    dataTableIdleData.Columns.Add("Status", typeof(bool));
        //    dataTableIdleData.Columns.Add("DateTimeFrom", typeof(DateTime));
        //    dataTableIdleData.Columns.Add("DateTimeTo", typeof(DateTime));
        //    dataTableIdleData.Columns.Add("Time", typeof(TimeSpan));
        //    dataTableIdleData.Columns.Add("SpeedMax", typeof(double));
        //    dataTableIdleData.Columns.Add("SpeedAve", typeof(double));
        //    dataTableIdleData.Columns.Add("Distance", typeof(double));
        //    dataTableIdleData.Columns.Add("Fuel", typeof(double));
        //    dataTableIdleData.Columns.Add("Cost", typeof(double));
        //    dataTableIdleData.Columns.Add("Geofence", typeof(string));

        //    try {

        //        bool externalPowerStatusNow = false;
        //        bool externalPowerStatusBefore = false;

        //        EventCode eventCode = EventCode.TRACK_BY_TIME_INTERVAL;

        //        int index = 0;

        //        double speed = 0;
        //        double speedDivisor = 0;
        //        double speedAccumulator = 0;
        //        double speedMax = 0;
        //        double speedAverage = 0;


        //        double distanceBefore = 0;
        //        double distance = 0;

        //        StringBuilder geofence = new StringBuilder();
        //        DateTime dateTimeRunningFrom = new DateTime();
        //        DateTime dateTimeRunningTo = new DateTime();

        //        TimeSpan timeSpan;


        //        for (int no = 0; no < dataTableHistoricalData.Rows.Count; no++) {
        //            if ((int)dataTableHistoricalData.Rows[no]["No"] == 1) {
        //                dateTimeRunningFrom = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];
        //                distanceBefore = (double)dataTableHistoricalData.Rows[no]["Mileage"];
        //            }

        //            eventCode = (dataTableHistoricalData.Rows[no]["EventCode"] == System.DBNull.Value) ?
        //             EventCode.TRACK_BY_TIME_INTERVAL : (EventCode)Enum.Parse(typeof(EventCode), (string)dataTableHistoricalData.Rows[no]["EventCode"], true);

        //            geofence.Append((dataTableHistoricalData.Rows[no]["Geofence"] == System.DBNull.Value) ? "" : (string)dataTableHistoricalData.Rows[no]["Geofence"] + " | ");

        //            double externalVolt = (double)dataTableHistoricalData.Rows[no]["ExternalVoltage"];

        //            if (eventCode == EventCode.EXTERNAL_BATTERY_CUT || externalVolt < 12) {
        //                externalPowerStatusNow = true;
        //            }
        //            if (eventCode == EventCode.EXTERNAL_BATTERY_ON || externalVolt >= 12) {
        //                externalPowerStatusNow = false;
        //            }

        //            speed = (int)dataTableHistoricalData.Rows[no]["Speed"];
        //            speedDivisor++;
        //            speedAccumulator += speed;
        //            if (speed > speedMax) {
        //                speedMax = speed;
        //            }

        //            if (externalPowerStatusBefore != externalPowerStatusNow) {
        //                externalPowerStatusBefore = externalPowerStatusNow;
        //                index++;
        //                dateTimeRunningTo = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];

        //                timeSpan = dateTimeRunningTo - dateTimeRunningFrom;
        //                distance = (double)dataTableHistoricalData.Rows[no]["Mileage"] - distanceBefore;
        //                distance = Math.Round(distance, 2);


        //                speedAverage = speedAccumulator / speedDivisor;
        //                speedAverage = Math.Round(speedAverage, 2);
        //                speedAccumulator = 0;
        //                speedDivisor = 0;


        //                DataRow dataRowIdleData = dataTableIdleData.NewRow();
        //                dataRowIdleData["No"] = index;
        //                dataRowIdleData["Status"] = !externalPowerStatusNow;
        //                dataRowIdleData["DateTimeFrom"] = dateTimeRunningFrom;
        //                dataRowIdleData["DateTimeTo"] = dateTimeRunningTo;
        //                dataRowIdleData["Time"] = timeSpan;
        //                dataRowIdleData["Distance"] = distance;

        //                dataRowIdleData["Fuel"] = Math.Round(((double)dataRowIdleData["Distance"]) / Settings.Default.fuelLiterToKilometer, 2);
        //                dataRowIdleData["Cost"] = Math.Round(((double)dataRowIdleData["Fuel"]) / Settings.Default.fuelLiterToCost, 2);

        //                dataRowIdleData["Geofence"] = geofence.ToString();
        //                dataRowIdleData["SpeedMax"] = speedMax;
        //                dataRowIdleData["SpeedAve"] = speedAverage;

        //                speedMax = 0;
        //                distanceBefore = distance + distanceBefore;
        //                dateTimeRunningFrom = dateTimeRunningTo;
        //                dataTableIdleData.Rows.Add(dataRowIdleData);
        //                geofence.Clear();
        //            }
        //            if (no == dataTableHistoricalData.Rows.Count - 1) {
        //                index++;
        //                dateTimeRunningTo = (DateTime)dataTableHistoricalData.Rows[no]["DateTime"];

        //                timeSpan = dateTimeRunningTo - dateTimeRunningFrom;
        //                distance = (double)dataTableHistoricalData.Rows[no]["Mileage"] - distanceBefore;
        //                distance = Math.Round(distance, 2);


        //                speedAverage = speedAccumulator / speedDivisor;
        //                speedAverage = Math.Round(speedAverage, 2);
        //                speedAccumulator = 0;
        //                speedDivisor = 0;


        //                DataRow dataRowIdleData = dataTableIdleData.NewRow();
        //                dataRowIdleData["No"] = index;
        //                dataRowIdleData["Status"] = externalPowerStatusNow;
        //                dataRowIdleData["DateTimeFrom"] = dateTimeRunningFrom;
        //                dataRowIdleData["DateTimeTo"] = dateTimeRunningTo;
        //                dataRowIdleData["Time"] = timeSpan;
        //                dataRowIdleData["Distance"] = distance;

        //                dataRowIdleData["Fuel"] = Math.Round(((double)dataRowIdleData["Distance"]) / Settings.Default.fuelLiterToKilometer, 2);
        //                dataRowIdleData["Cost"] = Math.Round(((double)dataRowIdleData["Fuel"]) / Settings.Default.fuelLiterToCost, 2);

        //                dataRowIdleData["Geofence"] = geofence.ToString();
        //                dataRowIdleData["SpeedMax"] = speedMax;
        //                dataRowIdleData["SpeedAve"] = speedAverage;

        //                speedMax = 0;
        //                distanceBefore = distance + distanceBefore;
        //                dateTimeRunningFrom = dateTimeRunningTo;
        //                dataTableIdleData.Rows.Add(dataRowIdleData);
        //                geofence.Clear();
        //            }
        //        }
        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (Exception exception) {
        //        throw new QueryException(1, exception.Message);
        //    } finally {

        //    }
        //    return dataTableIdleData;
        //}

        //public DataTable getTrackerOverSpeedData(User User, DateTime dateTimeDateFrom, DateTime dateTimeDateTo, ReportType reportType, int limit, int offset, Tracker tracker) {
        //    DataTable dataTableHistoricalData = this.getTrackerHistoricalData(User, dateTimeDateFrom, dateTimeDateTo, reportType, limit, offset, tracker);
        //    DataTable dataTableOverSpeedData = new DataTable();
        //    dataTableOverSpeedData.Columns.Add("No", typeof(int));
        //    dataTableOverSpeedData.Columns.Add("Status", typeof(bool));
        //    dataTableOverSpeedData.Columns.Add("DateTime", typeof(DateTime));
        //    dataTableOverSpeedData.Columns.Add("Latitude", typeof(double));
        //    dataTableOverSpeedData.Columns.Add("Longitude", typeof(double));
        //    dataTableOverSpeedData.Columns.Add("Speed", typeof(int));
        //    dataTableOverSpeedData.Columns.Add("Mileage", typeof(double));
        //    dataTableOverSpeedData.Columns.Add("Geofence", typeof(string));

        //    try {

        //        bool overSpeedStatusNow = false;
        //        int index = 0;
        //        StringBuilder geofence = new StringBuilder();


        //        foreach (DataRow dataRowNow in dataTableHistoricalData.Rows) {

        //            overSpeedStatusNow = (bool)dataRowNow["OverSpeed"];

        //            if (overSpeedStatusNow) {

        //                index++;

        //                geofence.Append((dataRowNow["Geofence"] == System.DBNull.Value) ? "" : (string)dataRowNow["Geofence"] + " | ");


        //                DataRow dataRowOverspeedData = dataTableOverSpeedData.NewRow();
        //                dataRowOverspeedData["No"] = index;
        //                dataRowOverspeedData["Status"] = overSpeedStatusNow;
        //                dataRowOverspeedData["DateTime"] = (DateTime)dataRowNow["DateTime"];
        //                dataRowOverspeedData["Latitude"] = (double)dataRowNow["Latitude"];
        //                dataRowOverspeedData["Longitude"] = (double)dataRowNow["Longitude"];
        //                dataRowOverspeedData["Speed"] = (int)dataRowNow["Speed"];
        //                dataRowOverspeedData["Mileage"] = (double)dataRowNow["Mileage"];
        //                dataRowOverspeedData["Geofence"] = geofence.ToString();
        //                geofence.Clear();
        //                dataTableOverSpeedData.Rows.Add(dataRowOverspeedData);
        //            }
        //        }
        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (Exception exception) {
        //        throw new QueryException(1, exception.Message);
        //    } finally {

        //    }
        //    return dataTableOverSpeedData;
        //}

        //public DataTable getTrackersGeofence(User User, DateTime dateTimeDateFrom, DateTime dateTimeDateTo, ReportType reportType, int limit, int offset, List<Tracker> trackerList) {

        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("No", typeof(int));
        //    dataTable.Columns.Add("VehicleRegistration", typeof(string));
        //    dataTable.Columns.Add("DriverName", typeof(string));
        //    dataTable.Columns.Add("DeviceImei", typeof(string));
        //    dataTable.Columns.Add("DateTimeFrom", typeof(DateTime));
        //    dataTable.Columns.Add("DateTimeTo", typeof(DateTime));
        //    dataTable.Columns.Add("SpeedMax", typeof(double));
        //    dataTable.Columns.Add("SpeedAve", typeof(double));
        //    dataTable.Columns.Add("Distance", typeof(double));
        //    dataTable.Columns.Add("Fuel", typeof(double));
        //    dataTable.Columns.Add("Cost", typeof(double));
        //    dataTable.Columns.Add("Geofence", typeof(string));

        //    try {
        //        int index = 0;
        //        foreach (Tracker tracker in trackerList) {
        //            index++;

        //            DataTable dataTableGeofence = (this.getTrackerGeofence(User, dateTimeDateFrom, dateTimeDateTo, reportType, limit, offset, tracker));

        //            if (dataTableGeofence.Rows.Count <= 0) {
        //                DataRow dataRow = dataTable.NewRow();
        //                dataRow["No"] = index;
        //                dataRow["VehicleRegistration"] = tracker.vehicleRegistration;
        //                dataRow["DriverName"] = tracker.driverName;
        //                dataRow["DeviceImei"] = tracker.deviceImei;

        //                dataRow["DateTimeFrom"] = dateTimeDateFrom;
        //                dataRow["DateTimeTo"] = dateTimeDateTo;
        //                dataRow["SpeedAve"] = 0;
        //                dataRow["SpeedMax"] = 0;
        //                dataRow["Distance"] = 0;
        //                dataRow["Fuel"] = 0;
        //                dataRow["Cost"] = 0;
        //                dataRow["Geofence"] = "";
        //                dataTable.Rows.Add(dataRow);
        //            } else {
        //                DataRow dataRow = dataTable.NewRow();
        //                dataRow["No"] = index;
        //                dataRow["VehicleRegistration"] = tracker.vehicleRegistration;
        //                dataRow["DriverName"] = tracker.driverName;
        //                dataRow["DeviceImei"] = tracker.deviceImei;

        //                dataRow["DateTimeFrom"] = dateTimeDateFrom;
        //                dataRow["DateTimeTo"] = dateTimeDateTo;

        //                double speedAve = 0;
        //                if (dataTableGeofence.Rows.Count > 0)
        //                    speedAve = Converter.dataTableColumnSumValue(dataTableGeofence, dataTableGeofence.Columns["SpeedAve"]) / dataTableGeofence.Rows.Count;
        //                dataRow["SpeedAve"] = Math.Round(speedAve, 2);

        //                double speedMax = Converter.dataTableColumnMaxValue(dataTableGeofence, dataTableGeofence.Columns["SpeedMax"]);
        //                dataRow["SpeedMax"] = Math.Round(speedMax, 2);

        //                dataRow["Distance"] = Math.Round(Converter.dataTableColumnSumValue(dataTableGeofence, dataTableGeofence.Columns["Distance"]), 2);
        //                dataRow["Fuel"] = Math.Round(Converter.dataTableColumnSumValue(dataTableGeofence, dataTableGeofence.Columns["Fuel"]), 2);
        //                dataRow["Cost"] = Math.Round(Converter.dataTableColumnSumValue(dataTableGeofence, dataTableGeofence.Columns["Cost"]), 2);

        //                dataRow["Geofence"] = Converter.dataTableColumnAppend(dataTableGeofence, dataTableGeofence.Columns["Geofence"], ":");
        //                dataTable.Rows.Add(dataRow);
        //            }
        //        }
        //        return dataTable;
        //    } catch (QueryException queryException) {
        //        throw queryException;
        //    } catch (MySqlException mySqlException) {
        //        throw new QueryException(1, mySqlException.Message);
        //    } catch (Exception exception) {
        //        throw new QueryException(1, exception.Message);
        //    }
        //}

    }
}
