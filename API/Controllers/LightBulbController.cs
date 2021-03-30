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
    [Route("users/{user_id}/houses/{house_id}/rooms/{room_id}/light_bulbs")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LightBulbController
    {
        private readonly HomeAssistantContext _context;

        public LightBulbController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LightBulb>>> Get(int user_id, int house_id, int room_id)
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

            return await _context.LightBulbs.Where(lb => lb.RoomId == room.Id).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LightBulb>> Get(int user_id, int house_id, int room_id, int id)
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

            LightBulb lightBulb = await _context.LightBulbs.Where(lb => lb.RoomId == room.Id)
                .FirstOrDefaultAsync(lb => lb.Id == id);
            if (lightBulb == null)
            {
                return new NotFoundResult();
            }

            return lightBulb;
        }

        [HttpPost]
        public async Task<ActionResult<LightBulb>> Post(int user_id, int house_id, int room_id,
            [FromBody] LightBulb lightBulb)
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

                lightBulb.RoomId = room.Id;
                LightBulb newLightBulb = (await _context.LightBulbs.AddAsync(lightBulb)).Entity;
                await _context.SaveChangesAsync();
                return newLightBulb;
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

                LightBulb lightBulb = await _context.LightBulbs.Where(lb => lb.RoomId == room.Id)
                    .FirstOrDefaultAsync(lb => lb.Id == id);
                if (lightBulb == null)
                {
                    return new NotFoundResult();
                }

                _context.LightBulbs.Remove(lightBulb);
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