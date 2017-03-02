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
				{ name: "Løb", selected: false },
				{ name: "Fodbold", selected: false },
				{ name: "Cykling", selected: false },
				{ name: "Badminton", selected: false },
				{ name: "Sejlads", selected: false },
				{ name: "Fiske", selected: false },
				{ name: "Haven", selected: false },
				{ name: "Tatoveringer", selected: false },
				{ name: "Yoga", selected: false },
				{ name: "Styrketræning", selected: false },
				{ name: "Håndbold", selected: false },
				{ name: "Computerspil", selected: false },
				{ name: "Brætspil", selected: false },
				{ name: "Håndarbejde", selected: false },
				{ name: "Madlavning", selected: false },
				{ name: "Fotografering", selected: false },
				{ name: "Bøger", selected: false },
				{ name: "Film", selected: false },
				{ name: "Rejser", selected: false },
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