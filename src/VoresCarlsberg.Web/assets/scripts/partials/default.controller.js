(function () {
	'use strict';

	angular.module('designa').controller('DefaultCtrl', function ($scope, $document) {

		$scope.showOverlay = false;

		$scope.hideOverlay = function() {
			$scope.showOverlay = false;
		}

		$document.bind('keyup', function (e) {
			if (e.keyCode === 27) {
				$scope.hideOverlay();
				//console.log('Yo');
			}
		});
		
	});

})();