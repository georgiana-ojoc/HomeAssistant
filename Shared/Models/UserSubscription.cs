﻿using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Shared.Models
{
    public class UserSubscription
    {
        [Required] public string Email { get; set; }
        [Required] public Guid SubscriptionId { get; set; }

        internal Subscription Subscription { get; set; }
    }
}