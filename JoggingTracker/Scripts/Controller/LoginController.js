app.controller('LoginController', function ($scope, $location, UserService, LoginService, Scopes) {
    Scopes.store('LoginController', $scope);

    //initilize user data object
    $scope.LoginData = {
        UserName: '',
        Password: ''
    }
    $scope.msg = "";
    $scope.Submited = false;
    $scope.IsLoggedIn = false;
    $scope.IsFormValid = false;
    $scope.LoginFailed = false;

    //Check whether the form is valid or not using $watch
    $scope.$watch("LoginForm.$valid", function (TrueOrFalse) {
        $scope.IsFormValid = TrueOrFalse;   //returns true if form valid
    });

    $scope.$watch("IsLoggedIn", function (TrueOrFalse) {

    });

    $scope.SubmitForm = function () {
        $scope.Submited = true;
        if ($scope.IsFormValid) {
            LoginService.IsValidUser($scope.UserModel).then(function (d) {
                Scopes.get('NavBarController').setLoggedStatus(d.data);
                if (d.data.IsValidUser === true) {
                    $location.path('/Home/JoggingTrack');
                }
                else {
                    $scope.LoginFailed = true;
                    $scope.msg = "Invalid user name or password."
                }
            });
        }
    }
})