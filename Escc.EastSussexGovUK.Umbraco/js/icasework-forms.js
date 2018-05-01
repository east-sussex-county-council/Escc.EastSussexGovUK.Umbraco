if (typeof (jQuery) !== 'undefined' && typeof (iFrameResize) !== 'undefined') {
    "use strict";

    $(function () {
        var iCaseWork = new RegExp(/^(https?:\/\/[A-Za-z0-9_-]+\.icasework\.com\/form\?[A-Za-z0-9=&_-]+)$/);
        $("a.embed").filter(function (index) {
            return iCaseWork.test(this.href);
        }).each(function () {
            // Swap iCaseWork link for embedded form
            var match = iCaseWork.exec(this.href);
            var target = $(this);
            if (target.parent()[0].tagName == "P" && target.parent().children().length == 1) { // this test misses text nodes, but the link should be on its own line
                target = target.parent();
            }
            target.replaceWith('<iframe src="' + match[1] + '" scrolling="no" class="icasework"></iframe>');
        });
        iFrameResize({ checkOrigin: false });
    });
}