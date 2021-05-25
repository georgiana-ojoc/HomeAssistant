using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Requests;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetRoomsAsync(string email, Guid houseId);
        Task<Room> GetRoomByIdAsync(string email, Guid houseId, Guid id);
        Task<Room> CreateRoomAsync(string email, Guid houseId, Room room);

        Task<Room> PartialUpdateRoomAsync(string email, Guid houseId, Guid id, JsonPatchDocument<RoomRequest>
            roomPatch);

        Task<Room> DeleteRoomAsync(string email, Guid houseId, Guid id);
    }
}