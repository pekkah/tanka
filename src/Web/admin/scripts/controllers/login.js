tankaAdmin.controller('LoginCtrl', ['$scope', '$http', 'authService', function ($scope, $http, authService) {
    $scope.submit = function () {
        var request = {
            UserName: $scope.UserName,
            Password: $scope.Password
        };
        $scope.$emit("busy");
        $http.post("/api/auth", request).success(function () {
            authService.loginConfirmed();
            $scope.$emit("done");
        });
    };
}]);