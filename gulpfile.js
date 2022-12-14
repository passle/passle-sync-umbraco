/// <binding ProjectOpened='default' />
const gulp = require("gulp");
var watch = require("gulp-watch");
var cleanDest = require("gulp-clean-dest");

var paths = {
  source: "./PassleSync.Core/App_Plugins",
  dest: "./PassleSync.Website/App_Plugins",
};

function watchAndCopyPluginFiles(cb) {
  return gulp.src(paths.source + "/**/*", { base: paths.source })
    .pipe(watch(paths.source, { base: paths.source }))
    .pipe(cleanDest(paths.dest))
    .pipe(gulp.dest(paths.dest));
}

exports.default = watchAndCopyPluginFiles;
