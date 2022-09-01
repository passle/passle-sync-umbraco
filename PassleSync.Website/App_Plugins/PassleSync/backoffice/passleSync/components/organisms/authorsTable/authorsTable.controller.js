angular.module("umbraco").controller(
    "AuthorsTableController",
    function (notificationsService, passleAuthorsResource) {
        var vm = this;
        console.log("vm", vm);
        vm.loading = false;

        let currentSortCol = "Name";
        let currentSortDir = "desc";
        vm.isSelectedAll = false;
        vm.syncedCount = 0;
        vm.unsyncedCount = 0;
        vm.selectedCount = 0;

        function getAuthorDataObject(author, syncOverride = null) {
            // Umbraco doesn't write quickly enough that the 
            // content has update before this we load the data again
            // so sometimes we need to do something smarter here - override the synced flag
            return Object.assign({}, {
                "name": author.Name,
                "role": author.RoleInfo,
                "editPath": (syncOverride || author.Synced) ? ("/content/content/edit/" + author.Id) : author.ProfileUrl,
                "shortcode": author.Shortcode,
                "synced": syncOverride || author.Synced
            });
        }
        function getOverriddenAuthorDataObject(author, shouldOverride, overrideValue) {
            if (shouldOverride) return getAuthorDataObject(author, overrideValue);
            return getAuthorDataObject(author);
        }

        function syncTree() {
            /*
             * TODO: Find a way to tell Umbraco that the content section should be loaded fresh when it's opened
             * Trying to refresh the content with navigationService.syncTree({ tree: "content" ...
             * fails with an error, as the 'content' tree doesn't exist when the Settings section is open
             */
        }

        function onload() {
            vm.loading = true;

            passleAuthorsResource.getAll().then((response) => {
                vm.authors = response.Authors.map((author) => getAuthorDataObject(author));
                vm.syncedCount = vm.authors.filter((author) => author.synced).length;
                vm.unsyncedCount = vm.authors.length - vm.syncedCount;
                vm.isSelectedAll = false;
                vm.selectedCount = 0;
                vm.loading = false;
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);
                vm.loading = false;
            });
        }
        onload();

        vm.refresh = function () {
            vm.isRefreshing = true;

            passleAuthorsResource.refreshAll().then(() => {
                passleAuthorsResource.getAll().then((response) => {
                    vm.authors = response.Authors.map((author) => getAuthorDataObject(author));

                    vm.syncedCount = vm.authors.filter((author) => author.synced).length;
                    vm.unsyncedCount = vm.authors.length - vm.syncedCount;
                    vm.isSelectedAll = false;
                    vm.selectedCount = 0;
                    vm.isRefreshing = false;

                    syncTree();
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);

                    vm.isRefreshing = false;
                });
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);

                vm.isRefreshing = false;
            });
        }

        vm.sync = function () {
            vm.isSyncing = true;

            let syncProm;
            let shortcodes;
            if (vm.isSelectedAll) {
                syncProm = passleAuthorsResource.syncAll();
            } else {
                if (vm.selectedCount > 0) {
                    shortcodes = vm.authors.filter((author) => author.selected).map((author) => author.shortcode);
                } else {
                    shortcodes = vm.authors.map((author) => author.shortcode);
                }
                syncProm = passleAuthorsResource.syncMany(shortcodes);
            }

            syncProm.then(() => {
                passleAuthorsResource.getAll().then((response) => {
                    vm.authors = response.Authors.map((author) => getOverriddenAuthorDataObject(
                        author,
                        vm.isSelectedAll || shortcodes.includes(author.Shortcode),
                        true
                    ));

                    vm.syncedCount = vm.authors.filter((author) => author.synced).length;
                    vm.unsyncedCount = vm.authors.length - vm.syncedCount;
                    vm.isSelectedAll = false;
                    vm.selectedCount = 0;

                    vm.isSyncing = false;

                    notificationsService.success("Success", "Authors have been synced");

                    syncTree();
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);

                    vm.isSyncing = false;
                });
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);

                vm.isSyncing = false;
            });
        }

        vm.delete = function () {
            vm.isDeleting = true;

            let deleteProm;
            let shortcodes;
            if (vm.isSelectedAll) {
                deleteProm = passleAuthorsResource.deleteAll();
            } else {
                if (vm.selectedCount > 0) {
                    shortcodes = vm.authors.filter((author) => author.selected).map((author) => author.shortcode);
                } else {
                    shortcodes = vm.authors.map((author) => author.shortcode);
                }
                deleteProm = passleAuthorsResource.deleteMany(shortcodes);
            }

            deleteProm.then(() => {
                passleAuthorsResource.getAll().then((response) => {
                    // Filter to ensure a half-deleted author isn't returned
                    vm.authors = response.Authors.filter((author) => author.Shortcode).map((author) => getOverriddenAuthorDataObject(
                        author,
                        vm.isSelectedAll || shortcodes.includes(author.Shortcode),
                        false
                    ));
                    vm.syncedCount = vm.authors.filter((author) => author.synced).length;
                    vm.unsyncedCount = vm.authors.length - vm.syncedCount;
                    vm.isSelectedAll = false;
                    vm.selectedCount = 0;
                    vm.isDeleting = false;

                    notificationsService.success("Success", "Authors have been deleted");

                    syncTree();
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);

                    vm.isDeleting = false;
                });
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);

                vm.isDeleting = false;
            });
        }


        vm.authors = [];
        vm.rowProperties = [
            { alias: "role", header: "Role", allowSorting: false },
            { alias: "synced", header: "Synced?", allowSorting: true }
        ];
        vm.allowSelectAll = true;
        vm.onSelectItem = function (selectedItem) {
            selectedItem.selected = !selectedItem.selected;
            vm.selectedCount = vm.authors.filter((author) => author.selected).length;
        }
        vm.onSelectAll = function () {
            vm.isSelectedAll = !vm.isSelectedAll;
            vm.authors.forEach((author) => author.selected = vm.isSelectedAll);
            vm.selectedCount = vm.isSelectedAll ? vm.authors.length : 0;
        }
        vm.onSelectedAll = function () {
            return vm.isSelectedAll;
        };
        vm.onSortDirection = function (col, direction) {
            return col === currentSortCol && direction === currentSortDir;
        }
        vm.onSort = function (field, allow) {
            if (allow) {
                if (field === currentSortCol) {
                    if (currentSortDir === "asc") {
                        currentSortDir = "desc";
                    } else {
                        currentSortDir = "asc";
                    }
                } else {
                    currentSortCol = field;
                    currentSortDir = "desc";
                }

                vm.authors.sort((a, b) => (a[currentSortCol] > b[currentSortCol] && currentSortDir === "desc") ? 1 : -1);
            }
        }
    }
);

