using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeAssistantAPI.Interfaces;
using HomeAssistantAPI.Models;
using HomeAssistantAPI.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace HomeAssistantAPI.Repositories
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

            return await GetHouseInternalAsync(email, id);
        }

        public async Task<House> CreateHouseAsync(string email, House house)
        {
            CheckString(email, "email");
            CheckString(house.Name, "name");

            int limit = await GetLimit("houses", email);
            int housesByEmail = await Context.Houses.CountAsync(h => h.Email == email);
            if (housesByEmail >= limit)
            {
                throw new ConstraintException("You have no houses left. Upgrade your subscription.");
            }

            int housesByEmailAndName = await Context.Houses.CountAsync(h => h.Email == email &&
                                                                            h.Name == house.Name);
            if (housesByEmailAndName > 0)
            {
                throw new DuplicateNameException("You already have a house with the specified name.");
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

            House house = await GetHouseInternalAsync(email, id);
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

            House house = await GetHouseInternalAsync(email, id);
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