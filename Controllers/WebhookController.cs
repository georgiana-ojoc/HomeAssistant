using System;
using System.IO;
using System.Threading.Tasks;
using HomeAssistantAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace HomeAssistantAPI.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("webhook")]
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
            String body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                Event stripeEvent = EventUtility.ParseEvent(body);

                if (stripeEvent.Type is Events.PaymentIntentCreated or Events.PaymentIntentSucceeded)
                {
                    return await HandlePaymentIntentSucceeded(stripeEvent.Data.Object as PaymentIntent);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return BadRequest();
        }

        public async Task<IActionResult> HandlePaymentIntentSucceeded(PaymentIntent paymentIntent)
        {
            try
            {
                if (paymentIntent.Metadata.Count == 0)
                {
                    return BadRequest();
                }

                String email = paymentIntent.Metadata["email"];
                if (email == null)
                {
                    return BadRequest();
                }

                String stringId = paymentIntent.Metadata["id"];
                if (stringId == null)
                {
                    return BadRequest();
                }

                Guid id = new Guid(stringId);
                Models.Subscription subscription = await _context.Subscriptions
                    .FirstOrDefaultAsync(s => s.Id == id);
                if (subscription == null)
                {
                    return BadRequest();
                }

                if (subscription.Price != paymentIntent.Amount)
                {
                    return BadRequest();
                }

                UserSubscription userSubscription = await _context.UserSubscriptions
                    .FirstOrDefaultAsync(us => us.Email == email);
                if (userSubscription == null)
                {
                    userSubscription = new UserSubscription
                    {
                        Email = email,
                        SubscriptionId = id
                    };
                    await _context.UserSubscriptions.AddAsync(userSubscription);
                }
                else
                {
                    userSubscription.SubscriptionId = id;
                    _context.UserSubscriptions.Update(userSubscription);
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}