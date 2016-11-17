if (typeof(jQuery) !== 'undefined') {
    $(function() {
        "use strict";

        // Load filters
        var id = $("[data-jobs-id]").data("jobs-id");
        var mask = $("[data-jobs-mask]").data("jobs-mask");
        $.get("/umbraco/api/talentlinkapi/searchfieldshtml?ID=" + encodeURIComponent(id) + "&mask=" + encodeURIComponent(mask), function(data) {
            $(".job-filters").html(data);
        });

        // Wire up alerts button
        $(".job-alerts input[type=submit]").click(function (e) {

            this.form.action = $("[data-alerts-url]").data("alerts-url");

            $(this.form).append('<input type="hidden" name="sagsubmit" value="Save" />')
                        .append('<input type="hidden" name="backpage" value="1" />')
                        .append('<input type="hidden" name="Create" value="1" />');
        });
    });
}