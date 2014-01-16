tankaAdmin.controller('SettingsCtrl', ['$scope', 'AdminApi', 'toaster', function ($scope, adminApi, toaster) {
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
            toaster.pop('success', "Settings saved");
        },
        function (data, status, headers, config) {
            toaster.pop('error', 'Failed to save settings');
        });
    };
}]);