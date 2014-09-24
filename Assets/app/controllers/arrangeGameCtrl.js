angular.module("arrangeGame", [])
.controller("arrangeGameCtrl", ['$scope', function ($scope) {
    $scope.numbers = [1, 2, 3, 4];
    $scope.rows = [1, 2, 3, 4];
    $scope.grid = new Grid(4, 4);
    $scope.gameId = $scope.gameIdAttr;

    var checkProgress = function () {
        var isSolved = true;
        var temp = '';
        for (var i = 1; i < $scope.grid.cells.length; i++) {
            if ($scope.grid.cells[i - 1].value != i)
                isSolved = false;
        }
        console.log(temp);
        if (isSolved)
            $scope.done();
    };

    $scope.done = function () {
        if ($scope.finishCallback) {
            $scope.$eval($scope.finishCallback);
            console.log('checking finishCallback');
        }
    };


    $scope.grid.shuffle();
    $scope.range = function (num) {
        return new Array(num);
    }
    function Grid(rows, cols, cells) {
        var self = this;

        self.rows = rows;
        self.cols = cols;
        if (cells == null)
            cells = makeArray(self.cols * self.rows, function (i) {
                if (i == self.cols * self.rows - 1) {
                    return new Cell("", self.cols, self.rows);
                }
                else
                    return new Cell(i + 1, i % cols + 1, ~~(i / rows) + 1);
            });
        self.cells = cells;

        self.getCell = function (x, y) {
            if (x <= 0 || y <= 0)
                return null;
            if (self.cells && (x * y <= self.cells.length))
                return cells[(y - 1) * cols + x - 1];
            else
                return null;
        }

        self.moveCell = function (x, y) {
            var cell = self.getCell(x, y);
            if (!cell.isEmpty()) {
                //try to move cell
                if (self.getCell(x - 1, y) && self.getCell(x - 1, y).isEmpty())
                    moveTo(x - 1, y);
                else if (self.getCell(x + 1, y) && self.getCell(x + 1, y).isEmpty())
                    moveTo(x + 1, y);
                else if (self.getCell(x, y - 1) && self.getCell(x, y - 1).isEmpty())
                    moveTo(x, y - 1);
                else if (self.getCell(x, y + 1) && self.getCell(x, y + 1).isEmpty())
                    moveTo(x, y + 1);
            }

            function moveTo(newX, newY) {
                var replaceCell = self.getCell(newX, newY);
                var tempValue = replaceCell.value;
                replaceCell.value = cell.value;
                cell.value = tempValue;

                console.log('moving to: %s and %s', replaceCell.x, replaceCell.y);

                checkProgress();
            }

            console.log('moving: %s and %s', x, y);
        }
        self.shuffle = function () {
            var newCells = shuffleGrid(self.cells, self.cols);
            for (var i = 0; i < self.rows; i++) {
                for (var j = 0; j < self.cols; j++) {
                    self.cells[i * self.rows + j].x = j + 1;
                    self.cells[i * self.rows + j].y = i + 1;
                }
            }
            // make sure it is a solvable puzzle

            self.cells = newCells;
        };
    }

    function Cell(value, x, y) {
        var self = this;
        self.value = value;
        self.isEmpty = function () { return self.value == "" };
        self.x = x;
        self.y = y;

    }

    function makeArray(count, content) {
        var result = [];
        if (typeof (content) == "function") {
            for (var i = 0; i < count; i++) {
                result.push(content(i));
            }
        } else {
            for (var i = 0; i < count; i++) {
                result.push(content);
            }
        }
        return result;
    }

    function shuffleGrid(array, cols) {
        for (var i = 0; i < 15; i++) {
            var j = Math.floor(Math.random() * 100);
            // TODO move backwards from initial puzzle
            if (j < 25)
            {
                //move up
            }
            if (j >=25 && j< 50) {
                //move down
            }
            if (j >= 50 && j < 75) {
                //move right
            }
            if (j >= 75 )
            {
                //move left
            }
            //var temp = array[k];
            //array[k] = array[j];
            //array[j] = temp;
        }
        return array;
    }
}])
.directive('myArrangeGame', function () {
    return {
        restrict: 'EAC',
        replace: false,
        scope: {
            gameIdAttr: '=gameId',
            finishCallback: '&finishCallback'
        },
       templateUrl: '/App/ArrangeGame'
    };
});