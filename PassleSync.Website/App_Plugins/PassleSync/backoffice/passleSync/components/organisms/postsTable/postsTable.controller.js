angular.module("umbraco").controller(
    "PostsTableController",
    function (passlePostsResource) {
        var vm = this;

        vm.entityInfo = {
            nameSingle: "Post",
            namePlural: "Posts",
            getRowData: function getRowData(post) {
                // Convert the returned model to the format the table needs
                return Object.assign({}, {
                    "name": post.Title,
                    "excerpt": post.Excerpt,
                    "shortcode": post.Shortcode,
                    "synced": post.Synced.toString()
                });
            },
            resource: passlePostsResource,
            entityReponseParam: 'Posts',
            rowProperties: [
                { alias: "excerpt", header: "Excerpt", allowSorting: false },
                { alias: "synced", header: "Synced?", allowSorting: true }
            ]
        };
    }
);

