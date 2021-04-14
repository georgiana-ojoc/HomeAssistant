using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using API.Commands.LightBulb;
using API.Models;
using API.Queries.LightBulb;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("users/{user_id}/houses/{house_id}/rooms/{room_id}/light_bulbs")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LightBulbController
    {
        private readonly IMediator _mediator;

        public LightBulbController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LightBulb>>> Get(int user_id, int house_id, int room_id)
        {
            IEnumerable<LightBulb> lightBulbs = await _mediator.Send(new LightBulbsQuery(user_id,
                house_id, room_id));
            if (lightBulbs == null)
            {
                return new NotFoundResult();
            }

            return new ActionResult<IEnumerable<LightBulb>>(lightBulbs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LightBulb>> Get(int user_id, int house_id, int room_id, int id)
        {
            LightBulb lightBulb = await _mediator.Send(new LightBulbById(user_id, house_id,
                room_id, id));
            if (lightBulb == null)
            {
                return new NotFoundResult();
            }
            return lightBulb;
        }

        [HttpPost]
        public async Task<ActionResult<LightBulb>> Post(int user_id, int house_id, int room_id,
            [FromBody] LightBulb lightBulb)
        {
            try
            {
                LightBulb newLightBulb = await _mediator.Send(new AddLightBulb(user_id, house_id
                    , room_id, lightBulb));
                if (newLightBulb == null)
                {
                    return new NotFoundResult();
                }
                return newLightBulb;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int user_id, int house_id, int room_id, int id)
        {
            try
            {
                LightBulb lightBulb = await _mediator.Send(new DeleteLightBulb(user_id,
                    house_id, room_id, id));
                if (lightBulb == null)
                {
                    return new NotFoundResult();
                }
                return new NoContentResult();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}