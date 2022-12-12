angular.module("umbraco.resources").factory("passleAuthorsResource",
    function ($q, $http, umbRequestHelper) {
        var baseUrl = "backoffice/PassleSync/PassleDashboardAuthors/";
        return {
            updateAll: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get(baseUrl + "RefreshAll"),
                    "Failed to refresh the list of all authors");
            },
            getExisting: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get(baseUrl + "GetExisting"),
                    "Failed to retrieve a list of all authors");
            },
            getPending: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get(baseUrl + "GetPending"),
                    "Failed to retrieve a list of all pending authors");
            },
            syncAll: function () {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "SyncAll"),
                    "Failed to sync all authors");
            },
            deleteAll: function () {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "DeleteAll"),
                    "Failed to delete all authors");
            },
            syncMany: function (shortcodes) {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "SyncMany", {
                        "shortcodes": shortcodes
                    }),
                    "Failed to sync selected authors");
            },
            deleteMany: function (shortcodes) {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "DeleteMany", {
                        "shortcodes": shortcodes
                    }),
                    "Failed to delete selected authors");
            },
            syncOne: function (shortcodes) {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "SyncOne", {
                        "shortcodes": shortcodes
                    }),
                    "Failed to sync selected author");
            },
            deleteOne: function (shortcodes) {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "DeleteOne", {
                        "shortcodes": shortcodes
                    }),
                    "Failed to delete selected author");
            },
        };
    }
);
