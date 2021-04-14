using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HomeAssistantContext _context;

        public UserRepository(HomeAssistantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            User newUser = (await _context.Users.AddAsync(user)).Entity;
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<User> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            //_context?.Dispose();
        }
    }
}