if (typeof(jQuery) != 'undefined') {
    $(function () {
        // Because Umbraco preview loads in an iframe over HTTPS, it won't frame an HTTP page because it's insecure (mixed content ).
        // Rather than having links to HTTP pages lead to a blank iframe, use a frame-busting script to break out of the iframe, which lets authors test that their links work.
        $("a[href^='http:']").attr("target", "_top");
    });
}