using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands;
using API.Models;
using API.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("users/{user_id}/houses/{house_id}/rooms/{room_id}/doors")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DoorController
    {
        private readonly IMediator _mediator;

        public DoorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Door>>> Get(int user_id, int house_id, int room_id)
        {
            var result = await _mediator.Send(new DoorsQuery(user_id, house_id, room_id));
            return result == null ? new NotFoundResult() : new ActionResult<IEnumerable<Door>>(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Door>> Get(int user_id, int house_id, int room_id, int id)
        {
            Door door = await _mediator.Send(new DoorById(user_id, house_id, room_id, id));
            if (door == null)
                return new NotFoundResult();
            return door;
        }

        [HttpPost]
        public async Task<ActionResult<Door>> Post(int user_id, int house_id, int room_id, [FromBody] Door door)
        {
            try
            {
                Door addedDoor = await _mediator.Send(new AddDoor(user_id, house_id, room_id, door));
                if (addedDoor == null)
                    return new NotFoundResult();
                return addedDoor;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int user_id, int house_id, int room_id, int id)
        {
            try
            {
                Door door = await _mediator.Send(new DeleteDoor(user_id, house_id, room_id, id));
                if (door == null)
                    return new NotFoundResult();
                return new NoContentResult();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}