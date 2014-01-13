﻿var services = angular.module('adminServices', ['services']);

services.service('AdminApi', ['$http', 'PublicApi', '$cacheFactory',
    function($http, publicApi, $cacheFactory) {
        var requestCache = $cacheFactory('requestCache');

        var blogPosts = function(success, error) {
            return $http({ method: 'GET', url: '/api/admin/blogposts' }).
                success(success).
                error(error);
        };

        var blogPostBySlug = function(id, success, error) {
            return $http({ method: 'GET', url: '/api/admin/blogposts/' + id }).
                success(success).
                error(error);
        };

        var saveBlogPost = function(blogPost, success, error) {
            return $http.post('/api/admin/blogposts', blogPost).
                success(success).
                error(error);
        };

        var deleteBlogPost = function(id, success, error) {
            return $http.delete('/api/admin/blogposts/' + id).
                success(success).
                error(error);
        };

        var saveSettings = function(settings, success, error) {
            return $http.put('/api/settings', settings).
                success(success).
                error(error);
        };

        var slugify = function(text, success, error) {
            return $http({ method: 'POST', url: '/api/admin/utils/slugs', data: { text: text } }).
                success(success).
                error(error);
        };

        var getConfiguration = function (success, error) {
            var configuration = requestCache.get('configuration');

            if (configuration != undefined)
                success(configuration);

            return $http({ method: 'GET', url: '/api/configuration/' }).
                success(function(loadedConfiguration) {
                    requestCache.put('configuration', loadedConfiguration);
                    success(loadedConfiguration);
                }).
                error(error);
        };

        var saveConfiguration = function(configuration, success, error) {
            return $http.put('/api/configuration', configuration).
                success(function(data) {
                    requestCache.remove('configuration');
                    success(data);
                }).
                error(error);
        };    

        return {
            BlogPosts: {
                All: blogPosts,
                Single: blogPostBySlug,
                Save: saveBlogPost,
                Delete: deleteBlogPost
            },

            Settings: {
                Save: saveSettings,
                Get: publicApi.Settings.Get
            },

            Configuration: {
                Save: saveConfiguration,
                Get: getConfiguration
            },

            Utils: {
                Slugify: slugify
            }
        };
    }]);