﻿angular.module('cms.directories').controller('AddDirectoryController', [
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