if (typeof (jQuery) != 'undefined') {
    $.getJSON('/alerts/', function(data) {
        $.each(data, function(key, val) {
            if (alertURLmatches(val)) {
                displayAlert(val);
            }
        });
    });

    function displayAlert(alertData) {
        /// <summary>Display an alert on the page</summary>
        var container = $("#main > .container");
        var breadcrumb = $(".breadcrumb, .breadcrumb-mobile", container);
        var alertNode = $('<div class="alert">' + alertData.alert + '</div>');

        if (breadcrumb.length) {
            alertNode.insertAfter(breadcrumb[breadcrumb.length - 1]);
        } else {
            alertNode.prependTo(container);
        }
    }

    function alertURLmatches(alertData) {
        /// <summary>Checks whether an alert's URL settings match the current URL</summary>
        return window.location.pathname.indexOf(stripTrailingSlash(alertData.url)) === 0;
    }

    function stripTrailingSlash(str) {
        /// <summary>Trim a trailing / from a string</summary>
        if (str.substr(str.length - 1) == '/') {
            return str.substr(0, str.length - 1);
        }
        return str;
    }
}