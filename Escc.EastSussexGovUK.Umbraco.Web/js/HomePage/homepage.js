$(function ()
{
    /* Wire-up tabs */
    if (jQuery.fn.tabs) $("#rap").tabs();

    /* Track which bits of news layout are clicked, even though destination URLs are changing */
    if (typeof(ga) !== 'undefined')
    {
        $(".only-feature a").click(function () { ga('send', 'event', 'home', "clicked only feature", $(this).attr("href"), 0); });
        $(".feature1 a").click(function () { _ga('send', 'event', 'home', "clicked first feature", $(this).attr("href"), 0); });
        $(".feature2 a").click(function () { ga('send', 'event', 'home', "clicked second feature", $(this).attr("href"), 0); });
    }
});