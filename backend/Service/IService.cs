using Microsoft.AspNetCore.SignalR;

namespace backend.Service
{
    public interface IService<T>
    {
        public Task<T> GetByIdAsync(int id);
        public Task<List<T>> GetAll();
        public Task<T> PostAsync(T objectToPost);
        public Task<T> PutAsync(T objectToPut);
        public Task<T> DeleteAsync(int id);
    }
}
