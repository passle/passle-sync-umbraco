angular.module("umbraco").controller(
    "PassleDashboardController",
    function () {
        var vm = this;

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

        vm.changeTab = function(selectedTab) {
            vm.tabs.forEach(function (tab) {
                tab.active = false;
            });
            selectedTab.active = true;
        };
    }
);

