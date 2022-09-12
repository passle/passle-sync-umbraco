﻿angular.module("umbraco").controller(
    "PostsTableController",
    function (notificationsService, passlePostsResource) {
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
                "editPath": post.Synced ? ("/content/content/edit/" + post.Id) : post.PostUrl,
                "shortcode": post.Shortcode,
                "synced": post.Synced
            });
        }
        function getOverriddenPostDataObject(post, shouldOverride, overrideValue) {
            if (shouldOverride) return getPostDataObject(post, overrideValue);
            return getPostDataObject(post);
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

            let startTime = Date.now();

            passlePostsResource.getExisting().then((response) => {
                vm.posts = response.Posts.map((post) => getPostDataObject(post));
                vm.syncedCount = vm.posts.filter((post) => post.synced).length;
                vm.unsyncedCount = vm.posts.length - vm.syncedCount;
                vm.isSelectedAll = false;
                vm.selectedCount = 0;
                vm.loading = false;

                let endTime = Date.now();
                console.log('Loaded in ', endTime - startTime);
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);
                vm.loading = false;

                let endTime = Date.now();
                console.log('Loaded in ', endTime - startTime);
            });
        }
        onload();

        vm.refresh = function () {
            vm.isRefreshing = true;

            let startTime = Date.now();

            passlePostsResource.refreshAll().then((response) => {
                vm.posts = response.Posts.map((post) => getPostDataObject(post));

                vm.syncedCount = vm.posts.filter((post) => post.synced).length;
                vm.unsyncedCount = vm.posts.length - vm.syncedCount;
                vm.isSelectedAll = false;
                vm.selectedCount = 0;
                vm.isRefreshing = false;

                syncTree();

                let endTime = Date.now();
                console.log('Loaded in ', endTime - startTime);
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);

                vm.isRefreshing = false;

                let endTime = Date.now();
                console.log('Loaded in ', endTime - startTime);
            });
        }

        vm.sync = function () {
            vm.isSyncing = true;

            let startTime = Date.now();

            let syncProm;
            let shortcodes = [];
            if (vm.isSelectedAll) {
                syncProm = passlePostsResource.syncAll();
            } else {
                if (vm.selectedCount == 0) {
                    syncProm = passlePostsResource.syncAll();
                } else if (vm.selectedCount == 1) {
                    shortcodes = vm.posts.filter((post) => post.selected).map((post) => post.shortcode);
                    syncProm = passlePostsResource.syncOne(shortcodes);
                } else {
                    shortcodes = vm.posts.filter((post) => post.selected).map((post) => post.shortcode);
                    syncProm = passlePostsResource.syncMany(shortcodes);
                }
            }

            syncProm.then(() => {
                passlePostsResource.refreshAll().then((response) => {
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

                    syncTree();

                    let endTime = Date.now();
                    console.log('Loaded in ', endTime - startTime);
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);

                    vm.isSyncing = false;

                    let endTime = Date.now();
                    console.log('Loaded in ', endTime - startTime);
                });
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);

                vm.isSyncing = false;

                let endTime = Date.now();
                console.log('Loaded in ', endTime - startTime);
            });
        }

        vm.delete = function () {
            vm.isDeleting = true;

            let startTime = Date.now();

            let deleteProm;
            let shortcodes = [];
            if (vm.isSelectedAll) {
                deleteProm = passlePostsResource.deleteAll();
            } else {
                if (vm.selectedCount == 0) {
                    deleteProm = passlePostsResource.deleteAll();
                } else if (vm.selectedCount == 1) {
                    shortcodes = vm.posts.filter((post) => post.selected).map((post) => post.shortcode);
                    deleteProm = passlePostsResource.deleteOne(shortcodes);
                } else {
                    shortcodes = vm.posts.filter((post) => post.selected).map((post) => post.shortcode);
                    deleteProm = passlePostsResource.deleteMany(shortcodes);
                }
            }

            deleteProm.then(() => {
                passlePostsResource.refreshAll().then((response) => {
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

                    syncTree();

                    let endTime = Date.now();
                    console.log('Loaded in ', endTime - startTime);
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);

                    vm.isDeleting = false;

                    let endTime = Date.now();
                    console.log('Loaded in ', endTime - startTime);
                });
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);

                vm.isDeleting = false;

                let endTime = Date.now();
                console.log('Loaded in ', endTime - startTime);
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

