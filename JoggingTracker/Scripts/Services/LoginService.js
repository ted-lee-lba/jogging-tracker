app.factory("LoginService", function ($http) {
    //initilize factory object.
    var fact = {};
    fact.IsValidUser = function (d) {
        return $http({
            url: 'api/Login',
            method: 'GET',
            params:d,
            headers: { 'content-type': 'application/json' }
        });
    };
    fact.IsLoggedIn = function () {
        return $http({
            url: 'api/IsLoggedIn',
            method: 'GET',
            headers: { 'content-type': 'application/json' }
        });
    };
    fact.Logoff = function () {
        return $http({
            url: 'api/Logoff',
            method: 'POST',
            headers: { 'content-type': 'application/json' }
        });
    }
    return fact;
});