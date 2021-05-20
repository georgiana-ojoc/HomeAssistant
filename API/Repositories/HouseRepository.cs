using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Shared;
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

            return await GetHouseInternalAsync(email, id);
        }

        public async Task<House> CreateHouseAsync(string email, House house)
        {
            CheckString(email, "email");
            CheckString(house.Name, "name");

            int housesByEmail = await Context.Houses.CountAsync(h => h.Email == email);
            if (housesByEmail >= 5)
            {
                throw new ConstraintException(nameof(CreateHouseAsync));
            }

            int housesByEmailAndName = await Context.Houses.CountAsync(h => h.Email == email &&
                                                                            h.Name == house.Name);
            if (housesByEmailAndName > 0)
            {
                throw new DuplicateNameException(nameof(CreateHouseAsync));
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