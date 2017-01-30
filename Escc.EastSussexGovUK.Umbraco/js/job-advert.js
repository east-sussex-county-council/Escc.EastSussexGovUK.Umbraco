if (typeof(jQuery) !== 'undefined' && typeof (ga) !== 'undefined') {
    // Add event tracking on the 'Apply' button
    $(".main-action a").click(function () { ga('send', 'event', 'job', "apply", $("#job-reference").html(), 0); });
}