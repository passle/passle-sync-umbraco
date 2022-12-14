angular.module("umbraco.directives").directive(
    "postsTable",
    function () {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/PassleSync/backoffice/passleSync/components/organisms/postsTable/postsTable.html',
            scope: {}
        }
    }
);