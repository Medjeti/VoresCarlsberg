(function () {
  'use strict';

	angular
		.module('designa', [
			'designa.templates',
            'designa.visualizer',
			'ngAnimate',
            'ngSanitize',
			'ngTouch',
			'ui.bootstrap',
            'gridster',
			'uiGmapgoogle-maps',
            'duScroll',
            'ngCookies'
           // 'ngCookies'
		])
		.config(function(uiGmapGoogleMapApiProvider) {
			uiGmapGoogleMapApiProvider.configure({
				//    key: 'your api key',
				v: '3.20', //defaults to latest 3.X anyhow
				//libraries: 'weather,geometry,visualization'
			});
		})
		.run(function run($window, $rootScope) {

		    // Setup Scrollspy
		    if (!window.history || !history.replaceState) {
		        return;
		    }

		    angular.element($window).bind('load', function () {
		        setupScrollSpy();
		    });

		    var updateHash = _.debounce(doUpdateHash, 500);

		    function doUpdateHash(id) {

		        if (!id) {
		            return;
		        }
                
		        var hash = id !== '/visualizer' ? '#' + id : $window.location.pathname;
		        history.replaceState(null, null, hash);
		    }

		    function setupScrollSpy() {
		        $rootScope.$on('duScrollspy:becameActive', function ($event, $element) {
		            updateHash($element.prop('id') || $element.prop('href').substring($element.prop('href').indexOf('#') + 1));
		        });
		    }



		});


})();
