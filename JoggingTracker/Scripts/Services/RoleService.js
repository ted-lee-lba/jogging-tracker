app.factory("RoleService", function ($http) {
    //initilize factory object.
    var fact = {};
    fact.getAllRoles = function () {
        return $http({
            url: 'api/Roles',
            method: 'GET',
            headers: { 'content-type': 'application/json' }
        });
    };
    return fact;
});