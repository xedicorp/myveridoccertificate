
angular.module("cms.headerfooter", ["ngRoute", "cms.shared"]).constant("_", window._).constant("headerfooter.modulePath", "/Admin/Modules/HeaderFooter/Js/");
angular.module("cms.headerfooter").config(["$routeProvider", "shared.routingUtilities", "headerfooter.modulePath",
    function (r, e, i) { e.registerCrudRoutes(r, i, "HeaderFooter") }]);

//angular.module("cms.headerfooter").factory("directories.directoryService",["$http","_","shared.serviceBase","directories.DirectoryTree",function(r,e,t,n){var u={},c=t+"page-directories";function i(e){return c+"/"+e}function o(e){return i(e)+"/access-rules"}return u.getAll=function(){return r.get(c)},u.getTree=function(){return r.get(c+"/tree").then(function(e){return e&&new n(e)})},u.getById=function(e){return r.get(i(e))},u.getAccessRulesByPageDirectoryId=function(e){return r.get(o(e))},u.add=function(e){return r.post(c,e)},u.update=function(e){return r.patch(i(e.pageDirectoryId),e)},u.remove=function(e){return r.delete(i(e))},u.updateUrl=function(e){return r.put(i(e.pageDirectoryId)+"/url",e)},u.updateAccessRules=function(e){return r.patch(o(e.pageDirectoryId),e)},u}]);
//angular.module("cms.headerfooter").factory("directories.DirectoryTree",["_",function(i){return function(r){var e=this;i.extend(e,r),e.flatten=function(t){var r=[];return function e(r,n){if(r.pageDirectoryId==t)return;n.push(r);i.each(r.childPageDirectories,function(r){e(r,n)})}(e,r),r},e.findNodeById=function(t){return function e(r){var n;if(!r)return;r.forEach(function(r){n=n||(r.pageDirectoryId==t?r:e(r.childPageDirectories))});return n}([e])}}}]);
//angular.module("cms.headerfooter").directive("cmsDirectoryGrid",["shared.permissionValidationService","headerfooter.modulePath",function(e,r){return{restrict:"E",templateUrl:r+"UIComponents/DirectoryGrid.html",scope:{pageDirectories:"=cmsDirectories",redirect:"=cmsRedirect"},replace:!1,controller:function(){this.canUpdate=e.canUpdate("COFDIR")},controllerAs:"vm",bindToController:!0}}]);
//angular.module("cms.headerfooter").directive("cmsDirectoryPath",["headerfooter.modulePath",function(r){return{restrict:"E",templateUrl:r+"UIComponents/DirectoryPath.html",scope:{pageDirectory:"=cmsDirectory"},replace:!1,controller:function(){},controllerAs:"vm",bindToController:!0}}]);
//angular.module("cms.headerfooter").controller("ChangeDirectoryUrlController",["$scope","$q","shared.LoadState","directories.directoryService","options","close",function(e,t,r,a,o,i){var n=e;function c(){n.submitLoadState.on(),a.updateUrl(n.command).then(o.onSave).then(i).finally(n.submitLoadState.off)}(function(){var e=o.pageDirectory;n.pageDirectory=e,n.selectableParentDirectories=o.selectableParentDirectories,n.hasChildContent=o.hasChildContent,n.command={pageDirectoryId:e.pageDirectoryId,urlPath:e.urlPath,parentPageDirectoryId:e.parentPageDirectoryId}})(),n.submitLoadState=new r,n.save=c,n.close=i}]);
//angular.module("cms.headerfooter").controller("AddHeaderFooterController",["$location","shared.stringUtilities","shared.LoadState","directories.directoryService",function(o,t,e,a){var n=this;function d(){n.globalLoadState.on(),a.add(n.command).then(c,n.globalLoadState.off)}function r(){n.formLoadState.off()}function i(){c()}function c(){o.path("/")}n.command={},n.formLoadState=new e(!0),n.globalLoadState=new e,n.editMode=!1,n.save=d,n.cancel=i,n.onDirectoriesLoaded=r}]);

