angular
    .module('cms.directories', ['ngRoute', 'cms.shared'])
    .constant('_', window._)
    .constant('directories.modulePath', '/Admin/Modules/Directories/Js/');
angular.module('cms.directories').config([
    '$routeProvider',
    'shared.routingUtilities',
    'directories.modulePath',
function (
    $routeProvider,
    routingUtilities,
    modulePath) {

    routingUtilities.registerCrudRoutes($routeProvider, modulePath, 'Directory');
}]);
angular.module('cms.directories').factory('directories.directoryService', [
    '$http',
    '_',
    'shared.serviceBase',
    'directories.DirectoryTree',
function (
    $http,
    _,
    serviceBase,
    DirectoryTree) {

    var service = {},
        directoryServiceBase = serviceBase + 'page-directories';

    /* QUERIES */

    service.getAll = function () {
        return $http.get(directoryServiceBase);
    }

    service.getTree = function () {
        return $http.get(directoryServiceBase + '/tree').then(function (tree) {
            return tree ? new DirectoryTree(tree) : tree;
        });
    }

    service.getById = function (pageDirectoryId) {

        return $http.get(getIdRoute(pageDirectoryId));
    }

    service.getAccessRulesByPageDirectoryId = function (pageDirectoryId) {

        return $http.get(getAccessRulesRoute(pageDirectoryId));
    }

    /* COMMANDS */

    service.add = function (command) {

        return $http.post(directoryServiceBase, command);
    }

    service.update = function (command) {

        return $http.patch(getIdRoute(command.pageDirectoryId), command);
    }

    service.remove = function (pageDirectoryId) {

        return $http.delete(getIdRoute(pageDirectoryId));
    }
    service.updateUrl = function (command) {

        return $http.put(getIdRoute(command.pageDirectoryId) + '/url', command);
    }

    service.updateAccessRules = function (command) {

        return $http.patch(getAccessRulesRoute(command.pageDirectoryId), command);
    }

    /* PRIVATES */

    function getIdRoute(pageDirectoryId) {
        return directoryServiceBase + '/' + pageDirectoryId;
    }

    function getAccessRulesRoute (pageDirectoryId) {
        return getIdRoute(pageDirectoryId) + '/access-rules';
    }

    return service;
}]);
angular.module('cms.directories').factory('directories.DirectoryTree', [
    '_',
function (
    _) {

    return DirectoryTree;

    /**
     * Represents a query for searching entities and returning a paged result, handling
     * the persistance of the query parameters in the query string.
     */
    function DirectoryTree(originalTree) {
        var me = this;

        _.extend(me, originalTree);

        /* Public Funcs */

        /**
         * Flattens the node tree into a single array of nodes, optionally
         * excluding the directory with the specified id.
         */
        me.flatten = function (pageDirectoryIdToExclude) {
            var allNodes = [];

            flattenNode(me, allNodes)

            return allNodes;

            function flattenNode(node, allNodes) {
                if (node.pageDirectoryId == pageDirectoryIdToExclude) return;
                allNodes.push(node);

                _.each(node.childPageDirectories, function (node) {
                    flattenNode(node, allNodes);
                });
            }
        }
        
        /**
         * Finds a directory node, searching through child nodes recursively.
         */
        me.findNodeById = function (pageDirectoryIdToFind) {
            return findDirectory([me]);

            function findDirectory(directories) {
                var result;

                if (!directories) return;

                directories.forEach(function (directory) {
                    if (result) return;

                    if (directory.pageDirectoryId == pageDirectoryIdToFind) {
                        result = directory;
                    } else {
                        result = findDirectory(directory.childPageDirectories);
                    }

                });

                return result;
            }
        }
    }
}]);
angular.module('cms.directories').directive('cmsDirectoryGrid', [
    'shared.permissionValidationService',
    'directories.modulePath',
function (
    permissionValidationService,
    modulePath) {

    return {
        restrict: 'E',
        templateUrl: modulePath + 'UIComponents/DirectoryGrid.html',
        scope: {
            pageDirectories: '=cmsDirectories',
            redirect: '=cmsRedirect'
        },
        replace: false,
        controller: Controller,
        controllerAs: 'vm',
        bindToController: true
    };

    /* CONTROLLER */

    function Controller() {
        var vm = this;

        vm.canUpdate = permissionValidationService.canUpdate('COFDIR');
    }

}]);
angular.module('cms.directories').directive('cmsDirectoryPath', [
    'directories.modulePath',
function (
    modulePath) {

    return {
        restrict: 'E',
        templateUrl: modulePath + 'UIComponents/DirectoryPath.html',
        scope: {
            pageDirectory: '=cmsDirectory'
        },
        replace: false,
        controller: Controller,
        controllerAs: 'vm',
        bindToController: true
    };

    /* CONTROLLER */

    function Controller() {
    }

}]);
angular.module('cms.directories').controller('ChangeDirectoryUrlController', [
    '$scope',
    '$q',
    'shared.LoadState',
    'directories.directoryService',
    'options',
    'close',
function (
    $scope,
    $q,
    LoadState,
    directoryService,
    options,
    close) {

    var vm = $scope;

    init();
    
    /* INIT */

    function init() {

        initData();

        vm.submitLoadState = new LoadState();

        vm.save = save;
        vm.close = close;
    }

    function initData() {
        var pageDirectory = options.pageDirectory;

        vm.pageDirectory = pageDirectory;
        vm.selectableParentDirectories = options.selectableParentDirectories;
        vm.hasChildContent = options.hasChildContent;
        vm.command = {
            pageDirectoryId: pageDirectory.pageDirectoryId,
            urlPath: pageDirectory.urlPath,
            parentPageDirectoryId: pageDirectory.parentPageDirectoryId
        };
    }

    /* EVENTS */

    function save() {
        vm.submitLoadState.on();

        directoryService
            .updateUrl(vm.command)
            .then(options.onSave)
            .then(close)
            .finally(vm.submitLoadState.off);
    }
}]);
angular.module('cms.directories').controller('AddDirectoryController', [
    '$location',
    'shared.stringUtilities',
    'shared.LoadState',
    'directories.directoryService',
function (
    $location,
    stringUtilities,
    LoadState,
    directoryService) {

    var vm = this;

    init();

    /* INIT */

    function init() {

        initData();

        vm.formLoadState = new LoadState(true);
        vm.globalLoadState = new LoadState();
        vm.editMode = false;

        vm.save = save;
        vm.cancel = cancel;
        vm.onDirectoriesLoaded = onDirectoriesLoaded;
    }

    /* EVENTS */

    function save() {
        vm.globalLoadState.on();

        directoryService
            .add(vm.command)
            .then(redirectToList, vm.globalLoadState.off);
    }

    function onDirectoriesLoaded() {
        vm.formLoadState.off();
    }

    function cancel() {
        redirectToList();
    }

    /* PRIVATE FUNCS */

    function redirectToList() {
        $location.path('/');
    }

    function initData() {
        vm.command = {};
    }
}]);
angular.module('cms.directories').controller('DirectoryDetailsController', [
    '$routeParams',
    '$q',
    '$location',
    '_',
    'shared.LoadState',
    'shared.modalDialogService',
    'shared.permissionValidationService',
    'shared.userAreaService',
    'shared.internalModulePath',
    'directories.modulePath',
    'directories.directoryService',
function (
    $routeParams,
    $q,
    $location,
    _,
    LoadState,
    modalDialogService,
    permissionValidationService,
    userAreaService,
    sharedModulePath,
    modulePath,
    directoryService
    ) {

    var vm = this,
        ENTITY_DEFINITION_CODE = 'COFDIR';

    init();
    
    /* INIT */

    function init() {

        // UI actions
        vm.edit = edit;
        vm.save = save;
        vm.cancel = reset;
        vm.deleteDirectory = deleteDirectory;
        vm.viewAccessRules = viewAccessRules;

        // Events
        vm.changeUrl = changeUrl;

        // Properties
        vm.editMode = false;
        vm.globalLoadState = new LoadState();
        vm.saveLoadState = new LoadState();
        vm.formLoadState = new LoadState(true);

        vm.canUpdate = permissionValidationService.canUpdate(ENTITY_DEFINITION_CODE);
        vm.canUpdateUrl = permissionValidationService.hasPermission(ENTITY_DEFINITION_CODE + 'UPDURL');
        vm.canDelete = permissionValidationService.canDelete(ENTITY_DEFINITION_CODE);

        // Init
        initData(vm.formLoadState);
    }

    /* UI ACTIONS */

    function edit() {
        vm.editMode = true;
        vm.mainForm.formStatus.clear();
    }

    function save() {
        setLoadingOn(vm.saveLoadState);

        directoryService.update(vm.command)
            .then(onSuccess.bind(null, 'Changes were saved successfully'))
            .finally(setLoadingOff.bind(null, vm.saveLoadState));
    }

    function reset() {
        vm.editMode = false;
        vm.command = mapUpdateCommand(vm.pageDirectory);
        vm.mainForm.formStatus.clear();
    }

    function deleteDirectory() {
        var options = {
            title: 'Delete Directory',
            message: 'Deleting this directory will delete ALL sub-directories and pages linked to this directory. Are you sure you want to continue?',
            okButtonTitle: 'Yes, delete it',
            onOk: onOk
        };

        modalDialogService.confirm(options);

        function onOk() {
            setLoadingOn();
            return directoryService
                .remove(vm.pageDirectory.pageDirectoryId)
                .then(redirectToList)
                .catch(setLoadingOff);
        }
    }

    function viewAccessRules() {

        modalDialogService.show({
            templateUrl: sharedModulePath + 'UIComponents/EntityAccess/EntityAccessEditor.html',
            controller: 'EntityAccessEditorController',
            options: {
                entityDefinitionCode: ENTITY_DEFINITION_CODE,
                entityIdPrefix: 'pageDirectory',
                entityDefinitionName: 'Directory',
                entityDescription: vm.pageDirectory.fullUrlPath,
                entityAccessLoader: directoryAccessLoader,
                saveAccess: directoryService.updateAccessRules
            }
        });

        function directoryAccessLoader() {
            return directoryService
                .getAccessRulesByPageDirectoryId(vm.pageDirectory.pageDirectoryId);
        }
    }

    function changeUrl() {

        modalDialogService.show({
            templateUrl: modulePath + 'Routes/Modals/ChangeDirectoryUrl.html',
            controller: 'ChangeDirectoryUrlController',
            options: {
                pageDirectory: vm.pageDirectory,
                selectableParentDirectories: vm.selectableParentDirectories,
                hasChildContent: vm.hasChildContent,
                onSave: onSuccess.bind(null, 'Url Changed')
            }
        });
    }

    /* PRIVATE FUNCS */

    function onSuccess(message) {
        return initData()
            .then(vm.mainForm.formStatus.success.bind(null, message));
    }

    function initData(loadStateToTurnOff) {
        var pageDirectoryId = $routeParams.id;

        return $q
            .all([getDirectory(), getUserAreas()])
            .then(setLoadingOff.bind(null, loadStateToTurnOff));

        function getDirectory() {
            return directoryService
                .getTree()
                .then(function loadDirectory(tree) {
                    var pageDirectory = tree.findNodeById(pageDirectoryId);
        
                    vm.pageDirectory = pageDirectory;
                    vm.parentDirectory = tree.findNodeById(pageDirectory.parentPageDirectoryId);
                    vm.selectableParentDirectories = tree.flatten(pageDirectoryId);
                    vm.command = mapUpdateCommand(pageDirectory);
                    vm.editMode = false;
                    vm.hasChildContent = pageDirectory.numPages || pageDirectory.childPageDirectories.length;
                });
        }

        function getUserAreas() {
            return userAreaService.getAll().then(function (userAreas) {
                vm.accessRulesEnabled = userAreas.length > 1;
            });
        }
    }

    function mapUpdateCommand(pageDirectory) {

        return _.pick(pageDirectory,
            'pageDirectoryId',
            'name'
            );
    }

    function redirectToList() {
        $location.path('');
    }

    function setLoadingOn(loadState) {
        vm.globalLoadState.on();
        if (loadState && _.isFunction(loadState.on)) loadState.on();
    }

    function setLoadingOff(loadState) {
        vm.globalLoadState.off();
        if (loadState && _.isFunction(loadState.off)) loadState.off();
    }
}]);
angular.module('cms.directories').controller('DirectoryListController', [
    '_',
    'shared.LoadState',
    'shared.SearchQuery',
    'shared.permissionValidationService',
    'directories.directoryService',
function (
    _,
    LoadState,
    SearchQuery,
    permissionValidationService,
    directoryService) {

    var vm = this;

    init();

    function init() {
        
        vm.gridLoadState = new LoadState();
        vm.canCreate = permissionValidationService.canCreate('COFDIR');

        loadGrid();
    }
    
    /* PRIVATE FUNCS */
    
    function loadGrid() {
        vm.gridLoadState.on();

        return directoryService.getTree().then(function (tree) {
            var result = tree.flatten();

            // remove the root directory
            vm.result = result;//.slice(1, result.length);
            vm.gridLoadState.off();
        });
    }

}]);