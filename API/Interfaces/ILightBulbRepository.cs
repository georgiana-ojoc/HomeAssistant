using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface ILightBulbRepository : IDisposable
    {
        Task<IEnumerable<LightBulb>> GetLightBulbsAsync(string email, int houseId, int roomId);
        Task<LightBulb> GetLightBulbByIdAsync(string email, int houseId, int roomId, int id);
        Task<LightBulb> CreateLightBulbAsync(string email, int houseId, int roomId, LightBulb lightBulb);
        Task<LightBulb> DeleteLightBulbAsync(string email, int houseId, int roomId, int id);
    }
}