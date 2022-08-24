angular.module("umbraco").controller(
    "SyncSettingsController",
    function (passleSettingsResource, notificationsService) {
        var vm = this;

        vm.notice = null;
        vm.setNotice = function (value) {
            vm.notice = value;
        }

        vm.buttonState = 'init';

        vm.settings = {};

        function onload() {
            passleSettingsResource.get().then((response) => {
                //vm.passleShortcodes = response.Shortcode;
                //vm.clientApiKey = response.ApiKey;
                //vm.apiUrl = response.ApiUrl;
                //vm.authorsParentNodeId = response.PeopleParentNodeId;
                //vm.postsParentNodeId = response.PostsParentNodeId;
                //vm.postPermalinkPrefix = response.PostPermalinkPrefix;
                //vm.authorPermalinkPrefix = response.PersonPermalinkPrefix;
                //vm.pluginApiKey = response.PluginApiKey;
                vm.settings = response;
            }, (error) => {
                vm.notice = {
                    "text": "Error:" + error,
                    "success": false
                };
                console.error(error);
            })
        }
        onload();

        vm.save = function () {
            vm.isSaving = true;
            vm.buttonState = 'busy';

            passleSettingsResource.save(vm.settings).then((response) => {
                vm.isSaving = false;
                vm.buttonState = 'init';

                notificationsService.success("Success", "Settings have been saved");
            }, (error) => {
                console.error(error);
                vm.isSaving = false;
                vm.buttonState = 'init';
            });
        }
    }
);

