tankaAdmin.controller("BlogPostCtrl",
    ['$scope', 'AdminApi', '$routeParams', '$location', '$timeout', function($scope, adminApi, $routeParams, $location, $timeout) {

        $scope.States = ["Draft", "Published"];

        var watchSlug = function() {
            $scope.$watch('BlogPost.Title',
                function(newValue, oldValue) {
                    if (newValue == undefined) {
                        return;
                    }

                    $timeout(function() {
                        adminApi.Utils.Slugify(newValue, function(response) {
                            $scope.BlogPost.Slug = response.Slug;
                        },
                            function(error) {
                                $scope.BlogPost.Slug = '';
                            });
                    }, 250);
                });
        };

        watchSlug();

        if ($routeParams.id != undefined) {
            $scope.$emit("busy");
            adminApi.BlogPosts.Single($routeParams.id,
                function (data, status, headers, config) {
                    $scope.BlogPost = data;
                    $scope.Tags = data.Tags.join(',');
                    $scope.$emit("done");
                },
                function(data, status, headers, config) {
                    $scope.error = data;
                });
        } else {
            $scope.BlogPost = { State: "Draft" };
        }

        $scope.save = function () {
            $scope.$emit("busy");

            $scope.BlogPost.Tags = $scope.Tags.split(',');
            adminApi.BlogPosts.Save($scope.BlogPost,
                function (data, status, headers, config) {
                    $scope.$emit("done");
                    if ($scope.BlogPost.Id == undefined) {
                        $location.path('blogposts/' + data);
                    }
                },
                function(data, status, headers, config) {

                });
        };

        $scope.delete = function () {
            $scope.$emit("busy");
            adminApi.BlogPosts.Delete($scope.BlogPost.Id,
                function (data, status, headers, config) {
                    $scope.$emit("done");
                    $location.path("blogposts");
                },
                function(data, status, headers, config) {

                });
        };
    }]);


tankaAdmin.controller("BlogPostCommentsCtrl",
    ['$scope', 'AdminApi', '$routeParams', '$location', '$timeout', function ($scope, adminApi, $routeParams, $location, $timeout) {

        $scope.delete = function (commentId) {
            $scope.$emit("busy");
            adminApi.Comments.Delete($routeParams.id, commentId,
                function () {
                    $scope.$emit("done");
                    getComments();
                },
                function () {

                });
        };

        var getComments = function () {
            $scope.$emit("busy");
            
            adminApi.Comments.GetAll($routeParams.id, function(comments) {
                $scope.Comments = comments;
                $scope.$emit("done");
            });
        };

        getComments();
    }]);