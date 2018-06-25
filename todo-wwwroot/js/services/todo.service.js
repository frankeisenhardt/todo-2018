var todoService = angular.module('todoService', [])

// super simple service
// each function returns a promise object
todoService.factory('todos', ['$http', function ($http) {
  return {
    get: function () {
      return $http.get('api/todo');
    },
    create: function (todoData) {
      return $http.post('api/todo', todoData);
    },
    delete: function (todo) {
      return $http.delete('api/todo/' + todo._id + "?rev=" + todo._rev);
    },
    update: function (todo) {
      return $http.put('api/todo/' + todo._id, todo);
    }
  }
}]);
