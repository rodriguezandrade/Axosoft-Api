(function () {
	$.validator.setDefaults({
		highlight: function () {
			var element = arguments[0];
			$(element).closest(".form-group").addClass("has-error");
		},
		unhighlight: function () {
			var element = arguments[0];
			$(element).closest(".form-group").removeClass("has-error");
		},
		errorClass: 'error help-inline',
		onkeyup: false
	});
})();
