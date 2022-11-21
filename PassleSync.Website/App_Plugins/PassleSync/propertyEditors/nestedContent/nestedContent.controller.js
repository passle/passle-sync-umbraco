var app = angular.module("umbraco");

app.requires.push('ngSanitize');

app.controller("PassleReadOnlyBooleanController",
    function ($scope, $interpolate) {
        var template = "";
        $scope.values = [];

        if ($scope.model && $scope.model.config && $scope.model.value) {
            if ($scope.model.config.contentTypes && $scope.model.config.contentTypes.length > 0) {
                template = $scope.model.config.contentTypes[0].nameTemplate.replaceAll("{{", "{{value.");
            }

            $scope.values = $scope.model.value.map(v => $interpolate(template)({ value: v }));
        }
    });