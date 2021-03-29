using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DeviceManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace DeviceManager.Controllers
{
    [ApiController]
    [Route("users/{id_user}/houses/{id_house}/rooms/{id_room}/thermostats")]
    public class ThermostatController
    {
        [HttpGet]
        public async Task<List<Thermostat>> Get(int id_user, int id_house, int id_room)
        {
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                House house = homeAssistantContext.Houses.Where(h => h.UserId == id_user).Where(h => h.Id == id_house)
                    .FirstOrDefault();
                Room room = homeAssistantContext.Rooms.Where(h => h.HouseId == house.Id)
                    .Where(r => r.Id == id_room).FirstOrDefault();
                List<Thermostat> thermostat =
                    homeAssistantContext.Thermostats.Where(lb => lb.RoomId == room.Id).ToList();
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
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                House house = homeAssistantContext.Houses.Where(h => h.UserId == id_user).Where(h => h.Id == id_house)
                    .FirstOrDefault();
                Room room = homeAssistantContext.Rooms.Where(h => h.HouseId == house.Id)
                    .Where(r => r.Id == id_room).FirstOrDefault();
                Thermostat thermostat = homeAssistantContext.Thermostats.Where(lb => lb.RoomId == room.Id)
                    .Where(lb => lb.Id == id).FirstOrDefault();
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
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                House house = homeAssistantContext.Houses.Where(h => h.UserId == id_user).Where(h => h.Id == id_house)
                    .FirstOrDefault();
                Room room = homeAssistantContext.Rooms.Where(h => h.HouseId == house.Id)
                    .Where(r => r.Id == id_room).FirstOrDefault();
                thermostat.RoomId = room.Id;
                Thermostat _thermostat_ = homeAssistantContext.Thermostats.Add(thermostat).Entity;
                await homeAssistantContext.SaveChangesAsync();
                return _thermostat_;
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
            HomeAssistantContext homeAssistantContext = new HomeAssistantContext();
            try
            {
                House house = homeAssistantContext.Houses.Where(h => h.UserId == id_user).Where(h => h.Id == id_house)
                    .FirstOrDefault();
                Room room = homeAssistantContext.Rooms.Where(h => h.HouseId == house.Id)
                    .Where(r => r.Id == id_room).FirstOrDefault();
                Thermostat thermostat = homeAssistantContext.Thermostats.Where(lb => lb.RoomId == room.Id)
                    .Where(lb => lb.Id == id).FirstOrDefault();

                homeAssistantContext.Thermostats.Remove(thermostat);
                homeAssistantContext.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}