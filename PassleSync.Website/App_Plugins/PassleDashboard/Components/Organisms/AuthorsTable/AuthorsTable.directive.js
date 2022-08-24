angular.module("umbraco.directives").directive(
    "authorsTable",
    function () {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/PassleDashboard/Components/Organisms/AuthorsTable/AuthorsTable.html',
            scope: {}
        }
    }
);