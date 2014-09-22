if (typeof (jQuery) != 'undefined') {
    $.getJSON('/alerts/', function(data) {
        $.each(data, function(key, val) {
            if (isUrlMatch(val) && isCascadeMatch(val)) {
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

    function isCascadeMatch(alertData) {
        /// <summary>Checks whether an alert can be displayed based on its cascade settings</summary>
        return (isExactUrlMatch(alertData) || alertData.cascade);
    }

    function isUrlMatch(alertData) {
        /// <summary>Checks whether an alert is displayed starting from the current URL or an ancestor</summary>
        var alertUrl = stripTrailingSlash(alertData.url);
        return window.location.pathname.indexOf(alertUrl) === 0;
    }

    function isExactUrlMatch(alertData) {
        /// <summary>Checks whether an alert is displayed starting from the current URL</summary>
        var pageUrl = stripTrailingSlash(window.location.pathname);
        var alertUrl = stripTrailingSlash(alertData.url);
        return (pageUrl === alertUrl);
    }

    function stripTrailingSlash(str) {
        /// <summary>Trim a trailing / from a string</summary>
        if (str.substr(str.length - 1) == '/') {
            return str.substr(0, str.length - 1);
        }
        return str;
    }
}