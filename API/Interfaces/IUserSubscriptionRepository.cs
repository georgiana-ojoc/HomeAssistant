using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IUserSubscriptionRepository
    {
        Task<UserSubscription> GetUserSubscriptionAsync(string email);
    }
}