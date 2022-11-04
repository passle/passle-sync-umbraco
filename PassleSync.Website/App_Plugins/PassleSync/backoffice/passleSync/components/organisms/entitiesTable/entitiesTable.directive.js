angular.module("umbraco.directives").directive(
    "entitiesTable",
    function () {
        function EntitiesTableController($scope, notificationsService, treeService) {
            this.isLoading = false;
            this.isUpdating = false;

            let currentSortCol = "Name";
            let currentSortDir = "desc";
            this.isSelectedAll = false;
            this.syncedCount = 0;
            this.unsyncedCount = 0;
            this.selectedCount = 0;
            this.entities = [];
            this.entitiesOnShow = [];

            this.pagination = {
                pageSize: 10,
                pageNumber: 1,
                totalPages: 1
            };

            this.rowProperties = $scope.entityInfo.rowProperties;
            this.entityReponseParam = $scope.entityInfo.entityReponseParam;
            const getRowData = $scope.entityInfo.getRowData;
            const getExisting = $scope.entityInfo.resource.getExisting;
            const updateAll = $scope.entityInfo.resource.updateAll;
            const syncAll = $scope.entityInfo.resource.syncAll;
            const syncOne = $scope.entityInfo.resource.syncOne;
            const syncMany = $scope.entityInfo.resource.syncMany;
            const deleteAll = $scope.entityInfo.resource.deleteAll;
            const deleteOne = $scope.entityInfo.resource.deleteOne;
            const deleteMany = $scope.entityInfo.resource.deleteMany;

            this.onload = () => {
                this.isLoading = true;

                getExisting().then((response) => {
                    this.entities = response[this.entityReponseParam].map((entity) => getRowData(entity));
                    this.syncedCount = this.entities.filter((entity) => entity.synced).length;
                    this.unsyncedCount = this.entities.length - this.syncedCount;
                    this.isSelectedAll = false;
                    this.selectedCount = 0;

                    updatePagination();
                    updateItemsOnShow();

                    this.isLoading = false;
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);
                    this.isLoading = false;
                });
            }

            this.update = () => {
                this.isUpdating = true;

                updateAll().then((response) => {
                    this.entities = response[this.entityReponseParam].map((entity) => getRowData(entity));

                    this.syncedCount = this.entities.filter((entity) => entity.synced).length;
                    this.unsyncedCount = this.entities.length - this.syncedCount;
                    this.isSelectedAll = false;
                    this.selectedCount = 0;

                    updatePagination();
                    updateItemsOnShow();

                    this.isUpdating = false;

                    syncTree();
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);

                    this.isUpdating = false;
                });
            }

            this.sync = () => {
                this.isUpdating = true;

                let syncProm;
                let shortcodes = [];

                if (this.selectedCount == 0) {
                    syncProm = syncAll();
                } else if (this.selectedCount == 1) {
                    shortcodes = this.entities.filter((entity) => entity.selected).map((entity) => entity.shortcode);
                    syncProm = syncOne(shortcodes);
                } else {
                    shortcodes = this.entities.filter((entity) => entity.selected).map((entity) => entity.shortcode);
                    syncProm = syncMany(shortcodes);
                }

                syncProm.then((response) => {
                    this.entities.forEach((entity, ii) => {
                        let matchingEntities = response[this.entityReponseParam].filter(x => x.Shortcode === entity.shortcode);
                        if (matchingEntities.length > 0) {
                            this.entities[ii] = getRowData(matchingEntities[0]);
                        }
                    });

                    this.syncedCount = this.entities.filter((entity) => entity.synced).length;
                    this.unsyncedCount = this.entities.length - this.syncedCount;
                    this.isSelectedAll = false;
                    this.selectedCount = 0;

                    updatePagination();
                    updateItemsOnShow();

                    this.isUpdating = false;

                    notificationsService.success("Success", $scope.entityInfo.namePlural + " have been synced");

                    syncTree();
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);

                    this.isUpdating = false;
                });
            }

            this.delete = () => {
                this.isUpdating = true;

                let deleteProm;
                let shortcodes = [];
                if (this.selectedCount == 0) {
                    deleteProm = deleteAll();
                } else if (this.selectedCount == 1) {
                    shortcodes = this.entities.filter((entity) => entity.selected).map((entity) => entity.shortcode);
                    deleteProm = deleteOne(shortcodes);
                } else {
                    shortcodes = this.entities.filter((entity) => entity.selected).map((entity) => entity.shortcode);
                    deleteProm = deleteMany(shortcodes);
                }

                deleteProm.then((response) => {
                    this.entities.forEach((entity, ii) => {
                        let matchingEntities = response[this.entityReponseParam].filter(x => x.Shortcode === entity.shortcode);
                        if (matchingEntities.length > 0) {
                            this.entities[ii] = getRowData(matchingEntities[0], true);
                        }
                    });
                    this.syncedCount = this.entities.filter((entity) => entity.synced).length;
                    this.unsyncedCount = this.entities.length - this.syncedCount;
                    this.isSelectedAll = false;
                    this.selectedCount = 0;

                    updatePagination();
                    updateItemsOnShow();

                    this.isUpdating = false;

                    notificationsService.success("Success", $scope.entityInfo.namePlural + " have been deleted");

                    syncTree();
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);

                    this.isUpdating = false;
                });
            }

            this.allowSelectAll = true;
            this.onSelectItem = (selectedItem) => {
                selectedItem.selected = !selectedItem.selected;
                this.selectedCount = this.entities.filter((entity) => entity.selected).length;
            }
            this.onSelectAll = () => {
                this.isSelectedAll = !this.isSelectedAll;
                this.entitiesOnShow.forEach((entity) => entity.selected = this.isSelectedAll);
                this.selectedCount = this.isSelectedAll ? this.entities.length : 0;
            }
            this.onSelectedAll = () => {
                return this.isSelectedAll;
            };
            this.onSortDirection = (col, direction) => {
                return col === currentSortCol && direction === currentSortDir;
            }
            this.onSort = (field, allow) => {
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

                    this.entities.sort((a, b) => (a[currentSortCol] > b[currentSortCol] && currentSortDir === "desc") ? 1 : -1);
                }
            }

            this.nextPage = () => this.goToPage(this.pagination.pageNumber + 1);
            this.prevPage = () => this.goToPage(this.pagination.pageNumber - 1);
            this.goToPage = (pageNo) => {
                this.pagination = {
                    ...this.pagination,
                    pageNumber: pageNo,
                };

                updateItemsOnShow();
            }
            const updatePagination = () => {
                this.pagination = {
                    ...this.pagination,
                    pageNumber: 1,
                    totalPages: Math.ceil(this.entities.length / this.pagination.pageSize)
                };
            }

            const syncTree = () => treeService.clearCache({ section: "content" });

            const updateItemsOnShow = () => {
                this.entitiesOnShow = this.entities.slice(
                    (this.pagination.pageNumber - 1) * this.pagination.pageSize,
                    Math.min(this.pagination.pageNumber * this.pagination.pageSize, this.entities.length)
                );
            }
        };

        function link(scope, element, attrs, controller) {
            controller.onload();
        }

        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/PassleSync/backoffice/passleSync/components/organisms/entitiesTable/entitiesTable.html',
            scope: {
                entityInfo: '='
            },
            controller: ['$scope', 'notificationsService', 'treeService', EntitiesTableController],
            controllerAs: 'vm',
            link: link
        }
    }
);