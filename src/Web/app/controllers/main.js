tankaApp.controller('MainCtrl', ['$scope', 'PublicApi', function($scope, publicApi) {

    $scope.TotalResults = 100000;
    $scope.PageSize = 10;
    $scope.Skip = 0;
    $scope.BlogPosts = [];
    $scope.isLoading = false;

    $scope.$emit("busy");

    publicApi.Settings.Get(function(settings) {
        $scope.setSubTitle(settings.SubTitle);
    });

    $scope.loadMore = function (done) {
        if ($scope.isLoading)
            return;

        $scope.isLoading = true;

        publicApi.BlogPosts.Range($scope.Skip, $scope.PageSize,
            function(result) {
                for (var post in result.Posts) {
                    $scope.BlogPosts.push(result.Posts[post]);
                }

                $scope.TotalResults = result.TotalResults;

                $scope.Skip = $scope.BlogPosts.length;


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

    var options = {
        distance: 50,
        callback: function(done) {
            $scope.loadMore(done());          
        }
    };

    // setup infinite scroll
    infiniteScroll(options);
}]);