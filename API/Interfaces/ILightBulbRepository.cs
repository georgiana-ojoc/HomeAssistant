using System;
using System.Collections.Generic;
using API.Models;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ILightBulbRepository: IDisposable
    {
        Task<IEnumerable<LightBulb>> GetLightBulbsAsync(int userId, int houseId, int roomId);
        Task<LightBulb> GetLightBulbByIdAsync(int userId, int houseId, int roomId,int id);
        Task<LightBulb> CreateLightBulb(int userId, int houseId, int roomId,LightBulb lightBulb);
        Task<LightBulb> DeleteLightBulb(int userId, int houseId, int roomId,int id);
        Task<LightBulb> UpdateLightBulb(int userId, int houseId, int roomId,LightBulb lightBulb);
        
    }
}