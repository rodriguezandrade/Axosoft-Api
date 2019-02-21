/// <binding ProjectOpened='watch' />
"use strict";

var babelify = require("babelify"),
	browserify = require("browserify"),
	buffer = require("vinyl-buffer"),
	gulp = require("gulp"),
	uglify = require("gulp-uglify"),
	merge = require("merge"),
	rename = require("gulp-rename"),
	source = require("vinyl-source-stream"),
	sourceMaps = require("gulp-sourcemaps"),
	concat = require("gulp-concat"),
	brfs = require("brfs-babel"),
	env = "dev",
	PROD_ENV = "production";

var config = {
	js: {
		src: "./App/app.js",
		outputDir: "./Scripts/",
		mapDir: "./maps/",
		outputFile: "app-bundle.min.js",
		vendorOutputFile: "angular-bundle.min.js"
	},
	css: {
		outputDir: "./Content/css/",
		outputFontsDir: "./Content/fonts/"
	},
	libs: [
		"angular",
		"angular-resource",
		"angular-ui-router",
		"angular-ui-bootstrap"
	]
};

gulp.task("app-bundle", function () {
	var bundler = browserify(config.js.src, {
				debug: true
			}).transform(brfs)
			.transform(babelify, { presets: ["es2015"] })
		;

	for (var i = 0; i < config.libs.length; i++) {
		var lib = config.libs[i];
		bundler.external(lib);
	}
	return bundler
		.bundle()
		.on("error", function (err) {
			// print the error (can replace with gulp-util)
			console.log(err.message);
			// end this stream
			this.emit("end");
		})
		.pipe(source(config.js.outputFile))
		.pipe(buffer())
		.pipe(sourceMaps.init({ loadMaps: true }))
		.pipe((env !== PROD_ENV) ? concat(config.js.outputFile) : uglify())
		.pipe(sourceMaps.write(config.js.mapDir))
		.pipe(gulp.dest(config.js.outputDir));
});

gulp.task("angular-bundle", function () {
	var bundler = browserify({
		debug: false
	});
	for (var i = 0; i < config.libs.length; i++) {
		var lib = config.libs[i];
		bundler.require(lib);
	}
	return bundler
		.bundle()
		.on("error", function (err) {
			// print the error (can replace with gulp-util)
			console.log(err.message);
			// end this stream
			this.emit("end");
		})
		.pipe(source(config.js.vendorOutputFile))
		.pipe(buffer())
		.pipe((env !== PROD_ENV) ? concat(config.js.vendorOutputFile) : uglify())
		.pipe(gulp.dest(config.js.outputDir));
});

gulp.task("bower-bundle", function () {
	return gulp.src([
			"./bower_components/js-custom-select/js/customSelect.js",
			"./bower_components/ng-focus-if/focusIf.js"
		])
		.pipe(concat("bower-bundle.min.js"))
		.pipe(uglify())
		.pipe(gulp.dest(config.js.outputDir));
});

gulp.task("css-bundle", function () {
	gulp.src([
			"./node_modules/bootstrap/dist/css/bootstrap.min.css",
			"./node_modules/font-awesome/css/font-awesome.min.css",
			"./bower_components/js-custom-select/css/style.css"
		])
		.pipe(concat("css-bundle.min.css"))
		.pipe(gulp.dest(config.css.outputDir));

	gulp.src([
			"./node_modules/font-awesome/fonts/**"
		])
		.pipe(gulp.dest(config.css.outputFontsDir));
});

gulp.task("watch", [
	"css-bundle",
	"angular-bundle",
	"app-bundle",
	"bower-bundle"
], function () {
	gulp.watch(["App/**/*.{js,html}"], ["app-bundle"]);
});

gulp.task("default", ["watch"]);

gulp.task("setProdFlag", function () {
	env = PROD_ENV;
});

gulp.task("prod", [
	"setProdFlag",
	"css-bundle",
	"angular-bundle",
	"app-bundle",
	"bower-bundle"
]);