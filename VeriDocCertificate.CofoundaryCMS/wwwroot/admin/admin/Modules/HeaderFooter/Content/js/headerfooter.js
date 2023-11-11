angular
    .module('cms.headerfooter', ['ngRoute', 'cms.shared'])
    .constant('_', window._)
    .constant('headerfooter.modulePath', '/Admin/Modules/HeaderFooter/Js/');
angular.module('cms.headerfooter').config([
    '$routeProvider',
    'shared.routingUtilities',
    'headerfooter.modulePath',
function (
    $routeProvider,
    routingUtilities,
    modulePath) {

    routingUtilities.registerCrudRoutes($routeProvider, modulePath, 'HeaderFooter');
}]);
//angular.module('cms.headerfooter').factory('headerfooter.headerfooterService', [
//    '$http',
//    '_',
//    'shared.serviceBase',
//    'directories.DirectoryTree',
//function (
//    $http,
//    _,
//    serviceBase,
//    DirectoryTree) {

//    var service = {},
//        directoryServiceBase = serviceBase + 'page-directories';

//    /* QUERIES */

//    service.getAll = function () {
//        return $http.get(directoryServiceBase);
//    }

//    service.getTree = function () {
//        return $http.get(directoryServiceBase + '/tree').then(function (tree) {
//            return tree ? new DirectoryTree(tree) : tree;
//        });
//    }

//    service.getById = function (pageDirectoryId) {

//        return $http.get(getIdRoute(pageDirectoryId));
//    }

//    service.getAccessRulesByPageDirectoryId = function (pageDirectoryId) {

//        return $http.get(getAccessRulesRoute(pageDirectoryId));
//    }

//    /* COMMANDS */

//    service.add = function (command) {

//        return $http.post(directoryServiceBase, command);
//    }

//    service.update = function (command) {

//        return $http.patch(getIdRoute(command.pageDirectoryId), command);
//    }

//    service.remove = function (pageDirectoryId) {

//        return $http.delete(getIdRoute(pageDirectoryId));
//    }
//    service.updateUrl = function (command) {

//        return $http.put(getIdRoute(command.pageDirectoryId) + '/url', command);
//    }

//    service.updateAccessRules = function (command) {

//        return $http.patch(getAccessRulesRoute(command.pageDirectoryId), command);
//    }

//    /* PRIVATES */

//    function getIdRoute(pageDirectoryId) {
//        return directoryServiceBase + '/' + pageDirectoryId;
//    }

//    function getAccessRulesRoute (pageDirectoryId) {
//        return getIdRoute(pageDirectoryId) + '/access-rules';
//    }

//    return service;
//}]);
 
//angular.module('cms.headerfooter').controller('AddHeaderFooterController', [
//    '$location',
//    'shared.stringUtilities',
//    'shared.LoadState',
//    'headerfooter.headerfooterService',
//function (
//    $location,
//    stringUtilities,
//    LoadState,
//    directoryService) {

//    var vm = this;

//    init();

//    /* INIT */

//    function init() {

//        initData();

//        vm.formLoadState = new LoadState(true);
//        vm.globalLoadState = new LoadState();
//        vm.editMode = false;

//        vm.save = save;
//        vm.cancel = cancel;
//        vm.onDirectoriesLoaded = onDirectoriesLoaded;
//    }

//    /* EVENTS */

//    function save() {
//        vm.globalLoadState.on();

//        directoryService
//            .add(vm.command)
//            .then(redirectToList, vm.globalLoadState.off);
//    }

//    function onDirectoriesLoaded() {
//        vm.formLoadState.off();
//    }

//    function cancel() {
//        redirectToList();
//    }

//    /* PRIVATE FUNCS */

//    function redirectToList() {
//        $location.path('/');
//    }

//    function initData() {
//        vm.command = {};
//    }
//}]);
//angular.module('cms.headerfooter').controller('HeaderFooterDetailsController', [
//    '$routeParams',
//    '$q',
//    '$location',
//    '_',
//    'shared.LoadState',
//    'shared.modalDialogService',
//    'shared.permissionValidationService',
//    'shared.userAreaService',
//    'shared.internalModulePath',
//    'directories.modulePath',
//    'headerfooter.headerfooterService',
//function (
//    $routeParams,
//    $q,
//    $location,
//    _,
//    LoadState,
//    modalDialogService,
//    permissionValidationService,
//    userAreaService,
//    sharedModulePath,
//    modulePath,
//    directoryService
//    ) {

//    var vm = this,
//        ENTITY_DEFINITION_CODE = 'COFCHF';

//    init();
    
//    /* INIT */

//    function init() {

//        // UI actions
//        vm.edit = edit;
//        vm.save = save;
//        vm.cancel = reset;
//        vm.deleteDirectory = deleteDirectory;
//        vm.viewAccessRules = viewAccessRules;

//        // Events
//        vm.changeUrl = changeUrl;

//        // Properties
//        vm.editMode = false;
//        vm.globalLoadState = new LoadState();
//        vm.saveLoadState = new LoadState();
//        vm.formLoadState = new LoadState(true);

//        vm.canUpdate = permissionValidationService.canUpdate(ENTITY_DEFINITION_CODE);
//        vm.canUpdateUrl = permissionValidationService.hasPermission(ENTITY_DEFINITION_CODE + 'UPDURL');
//        vm.canDelete = permissionValidationService.canDelete(ENTITY_DEFINITION_CODE);

