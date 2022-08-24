angular.module("umbraco").controller(
    "SyncSettingsController",
    function (passleSettingsResource, notificationsService) {
        var vm = this;

        vm.buttonState = 'init';
        vm.settings = {};

        function onload() {
            passleSettingsResource.get().then((response) => {
                vm.settings = response;
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);
            })
        }
        onload();

        vm.save = function () {
            vm.isSaving = true;
            vm.buttonState = 'busy';

            passleSettingsResource.save(vm.settings).then(() => {
                vm.isSaving = false;
                vm.buttonState = 'init';

                notificationsService.success("Success", "Settings have been saved");
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);

                vm.isSaving = false;
                vm.buttonState = 'init';
            });
        }
    }
);

