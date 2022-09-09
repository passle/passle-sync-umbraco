angular.module("umbraco").controller(
    "TagsTableController",
    function (notificationsService, passleTagsResource) {
        var vm = this;
        vm.isLoading = false;
        vm.isRefreshing = false;

        let currentSortCol = "name";
        let currentSortDir = "desc";
        vm.isSelectedAll = false;

        function getTagDataObject(tag) {
            return Object.assign({}, {
                "name": tag.Title,
                "nonPassleCount": tag.NonPasslePostCount,
                "syncedPassleCount": tag.SyncedPasslePostCount,
                "unsyncedPassleCount": tag.UnsyncedPasslePostCount,
            });
        }

        function onload() {
            vm.isLoading = true;
            getAll();
        }
        onload();

        function getAll() {
            passleTagsResource.getAll().then((response) => {
                vm.tags = response.Tags.map(t => getTagDataObject(t));
                sortTags();

                vm.isLoading = false;
                vm.isRefreshing = false;
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);
                vm.isLoading = false;
                vm.isRefreshing = false;
            });
        }

        vm.refresh = function () {
            vm.isRefreshing = true;
            getAll();
        }


        vm.tags = [];
        vm.rowProperties = [
            { alias: "nonPassleCount", header: "Non-Passle Posts", allowSorting: true },
            { alias: "syncedPassleCount", header: "Synced Passle Posts", allowSorting: true },
            { alias: "unsyncedPassleCount", header: "Unsynced Passle Posts", allowSorting: true }
        ];
        vm.allowSelectAll = false;
        vm.onSelectItem = function (selectedItem) { }
        vm.onSelectAll = function () { }
        vm.onSelectedAll = function () {
            return vm.isSelectedAll;
        };
        vm.onSortDirection = function (col, direction) {
            let sortCol = col === "Name" ? col.toLowerCase() : col;
            return sortCol === currentSortCol && direction === currentSortDir;
        }
        vm.onSort = function (field, allow) {
            let sortField = field === "Name" ? field.toLowerCase() : field;

            if (allow) {
                if (sortField === currentSortCol) {
                    if (currentSortDir === "asc") {
                        currentSortDir = "desc";
                    } else {
                        currentSortDir = "asc";
                    }
                } else {
                    currentSortCol = sortField;
                    currentSortDir = "desc";
                }

                sortTags();
            }
        }
        function sortTags() {
            // Order by high to low, unless currentSortCol === "name", in which case A-Z requires low to high.
            // ^ is XOR. A ^ B iff A or B and not both.
            vm.tags.sort((a, b) => ((b[currentSortCol] > a[currentSortCol] ^ currentSortCol === "name") ^ currentSortDir === "asc") ? 1 : -1);
        }
    }
);

