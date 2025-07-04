
using backend.Repository;
using backend.Repository.RoomRepository;

namespace backend.Service.RoomsServices
{
    public class RoomService : IRoomServices
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public Task<bool> ExistsAsync(string roomName)
        {
            return _roomRepository.ExistsAsync(roomName);
        }
    }
}
