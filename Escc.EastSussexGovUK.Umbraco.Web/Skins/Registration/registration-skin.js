if (typeof (jQuery) !== 'undefined') {
    $(function () {
        var domain = (document.location.hostname === "localhost") ? "" : "https://www.eastsussex.gov.uk";
        $('<img src="' + domain + '/skins/registration/registration-skin.png" class="content-small content-medium supporting registration-logo" alt="East Sussex Registration logo" />').insertBefore($(".article").first());
    });
}