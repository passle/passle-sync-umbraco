angular.module("umbraco.directives").directive(
    "remoteHostingSetup",
    function () {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/PassleSync/backoffice/passleSync/components/organisms/remoteHostingSetup/remoteHostingSetup.html',
            scope: {}
        }
    }
);