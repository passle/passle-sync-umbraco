angular.module("umbraco").controller(
    "AuthorsTableController",
    function (passleAuthorsResource) {
        var vm = this;

        vm.entityInfo = {
            nameSingle: "Author",
            namePlural: "Authors",
            getRowData: function getRowData(author, deletedOverride = false) {
                // Convert the returned model to the format the table needs
                // The override is used when deleting data:
                // - we return the last state of the content before it was deleted, so we need to override the synced and url values

                return Object.assign({}, {
                    "name": author.Name,
                    "role": author.RoleInfo,
                    "editPath": (author.Synced && !deletedOverride) ? ("/content/content/edit/" + author.Id) : author.ProfileUrl,
                    "shortcode": author.Shortcode,
                    "synced": author.Synced && !deletedOverride
                });
            },
            resource: passleAuthorsResource,
            entityReponseParam: 'Authors',
            rowProperties: [
                { alias: "role", header: "Role", allowSorting: false },
                { alias: "synced", header: "Synced?", allowSorting: true }
            ]
        };
    }
);

