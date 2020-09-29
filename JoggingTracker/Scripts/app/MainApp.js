var app = angular.module('myApp', [
    'ngRoute', 'ui.bootstrap','ui.bootstrap.datetimepicker','ui.dateTimeInput'
]).config(['$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {

    $httpProvider.defaults.withCredentials = true;
    $routeProvider.when('/', {
        templateUrl: '/Home/JoggingTrack',
        activetab: "home"

    }).when('/JoggingTrack', {
        templateUrl: '/Home/JoggingTrack',

    }).when('/AddJoggingTrack/:id', {
        templateUrl: '/Home/CreateJoggingTrack',

    }).when('/EditJoggingTrack/:id', {
        templateUrl: '/Home/EditJoggingTrack'

    }).when('/users', {
        templateUrl: '/Home/Users',
        activetab: "users",

    }).when('/EditUser/:id', {
        templateUrl: '/Home/EditUser',

    }).when('/AddUser', {
        templateUrl: '/Home/CreateUser',

    }).when('/ChangePass', {
        templateUrl: '/Home/ChangePassword',

    }).when('/Login', {
        templateUrl: '/Home/Login',
        //controller: 'LoginController'

    }).when('/AccessDenied', {
        templateUrl: '/Home/AccessDenied',
        //controller: 'LoginController'

    }).otherwise({
        redirectTo: '/'
    });
    // Specify HTML5 mode (using the History APIs) or HashBang syntax.
    $locationProvider.html5Mode(false).hashPrefix('');
    $httpProvider.interceptors.push('authHttpResponseInterceptor');
}]);

app.factory('authHttpResponseInterceptor', ['$q', '$location', function ($q, $location) {

    return {
        response: function (response) {
            return response || $q.when(response);
        },
        responseError: function (rejection) {
            if (rejection.status === 401) {
                if (rejection.headers("IsAuthenticated") === "1") {
                    $location.path("/AccessDenied");

                } else {
                    $location.path("/Login");

                }
            }

            return $q.reject(rejection);
        }
    };
}]);

app.factory('Scopes', function ($rootScope) {
    var mem = {};

    return {
        store: function (key, value) {
            $rootScope.$emit('scope.stored', key);
            mem[key] = value;
        },
        get: function (key) {
            return mem[key];
        }
    };
});

app.directive("compareTo", function () {
    return {
        require: "ngModel",
        scope: {
            otherModelValue: "=compareTo"
        },
        link: function (scope, element, attributes, ngModel) {
            ngModel.$validators.compareTo = function (modelValue) {
                return modelValue == scope.otherModelValue;
            };

            scope.$watch("otherModelValue", function () {
                ngModel.$validate();
            });
        }
    };
});