﻿(function () {
	'use strict';

	angular.module('VoresCarlsberg').controller('FormCtrl', function ($scope, $location, $anchorScroll, filterFilter, formService) {

		// -------------------------------------------------------------------------
		// Public Method & Variables
		// -------------------------------------------------------------------------

		//alert(new Date().getTime() > new Date(2017, 2, 24, 20).getTime());

		$scope.formFlow = 1; // new Date().getTime() > new Date(2017, 2, 25, 7).getTime() ? - 2 : 1;
		$scope.loginError = false;
		$scope.guest = {};
		//$scope.selectedHobbies = [];
		$scope.selectedHobbies = selectedHobbies;

		$scope.submitForm = submitForm;
		$scope.submitFinalForm = submitFinalForm;
		$scope.completeSubmission = completeSubmission;
		$scope.loginUser = loginUser;
		$scope.change = change;
		$scope.hobbyClick = hobbyClick;
		$scope.isOtherHobbySelected = isOtherHobbySelected;

		$scope.divisions = getDivisions();
		$scope.hobbies = getHobbies();
		
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
					$scope.hobbies = getHobbies($scope.guest);
					$scope.formFlow = 2;
					// the element you wish to scroll to.
					$location.hash('step2');

					// call $anchorScroll()
					$anchorScroll();
				} else {
					completeSubmission(0);
				}
			});

		}

		// -------------------------------------------------------------------------

		function submitFinalForm(form) {
			console.log($scope.guest.otherHobby);
			if (!form.$valid) {
				$scope.validationError = true;
			} else {
				$scope.guest.selectedHobbies = $scope.selectedHobbies.toString();
				if (!isOtherHobbySelected()) {
					$scope.guest.otherHobby = "";
				}
				//console.log($scope.guest);
				completeSubmission(3);
			}
		}

		// -------------------------------------------------------------------------

		function completeSubmission(nextStep) {

			formService.completeSubmission($scope.guest).success(function (data) {
				$scope.formFlow = nextStep;
			}).error(function () {
				alert("Der er desværre sket en fejl.");
			});
		}

		// -------------------------------------------------------------------------

		function getDivisions() {
			var list = formService.getDivisions();
			return list;
		}

		// -------------------------------------------------------------------------

		function getHobbies(guest) {
			var list = formService.getHobbies(guest);
			return list;
		}

		function selectedHobbies() {
			return filterFilter($scope.hobbies, { selected: true });
		};

		function isOtherHobbySelected() {
			return $scope.selectedHobbies.indexOf('Andet') > -1;
		}

		function hobbyClick(hobby) {
			
			if ($scope.selectedHobbies.length > 2) {
				hobby.selected = false;
			}
		}

		$scope.$watch('hobbies|filter:{selected:true}', function (nv) {
			$scope.selectedHobbies = nv.map(function (hobby) {
				return hobby.name;
			});
		}, true);
	});

})();