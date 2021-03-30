using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("users/{user_id}/houses/{house_id}/rooms/{room_id}/thermostats")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ThermostatController
    {
        private readonly HomeAssistantContext _context;

        public ThermostatController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Thermostat>>> Get(int user_id, int house_id, int room_id)
        {
            House house = await _context.Houses.Where(h => h.UserId == user_id)
                .FirstOrDefaultAsync(h => h.Id == house_id);
            if (house == null)
            {
                return new NotFoundResult();
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == room_id);
            if (room == null)
            {
                return new NotFoundResult();
            }

            return await _context.Thermostats.Where(t => t.RoomId == room.Id).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Thermostat>> Get(int user_id, int house_id, int room_id, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == user_id)
                .FirstOrDefaultAsync(h => h.Id == house_id);
            if (house == null)
            {
                return new NotFoundResult();
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == room_id);
            if (room == null)
            {
                return new NotFoundResult();
            }

            Thermostat thermostat = await _context.Thermostats.Where(t => t.RoomId == room.Id)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (thermostat == null)
            {
                return new NotFoundResult();
            }

            return thermostat;
        }

        [HttpPost]
        public async Task<ActionResult<Thermostat>> Post(int user_id, int house_id, int room_id,
            [FromBody] Thermostat thermostat)
        {
            try
            {
                House house = await _context.Houses.Where(h => h.UserId == user_id)
                    .FirstOrDefaultAsync(h => h.Id == house_id);
                if (house == null)
                {
                    return new NotFoundResult();
                }

                Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                    .FirstOrDefaultAsync(r => r.Id == room_id);
                if (room == null)
                {
                    return new NotFoundResult();
                }

                thermostat.RoomId = room.Id;
                Thermostat newThermostat = (await _context.Thermostats.AddAsync(thermostat)).Entity;
                await _context.SaveChangesAsync();
                return newThermostat;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int user_id, int house_id, int room_id, int id)
        {
            try
            {
                House house = await _context.Houses.Where(h => h.UserId == user_id)
                    .FirstOrDefaultAsync(h => h.Id == house_id);
                if (house == null)
                {
                    return new NotFoundResult();
                }

                Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                    .FirstOrDefaultAsync(r => r.Id == room_id);
                if (room == null)
                {
                    return new NotFoundResult();
                }

                Thermostat thermostat = await _context.Thermostats.Where(t => t.RoomId == room.Id)
                    .FirstOrDefaultAsync(t => t.Id == id);
                if (thermostat == null)
                {
                    return new NotFoundResult();
                }

                _context.Thermostats.Remove(thermostat);
                await _context.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}