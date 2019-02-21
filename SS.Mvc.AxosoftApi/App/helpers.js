export function addRegisterMethods(angularModule) {
    angularModule.registerComponents = function (components) {
        for (let i = 0; i < components.length; i++) {
            var componentClass = components[i];
            componentClass.componentDef.controller = componentClass;
            angularModule.component(componentClass.componentDef.selector, componentClass.componentDef);
        }
    }
    angularModule.registerDirectives = function (directives) {
        for (let i = 0; i < directives.length; i++) {
            var directive = directives[i];
            angularModule.directive(directive.selector, directive);
        }
    }
    angularModule.registerServices = function (services) {
        for (let i = 0; i < services.length; i++) {
            var service = services[i];
            console.log(service.name, service);
            angularModule.service(service.name, service);
        }
    }
}

export function setModuleDefaults(angularModule) {
    angularModule.config(["$stateProvider", "$urlRouterProvider", function ($stateProvider, $urlRouterProvider) {
        // For any unmatched url, send to default page
        $urlRouterProvider.otherwise("/");
    }]);
    angularModule.run(["$rootScope", "$window", function ($rootScope, $window) {
        $rootScope.safeApply = function (fn) {
            var phase = this.$root.$$phase;
            if (phase == "$apply" || phase == "$digest") {
                if (typeof (fn) === "function") {
                    fn.apply(this);
                }
            } else {
                this.$apply(fn);
            }
        };
        $rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams) {
            if (toState.data && toState.data.title) {
                $window.document.title = toState.data.title;
            }
        });
    }]);
}