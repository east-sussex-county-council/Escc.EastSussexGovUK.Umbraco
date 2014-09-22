if (typeof (jQuery) != 'undefined') {
    $.getJSON('/alerts/', function(data) {

        var alerts = data;
        alerts = filterByUrl(alerts);
        alerts = filterByCascade(alerts);
        alerts = filterByInherit(alerts);

        if (alerts.length) {
            displayAlerts(alerts);           
        }
    });

    function displayAlerts(alertData) {
        /// <summary>Display an alert on the page</summary>
        var container = $("#main > .container");
        var breadcrumb = $(".breadcrumb, .breadcrumb-mobile", container);

        var alertHtml = '';
        $.each(alertData, function (key, val) {
            alertHtml += val.alert;
        });


        var alertNode = $('<div class="alert">' + alertHtml + '</div>');

        if (breadcrumb.length) {
            alertNode.insertAfter(breadcrumb[breadcrumb.length - 1]);
        } else {
            alertNode.prependTo(container);
        }
    }

    function filterByInherit(alertData) {
        /// <summary>Filters alerts based on their inheritance settings</summary>

        // If inheritance is blocked, that's a new base URL. Anything matching that URL or longer is valid.
        var minimumUrl = '';

        // Start by getting the deepest alert URL which blocks inheritance.
        $.each(alertData, function (key, val) {
            if (!val.append) {
                var minimumUrlCandidate = stripTrailingSlash(val.url);
                if (minimumUrlCandidate.length > minimumUrl.length) minimumUrl = minimumUrlCandidate;
            }
        });

        // If nothing blocks inheritance, return unchanged data
        if (!minimumUrl.length) return alertData;

        // Otherwise go through all the alerts, and select only those matching the new base URL or longer.
        var alerts = [];
        $.each(alertData, function (key, val) {
            var alertUrl = stripTrailingSlash(val.url);
            if (alertUrl.indexOf(minimumUrl) === 0) {
                alerts.push(val);
            }
        });

        return alerts;
    }

    function filterByCascade(alertData) {
        /// <summary>Filters alerts based on their cascade settings</summary>
        var alerts = [];

        $.each(alertData, function(key, val) {
            if (isExactUrlMatch(val) || val.cascade) {
                alerts.push(val);
            }
        });

        return alerts;
    }

    function filterByUrl(alertData) {
        /// <summary>Filters alerts based on whether they start from the current URL or an ancestor</summary>
        var alerts = [];

        $.each(alertData, function(key, val) {
            if (isUrlMatch(val)) {
                alerts.push(val);
            }
        });

        return alerts;
    }

    function isUrlMatch(alertData) {
        /// <summary>Checks whether an alert is displayed starting from the current URL or an ancestor</summary>
        var alertUrl = stripTrailingSlash(alertData.url);
        return (window.location.pathname.indexOf(alertUrl) === 0);
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