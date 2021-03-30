using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("users/{id_user}/houses/{id_house}/rooms/{id_room}/lightbulbs")]
    public class LightBulbController
    {
        private readonly HomeAssistantContext _context;

        public LightBulbController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<LightBulb>> Get(int id_user, int id_house, int id_room)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms
                    .Where(h => h.HouseId == house.Id).FirstOrDefault(r => r.Id == id_room);
                List<LightBulb> lightBulb = _context.LightBulbs.Where(lb => lb.RoomId == room.Id).ToList();
                return lightBulb;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<LightBulb> Get(int id_user, int id_house, int id_room, int id)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms
                    .Where(h => h.HouseId == house.Id).FirstOrDefault(r => r.Id == id_room);
                LightBulb lightBulb = _context.LightBulbs
                    .Where(lb => lb.RoomId == room.Id).FirstOrDefault(lb => lb.Id == id);
                return lightBulb;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<LightBulb> Post(int id_user, int id_house, int id_room, [FromBody] LightBulb lightBulb)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms
                    .Where(h => h.HouseId == house.Id).FirstOrDefault(r => r.Id == id_room);
                lightBulb.RoomId = room.Id;
                LightBulb newLightBulb = _context.LightBulbs.Add(lightBulb).Entity;
                await _context.SaveChangesAsync();
                return newLightBulb;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        [HttpDelete("{id}")]
        public async Task<HttpResponseMessage> Delete(int id_user, int id_house, int id_room, int id)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms
                    .Where(h => h.HouseId == house.Id).FirstOrDefault(r => r.Id == id_room);
                LightBulb lightBulb = _context.LightBulbs
                    .Where(lb => lb.RoomId == room.Id).FirstOrDefault(lb => lb.Id == id);

                _context.LightBulbs.Remove(lightBulb);
                _context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}