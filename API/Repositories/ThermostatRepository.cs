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
    public class ThermostatRepository : BaseRepository, IThermostatRepository
    {
        public ThermostatRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<Thermostat>> GetThermostatsAsync(string email, Guid houseId, Guid roomId)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(roomId, "room_id");
            
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

            return await Context.Thermostats.Where(thermostat => thermostat.RoomId == room.Id).ToListAsync();
        }

        public async Task<Thermostat> GetThermostatByIdAsync(string email, Guid houseId, Guid roomId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(roomId, "room_id");
            CheckGuid(id, "id");
            
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

            return await Context.Thermostats.Where(thermostat => thermostat.RoomId == room.Id)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Thermostat> CreateThermostatAsync(string email, Guid houseId, Guid roomId,
            Thermostat thermostat)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(roomId, "room_id");
            CheckString(thermostat.Name, "name");
            
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

            int thermostats = await Context.Thermostats.CountAsync(t => t.RoomId == roomId);
            if (thermostats >= 10)
            {
                throw new ConstraintException(nameof(CreateThermostatAsync));
            }
            
            thermostat.RoomId = room.Id;
            Thermostat newThermostat = (await Context.Thermostats.AddAsync(thermostat)).Entity;
            await Context.SaveChangesAsync();
            return newThermostat;
        }

        public async Task<Thermostat> PartialUpdateThermostatAsync(string email, Guid houseId, Guid roomId, Guid id, JsonPatchDocument<ThermostatRequest> thermostatPatch)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(roomId, "room_id");
            CheckGuid(id, "id");
            
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

            Thermostat thermostat = await Context.Thermostats.Where(t => t.RoomId == room.Id)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (thermostat == null)
            {
                return null;
            }
            
            ThermostatRequest thermostatToPatch = Mapper.Map<ThermostatRequest>(thermostat);
            thermostatPatch.ApplyTo(thermostatToPatch);
            CheckString(thermostatToPatch.Name, "name");

            Mapper.Map(thermostatToPatch, thermostat);
            await Context.SaveChangesAsync();
            return thermostat;
        }

        public async Task<Thermostat> DeleteThermostatAsync(string email, Guid houseId, Guid roomId, Guid id)
        {
            CheckString(email, "email");
            CheckGuid(houseId, "house_id");
            CheckGuid(roomId, "room_id");
            CheckGuid(id, "id");
            
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

            Thermostat thermostat = await Context.Thermostats.Where(t => t.RoomId == room.Id)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (thermostat == null)
            {
                return null;
            }

            Context.Thermostats.Remove(thermostat);
            await Context.SaveChangesAsync();
            return thermostat;
        }
    }
}