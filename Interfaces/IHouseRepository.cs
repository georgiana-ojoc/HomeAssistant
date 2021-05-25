using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAssistantAPI.Models;
using HomeAssistantAPI.Requests;
using Microsoft.AspNetCore.JsonPatch;

namespace HomeAssistantAPI.Interfaces
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