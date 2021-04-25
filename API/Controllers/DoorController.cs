using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.Door;
using API.Queries.Door;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Patch;

namespace API.Controllers
{
    [ApiController]
    [Route("houses/{house_id}/rooms/{room_id}/doors")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DoorController
    {
        private readonly Identity _identity;
        private readonly IMediator _mediator;

        public DoorController(Identity identity, IMediator mediator)
        {
            _identity = identity;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Door>>> GetAsync(Guid house_id, Guid room_id)
        {
            IEnumerable<Door> doors = await _mediator.Send(new DoorsQuery(_identity.Email, house_id, room_id));
            if (doors == null)
            {
                return new NotFoundResult();
            }

            return new ActionResult<IEnumerable<Door>>(doors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Door>> GetAsync(Guid house_id, Guid room_id, Guid id)
        {
            Door door = await _mediator.Send(new DoorByIdQuery(_identity.Email, house_id, room_id, id));
            if (door == null)
            {
                return new NotFoundResult();
            }

            return door;
        }

        [HttpPost]
        public async Task<ActionResult<Door>> PatchAsync(Guid house_id, Guid room_id, [FromBody] Door door)
        {
            try
            {
                Door newDoor = await _mediator.Send(new AddDoorCommand(_identity.Email, house_id, room_id, door));
                if (newDoor == null)
                {
                    return new NotFoundResult();
                }

                return new CreatedResult($"houses/{house_id}/rooms/{room_id}/doors", newDoor);
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Door>> UpdateAsync(Guid house_id, Guid room_id, Guid id,
            [FromBody] JsonPatchDocument<DoorPatch> patch)
        {
            try
            {
                Door door = await _mediator.Send(new UpdateDoorCommand(_identity.Email, house_id, room_id, id,
                    patch));
                if (door == null)
                {
                    return new NotFoundResult();
                }

                return door;
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
                Door door = await _mediator.Send(new DeleteDoorCommand(_identity.Email, house_id, room_id, id));
                if (door == null)
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