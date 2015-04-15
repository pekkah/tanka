tankaAdmin.directive("expand", function() {
    return {
        replace: false,
        restrict: 'A',
        link: function(scope, iElement, iAttrs, ngModel) {
            iElement.TextAreaExpander();
        }
    };
});