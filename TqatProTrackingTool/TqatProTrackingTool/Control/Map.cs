using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using TqatProModel;

namespace TqatProTrackingTool.Control {
    public class Map {

        mshtml.IHTMLDocument3 htmlDocument = null;

        public void loadPoi(WebBrowser webBrowser, Poi poi) {
            if (webBrowser != null) {
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

                    if (element.id == "poiSubmit") {
                        mshtml.HTMLInputElement btnSubmit = element;
                        btnSubmit.click();
                    }

                }
            }
        }

        public void loadTracker(WebBrowser webBrowser, Tracker tracker, TrackerData trackerData) {
            if (webBrowser != null) {

                htmlDocument = (mshtml.IHTMLDocument3)webBrowser.Document;

                mshtml.IHTMLElementCollection collection = htmlDocument.getElementsByName("formTracker");


                foreach (mshtml.HTMLInputElement element in collection.item(name: "formTracker")) {

                    if (element.id == "trackerId") {
                        element.value = trackerData.TrackerId.ToString();
                    }
                    if (element.id == "trackerLabel") {
                        element.value = tracker.VehicleRegistration;
                    }
                    if (element.id == "trackerLatitude") {
                        element.value = trackerData.Coordinate.latitude.ToString();
                    }
                    if (element.id == "trackerLongitude") {
                        element.value = trackerData.Coordinate.longitude.ToString();
                    }
                    if (element.id == "trackerDegrees") {
                        element.value = trackerData.Degrees.ToString();
                    }

                    if (element.id == "trackerSubmit") {
                        mshtml.HTMLInputElement buttonSubmit = element;
                        buttonSubmit.click();
                    }

                }
            }
        }

    }
}
