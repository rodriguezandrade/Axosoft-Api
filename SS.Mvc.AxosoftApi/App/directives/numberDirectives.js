export function IntegerDirective() {
	var integerRegexp = /^(\-\d{0,10})|(\d{0,10})$/, minIntValue = -2147483648, maxIntValue = 2147483647;
	return {
		require: "ngModel",
		link: function(scope, elm, attrs, ctrl) {
			ctrl.$parsers.unshift(function(viewValue) {
				if (viewValue === "") {
					ctrl.$setValidity("integer", true);
					return undefined;
				}
				var intValue;
				if (integerRegexp.test(viewValue) &&
					!isNaN((intValue = parseInt(viewValue))) &&
					intValue >= minIntValue &&
					intValue <= maxIntValue) {
					// it is valid
					ctrl.$setValidity("integer", true);
					return intValue;
				} else {
					// it is invalid, return undefined (no model update)
					ctrl.$setValidity("integer", false);
					return undefined;
				}
			});
		}
	};
}
IntegerDirective.selector = 'integer';

export function FixedDecimalDirective() {
	return {
		require: "ngModel",
		link: function(scope, elm, attrs, ctrl) {
			var numOfDecimals = parseInt(scope.$eval(attrs.fixedDecimal));

			if (isNaN(numOfDecimals)) {
				numOfDecimals = 2;
			}
			var regex = new RegExp("^\\-?\\d*\\.\\d{" + numOfDecimals + "}?$");

			ctrl.$parsers.unshift(function(viewValue) {
				if (viewValue === "") {
					ctrl.$setValidity("fixedDecimal", true);
					return viewValue;
				}
				if (regex.test(viewValue)) {
					// it is valid
					ctrl.$setValidity("fixedDecimal", true);
					return parseFloat(viewValue);
				} else {
					// it is invalid, return undefined (no model update)
					ctrl.$setValidity("fixedDecimal", false);
					return viewValue;
				}
			});
		}
	};
}
FixedDecimalDirective.selector = 'fixedDecimal';

export function PadLeftDirective() {
	return function(n, len) {
		if (n) {
			var num = n + "";
			while (num.length < len) {
				num = "0" + num;
			}
			return num;
		} else {
			return "";
		}
	};
}
PadLeftDirective.selector = 'padLeft';