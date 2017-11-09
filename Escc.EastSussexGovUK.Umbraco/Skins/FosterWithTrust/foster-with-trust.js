if (typeof (jQuery) !== 'undefined') {
    $(function () {
        // Load two copies of the logo for different resolutions. The <picture> element or srcset attribute could handle this better, but the design wouldn't work in IE11.
        var domain = (document.location.hostname === "localhost") ? "" : "https://www.eastsussex.gov.uk";
        $('<img src="' + domain + '/skins/fosterwithtrust/foster-with-trust-small.png" alt="Foster with trust" class="foster-with-trust-logo small content" />' +
          '<img src="' + domain + '/skins/fosterwithtrust/foster-with-trust.png" alt="Foster with trust" class="foster-with-trust-logo medium large content" />' +
          '<p class="foster-with-trust-call"><span class="aural">Call us: </span>01323 464129</p>').insertAfter($("#1_smallCrumb"));
    });
}