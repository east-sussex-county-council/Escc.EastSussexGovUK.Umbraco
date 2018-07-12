if (typeof (jQuery) !== 'undefined') {
    $(function() {
        "use strict";

        function addParameterToQueryString(url, parameters) {
            // Based on https://samaxes.com/2011/09/change-url-parameters-with-jquery/

            /*
             * queryParameters -> handles the query string parameters
             * queryString -> the query string without the first '?' character
             * re -> the regular expression
             * m -> holds the string matching the regular expression
             */
            var queryPos = url.indexOf("?"), queryParameters = {}, re = /([^&=]+)=([^&]*)/g, m;
            var queryString = (queryPos > -1) ? url.substring(queryPos + 1) : "";
            url = (queryPos > -1) ? url.substring(0, queryPos) : url;

            // Creates a map with the query string parameters
            while (m = re.exec(queryString)) {
                queryParameters[decodeURIComponent(m[1])] = decodeURIComponent(m[2].replace(/\+/g, ' '));
            }

            // Add new parameters or update existing ones
            for (var i = 0; i < parameters.length; i++) {
                queryParameters[parameters[i].key] = parameters[i].value.replace(/\+/g,' ');
            };

            /*
             * Replace the query portion of the URL.
             * jQuery.param() -> create a serialized representation of an array or
             *     object, suitable for use in a URL query string or Ajax request.
             */
            return url + "?" + $.param(queryParameters);
        }

        // Look for embedded links to RSS feeds, load the linked URL using a template for embedding and replace it with the resulting HTML
        var templateParams = [{ key: 'altTemplate', value: 'JobsRssAsTable' }];
        $(".rss.embed-jobs-rss a").each(function() {
            this.href = addParameterToQueryString(this.href, templateParams);
            var that = $(this);
            $.get(this.href, function (html) {
                that.parent().replaceWith(html);
            });
        });
    });
}