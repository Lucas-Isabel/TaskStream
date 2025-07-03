using backend.Model.DTO;
using backend.Service;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Repository.InMemoryRepository
{
    public class ColumnInMemoryRepository : IRepository<ColumnsDataDTO>
    {
        public List<ColumnsDataDTO> Columns { get; set; } = new();

        public Task<ColumnsDataDTO> GetByIdAsync( int id)
        {
            if(Columns.Any(c => c.Id == id) == false)
            {
                throw new KeyNotFoundException($"Column with ID {id} not found.");
            }
            return  Task.FromResult(Columns.First(c => c.Id == id));
        }

        public Task<List<ColumnsDataDTO>> GetAll()
        {
            return Task.FromResult((Columns));
        }

        public Task<ColumnsDataDTO> PostAsync(ColumnsDataDTO objectToPost)
        {
            ColumnsDataDTO op = null;
            if(Columns.Any(c => c.Id == objectToPost.Id))
            {
                throw new ArgumentException($"Column with ID {objectToPost.Id} already exists.");
            }
            else
            {
                Columns.Add(objectToPost);
                op = objectToPost;
            }
            return Task.FromResult(op);
        }

        public Task<ColumnsDataDTO> PutAsync(ColumnsDataDTO objectToPut)
        {
            ColumnsDataDTO op = null;
            if (Columns.Any(c => c.Id == objectToPut.Id))
            {
                var index = Columns.FindIndex(c => c.Id == objectToPut.Id);
                if (index >= 0)
                {
                    Columns[index] = objectToPut;
                    op = objectToPut;
                }
            }
            else
            {
                throw new KeyNotFoundException($"Column with ID {objectToPut.Id} not found.");
            }
            return Task.FromResult(op);
        }

        public Task<ColumnsDataDTO> DeleteAsync(int id)
        {
            ColumnsDataDTO op = null;
            try
            {
                op = GetByIdAsync( id).Result;
                Columns.Remove(op);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Column with ID {id} not found.");
            }
            return Task.FromResult(op);
        }


    }
}
