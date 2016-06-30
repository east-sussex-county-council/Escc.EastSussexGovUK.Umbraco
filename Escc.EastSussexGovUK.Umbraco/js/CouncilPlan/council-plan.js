$(function () {
    // React to browser being resized. 
    var onSmallAndMediumChange = function (widthBefore, widthNow) {
        // If window width has increased from small to medium, do nothing. If it's decreased from large to medium, carry on.
        if (widthBefore && widthBefore <= widthNow) return;

        // For small and medium sizes, hide the menu and display a link to toggle it on and off
        var nav = $(".plan-nav");
        var toggle = $("#toggle-plan-nav");

        // Get the name of the current plan from the breadcrumb trail
        var plan = $(".breadcrumb .current").html().replace(/<[^>]+>/gi, '');
        var prefix = plan.lastIndexOf(":");
        if (prefix > -1 && plan.length > (prefix + 3)) {
            plan = plan.substring(prefix + 2);
        }

        // If the toggle doesn't exist yet, create it
        var show = 'Show ' + plan + ' sections &#9660;', hide = 'Hide ' + plan + ' sections &#9650;';
        if (!toggle.length) {
            toggle = $('<a href="#" id="toggle-plan-nav">' + show + '</a>').insertBefore(nav).click(function (e) {
                e.preventDefault();
                $(this).html(nav.is(":visible") ? show : hide);
                nav.slideToggle();
            })
                .wrap('<p class="menu-toggle small medium">');
        }

        toggle.show();
        toggle.html(show);
        nav.hide();
    };

    var onLargeChange = function () {
        // For large sizes, always show the menu and hide the redundant menu toggle
        $("#toggle-plan-nav").hide();
        $(".plan-nav").show();
    };

    onEsccBreakpointChange(onSmallAndMediumChange,
        onSmallAndMediumChange,
        onLargeChange,
    // Not sure why medium-large breakpoint is 784, should be 802. May be significant that 784 is (802 - 18), ie one gutter width.
        null, 784);
});