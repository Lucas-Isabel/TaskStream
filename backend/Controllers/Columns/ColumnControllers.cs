using backend.Model.DTO;
using backend.Service;
using backend.Service.ColumnDataServices;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Columns
{
    [ApiController]
    [Route("/{roomName}/[controller]")]
    public class ColumnControllers : ControllerBase, IService<ColumnsDataDTO>
    {
        private readonly IService<ColumnsDataDTO> _columnDataService;
        public ColumnControllers(IService<ColumnsDataDTO> columnDataService)
        {
            _columnDataService = columnDataService;
        }

        [HttpDelete("{id}")]
        public Task<ColumnsDataDTO> DeleteAsync(int id)
        {
            return _columnDataService.DeleteAsync(id);
        }
        [HttpGet]
        public Task<List<ColumnsDataDTO>> GetAll()
        {
            return _columnDataService.GetAll();
        }
        [HttpGet("{id}")]
        public Task<ColumnsDataDTO> GetByIdAsync(int id)
        {
            return _columnDataService.GetByIdAsync(id);
        }
        [HttpPost]
        public Task<ColumnsDataDTO> PostAsync(ColumnsDataDTO objectToPost)
        {
            return _columnDataService.PostAsync(objectToPost);
        }
        [HttpPut]
        public Task<ColumnsDataDTO> PutAsync(ColumnsDataDTO objectToPut)
        {
            return _columnDataService.PutAsync(objectToPut);
        }
    }
}