//        // Init
//        initData(vm.formLoadState);
//    }

//    /* UI ACTIONS */

//    function edit() {
//        vm.editMode = true;
//        vm.mainForm.formStatus.clear();
//    }

//    function save() {
//        setLoadingOn(vm.saveLoadState);

//        directoryService.update(vm.command)
//            .then(onSuccess.bind(null, 'Changes were saved successfully'))
//            .finally(setLoadingOff.bind(null, vm.saveLoadState));
//    }

//    function reset() {
//        vm.editMode = false;
//        vm.command = mapUpdateCommand(vm.pageDirectory);
//        vm.mainForm.formStatus.clear();
//    }

//    function deleteDirectory() {
//        var options = {
//            title: 'Delete Directory',
//            message: 'Deleting this directory will delete ALL sub-directories and pages linked to this directory. Are you sure you want to continue?',
//            okButtonTitle: 'Yes, delete it',
//            onOk: onOk
//        };

//        modalDialogService.confirm(options);

//        function onOk() {
//            setLoadingOn();
//            return directoryService
//                .remove(vm.pageDirectory.pageDirectoryId)
//                .then(redirectToList)
//                .catch(setLoadingOff);
//        }
//    }

//    function viewAccessRules() {

//        modalDialogService.show({
//            templateUrl: sharedModulePath + 'UIComponents/EntityAccess/EntityAccessEditor.html',
//            controller: 'EntityAccessEditorController',
//            options: {
//                entityDefinitionCode: ENTITY_DEFINITION_CODE,
//                entityIdPrefix: 'pageDirectory',
//                entityDefinitionName: 'Directory',
//                entityDescription: vm.pageDirectory.fullUrlPath,
//                entityAccessLoader: directoryAccessLoader,
//                saveAccess: directoryService.updateAccessRules
//            }
//        });

//        function directoryAccessLoader() {
//            return directoryService
//                .getAccessRulesByPageDirectoryId(vm.pageDirectory.pageDirectoryId);
//        }
//    }

//    function changeUrl() {

//        modalDialogService.show({
//            templateUrl: modulePath + 'Routes/Modals/ChangeDirectoryUrl.html',
//            controller: 'ChangeDirectoryUrlController',
//            options: {
//                pageDirectory: vm.pageDirectory,
//                selectableParentDirectories: vm.selectableParentDirectories,
//                hasChildContent: vm.hasChildContent,
//                onSave: onSuccess.bind(null, 'Url Changed')
//            }
//        });
//    }

//    /* PRIVATE FUNCS */

//    function onSuccess(message) {
//        return initData()
//            .then(vm.mainForm.formStatus.success.bind(null, message));
//    }

//    function initData(loadStateToTurnOff) {
//        var pageDirectoryId = $routeParams.id;

//        return $q
//            .all([getDirectory(), getUserAreas()])
//            .then(setLoadingOff.bind(null, loadStateToTurnOff));

//        function getDirectory() {
//            return directoryService
//                .getTree()
//                .then(function loadDirectory(tree) {
//                    var pageDirectory = tree.findNodeById(pageDirectoryId);
        
//                    vm.pageDirectory = pageDirectory;
//                    vm.parentDirectory = tree.findNodeById(pageDirectory.parentPageDirectoryId);
//                    vm.selectableParentDirectories = tree.flatten(pageDirectoryId);
//                    vm.command = mapUpdateCommand(pageDirectory);
//                    vm.editMode = false;
//                    vm.hasChildContent = pageDirectory.numPages || pageDirectory.childPageDirectories.length;
//                });
//        }

//        function getUserAreas() {
//            return userAreaService.getAll().then(function (userAreas) {
//                vm.accessRulesEnabled = userAreas.length > 1;
//            });
//        }
//    }

//    function mapUpdateCommand(pageDirectory) {

//        return _.pick(pageDirectory,
//            'pageDirectoryId',
//            'name'
//            );
//    }

//    function redirectToList() {
//        $location.path('');
//    }

//    function setLoadingOn(loadState) {
//        vm.globalLoadState.on();
//        if (loadState && _.isFunction(loadState.on)) loadState.on();
//    }

//    function setLoadingOff(loadState) {
//        vm.globalLoadState.off();
//        if (loadState && _.isFunction(loadState.off)) loadState.off();
//    }
//}]);
angular.module('cms.headerfooter').controller('HeaderFooterListController', [
    '_',
    'shared.LoadState',
    'shared.SearchQuery',
    'shared.permissionValidationService',
    'headerfooter.headerfooterService',
function (
    _,
    LoadState,
    SearchQuery,
    permissionValidationService,
    headerfooterService) {

    var vm = this;

    init();

    function init() {
        
       // vm.gridLoadState = new LoadState();
        vm.canCreate = permissionValidationService.canCreate('COFCHF');

       // loadGrid();
    }
    
    /* PRIVATE FUNCS */
    
    function loadGrid() {
        //vm.gridLoadState.on();

        //return directoryService.getTree().then(function (tree) {
        //    var result = tree.flatten();

        //    // remove the root directory
        //    vm.result = result;//.slice(1, result.length);
        //    vm.gridLoadState.off();
        //});
    }

}]);