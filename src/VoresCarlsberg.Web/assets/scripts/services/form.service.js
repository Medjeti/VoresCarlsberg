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

		
	}
})();