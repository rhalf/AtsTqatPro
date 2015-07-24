var map;
var polyLines = new Array();
var markers = new Array();
var infoWindows = new Array();
var geofences = new Array();
var geofencesLabels = new Array();
var pois = new Array();
var mapLabels = new Array();




function initialize() {
    var coordinate = new google.maps.LatLng(25.3000, 51.5167);

    var mapProp = {
        center: coordinate,
        zoom: 11,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

    //setInterval(update, 3000);
}

function update() {
    for (var index = 0; index < markers.length; index++) {
        markers[index].setMap(map);
    }

    for (var index = 0; index < polyLines.length; index++) {
        polyLines[index].setMap(map);
    }

    for (var index = 0; index < infoWindows.length; index++) {
        infoWindows[index].open(map);
    }

    for (var index = 0; index < geofences.length; index++) {
        geofences[index].setMap(map);
    }

    for (var index = 0; index < pois.length; index++) {
        pois[index].setMap(map);
    }

    for (var index = 0; index < mapLabels.length; index++) {
        mapLabels[index].setMap(map);
    }
}

google.maps.event.addDomListener(window, 'load', initialize);

