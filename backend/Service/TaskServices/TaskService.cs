using backend.Model.DTO;
using backend.Repository;
using backend.Repository.SqLiteRepository;

namespace backend.Service.TaskService
{
    public class TaskService : IService<TaskDTO>
    {
        private readonly IRepository<TaskDTO> _repository;
        public TaskService(IRepository<TaskDTO> taskRepository)
        {
            _repository = taskRepository;
        }
        public Task<TaskDTO> DeleteAsync(int id)
        {
          return _repository.DeleteAsync(id);
        }

        public Task<List<TaskDTO>> GetAll()
        {
            return _repository.GetAll(); 
        }
        
        public Task<TaskDTO> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<TaskDTO> PostAsync(TaskDTO objectToPost)
        {
            return _repository.PostAsync(objectToPost);
        }

        public Task<TaskDTO> PutAsync(TaskDTO objectToPut)
        {
            return _repository.PutAsync(objectToPut);
        }
    }
}
