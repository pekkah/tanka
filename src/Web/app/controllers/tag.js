tankaApp.controller('TagCtrl', ['$scope', 'PublicApi', '$routeParams', function($scope, publicApi, $routeParams) {
    $scope.BlogPosts = [];
    $scope.isLoading = false;

    $scope.$emit("busy");

    publicApi.Settings.Get(function(settings) {
        $scope.setSubTitle($routeParams.tag);
    });

    $scope.loadMore = function (done) {
        if ($scope.isLoading)
            return;

        $scope.isLoading = true;

        publicApi.BlogPosts.ByTag($routeParams.tag,
            function(result) {
                for (var post in result.Posts) {
                    $scope.BlogPosts.push(result.Posts[post]);
                }

                $scope.isLoading = false;
                if (done != undefined)
                    done();

                $scope.$emit('done');
            },
            function(data, status, headers, config) {
                $scope.error = data;
            });
    };

    $scope.loadMore();
}]);