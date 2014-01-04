angular.module('services', [])
    .service('PublicApi', ['$http','$cacheFactory', function($http, $cacheFactory) {
        var blogPosts = function(skip, take, success, error) {
            $http({ method: 'GET', url: '/api/blogposts'+'?skip='+skip+'&take='+take}).
                success(success).
                error(error);
        };

        var blogPostBySlug = function(slug, success, error) {
            $http({ method: 'GET', url: '/api/blogposts/' + slug }).
                success(success).
                error(error);
        };
        
        var blogPostsByTag = function (tag, success, error) {
            $http({ method: 'GET', url: '/api/blogposts/tags/' + tag }).
                success(success).
                error(error);
        };

        var getSettings = function (success, error) {
            $http({ method: 'GET', url: '/api/settings' }).
                success(success).
                error(error);
        };

        var getBlogPostCommentsById = function(blogPostId, success, error) {
            $http({ method: 'GET', url: '/api/blogposts/{0}/comments'.format(blogPostId) }).
                success(success).
                error(error);
        };

        var postCommentForBlogPost = function(comment, success, error) {
            $http({ method: 'POST', url: '/api/blogposts/{0}/comments'.format(comment.BlogPostId), data: comment }).
                success(success).
                error(error);
        };

        var getContent = function(contentId, success, error) {
            $http({ method: 'GET', url: '/api/contents/' + contentId}).
                success(success).
                error(error);
        };

        return {
            BlogPosts: {
                Range: blogPosts,
                Single: blogPostBySlug,
                ByTag: blogPostsByTag
            },

            Comments: {
                ForBlogPost: getBlogPostCommentsById,
                Post: postCommentForBlogPost
            },

            Settings: {
                Get: getSettings
            },
            
            Content : {
                Get : getContent
            }
            
        };
    }]);