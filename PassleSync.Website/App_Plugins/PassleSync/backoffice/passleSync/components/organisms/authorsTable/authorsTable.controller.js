angular.module("umbraco").controller(
    "AuthorsTableController",
    function (passleAuthorsResource) {
        var vm = this;

        vm.entityInfo = {
            nameSingle: "Author",
            namePlural: "Authors",
            getRowData: function getRowData(author) {
                // Convert the returned model to the format the table needs
                return Object.assign({}, {
                    "name": author.Name,
                    "role": author.RoleInfo,
                    "shortcode": author.Shortcode,
                    "synced": author.Synced.toString()
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

