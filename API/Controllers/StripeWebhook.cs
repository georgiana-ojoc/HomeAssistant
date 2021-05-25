using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Models;
using Stripe;


namespace API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("webhook")]
    [ApiController]
    [AllowAnonymous]
    public class WebhookController : Controller
    {
        private readonly HomeAssistantContext _context;

        public WebhookController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                {
                    if (stripeEvent.Type == Events.PaymentIntentSucceeded &&
                        stripeEvent.Data.Object is PaymentIntent paymentIntent)
                    {
                        Console.WriteLine("A successful payment for {0} was made from {1} with the amount {2}.",
                            paymentIntent.Amount,
                            paymentIntent.Id, paymentIntent.Amount);
                        return await HandlePaymentIntentSucceeded(paymentIntent);
                    }
                }
                if (stripeEvent.Type == Events.PaymentIntentCreated)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine("A payment intent was created from id: {0}.", paymentIntent?.Id);
                    return await HandlePaymentIntentSucceeded(paymentIntent);
                }
                else
                {
                    Console.WriteLine("An event of type {0} has been created.", stripeEvent.Type);
                    if (stripeEvent.Data.Object is PaymentIntent paymentIntent)
                    {
                        Console.WriteLine("Something was made from {0}.", paymentIntent.Id);
                    }
                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> HandlePaymentIntentSucceeded(PaymentIntent paymentIntent)
        {
            if (paymentIntent.Metadata.Count != 0)
            {
                String email = paymentIntent.Metadata["email"];
                if (email != null)
                {
                    Console.WriteLine("The email is {0}.", paymentIntent.Metadata["email"]);
                    Guid id = new Guid(paymentIntent.Metadata["id"]);
                    CheckoutOffer checkoutOffer = await _context.CheckoutOffer.FindAsync(id);
                    if (checkoutOffer != null)
                    {
                        if (checkoutOffer.OfferValue != paymentIntent.Amount)
                        {
                            return BadRequest();
                        }
                        UserCheckoutOffer userCheckoutOffer = _context.UserCheckoutOffer
                            .FirstOrDefault(u => u.Email.Equals(email)) ?? new UserCheckoutOffer
                        {
                            Email = email
                        };

                        userCheckoutOffer.CheckoutOffersId = id;
                        _context.UserCheckoutOffer.Update(userCheckoutOffer);
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                }
            }

            return BadRequest();
        }
    }
}