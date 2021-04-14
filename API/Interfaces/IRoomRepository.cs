using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface IRoomRepository : IDisposable
    {
        Task<IEnumerable<Room>> GetRoomsAsync(int userId, int houseId);
        Task<Room> GetRoomByIdAsync(int userId, int houseId, int id);
        Task<Room> CreateRoomAsync(int userId, int houseId, Room room);
        Task<Room> DeleteRoomAsync(int userId, int houseId, int id);
        Task<Room> UpdateRoomAsync(int userId, int houseId, Room room);
    }
}