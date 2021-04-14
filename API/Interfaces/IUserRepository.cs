using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models;

namespace API.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User> DeleteUserAsync(int id);
        Task<User> UpdateUserAsync(User user);
    }
}