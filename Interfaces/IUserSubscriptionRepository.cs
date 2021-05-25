using System.Threading.Tasks;
using HomeAssistantAPI.Models;

namespace HomeAssistantAPI.Interfaces
{
    public interface IUserSubscriptionRepository
    {
        Task<UserSubscription> GetUserSubscriptionAsync(string email);
    }
}