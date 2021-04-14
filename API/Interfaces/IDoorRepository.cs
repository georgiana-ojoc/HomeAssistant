using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface IDoorRepository : IDisposable
    {
        Task<IEnumerable<Door>> GetDoorsAsync(string email, int houseId, int roomId);
        Task<Door> GetDoorByIdAsync(string email, int houseId, int roomId, int id);
        Task<Door> CreateDoorAsync(string email, int houseId, int roomId, Door door);
        Task<Door> DeleteDoorAsync(string email, int houseId, int roomId, int id);
    }
}