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
    [Route("users/{user_id}/houses/{house_id}/rooms")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RoomController
    {
        private readonly HomeAssistantContext _context;

        public RoomController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> Get(int user_id, int house_id)
        {
            House house = await _context.Houses.Where(h => h.UserId == user_id)
                .FirstOrDefaultAsync(h => h.Id == house_id);
            if (house == null)
            {
                return new NotFoundResult();
            }

            return await _context.Rooms.Where(r => r.HouseId == house.Id).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> Get(int user_id, int house_id, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == user_id)
                .FirstOrDefaultAsync(h => h.Id == house_id);
            if (house == null)
            {
                return new NotFoundResult();
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return new NotFoundResult();
            }

            return room;
        }

        [HttpPost]
        public async Task<ActionResult<Room>> Post(int user_id, int house_id, [FromBody] Room room)
        {
            try
            {
                House house = await _context.Houses.Where(h => h.UserId == user_id)
                    .FirstOrDefaultAsync(h => h.Id == house_id);
                if (house == null)
                {
                    return new NotFoundResult();
                }

                room.HouseId = house.Id;
                Room newRoom = (await _context.Rooms.AddAsync(room)).Entity;
                await _context.SaveChangesAsync();
                return newRoom;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int user_id, int house_id, int id)
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
                    .FirstOrDefaultAsync(r => r.Id == id);
                if (room == null)
                {
                    return new NotFoundResult();
                }

                _context.Rooms.Remove(room);
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