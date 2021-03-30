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
    [Route("users/{id_user}/houses")]
    public class HouseController
    {
        private readonly HomeAssistantContext _context;

        public HouseController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<House>> Get(int id_user)
        {
            try
            {
                User user = _context.Users.Find(id_user);
                List<House> houses = _context.Houses.Where(h => h.UserId == user.Id).ToList();
                return houses;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<House> Get(int id_user, int id)
        {
            try
            {
                User user = _context.Users.Find(id_user);
                House house = _context.Houses.Where(h => h.UserId == user.Id)
                    .FirstOrDefault(h => h.Id == id);
                return house;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<House> Post(int id_user, [FromBody] House house)
        {
            try
            {
                house.UserId = id_user;
                House newHouse = _context.Houses.Add(house).Entity;
                await _context.SaveChangesAsync();
                return newHouse;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        [HttpDelete("{id}")]
        public async Task<HttpResponseMessage> Delete(int id_user, int id)
        {
            try
            {
                User user = _context.Users.Find(id_user);
                House house = _context.Houses.Where(h => h.UserId == user.Id)
                    .FirstOrDefault(h => h.Id == id);
                _context.Houses.Remove(house);
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