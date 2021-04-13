using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IDoorRepository : IDisposable
    {
        Task<IEnumerable<Door>> GetDoorsAsync(int userId, int houseId, int roomId);
        Task<Door> GetDoorByIdAsync(int userId, int houseId, int roomId, int id);
        Task<Door> CreateDoor(int userId, int houseId, int roomId, Door door);
        Task<Door> DeleteDoor(int userId, int houseId, int roomId, int id);
        Task<Door> UpdateDoor(int userId, int houseId, int roomId, Door door);
    }
}