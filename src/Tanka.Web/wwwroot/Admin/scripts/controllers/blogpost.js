tankaAdmin.controller("BlogPostCtrl",
    ['$scope', 'AdminApi', '$routeParams', '$location', '$timeout', 'toaster',
        function($scope, adminApi, $routeParams, $location, $timeout, toaster) {

            $scope.States = ["Draft", "Published"];
            $scope.datePicker = {
                showPublishedOnSelector: false
            };

            var watchSlug = function() {
                $scope.$watch('BlogPost.Title',
                    function(newValue, oldValue) {
                        if (newValue == undefined) {
                            return;
                        }

                        $timeout(function() {
                            adminApi.Utils.Slugify(newValue)
                                .success(function (response) {
                                    //todo: should be response.Slug
                                    $scope.BlogPost.Slug = response;
                                })
                                .error(
                                    function(error) {
                                        $scope.BlogPost.Slug = '';
                                    });
                        }, 250);
                    });
            };

            $scope.preview = function() {
                adminApi.Utils.PreviewMarkdown($scope.BlogPost.Content)
                    .success(function(html) {
                        $scope.previewContent = html;
                    });
            };

            $scope.editorLoaded = function(editor) {
                $scope.editor = editor;

                editor.setTheme("ace/theme/textmate");

                var session = editor.getSession();
                session.setMode("ace/mode/markdown");
                session.setUseWrapMode(true);
            };

            watchSlug();

            if ($routeParams.id != undefined) {
                $scope.$emit("busy");
                adminApi.BlogPosts.Single($routeParams.id)
                    .success(
                        function (data) {
                            data.PublishedOn = moment(data.PublishedOn).toDate();
                            $scope.BlogPost = data;
                            $scope.BlogPost.TagsAsString = data.Tags.join(',');
                            $scope.$emit("done");
                        })
                    .error(function(data) {
                        $scope.error = data;
                    });
            } else {
                $scope.BlogPost = { State: "Draft", PublishedOn: new Date(), Tags: ''};
            }

            $scope.save = function () {
                if ($scope.BlogPost.TagsAsString !== undefined && $scope.BlogPost.TagsAsString != '')
                    $scope.BlogPost.Tags = $scope.BlogPost.TagsAsString.split(',');
                
                adminApi.BlogPosts.Save($scope.BlogPost)
                    .success(
                        function(data, status, headers, config) {
                            toaster.pop('success', 'blog post saved');

                            if ($scope.BlogPost.Id == undefined) {
                                $location.path(headers('location'));
                            }
                        })
                    .error(
                        function(data, status, headers, config) {

                        });
            };

            $scope.delete = function() {
                adminApi.BlogPosts.Delete($scope.BlogPost.Id)
                    .success(
                        function(data) {
                            toaster.pop('success', 'Blog post deleted.');
                            $location.path("blogposts");
                        })
                    .error(
                        function(data) {
                            toaster.pop('error', "Failed to delete blog post");
                        });
            };

            $scope.open = function() {
                $scope.datePicker.showPublishedOnSelector = true;
            }
        }]);