angular.module('cms.headerfooter').factory('headerfooter.headerfooterService', [
    '$http',
    '_',
    'shared.serviceBase',

    function (
        $http,
        _,
        serviceBase,
    ) {

        var service = {},
            headerfooterServiceBase = serviceBase + 'headerfooter';

        /* QUERIES */

        service.getAll = function () {
            return $http.get(headerfooterServiceBase);
        }

        //service.getTree = function () {
        //    return $http.get(directoryServiceBase + '/tree').then(function (tree) {
        //        return tree ? new HeaderFooterTree(tree) : tree;
        //    });
        //}

        //service.getById = function (pageDirectoryId) {

        //    return $http.get(getIdRoute(pageDirectoryId));
        //}

        //service.getAccessRulesByPageDirectoryId = function (pageDirectoryId) {

        //    return $http.get(getAccessRulesRoute(pageDirectoryId));
        //}

        /* COMMANDS */

        //service.add = function (command) {

        //    return $http.post(directoryServiceBase, command);
        //}

        service.update = function (command) {

            return $http.post(headerfooterServiceBase, command);
        }

        //service.remove = function (pageDirectoryId) {

        //    return $http.delete(getIdRoute(pageDirectoryId));
        //}
        //service.updateUrl = function (command) {

        //    return $http.put(getIdRoute(command.pageDirectoryId) + '/url', command);
        //}

        //service.updateAccessRules = function (command) {

        //    return $http.patch(getAccessRulesRoute(command.pageDirectoryId), command);
        //}

        /* PRIVATES */

        //function getIdRoute(pageDirectoryId) {
        //    return directoryServiceBase + '/' + pageDirectoryId;
        //}

        //function getAccessRulesRoute (pageDirectoryId) {
        //    return getIdRoute(pageDirectoryId) + '/access-rules';
        //}

        return service;
    }]);

angular.module("cms.headerfooter").controller("AddHeaderFooterController", [
    "$location",
    "shared.stringUtilities",
    "shared.LoadState",
    "headerfooter.headerfooterService",
    function (o, t, e, a) {
        var n = this;
        function d() {
            n.globalLoadState.on(), a.add(n.command).then(c, n.globalLoadState.off);
        }
        function r() {
            n.formLoadState.off();
        }
        function i() {
            c();
        }
        function c() {
            o.path("/");
        }
        (n.command = {}), (n.formLoadState = new e(!0)), (n.globalLoadState = new e()),
            (n.editMode = !1), (n.save = d), (n.cancel = i), (n.onDirectoriesLoaded = r);
    },
]);

