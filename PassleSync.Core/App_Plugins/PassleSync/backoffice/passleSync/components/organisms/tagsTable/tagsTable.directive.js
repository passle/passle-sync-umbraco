angular.module("umbraco.directives").directive(
    "tagsTable",
    function () {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/PassleSync/backoffice/passleSync/components/organisms/tagsTable/tagsTable.html',
            scope: {}
        }
    }
);