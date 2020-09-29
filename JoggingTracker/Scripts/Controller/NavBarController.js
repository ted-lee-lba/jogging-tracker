app.controller("NavBarController", function ($scope, $location, $window, LoginService, UserService, Scopes) {
    Scopes.store('NavBarController', $scope);
    $scope.IsLoggedIn = false;
    $scope.UserName = "";

    $scope.setLoggedStatus = function (state) {
        $scope.IsLoggedIn = state.IsLoggedIn;
        $scope.UserName = state.UserName;
    };

    $scope.Init = function () {
        LoginService.IsLoggedIn().then(function (d) {
            $scope.IsLoggedIn = d.data.IsLoggedIn;
            $scope.UserName = d.data.UserName;
            //if (!$scope.IsLoggedIn) {
            //    $location.path('/Login');
            //    return;
            //}
        });
    }

    $scope.Logoff = function () {
        LoginService.Logoff().then(function (d) {
            $scope.setLoggedStatus(d.data);
            $window.location.reload();
        });
    }

    $scope.ChangePass = function () {
        $location.path('/ChangePass');
    }
    

}).run(function ($rootScope, $route) {
    //rootScope.$route = $route;
});