app.factory("JoggingTrackService", function ($http) {
    //initilize factory object.
    var fact = {};
    fact.getRecords = function (d) {
        return $http({
            url: 'api/JoggingTrack',
            method: 'GET',
            params:d,
            headers: { 'content-type': 'application/json' }
        });
    };
    fact.getJoggingTrack = function (d) {
        return $http({
            url: 'api/getJoggingTrack',
            method: 'GET',
            params: d,
            headers: { 'content-type': 'application/json' }
        });
    };
    fact.delete = function (data) {
        return $http({
            url: 'api/deletejoggingtrack',
            method: 'POST',
            data: JSON.stringify(data),
            headers: { 'content-type': 'application/json' }
        });
    };
    fact.save = function (data) {
        return $http({
            url: 'api/savejoggingtrack',
            method: 'POST',
            data: JSON.stringify(data),
            headers: { 'content-type': 'application/json' }
        });
    };
    return fact;
});