var tankaAdmin = angular.module('tankaAdmin', ['ngRoute', 'adminServices', 'common', 'ui.bootstrap', 'ui.ace', 'toaster']).
    config(['$routeProvider', '$locationProvider',
        function($routeProvider, $locationProvider) {
            $locationProvider.html5Mode(false);
            $locationProvider.hashPrefix('!');

            $routeProvider.
                when('/', { templateUrl: '/admin/views/index.html', controller: 'AdminHomeCtrl' }).
                when('/settings', { templateUrl: '/admin/views/settings.html', controller: 'SettingsCtrl' }).
                when('/blogposts/create', { templateUrl: '/admin/views/blogpost.html', controller: 'BlogPostCtrl' }).
                when('/blogposts/:id', { templateUrl: '/admin/views/blogpost.html', controller: 'BlogPostCtrl' }).
                when('/blogposts/:id/comments', { templateUrl: '/admin/views/blogpost-comments.html', controller: 'BlogPostCommentsCtrl' }).
                when('/blogposts', { templateUrl: '/admin/views/blogposts.html', controller: 'BlogPostsCtrl' }).
                otherwise({ redirectTo: '/' });
        }]).
    run([function() {
        var lang = (navigator.language || navigator.browserLanguage).slice(0, 2);
        moment.lang(lang);
    }]);

tankaAdmin.controller('AppCtrl',
    ['$scope', 'PublicApi', '$location', '$window', 'AdminApi',
        function($scope, publicApi, $location, $window, adminApi) {

        }]);
