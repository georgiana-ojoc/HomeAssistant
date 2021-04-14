using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.LightBulb;
using API.Queries.LightBulb;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("houses/{house_id}/rooms/{room_id}/light_bulbs")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LightBulbController
    {
        private readonly Identity _identity;
        private readonly IMediator _mediator;

        public LightBulbController(Identity identity, IMediator mediator)
        {
            _identity = identity;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LightBulb>>> GetAsync(int house_id, int room_id)
        {
            IEnumerable<LightBulb> lightBulbs = await _mediator.Send(new LightBulbsQuery(_identity.Email,
                house_id, room_id));
            if (lightBulbs == null)
            {
                return new NotFoundResult();
            }

            return new ActionResult<IEnumerable<LightBulb>>(lightBulbs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LightBulb>> GetAsync(int house_id, int room_id, int id)
        {
            LightBulb lightBulb = await _mediator.Send(new LightBulbByIdQuery(_identity.Email, house_id,
                room_id, id));
            if (lightBulb == null)
            {
                return new NotFoundResult();
            }

            return lightBulb;
        }

        [HttpPost]
        public async Task<ActionResult<LightBulb>> PostAsync(int house_id, int room_id, [FromBody] LightBulb lightBulb)
        {
            try
            {
                LightBulb newLightBulb = await _mediator.Send(new AddLightBulbCommand(_identity.Email, house_id,
                    room_id, lightBulb));
                if (newLightBulb == null)
                {
                    return new NotFoundResult();
                }

                return new CreatedResult($"houses/{house_id}/rooms/{room_id}/light_bulbs", newLightBulb);
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int house_id, int room_id, int id)
        {
            try
            {
                LightBulb lightBulb = await _mediator.Send(new DeleteLightBulbCommand(_identity.Email, house_id,
                    room_id, id));
                if (lightBulb == null)
                {
                    return new NotFoundResult();
                }

                return new NoContentResult();
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}