if (typeof (jQuery) != 'undefined') {
    $(function () {
        if (esccGoogleMaps != 'undefined') esccGoogleMaps.loadGoogleMapsApi({ callback: "esccGoogleMaps.displaySingleMarkerOnAMap" });
        contentPanels({ className: ".accordion", slide: true, allowCollapse: true, expandFirst: false });
        contentPanels({ className: ".tablist", slide: false, allowCollapse: false, expandFirst: true });

        function contentPanels(options) {
            // Note: adding the .selected class is essential for IE8, which doesn't update styles when the aria-selected attribute is changed.
            // Changing the className property, in any way, forces an update.

            // Get the key elements
            var container = $(options.className);
            var triggers = $("[role=tab]", container);
            var panels = $("[role=tabpanel]", container);

            // Hide panels by default
            var toHide = options.expandFirst ? panels.not(panels.first()) : panels;
            toHide.each(function () { $(this).hide().attr("aria-hidden", true); });

            triggers.attr("tabindex", 0).attr("aria-selected", false).removeClass("selected");
            if (options.expandFirst) triggers.first().attr("tabindex", -1).attr("aria-selected", true).addClass("selected");

            // Expand and focus on a panel when an event is triggered on its tab
            var expandPanel = function (e) {
                var toClose = panels.filter(":visible").attr("aria-hidden", true);
                var toOpen = panels.filter("[aria-labelledby=" + e.target.id + "]").attr("aria-hidden", false);

                if (options.slide) {
                    toClose.slideUp();
                    toOpen.slideDown();
                } else {
                    toClose.hide();
                    toOpen.show();
                }

                triggers.attr("tabindex", 0).attr("aria-selected", false).removeClass("selected");
                if (!options.allowCollapse) $(e.target).attr("tabindex", -1);
                $(e.target).attr("aria-selected", true).addClass("selected");
            }

            // Collapse a panel when an event is triggered on its tab
            var collapsePanel = function (e) {
                if (!options.allowCollapse) return;

                var toClose = panels.filter("[aria-labelledby=" + e.target.id + "]").attr("aria-hidden", true);

                if (options.slide) {
                    toClose.slideUp();
                } else {
                    toClose.hide();
                }

                $(e.target).attr("tabindex", 0).attr("aria-selected", false);
            }

            // When the trigger element gets clicked, toggle the visibility of its panel 
            container.click(function(e) {
                if ($(e.target).attr("role") === "tab") {
                    if (panels.filter("[aria-labelledby=" + e.target.id + "]").is(":visible")) {
                        collapsePanel(e);
                    } else {
                        expandPanel(e);
                    }
                }
            })
            // When a key is pressed, if the focus is on a trigger element and its Enter or an arrow key, expand or collapse as appropriate
            .keydown(function (e) {
                if ($(e.target).attr("role") === "tab") {
                    if (panels.filter("[aria-labelledby=" + e.target.id + "]").is(":visible")) {
                        if (e.keyCode === 13 || e.keyCode === 37 || e.keyCode === 38) {
                            collapsePanel(e);
                            e.preventDefault();
                        }
                    } else {
                        if (e.keyCode === 13 || e.keyCode === 39 || e.keyCode === 40) {
                            expandPanel(e);
                            e.preventDefault();
                        }
                    }
                }
            });
        }
    });
}