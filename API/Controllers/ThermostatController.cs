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
    public class ThermostatController : BaseController
    {
        public ThermostatController(Identity identity, IMediator mediator) : base(identity, mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Thermostat>>> GetAsync(Guid house_id, Guid room_id)
        {
            IEnumerable<Thermostat> thermostats = await Mediator.Send(new ThermostatsQuery(Identity.Email,
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
            Thermostat thermostat = await Mediator.Send(new ThermostatByIdQuery(Identity.Email, house_id,
                room_id, id));
            if (thermostat == null)
            {
                return new NotFoundResult();
            }

            return thermostat;
        }

        [HttpPost]
        public async Task<ActionResult<Thermostat>> PatchAsync(Guid house_id, Guid room_id,
            [FromBody] Thermostat thermostat)
        {
            try
            {
                Thermostat newThermostat = await Mediator.Send(new AddThermostatCommand(Identity.Email,
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
                Thermostat thermostat = await Mediator.Send(new UpdateThermostatCommand(Identity.Email,
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
                Thermostat thermostat = await Mediator.Send(new DeleteThermostatCommand(Identity.Email,
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