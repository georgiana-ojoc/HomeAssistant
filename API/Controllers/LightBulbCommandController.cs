using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.LightBulbCommand;
using API.Models;
using API.Queries.LightBulbCommand;
using API.Requests;
using API.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<LightBulbCommandResponse>>> GetAsync(Guid schedule_id)
        {
            try
            {
                IEnumerable<LightBulbCommandResponse> lightBulbCommands =
                    await Mediator.Send(new GetLightBulbCommandsQuery {ScheduleId = schedule_id});
                if (lightBulbCommands == null)
                {
                    return NotFound();
                }

                return Ok(lightBulbCommands);
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
        public async Task<ActionResult<LightBulbCommandResponse>> GetAsync(Guid schedule_id, Guid id)
        {
            try
            {
                LightBulbCommandResponse lightBulbCommand = await Mediator.Send(new GetLightBulbCommandByIdQuery
                    {ScheduleId = schedule_id, Id = id});
                if (lightBulbCommand == null)
                {
                    return NotFound();
                }

                return Ok(lightBulbCommand);
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
        public async Task<ActionResult<LightBulbCommand>> PostAsync(Guid schedule_id,
            [FromBody] LightBulbCommandRequest request)
        {
            try
            {
                LightBulbCommand newLightBulbCommand = await Mediator.Send(new CreateLightBulbCommandCommand
                    {ScheduleId = schedule_id, Request = request});
                if (newLightBulbCommand == null)
                {
                    return NotFound();
                }

                return Created($"schedules/{schedule_id}/light_bulb_commands/{newLightBulbCommand.Id}",
                    newLightBulbCommand);
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
        public async Task<ActionResult<LightBulbCommand>> PatchAsync(Guid schedule_id, Guid id,
            [FromBody] JsonPatchDocument<LightBulbCommandRequest> patch)
        {
            try
            {
                LightBulbCommand lightBulbCommand = await Mediator.Send(new PartialUpdateLightBulbCommandCommand
                {
                    ScheduleId = schedule_id,
                    Id = id,
                    Patch = patch
                });
                if (lightBulbCommand == null)
                {
                    return NotFound();
                }

                return Ok(lightBulbCommand);
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
        public async Task<ActionResult> DeleteAsync(Guid schedule_id, Guid id)
        {
            try
            {
                LightBulbCommand lightBulbCommand =
                    await Mediator.Send(new DeleteLightBulbCommandCommand {ScheduleId = schedule_id, Id = id});
                if (lightBulbCommand == null)
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