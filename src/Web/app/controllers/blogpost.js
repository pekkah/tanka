tankaApp.controller('CommentsCtrl', ['$scope', 'PublicApi', '$routeParams', function ($scope, publicApi, $routeParams) {
    $scope.$emit("busy");
    
    $scope.commentingEnabled = false;
    $scope.Comment = {
        Author:"",
        Content:""
    };

    publicApi.Comments.ForBlogPost($scope.BlogPostId,
                function (comments) {
                    $scope.Comments = comments;
                    $scope.$emit("done");
                },
                function (error) {
                    $scope.error = error;
                });

    $scope.postComment = function () {
        if (!$scope.commentingEnabled)
            return;
        $scope.$emit("busy");

        $scope.Comment.BlogPostId = $scope.BlogPost.Id;
        publicApi.Comments.Post($scope.Comment,
            function (data, status, headers, config) {
                $scope.Comments.unshift({
                    Author: $scope.Comment.Author,
                    Content: $scope.Comment.Content,
                    Created: Date.now()
                });

                $scope.Comment = {
                    Author:"",
                    Content:""
                };
                $scope.$emit("done");
            },
            function (error) {

            });
    };
}]);
