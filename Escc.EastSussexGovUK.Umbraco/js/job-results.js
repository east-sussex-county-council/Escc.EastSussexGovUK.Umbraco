if (typeof(jQuery) !== 'undefined') {
    $(function() {
        "use strict";

        // Validate the email alerts form to make it harder to send bad data to the job alerts service
        $("form.job-alerts").submit(function () {
            var errors = [];
            if (!$("input[name=vDeliveryFrequency]:checked").length) {
                errors.push("Please select how often you want to get alerts");
            }
            if (!$("input[name=dExpirationDate]:checked").length) {
                errors.push("Please select how long you want to get alerts");
            }
            if (!$("#semail").val()) {
                errors.push("Please enter the address you would like alerts to be sent to");
            }
            var validEmail = new RegExp("^[0-9A-Za-z'\._-]{1,127}@[0-9A-Za-z'\.\-_]{1,127}$");
            if (!validEmail.test($("#semail").val())) {
                errors.push("Please enter a valid email address to send the alerts to");
            }

            if (errors.length) {
                var errorList = $("<ul>");
                for (var i = 0; i < errors.length; i++) {
                    errorList.append($("<li>").text(errors[i]));
                }
                $(".job-alert-errors").remove();
                $('<div class="errorSummary job-alert-errors">').append(errorList).insertBefore("form.job-alerts");
                return false;
            }

            return true;
        });

    });
}