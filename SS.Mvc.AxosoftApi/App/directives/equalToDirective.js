export function EqualToDirective () {
	return {
		require: "ngModel",
		link: function (scope, elm, attrs, ctrl) {
			ctrl.$parsers.unshift(function (viewValue) {
				if (viewValue === "" || viewValue == null) {
					ctrl.$setValidity("equalTo", true);
				}
				else {
					var valid = scope.$eval(attrs["equalTo"]) === viewValue;
					ctrl.$setValidity("equalTo", valid);
				}
				return viewValue;
			});
		}
	};
}
EqualToDirective.selector = "equalTo"