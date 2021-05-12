using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.LightBulbCommand;
using API.Queries.LightBulbCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Requests;

namespace API.Controllers
{
    [ApiController]
    [Route("schedules/{schedule_id:guid}/light_bulb_commands")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LightBulbCommandController : BaseController
    {
        public LightBulbCommandController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LightBulbCommand>>> GetAsync(Guid schedule_id)
        {
            try
            {
                IEnumerable<LightBulbCommand> LightBulbCommands =
                    await Mediator.Send(new GetLightBulbCommandsQuery {ScheduleId = schedule_id});
                if (LightBulbCommands == null)
                {
                    return NotFound();
                }

                return Ok(LightBulbCommands);
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
        public async Task<ActionResult<LightBulbCommand>> GetAsync(Guid schedule_id, Guid id)
        {
            try
            {
                LightBulbCommand LightBulbCommand = await Mediator.Send(new GetLightBulbCommandByIdQuery
                    {ScheduleId = schedule_id, Id = id});
                if (LightBulbCommand == null)
                {
                    return NotFound();
                }

                return Ok(LightBulbCommand);
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
        public async Task<ActionResult<LightBulbCommand>> PostAsync(Guid schedule_id,
            [FromBody] LightBulbCommandRequest request)
        {
            try
            {
                LightBulbCommand newLightBulbCommand = await Mediator.Send(new CreateLightBulbCommand
                    {ScheduleId = schedule_id, Request = request});
                if (newLightBulbCommand == null)
                {
                    return NotFound();
                }

                return Created($"schedules/{schedule_id}/light_bulb_commands/{newLightBulbCommand.Id}",
                    newLightBulbCommand);
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
        public async Task<ActionResult<LightBulbCommand>> PatchAsync(Guid schedule_id, Guid id,
            [FromBody] JsonPatchDocument<LightBulbCommandRequest> patch)
        {
            try
            {
                LightBulbCommand LightBulbCommand = await Mediator.Send(new PartialUpdateLightBulbCommand
                {
                    ScheduleId = schedule_id,
                    Id = id,
                    Patch = patch
                });
                if (LightBulbCommand == null)
                {
                    return NotFound();
                }

                return Ok(LightBulbCommand);
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
                LightBulbCommand LightBulbCommand =
                    await Mediator.Send(new DeleteLightBulbCommand {ScheduleId = schedule_id, Id = id});
                if (LightBulbCommand == null)
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