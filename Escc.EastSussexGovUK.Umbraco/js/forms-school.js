if (typeof (jQuery) !== 'undefined' && typeof (jQuery().autocomplete) !== 'undefined') {
    "use strict";
    $(function () {
        let autocompleteOptions = {
            minLength: 2,
            delay: 0,
            source: function (request, response) {
                let element = $(this.element);
                $.getJSON(element.data("autocomplete-schools-url"), { name: element.val() }, response);
            },
            focus: function (event, ui) {
                return false;
            },
            select: function (event, ui) {
                this.value = ui.item.SchoolName;
                document.getElementById(this.id + "-school-code").value = ui.item.SchoolCode;
                return false;
            }
        };

        $("input[data-autocomplete-schools-url]").autocomplete(autocompleteOptions).data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li></li>")
                .append("<a data-school-code=\"" + item.SchoolCode + "\">" + item.SchoolName + "</a>")
                .appendTo(ul);
        };
    })
}