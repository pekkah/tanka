(function () {
    'use strict';

    var module = angular.module('user', ['ngRoute', 'toaster']);
    
    module.config(['$routeProvider', function($routeProvider) {
        $routeProvider.when('/user/current/password',
            {
                templateUrl: '/admin/views/user-change-password.html',
                controller: 'ChangePasswordCtrl'
            });
    }]);

    module.controller('ChangePasswordCtrl', ['$scope', '$http', 'toaster', function ($scope, $http, toaster) {
        $scope.password = '';
        $scope.verification = '';
        $scope.canSave = false;

        $scope.$watch('verification', function(newValue, oldValue) {
            if ($scope.password == '') {
                $scope.canSave = false;
                return;
            }

            if ($scope.password == newValue) {
                $scope.canSave = true;
            } else {
                $scope.canSave = false;
            }
        });

        $scope.changePassword = function() {
            $http.post('/api/users/current/password', {
                password: $scope.password
            })
            .success(function() {
                toaster.pop('success', "Password changed");
            })
            .error(function(response) {
                toaster.pop('error', "Failed to change the password. Check the requirements.");
            });
        };
    }]);

})();
