if (typeof (jQuery) !== 'undefined') {
    "use strict";
    $(function () {
        // If an address field is required, the standard required field validation doesn't work because it's
        // made up of several fields, none of which have the exact id that the validation is looking for.
        //
        // To make it work, there is another hidden field with the correct id and this script copies the values
        // into it. It requires something to be in one of the first 6 fields. Postcode isn't counted because
        // that could still be awaiting a click on 'Find address' or 'Confirm address'.
        $('.umbraco-forms-field.address.mandatory').each(function () {
            var findAddress = this;
            var addressFields = $(".saon,.paon,.street,.locality,.town,.administrative-area", findAddress);
            addressFields.on("change", function () {
                var address = "";
                for (var i = 0; i < addressFields.length; i++) {
                    address += $.trim(addressFields[i].value);
                }
                $(".umbraco-forms-address-validate", findAddress).val(address);
            });
        });
    });
}