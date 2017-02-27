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
		this.subscribe = subscribe;
		this.getDivisions = getDivisions;


		// -------------------------------------------------------------------------

		function subscribe(email) {
		    
		}

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