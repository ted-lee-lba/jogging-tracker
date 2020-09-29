app.controller("JoggingTrackListController",
    ['$scope', '$location', '$window', 'UserService', 'JoggingTrackService', 'Scopes',
        function ($scope, $location, $window, UserService, JoggingTrackService, Scopes) {
            var currentMoment = moment();
    $scope.DateFilter = {
        FromDateTime: currentMoment.subtract(7, 'days'),
        ToDateTime: moment()
    };
    $scope.ShowUserList = 0;
    $scope.UserList = {};
    $scope.selectedUser = {};

    $scope.dropboxitemselected = function (item) {
        $scope.selectedUser = item;
    }

    $scope.$watch("selectedUser", function (newValue) {
        this.getRecord();

    });

    $scope.$watch("DateFilter.FromDateTime", function (newValue) {
        this.getRecord();
    });

    $scope.$watch("DateFilter.ToDateTime", function (newValue) {
        this.getRecord();

    });

    $scope.JoggingTrackList = [];

    $scope.delete = function (row) {
        var _bYes = confirm("Are you sure to delete this record?");
        if (_bYes) {
            JoggingTrackService.delete({
                Users_Id: $scope.selectedUser.Users_Id,
                JoggingTrack_Id: row.JoggingTrack_Id
            }).then(function (d) {
                if (d.data.Delete) {
                    $scope.JoggingTrackList = d.data.DataList;

                } else {
                    $scope.Message = d.data.Message;

                }
            });
        }
    };

    $scope.edit = function (row) {
        $location.path('/EditJoggingTrack/' + row.JoggingTrack_Id);
    };
    $scope.add = function () {        
        $location.path('/AddJoggingTrack/' + $scope.selectedUser.Users_Id);
    }

    getRecord = function () {
        if (angular.equals({}, $scope.selectedUser)) {
            return;
        }

        JoggingTrackService.getRecords({
            Id: $scope.selectedUser.Users_Id,
            FromDateTime: $scope.DateFilter.FromDateTime,
            ToDateTime: $scope.DateFilter.ToDateTime
        }).then(function (d) {
            $scope.JoggingTrackList = d.data.DataList;
            $scope.WeeklyList = d.data.WeeklyReport;
        });

    };

    $scope.fromDateBeforeRender = function ($view, $dates, $leftDate, $upDate, $rightDate) {
        if ($scope.DateFilter.ToDateTime) {
            var activeDate = moment($scope.DateFilter.ToDateTime);

            $dates.filter(function (date) {
                return date.localDateValue() >= activeDate.valueOf()
            }).forEach(function (date) {
                date.selectable = false;
            })
        }
    }

    $scope.toDateBeforeRender = function ($view, $dates, $leftDate, $upDate, $rightDate) {
        if ($scope.DateFilter.FromDateTime) {
            var activeDate = moment($scope.DateFilter.FromDateTime).subtract(1, $view).add(1, 'minute');

            $dates.filter(function (date) {
                return date.localDateValue() <= activeDate.valueOf()
            }).forEach(function (date) {
                date.selectable = false;
            })
        }
    }

    UserService.getShowUserListFlag().then(function (d) {
        $scope.ShowUserList = d.data.ShowUserList;
    });

    UserService.getMeAndAll().then(function (d) {
        $scope.UserList = d.data.userList;
        $scope.selectedUser = d.data.userList[0];
    });
}]);