using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Models;
using Stripe;


//To run on backend install stripe cli and run 'stripe listen --forward-to localhost:5000/webhook'
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
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine("A successful payment for {0} was made from {1} with the amount {2}.",
                        paymentIntent.Amount,
                        paymentIntent.Id, paymentIntent.Amount);
                    return await handlePaymentIntentSucceeded(paymentIntent);
                }
                else if (stripeEvent.Type == Events.PaymentIntentCreated)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine("A payment intent was created from id: {0}.", paymentIntent.Id);
                    // Uncomment this for testing
                    return await handlePaymentIntentSucceeded(paymentIntent);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("An event of type {0} has been created.", stripeEvent.Type);
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine("Something was made from {0}.", paymentIntent.Id);
                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> handlePaymentIntentSucceeded(PaymentIntent paymentIntent)
        {
            if (paymentIntent.Metadata.Count != 0)
            {
                String email = paymentIntent.Metadata["email"];
                if (email != null)
                {
                    Console.WriteLine("The email is {0}.", paymentIntent.Metadata["email"]);
                    Guid id = new Guid(paymentIntent.Metadata["id"]);
                    CheckoutOffer checkoutOffer = _context.CheckoutOffer.Find(id);
                    if (checkoutOffer != null)
                    {
                        if (checkoutOffer.OfferValue != paymentIntent.Amount)
                            return BadRequest();
                        UserCheckoutOffer userCheckoutOffer = _context.UserCheckoutOffer
                            .FirstOrDefault(u => u.Email.Equals(email));
                        if (userCheckoutOffer == null)
                        {
                            userCheckoutOffer = new UserCheckoutOffer();
                            userCheckoutOffer.Email = email;
                        }

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