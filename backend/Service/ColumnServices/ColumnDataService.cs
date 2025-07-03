using backend.Model.DTO;
using backend.Repository;
using backend.Repository.InMemoryRepository;

namespace backend.Service.ColumnDataServices
{
    public class ColumnDataService : IService<ColumnsDataDTO>
    {
        private readonly IRepository<ColumnsDataDTO> _repository;
        public ColumnDataService(IRepository<ColumnsDataDTO> columnRepository)
        {
            _repository = columnRepository;
        }

        public Task<ColumnsDataDTO> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<List<ColumnsDataDTO>> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<ColumnsDataDTO> PostAsync(ColumnsDataDTO objectToPost)
        {
            return _repository.PostAsync(objectToPost);
        }

        public Task<ColumnsDataDTO> PutAsync(ColumnsDataDTO objectToPut)
        {
            return _repository.PutAsync(objectToPut);
        }

        public Task<ColumnsDataDTO> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
