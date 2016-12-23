if (typeof (jQuery) !== 'undefined') {
    $(function() {
        "use strict";

        var feedLinks = $(".embed.rss a");
        $.get(feedLinks[0].href + "?altTemplate=JobsRssAsTable", function (html) {
            feedLinks.parent().replaceWith(html);
        });
    });
}