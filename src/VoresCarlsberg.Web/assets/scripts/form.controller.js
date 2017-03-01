(function () {
	'use strict';

	angular.module('VoresCarlsberg').controller('FormCtrl', function ($scope, $document, formService) {

		// -------------------------------------------------------------------------
		// Public Method & Variables
		// -------------------------------------------------------------------------

		$scope.formFlow = 1;
		$scope.loginError = false;
		$scope.guest = {};

		$scope.submitForm = submitForm;
		$scope.completeSubmission = completeSubmission;
		$scope.loginUser = loginUser;
		$scope.change = change;

		$scope.divisions = getDivisions();
		
		// -------------------------------------------------------------------------

		function loginUser(callback) {
			var login = {
				employeeno: $scope.guest.employeeno,
				firstname: $scope.guest.firstname
			};

			formService.login(login).success(function (data) {
				if (data === false) {
					$scope.loginError = true;
				} else {
					callback(data);
				}
			}).error(function () {
				$scope.loginError = true;
			});
		}
		
		// -------------------------------------------------------------------------

		function change() {
			$scope.loginError = false;
		}

		// -------------------------------------------------------------------------

		function submitForm(isAttending) {

			loginUser(function (data) {
				$scope.guest.isAttending = isAttending;
				if (isAttending) {
					var login = $scope.guest;

					if (data.guest) {
						$scope.guest = data.guest;
					}
					$scope.guest.isAttending = true;
					$scope.guest.firstname = login.firstname;
					$scope.guest.employeeno = login.employeeno;
					$scope.formFlow = 2;

					// Make two first fields read-only

				} else {
					completeSubmission();
					$scope.formFlow = 0;
				}
			});

		}

		// -------------------------------------------------------------------------

		function completeSubmission() {

			formService.completeSubmission($scope.guest).success(function (data) {
				$scope.formFlow = 3;
			}).error(function () {
				alert("Der er desværre sket en fejl.");
			});
		}

		// -------------------------------------------------------------------------

		function getDivisions() {
			var list = formService.getDivisions();
			return list;
		}
	});

})();