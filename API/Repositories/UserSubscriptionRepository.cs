using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
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