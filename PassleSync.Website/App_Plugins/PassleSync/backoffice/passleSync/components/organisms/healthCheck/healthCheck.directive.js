angular.module("umbraco.directives").directive(
    "healthCheck",
    function () {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/PassleSync/backoffice/passleSync/components/organisms/healthCheck/healthCheck.html',
            scope: {}
        }
    }
);
