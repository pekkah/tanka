tankaAdmin.controller('BlogPostsCtrl', ['$scope', 'AdminApi', function ($scope, adminApi) {
    $scope.$emit("busy");
    adminApi.BlogPosts.All(
        function (data, status, headers, config) {
            $scope.BlogPosts = data;
            $scope.$emit("done");
        },
        function (data, status, headers, config) {
            $scope.error = data;
        });

    $scope.selectedBlogPost = null;

    $scope.selectBlogPost = function(post) {
        $scope.selectedBlogPost = post;
    };
}]);