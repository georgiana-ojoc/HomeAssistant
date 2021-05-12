using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.ThermostatCommand;
using API.Queries.ThermostatCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Requests;

namespace API.Controllers
{
    [ApiController]
    [Route("schedules/{schedule_id:guid}/thermostat_commands")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ThermostatCommandController : BaseController
    {
        public ThermostatCommandController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThermostatCommand>>> GetAsync(Guid schedule_id)
        {
            try
            {
                IEnumerable<ThermostatCommand> ThermostatCommands =
                    await Mediator.Send(new GetThermostatCommandsQuery {ScheduleId = schedule_id});
                if (ThermostatCommands == null)
                {
                    return NotFound();
                }

                return Ok(ThermostatCommands);
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
        public async Task<ActionResult<ThermostatCommand>> GetAsync(Guid schedule_id, Guid id)
        {
            try
            {
                ThermostatCommand ThermostatCommand = await Mediator.Send(new GetThermostatCommandByIdQuery
                    {ScheduleId = schedule_id, Id = id});
                if (ThermostatCommand == null)
                {
                    return NotFound();
                }

                return Ok(ThermostatCommand);
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
        public async Task<ActionResult<ThermostatCommand>> PostAsync(Guid schedule_id,
            [FromBody] ThermostatCommandRequest request)
        {
            try
            {
                ThermostatCommand newThermostatCommand = await Mediator.Send(new CreateThermostatCommand
                    {ScheduleId = schedule_id, Request = request});
                if (newThermostatCommand == null)
                {
                    return NotFound();
                }

                return Created($"schedules/{schedule_id}/thermostat_commands/{newThermostatCommand.Id}",
                    newThermostatCommand);
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
        public async Task<ActionResult<ThermostatCommand>> PatchAsync(Guid schedule_id, Guid id,
            [FromBody] JsonPatchDocument<ThermostatCommandRequest> patch)
        {
            try
            {
                ThermostatCommand ThermostatCommand = await Mediator.Send(new PartialUpdateThermostatCommand
                {
                    ScheduleId = schedule_id,
                    Id = id,
                    Patch = patch
                });
                if (ThermostatCommand == null)
                {
                    return NotFound();
                }

                return Ok(ThermostatCommand);
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
                ThermostatCommand ThermostatCommand =
                    await Mediator.Send(new DeleteThermostatCommand {ScheduleId = schedule_id, Id = id});
                if (ThermostatCommand == null)
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