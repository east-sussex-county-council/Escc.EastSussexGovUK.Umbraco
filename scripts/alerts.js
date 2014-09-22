if (typeof (jQuery) != 'undefined') {
    $.getJSON('/alerts/', function(data) {

        var alerts = data;
        alerts = filterByUrl(alerts);
        alerts = filterByCascade(alerts);

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

    function filterByCascade(alertData) {
        /// <summary>Checks whether an alert can be displayed based on its cascade settings</summary>
        var alerts = [];

        $.each(alertData, function(key, val) {
            if (isExactUrlMatch(val) || val.cascade) {
                alerts.push(val);
            }
        });

        return alerts;
    }

    function filterByUrl(alertData) {
        /// <summary>Checks whether an alert is displayed starting from the current URL or an ancestor</summary>
        var alerts = [];

        $.each(alertData, function(key, val) {
            var alertUrl = stripTrailingSlash(val.url);
            if (window.location.pathname.indexOf(alertUrl) === 0) {
                alerts.push(val);
            }
        });

        return alerts;
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