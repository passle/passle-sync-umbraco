angular.module("umbraco").controller(
    "PassleSync.DashboardController",
    function ($timeout, navigationService) {
        var vm = this;

        vm.pageTitle = "Passle Sync Settings";

        vm.tabs = [
            {
                "label": "Settings",
                "alias": "Settings",
                "active": true
            },
            {
                "label": "Posts",
                "alias": "Posts",
            },
            {
                "label": "Authors",
                "alias": "Authors",
            }
        ];

        vm.changeTab = function (selectedTab) {
            vm.tabs.forEach(function (tab) {
                tab.active = false;
            });
            selectedTab.active = true;
            vm.pageTitle = "Passle Sync " + selectedTab.label;
        };

        $timeout(function () {
            navigationService.syncTree({
                tree: "passleSync",
                path: "-1"
            });
        });
    }
);
