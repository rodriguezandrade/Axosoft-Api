export function interceptorsSetup (app) {

	app.factory("prendingRequestsInterceptor", ["$q", "$rootScope", function ($q, $rootScope) {

		var pending = 0;

		function updateInProgress() {
			$rootScope.ajaxInProgress = --pending > 0;
		}

		return {

			request: function (config) {
				pending++;
				$rootScope.ajaxInProgress = true;
				return config || $q.when(config);
			},
			requestError: function (response) {
				updateInProgress();
				return $q.reject(response);
			},
			response: function (response) {
				updateInProgress();
				return response || $q.when(response);
			},
			responseError: function (response) {
				updateInProgress();
				return $q.reject(response);
			}
		};
	}]);

	app.factory("authorizationInterceptor", ["$q", "$window", "$timeout", function ($q, $window, $timeout) {

		var timeoutHandle,
			timeoutDuration = 30 * 60 * 1000; // 30 minutes

		function initTimeout() {
			if (timeoutHandle) {
				$timeout.cancel(timeoutHandle);
			}

			timeoutHandle = $timeout(function () {
				$window.location = $window.virtualPath + "Account/LogOut";
			}, timeoutDuration);
		}

		return {
			// Uncomment to auto-logout after a period of inactivity
			/*
			requestError: function (response) {
				initTimeout();
				return $q.reject(response);
			},
			response: function (response) {
				initTimeout();
				return response || $q.when(response);
			},
			*/
			responseError: function (response) {
				if (response.status == 401) { // Unauthorized
					$window.location = $window.virtualPath + "Account/Login";
				}

				// TODO: Show unauthorized message and prompt to login with a different user
				if (response.status == 403) { // Forbidden
					$window.location = $window.virtualPath + "Account/Login";
				}

				return $q.reject(response);
			}
		};
	}]);

	app.factory("virtualPathInterceptor", ["$q", "$window", function ($q, $window) {

		return {

			request: function (config) {
				if (config && config.url) {
					var url = config.url;
					if (url.indexOf($window.virtualPath) !== 0) {
						if (url.indexOf("/") === 0) {
							url = url.substring(1);
						}
						config.url = $window.virtualPath + url;
					}
				}

				return config || $q.when(config);
			}
		};
	}]);

	app.config(["$httpProvider", function ($httpProvider) {
		$httpProvider.interceptors.push("prendingRequestsInterceptor");
		$httpProvider.interceptors.push("authorizationInterceptor");
		$httpProvider.interceptors.push("virtualPathInterceptor");
		
		$httpProvider.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
	}]);

};