using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IHouseRepository : IDisposable
    {
        Task<IEnumerable<House>> GetHousesAsync(int userId);
        Task<House> GetHouseByIdAsync(int userId,int id);
        Task<House> CreateHouse(int userId,House house);
        Task<House> DeleteHouse(int userId,int id);
        Task<House> UpdateHouse(int userId,House house);
        
    }
}