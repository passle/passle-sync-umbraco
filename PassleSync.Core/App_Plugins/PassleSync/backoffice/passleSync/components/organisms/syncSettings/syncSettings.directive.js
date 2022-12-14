angular.module("umbraco.directives").directive(
    "syncSettings",
    function () {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/PassleSync/backoffice/passleSync/components/organisms/syncSettings/syncSettings.html',
            scope: {}
        }
    }
);