app.controller('ChangePassController', function ($scope, $location, $routeParams, UserService, Scopes) {
    $scope.ChangePassModel = {};
    $scope.Submited = false;
    //Check whether the form is valid or not using $watch
    $scope.$watch("EntryForm.$valid", function (TrueOrFalse) {
        $scope.IsFormValid = TrueOrFalse;   //returns true if form valid
    });

    $scope.Save = function () {
        $scope.Submited = true;
        if ($scope.IsFormValid) {
            UserService.ChangePass($scope.ChangePassModel).then(function (d) {
                if (d.data.ChangeSuccess === true) {
                    $scope.ChangeFailed = false;
                    alert('Record save successful !');
                }
                else {
                    $scope.ChangeFailed = true;
                    $scope.Message = d.data.Message;
                }
            });
        }
    };
})