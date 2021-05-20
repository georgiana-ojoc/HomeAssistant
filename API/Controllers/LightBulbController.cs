using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.LightBulb;
using API.Queries.LightBulb;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Requests;

namespace API.Controllers
{
    [ApiController]
    [Route("houses/{house_id:guid}/rooms/{room_id:guid}/light_bulbs")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LightBulbController : BaseController
    {
        public LightBulbController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LightBulb>>> GetAsync(Guid house_id, Guid room_id)
        {
            try
            {
                IEnumerable<LightBulb> lightBulbs = await Mediator.Send(new GetLightBulbsQuery
                {
                    HouseId = house_id,
                    RoomId = room_id
                });
                if (lightBulbs == null)
                {
                    return NotFound();
                }

                return Ok(lightBulbs);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LightBulb>> GetAsync(Guid house_id, Guid room_id, Guid id)
        {
            try
            {
                LightBulb lightBulb = await Mediator.Send(new GetLightBulbByIdQuery
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Id = id
                });
                if (lightBulb == null)
                {
                    return NotFound();
                }

                return Ok(lightBulb);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<LightBulb>> PostAsync(Guid house_id, Guid room_id,
            [FromBody] LightBulbRequest request)
        {
            try
            {
                LightBulb newLightBulb = await Mediator.Send(new CreateLightBulbCommand
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Request = request
                });
                if (newLightBulb == null)
                {
                    return NotFound();
                }

                return Created($"houses/{house_id}/rooms/{room_id}/light_bulbs/{newLightBulb.Id}", newLightBulb);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (ConstraintException)
            {
                return Forbid("You do not have any light bulbs left in this room.");
            }
            catch (DuplicateNameException)
            {
                return Conflict("You already have a light bulb with the specified name in this room.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<LightBulb>> PatchAsync(Guid house_id, Guid room_id, Guid id,
            [FromBody] JsonPatchDocument<LightBulbRequest> patch)
        {
            try
            {
                LightBulb lightBulb = await Mediator.Send(new PartialUpdateLightBulbCommand
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Id = id,
                    Patch = patch
                });
                if (lightBulb == null)
                {
                    return NotFound();
                }

                return Ok(lightBulb);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteAsync(Guid house_id, Guid room_id, Guid id)
        {
            try
            {
                LightBulb lightBulb = await Mediator.Send(new DeleteLightBulbCommand
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Id = id
                });
                if (lightBulb == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}