using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.DoorCommand;
using API.Queries.DoorCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Requests;

namespace API.Controllers
{
    [ApiController]
    [Route("schedules/{schedule_id:guid}/door_commands")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DoorCommandController : BaseController
    {
        public DoorCommandController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoorCommand>>> GetAsync(Guid schedule_id)
        {
            try
            {
                IEnumerable<DoorCommand> doorCommands =
                    await Mediator.Send(new GetDoorCommandsQuery {ScheduleId = schedule_id});
                if (doorCommands == null)
                {
                    return NotFound();
                }

                return Ok(doorCommands);
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
        public async Task<ActionResult<DoorCommand>> GetAsync(Guid schedule_id, Guid id)
        {
            try
            {
                DoorCommand doorCommand = await Mediator.Send(new GetDoorCommandByIdQuery
                    {ScheduleId = schedule_id, Id = id});
                if (doorCommand == null)
                {
                    return NotFound();
                }

                return Ok(doorCommand);
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
        public async Task<ActionResult<DoorCommand>> PostAsync(Guid schedule_id, [FromBody] DoorCommandRequest request)
        {
            try
            {
                DoorCommand newDoorCommand = await Mediator.Send(new CreateDoorCommandCommand
                    {ScheduleId = schedule_id, Request = request});
                if (newDoorCommand == null)
                {
                    return NotFound();
                }

                return Created($"schedules/{schedule_id}/door_commands/{newDoorCommand.Id}", newDoorCommand);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (ConstraintException)
            {
                return Forbid();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<DoorCommand>> PatchAsync(Guid schedule_id, Guid id,
            [FromBody] JsonPatchDocument<DoorCommandRequest> patch)
        {
            try
            {
                DoorCommand doorCommand = await Mediator.Send(new PartialUpdateDoorCommandCommand
                {
                    ScheduleId = schedule_id,
                    Id = id,
                    Patch = patch
                });
                if (doorCommand == null)
                {
                    return NotFound();
                }

                return Ok(doorCommand);
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
        public async Task<ActionResult> DeleteAsync(Guid schedule_id, Guid id)
        {
            try
            {
                DoorCommand doorCommand =
                    await Mediator.Send(new DeleteDoorCommandCommand {ScheduleId = schedule_id, Id = id});
                if (doorCommand == null)
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