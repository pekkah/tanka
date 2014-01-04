tankaAdmin.directive("expand", function() {
    return {
        replace: false,
        restrict: 'A',
        link: function(scope, iElement, iAttrs, ngModel) {
            iElement.TextAreaExpander();
        }
    };
});

tankaAdmin.directive('login', function() {
    return {
        restrict: 'A',
        link: function(scope, elem, attrs) {
            var main = $("#view");
            var login = elem;

            login.hide();

            scope.$on('event:auth-loginRequired', function() {
                login.show();
                login.slideDown(300);
            });
            scope.$on('event:auth-loginConfirmed', function() {
                login.slideUp(100);
                login.hide();
            });
        }
    };
});

tankaAdmin.directive('ace', ['$timeout', function ($timeout) {

    var resizeEditor = function(editor, elem) {
        var lineHeight = editor.renderer.lineHeight;
        var rows = editor.getSession().getLength();

        elem.height(rows * lineHeight);
        editor.resize();
    };

    return {
        restrict: 'A',
        require: '?ngModel',
        scope: true,
        link: function(scope, elem, attrs, ngModel) {
            var node = elem[0];

            var editor = ace.edit(node);

            editor.setTheme('ace/theme/xcode');

            var MarkdownMode = require('ace/mode/markdown').Mode;
            editor.getSession().setMode(new MarkdownMode());

            // set editor options
            editor.setShowPrintMargin(false);

            // add commands
            editor.commands.addCommand({
                name: 'insertImage',
                bindKey: { win: 'Ctrl-I', mac: 'Command-I' },
                exec: function(editor) {
                   
                },
                readOnly: true
            });

            // data binding to ngModel
            ngModel.$render = function() {
                editor.setValue(ngModel.$viewValue);
                resizeEditor(editor, elem);
            };

            editor.on('change', function () {
                $timeout(function() {
                    scope.$apply(function () {
                        var value = editor.getValue();
                        ngModel.$setViewValue(value);
                    });
                });

                resizeEditor(editor, elem);
            });
        }
    };
}]);