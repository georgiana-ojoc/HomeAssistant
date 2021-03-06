using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Requests;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Interfaces
{
    public interface ILightBulbRepository
    {
        Task<IEnumerable<LightBulb>> GetLightBulbsAsync(string email, Guid houseId, Guid roomId);
        Task<LightBulb> GetLightBulbByIdAsync(string email, Guid houseId, Guid roomId, Guid id);
        Task<LightBulb> CreateLightBulbAsync(string email, Guid houseId, Guid roomId, LightBulb lightBulb);

        Task<LightBulb> PartialUpdateLightBulbAsync(string email, Guid houseId, Guid roomId, Guid id,
            JsonPatchDocument<LightBulbRequest> lightBulbPatch);

        Task<LightBulb> DeleteLightBulbAsync(string email, Guid houseId, Guid roomId, Guid id);
    }
}