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
    [Route("users/{id_user}/houses/{id_house}/rooms/{id_room}/thermostats")]
    public class ThermostatController
    {
        private readonly HomeAssistantContext _context;

        public ThermostatController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Thermostat>> Get(int id_user, int id_house, int id_room)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms
                    .Where(h => h.HouseId == house.Id).FirstOrDefault(r => r.Id == id_room);
                List<Thermostat> thermostat =
                    _context.Thermostats.Where(lb => lb.RoomId == room.Id).ToList();
                return thermostat;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<Thermostat> Get(int id_user, int id_house, int id_room, int id)
        {
            
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms
                    .Where(h => h.HouseId == house.Id).FirstOrDefault(r => r.Id == id_room);
                Thermostat thermostat = _context.Thermostats
                    .Where(lb => lb.RoomId == room.Id).FirstOrDefault(lb => lb.Id == id);
                return thermostat;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<Thermostat> Post(int id_user, int id_house, int id_room, [FromBody] Thermostat thermostat)
        {
            try
            {
                House house = _context.Houses.Where(h => h.UserId == id_user)
                    .FirstOrDefault(h => h.Id == id_house);
                Room room = _context.Rooms
                    .Where(h => h.HouseId == house.Id).FirstOrDefault(r => r.Id == id_room);
                thermostat.RoomId = room.Id;
                Thermostat newThermostat = _context.Thermostats.Add(thermostat).Entity;
                await _context.SaveChangesAsync();
                return newThermostat;
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
                Thermostat thermostat = _context.Thermostats
                    .Where(lb => lb.RoomId == room.Id).FirstOrDefault(lb => lb.Id == id);

                _context.Thermostats.Remove(thermostat);
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