import { ParamsController } from '../../ParamsController';
import { HomePageComponent } from './homePageComponent';

export function homeSetup (app) {

    app.registerComponents([HomePageComponent]);

    app.config(["$stateProvider", "$urlRouterProvider", function ($stateProvider, $urlRouterProvider) {

        $stateProvider
			.state("about", {
			    url: "/about",
			    template: '<home-page id="\'about\'"></home-page>',
			    controllerAs: '$ctrl', 
			    controller: ParamsController,
			    data: {
			        title: "Home"
			    }
			})
			.state("contact", {
			    url: "/contact",
			    template: '<home-page id="\'contact me!!!\'"></home-page>',
			    controllerAs: '$ctrl', 
			    controller: ParamsController,
			    data: {
			        title: "Home"
			    }
			})
			.state("home", {
			    url: "/:id",
			    template: '<home-page id="$ctrl.params.id"></home-page>',
			    controllerAs: '$ctrl', 
			    controller: ParamsController,
			    data: {
			        title: "Home"
			    }
			})
    }]);
};