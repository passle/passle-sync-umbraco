angular.module("umbraco").controller(
    "AuthorsTableController",
    function (notificationsService, treeService, passleAuthorsResource) {
        var vm = this;
        vm.isLoading = false;

        let currentSortCol = "Name";
        let currentSortDir = "desc";
        vm.isSelectedAll = false;
        vm.syncedCount = 0;
        vm.unsyncedCount = 0;
        vm.selectedCount = 0;
        vm.authors = [];
        vm.authorsOnShow = [];

        vm.pagination = {
            pageSize: 10,
            pageNumber: 1,
            totalPages: 1
        };

        function getAuthorDataObject(author, deletedOverride = null) {
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
        }
        function getOverriddenAuthorDataObject(author) {
            return getAuthorDataObject(author, true);
        }

        function syncTree() {
            treeService.clearCache({ section: "content" });
        }

        function UpdatePagination() {
            vm.pagination = {
                ...vm.pagination,
                pageNumber: 1,
                totalPages: Math.ceil(vm.authors.length / vm.pagination.pageSize)
            };
        }

        function UpdateItemsOnShow() {
            vm.authorsOnShow = vm.authors.slice(
                (vm.pagination.pageNumber - 1) * vm.pagination.pageSize,
                Math.min(vm.pagination.pageNumber * vm.pagination.pageSize, vm.authors.length)
            );
        }

        function onload() {
            vm.isLoading = true;

            passleAuthorsResource.getExisting().then((response) => {
                vm.authors = response.Authors.map((author) => getAuthorDataObject(author));
                vm.syncedCount = vm.authors.filter((author) => author.synced).length;
                vm.unsyncedCount = vm.authors.length - vm.syncedCount;
                vm.isSelectedAll = false;
                vm.selectedCount = 0;

                UpdatePagination();
                UpdateItemsOnShow();

                vm.isLoading = false;
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);
                vm.isLoading = false;
            });
        }
        onload();

        vm.update = function () {
            vm.isUpdating = true;

            passleAuthorsResource.updateAll().then((response) => {
                vm.authors = response.Authors.map((author) => getAuthorDataObject(author));

                vm.syncedCount = vm.authors.filter((author) => author.synced).length;
                vm.unsyncedCount = vm.authors.length - vm.syncedCount;
                vm.isSelectedAll = false;
                vm.selectedCount = 0;

                UpdatePagination();
                UpdateItemsOnShow();

                vm.isUpdating = false;

                syncTree();
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);

                vm.isUpdating = false;
            });
        }

        vm.sync = function () {
            vm.isUpdating = true;

            let syncProm;
            let shortcodes;

            if (vm.selectedCount == 0) {
                syncProm = passleAuthorsResource.syncAll();
            } else if (vm.selectedCount == 1) {
                shortcodes = vm.authors.filter((author) => author.selected).map((author) => author.shortcode);
                syncProm = passleAuthorsResource.syncOne(shortcodes);
            } else {
                shortcodes = vm.authors.filter((author) => author.selected).map((author) => author.shortcode);
                syncProm = passleAuthorsResource.syncMany(shortcodes);
            }

            syncProm.then((response) => {
                vm.authors.forEach((author, ii) => {
                    let matchingAuthors = response.Authors.filter(x => x.Shortcode === author.shortcode);
                    if (matchingAuthors.length > 0) {
                        vm.authors[ii] = getAuthorDataObject(matchingAuthors[0]);
                    }
                });

                vm.syncedCount = vm.authors.filter((author) => author.synced).length;
                vm.unsyncedCount = vm.authors.length - vm.syncedCount;
                vm.isSelectedAll = false;
                vm.selectedCount = 0;

                UpdatePagination();
                UpdateItemsOnShow();

                vm.isUpdating = false;

                notificationsService.success("Success", "Authors have been synced");

                syncTree();
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);

                vm.isUpdating = false;
            });
        }

        vm.delete = function () {
            vm.isUpdating = true;

            let deleteProm;
            let shortcodes;

            if (vm.selectedCount == 0) {
                deleteProm = passleAuthorsResource.deleteAll();
            } else if (vm.selectedCount == 1) {
                shortcodes = vm.authors.filter((author) => author.selected).map((author) => author.shortcode);
                deleteProm = passleAuthorsResource.deleteOne(shortcodes);
            } else {
                shortcodes = vm.authors.filter((author) => author.selected).map((author) => author.shortcode);
                deleteProm = passleAuthorsResource.deleteMany(shortcodes);
            }

            deleteProm.then((response) => {
                vm.authors.forEach((author, ii) => {
                    let matchingAuthors = response.Authors.filter(x => x.Shortcode === author.shortcode);
                    if (matchingAuthors.length > 0) {
                        vm.authors[ii] = getOverriddenAuthorDataObject(matchingAuthors[0]);
                    }
                });

                vm.syncedCount = vm.authors.filter((author) => author.synced).length;
                vm.unsyncedCount = vm.authors.length - vm.syncedCount;
                vm.isSelectedAll = false;
                vm.selectedCount = 0;

                UpdatePagination();
                UpdateItemsOnShow();

                vm.isUpdating = false;

                notificationsService.success("Success", "Authors have been deleted");

                syncTree();
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);

                vm.isUpdating = false;
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
            vm.authorsOnShow.forEach((author) => author.selected = vm.isSelectedAll);
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

        vm.nextPage = function () {
            vm.pagination = {
                ...vm.pagination,
                pageNumber: vm.pagination.pageNumber + 1,
            };

            UpdateItemsOnShow();
        }

        vm.prevPage = function () {
            vm.pagination = {
                ...vm.pagination,
                pageNumber: vm.pagination.pageNumber - 1,
            };

            UpdateItemsOnShow();
        }

        vm.changePage = function (pageNo) {
            vm.pagination = {
                ...vm.pagination,
                pageNumber: pageNo,
            };

            UpdateItemsOnShow();
        }

        vm.goToPage = function (pageNo) {
            vm.pagination = {
                ...vm.pagination,
                pageNumber: pageNo,
            };

            UpdateItemsOnShow();
        }
    }
);

