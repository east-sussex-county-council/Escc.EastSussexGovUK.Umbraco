if (typeof(jQuery) !== 'undefined' && typeof (ga) !== 'undefined') {
    // Add event tracking on the 'Apply' button
    $("a.apply").click(function () { ga('send', 'event', 'job', "apply", $("#job-reference").html(), 0); });
    $("a.apply-mobile").click(function () { ga('send', 'event', 'job', "apply (mobile button)", $("#job-reference").html(), 0); });
    $(".login-continue a").click(function () { ga('send', 'event', 'job', "continue application", $("#job-reference").html(), 0); });
}