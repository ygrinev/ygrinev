var AngularMVCApp = angular.module('AngularMVCApp', ['ngRoute']);

AngularMVCApp.controller('LandingPageController', LandingPageController);

var configFunc = function ($routeProvider) {
    $routeProvider
        .when('/routeOne', { templateUrl: 'routesDemo/one' })
        .when('/routeTwo', { templateUrl: 'routesDemo/two' })
        .when('/routeThree', { templateUrl: 'routesDemo/three' });
}
configFunc.$inject = ['$routeProvider'];

AngularMVCApp.config(configFunc);