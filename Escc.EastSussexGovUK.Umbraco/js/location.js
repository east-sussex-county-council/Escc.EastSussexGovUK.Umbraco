if (typeof (jQuery) != 'undefined') {
    $(function () {
        if (esccGoogleMaps != 'undefined') esccGoogleMaps.loadGoogleMapsApi({ callback: "esccGoogleMaps.displaySingleMarkerOnAMap" });
    });
}