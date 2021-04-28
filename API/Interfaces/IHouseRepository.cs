using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Shared.Models;
using Shared.Requests;

namespace API.Interfaces
{
    public interface IHouseRepository
    {
        Task<IEnumerable<House>> GetHousesAsync(string email);
        Task<House> GetHouseByIdAsync(string email, Guid id);
        Task<House> CreateHouseAsync(string email, House house);
        Task<House> PartialUpdateHouseAsync(string email, Guid id, JsonPatchDocument<HouseRequest> housePatch);
        Task<House> DeleteHouseAsync(string email, Guid id);
    }
}