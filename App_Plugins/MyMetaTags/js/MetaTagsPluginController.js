angular.module('umbraco').controller('MetaTagsPluginController', function ($scope) {
    if ($scope.model.value === null || $scope.model.value === "") {
        $scope.model.value = {noFollow:false, noIndex:false};
    }
});