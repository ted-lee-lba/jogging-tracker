app.controller("UserListController", ['$scope', '$location', '$window', 'UserService', 'Scopes',
    function ($scope, $location, $window, UserService, Scopes) {

    $scope.UserList = [];
    $scope.gridOptions = {
        data: 'UserList',
        columnDefs: [
            {
                field: '',
                displayName: 'Delete',
                cellTemplate:
                   '<div class="ngCellText" ng-class="col.colIndex()">' +
                   '<a class="glyphicon glyphicon-remove" ng-click="delete(row)"></a>' +
                   '<a class="glyphicon glyphicon-edit" ng-click="edit(row)"></a>' +
                   '</div>'
            },
            {
                field: 'UserName', displayName: 'User Name'
            },
            {
                field: 'RolesName', displayName: 'Role'
            },
            {
                field: 'FullName', displayName: 'Name'
            }
        ],
    };

    $scope.delete = function (row) {
        var _bYes = confirm("Are you sure to delete this record?");
        if (_bYes) {
            UserService.delete(row).then(function (d) {
                if (d.data.Delete) {
                    $scope.UserList = d.data.DataList;

                } else {
                    $scope.Message = d.data.Message;

                }
            });
        }
    };

    $scope.edit = function (row) {
        $location.path('/EditUser/' + row.Users_Id);
    };

    UserService.getAllUser().then(function (d) {
        $scope.UserList = d.data.DataList;
    });
}]);