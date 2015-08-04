var map;
var polyLines = new Array();
var trackers = new Array();
var infoWindows = new Array();
var geofences = new Array();
var geofencesLabels = new Array();
var pois = new Array();
var mapLabels = new Array();




function initialize() {
    var coordinate = new google.maps.LatLng(25.2606, 51.4431);

    var mapProp = {
        center: coordinate,
        zoom: 12,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
}

google.maps.event.addDomListener(window, 'load', initialize);

