angular.module("umbraco").controller(
    "SyncSettingsController",
    function (passleSettingsResource) {
        var vm = this;

        vm.notice = null;
        vm.setNotice = function (value) {
            vm.notice = value;
        }

        vm.pluginApiKey = "";
        vm.clientApiKey = "";
        vm.passleShortcodes = "";

        function onload() {
            passleSettingsResource.get().then((settings) => {
                console.log(settings);
                vm.pluginApiKey = settings.PluginApiKey;
                vm.clientApiKey = settings.ApiKey;
                vm.passleShortcodes = settings.Shortcode;
            }, (error) => {
                vm.notice = {
                    "text": "Error:" + error,
                    "success": false
                };
                console.error(error);
            })
        }
        onload();
    }
);

