var tankaAdmin = angular.module('tankaAdmin', ['adminServices', 'common', 'ui.bootstrap']).
    config(['$routeProvider', '$locationProvider',
        function($routeProvider, $locationProvider) {
            $locationProvider.html5Mode(false);
            $locationProvider.hashPrefix('!');

            $routeProvider.
                when('/', { templateUrl: '/admin/views/index.html', controller: 'AdminHomeCtrl' }).
                when('/settings', { templateUrl: '/admin/views/settings.html', controller: 'SettingsCtrl' }).
                when('/configuration', { templateUrl: '/admin/views/configuration.html', controller: 'ConfigurationCtrl' }).
                when('/blogposts/create', { templateUrl: '/admin/views/blogpost.html', controller: 'BlogPostCtrl' }).
                when('/blogposts/:id', { templateUrl: '/admin/views/blogpost.html', controller: 'BlogPostCtrl' }).
                when('/blogposts/:id/comments', { templateUrl: '/admin/views/blogpost-comments.html', controller: 'BlogPostCommentsCtrl' }).
                when('/blogposts', { templateUrl: '/admin/views/blogposts.html', controller: 'BlogPostsCtrl' }).
                when('/cms', { templateUrl: '/admin/views/cms.html', controller: 'CmsCtrl' }).
                when('/cms/create', { templateUrl: '/admin/views/cms-editor.html', controller: 'CmsCreateCtrl' }).
                when('/cms/:id', { templateUrl: '/admin/views/cms-editor.html', controller: 'CmsEditCtrl' }).
                otherwise({ redirectTo: '/' });
        }]).
    run(['AdminApi', function(adminApi) {
        var lang = (navigator.language || navigator.browserLanguage).slice(0, 2);
        moment.lang(lang);
        
        adminApi.Configuration.Get(function (configuration) {
        });
    }]);

tankaAdmin.controller('AppCtrl',
    ['$scope', 'PublicApi', '$location', '$window', 'AdminApi',
        function($scope, publicApi, $location, $window, adminApi) {

            $scope.setSubTitle = function(title) {
                $scope.subTitle = title;
                $window.document.title = $scope.windowTitle + ' ' + title;
            };
            
            adminApi.Configuration.Get(function (serverConfiguration) {
                $scope.Configuration = serverConfiguration;
            });

            publicApi.Settings.Get(
                function (serverSettings) {
                    $scope.Settings = serverSettings;
                    
                    $scope.windowTitle = serverSettings.Title;
                    $scope.setSubTitle(serverSettings.SubTitle);
                });
        }]);