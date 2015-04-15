tankaAdmin.controller('SettingsCtrl', ['$scope', 'AdminApi', 'toaster', function($scope, adminApi, toaster) {
    adminApi.Settings.Get()
        .success(
            function(data, status, headers, config) {
                $scope.Settings = data;
                $scope.$emit("done");
            })
        .error(
            function(data, status, headers, config) {
                $scope.error = data;
            });

    $scope.save = function() {
        $scope.$emit("busy");

        adminApi.Settings.Save($scope.Settings)
            .success(
                function(data, status) {
                    toaster.pop('success', "Settings saved");
                })
            .error(
                function(data, status) {
                    toaster.pop('error', 'Failed to save settings');
                });
    };
}]);