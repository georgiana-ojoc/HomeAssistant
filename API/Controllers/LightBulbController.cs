using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.LightBulb;
using API.Queries.LightBulb;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Patch;

namespace API.Controllers
{
    [ApiController]
    [Route("houses/{house_id}/rooms/{room_id}/light_bulbs")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LightBulbController : BaseController
    {
        public LightBulbController(Identity identity, IMediator mediator) : base(identity, mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LightBulb>>> GetAsync(Guid house_id, Guid room_id)
        {
            IEnumerable<LightBulb> lightBulbs = await Mediator.Send(new LightBulbsQuery(Identity.Email,
                house_id, room_id));
            if (lightBulbs == null)
            {
                return new NotFoundResult();
            }

            return new ActionResult<IEnumerable<LightBulb>>(lightBulbs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LightBulb>> GetAsync(Guid house_id, Guid room_id, Guid id)
        {
            LightBulb lightBulb = await Mediator.Send(new LightBulbByIdQuery(Identity.Email, house_id,
                room_id, id));
            if (lightBulb == null)
            {
                return new NotFoundResult();
            }

            return lightBulb;
        }

        [HttpPost]
        public async Task<ActionResult<LightBulb>> PostAsync(Guid house_id, Guid room_id,
            [FromBody] LightBulb lightBulb)
        {
            try
            {
                LightBulb newLightBulb = await Mediator.Send(new AddLightBulbCommand(Identity.Email, house_id,
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

        [HttpPatch("{id}")]
        public async Task<ActionResult<LightBulb>> PatchAsync(Guid house_id, Guid room_id, Guid id,
            [FromBody] JsonPatchDocument<LightBulbPatch> patch)
        {
            try
            {
                LightBulb lightBulb = await Mediator.Send(new UpdateLightBulbCommand(Identity.Email, house_id,
                    room_id, id, patch));
                if (lightBulb == null)
                {
                    return new NotFoundResult();
                }

                return lightBulb;
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid house_id, Guid room_id, Guid id)
        {
            try
            {
                LightBulb lightBulb = await Mediator.Send(new DeleteLightBulbCommand(Identity.Email, house_id,
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