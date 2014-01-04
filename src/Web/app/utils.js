var md = function () {
    marked.setOptions({
        gfm:true,
        pedantic:false,
        sanitize:true,
        // callback for code highlighter
        highlight:function (code, lang) {
            if (lang != undefined)
                return hljs.highlight(lang, code).value;

            return hljs.highlightAuto(code).value;
        }
    });

    var toHtml = function (markdown) {
        if (markdown == undefined)
            return '';

        return marked(markdown);
    };
    
    // hljs.tabReplace = '    ';

    return {
        toHtml:toHtml
    };
}();

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
          ? args[number]
          : match
        ;
    });
};