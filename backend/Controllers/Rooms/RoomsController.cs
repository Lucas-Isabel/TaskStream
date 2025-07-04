using backend.Model.DTO;
using backend.Service;
using backend.Service.RoomsServices;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Rooms;

[ApiController]
[Route("/{roomName}/[controller]")]
public class RoomsController : ControllerBase, IRoomServices
{
    private readonly IRoomServices _roomService;
    public RoomsController(IRoomServices roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public Task<bool> ExistsAsync(string roomName)
    {
        return _roomService.ExistsAsync(roomName);
    }
}
