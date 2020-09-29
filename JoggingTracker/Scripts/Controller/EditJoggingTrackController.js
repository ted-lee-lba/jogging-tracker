app.controller('EditJoggingTrackController', function ($scope, $location, $routeParams, UserService, JoggingTrackService, Scopes) {
    Scopes.store('EditJoggingTrackController', $scope);
    $scope.JoggingTrackModel = {};

    //Check whether the form is valid or not using $watch
    $scope.$watch("EntryForm.$valid", function (TrueOrFalse) {
        $scope.IsFormValid = TrueOrFalse;   //returns true if form valid
    });

    $scope.Save = function () {
        $scope.Submited = true;
        if ($scope.IsFormValid) {
            JoggingTrackService.save($scope.JoggingTrackModel).then(function (d) {
                if (d.data.Save === true) {
                    alert('Record save successful !');
                    $location.path('/JoggingTrack');
                }
                else {
                    $scope.SaveFailed = true;
                    $scope.Message = d.data.Message;
                }
            });
        }
    };

    $scope.Cancel = function () {
        $location.path('/JoggingTrack');
    }

    $scope.fromDateTimeBeforeRender = function ($view, $dates, $leftDate, $upDate, $rightDate) {
        if ($scope.JoggingTrackModel.ToDateTime) {
            var activeDate = moment($scope.JoggingTrackModel.ToDateTime);

            $dates.filter(function (date) {
                return date.localDateValue() >= activeDate.valueOf()
            }).forEach(function (date) {
                date.selectable = false;
            })
        }
    }

    $scope.toDateTimeBeforeRender = function ($view, $dates, $leftDate, $upDate, $rightDate) {
        if ($scope.JoggingTrackModel.FromDateTime) {
            var activeDate = moment($scope.JoggingTrackModel.FromDateTime).subtract(1, $view).add(1, 'minute');

            $dates.filter(function (date) {
                return date.localDateValue() <= activeDate.valueOf()
            }).forEach(function (date) {
                date.selectable = false;
            })
        }
    }

    JoggingTrackService.getJoggingTrack($routeParams).then(function (d) {
        $scope.JoggingTrackModel = d.data.JoggingTrack;
    });
})