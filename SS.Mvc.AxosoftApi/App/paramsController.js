export class ParamsController {
    static get $inject() {
        return ['$stateParams'];
    }
    constructor($stateParams) {
        this.params = $stateParams;
    }
}