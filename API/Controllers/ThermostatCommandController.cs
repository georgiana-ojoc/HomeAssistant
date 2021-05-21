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
using Shared.Responses;

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
        public async Task<ActionResult<IEnumerable<ThermostatCommandResponse>>> GetAsync(Guid schedule_id)
        {
            try
            {
                IEnumerable<ThermostatCommandResponse> thermostatCommands =
                    await Mediator.Send(new GetThermostatCommandsQuery {ScheduleId = schedule_id});
                if (thermostatCommands == null)
                {
                    return NotFound();
                }

                return Ok(thermostatCommands);
            }
            catch (ArgumentNullException exception)
            {
                return BadRequest(exception.Message);
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
                ThermostatCommand thermostatCommand = await Mediator.Send(new GetThermostatCommandByIdQuery
                    {ScheduleId = schedule_id, Id = id});
                if (thermostatCommand == null)
                {
                    return NotFound();
                }

                return Ok(thermostatCommand);
            }
            catch (ArgumentNullException exception)
            {
                return BadRequest(exception.Message);
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
                ThermostatCommand newThermostatCommand = await Mediator.Send(new CreateThermostatCommandCommand
                    {ScheduleId = schedule_id, Request = request});
                if (newThermostatCommand == null)
                {
                    return NotFound();
                }

                return Created($"schedules/{schedule_id}/thermostat_commands/{newThermostatCommand.Id}",
                    newThermostatCommand);
            }
            catch (ArgumentNullException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (ConstraintException exception)
            {
                return Forbid(exception.Message);
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
        public async Task<ActionResult<ThermostatCommand>> PatchAsync(Guid schedule_id, Guid id,
            [FromBody] JsonPatchDocument<ThermostatCommandRequest> patch)
        {
            try
            {
                ThermostatCommand thermostatCommand = await Mediator.Send(new PartialUpdateThermostatCommandCommand
                {
                    ScheduleId = schedule_id,
                    Id = id,
                    Patch = patch
                });
                if (thermostatCommand == null)
                {
                    return NotFound();
                }

                return Ok(thermostatCommand);
            }
            catch (ArgumentNullException exception)
            {
                return BadRequest(exception.Message);
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
                ThermostatCommand thermostatCommand =
                    await Mediator.Send(new DeleteThermostatCommandCommand {ScheduleId = schedule_id, Id = id});
                if (thermostatCommand == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ArgumentNullException exception)
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