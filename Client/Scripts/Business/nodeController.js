function bytesToSize(bytes) {
    var sizes = ['bytes', 'Kb', 'Mb', 'Gb'];

    if (bytes == 0) {
        return '0 bytes';
    }

    var order = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, order), 2) + ' ' + sizes[order];
};

var apiAddress = 'http://localhost:63319/api/node';


var filesExplorerApp = angular.module('filesExplorerApp');
filesExplorerApp.controller('nodeController', function ($scope, $http) {
    $http.get(apiAddress).success(function (response) {

        $scope.nodeTree = response;
        $scope.currentPath = "";
        $scope.filters;
        $scope.currentNode = null;
        $scope.buttonGoBackDisabled = true;

        $scope.Initialize = function () {
            $scope.filters = [less = 0,
                              between = 0,
                              more = 0];

            $scope.FiltersReset();
        }

        $scope.FiltersReset = function () {
            $scope.filters.less = 0;
            $scope.filters.between = 0;
            $scope.filters.more = 0;
        }

        $scope.FilterFileSize = function (node) {
            var size = node.Size;
            var borders = [];
            borders['10mb'] = 10485760;
            borders['50mb'] = 52428800;
            borders['100mb'] = 104857600;

            if (size <= borders['10mb']) {
                $scope.filters.less++;
            }
            else if (borders['10mb'] > size && size <= borders['50mb']) {
                $scope.filters.between++;
            }
            else {
                $scope.filters.more++;
            }
        }

        $scope.UpdateTree = function (json) {
            var newNodeTree = $scope.nodeTree;
            $scope.nodeTree = [];

            $scope.FiltersReset();

            angular.forEach(json.data, function (i) {
                if (i.Type == "file") {
                    $scope.FilterFileSize(i);
                    i.Size = bytesToSize(i.Size);
                }

                $scope.nodeTree.push(i);
            });
        }

        $scope.UpdateCurrentPath = function (node) {
            $scope.currentPath = node == null ? "" : $scope.currentNode.Path;
        }

        $scope.SelectNode = function (node) {
            $scope.buttonGoBackDisabled = false;

            if (node.Type == "directory" || node.Type == "drive") {
                $scope.currentNode = node;

                $scope.UpdateCurrentPath(node);

                var jsondata = JSON.stringify(node);

                $http({
                    method: 'POST',
                    url: apiAddress,
                    data: jsondata,
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                }).then(function successCallback(response) {
                    $scope.UpdateTree(response);
                }, function errorCallback(response) {
                    alert("error");
                });
            }
        }

        $scope.GoParentNode = function () {
            if ($scope.currentNode.Type == "drive") {
                $scope.buttonGoBackDisabled = true;

                $scope.UpdateCurrentPath(null);

                $http({
                    method: 'GET',
                    url: apiAddress,
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                }).then(function successCallback(response) {
                    $scope.UpdateTree(response);
                }, function errorCallback(response) {
                    alert("error");
                });
            }
            else {
                var node = $scope.currentNode.Parent;
                $scope.buttonGoBackDisabled = true;

                $scope.SelectNode(node);
            }
        }

        $scope.Initialize();
    }
    );
});