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

    function filterByInherit(alertsData) {
        /// <summary>Filters alerts based on their inheritance settings</summary>

        // If inheritance is blocked, that's a new base URL. Anything matching that URL or longer is valid.
        var minimumUrl = '';

        // Start by getting the deepest alert URL which blocks inheritance.
        $.each(alertsData, function (key, singleAlert) {
            if (!singleAlert.append) {
                $.each(singleAlert.urls, function(key2, url) {
                    if (doesUrlMatchCurrentPage(url)) {
                        var minimumUrlCandidate = stripTrailingSlash(url);
                        if (minimumUrlCandidate.length > minimumUrl.length) minimumUrl = minimumUrlCandidate;
                    }
                });
            }
        });

        // If nothing blocks inheritance, return unchanged data
        if (!minimumUrl.length) return alertsData;

        // Otherwise go through all the alerts, and select only those matching the new base URL or longer.
        var alerts = [];
        $.each(alertsData, function (key, singleAlert) {
            $.each(singleAlert.urls, function(key2, url) {
                var alertUrl = stripTrailingSlash(url);
                if (alertUrl.indexOf(minimumUrl) === 0) {
                    alerts.push(singleAlert);
                    return false;
                }
                return true;
            });
        });

        return alerts;
    }

    function filterByCascade(alertsData) {
        /// <summary>Filters alerts based on their cascade settings</summary>
        var alerts = [];

        $.each(alertsData, function(key, singleAlert) {
            if (isExactUrlMatch(singleAlert) || singleAlert.cascade) {
                alerts.push(singleAlert);
            }
        });

        return alerts;
    }

    function filterByUrl(alertsData) {
        /// <summary>Filters alerts based on whether they start from the current URL or an ancestor</summary>
        var alerts = [];

        $.each(alertsData, function(key, singleAlert) {
            if (alertHasUrlMatch(singleAlert)) {
                alerts.push(singleAlert);
            }
        });

        return alerts;
    }

    function alertHasUrlMatch(singleAlert) {
        /// <summary>Checks whether an alert is displayed starting from the current URL or an ancestor</summary>
        var match = false;
        $.each(singleAlert.urls, function (key, url) {
            match = doesUrlMatchCurrentPage(url);
            return !match;
        });
        return match;
    }

    function doesUrlMatchCurrentPage(url) {
        /// <summary>Checks whether a URL matches the current URL or an child page</summary>
        var alertUrl = stripTrailingSlash(url);
        if (window.location.pathname.indexOf(alertUrl) === 0) {
            return true;
        }
        return false;
    }

    function isExactUrlMatch(singleAlert) {
        /// <summary>Checks whether an alert is displayed starting from the current URL</summary>
        var pageUrl = stripTrailingSlash(window.location.pathname);
        var match = false;
        $.each(singleAlert.urls, function (key, url) {
            var alertUrl = stripTrailingSlash(url);
            if (pageUrl === alertUrl) {
                match = true;
                return false;
            }
            return true;
        });
        return match;
    }

    function stripTrailingSlash(str) {
        /// <summary>Trim a trailing / from a string</summary>
        if (str.substr(str.length - 1) == '/') {
            return str.substr(0, str.length - 1);
        }
        return str;
    }
}