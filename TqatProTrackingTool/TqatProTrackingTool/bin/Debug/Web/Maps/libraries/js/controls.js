function parse(objectType) {
    var parser = new Parser();
    var mapTool = new MapTool();

    switch (objectType) {
        case "POI":
            var poi = parser.getPoi();
            mapTool.addPoi(poi);
            break;
        case "TRACKER":
            setTimeout(function () {
                var tracker = parser.getTracker();
                mapTool.addTracker(tracker);
            }, 0);
            break;
        case "GEOFENCE":
            var geofence = parser.getGeofence();
            mapTool.addGeofence(geofence);
            break;
        case "COMMAND":
            var command = parser.getCommand();
            mapTool.processCommand(command);
            break;
    }
}

function Parser() {
    this.getPoi = function () {
        poi = new Poi();
        poi.Id = document.getElementById("poiId").value;
        poi.Latitude = document.getElementById("poiLatitude").value;
        poi.Longitude = document.getElementById("poiLongitude").value;
        poi.Name = document.getElementById("poiName").value;
        poi.Icon = document.getElementById("poiIcon").value;

        return poi;
    }
    this.getTracker = function () {
        tracker = new Tracker();
        tracker.Id = document.getElementById("trackerId").value;
        tracker.Latitude = document.getElementById("trackerLatitude").value;
        tracker.Longitude = document.getElementById("trackerLongitude").value;
        tracker.Label = document.getElementById("trackerLabel").value;
        tracker.Degrees = document.getElementById("trackerDegrees").value;
        tracker.Icon = document.getElementById("trackerIcon").value;
        tracker.IconAlert = document.getElementById("trackerIconAlert").value;
        tracker.IsEnabled = document.getElementById("trackerIsEnabled").value;
        tracker.IconStatus = document.getElementById("trackerIconStatus").value;

        return tracker;
    }
    this.getGeofence = function () {
        geofence = new Geofence();
        geofence.Id = document.getElementById("geofenceId").value;
        geofence.Coordinates = document.getElementById("geofenceCoordinates").value;
        geofence.Name = document.getElementById("geofenceName").value;
        return geofence;
    }
    this.getCommand = function () {
        command = new Command();
        command.Id = document.getElementById("commandId").value;
        command.Name = document.getElementById("commandName").value;
        command.Value = document.getElementById("commandValue").value;
        return command;
    }
}


