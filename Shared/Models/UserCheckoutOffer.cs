using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class UserCheckoutOffer : BaseModel
    {
        [Required] public String Email { get; set; }

        [Required] public Guid CheckoutOffersId { get; set; }

        internal CheckoutOffer CheckoutOffer { get; set; }
    }
}