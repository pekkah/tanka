tankaAdmin.controller('SettingsCtrl', ['$scope', 'AdminApi', function ($scope, adminApi) {
    $scope.$emit("busy");
    adminApi.Settings.Get(
        function (data, status, headers, config) {
            $scope.Settings = data;
            $scope.$emit("done");
        },
        function (data, status, headers, config) {
            $scope.error = data;
        });

    $scope.save = function () {
        $scope.$emit("busy");
        
        adminApi.Settings.Save($scope.Settings,
        function (data, status, headers, config) {
            $scope.$emit("done");
        },
        function (data, status, headers, config) {
            
        });
    };
}]);