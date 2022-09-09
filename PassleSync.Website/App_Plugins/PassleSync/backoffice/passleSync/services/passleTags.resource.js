angular.module("umbraco.resources").factory("passleTagsResource",
    function ($q, $http, umbRequestHelper) {
        var baseUrl = "backoffice/PassleSync/PassleDashboardTags/";
        return {
            getAll: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get(baseUrl + "GetAll"),
                    "Failed to retrieve a list of all tags");
            },
        };
    }
);
