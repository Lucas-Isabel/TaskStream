
namespace backend.Repository.RoomRepository
{
    public class RoomRepository : IRoomRepository
    {
        public Task<bool> ExistsAsync(string roomName)
        {
            return Task.FromResult(DynamicSetup.FileExists(roomName));
        }
    }
}
