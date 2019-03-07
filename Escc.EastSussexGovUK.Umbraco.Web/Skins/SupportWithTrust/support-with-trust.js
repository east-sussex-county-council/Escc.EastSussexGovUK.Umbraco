if (typeof (jQuery) !== 'undefined') {
    $(function () {
        // Load two copies of the logo for different resolutions. The <picture> element or srcset attribute could handle this better, but the design wouldn't work in IE11.
        var domain = (document.location.hostname === "localhost") ? "" : "https://www.eastsussex.gov.uk";
        $('<img src="' + domain + '/skins/supportwithtrust/support-with-trust-small.png" alt="Support with trust" class="support-with-trust-logo small content" />' +
          '<img src="' + domain + '/skins/supportwithtrust/support-with-trust-medium.png" alt="Support with trust" class="support-with-trust-logo medium large content" />' +
            '<p class="support-with-trust-call"><span class="aural">Call us: </span>01424 726155</p>').insertAfter($(".breadcrumb-mobile").parent());
    });
}