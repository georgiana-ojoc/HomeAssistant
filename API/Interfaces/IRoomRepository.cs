using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IRoomRepository : IDisposable
    {
        Task<IEnumerable<Room>> GetRoomsAsync(int userId, int houseId);
        Task<Room> GetRoomByIdAsync(int userId, int houseId, int id);
        Task<Room> CreateRoom(int userId, int houseId, Room room);
        Task<Room> DeleteRoom(int userId, int houseId, int id);
        Task<Room> UpdateRoom(int userId, int houseId, Room room);
    }
}