namespace backend.Service.RoomsServices;

public interface IRoomServices
{
    public Task<bool> ExistsAsync(string roomName);
}
