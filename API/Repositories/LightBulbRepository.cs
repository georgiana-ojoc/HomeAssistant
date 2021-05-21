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
    public class LightBulbRepository : BaseRepository, ILightBulbRepository
    {
        public LightBulbRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        private async Task<LightBulb> GetLightBulbInternalAsync(string email, Guid houseId, Guid roomId, Guid id)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await Context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            return await Context.LightBulbs.Where(lightBulb => lightBulb.RoomId == room.Id)
                .FirstOrDefaultAsync(lightBulb => lightBulb.Id == id);
        }

        public async Task<IEnumerable<LightBulb>> GetLightBulbsAsync(string email, Guid houseId, Guid roomId)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house id");
            CheckGuid(roomId, "room id");

            Room room = await GetRoomInternalAsync(email, houseId, roomId);
            if (room == null)
            {
                return null;
            }

            return await Context.LightBulbs.Where(lightBulb => lightBulb.RoomId == room.Id).ToListAsync();
        }

        public async Task<LightBulb> GetLightBulbByIdAsync(string email, Guid houseId, Guid roomId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house id");
            CheckGuid(roomId, "room id");
            CheckGuid(id, "id");

            return await GetLightBulbInternalAsync(email, houseId, roomId, id);
        }

        public async Task<LightBulb> CreateLightBulbAsync(string email, Guid houseId, Guid roomId, LightBulb lightBulb)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house id");
            CheckGuid(roomId, "room id");
            CheckString(lightBulb.Name, "name");

            Room room = await GetRoomInternalAsync(email, houseId, roomId);
            if (room == null)
            {
                return null;
            }

            UserLimit userLimit = Context.UserLimits.Where(u => u.Email == email).FirstOrDefault();
            int limit = 10;
            if (userLimit != null)
                limit = userLimit.LightBulbLimit;
            int lightBulbsByRoomId = await Context.LightBulbs.CountAsync(lb => lb.RoomId == roomId);
            if (lightBulbsByRoomId >= limit)
            {
                throw new ConstraintException("You have no light bulbs left in this room. Upgrade your plan.");
            }

            int lightBulbsByRoomIdAndName = await Context.LightBulbs.CountAsync(lb => lb.RoomId == roomId &&
                lb.Name == lightBulb.Name);
            if (lightBulbsByRoomIdAndName > 0)
            {
                throw new DuplicateNameException("You already have a light bulb with the specified name in this " +
                                                 "room.");
            }

            lightBulb.RoomId = room.Id;
            LightBulb newLightBulb = (await Context.LightBulbs.AddAsync(lightBulb)).Entity;
            await Context.SaveChangesAsync();
            return newLightBulb;
        }

        public async Task<LightBulb> PartialUpdateLightBulbAsync(string email, Guid houseId, Guid roomId, Guid id,
            JsonPatchDocument<LightBulbRequest> lightBulbPatch)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house id");
            CheckGuid(roomId, "room id");
            CheckGuid(id, "id");

            LightBulb lightBulb = await GetLightBulbInternalAsync(email, houseId, roomId, id);
            if (lightBulb == null)
            {
                return null;
            }

            LightBulbRequest lightBulbToPatch = Mapper.Map<LightBulbRequest>(lightBulb);
            lightBulbPatch.ApplyTo(lightBulbToPatch);
            CheckString(lightBulbToPatch.Name, "name");

            Mapper.Map(lightBulbToPatch, lightBulb);
            await Context.SaveChangesAsync();
            return lightBulb;
        }

        public async Task<LightBulb> DeleteLightBulbAsync(string email, Guid houseId, Guid roomId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house id");
            CheckGuid(roomId, "room id");
            CheckGuid(id, "id");

            LightBulb lightBulb = await GetLightBulbInternalAsync(email, houseId, roomId, id);
            if (lightBulb == null)
            {
                return null;
            }

            Context.LightBulbs.Remove(lightBulb);
            await Context.SaveChangesAsync();
            return lightBulb;
        }
    }
}