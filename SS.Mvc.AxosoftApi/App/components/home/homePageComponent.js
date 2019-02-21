var componentDef = {
    selector: 'homePage',
    bindings: {
        id: '<'
    },
    templateUrl: '/templates/home/homePageComponent',
};
export class HomePageComponent {
    static get $inject() { return ['$timeout', 'dateParser'] }
    static get componentDef() { return componentDef }

    constructor($timeout, dateParser) {
        var self = this;
        this.message = 'Hello ....';
        $timeout(function() {
            self.message = 'Hello World! ' + self.id + dateParser.parse('2015-05-01');
        }, 100);
    }
}