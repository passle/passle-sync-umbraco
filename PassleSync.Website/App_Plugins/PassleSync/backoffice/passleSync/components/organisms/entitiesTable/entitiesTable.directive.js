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
            this.pendingSync = 0;
            this.pendingDelete = 0;
            this.previousPending = {
                ToSync: [],
                ToDelete: []
            };

            this.pagination = {
                pageSize: 20,
                pageNumber: 1,
                totalPages: 1
            };

            this.rowProperties = $scope.entityInfo.rowProperties;
            this.entityReponseParam = $scope.entityInfo.entityReponseParam;
            const getRowData = $scope.entityInfo.getRowData;
            const getExisting = $scope.entityInfo.resource.getExisting;
            const getPending = $scope.entityInfo.resource.getPending;
            const updateAll = $scope.entityInfo.resource.updateAll;
            const syncAll = $scope.entityInfo.resource.syncAll;
            const syncOne = $scope.entityInfo.resource.syncOne;
            const syncMany = $scope.entityInfo.resource.syncMany;
            const deleteAll = $scope.entityInfo.resource.deleteAll;
            const deleteOne = $scope.entityInfo.resource.deleteOne;
            const deleteMany = $scope.entityInfo.resource.deleteMany;

            const entityActionTriplet = (singleAct, manyAct, allAct, defaultStatus, verb, verbPast) => {
                if (this.isUpdating) return;

                // As the Sync/Delete flows are so similar, create a base method...
                this.isUpdating = true;

                let promise;
                let shortcodes = [];
                let updateMultiple = false;

                if (this.selectedCount == 0) {
                    shortcodes = this.entities.filter((entity) => entity.synced !== 'pending').map((entity) => entity.shortcode);
                    promise = allAct();
                    updateMultiple = true;
                } else if (this.selectedCount == 1) {
                    shortcodes = this.entities.filter((entity) => entity.selected && entity.synced !== 'pending').map((entity) => entity.shortcode);
                    promise = singleAct(shortcodes);
                } else {
                    shortcodes = this.entities.filter((entity) => entity.selected && entity.synced !== 'pending').map((entity) => entity.shortcode);
                    promise = manyAct(shortcodes);
                    updateMultiple = true;
                }

                promise.then(() => {
                    updateEntityStatuses(shortcodes, updateMultiple, defaultStatus);

                    updateView();
                    this.isUpdating = false;

                    if (updateMultiple) {
                        notificationsService.success("Success", $scope.entityInfo.namePlural + " have been queued to " + verb);
                    } else {
                        notificationsService.success("Success", $scope.entityInfo.namePlural + " have been " + verbPast);
                    }
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);

                    this.isUpdating = false;
                });
            }

            // ... and then define specific instances
            this.sync = () => entityActionTriplet(syncOne, syncMany, syncAll, 'true', 'sync', 'synced');
            this.delete = () => entityActionTriplet(deleteOne, deleteMany, deleteAll, 'false', 'delete', 'deleted');

            this.update = () => {
                if (this.isUpdating) return;

                // Update the list of items from the backend
                this.isUpdating = true;

                updateAll().then((response) => {
                    this.entities = response[this.entityReponseParam].map((entity) => getRowData(entity));

                    // If there are any items pending sync/delete, ensure they remain so after the data has been refreshed
                    this.entities.filter(x => this.previousPending.ToSync.includes(x.shortcode) || this.previousPending.ToDelete.includes(x.shortcode))
                        .forEach(x => x.synced = 'pending');

                    updateView();
                    this.isUpdating = false;
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);

                    this.isUpdating = false;
                });
            }

            const updatePending = () => {
                getPending().then((response) => {
                    // Check which items are pending sync / delete
                    // Then diff the changes to update the Synced column without reloading the data
                    var wasPendingSync = this.previousPending.ToSync.filter(x => !response.ToSync.includes(x));
                    var wasPendingDelete = this.previousPending.ToDelete.filter(x => !response.ToDelete.includes(x));

                    var newPendingSync = response.ToSync.filter(x => !this.previousPending.ToSync.includes(x));
                    var newPendingDelete = response.ToDelete.filter(x => !this.previousPending.ToDelete.includes(x));

                    this.pendingSync = response.ToSync?.length ?? 0;
                    this.pendingDelete = response.ToDelete?.length ?? 0;

                    this.entities.filter(x => newPendingSync.includes(x.shortcode) || newPendingDelete.includes(x.shortcode))
                        .forEach(x => x.synced = 'pending');

                    this.entities.filter(x => wasPendingSync.includes(x.shortcode))
                        .forEach(x => x.synced = 'true');

                    this.entities.filter(x => wasPendingDelete.includes(x.shortcode))
                        .forEach(x => x.synced = 'false');

                    this.previousPending = response;

                    updateSyncedCount();
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);
                });
            }

            this.onload = () => {
                this.isLoading = true;

                getExisting().then((response) => {
                    this.entities = response[this.entityReponseParam].map((entity) => getRowData(entity));

                    updatePending();

                    updateView();
                    this.isLoading = false;
                }, (error) => {
                    console.error(error);
                    notificationsService.error("Error", error);
                    this.isLoading = false;
                });

                setInterval(() => updatePending(), 5000);
            }

            const updateSyncedCount = () => {
                this.syncedCount = this.entities.filter((entity) => entity.synced === 'true').length;
                let pendingCount = this.entities.filter((entity) => entity.synced === 'pending').length;
                this.unsyncedCount = this.entities.length - this.syncedCount - pendingCount;
            }

            const deselectAll = () => {
                this.isSelectedAll = false;
                this.selectedCount = 0;
                this.entities.forEach(x => x.selected = false);
            }

            const updateView = () => {
                updateSyncedCount();
                deselectAll();
                updatePagination();
                updateItemsOnShow();
                syncTree();
            }

            const updateEntityStatuses = (shortcodes, updateMultiple, defaultVal) => {
                // Update the selected items to show the correct synced status
                // If many were selected at once, show as 'pending' while the Background runner does its thing
                this.entities.filter(x => shortcodes.includes(x.shortcode))
                    .forEach(x => x.synced = updateMultiple ? 'pending' : defaultVal);
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
                        currentSortDir = currentSortDir === "asc" ? "desc" : "asc";
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