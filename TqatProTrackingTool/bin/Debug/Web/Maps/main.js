var map;
var polyLines = new Array();
var trackers = new Array();
var infoWindows = new Array();
var geofences = new Array();
var geofencesLabels = new Array();
var pois = new Array();
var mapLabels = new Array();
var coordinateCenter = new google.maps.LatLng(25.260610, 51.493537);
var zoomLevel = 11;


function initialize() {
    

    var mapProp = {
        center: coordinateCenter,
        zoom: zoomLevel,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
}

google.maps.event.addDomListener(window, 'load', initialize);

