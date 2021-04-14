using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface IRoomRepository : IDisposable
    {
        Task<IEnumerable<Room>> GetRoomsAsync(string email, int houseId);
        Task<Room> GetRoomByIdAsync(string email, int houseId, int id);
        Task<Room> CreateRoomAsync(string email, int houseId, Room room);
        Task<Room> DeleteRoomAsync(string email, int houseId, int id);
    }
}