angular.module('cms.shared').controller('MenuPickerDialogController', [
    '$scope',
    '$q',
    'shared.LoadState',
    'shared.customEntityService',
    'shared.SearchQuery',
    'shared.modalDialogService',
    'shared.internalModulePath',
    'shared.permissionValidationService',
    'shared.ModelPreviewFieldset',
    'shared.ImagePreviewFieldCollection',
    'options',
    'close',
    'headerfooter.headerfooterService',
    function (
        $scope,
        $q,
        LoadState,
        customEntityService,
        SearchQuery,
        modalDialogService,
        modulePath,
        permissionValidationService,
        ModelPreviewFieldset,
        ImagePreviewFieldCollection,
        options,
        close,
        headerfooterService) {

        var vm = $scope;
        vm.selectedIds = [];
        init();
        var CUSTOM_ENTITY_ID_PROP = 'customEntityId';
        /* INIT */

        function init() {
            angular.extend($scope, options);

            vm.onOk = onOk;
            vm.onCancel = onCancel;
            vm.onSelect = onSelect;
            vm.onCreate = onCreate;
            vm.selectedEntity = vm.currentEntity; // current entity is null in single mode
            vm.onSelectAndClose = onSelectAndClose;
            vm.close = onCancel;
            vm.multiMode = false;
            vm.gridLoadState = new LoadState();
            vm.query = new SearchQuery({
                onChanged: onQueryChanged,
                useHistory: false
            });
            //vm.query = new SearchQuery({
            //    onChanged: onQueryChanged,
            //    useHistory: false,
            //    defaultParams: options.filter
            //});
            //vm.presetFilter = options.filter;

            // vm.filter = vm.query.getFilters();
            // vm.toggleFilter = toggleFilter;

            //  if (options.selectedParentIds != undefined) {
            if (options.selectedParentIds && options.selectedParentIds.length > 0)
                vm.selectedIds = options.selectedParentIds.split(',').map(Number);
            else
                vm.selectedIds = [];
            // vm.selectedIds = options.selectedParentIds;
            // }

            vm.isSelected = isSelected;
            // vm.customEntityDefinition = options.customEntityDefinition;
            //  vm.multiMode = vm.selectedIds ? true : false;

            // vm.canCreate = getPermission('COMCRT');

            //toggleFilter(false);
            // reloadData();
            loadGrid();
            vm.onSelected = onSelected;
        }

        /* ACTIONS */

        function toggleFilter(show) {
            vm.isFilterVisible = _.isUndefined(show) ? !vm.isFilterVisible : show;
        }

        function onQueryChanged() {
            toggleFilter(false);
            reloadData();
        }
        function loadGrid() {
            vm.gridLoadState.on();
            if (options.parentType == "H" || options.parentType == "U") {
                customEntityService.getAll(vm.query.getParameters(), "HEMENU").then(function (result) {

                    vm.result = result;
                    vm.menuItems = result;
                    //vm.menuItems.items.forEach(function (currentValue, index, arr) {

                    //    var exists = vm.selectedIds.find(function (e) { return e === currentValue.customEntityId });
                    //    if (exists) { onSelect(currentValue); }
                    //});

                });
            }
            if (options.parentType == "S") {
                customEntityService.getAll(vm.query.getParameters(), "XMEDIA").then(function (result) {

                    vm.result = result;
                    vm.menuItems = result;
                    //    vm.gridLoadState.off();
                    //vm.menuItems.items.forEach(function (currentValue, index, arr) {

                    //    var exists = vm.selectedIds.find(function (e) { return e === currentValue.customEntityId });
                    //    if (exists  ) { onSelect(currentValue); }
                    //});
                });
            }
            vm.gridLoadState.off();
            //vm.menuItems = {
            //    "items": [{"customEntityId":1, "text": "Home", "url": "google.com" },
            //        { "customEntityId": 2, "text": "about", "url": "gmail.com" }]
            //};
        }
        function reloadData() {

            var metaDataDef,
                definitionCode = options.customEntityDefinition.customEntityDefinitionCode,
                gridDef = loadGrid();

            if (vm.previewFields) {
                metaDataDef = $q.defer();
                metaDataDef.resolve();
            } else {
                metaDataDef = getMetaData().then(loadMetaData);
            }

            return $q
                .all([metaDataDef, gridDef])
                .then(loadImages);

            function loadGrid() {
                vm.gridLoadState.on();

                return customEntityService.getAll(vm.query.getParameters(), definitionCode).then(function (result) {
                    vm.result = result;
                    vm.gridLoadState.off();
                });
            }

            function getMetaData() {
                return customEntityService.getDataModelSchema(definitionCode);
            }

            function loadMetaData(modelMetaData) {
                vm.previewFields = new ModelPreviewFieldset(modelMetaData);
            }

            function loadImages() {
                vm.gridImages = new ImagePreviewFieldCollection();
                return vm.gridImages.load(vm.result.items, vm.previewFields);
            }

        }
        function onSelected(newEntityArr) {

            if (!newEntityArr || !newEntityArr.length) {
                $scope.gridData = [];
            }
            else {
                //$scope.gridData = newEntityArr;
                // Remove deselected
                //debugger;
                var entitiesToRemove = _.filter($scope.gridData, function (entity) {
                    return !_.contains(newEntityArr, entity[CUSTOM_ENTITY_ID_PROP]);
                });

                $scope.gridData = _.difference($scope.gridData, entitiesToRemove);

                //// Add new
                //var newIds = _.difference(newEntityArr, getSelectedIds());

                //if (newIds.length) {
                //    $scope.gridLoadState.on();

                //    customEntityService.getByIdRange(newIds).then(function (items) {
                //        $scope.gridData = _.union($scope.gridData, items);
                //        $scope.gridLoadState.off();
                //    });
                //}
            }
        }
        /* EVENTS */

        function onCancel() {
            if (!vm.multiMode) {
                // in single-mode reset the entity
                vm.onSelected(vm.currentEntity);
            }
            close();
        }

        function onSelect(entity) {

            //if (!vm.multiMode) {
            vm.selectedEntity = entity;
            //    return;
            //}

            addOrRemove(entity);
        }

        function onSelectAndClose(entity) {
            if (!vm.multiMode) {
                vm.selectedEntity = entity;
                onOk();
                return;
            }

            addOrRemove(entity);
            onOk();
        }

        function onOk() {
            //debugger;
            //debugger;
            //if (!vm.multiMode) {
            //    vm.onSelected(vm.selectedEntity);
            //} else {
            vm.onSelected(vm.selectedIds);
            // }

            var tmpSelectedEntities = [];
            var entitiesToRemove = _.filter(vm.menuItems.items, function (entity) {
                return _.contains(vm.selectedIds, entity[CUSTOM_ENTITY_ID_PROP]);
            });
            vm.menuItems.items.forEach(function (currentValue, index, arr) {


                var exists = vm.selectedIds.find(function (e) { return e === currentValue.customEntityId });
                if (exists) {
                    tmpSelectedEntities.push(currentValue);
                }
            });
            //send to parent

            options.onSelected(options.parentType, tmpSelectedEntities, vm.selectedIds);
            close();
        }

        function onCreate() {
            modalDialogService.show({
                templateUrl: modulePath + 'UIComponents/CustomEntities/AddCustomEntityDialog.html',
                controller: 'AddCustomEntityDialogController',
                options: {
                    customEntityDefinition: options.customEntityDefinition,
                    onComplete: onComplete
                }
            });

            function onComplete(customEntityId) {
                if (vm.multiMode) {
                    onSelect({ customEntityId: customEntityId });
                    reloadData();
                } else {
                    onSelectAndClose({ customEntityId: customEntityId });
                }
            }
        }

        /* PUBLIC HELPERS */

        //function getPermission(code) {
        //    return permissionValidationService.hasPermission(options.customEntityDefinition.customEntityDefinitionCode + code);
        //}

        function isSelected(entity) {

            if (vm.selectedIds && entity && vm.selectedIds.indexOf(entity.customEntityId) > -1) return true;

            //return false;
            if (!entity || !vm.selectedEntity) return false;

            return false;
            //  return entity.customEntityId === vm.selectedEntity.customEntityId;
        }

        function addOrRemove(entity) {

            if (!isSelected(entity)) {
                vm.selectedIds.push(entity.customEntityId);
            } else {
                var index = vm.selectedIds.indexOf(entity.customEntityId);
                vm.selectedIds.splice(index, 1);
            }
        }
    }]);
