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

    //service.getAll = function () {
    //    return $http.get(directoryServiceBase);
    //}

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

     this.update = function (command) {
         debugger;
       // return $http.patch(getIdRoute(command.pageDirectoryId), command);
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