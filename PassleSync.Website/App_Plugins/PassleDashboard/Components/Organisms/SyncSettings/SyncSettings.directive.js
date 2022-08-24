angular.module("umbraco.directives").directive(
    "syncSettings",
    function () {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/PassleDashboard/Components/Organisms/SyncSettings/SyncSettings.html',
            scope: {}
        }
    }
);