var tankaApp = angular.module('tankaApp', ['services', 'common'])
    .run(function () {
        var lang = (navigator.language || navigator.browserLanguage).slice(0, 2);
        moment.lang(lang);

    });