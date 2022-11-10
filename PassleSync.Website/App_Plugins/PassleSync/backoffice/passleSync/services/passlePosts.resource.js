angular.module("umbraco.resources").factory("passlePostsResource",
    function ($q, $http, umbRequestHelper) {
        var baseUrl = "backoffice/PassleSync/PassleDashboardPosts/";
        return {
            updateAll: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get(baseUrl + "RefreshAll"),
                    "Failed to refresh the list of all posts");
            },
            getExisting: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get(baseUrl + "GetExisting"),
                    "Failed to retrieve a list of all posts");
            },
            syncAll: function () {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "SyncAll"),
                    "Failed to sync all posts");
            },
            deleteAll: function () {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "DeleteAll"),
                    "Failed to delete all posts");
            },
            syncMany: function (shortcodes) {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "SyncMany", {
                        "shortcodes": shortcodes
                    }),
                    "Failed to sync selected posts");
            },
            deleteMany: function (shortcodes) {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "DeleteMany", {
                        "shortcodes": shortcodes
                    }),
                    "Failed to delete selected posts");
            },
            syncOne: function (shortcodes) {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "SyncOne", {
                        "shortcodes": shortcodes
                    }),
                    "Failed to sync selected post");
            },
            deleteOne: function (shortcodes) {
                return umbRequestHelper.resourcePromise(
                    $http.post(baseUrl + "DeleteOne", {
                        "shortcodes": shortcodes
                    }),
                    "Failed to delete selected post");
            },
        };
    }
);
