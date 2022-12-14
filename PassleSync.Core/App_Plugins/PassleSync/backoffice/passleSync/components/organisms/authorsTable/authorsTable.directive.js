angular.module("umbraco.directives").directive(
    "authorsTable",
    function () {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/PassleSync/backoffice/passleSync/components/organisms/authorsTable/authorsTable.html',
            scope: {}
        }
    }
);