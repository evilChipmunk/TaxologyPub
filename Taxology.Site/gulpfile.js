/// <binding AfterBuild='postbuild' Clean='clean' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var del = require('del');

var paths = {
    scripts: ['scripts/**/*.js', 'scripts/**/*.ts', 'scripts/**/*.map'],
    css: ['css/**/*.css'],
    images: ['images/**/*.*']
};



gulp.task('clean', function () {
    return del(['wwwroot/js/**/*', "wwwroot/css/**/*"]);
});

gulp.task('createJavascript', function () {
    gulp.src(paths.scripts).pipe(gulp.dest('wwwroot/js'));
}); 

gulp.task('createCSS', function () {
    gulp.src(paths.css).pipe(gulp.dest('wwwroot/css'));
});

gulp.task('createImages', function () {
    gulp.src(paths.images).pipe(gulp.dest('wwwroot/images'));
});

gulp.task("postbuild", ["createJavascript", "createCSS", "createImages"]);


gulp.task('watch', function () {
    gulp.watch(paths.css, ['clean', 'createCSS']);
    gulp.watch(paths.scripts, ['createJavascript']);
});


gulp.task("cleanAndCreate", ["clean", "createJavascript", "createCSS", "createImages"]);