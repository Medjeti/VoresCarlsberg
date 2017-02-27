(function () {
  'use strict';

  angular.module('designa.templates', [
    'template/carousel/carousel.html',
    'template/carousel/slide.html',
    'template/modal/backdrop.html',
    'template/modal/window.html'
  ]);

  angular.module('template/carousel/carousel.html', []).run(['$templateCache', function($templateCache) {
    $templateCache.put('template/carousel/carousel.html',
      '<div ng-mouseenter="pause()" ng-mouseleave="play()" class="carousel" ng-swipe-right="prev()" ng-swipe-left="next()">\n' +
      '  <div class="carousel-inner" ng-transclude></div>\n' +
      '  <a role="button" href class="left carousel-control hidden-xs" ng-click="prev()" ng-show="slides.length > 1">\n' +
      '    <span aria-hidden="true" class="glyphicon glyphicon-chevron-left icon-left"></span>\n' +
      '    <span class="sr-only">previous</span>\n' +
      '  </a>\n' +
      '  <a role="button" href class="right carousel-control hidden-xs" ng-click="next()" ng-show="slides.length > 1">\n' +
      '    <span aria-hidden="true" class="glyphicon glyphicon-chevron-right icon-right"></span>\n' +
      '    <span class="sr-only">next</span>\n' +
      '  </a>\n' +
      //'  <div class="carousel-index">{{ carousel.getCurrentIndex() + 1 }} / {{ slides.length }}</div>\n' +
      '  <ol class="carousel-indicators" ng-show="slides.length > 1">\n' +
      '    <li ng-repeat="slide in slides | orderBy:indexOfSlide track by $index" ng-class="{ active: isActive(slide) }" ng-click="select(slide)">\n' +
      '      <span class="sr-only">slide {{ $index + 1 }} of {{ slides.length }}<span ng-if="isActive(slide)">, currently active</span></span>\n' +
      '    </li>\n' +
      '  </ol>\n' +
      '</div>\n' +
      '');
  }]);

  angular.module('template/carousel/slide.html', []).run(['$templateCache', function($templateCache) {
    $templateCache.put('template/carousel/slide.html',
      '<div ng-class="{\n' +
      '    \'active\': active\n' +
      '  }" class="item text-center" ng-transclude></div>\n' +
      '');
  }]);

  angular.module('template/modal/backdrop.html', []).run(['$templateCache', function($templateCache) {
    $templateCache.put('template/modal/backdrop.html',
      '<div uib-modal-animation-class="fade"\n' +
      '     modal-in-class="in"\n' +
      '     ng-style="{\'z-index\': 1040 + (index && 1 || 0) + index*10}"\n' +
      '><button type="button" class="close" aria-label="Close"><span aria-hidden="true" class="icon-close"></span></button></div>\n' +
      '');
  }]);

  angular.module('template/modal/window.html', []).run(['$templateCache', function($templateCache) {
    $templateCache.put('template/modal/window.html',
      '<div modal-render="{{$isRendered}}" tabindex="-1" role="dialog" class="modal"\n' +
      '    uib-modal-animation-class="fade"\n' +
      '    modal-in-class="in"\n' +
      '    ng-style="{\'z-index\': 1050 + index*10, display: \'block\'}">\n' +
      '    <div class="modal-dialog" ng-class="size ? \'modal-\' + size : \'\'"><div class="modal-content" uib-modal-transclude></div></div>\n' +
      '</div>\n' +
      '');
  }]);
})();