angular.module("cms.headerfooter").controller("HeaderFooterDetailsController", [
    "$routeParams",
    "$q",
    "$location",
    "_",
    "shared.LoadState",
    "shared.modalDialogService",
    "shared.permissionValidationService",
    "shared.userAreaService",
    "shared.internalModulePath",
    "headerfooter.modulePath",
    "headerfooter.headerfooterService",
    function (t, o, e, n, r, i, a, c, l, s, d) {
        var u = this,
            f = "COFCHF";
        function h() {
            (u.editMode = !0), u.mainForm.formStatus.clear();
        }
        function y() {
            U(u.saveLoadState), d.update(u.command).then(v.bind(null, "Changes were saved successfully")).finally(b.bind(null, u.saveLoadState));
        }
        function m() {
            (u.editMode = !1), (u.command = C(u.pageDirectory)), u.mainForm.formStatus.clear();
        }
        function D() {
            var e = {
                title: "Delete Directory",
                message: "Deleting this directory will delete ALL sub-directories and pages linked to this directory. Are you sure you want to continue?",
                okButtonTitle: "Yes, delete it",
                onOk: function () {
                    return U(), d.remove(u.pageDirectory.pageDirectoryId).then(P).catch(b);
                },
            };
            i.confirm(e);
        }
        function g() {
            i.show({
                templateUrl: l + "UIComponents/EntityAccess/EntityAccessEditor.html",
                controller: "EntityAccessEditorController",
                options: {
                    entityDefinitionCode: f,
                    entityIdPrefix: "pageDirectory",
                    entityDefinitionName: "Directory",
                    entityDescription: u.pageDirectory.fullUrlPath,
                    entityAccessLoader: function () {
                        return d.getAccessRulesByPageDirectoryId(u.pageDirectory.pageDirectoryId);
                    },
                    saveAccess: d.updateAccessRules,
                },
            });
        }
        function p() {
            i.show({
                templateUrl: s + "Routes/Modals/ChangeDirectoryUrl.html",
                controller: "ChangeDirectoryUrlController",
                options: { pageDirectory: u.pageDirectory, selectableParentDirectories: u.selectableParentDirectories, hasChildContent: u.hasChildContent, onSave: v.bind(null, "Url Changed") },
            });
        }
        function v(e) {
            return S().then(u.mainForm.formStatus.success.bind(null, e));
        }
        function S(e) {
            var n = t.id;
            return o
                .all([
                    //d.getTree().then(function (e) {
                    //    var t = e.findNodeById(n);
                    //    (u.pageDirectory = t),
                    //        (u.parentDirectory = e.findNodeById(t.parentPageDirectoryId)),
                    //        (u.selectableParentDirectories = e.flatten(n)),
                    //        (u.command = C(t)),
                    //        (u.editMode = !1),
                    //        (u.hasChildContent = t.numPages || t.childPageDirectories.length);
                    //}),
                    c.getAll().then(function (e) {
                        u.accessRulesEnabled = 1 < e.length;
                    }),
                ])
                .then(b.bind(null, e));
        }
        function C(e) {
            return n.pick(e, "pageDirectoryId", "name");
        }
        function P() {
            e.path("");
        }
        function U(e) {
            u.globalLoadState.on(), e && n.isFunction(e.on) && e.on();
        }
        function b(e) {
            u.globalLoadState.off(), e && n.isFunction(e.off) && e.off();
        }
        (u.edit = h),
            (u.save = y),
            (u.cancel = m),
            (u.deleteDirectory = D),
            (u.viewAccessRules = g),
            (u.changeUrl = p),
            (u.editMode = !1),
            (u.globalLoadState = new r()),
            (u.saveLoadState = new r()),
            (u.formLoadState = new r(!0)),
            (u.canUpdate = a.canUpdate(f)),
            (u.canUpdateUrl = a.hasPermission(f + "UPDURL")),
            (u.canDelete = a.canDelete(f)),
            S(u.formLoadState);
    },
]);

