using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface IDoorRepository : IDisposable
    {
        Task<IEnumerable<Door>> GetDoorsAsync(int userId, int houseId, int roomId);
        Task<Door> GetDoorByIdAsync(int userId, int houseId, int roomId, int id);
        Task<Door> CreateDoorAsync(int userId, int houseId, int roomId, Door door);
        Task<Door> DeleteDoorAsync(int userId, int houseId, int roomId, int id);
        Task<Door> UpdateDoorAsync(int userId, int houseId, int roomId, Door door);
    }
}