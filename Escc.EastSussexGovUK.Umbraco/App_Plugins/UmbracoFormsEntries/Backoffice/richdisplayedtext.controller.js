(function() {
    "use strict";

    angular.module("umbraco")
        .controller("umbracoformscontrib.richdisplayedtext.controller",
        [
            "$scope",
            function(scope) {
                scope.model = scope.setting;
                scope.model.config = {
                    editor: {
                        "toolbar": [
                          "removeformat",
                          "cut",
                          "copy",
                          "paste",
                          "bold",
                          "italic",
                          "bullist",
                          "numlist",
                          "link",
                          "unlink",
                          "table"
                        ],
                        "stylesheets": [
                            "/css/TinyMCE-Content.css"
                        ],
                        "dimensions": {
                            "height": 400
                        }
                    }
                };
            }
        ]);

}());