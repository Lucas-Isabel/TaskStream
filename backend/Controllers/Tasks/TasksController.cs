using backend.Model.DTO;
using backend.Service;
using backend.Service.TaskService;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Tasks
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase, IService<TaskDTO>
    {
        private readonly IService<TaskDTO> _taskService;
        public TasksController(IService<TaskDTO> taskService)
        {
            _taskService = taskService;
        }
        [HttpDelete("{id}")]
        public Task<TaskDTO> DeleteAsync(int id)
        {
            return _taskService.DeleteAsync(id);
        }
        [HttpGet]
        public Task<List<TaskDTO>> GetAll()
        {
            return _taskService.GetAll();
        }
        [HttpGet("{id}")]
        public Task<TaskDTO> GetByIdAsync(int id)
        {
            return _taskService.GetByIdAsync(id);
        }
        [HttpPost]
        public Task<TaskDTO> PostAsync(TaskDTO objectToPost)
        {
            if (objectToPost == null)
            {
                throw new ArgumentNullException(nameof(objectToPost), "Task object cannot be null.");
            }
            return _taskService.PostAsync(objectToPost);
        }
        [HttpPut]
        public Task<TaskDTO> PutAsync(TaskDTO objectToPut)
        {
            if (objectToPut == null)
            {
                throw new ArgumentNullException(nameof(objectToPut), "Task object cannot be null.");
            }
            return _taskService.PutAsync(objectToPut);
        }
    }
}
