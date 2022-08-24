angular.module("umbraco").controller("PassleSyncDashboardController", function ($scope, $http, navigationService, notificationsService, userService, logResource, entityResource) {
    var vm = this;
    vm.UserName = "guest";
    vm.UserLogHistory = [];
    vm.buttonState = 'init';
    vm.buttonPostsStateSync = 'init';
    vm.buttonAuthorsStateSync = 'init';
    vm.loading = false;

    vm.init = function () {        
        $http({
            url: "/umbraco/backoffice/PassleSync/passleSync/Get",
            method: "GET"
        }).then(function (response) {
            console.log(response.data);

            vm.shortcode = response.data.Shortcode;
            vm.apiKey = response.data.ApiKey;
            vm.apiUrl = response.data.ApiUrl;
            vm.peopleParentNodeId = response.data.PeopleParentNodeId;
            vm.postsParentNodeId = response.data.PostsParentNodeId;
            vm.postPermalinkPrefix = response.data.PostPermalinkPrefix;
            vm.personPermalinkPrefix = response.data.PersonPermalinkPrefix;
            vm.pluginApiKey = response.data.PluginApiKey;
        });
    };

    vm.init();

    vm.clickButton = function () {
        vm.buttonState = 'busy';
        vm.loading = true;

        $http({
            url: "/umbraco/backoffice/PassleSync/passleSync/Save",
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

            notificationsService.success("Success", "Settings have been saved");

            
        });
    };

    vm.clickButtonSyncAuthors = function () {
        vm.buttonAuthorsStateSync = 'busy';
        vm.loading = true;

        $http({
            url: "/umbraco/backoffice/PassleSync/passleSync/SyncAuthors",
            method: "POST"
        }).then(function (response) {
            vm.loading = false;
            vm.buttonAuthorsStateSync = 'init';

            notificationsService.success("Success", "Authors have been synced");

            navigationService.syncTree({ tree: 'content', path: ["-1", vm.postsParentNodeId], forceReload: true });
        });
    };

    vm.clickButtonSyncPosts = function () {
        vm.buttonPostsStateSync = 'busy';
        vm.loading = true;

        $http({
            url: "/umbraco/backoffice/PassleSync/passleSync/SyncPosts",
            method: "POST"
        }).then(function (response) {
            vm.loading = false;
            vm.buttonPostsStateSync = 'init';

            notificationsService.success("Success", "Post have been synced");

            navigationService.syncTree({ tree: 'content', path: ["-1", vm.peopleParentNodeId], forceReload: true });
        });
    };
});