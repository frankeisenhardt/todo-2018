using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TodoApi.Controllers
{
    [Route("api/todo")]
    public class TodoController : Controller
    {
        private readonly ITodoDbService _dbService;
        private readonly ILogger _logger;

        public TodoController(ITodoDbService dbService,ILogger<TodoController> logger)
        {
            _dbService = dbService;
            _logger = logger;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _dbService.GetTodoItemsAsync();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(string id)
        {
            return NotFound();
            //return new ObjectResult(item);
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> CreateAsync([FromBody] TodoItem item)
        {
            item.type = "todo";
            item._id = "";
            item._rev = "";
            var ret = await _dbService.AddItemAsync(item);
            if (ret != null)
            {
               return CreatedAtRoute("GetTodo", new { id = ret._id }, ret);
            }
            else
            {
                return null;
            }
        }

        [HttpPut("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> UpdateAsync(string id, [FromBody] TodoItem item)
        {
            if (item == null || !item._id.Equals(id))
            {
                return BadRequest();
            }
            var ret = await _dbService.UpdateItemAsync(item);
            if (ret != null)
            {
                return new NoContentResult();
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<IActionResult> DeleteAsync(string id,[FromQuery]QueryParameters parameters)
        {
			if (!ModelState.IsValid)
			{
                return BadRequest();
			}
			
            var ret = await _dbService.DeleteItemAsync(id,parameters.rev);
            if (ret != null)
            {
                return new NoContentResult();
            }
            else
            {
                return NotFound();
            }
        }
    }

	public class QueryParameters
	{
		[BindRequired]
		public string rev { get; set; }
	}

}
