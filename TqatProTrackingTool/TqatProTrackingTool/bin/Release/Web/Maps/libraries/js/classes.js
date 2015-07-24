var TqatProObject = { "TRACKER": 1, "POI": 2, "GEOFENCE": 3 };

function Tracker() {
    this.Id = "";
    this.Label = "";
    this.Latitude = "";
    this.Longitude = "";
    this.Icon = "";
    this.IconAlert = "";
    this.Orientation = "";
    this.Longitude = "";
    this.IsChecked = "";
}

function Poi() {
    this.Id = "";
    this.Name = "";
    this.Description = "";
    this.Latitude = "";
    this.Longitude = "";
    this.Icon = "";

}

function Geofence() {
    this.Id = "";
    this.Name = "";
    this.Coordinates = "";
}