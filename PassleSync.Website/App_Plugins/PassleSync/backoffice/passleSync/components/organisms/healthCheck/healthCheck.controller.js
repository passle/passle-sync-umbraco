angular.module("umbraco").controller(
    "HealthCheckController",
    function (passleSettingsResource) {
        var vm = this;

        vm.loading = false;
        vm.settings = {};

        function onload() {
            vm.loading = true;

            passleSettingsResource.get().then((response) => {
                vm.settings = response;
                vm.loading = false;
                initPenpalConnection();
            }, (error) => {
                console.error(error);
                notificationsService.error("Error", error);
                vm.loading = false;
            });
        }

        onload();

        function initPenpalConnection() {
            const domainExt = vm.settings["DomainExt"];

            const connection = Penpal.connectToChild({
                url: `https://www.passle.${domainExt}/cms-integration-health-check`,
                appendTo: document.getElementById("health-check-iframe-container"),
                methods: {
                    getOptions() {
                        return {
                            apiKey: vm.settings["ClientApiKey"],
                            passleShortcodes: vm.settings["PassleShortcodes"].split(","),
                        };
                    },
                },
            });

            connection.promise.then(() => {
                vm.loading = false;
            });
        }
    }
);
