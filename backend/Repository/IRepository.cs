namespace backend.Repository
{
    public interface IRepository<T>
    {
        public Task<T> GetByIdAsync(int id);
        public Task<List<T>> GetAll();
        public Task<T> PostAsync(T objectToPost);
        public Task<T> PutAsync(T objectToPut);
        public Task<T> DeleteAsync(int id);
    }
}
