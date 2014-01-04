tankaAdmin.controller('ConfigurationCtrl', ['$scope', 'AdminApi', function ($scope, adminApi) {
    $scope.$emit("busy");
    adminApi.Configuration.Get(
        function (data, status, headers, config) {
            $scope.Configuration = data;
            $scope.$emit("done");
        },
        function (data, status, headers, config) {
            $scope.error = data;
        });

    $scope.save = function () {
        $scope.$emit("busy");
        
        adminApi.Configuration.Save($scope.Configuration,
        function (data, status, headers, config) {
            $scope.$emit("done");
        },
        function (data, status, headers, config) {
            
        });
    };
}]);