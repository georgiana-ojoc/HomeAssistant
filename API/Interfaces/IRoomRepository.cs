using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface IRoomRepository : IDisposable
    {
        Task<IEnumerable<Room>> GetRoomsAsync(string email, Guid houseId);
        Task<Room> GetRoomByIdAsync(string email, Guid houseId, Guid id);
        Task<Room> CreateRoomAsync(string email, Guid houseId, Room room);
        Task<Room> DeleteRoomAsync(string email, Guid houseId, Guid id);
    }
}