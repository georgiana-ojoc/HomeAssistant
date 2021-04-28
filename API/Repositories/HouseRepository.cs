using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Requests;

namespace API.Repositories
{
    public class HouseRepository : BaseRepository, IHouseRepository
    {
        public HouseRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<House>> GetHousesAsync(string email)
        {
            CheckString(email, "email");

            return await Context.Houses.Where(house => house.Email == email).ToListAsync();
        }

        public async Task<House> GetHouseByIdAsync(string email, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(id, "id");

            return await Context.Houses.Where(house => house.Email == email)
                .FirstOrDefaultAsync(house => house.Id == id);
        }

        public async Task<House> CreateHouseAsync(string email, House house)
        {
            CheckString(email, "email");
            CheckString(house.Name, "name");

            int houses = await Context.Houses.CountAsync(h => h.Email == email);
            if (houses >= 5)
            {
                throw new ConstraintException(nameof(CreateHouseAsync));
            }

            house.Email = email;
            House newHouse = (await Context.Houses.AddAsync(house)).Entity;
            await Context.SaveChangesAsync();
            return newHouse;
        }

        public async Task<House> PartialUpdateHouseAsync(string email, Guid id,
            JsonPatchDocument<HouseRequest> housePatch)
        {
            CheckString(email, "email");
            CheckGuid(id, "id");

            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (house == null)
            {
                return null;
            }

            HouseRequest houseToPatch = Mapper.Map<HouseRequest>(house);
            housePatch.ApplyTo(houseToPatch);
            CheckString(houseToPatch.Name, "name");

            Mapper.Map(houseToPatch, house);
            await Context.SaveChangesAsync();
            return house;
        }

        public async Task<House> DeleteHouseAsync(string email, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(id, "id");

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
    }
}