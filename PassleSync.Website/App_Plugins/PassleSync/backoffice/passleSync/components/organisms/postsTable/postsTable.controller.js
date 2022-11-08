angular.module("umbraco").controller(
    "PostsTableController",
    function (passlePostsResource) {
        var vm = this;

        vm.entityInfo = {
            nameSingle: "Post",
            namePlural: "Posts",
            getRowData: function getRowData(post, deletedOverride = false) {
                // Convert the returned model to the format the table needs
                // The override is used when deleting data:
                // - we return the last state of the content before it was deleted, so we need to override the synced and url values

                return Object.assign({}, {
                    "name": post.Title,
                    "excerpt": post.Excerpt,
                    "shortcode": post.Shortcode,
                    "synced": post.Synced && !deletedOverride
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

