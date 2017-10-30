if (typeof (jQuery) !== 'undefined') {
    $(function () {
        var domain = (document.location.hostname === "localhost") ? "" : "https://www.eastsussex.gov.uk";
        $('<div class="content-small content-medium supporting foster-with-trust-logo"><img src="' + domain + '/skins/fosterwithtrust/foster-with-trust.png" alt="Foster with trust" /></div>').insertBefore($(".article").first());
    });
}