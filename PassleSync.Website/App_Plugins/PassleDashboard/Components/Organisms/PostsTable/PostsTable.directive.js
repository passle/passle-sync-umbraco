angular.module("umbraco.directives").directive(
    "postsTable",
    function () {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/PassleDashboard/Components/Organisms/PostsTable/PostsTable.html',
            scope: {}
        }
    }
);