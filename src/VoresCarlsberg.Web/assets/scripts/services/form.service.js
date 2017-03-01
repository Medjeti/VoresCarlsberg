(function () {
	'use strict';

	angular
        .module('VoresCarlsberg')
        .service('formService', formService);

	/* @ngInject */
	function formService($http) {

		// -------------------------------------------------------------------------
		// Public Method & Variables
		// -------------------------------------------------------------------------

		/*jshint validthis:true */
		this.getDivisions = getDivisions;
		this.getHobbies = getHobbies;
		this.login = login;
		this.completeSubmission = completeSubmission;

		// -------------------------------------------------------------------------
		// -------------------------------------------------------------------------

		function login(login) {
			var response = $http({
				url: '/api/guest/login',
				data: login,
				method: 'POST'
			});

			return response;
		}

		// -------------------------------------------------------------------------

		function completeSubmission(guest) {
			var response = $http({
				url: '/api/guest/submit',
				data: guest,
				method: 'POST'
			});

			return response;
		}

		// -------------------------------------------------------------------------

		function getDivisions() {
			var list = [
				"Business Development",
				"Business IT",
				"Communication",
				"Finance",
				"Log. Stabe",
				"Marketing",
				"Prod. Stabe",
				"Prod. Øst",
				"Salg Off Trade",
				"Salg On Trade",
				"Øvrige"
			];
			return list;
		}

		// -------------------------------------------------------------------------

		function getHobbies(guest) {
			var list = [
				{ name: "Biler", selected: false },
				{ name: "Fodbold", selected: false },
				{ name: "Cykling", selected: false },
				{ name: "Musikband", selected: false },
				{ name: "Blomster", selected: false },
				{ name: "Badminton", selected: false },
				{ name: "Sejle", selected: false },
				{ name: "Fiske", selected: false },
				{ name: "Øl", selected: false },
				{ name: "Haven", selected: false },
				{ name: "Tatoveringer", selected: false },
				{ name: "Andet", selected: false }
			];

			// Populate selected hobbies
			if (guest && guest.selectedHobbies) {
				var hobbyArr = guest.selectedHobbies.split(",");
				angular.forEach(list,
					function(value, key) {
						if (hobbyArr.indexOf(value.name) > -1) {
							value.selected = true;
						}
					});
			}
			return list;
		}

		
	}
})();