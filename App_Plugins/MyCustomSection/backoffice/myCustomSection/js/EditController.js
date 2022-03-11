angular.module('umbraco').controller('MyCustomSectionEditController', function ($scope, $http) {
    $scope.create = false;
    var path = window.location.href;
    var page = path.split("/")[path.split("/").length - 1];
    page = (page.includes("?") ? page.split("?")[0] : page);
    $scope.model = {
        Title: "",
        Description: ""
    }
    if (page == 0) {
        $scope.create = true;
        $scope.model.btnText = "Create";
    }
    else {
        $scope.model.btnText = "Update";
        //Fill data
        $http.get('/CustomSection/GetById?id=' + page).then(function successCallback(response) {
            let data = response.data;
            $scope.model.Id = data.Id;
            $scope.model.Title = data.Title;
            $scope.model.Description = data.Description;
            $scope.model.Created = data.Created;

        }, function errorCallback(response) {
            alert("Er ging iets fout!");
        });
    }
    $scope.send = function () {
        alert($scope.model.Title + "\n" + $scope.model.Description);
        console.log($scope.create);
        if ($scope.create) {
            $http.post('/CustomSection/Create', JSON.stringify($scope.model)).then(function () {
                $scope.model.Title = "";
                $scope.model.Description = "";
                document.location.reload();
            });
        }
        else {
            $http.post('/CustomSection/Update', JSON.stringify($scope.model)).then(function () {
                document.location.reload();
            });
        }
    }
});