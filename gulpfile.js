const { src, dest, series } = require("gulp");
const replace = require("gulp-replace");
const rename = require("gulp-rename");
const watch = require("gulp-watch");
const cleanDest = require("gulp-clean-dest");
const fs = require("fs");
const { spawn } = require("child_process");
const packageJson = require("./package.json");

const paths = {
  source: "./PassleSync.Core/App_Plugins",
  dest: "./PassleSync.Website/App_Plugins",
};

const copyPluginFiles = (cb) => {
  return src(paths.source + "/**/*", { base: paths.source })
    .pipe(cleanDest(paths.dest))
    .pipe(dest(paths.dest));
}

const watchAndCopyPluginFiles = (cb) => {
  return src(paths.source + "/**/*", { base: paths.source })
    .pipe(watch(paths.source, { base: paths.source }))
    .pipe(cleanDest(paths.dest))
    .pipe(dest(paths.dest));
}

const getVersion = () => packageJson.version;

const updatePackageXML = (cb) => {
  src("package_template.xml")
    .pipe(replace("PASSLESYNC_VERSION", getVersion()))
    .pipe(replace("HOMEPAGE_TEMPLATE", fs.readFileSync("PassleSync.Website/Views/HomePage.cshtml", "utf-8")))
    .pipe(replace("INSIGHTSPAGE_TEMPLATE", fs.readFileSync("PassleSync.Website/Views/InsightsPage.cshtml", "utf-8")))
    .pipe(replace("LAYOUT_TEMPLATE", fs.readFileSync("PassleSync.Website/Views/Layout.cshtml", "utf-8")))
    .pipe(replace("AUTHOR_TEMPLATE", fs.readFileSync("PassleSync.Website/Views/PassleAuthor.cshtml", "utf-8")))
    .pipe(replace("POST_TEMPLATE", fs.readFileSync("PassleSync.Website/Views/PasslePost.cshtml", "utf-8")))
    .pipe(replace("README", fs.readFileSync("README.md", "utf-8")))
    .pipe(replace(/\uFEFF/g, '')) // Remove byte-order marks
    .pipe(rename({basename: "package"}))
    .pipe(dest("."));

  cb();
}

const runUmbPack = (cb) => {
  spawn("umbpack", ["pack", ".\\package.xml"], {
    stdio: "inherit",
    shell: true,
  }).on("close", cb);
};

exports.copyPluginFiles = copyPluginFiles;
exports.watch = watchAndCopyPluginFiles;
exports.package = series(copyPluginFiles, updatePackageXML);
exports.packageAndZip = series(copyPluginFiles, updatePackageXML, runUmbPack);
