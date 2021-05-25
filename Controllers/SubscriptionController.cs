using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using HomeAssistantAPI.Commands.Subscription;
using HomeAssistantAPI.Models;
using HomeAssistantAPI.Queries.Subscription;
using HomeAssistantAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HomeAssistantAPI.Controllers
{
    [ApiController]
    [Route("subscriptions")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SubscriptionController : BaseController
    {
        public SubscriptionController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetAsync()
        {
            try
            {
                return Ok(await Mediator.Send(new GetSubscriptionsQuery()));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Subscription>> GetAsync(Guid id)
        {
            try
            {
                Subscription subscription = await Mediator.Send(new GetSubscriptionByIdQuery {Id = id});
                if (subscription == null)
                {
                    return NotFound();
                }

                return Ok(subscription);
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

        [HttpPost]
        public async Task<ActionResult<Subscription>> PostAsync([FromBody] SubscriptionRequest request)
        {
            try
            {
                Subscription newSubscription = await Mediator.Send(new CreateSubscriptionCommand
                {
                    Request = request
                });
                return Created($"/subscriptions/{newSubscription.Id}", newSubscription);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (DuplicateNameException exception)
            {
                return Conflict(exception.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<Subscription>> PatchAsync(Guid id,
            [FromBody] JsonPatchDocument<SubscriptionRequest> patch)
        {
            try
            {
                Subscription subscription = await Mediator.Send(new PartialUpdateSubscriptionCommand
                {
                    Id = id,
                    Patch = patch
                });
                if (subscription == null)
                {
                    return NotFound();
                }

                return Ok(subscription);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                Subscription subscription = await Mediator.Send(new DeleteSubscriptionCommand {Id = id});
                if (subscription == null)
                {
                    return NotFound();
                }

                return NoContent();
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