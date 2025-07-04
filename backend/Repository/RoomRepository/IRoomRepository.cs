namespace backend.Repository.RoomRepository;

public interface IRoomRepository
{
    public Task<bool> ExistsAsync(string roomName);
}
