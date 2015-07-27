using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using TqatProModel;
using TqatProViewModel;


namespace TqatProTrackingTool.Control {
    public class Map {

        mshtml.IHTMLDocument3 htmlDocument = null;

        public void loadPoi(WebBrowser webBrowser, Poi poi) {
            if (webBrowser != null) {
                try {
                    htmlDocument = (mshtml.IHTMLDocument3)webBrowser.Document;

                    mshtml.IHTMLElementCollection collection = htmlDocument.getElementsByName("formPoi");

                    foreach (mshtml.HTMLInputElement element in collection.item(name: "formPoi")) {

                        if (element.id == "poiId") {
                            element.value = poi.id.ToString();
                        }

                        if (element.id == "poiLatitude") {
                            element.value = poi.location.latitude.ToString();
                        }
                        if (element.id == "poiLongitude") {
                            element.value = poi.location.longitude.ToString();
                        }

                        if (element.id == "poiName") {
                            element.value = poi.name;
                        }

                        if (element.id == "poiDescription") {
                            element.value = poi.description;
                        }

                        if (element.id == "poiIcon") {
                            element.value = poi.image;
                        }

                        if (element.id == "poiSubmit") {
                            mshtml.HTMLInputElement btnSubmit = element;
                            btnSubmit.click();
                        }

                    }
                } catch {

                }
            }
        }
        public void loadGeofence(WebBrowser webBrowser, Geofence geofence) {
            if (webBrowser != null) {
                try {
                    htmlDocument = (mshtml.IHTMLDocument3)webBrowser.Document;

                    mshtml.IHTMLElementCollection collection = htmlDocument.getElementsByName("formGeofence");

                    foreach (mshtml.HTMLInputElement element in collection.item(name: "formGeofence")) {

                        if (element.id == "geofenceId") {
                            element.value = geofence.Id.ToString();
                        }

                        if (element.id == "geofenceCoordinates") {
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.Append("[");
                            foreach (Coordinate coordinate in geofence.getPoints()) {
                                stringBuilder.Append("{\"Latitude\":\"");
                                stringBuilder.Append(coordinate.latitude.ToString());
                                stringBuilder.Append("\",\"Longitude\":\"");
                                stringBuilder.Append(coordinate.longitude.ToString());
                                stringBuilder.Append("\"},");
                            }
                            stringBuilder.Remove(stringBuilder.Length - 1, 1);
                            stringBuilder.Append("]");

                            element.value = stringBuilder.ToString();

                        }
                        if (element.id == "geofenceName") {
                            element.value = geofence.Name;
                        }

                        if (element.id == "geofenceSubmit") {
                            mshtml.HTMLInputElement btnSubmit = element;
                            btnSubmit.click();
                        }

                    }
                } catch {

                }
            }
        }
        public void loadTracker(WebBrowser webBrowser, TrackerItem trackerItem, TrackerData trackerData, string displayMember) {
            if (webBrowser != null) {
                try {

                    htmlDocument = (mshtml.IHTMLDocument3)webBrowser.Document;

                    mshtml.IHTMLElementCollection collection = htmlDocument.getElementsByName("formTracker");


                    foreach (mshtml.HTMLInputElement element in collection.item(name: "formTracker")) {

                        if (element.id == "trackerId") {
                            element.value = trackerData.Tracker.Id.ToString();
                        }
                        if (element.id == "trackerLabel") {
                            if (displayMember == "VehicleRegistration") {
                                element.value = trackerData.Tracker.VehicleRegistration;
                            } else if (displayMember == "DriverName") {
                                element.value = trackerData.Tracker.DriverName;
                            } else if (displayMember == "VehicleModel") {
                                element.value = trackerData.Tracker.VehicleModel;
                            } else if (displayMember == "OwnerName") {
                                element.value = trackerData.Tracker.OwnerName;
                            } else if (displayMember == "TrackerImei") {
                                element.value = trackerData.Tracker.TrackerImei;
                            } else if (displayMember == "SimNumber") {
                                element.value = trackerData.Tracker.SimNumber;
                            }
                        }
                        if (trackerData.isDataEmpty == false) {
                            if (element.id == "trackerLatitude") {
                                element.value = trackerData.Coordinate.latitude.ToString();
                            }
                            if (element.id == "trackerLongitude") {
                                element.value = trackerData.Coordinate.longitude.ToString();
                            }
                            if (element.id == "trackerDegrees") {
                                element.value = trackerData.Degrees.ToString();
                            }
                            if (element.id == "trackerIcon") {
                                element.value = trackerData.Tracker.ImageNumber.ToString();
                            }
                            if (element.id == "trackerIconStatus") {
                                if (trackerData.Speed > 0 && trackerData.ACC == true) {
                                    element.value = "driver";
                                } else if (trackerData.Speed == 0 && trackerData.ACC == false) {
                                    element.value = "stop";
                                } else if (trackerData.Speed == 0 && trackerData.ACC == true) {
                                    element.value = "idle";
                                } else if (trackerData.SOS == true) {
                                    element.value = "alarm";
                                } else {
                                    element.value = "lost";
                                }
                            }
                            if (element.id == "trackerIconAlert") {
                                if (trackerData.Tracker.DateTimeExpired.CompareTo(DateTime.Now) <= 0 /*|| trackerItem.Tracker.VehicleRegistrationExpiry.CompareTo(DateTime.Now) <= 0*/) {
                                    element.value = "alarmExpiry";
                                } else if (trackerData.OverSpeed == true) {
                                    element.value = "alarmOverSpeed";
                                } else if (trackerData.GpsSatellites == 0 || trackerData.GsmSignal == 0) {
                                    element.value = "alarmLostSignal";
                                    //} else if (trackerData.DateTime.Subtract(DateTime.Now) > (new TimeSpan(01, 00, 00))) {
                                    //    element.value = "alarmLostTracker";
                                } else if ((trackerData.Mileage - trackerData.Tracker.MileageInitial) > trackerData.Tracker.MileageLimit) {
                                    element.value = "alarmMileageLimit";
                                } else if (trackerData.Geofence != null) {
                                    element.value = "alarmGeofence";
                                } else {
                                    element.value = "alarmOk!";
                                }
                            }
                        }

                        if (element.id == "trackerIsEnabled") {
                            element.value = trackerItem.IsChecked ? "1" : "0";
                        }
                        if (element.id == "trackerSubmit") {
                            mshtml.HTMLInputElement buttonSubmit = element;
                            buttonSubmit.click();
                        }
                    }
                } catch {

                }
            }
        }

        public void processCommand(WebBrowser webBrowser, MapCommand mapCommand) {
            if (webBrowser != null) {
                try {
                    htmlDocument = (mshtml.IHTMLDocument3)webBrowser.Document;

                    mshtml.IHTMLElementCollection collection = htmlDocument.getElementsByName("formCommand");

                    foreach (mshtml.HTMLInputElement element in collection.item(name: "formCommand")) {

                        if (element.id == "commandId") {
                            element.value = mapCommand.Id.ToString();
                        }
                        if (element.id == "commandName") {
                            element.value = mapCommand.Name;
                        }
                        if (element.id == "commandValue") {
                            element.value = mapCommand.Value;
                        }
                        if (element.id == "commandSubmit") {
                            mshtml.HTMLInputElement btnSubmit = element;
                            btnSubmit.click();
                        }

                    }
                } catch {

                }
            }
        }

    }
}
