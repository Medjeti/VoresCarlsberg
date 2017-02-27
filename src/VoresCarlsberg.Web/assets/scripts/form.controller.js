(function () {
	'use strict';

	angular.module('VoresCarlsberg').controller('FormCtrl', function ($scope, $document, formService) {

		// -------------------------------------------------------------------------
		// Public Method & Variables
		// -------------------------------------------------------------------------

		$scope.formFlow = 1;
		$scope.guest = {};

		$scope.nextStep = nextStep;
		$scope.submitForm = submitForm;

		$scope.divisions = getDivisions();
		
		
		// -------------------------------------------------------------------------

		function nextStep(form, step) {
			console.log(form);
			if (form.$valid) {
				$scope.formFlow = step;
				if (step === 2) {
					location.href = "#step2";
				}
			} else {
				
			}
		}

		// -------------------------------------------------------------------------

		function submitForm() {
			
		}

		// -------------------------------------------------------------------------

		function getDivisions() {
			var list = formService.getDivisions();
			return list;
		}
	});

})();