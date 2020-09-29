app.controller('CreateUserController', function ($scope, $location, $routeParams, UserService, RoleService, Scopes) {
    Scopes.store('CreateUserController', $scope);
    $scope.dateOptions = {
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        yearRange: '1900:-0',
    };
    $scope.Submited = false;
    $scope.UserModel = {
        DOB: new Date()
    };
    $scope.selectedItem;
    $scope.dropboxitemselected = function (item) {
        $scope.selectedItem = item;
        $scope.UserModel.UserRoles_Id = $scope.selectedItem.UserRoles_Id;
        $scope.UserModel.RolesName = $scope.selectedItem.RolesName;
    }

    //Check whether the form is valid or not using $watch
    $scope.$watch("EntryForm.$valid", function (TrueOrFalse) {
        $scope.IsFormValid = TrueOrFalse;   //returns true if form valid
    });

    $scope.Save = function () {
        $scope.Submited = true;
        if ($scope.IsFormValid) {
            UserService.Save($scope.UserModel).then(function (d) {
                if (d.data.Save === true) {
                    alert('Record save successful !');
                    $location.path('/users');
                }
                else {
                    $scope.SaveFailed = true;
                    $scope.Message = d.data.Message;
                }
            });
        }
    };
    $scope.Cancel = function () {
        $location.path('/users');
    };

    RoleService.getAllRoles($routeParams).then(function (d) {
        $scope.RolesList = d.data.roles;
        $scope.selectedItem = {
            UserRoles_Id: $scope.RolesList[0].UserRoles_Id,
            RolesName: $scope.RolesList[0].RolesName
        };
        $scope.UserModel.UserRoles_Id = $scope.selectedItem.UserRoles_Id;
    });
})