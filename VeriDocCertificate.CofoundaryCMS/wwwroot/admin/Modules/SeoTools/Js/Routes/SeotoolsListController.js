﻿angular.module('cms.seotools').controller('seotoolsListController', [
    '_',
    'shared.LoadState',
    'shared.SearchQuery',
    'shared.permissionValidationService',
    'seotools.seotoolsService',
function (
    _,
    LoadState,
    SearchQuery,
    permissionValidationService,
    seotoolsService) {

    var vm = this;

    init();

    function init() {
        
        //vm.gridLoadState = new LoadState();
        //vm.canCreate = permissionValidationService.canCreate('COFDIR');

        //loadGrid();
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