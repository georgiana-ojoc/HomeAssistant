using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using HomeAssistantAPI.Commands.Room;
using HomeAssistantAPI.Models;
using HomeAssistantAPI.Queries.Room;
using HomeAssistantAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HomeAssistantAPI.Controllers
{
    [ApiController]
    [Route("houses/{house_id:guid}/rooms")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RoomController : BaseController
    {
        public RoomController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAsync(Guid house_id)
        {
            try
            {
                IEnumerable<Room> rooms = await Mediator.Send(new GetRoomsQuery {HouseId = house_id});
                if (rooms == null)
                {
                    return NotFound();
                }

                return Ok(rooms);
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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Room>> GetAsync(Guid house_id, Guid id)
        {
            try
            {
                Room room = await Mediator.Send(new GetRoomByIdQuery {HouseId = house_id, Id = id});
                if (room == null)
                {
                    return NotFound();
                }

                return Ok(room);
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
        public async Task<ActionResult<Room>> PostAsync(Guid house_id, [FromBody] RoomRequest request)
        {
            try
            {
                Room newRoom = await Mediator.Send(new CreateRoomCommand {HouseId = house_id, Request = request});
                if (newRoom == null)
                {
                    return NotFound();
                }

                return Created($"houses/{house_id}/rooms/{newRoom.Id}", newRoom);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ConstraintException exception)
            {
                return StatusCode(StatusCodes.Status402PaymentRequired, exception.Message);
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
        public async Task<ActionResult<Room>> PatchAsync(Guid house_id, Guid id,
            [FromBody] JsonPatchDocument<RoomRequest> patch)
        {
            try
            {
                Room room = await Mediator.Send(new PartialUpdateRoomCommand
                {
                    HouseId = house_id,
                    Id = id,
                    Patch = patch
                });
                if (room == null)
                {
                    return NotFound();
                }

                return Ok(room);
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

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteAsync(Guid house_id, Guid id)
        {
            try
            {
                Room room = await Mediator.Send(new DeleteRoomCommand {HouseId = house_id, Id = id});
                if (room == null)
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