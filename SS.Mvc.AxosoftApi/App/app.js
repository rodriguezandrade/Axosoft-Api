import angular from 'angular';
import ngResource from 'angular-resource';
import uiRouter from 'angular-ui-router';
import ngBootstrap from 'angular-ui-bootstrap';
import { addRegisterMethods, setModuleDefaults } from './helpers';
import { interceptorsSetup } from './httpInterceptor';

// features
import { homeSetup } from './components/home';

// directives
import { EqualToDirective } from './directives/equalToDirective';
import { IntegerDirective, FixedDecimalDirective, PadLeftDirective } from './directives/numberDirectives';
import { TableSortDirective } from './directives/tablesortDirective';

// services
import { DateParser } from './services/dateParser';

var directives = [
    EqualToDirective, 
    IntegerDirective, 
    FixedDecimalDirective, 
    PadLeftDirective,
    TableSortDirective
];

var services = [
    DateParser
];

// register app module
var app = angular.module("app", [ngResource, uiRouter, ngBootstrap]);
addRegisterMethods(app);
setModuleDefaults(app);
interceptorsSetup(app);

// register directives and services
app.registerDirectives(directives);
app.registerServices(services);

// setup features (components)
homeSetup(app);
