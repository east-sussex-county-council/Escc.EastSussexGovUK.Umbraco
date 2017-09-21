if (typeof(jQuery) !== 'undefined' && typeof (ga) !== 'undefined') {
    $("a.apply").click(function () { ga('send', 'event', 'job', "apply", $("#job-reference").html(), 0); });
    $("a.apply-mobile").click(function () { ga('send', 'event', 'job', "apply (mobile button)", $("#job-reference").html(), 0); });
    $("a.JD-DocumentName").click(function () { ga('send', 'event', 'job', "download job description", $("#job-reference").html(), 0); });
    $(".login-continue a").click(function () { ga('send', 'event', 'job', "continue application", $("#job-reference").html(), 0); });
    $(".share a[href*='friend.aspx']").click(function () { ga('send', 'event', 'job', "share by email", $("#job-reference").html(), 0); });
    $(".share a[href^='https://www.facebook.com']").click(function () { ga('send', 'event', 'job', "share on Facebook", $("#job-reference").html(), 0); });
    $(".share a[href^='https://twitter.com']").click(function () { ga('send', 'event', 'job', "share on Twitter", $("#job-reference").html(), 0); });
    $(".share a[href^='https://www.linkedin.com']").click(function () { ga('send', 'event', 'job', "share on LinkedIn", $("#job-reference").html(), 0); });
}