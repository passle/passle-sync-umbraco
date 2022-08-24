angular.module("umbraco.resources").factory("passleSettingsResource",
    function ($q, $http, umbRequestHelper) {
        var baseUrl = "backoffice/PassleSync/PassleDashboardSettings/";
        return {
            get: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get(baseUrl + "Get"),
                    "Failed to fetch the settings");
            },
            save: function (settings) {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "Save", settings),
                    "Failed to save the settings");
            },
        };
    }
);
