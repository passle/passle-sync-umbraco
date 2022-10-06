angular.module("umbraco").controller(
    "PostsTableController",
    function (notificationsService, treeService, passlePostsResource) {
        var vm = this;
        vm.isLoading = false;
        vm.isUpdating = false;

        let currentSortCol = "Name";
        let currentSortDir = "desc";
        vm.isSelectedAll = false;
        vm.syncedCount = 0;
        vm.unsyncedCount = 0;
        vm.selectedCount = 0;
        vm.posts = [];
        vm.postsOnShow = [];

        vm.pagination = {
            pageSize: 10,
            pageNumber: 1,
            totalPages: 1
        };

        function getPostDataObject(post, deletedOverride = false) {
            // Convert the returned model to the format the table needs
            // The override is used when deleting data:
            // - we return the last state of the content before it was deleted, so we need to override the synced and url values

            return Object.assign({}, {
                "name": post.Title,
                "excerpt": post.Excerpt,
                "editPath": (post.Synced && !deletedOverride) ? ("/content/content/edit/" + post.Id) : post.PostUrl,
                "shortcode": post.Shortcode,
                "synced": post.Synced && !deletedOverride
            });
        }
        function getOverriddenPostDataObject(post) {
            return getPostDataObject(post, true);
        }

        function syncTree() {
            treeService.clearCache({ section: "content" });
        }

        function UpdatePagination() {
            vm.pagination = {
                ...vm.pagination,
                pageNumber: 1,
                totalPages: Math.ceil(vm.posts.length / vm.pagination.pageSize)
            };
        }

        function UpdateItemsOnShow() {
            vm.postsOnShow = vm.posts.slice(
                (vm.pagination.pageNumber - 1) * vm.pagination.pageSize,
                Math.min(vm.pagination.pageNumber * vm.pagination.pageSize, vm.posts.length)
            );
        }

        function onload() {
            vm.isLoading = true;

            passlePostsResource.getExisting().then((response) => {
                vm.posts = response.Posts.map((post) => getPostDataObject(post));
                vm.syncedCount = vm.posts.filter((post) => post.synced).length;
                vm.unsyncedCount = vm.posts.length - vm.syncedCount;
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

            passlePostsResource.updateAll().then((response) => {
                vm.posts = response.Posts.map((post) => getPostDataObject(post));

                vm.syncedCount = vm.posts.filter((post) => post.synced).length;
                vm.unsyncedCount = vm.posts.length - vm.syncedCount;
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
            let shortcodes = [];

            if (vm.selectedCount == 0) {
                syncProm = passlePostsResource.syncAll();
            } else if (vm.selectedCount == 1) {
                shortcodes = vm.posts.filter((post) => post.selected).map((post) => post.shortcode);
                syncProm = passlePostsResource.syncOne(shortcodes);
            } else {
                shortcodes = vm.posts.filter((post) => post.selected).map((post) => post.shortcode);
                syncProm = passlePostsResource.syncMany(shortcodes);
            }

            syncProm.then((response) => {
                vm.posts.forEach((post, ii) => {
                    let matchingPosts = response.Posts.filter(x => x.Shortcode === post.shortcode);
                    if (matchingPosts.length > 0) {
                        vm.posts[ii] = getPostDataObject(matchingPosts[0]);
                    }
                });

                vm.syncedCount = vm.posts.filter((post) => post.synced).length;
                vm.unsyncedCount = vm.posts.length - vm.syncedCount;
                vm.isSelectedAll = false;
                vm.selectedCount = 0;

                UpdatePagination();
                UpdateItemsOnShow();

                vm.isUpdating = false;

                notificationsService.success("Success", "Posts have been synced");

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
            let shortcodes = [];
            if (vm.selectedCount == 0) {
                deleteProm = passlePostsResource.deleteAll();
            } else if (vm.selectedCount == 1) {
                shortcodes = vm.posts.filter((post) => post.selected).map((post) => post.shortcode);
                deleteProm = passlePostsResource.deleteOne(shortcodes);
            } else {
                shortcodes = vm.posts.filter((post) => post.selected).map((post) => post.shortcode);
                deleteProm = passlePostsResource.deleteMany(shortcodes);
            }

            deleteProm.then((response) => {
                vm.posts.forEach((post, ii) => {
                    let matchingPosts = response.Posts.filter(x => x.Shortcode === post.shortcode);
                    if (matchingPosts.length > 0) {
                        vm.posts[ii] = getOverriddenPostDataObject(matchingPosts[0]);
                    }
                });
                vm.syncedCount = vm.posts.filter((post) => post.synced).length;
                vm.unsyncedCount = vm.posts.length - vm.syncedCount;
                vm.isSelectedAll = false;
                vm.selectedCount = 0;

                UpdatePagination();
                UpdateItemsOnShow();

                vm.isUpdating = false;

                notificationsService.success("Success", "Posts have been deleted");

                syncTree();
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);

                vm.isUpdating = false;
            });
        }


        vm.posts = [];
        vm.rowProperties = [
            { alias: "excerpt", header: "Excerpt", allowSorting: false },
            { alias: "synced", header: "Synced?", allowSorting: true }
        ];
        vm.allowSelectAll = true;
        vm.onSelectItem = function (selectedItem) {
            selectedItem.selected = !selectedItem.selected;
            vm.selectedCount = vm.posts.filter((post) => post.selected).length;
        }
        vm.onSelectAll = function () {
            vm.isSelectedAll = !vm.isSelectedAll;
            vm.postsOnShow.forEach((post) => post.selected = vm.isSelectedAll);
            vm.selectedCount = vm.isSelectedAll ? vm.posts.length : 0;
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

                vm.posts.sort((a, b) => (a[currentSortCol] > b[currentSortCol] && currentSortDir === "desc") ? 1 : -1);
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

