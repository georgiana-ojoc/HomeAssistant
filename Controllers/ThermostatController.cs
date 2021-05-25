using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using HomeAssistantAPI.Commands.Thermostat;
using HomeAssistantAPI.Models;
using HomeAssistantAPI.Queries.Thermostat;
using HomeAssistantAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HomeAssistantAPI.Controllers
{
    [ApiController]
    [Route("houses/{house_id:guid}/rooms/{room_id:guid}/thermostats")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ThermostatController : BaseController
    {
        public ThermostatController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Thermostat>>> GetAsync(Guid house_id, Guid room_id)
        {
            try
            {
                IEnumerable<Thermostat> thermostats = await Mediator.Send(new GetThermostatsQuery
                {
                    HouseId = house_id,
                    RoomId = room_id
                });
                if (thermostats == null)
                {
                    return NotFound();
                }

                return Ok(thermostats);
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
        public async Task<ActionResult<Thermostat>> GetAsync(Guid house_id, Guid room_id, Guid id)
        {
            try
            {
                Thermostat thermostat = await Mediator.Send(new GetThermostatByIdQuery
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Id = id
                });
                if (thermostat == null)
                {
                    return NotFound();
                }

                return Ok(thermostat);
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
        public async Task<ActionResult<Thermostat>> PostAsync(Guid house_id, Guid room_id,
            [FromBody] ThermostatRequest request)
        {
            try
            {
                Thermostat newThermostat = await Mediator.Send(new CreateThermostatCommand
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Request = request
                });
                if (newThermostat == null)
                {
                    return NotFound();
                }

                return Created($"houses/{house_id}/rooms/{room_id}/thermostats/{newThermostat.Id}", newThermostat);
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
        public async Task<ActionResult<Thermostat>> PatchAsync(Guid house_id, Guid room_id, Guid id,
            [FromBody] JsonPatchDocument<ThermostatRequest> patch)
        {
            try
            {
                Thermostat thermostat = await Mediator.Send(new PartialUpdateThermostatCommand
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Id = id,
                    Patch = patch
                });
                if (thermostat == null)
                {
                    return NotFound();
                }

                return Ok(thermostat);
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
        public async Task<ActionResult> DeleteAsync(Guid house_id, Guid room_id, Guid id)
        {
            try
            {
                Thermostat thermostat = await Mediator.Send(new DeleteThermostatCommand
                {
                    HouseId = house_id,
                    RoomId = room_id,
                    Id = id
                });
                if (thermostat == null)
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