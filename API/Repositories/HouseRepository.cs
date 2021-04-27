using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Repositories
{
    public class HouseRepository : Repository, IHouseRepository
    {
        public HouseRepository(HomeAssistantContext context) : base(context)
        {
        }

        public async Task<IEnumerable<House>> GetHousesAsync(string email)
        {
            return await Context.Houses.Where(house => house.Email == email).ToListAsync();
        }

        public async Task<House> GetHouseByIdAsync(string email, Guid id)
        {
            return await Context.Houses.Where(house => house.Email == email)
                .FirstOrDefaultAsync(house => house.Id == id);
        }

        public async Task<House> CreateHouseAsync(string email, House house)
        {
            house.Email = email;
            House newHouse = (await Context.Houses.AddAsync(house)).Entity;
            await Context.SaveChangesAsync();
            return newHouse;
        }

        public async Task<House> DeleteHouseAsync(string email, Guid id)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (house == null)
            {
                return null;
            }

            Context.Houses.Remove(house);
            await Context.SaveChangesAsync();
            return house;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync() >= 0;
        }
    }
}