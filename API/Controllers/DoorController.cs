using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.Door;
using API.Queries.Door;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Requests;

namespace API.Controllers
{
    [ApiController]
    [Route("houses/{house_id:guid}/rooms/{room_id:guid}/doors")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DoorController : BaseController
    {
        public DoorController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Door>>> GetAsync(Guid house_id, Guid room_id)
        {
            try
            {
                IEnumerable<Door> doors = await Mediator.Send(new GetDoorsQuery
                {
                    HouseId = house_id,
                    RoomId = room_id
                });
                if (doors == null)
                {
                    return NotFound();
                }

                return Ok(doors);
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
        public async Task<ActionResult<Door>> GetAsync(Guid house_id, Guid room_id, Guid id)
        {
            try
            {
                Door door = await Mediator.Send(new GetDoorByIdQuery
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Id = id
                });
                if (door == null)
                {
                    return NotFound();
                }

                return Ok(door);
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
        public async Task<ActionResult<Door>> PatchAsync(Guid house_id, Guid room_id, [FromBody] DoorRequest request)
        {
            try
            {
                Door newDoor = await Mediator.Send(new CreateDoorCommand
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Request = request
                });
                if (newDoor == null)
                {
                    return NotFound();
                }

                return Created($"houses/{house_id}/rooms/{room_id}/doors/{newDoor.Id}", newDoor);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (ConstraintException)
            {
                return Forbid("You do not have any doors left in this room.");
            }
            catch (DuplicateNameException)
            {
                return Conflict("You already have a door with the specified name in this room.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<Door>> UpdateAsync(Guid house_id, Guid room_id, Guid id,
            [FromBody] JsonPatchDocument<DoorRequest> patch)
        {
            try
            {
                Door door = await Mediator.Send(new PartialUpdateDoorCommand
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Id = id,
                    Patch = patch
                });
                if (door == null)
                {
                    return NotFound();
                }

                return Ok(door);
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
                Door door = await Mediator.Send(new DeleteDoorCommand
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Id = id
                });
                if (door == null)
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