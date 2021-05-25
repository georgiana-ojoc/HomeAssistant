using System.Threading.Tasks;
using AutoMapper;
using HomeAssistantAPI.Interfaces;
using HomeAssistantAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeAssistantAPI.Repositories
{
    public class UserSubscriptionRepository : BaseRepository, IUserSubscriptionRepository
    {
        public UserSubscriptionRepository(HomeAssistantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<UserSubscription> GetUserSubscriptionAsync(string email)
        {
            CheckString(email, "email");

            return await Context.UserSubscriptions.FirstOrDefaultAsync(us => us.Email == email);
        }
    }
}