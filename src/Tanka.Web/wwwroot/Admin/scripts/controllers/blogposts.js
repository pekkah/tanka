tankaAdmin.controller('BlogPostsCtrl', ['$scope', 'AdminApi', function($scope, adminApi) {
    adminApi.BlogPosts.All()
        .success(
            function(data, statug) {
                $scope.BlogPosts = data;
                $scope.$emit("done");
            })
        .error(
            function(data, status) {
                $scope.error = data;
            });

    $scope.selectedBlogPost = null;

    $scope.selectBlogPost = function(post) {
        adminApi.BlogPosts.Render(post.Id)
            .success(function(renderedPost) {
                $scope.selectedBlogPost = renderedPost;
            });
    };
}]);