(function() {
    'use strict';

    var module = angular.module('users', ['ngRoute', 'toaster']);

    module.config([
        '$routeProvider', function($routeProvider) {
            $routeProvider.when('/users', {
                templateUrl: '/admin/views/users.html',
                controller: 'UsersCtrl'
            });
        }
    ]);

    module.controller('UsersCtrl', [
        '$scope', '$http', 'toaster', '$modal',
        function($scope, $http, toaster, $modal) {
            $scope.delete = function(user) {
                $http.delete('api/users/' + user.Id)
                    .success(function(response) {
                        toaster.pop('success', 'User deleted');
                        getUsers();
                    })
                    .error(function(response) {
                        toaster.pop('error', 'Failed to delete user');
                        getUsers();
                    });
            };

            $scope.addUser = function() {
                var modalInstance = $modal.open({
                    templateUrl: '/admin/templates/add-user.html',
                    controller: 'AddUserModalCtrl'
                });

                modalInstance.result.then(function () {
                    getUsers();
                }, function () {
                    
                });
            };

            function getUsers() {
                $http.get('api/users').success(function(response) {
                    $scope.users = response.Users;
                });
            };

            getUsers();
        }
    ]);

    module.controller('AddUserModalCtrl',
        ['$scope','$http', '$modalInstance', 'toaster',
        function ($scope, $http, $modalInstance, toaster) {
            $scope.user = {};

            $scope.add = function () {
                $http.post('api/users', { UserName: $scope.user.username, Password: $scope.user.password })
                    .success(function() {
                        toaster.pop('success', 'User added');
                        $modalInstance.close();
                });

            };

            $scope.cancel = function () {
                $modalInstance.dismiss();
            };
        }
    ]);

})();