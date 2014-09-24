angular.module('home', ['arrangeGame', 'timer', 'ngCookies'])
    .controller('homeCtrl',['$scope','$http', '$cookies',  function ($scope, $http, $cookies) {
        $scope.finished = function (gameId) {
            $scope.$broadcast('timer-stop');
        };
        $scope.isGameover = false;
        $scope.isSaved = false;
        $scope.gameId = null;
        $scope.player = null;
        $scope.isEditMode = false;
        $scope.time = {};
        $scope.$on('timer-stopped', function (event, data) {
            if (data)
                $scope.time = data;
            if (!$scope.isSaved) {
                $scope.player = 'Anonymous'
                $scope.saveScore($scope.player, $scope.time.millis, $scope.gameId);
                $scope.isEditMode = true;
               
            }

            $scope.isGameover = true;
        });
        $scope.topPlayers = null;

        $scope.getTopList = function () {
            $http.get('/api/ArrangeGame/TopPlayers')
                .success(function (data, status, headers, config) {
                    $scope.topPlayers = data;
                });
        }

        $scope.saveScore = function (name, time, gameId) {
            item =
              {
                  player: name,
                  time: time,
                  gameid: gameId
              };
            if ($scope.time) {
                $http.post('/api/ArrangeGame/SaveGame', item)
                    .success(function (data, status, headers, config) {
                        $scope.isSaved = true;

                        // refresh top list
                        $scope.getTopList();
                    });
            }
        };

        $scope.savePlayer = function () {
            item =
              {
                  player: $scope.player,
                  time: $scope.time,
                  gameid: $scope.gameId
              };
            $http.put('/api/ArrangeGame/SavePlayer', item)
            .success(function (data, status, headers, config) {
                // refresh top list
                $scope.getTopList();
            });

            $scope.isEditMode = false;
        };

        // get current top list when the page loads
        $scope.getTopList();
    }])
    .filter('time', function () {
        return function (time) {
            var t = utils.millsToTime(time);
            var timeStr = '';
            if (t.hours > 0)
                timeStr += t.hours + ' hours ';
            if (t.minutes > 0)
                timeStr += t.minutes + ' min ';
            timeStr += t.seconds + ' sec.';

            return timeStr;
        };
    });

