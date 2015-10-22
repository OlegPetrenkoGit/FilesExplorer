function bytesToSize(bytes) {
    var sizes = ['bytes', 'Kb', 'Mb', 'Gb'];

    if (bytes == 0) {
        return '0 bytes';
    }

    var order = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, order), 2) + ' ' + sizes[order];
};

var apiNodes = 'http://localhost:63319/api/nodes';
var apiNodesCount = 'http://localhost:63319/api/nodescount';
var apiDrives = 'http://localhost:63319/api/drives';

var filesExplorerApp = angular.module('filesExplorerApp');
filesExplorerApp.controller('nodeController', function ($scope, $http) {
    $scope.nodeTree;
    $scope.currentPath = "";
    $scope.filters = [less = 0,
                      between = 0,
                      more = 0];
    $scope.currentNode = null;
    $scope.buttonGoBackDisabled = true;

    $scope.Initialize = function () {
        $scope.FiltersReset();

        $http.get(apiDrives).success(function (response) {
            $scope.nodeTree = response;
        });
    }

    $scope.FiltersReset = function () {
        $scope.filters.less = 0;
        $scope.filters.between = 0;
        $scope.filters.more = 0;
    }

    $scope.UpdateCounters = function (json) {
        $scope.FiltersReset();

        var data = json.data;

        $scope.filters.less = data.Less;
        $scope.filters.between = data.Between;
        $scope.filters.more = data.More;
    }

    $scope.UpdateTree = function (json) {
        var newNodeTree = $scope.nodeTree;
        $scope.nodeTree = [];

        var nodes = json.data;

        angular.forEach(nodes, function (i) {
            $scope.nodeTree.push(i);

            if (i.Type == "file")
            {
                i.Size = bytesToSize(i.Size);
            }
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
                url: apiNodes,
                data: jsondata,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).then(function successCallback(response) {
                $scope.UpdateTree(response);

                $scope.NodesCountGet(node);
            }, function errorCallback(response) {
                alert("error select node");
            });
        }
    }

    $scope.NodesCountGet = function (node) {
        var jsondata = JSON.stringify(node);

        $http({
            method: 'POST',
            url: apiNodesCount,
            data: jsondata,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).then(function successCallback(response) {
            $scope.UpdateCounters(response);
        }, function errorCallback(response) {
            alert("error nodes count");
        });
    }

    $scope.GoParentNode = function () {
        if ($scope.currentNode.Type == "drive") {
            $scope.buttonGoBackDisabled = true;

            $scope.UpdateCurrentPath(null);

            $http({
                method: 'GET',
                url: apiDrives,
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
});