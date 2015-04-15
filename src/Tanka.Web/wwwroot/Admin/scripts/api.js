var services = angular.module('adminServices',[]);

services.service('AdminApi', ['$http', '$cacheFactory',
    function($http) {
        var blogPosts = function() {
            return $http({ method: 'GET', url: '/api/blog-posts' });
        };

        var blogPostBySlug = function(id) {
            return $http({ method: 'GET', url: '/api/blog-posts/' + id });
        };

        var renderedBlogPostById = function(id) {
            return $http.get('/api/blog-posts/' + id + '/as-html');
        };

        var saveBlogPost = function(blogPost) {
            return $http.post('/api/blog-posts', blogPost);
        };

        var deleteBlogPost = function(id) {
            return $http.delete('/api/blog-posts/' + id);
        };

        var saveSettings = function(settings) {
            return $http.put('/api/settings', settings);
        };
        
        var getSettings = function () {
            return $http({ method: 'GET', url: '/api/settings' });
        };

        var slugify = function(text) {
            return $http({ method: 'POST', url: '/api/utils/slugs', data: { Text: text } });
        };

        var previewMarkdown = function(markdown) {
            return $http.post('/api/utils/markdown/render', { markdown: markdown });
        };

        var getConfiguration = function () {
            return $http({ method: 'GET', url: '/api/configuration/' });
        };

        var saveConfiguration = function(configuration) {
            return $http.put('/api/configuration', configuration);
        };
        

        return {
            BlogPosts: {
                All: blogPosts,
                Single: blogPostBySlug,
                Save: saveBlogPost,
                Delete: deleteBlogPost,
                Render: renderedBlogPostById
            },

            Configuration: {
                Save: saveConfiguration,
                Get: getConfiguration
            },
            Settings: {
                Get: getSettings,
                Save: saveSettings
            }, 

            Utils: {
                Slugify: slugify,
                PreviewMarkdown: previewMarkdown
            }
        };
    }]);