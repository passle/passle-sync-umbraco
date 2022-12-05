angular.module("umbraco")
    .controller("PassleReadOnlyTagsController",
        function ($scope) {
            $scope.tags = $scope.model.value;
        });