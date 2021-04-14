using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface ILightBulbRepository : IDisposable
    {
        Task<IEnumerable<LightBulb>> GetLightBulbsAsync(int userId, int houseId, int roomId);
        Task<LightBulb> GetLightBulbByIdAsync(int userId, int houseId, int roomId, int id);
        Task<LightBulb> CreateLightBulbAsync(int userId, int houseId, int roomId, LightBulb lightBulb);
        Task<LightBulb> DeleteLightBulbAsync(int userId, int houseId, int roomId, int id);
        Task<LightBulb> UpdateLightBulbAsync(int userId, int houseId, int roomId, LightBulb lightBulb);
    }
}