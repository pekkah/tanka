tankaAdmin.controller("BlogPostCtrl",
    ['$scope', 'AdminApi', '$routeParams', '$location', '$timeout', 'toaster',
        function ($scope, adminApi, $routeParams, $location, $timeout, toaster) {

        $scope.States = ["Draft", "Published"];

        $scope.editorLoaded = function (editor) {
            $scope.editor = editor;
            
            editor.setTheme("ace/theme/textmate");

            var session = editor.getSession();
            session.setMode("ace/mode/markdown");
            session.setUseWrapMode(true);
        };

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
            $scope.BlogPost.Tags = $scope.Tags.split(',');
            adminApi.BlogPosts.Save($scope.BlogPost,
                function (data, status, headers, config) {
                    toaster.pop('success', 'blog post saved');
                    
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