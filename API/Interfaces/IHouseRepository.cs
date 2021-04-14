using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface IHouseRepository : IDisposable
    {
        Task<IEnumerable<House>> GetHousesAsync(int userId);
        Task<House> GetHouseByIdAsync(int userId, int id);
        Task<House> CreateHouseAsync(int userId, House house);
        Task<House> DeleteHouseAsync(int userId, int id);
        Task<House> UpdateHouseAsync(int userId, House house);
    }
}