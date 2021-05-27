using System;

#nullable disable

namespace Client.Models
{
    public class UserSubscription
    {
        public string Email { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}