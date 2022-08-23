angular.module("umbraco").controller("PassleSyncDashboardController", function ($scope, $http, navigationService, notificationsService, userService, logResource, entityResource) {
    var vm = this;
    vm.UserName = "guest";
    vm.UserLogHistory = [];
    vm.buttonState = 'init';
    vm.buttonStateSync = 'init';
    vm.loading = false;

    vm.init = function () {        
        $http({
            url: "/umbraco/backoffice/myPassleSync/myPassleSync/Get",
            method: "GET"
        }).then(function (response) {
            console.log(response.data);

            vm.shortcode = response.data.shortcode;
            vm.apiKey = response.data.apiKey;
            vm.apiUrl = response.data.apiUrl;
            vm.peopleParentNodeId = response.data.peopleParentNodeId;
            vm.postsParentNodeId = response.data.postsParentNodeId;
            vm.postPermalinkPrefix = response.data.postPermalinkPrefix;
            vm.personPermalinkPrefix = response.data.personPermalinkPrefix;
            vm.pluginApiKey = response.data.pluginApiKey;
        });
    };

    vm.init();

    vm.clickButton = function () {
        vm.buttonState = 'busy';
        vm.loading = true;

        $http({
            url: "/umbraco/backoffice/myPassleSync/myPassleSync/Save",
            method: "POST",
            data: {
                shortcode: vm.shortcode,
                apiKey: vm.apiKey,
                apiUrl: vm.apiUrl,
                peopleParentNodeId: vm.peopleParentNodeId,
                postsParentNodeId: vm.postsParentNodeId,

                postPermalinkPrefix: vm.postPermalinkPrefix,
                personPermalinkPrefix: vm.personPermalinkPrefix,
                pluginApiKey: vm.pluginApiKey
            }
        }).then(function (response) {
            vm.loading = false;
            vm.buttonState = 'init';

            notificationsService.success("Success", "Settings bas been saved");

            
        });
    };

    vm.clickButtonSync = function () {
        vm.buttonStateSync = 'busy';
        vm.loading = true;

        $http({
            url: "/umbraco/backoffice/myPassleSync/myPassleSync/Sync",
            method: "POST"
        }).then(function (response) {
            vm.loading = false;
            vm.buttonStateSync = 'init';

            notificationsService.success("Success", "Authors and post have been synced");

            navigationService.syncTree({ tree: 'content', path: ["-1", vm.postsParentNodeId], forceReload: true }); 
            navigationService.syncTree({ tree: 'content', path: ["-1", vm.peopleParentNodeId], forceReload: true }); 
        });
    };

    vm.clickButtonSyncAuthors = function () {
        vm.buttonStateSync = 'busy';
        vm.loading = true;

        $http({
            url: "/umbraco/backoffice/myPassleSync/myPassleSync/SyncAuthors",
            method: "POST"
        }).then(function (response) {
            vm.loading = false;
            vm.buttonStateSync = 'init';

            notificationsService.success("Success", "Authors have been synced");

            navigationService.syncTree({ tree: 'content', path: ["-1", vm.postsParentNodeId], forceReload: true });
        });
    };

    vm.clickButtonSyncPosts = function () {
        vm.buttonStateSync = 'busy';
        vm.loading = true;

        $http({
            url: "/umbraco/backoffice/myPassleSync/myPassleSync/SyncPosts",
            method: "POST"
        }).then(function (response) {
            vm.loading = false;
            vm.buttonStateSync = 'init';

            notificationsService.success("Success", "Post have been synced");

            navigationService.syncTree({ tree: 'content', path: ["-1", vm.peopleParentNodeId], forceReload: true });
        });
    };
});