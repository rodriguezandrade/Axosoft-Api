var direction = {
	asc: "asc",
	desc: "desc"
};
var css = {
	asc: "tablesort-asc",
	desc: "tablesort-desc"
};

export function TableSortDirective() {
	return {
		restrict: "A",
		require: ["tablesort", "?ngModel"],
		controller: ["$parse", "$scope", "$attrs", function ($parse, $scope, $attrs) {
			var self = this,
				sortItems = [],
				headings = [],
				changeFn = $attrs.sortChange && $parse($attrs.sortChange),
				options = $attrs.tablesort && $scope.$eval($attrs.tablesort) || {};

			if (typeof options.onSort !== "function") {
				options.onSort = angular.noop;
			}

			self.setSortField = function(name, element) {
				if (sortItems.length === 1 && sortItems[0].name === name) {
					toggleOrder(sortItems[0]);
				} else {
					clearSortItems();

					sortItems = [{ name: name, element: element, asc: true }];
				}

				updateClasses(sortItems[0]);

				onSort();
			};

			self.addSortField = function (name, element) {
				var index = -1;
				for (var i = 0; i < sortItems.length; i++) {
					if (sortItems[i].name === name) {
						index = i;
						toggleOrder(sortItems[i]);
						break;
					}
				}
				if (index === -1) {
					sortItems.push({ name: name, element: element, asc: true });
					index = sortItems.length - 1;
				}
				updateClasses(sortItems[index]);
				onSort();
			};

			self.setSortFields = function(fields) {
				clearSortItems();

				sortItems = fields;

				for (var i = 0; i < fields.length; i++) {
					updateClasses(fields[i]);
				}

				onSort();
			};

			self.registerHeading = function(name, element) {
				headings.push({ name: name, element: element });
			};

			self.update = angular.noop;

			function toggleOrder(item) {
				item.asc = !item.asc;
			}

			function updateClasses(item) {
				item.element[item.asc ? "addClass" : "removeClass"](css.asc);
				item.element[!item.asc ? "addClass" : "removeClass"](css.desc);
			}

			function clearSortItems() {
				for (var i = 0; i < sortItems.length; i++) {
					sortItems[i].element.removeClass(css.asc)
						.removeClass(css.desc);
				}
				sortItems.length = 0;
			}

			function onSort() {
				var expr = [];
				for (var i = 0; i < sortItems.length; i++) {
					//expr.push({ name: sortItems[i].name, order: sortItems[i].asc ? direction.asc : direction.desc });
					expr.push(sortItems[i].name + (sortItems[i].asc ? "" : " " + direction.desc));
				}
				var orderBy = expr.join(",");

				self.update(orderBy);

				if (typeof changeFn === "function") {
					var locals = {};
					locals["$orderBy"] = orderBy;
					changeFn($scope, locals);
				}

				options.onSort(orderBy);
			}
		}],
		link: function (scope, elem, attrs, controllers) {
			var controller = controllers[0],
				ngModelController = controllers[1];

			if (ngModelController) {
				controller.update = function(value) {
					ngModelController.$setViewValue(value);
				};

				ngModelController.$render = function () {
					var orderBy = ngModelController.$modelValue || "";
					var criteria = orderBy.split(","),
						fields = [];

					var thead = elem.find("thead");

					for (var i = 0; i < criteria.length; i++) {
						var criterion = criteria[i],
							parts = criterion.split(" "),
							name = parts[0],
							asc = parts[1] !== direction.desc;

						if (name) {
							fields.push({ name: name, element: thead.find("[tablesort-col=" + name + "]"), asc: asc });
						}
					}

					controller.setSortFields(fields);
				};
			}
		}
	};
}
TableSortDirective.selector = 'tableSort';

export function TablesortColDirective() {
	return {
		restrict: "A",
		require: "^tablesort",
		link: function (scope, elem, attrs, ctrl) {
			var name = attrs.tablesortCol;
			if (!name) {
				throw new Error("Expected name for tablesort column.");
			}
			elem.on("click", function (e) {
				var fn = e.shiftKey ? ctrl.addSortField : ctrl.setSortField;
				scope.$apply(function () {
					fn(name, elem);
				});
			});
			elem.addClass("tablesort-col");
			ctrl.registerHeading(name, elem);
		}
	};
}
TablesortColDirective.selector = 'tablesortCol';