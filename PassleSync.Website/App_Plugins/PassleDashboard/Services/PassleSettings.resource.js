angular.module("umbraco.resources").factory("passleSettingsResource",
    function ($q, $http, umbRequestHelper) {
        var baseUrl = "backoffice/PassleSync/PassleSync/";
        return {
            get: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get(baseUrl + "Get"),
                    "Failed to fetch the settings");
            },
        };
    }
);
