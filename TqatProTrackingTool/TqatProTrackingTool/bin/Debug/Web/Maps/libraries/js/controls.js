function parse(objectType) {
    var parser = new Parser();
    var mapTool = new MapTool();

    switch (objectType) {
        case "POI":
            var poi = parser.getPoi();
            mapTool.addPoi(poi);
            break;
        case "TRACKER":
            var tracker = parser.getTracker();
            mapTool.addTracker(tracker);
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
        return poi;
    }
    this.getTracker = function () {
        tracker = new Tracker();
        tracker.Id = document.getElementById("trackerId").value;
        tracker.Latitude = document.getElementById("trackerLatitude").value;
        tracker.Longitude = document.getElementById("trackerLongitude").value;
        tracker.Label = document.getElementById("trackerLabel").value;
        tracker.Degrees = document.getElementById("trackerDegrees").value;
        return tracker;
    }
}


function MapTool() {
    this.addPoi = function (poi) {
        var coordinate = new google.maps.LatLng(poi.Latitude, poi.Longitude);

        var image = {
            url: 'images/poi/building_01.png',
            size: new google.maps.Size(32, 32),
            scaledSize: new google.maps.Size(32, 32),
            origin: new google.maps.Point(0, 0),
            anchor: new google.maps.Point(16, 32),
            scale: .25,
            rotation: 0
        };

        var shape = {
            coords: [32, 32, 32, 32],
            type: 'poly'
        };

        var markerPoi = new MarkerWithLabel({
            position: coordinate,
            map: map,
            icon: image,
            labelContent: poi.Name,
            labelAnchor: new google.maps.Point(32, 0),
            draggable: false,
            labelClass: "markerPoi",
            shape: shape
        });


        pois.push(markerPoi);

    }
    this.addTracker = function () {
        var coordinate = new google.maps.LatLng(tracker.Latitude, tracker.Longitude);

        var markerTracker =
       '<div class="divTracker">' +
           '<img class="imgTrackerAlarm" src="images/tracker/icon_0_driver.gif"/>' +
           '<img class="imgTrackerVehicle" src="images/tracker/icon_0_driver.gif" style="display:block;transform:rotate(' + tracker.Degrees + 'deg)";/>' +
           '<label class="labelTracker">' + tracker.Label + '</label>'
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

       


        var shape = {
            coords: [32, 32, 32, 32],
            type: 'poly'
        };

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
            shape: shape,
            trackerId: tracker.Id
        });

        markers.push(markerTracker);
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

//}