angular.module("cms.headerfooter").controller(
    "HeaderFooterListController",
    [
        '$scope',
        "_",
        "shared.LoadState",
        "shared.SearchQuery",
        "shared.permissionValidationService",
        "headerfooter.headerfooterService",
        'shared.modalDialogService',
        'shared.internalModulePath',
        'shared.arrayUtilities',

        function ($scope, e, LoadState, t, a, headerfooterService,
            modalDialogService, sharedModulePath, arrayUtilities) {
            var vm = this;
            init();

            getAll();
            var CUSTOM_ENTITY_ID_PROP = 'customEntityId';
            function init() {

                vm.save = save.bind(null);
                vm.getAll = getAll.bind(null);
                vm.showPicker = showPicker.bind(null);
                vm.saveLoadState = new LoadState();
                vm.globalLoadState = new LoadState();
                vm.onOk = onOk;
                vm.onCancel = onCancel;
                vm.onSelect = onSelect;
                // vm.onCreate = onCreate;
                vm.selectedEntity = vm.currentEntity; // current entity is null in single mode
                vm.onSelectAndClose = onSelectAndClose;
                vm.close = onCancel;
                vm.remove = remove;
                vm.gridLoadState = new LoadState();
                // vm.onChange = onChange.bind(vm);
                vm.onMenuCollectionChange = onMenuCollectionChange.bind(vm.command);
                vm.onDrop = onDrop;
                vm.onDropSuccess = onDropSuccess;
            }
            function onDrop($index, droppedEntity) {
                debugger;
                // drag drop doesnt give us the to/from index data in the same event, and 
                // we can't use property tracking here, so stuff the index in a variable
                lastDragToIndex = $index;

            }
            var lastDragToIndex = 0;
            function onDropSuccess($index, type) {
                debugger;
                console.log(type)
                vm.globalLoadState.on();
                if (type == "H") {
                    arrayUtilities.move(vm.command.gridData, $index, lastDragToIndex);
                    vm.globalLoadState.off();
                    vm.command.hIds = vm.command.gridData.map(a => a.customEntityId).toString();
                }
                if (type == "S") {
                    arrayUtilities.move(vm.command.gridDataS, $index, lastDragToIndex);
                    vm.globalLoadState.off();
                    vm.command.sIds = vm.command.gridDataS.map(a => a.customEntityId).toString();

                }
                if (type == "U") {
                    arrayUtilities.move(vm.command.gridDataU, $index, lastDragToIndex);
                    vm.globalLoadState.off();
                    vm.command.uIds = vm.command.gridDataU.map(a => a.customEntityId).toString();

                }




            }
            function triggerModelChange() {
                // Because the model is an array, angular can't track the changes
                // which prevents validation from being updated

                var data = vm.command.gridData.slice(0);

                var itemCount = vm.command.gridData.length;
                debugger;
                remove(vm.command.gridData[1], 1, "H");

                //vm.command.gridData.remove(0);
                //  vm.command.gridData = [];
                //data.forEach(  
                //    function (item, index, arr) { 
                //      //  arrayUtilities.remove(vm.command.gridData,  index);
                //        vm.command.gridData.remove(index);
                //       // vm.command.gridData.push(item);
                //    });

                // vm.command.gridData.push(data[0]);
                //vm.command.gridData.forEach(
                //    function (item, index, arr) {
                //        if (index < itemCount) {
                //            debugger;
                //            arrayUtilities.removeObject(vm.command.gridData, item);
                //            debugger;
                //        }
                //        //vm.command.gridData.push(item);
                //    });
                //  vm.command.gridData = data; // vm.command.gridData.slice(0);
                debugger;
                // vm.command.gridData = null;
            }
            function remove(customEntity, indx, type) {


                if (type == "H") {
                    arrayUtilities.removeObject(vm.command.gridData, customEntity);
                    vm.command.hIds = vm.command.gridData.map(a => a.customEntityId).toString()
                }
                if (type == "S") {
                    arrayUtilities.removeObject(vm.command.gridDataS, customEntity);
                    vm.command.sIds = vm.command.gridDataS.map(a => a.customEntityId).toString();

                }
                if (type == "U") {
                    arrayUtilities.removeObject(vm.command.gridDataU, customEntity);
                    vm.command.uIds = vm.command.gridDataU.map(a => a.customEntityId).toString();

                }

            }
            function onCancel() {

                if (!vm.multiMode) {
                    // in single-mode reset the entity
                    vm.onSelected(vm.currentEntity);
                }
                close();
            }
            function close() {

                modalDialogService.hide();
            }
            function onSelect(entity) {
                if (!vm.multiMode) {
                    vm.selectedEntity = entity;
                    return;
                }

                addOrRemove(entity);
            }

            function onSelectAndClose(entity) {
                if (!vm.multiMode) {
                    vm.selectedEntity = entity;
                    onOk();
                    return;
                }

                addOrRemove(entity);
                onOk();
            }

            function onOk() {
                if (!vm.multiMode) {
                    vm.onSelected(vm.selectedEntity);
                } else {
                    vm.onSelected(vm.selectedIds);
                }

                close();
            }
            function isSelected(entity) {
                if (vm.selectedIds && entity && vm.selectedIds.indexOf(entity.customEntityId) > -1) return true;

                if (!entity || !vm.selectedEntity) return false;

                return entity.customEntityId === vm.selectedEntity.customEntityId;
            }
            /* HELPERS */

            //function getSelectedIds() {
            //    return _.pluck( vm.gridData, CUSTOM_ENTITY_ID_PROP);
            //}
            function setLoadingOn(loadState) {
                vm.globalLoadState.on();
                if (loadState && _.isFunction(loadState.on)) loadState.on();
            }

            function setLoadingOff(loadState) {
                vm.globalLoadState.off();
                if (loadState && _.isFunction(loadState.off)) loadState.off();
            }
            function loadGrid() {
                vm.gridLoadState.on();

                //return customEntityService.getAll(vm.query.getParameters(), definitionCode).then(function (result) {
                //    vm.result = result;
                //    vm.gridLoadState.off();
                //});
            }
            function showPicker(parent) {

                var _selectedIds = "";
                if (parent == "H") {
                    _selectedIds = vm.command.hIds
                }

                if (parent == "S") {
                    _selectedIds = vm.command.sIds
                }
                if (parent == "U") {
                    _selectedIds = vm.command.uIds
                }

                // loadGrid();
                // gridLoadState.isLoading = true;
                modalDialogService.show({
                    templateUrl: '/Admin/Modules/HeaderFooter/Js/Routes/MenuPickerDialog.html',
                    controller: 'MenuPickerDialogController',
                    options: {

                        //customEntityDefinition: customEntityOptions,
                        //filter: {
                        //    localeId: $scope.command.localeId
                        //},
                        onSelected: onSelected,
                        parentType: parent,
                        selectedParentIds: _selectedIds
                        // MenuItemType: "Header Menu Item"
                    }
                }

                );

                function onSelected(parentType, newEntityArr, ids) {

                    var loadState;
                    var loadState = vm.saveLoadState;

                    setLoadingOn(loadState);
                    if (parentType == "H") {

                        vm.command.gridData = newEntityArr;
                        vm.command.hIds = ids.toString();
                    }

                    if (parentType == "S") {
                        vm.command.gridDataS = newEntityArr;
                        vm.command.sIds = ids.toString();
                    }
                    if (parentType == "U") {

                        vm.command.gridDataU = newEntityArr;
                        vm.command.uIds = ids.toString();

                    }
                    setLoadingOff.bind(null, loadState);
                    // vm.onChange();
                }

                //    if (!newEntityArr || !newEntityArr.length) {
                //        $scope.gridData = [];
                //    }
                //    else {
                //        // Remove deselected
                //        var entitiesToRemove = _.filter($scope.gridData, function (entity) {
                //            return !_.contains(newEntityArr, entity[CUSTOM_ENTITY_ID_PROP]);
                //        });

                //        $scope.gridData = _.difference($scope.gridData, entitiesToRemove);

                //        // Add new
                //        var newIds = _.difference(newEntityArr, getSelectedIds());

                //        if (newIds.length) {
                //            $scope.gridLoadState.on();

                //            customEntityService.getByIdRange(newIds).then(function (items) {
                //                $scope.gridData = _.union($scope.gridData, items);
                //                $scope.gridLoadState.off();
                //            });
                //        }
                //    }
                //}
            }
            //function onChange() {
            //    var vm = this;


            //    if (vm.change) {
            //        // run after digest cycle completes so the parent ngModel is updated
            //        $timeout(vm.change, 0);
            //    }
            //}

            function onMenuCollectionChange() {
                var vm = this;
                if (vm.change) {
                    $timeout(vm.change, 0);
                }
            }

            function getAll() {
                var loadState;
                var loadState = vm.saveLoadState;

                setLoadingOn(loadState);
                headerfooterService
                    .getAll()
                    .then(function (e) {
                        ;
                        vm.command = mapCommand(e);
                    })
                    .finally(setLoadingOff.bind(null, loadState));
                // vm.gridData = { "gridData": vm.command.gridData };
            }


            function mapCommand(e) {

                return {
                    address: e.address,
                    email: e.email,
                    phone: e.phone,
                    headerLogoImageId: e.headerLogoImageId,
                    footerLogoImageId: e.footerLogoImageId,
                    partnerLogoImageId: e.partnerLogoImageId,
                    gridData: mapGridData(e.headerMenuItems),
                    gridDataU: mapGridData(e.usefulLinks),
                    gridDataS: mapGridData(e.socialMediaIcons),
                    hIds: e.headerMenuItemsIds,
                    uIds: e.usefulLinksIds,
                    sIds: e.socialMediaIconsIds
                }
            }

            function mapGridData(arr) {
                var items = [];
                if (arr && arr.length > 0) {
                    for (var i = 0; i < arr.length; i++) {
                        var data = {
                            customEntityId: arr[i].customEntityId,
                            title: arr[i].text,
                            model: { url: arr[i].url, className: arr[i].className }

                        };
                        items.push(data);

                    }
                }
                return items;
            }
            function onSuccess(message, loadStateToTurnOff) {
                // alert(message);

                vm.message = message;
                vm.mainForm.formStatus.success.bind(null, message);
            }
            function save() {
                var loadState;
                var loadState = vm.saveLoadState;


                setLoadingOn(loadState);;
                headerfooterService
                    .update(vm.command)
                    // .then(pageService.updateDraft.bind(this, vm.updateDraftCommand))
                    .then(onSuccess.bind(null, 'Changes were saved successfully'))
                    .finally(setLoadingOff.bind(null, loadState));
            }

            //var o = this; o.gridLoadState = new r,
            //    o.canCreate = a.canCreate("COFCHF"), o.gridLoadState.on(),
            //    i.getTree().then(function (e) { e = e.flatten(); o.result = e, o.gridLoadState.off() })
        }]);