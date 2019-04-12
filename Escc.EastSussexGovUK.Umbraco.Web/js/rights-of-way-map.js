if (typeof (jQuery) !== 'undefined') {
        function displayMarkers() {

            var element = $("#map");
            if (!element.length) return;
            if (!locations || !locations.length) return;

            // Ensure we have locations with geo data before going any further
            var locationsWithCoordinates = [];
            for (var i = 0; i < locations.length; i++) {
                if (locations[i].geo.latitude || locations[i].geo.longitude) {
                    locationsWithCoordinates.push(locations[i]);
                }
            }
            if (!locationsWithCoordinates.length) return;

            var sectionHeading = $("<h2>Map of the area affected</h2>" +
                "<p>For a detailed map of the site, download the associated documents.</p>").insertBefore(element);
            element.addClass("deposits-map");

            // Set up a map centred on the mid-point of all the locations
            var minLatitude, maxLatitude, minLongitude, maxLongitude;

            for (i = 0; i < locationsWithCoordinates.length; i++) {
                locationsWithCoordinates[i].geo.latitude = parseFloat(locationsWithCoordinates[i].geo.latitude);
                locationsWithCoordinates[i].geo.longitude = parseFloat(locationsWithCoordinates[i].geo.longitude);
                if (!minLatitude || locationsWithCoordinates[i].geo.latitude < minLatitude) minLatitude = locationsWithCoordinates[i].geo.latitude;
                if (!maxLatitude || locationsWithCoordinates[i].geo.latitude > maxLatitude) maxLatitude = locationsWithCoordinates[i].geo.latitude;
                if (!minLongitude || locationsWithCoordinates[i].geo.longitude < minLongitude) minLongitude = locationsWithCoordinates[i].geo.longitude;
                if (!maxLongitude || locationsWithCoordinates[i].geo.longitude > maxLongitude) maxLongitude = locationsWithCoordinates[i].geo.longitude;
            }

            var latDiff = maxLatitude - minLatitude;
            var longDiff = maxLongitude - minLongitude;
            var centre = new google.maps.LatLng(minLatitude + latDiff, minLongitude + longDiff);

            var map = new google.maps.Map(element[0], { center: centre });

            // In case there are multiple locations some distance apart, recalculate the bounds to include all locations.
            // But first, check that this won't zoom us in too far (which would happen for a single point), and tweak the 
            // data to impose a maximum zoom level.
            if (latDiff < .01) {
                minLatitude -= .005;
                maxLatitude += .005;
            }

            var sw = new google.maps.LatLng(minLatitude, minLongitude);
            var ne = new google.maps.LatLng(maxLatitude, maxLongitude);
            var bounds = new google.maps.LatLngBounds(sw, ne);
            map.fitBounds(bounds);

            // Place a marker on the map for each location
            for (i = 0; i < locationsWithCoordinates.length; i++) {
                var marker = new google.maps.Marker({
                    position: new google.maps.LatLng(locationsWithCoordinates[i].geo.latitude, locationsWithCoordinates[i].geo.longitude),
                    map: map,
                    title: locationsWithCoordinates[i].address
                });
            }

            // Centre the map on resize, as the changing aspect ratio makes the marker wander off-centre, and sometimes out of view
            var mapResizeTimeout;
            $(window).resize(function () {
                clearTimeout(mapResizeTimeout);
                mapResizeTimeout = setTimeout(centreMap, 50);
            });

            function centreMap() {
                map.setCenter(centre);
            }
        }

    $(function () {
        if (esccGoogleMaps !== 'undefined') esccGoogleMaps.loadGoogleMapsApi({ callback: "displayMarkers" });
    });
}