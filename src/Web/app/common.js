var common = angular.module('common', ['services']);

common.directive('markdown', function() {
    return {
        restrict: 'A',
        link: function(scope, element, attrs) {
            scope.$watch(attrs.markdown, function(value) {
                var html = md.toHtml(value);
                element.html(html);
            });
        }
    };
});

common.directive('progress', function() {
    return {
        restrict: 'E',

        link: function(scope, element, attrs) {
            scope.$on("busy", function() {
                element.show();
                element.val(40);
            });

            scope.$on('done', function() {
                element.val(100);
                element.hide();
            });

            element.hide();
        }
    };
});

common.filter("formatDateTime", function() {
    return function(text, format) {
        if (text == null)
            return '';

        return moment(text).format(format);
    };
});

common.filter("fromNow", function() {
    return function(text) {
        if (text == null)
            return '';

        return moment(text).fromNow();
    };
});

common.directive('datepicker', function() {
    return {
        require: 'ngModel',
        link: function(scope, element, attrs, ctrl) {
            element.datepicker({
                dateFormat: "yy-mm-dd",
                onSelect: function(value, picker) {
                    scope.$apply(function() {
                        ctrl.$setViewValue(value);
                    });
                }
            });

            ctrl.$formatters.push(function(value) {
                if (angular.isString(value)) {
                    return moment(value).format('YYYY-MM-DD');
                }
            });

            ctrl.$parsers.push(function(value) {
                if (value) {
                    return value;
                }
            });
        }
    };
});

common.directive('whenScrolled', function () {
    return function (scope, elm, attr) {
        var raw = elm[0];

        elm.bind('scroll', function () {
            if (raw.scrollTop + raw.offsetHeight >= raw.scrollHeight) {
                scope.$apply(attr.whenScrolled);
            }
        });
    };
});

common.directive('contentId', ['PublicApi', function(publicApi) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            publicApi.Content.Get(attrs.contentId, function(content) {
                var html = md.toHtml(content.Content);
                element.html(html);
            });
        }
    };
}]);