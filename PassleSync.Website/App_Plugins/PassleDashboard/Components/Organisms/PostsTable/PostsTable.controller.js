angular.module("umbraco").controller(
    "PostsTableController",
    function (navigationService, notificationsService, passlePostsResource) {
        var vm = this;
        vm.loading = false;

        let currentSortCol = "Name";
        let currentSortDir = "desc";
        vm.isSelectedAll = false;
        vm.syncedCount = 0;
        vm.unsyncedCount = 0;
        vm.selectedCount = 0;

        function getPostDataObject(post, syncOverride = null) {
            // Umbraco doesn't write quickly enough that the
            // content has update before this we load the data again
            // so sometimes we need to do something smarter here - override the synced flag

            //TODO: Is this still needed?
            return Object.assign({}, {
                "name": post.Title,
                "excerpt": post.Excerpt,
                "editPath": (syncOverride || post.Synced) ? ("/content/content/edit/" + post.Id) : post.PostUrl,
                "shortcode": post.Shortcode,
                "synced": syncOverride || post.Synced
            });
        }
        function getOverriddenPostDataObject(post, shouldOverride, overrideValue) {
            if (shouldOverride) return getPostDataObject(post, overrideValue);
            return getPostDataObject(post);
        }

        function onload() {
            vm.loading = true;

            passlePostsResource.getAll().then((response) => {
                vm.posts = response.Posts.map((post) => getPostDataObject(post));
                vm.syncedCount = vm.posts.filter((post) => post.synced).length;
                vm.unsyncedCount = vm.posts.length - vm.syncedCount;
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

            passlePostsResource.refreshAll().then(() => {
                passlePostsResource.getAll().then((response) => {
                    vm.posts = response.Posts.map((post) => getPostDataObject(post));

                    vm.syncedCount = vm.posts.filter((post) => post.synced).length;
                    vm.unsyncedCount = vm.posts.length - vm.syncedCount;
                    vm.isSelectedAll = false;
                    vm.selectedCount = 0;
                    vm.isRefreshing = false;

                    //TODO: Use correct nodeId
                    navigationService.syncTree({ tree: 'content', path: ["-1", vm.postsParentNodeId], forceReload: true });
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
                syncProm = passlePostsResource.syncAll();
            } else {

                if (vm.selectedCount > 0) {
                    shortcodes = vm.posts.filter((post) => post.selected).map((post) => post.shortcode);
                } else {
                    shortcodes = vm.posts.map((post) => post.shortcode);
                }
                syncProm = passlePostsResource.syncMany(shortcodes);
            }

            syncProm.then(() => {
                passlePostsResource.getAll().then((response) => {
                    vm.posts = response.Posts.map((post) => getOverriddenPostDataObject(
                        post,
                        vm.isSelectedAll || shortcodes.includes(post.Shortcode),
                        true
                    ));

                    vm.syncedCount = vm.posts.filter((post) => post.synced).length;
                    vm.unsyncedCount = vm.posts.length - vm.syncedCount;
                    vm.isSelectedAll = false;
                    vm.selectedCount = 0;

                    vm.isSyncing = false;

                    notificationsService.success("Success", "Posts have been synced");

                    //TODO: Use correct nodeId
                    navigationService.syncTree({ tree: 'content', path: ["-1", vm.postsParentNodeId], forceReload: true });
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
                deleteProm = passlePostsResource.deleteAll();
            } else {
                if (vm.selectedCount > 0) {
                    shortcodes = vm.posts.filter((post) => post.selected).map((post) => post.shortcode);
                } else {
                    shortcodes = vm.posts.map((post) => post.shortcode);
                }
                deleteProm = passlePostsResource.deleteMany(shortcodes);
            }

            deleteProm.then(() => {
                passlePostsResource.getAll().then((response) => {
                    // Filter to ensure a half-deleted post isn't returned
                    vm.posts = response.Posts.filter((post) => post.Shortcode).map((post) => getOverriddenPostDataObject(
                        post,
                        vm.isSelectedAll || shortcodes.includes(post.Shortcode),
                        false
                    ));
                    vm.syncedCount = vm.posts.filter((post) => post.synced).length;
                    vm.unsyncedCount = vm.posts.length - vm.syncedCount;
                    vm.isSelectedAll = false;
                    vm.selectedCount = 0;
                    vm.isDeleting = false;

                    notificationsService.success("Success", "Posts have been deleted");

                    //TODO: Use correct nodeId
                    navigationService.syncTree({ tree: 'content', path: ["-1", vm.postsParentNodeId], forceReload: true });
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
            vm.posts.forEach((post) => post.selected = vm.isSelectedAll);
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
    }
);

