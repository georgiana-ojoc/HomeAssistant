using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController
    {
        private readonly HomeAssistantContext _context;

        public UserController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        [HttpPost]
        public async Task<User> Post([FromBody] User user)
        {
            try
            {
                User user1 = _context.Users.Add(user).Entity;
                await _context.SaveChangesAsync();
                return user1;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        [HttpDelete("{id}")]
        public async Task<HttpResponseMessage> Delete(int id)
        {
            try
            {
                User user = _context.Users.Find(id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}