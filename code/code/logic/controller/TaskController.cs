using code.logic.model;
using code.logic.mongo.service;
using Microsoft.AspNetCore.Mvc;

namespace code.logic.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult<List<ToDoListTask>> Get() =>
            _taskService.Get();

        [HttpGet("{id:length(24)}", Name = "GetTask")]
        public ActionResult<ToDoListTask> Get(string id)
        {
            var task = _taskService.Get(id);

            if (task == null)
            {
                return new NotFoundResult();
            }

            return task;
        }

        [HttpPost]
        public ActionResult<ToDoListTask> Create(ToDoListTask task)
        {
            _taskService.Create(task);

            return new CreatedAtRouteResult("GetTask", new { id = task.Id.ToString() }, task);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ToDoListTask taskIn)
        {
            var task = _taskService.Get(id);

            if (task == null)
            {
                return new NotFoundResult();
            }

            _taskService.Update(id, taskIn);

            return new NoContentResult();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var task = _taskService.Get(id);

            if (task == null)
            {
                return new NotFoundResult();
            }

            _taskService.Remove(task.Id.ToString());

            return new NoContentResult();
        }

        [HttpPatch("{id:length(24)}")]
        public IActionResult Patch(string id, ToDoListTask taskIn)
        {
            var task = _taskService.Get(id);

            if (task == null)
            {
                return new NotFoundResult();
            }

            _taskService.Patch(id, taskIn);

            return new NoContentResult();
        }

    }
}