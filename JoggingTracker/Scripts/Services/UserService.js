app.factory("UserService", function ($http) {
    //initilize factory object.
    var fact = {};
    var _role = "";
    fact.getAllUser = function (d) {
        return $http({
            url: 'api/Users',
            method: 'GET',
            params:d,
            headers: { 'content-type': 'application/json' }
        });
    };
    fact.getUser = function (d) {
        return $http({
            url: 'api/getuser',
            method: 'GET',
            params: d,
            headers: { 'content-type': 'application/json' }
        });
    };
    fact.delete = function (data) {
        return $http({
            url: 'api/deleteuser',
            method: 'POST',
            data: JSON.stringify(data),
            headers: { 'content-type': 'application/json' }
        });
    };
    fact.Save = function (data) {
        return $http({
            url: 'api/saveuser',
            method: 'POST',
            data: JSON.stringify(data),
            headers: { 'content-type': 'application/json' }
        });
    };
    fact.ChangePass = function (data) {
        return $http({
            url: 'api/ChangePass',
            method: 'POST',
            data: JSON.stringify(data),
            headers: { 'content-type': 'application/json' }
        });
    };
    fact.getMeAndAll = function () {
        return $http({
            url: 'api/getmeandalluser',
            method: 'GET',
            headers: { 'content-type': 'application/json' }
        });
    };
    fact.getShowUserListFlag = function () {
        return $http({
            url: 'api/getShowUserListFlag',
            method: 'GET',
            headers: { 'content-type': 'application/json' }
        });
    };
    return fact;
});