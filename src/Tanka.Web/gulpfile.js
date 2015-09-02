/// <binding AfterBuild='default' />

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    less = require("gulp-less"),
    plumber = require("gulp-plumber"),
    project = require("./project.json");

var paths = {
    webroot: "./" + project.webroot + "/"
};

paths.uiLess = "./UI/default/bootstrap.less";
paths.uiCss = paths.webroot + "css";

gulp.task("build:css", function () {
    return gulp.src(paths.uiLess)
        .pipe(plumber())
        .pipe(less())
        .pipe(gulp.dest(paths.uiCss));
});

gulp.task("default", ["build:css"]);
