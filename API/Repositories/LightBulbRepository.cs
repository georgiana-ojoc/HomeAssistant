using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Repositories
{
    public class LightBulbRepository : BaseRepository, ILightBulbRepository
    {
        public LightBulbRepository(HomeAssistantContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LightBulb>> GetLightBulbsAsync(string email, Guid houseId, Guid roomId)
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

            return await Context.LightBulbs.Where(lightBulb => lightBulb.RoomId == room.Id).ToListAsync();
        }

        public async Task<LightBulb> GetLightBulbByIdAsync(string email, Guid houseId, Guid roomId, Guid id)
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

        public async Task<LightBulb> CreateLightBulbAsync(string email, Guid houseId, Guid roomId, LightBulb lightBulb)
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

            lightBulb.RoomId = room.Id;
            LightBulb newLightBulb = (await Context.LightBulbs.AddAsync(lightBulb)).Entity;
            await Context.SaveChangesAsync();
            return newLightBulb;
        }

        public async Task<LightBulb> DeleteLightBulbAsync(string email, Guid houseId, Guid roomId, Guid id)
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

            LightBulb lightBulb = await Context.LightBulbs.Where(lb => lb.RoomId == room.Id)
                .FirstOrDefaultAsync(lb => lb.Id == id);
            if (lightBulb == null)
            {
                return null;
            }

            Context.LightBulbs.Remove(lightBulb);
            await Context.SaveChangesAsync();
            return lightBulb;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync() >= 0;
        }
    }
}