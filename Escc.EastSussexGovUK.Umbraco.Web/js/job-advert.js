if (typeof (jQuery) !== 'undefined' && typeof (ga) !== 'undefined') {
    let eventLabel = $("#job-reference").html() + '/' + $("#department").html();
    $("a.apply").click(function () { ga('send', 'event', 'job', "apply", eventLabel, 0); });
    $("a.apply-mobile").click(function () { ga('send', 'event', 'job', "apply (mobile button)", eventLabel, 0); });
    $("a.JD-DocumentName").click(function () { ga('send', 'event', 'job', "download job description", eventLabel, 0); });
    $(".login-continue a").click(function () { ga('send', 'event', 'job', "continue application", eventLabel, 0); });
    $(".share a[href*='friend.aspx']").click(function () { ga('send', 'event', 'job', "share by email", eventLabel, 0); });
    $(".share a[href^='https://www.facebook.com']").click(function () { ga('send', 'event', 'job', "share on Facebook", eventLabel, 0); });
    $(".share a[href^='https://twitter.com']").click(function () { ga('send', 'event', 'job', "share on Twitter", eventLabel, 0); });
    $(".share a[href^='https://www.linkedin.com']").click(function () { ga('send', 'event', 'job', "share on LinkedIn", eventLabel, 0); });
}