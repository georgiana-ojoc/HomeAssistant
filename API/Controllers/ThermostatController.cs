using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.Thermostat;
using API.Queries.Thermostat;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Patch;

namespace API.Controllers
{
    [ApiController]
    [Route("houses/{house_id}/rooms/{room_id}/thermostats")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ThermostatController
    {
        private readonly Identity _identity;
        private readonly IMediator _mediator;

        public ThermostatController(Identity identity, IMediator mediator)
        {
            _identity = identity;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Thermostat>>> GetAsync(Guid house_id, Guid room_id)
        {
            IEnumerable<Thermostat> thermostats = await _mediator.Send(new ThermostatsQuery(_identity.Email,
                house_id, room_id));
            if (thermostats == null)
            {
                return new NotFoundResult();
            }

            return new ActionResult<IEnumerable<Thermostat>>(thermostats);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Thermostat>> GetAsync(Guid house_id, Guid room_id, Guid id)
        {
            Thermostat thermostat = await _mediator.Send(new ThermostatByIdQuery(_identity.Email, house_id,
                room_id, id));
            if (thermostat == null)
            {
                return new NotFoundResult();
            }

            return thermostat;
        }

        [HttpPost]
        public async Task<ActionResult<Thermostat>> PostAsync(Guid house_id, Guid room_id,
            [FromBody] Thermostat thermostat)
        {
            try
            {
                Thermostat newThermostat = await _mediator.Send(new AddThermostatCommand(_identity.Email,
                    house_id, room_id, thermostat));
                if (newThermostat == null)
                {
                    return new NotFoundResult();
                }

                return new CreatedResult($"houses/{house_id}/rooms/{room_id}/thermostats", newThermostat);
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Thermostat>> UpdateAsync(Guid house_id, Guid room_id, Guid id,
            [FromBody] JsonPatchDocument<ThermostatPatch> patch)
        {
            try
            {
                Thermostat thermostat = await _mediator.Send(new UpdateThermostatCommand(_identity.Email,
                    house_id, room_id, id, patch));
                if (thermostat == null)
                {
                    return new NotFoundResult();
                }

                return thermostat;
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid house_id, Guid room_id, Guid id)
        {
            try
            {
                Thermostat thermostat = await _mediator.Send(new DeleteThermostatCommand(_identity.Email,
                    house_id, room_id, id));
                if (thermostat == null)
                {
                    return new NotFoundResult();
                }

                return new NoContentResult();
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}