function MapTool() {
    this.addPoi = function (poi) {
        var coordinate = new google.maps.LatLng(poi.Latitude, poi.Longitude);

        var image = {
            //url: 'images/poi/building_01.png',
            url: 'images/poi/' + poi.Icon + '.png',
            size: new google.maps.Size(32, 32),
            scaledSize: new google.maps.Size(32, 32),
            origin: new google.maps.Point(0, 0),
            anchor: new google.maps.Point(16, 32),
            scale: .25,
            rotation: 0
        }

        var markerPoi = new MarkerWithLabel({
            position: coordinate,
            map: map,
            icon: image,
            labelContent: poi.Name,
            labelAnchor: new google.maps.Point(32, 0),
            draggable: false,
            labelClass: "markerPoi",
        });

        pois.push(markerPoi);

    }

    this.addGeofence = function () {
        //alert(geofence.Coordinates);


        var coordinates = JSON.parse(geofence.Coordinates);
        var polygonCoordinates = new Array();
        for (var index = 0; coordinates.length > index; index++) {
            polygonCoordinates.push(new google.maps.LatLng(coordinates[index].Latitude, coordinates[index].Longitude));
        }

        var markerGeofence = new google.maps.Polygon({
            paths: polygonCoordinates,
            map: map,
            strokeColor: '#0000FF',
            strokeOpacity: 0.6,
            strokeWeight: 1,
            fillColor: '#0000FF',
            fillOpacity: 0.1
        });

        var bounds = new google.maps.LatLngBounds();

        for (index = 0; index < polygonCoordinates.length; index++) {
            bounds.extend(polygonCoordinates[index]);
        }

        var labelGeofence = new MarkerWithLabel({
            position: bounds.getCenter(),
            map: map,
            icon: {
                url: ""
            },
            labelContent: geofence.Name,
            labelAnchor: new google.maps.Point(32, 0),
            draggable: false,
            labelClass: "markerGeofence",
        });

        geofences.push(markerGeofence);
        geofencesLabels.push(labelGeofence);
    }
    this.addTracker = function () {
        //alert(tracker.IconAlert);
        var coordinate = new google.maps.LatLng(tracker.Latitude, tracker.Longitude);


        if (tracker.IsEnabled != 1) {
            for (var index in markers) {
                if (markers[index].trackerId == tracker.Id) {
                    markers[index].setMap(null);
                    delete markers[index];
                    return;
                }
            }
            return;
        }



        var markerTracker =
       '<div class="divTracker">' +
           '<img class="imgTrackerAlarm" src="images/alarm/' + tracker.IconAlert + '.png"/>' +
           '<img class="imgTrackerVehicle" src="images/tracker/icon_' + tracker.Icon + '_' + tracker.IconStatus + '.gif" style="display:block;transform:rotate(' + tracker.Degrees + 'deg)";/>' +
           '<label class="labelTracker">' + tracker.Label + '</label>' +
        '</div>';

        for (var index in markers) {
            if (markers[index].trackerId == tracker.Id) {
                markers[index].setPosition(coordinate);
                markers[index].set('labelContent', markerTracker);
                return;
            }
        }



        //var div = document.createElement('div');
        //div.className = "divTracker";

        //var imgAlarm = document.createElement('img');
        //imgAlarm.className = "imgTrackerAlarm";

        //var imgTracker = document.createElement('img');
        //imgTracker.src = "images/tracker/icon_0_driver.gif";
        //imgTracker.className = "imgTracker";

        //var labelTracker = document.createElement('label');
        //labelTracker.textContent = tracker.Label;
        //labelTracker.className = "labelTracker";

        //div.appendChild(imgAlarm);
        //div.appendChild(imgTracker);
        //div.appendChild(labelTracker);


        var markerTracker = new MarkerWithLabel({
            position: coordinate,
            map: map,
            icon: {
                url: ""
            },
            labelContent: markerTracker,
            labelAnchor: new google.maps.Point(32, 0),
            draggable: false,

            labelClass: "markerTracker",

            trackerId: tracker.Id
        });

        markers.push(markerTracker);
    }
    this.processCommand = function (command) {

        switch (command.Name) {
            case "ClearPoi": {
                for (var index = 0; index < pois.length; index++) {
                    if (pois[index] != null) {
                        pois[index].setMap(null);
                        delete pois[index];
                    }
                }
                break;
            }
            case "ClearGeofence": {
                for (var index = 0; index < geofences.length; index++) {
                    if (geofences[index] != null) {
                        geofences[index].setMap(null);
                        geofencesLabels[index].setMap(null);
                        delete geofences[index];
                        delete geofencesLabels[index];
                    }
                }
                break;
            }
            case "SetFocus": {
                var coordinate = JSON.parse(command.Value);
                map.setCenter(new google.maps.LatLng(coordinate[0].Latitude, coordinate[0].Longitude));
                break;
            }
        }
    }




    //function rotator(options) {

    //    var a = options.delay;
    //    var b = options.media;
    //    var mediaArr = [];

    //    for (var i = 0, j = b.length; i < j; i++) {
    //        mediaArr.push(b[i].img);
    //    }

    //    document.write('<div id="rotatorContainer"></div>');
    //    var container = document.getElementById('rotatorContainer');
    //    var Start = 0;

    //    rotatorCore();

    //    function rotatorCore() {
    //        Start = Start + 1;

    //        if (Start >= mediaArr.length)
    //            Start = 0;
    //        container.innerHTML = mediaArr[Start];
    //        setTimeout(rotatorCore, a);

    //    }

}

