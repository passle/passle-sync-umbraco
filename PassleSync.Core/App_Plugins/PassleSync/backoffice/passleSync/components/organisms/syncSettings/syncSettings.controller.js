angular.module("umbraco").controller(
    "SyncSettingsController",
    function (passleSettingsResource, notificationsService, $scope) {
        var vm = this;
        vm.loading = false;

        vm.buttonState = 'init';
        vm.settings = {};

        function onload() {
            vm.loading = true;

            passleSettingsResource.get().then((response) => {
                vm.settings = response;
                vm.loading = false;
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);
                vm.loading = false;
            })
        }
        onload();

        vm.save = function () {
            vm.isSaving = true;
            vm.buttonState = 'busy';

            passleSettingsResource.save(vm.settings).then(() => {
                vm.isSaving = false;
                vm.buttonState = 'init';

                // 'tabsForm' is in the parent 'dashboard' scope, but accessible here because `scope: true` in the 'syncSettings' directive
                $scope.tabsForm.$setPristine();

                notificationsService.success("Success", "Settings have been saved");
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error.data.Message);

                vm.isSaving = false;
                vm.buttonState = 'init';
            });
        }
    }
);

