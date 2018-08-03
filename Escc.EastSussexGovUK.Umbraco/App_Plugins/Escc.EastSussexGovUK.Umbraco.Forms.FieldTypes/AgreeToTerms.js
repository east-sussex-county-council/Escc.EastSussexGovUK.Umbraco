(function() {
    "use strict";

    angular.module("umbraco")
        .controller("umbracoformscontrib.richdisplayedlinks.controller",
        [
            "$scope",
            function (scope) {
                scope.model = scope.setting;
                scope.model.config = {
                    editor: {
                        "toolbar": [
                            "link",
                            "unlink"
                        ],
                        "stylesheets": [],
                        "dimensions": {
                            "height": 50
                        }
                    }
                };
            }
        ]);
}());