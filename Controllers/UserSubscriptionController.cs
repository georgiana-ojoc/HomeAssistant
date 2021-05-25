using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using HomeAssistantAPI.Models;
using HomeAssistantAPI.Queries.UserSubscription;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeAssistantAPI.Controllers
{
    [ApiController]
    [Route("user_subscription")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UserSubscriptionController : BaseController
    {
        public UserSubscriptionController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<UserSubscription>> GetAsync()
        {
            try
            {
                UserSubscription userSubscription = await Mediator.Send(new GetUserSubscriptionQuery());
                if (userSubscription == null)
                {
                    return NotFound();
                }

                return Ok(userSubscription);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}