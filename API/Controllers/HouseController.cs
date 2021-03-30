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
    [Route("users/{user_id}/houses")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class HouseController
    {
        private readonly HomeAssistantContext _context;

        public HouseController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<House>>> Get(int user_id)
        {
            User user = await _context.Users.FindAsync(user_id);
            if (user == null)
            {
                return new NotFoundResult();
            }

            return await _context.Houses.Where(h => h.UserId == user.Id).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<House>> Get(int user_id, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == user_id)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (house == null)
            {
                return new NotFoundResult();
            }

            return house;
        }

        [HttpPost]
        public async Task<ActionResult<House>> Post(int user_id, [FromBody] House house)
        {
            try
            {
                User user = await _context.Users.FindAsync(user_id);
                if (user == null)
                {
                    return new NotFoundResult();
                }

                house.UserId = user.Id;
                House newHouse = (await _context.Houses.AddAsync(house)).Entity;
                await _context.SaveChangesAsync();
                return newHouse;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int user_id, int id)
        {
            try
            {
                House house = await _context.Houses.Where(h => h.UserId == user_id)
                    .FirstOrDefaultAsync(h => h.Id == id);
                if (house == null)
                {
                    return new NotFoundResult();
                }

                _context.Houses.Remove(house);
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