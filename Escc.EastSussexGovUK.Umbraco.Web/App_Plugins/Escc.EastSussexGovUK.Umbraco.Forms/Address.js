// This is the controller for /App_Plugins/UmbracoForms/BackOffice/Common/RenderTypes/address.html
// The value of the /Forms/FieldTypes/Address.cs field type is stored as a JSON object. 
// This deserialises it so that its properties are available to the Angular view.
angular.module("umbraco").controller("UmbracoForms.RenderTypes.Address",
    function ($scope) {
        if ($scope.field) {
            $scope.Address = JSON.parse($scope.field.substring($scope.field.indexOf('{')));
        }
    }
);