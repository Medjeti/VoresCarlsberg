'use strict';

// Include Gulp & tools we'll use
var gulp = require('gulp');
var $ = require('gulp-load-plugins')();
var browserSync = require('browser-sync').create();
var reload = browserSync.reload;
var del = require('del');
var gulpFilter = require('gulp-filter');
var runSequence = require('run-sequence');
var header = require('gulp-header');
var wiredep = require('wiredep').stream;

//var blocksBuilder = require('gulp-usemin/lib/blocksBuilder');

var projectHostUrl = 'http://localhost:45369/';

var paths = {
    root: '../',
    masterViewFile: '../Views/Shared/_Layout.cshtml',
    viewsFolder: '../Views/',
    viewsSharedFolder: '../Views/Shared/'
}


var AUTOPREFIXER_BROWSERS = [
  'ie >= 9',
  'ie_mob >= 10',
  'ff >= 30',
  'chrome >= 34',
  'safari >= 7',
  'opera >= 23',
  'ios >= 7',
  'android >= 4.4',
  'bb >= 10'
];

// Clean output directory
gulp.task('clean', del.bind(null, [
  '../css',
  '!../css/TinyMCE*',
  '../images',
  '../scripts',
  '../templates',
  '../fonts',
  '../index.html'
], {
  dot: true,
  force: true
}));

// Lint JavaScript
gulp.task('jshint', function () {
  return gulp.src('scripts/**/*.js')
    .pipe($.jshint())
    .pipe($.jshint.reporter('jshint-stylish'))
    .pipe(reload({stream: true}));
});

// Lint Sass
gulp.task('scss-lint', function () {
  var scssFilter = gulpFilter('styles/bootstrap/**/*.scss');
  gulp.src('styles/*.scss')
    .pipe(scssFilter)
    .pipe($.scssLint({
      'config': 'scss-lint.yml'
    }));
});

// Compile and automatically prefix stylesheets
gulp.task('styles', function () {
  // For best performance, don't add Sass partials to `gulp.src`
  return gulp.src(['styles/*.scss'])
    .pipe($.sourcemaps.init())
    .pipe($.sass({ precision: 10 }).on('error', $.sass.logError))
    .pipe($.autoprefixer({browsers: AUTOPREFIXER_BROWSERS}))
    .pipe($.sourcemaps.write())
    .pipe(gulp.dest('../css'))
    .pipe($.size({title: 'styles'}))
    .pipe(reload({stream: true}));
});

// Auto inject bower-components
gulp.task('bower', function () {
    gulp.src(paths.masterViewFile)
    .pipe(wiredep({
      ignorePath: '../..',
      exclude: [/es5-shim/, /json3/, /jquery/, /bootstrap-sass/, /angular-gridster/, /detect-element-resize/]
    }))
    .pipe(header('\ufeff'))
    .pipe(gulp.dest(paths.viewsSharedFolder));
});

// Reads HTML for usemin blocks to enable smart deploys that automatically concats files
gulp.task('usemin', function () {
    return gulp.src(paths.masterViewFile)
        .pipe($.usemin({
            css: [$.csso()],
            jsShim: [$.uglify()],
            jsLibs: [$.uglify()],
            jsMain: [$.ngAnnotate(), $.uglify()],
            jsNull: [$.uglify()]
        }))
        .pipe(gulp.dest(paths.root));
});

// Browser-sync
gulp.task('browser-sync', function () {
  browserSync.init({
      proxy: projectHostUrl
    /*
    server: {
      baseDir: "../",
      routes: {
        "/bower_components": "bower_components"
      }
    }
    */
  });
});

// Copy images
gulp.task('copyImages', function () {
    return gulp.src(['images/**/*'], { base: '.' })
    .pipe(gulp.dest('../'))
    .pipe($.size({title: 'images'}));
});

gulp.task('copyVisualizerImages', function () {
    return gulp.src(['images/visualizer/**/*'], { base: '.' })
    .pipe(gulp.dest('../'))
    .pipe($.size({ title: 'images' }));
});

// Optimize images
gulp.task('optimizeImages', function () {
    return gulp.src(['images/**/*', '!images/visualizer/**/*'], { base: '.' })
    .pipe($.imagemin({
      progressive: true, optimizationLevel: 5
    }))
    .pipe(gulp.dest('../'))
    .pipe($.size({ title: 'images' }));
});

// Copy Templates
gulp.task('templates', function () {
  return gulp.src(['templates/**/*'], {base: '.'})
    .pipe(gulp.dest('../'))
    .pipe($.size({title: 'templates'}))
    .pipe(reload({stream: true}));
});

// Copy Fonts
gulp.task('fonts', function () {
  return gulp.src(['fonts/**/*'], {base: '.'})
    .pipe(gulp.dest('../'))
    .pipe($.size({title: 'fonts'}));
});

// Rerun the task when a file changes
gulp.task('watch', function () {
  gulp.watch('scripts/**/*.js', ['jshint']);
  gulp.watch('images/**/*', ['copyImages']);
  gulp.watch('styles/**/*.scss', ['styles', 'scss-lint']);
  gulp.watch('templates/**/*', ['templates']);
  gulp.watch('../Views/**/*').on('change', browserSync.reload);
  gulp.watch('index.html').on('change', browserSync.reload);
});

// Build production files, the default task
gulp.task('default', ['clean'], function (cb) {
  runSequence('styles', ['bower', 'jshint', 'scss-lint', 'copyImages', 'templates', 'fonts', 'watch', 'browser-sync'], cb);
});

// Build production files, the default task
gulp.task('deploy', ['clean'], function (cb) {
    runSequence('styles', ['jshint', 'optimizeImages', 'copyVisualizerImages', 'templates', 'fonts', 'usemin'], cb);
});

gulp.task('deployWithoutOptimize', ['clean'], function (cb) {
    runSequence('styles', ['copyImages', 'templates', 'fonts', 'usemin'], cb);
});
