tankaAdmin.controller('CmsCtrl', ['$scope', 'AdminApi', 'PublicApi',function ($scope, adminApi, publicApi) {
    $scope.$emit("busy");
    
    adminApi.Cms.GetAll(
        function (contents) {
            $scope.Contents = contents;
            $scope.$emit("done");
        },
        function (error) {
            $scope.error = error;
        });
}]);

tankaAdmin.controller('CmsCreateCtrl', ['$scope', 'AdminApi', '$location',function ($scope, adminApi, $location) {

    $scope.save = function() {
        adminApi.Cms.Save($scope.Content, function(result) {
            $location.path('cms/' + result.Id);
        });
    };

}]);

tankaAdmin.controller('CmsEditCtrl', ['$scope', 'AdminApi', '$routeParams', '$location',function ($scope, adminApi, $routeParams, $location) {

    adminApi.Cms.Get($routeParams.id, function(content) {
        $scope.Content = content;
    });

    $scope.save = function () {
        adminApi.Cms.Save($scope.Content, function (content) {
            $scope.Content = content;
        });
    };

    $scope.delete = function() {
        adminApi.Cms.Delete($scope.Content.Id, function(result) {
            $location.path('cms');
        });
    };

}]);

