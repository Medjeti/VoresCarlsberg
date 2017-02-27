(function () {
	'use strict';

	angular
        .module('designa')
        .service('newsletterService', newsletterService);

	/* @ngInject */
	function newsletterService($http, googleAnalyticsService) {

		// -------------------------------------------------------------------------
		// Public Method & Variables
		// -------------------------------------------------------------------------

		/*jshint validthis:true */
		this.subscribe = subscribe;


		// -------------------------------------------------------------------------

		function subscribe(email) {
		    googleAnalyticsService.trackEvent('Newsletter', 'Subscribe');
			var response = $http({
				url: '/api/newsletter/subscribe',
				data: { email: email },
				method: 'POST'
			});

			return response;
		}

		
	}
})();