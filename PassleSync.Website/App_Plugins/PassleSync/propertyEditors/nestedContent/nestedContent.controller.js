var app = angular.module("umbraco");

app.requires.push('ngSanitize');

app.controller("PassleReadOnlyBooleanController",
    function ($scope, $interpolate) {
        var template = "";
        $scope.values = [];

        // The Property Editor allows for multiple Element Types in the nested content.
        // We only use one though, so it's okay to just take the first contentType for now.
        // The replaceAll is needed for $interpolate to take the given template and get the value from the model.
        if ($scope.model && $scope.model.config && $scope.model.value) {
            if ($scope.model.config.contentTypes && $scope.model.config.contentTypes.length > 0) {
                template = $scope.model.config.contentTypes[0].nameTemplate.replaceAll("{{", "{{value.");
            }

            $scope.values = $scope.model.value.map(v => $interpolate(template)({ value: v }));
        }
    });