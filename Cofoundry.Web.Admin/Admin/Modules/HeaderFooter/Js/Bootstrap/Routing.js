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