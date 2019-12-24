/// <binding AfterBuild='min' Clean='clean' />
const { src, dest, pipe, series, parallel } = require('gulp');
const bundleConfig = require('./bundleconfig.json');
const concat = require("gulp-concat");
const del = require('del');
const htmlmin = require('gulp-htmlmin');
const merge = require('merge2');
const cleanCSS = require('gulp-clean-css');
const plumber = require('gulp-plumber');
const sourcemaps = require('gulp-sourcemaps');
const uglify = require('gulp-uglify');

const regex = {
  css: /\.css$/,
  html: /\.(html|htm)$/,
  js: /\.js$/
};

function getBundles(regexPattern) {
  return bundleConfig.filter(bundle =>
    regexPattern.test(bundle.outputFileName));
}

function clean() {
  const files = bundleConfig.map(bundle => bundle.outputFileName);
  files.push("wwwroot/js/**/*.map");
  return del(files);
}

function cssMinify() {
  const tasks = getBundles(regex.css).map(function (bundle) {
    return src(bundle.inputFiles, { base: "." })
      .pipe(concat(bundle.outputFileName))
      .pipe(cleanCSS())
      .pipe(dest("."));
  });
  return merge(tasks);
}

function jsMinify() {
  const tasks = getBundles(regex.js).map(bundle => {
    return src(bundle.inputFiles, { base: "." })
      .pipe(sourcemaps.init())
      .pipe(plumber())
      .pipe(concat(bundle.outputFileName))
      .pipe(uglify())
      .pipe(sourcemaps.write("."))
      .pipe(dest("."));
  });
  return merge(tasks);
}

function htmlMinify() {
  const tasks = getBundles(regex.html).map(bundle => {
    return src(bundle.inputFiles, { base: "." })
      .pipe(concat(bundle.outputFileName))
      .pipe(htmlmin({ collapseWhitespace: true, minifyCSS: true, minifyJS: true }))
      .pipe(dest("."));
  });
  return merge(tasks);
}

exports.default = series(
  clean,
  parallel(cssMinify, jsMinify, htmlMinify)
);
exports.clean = clean;
