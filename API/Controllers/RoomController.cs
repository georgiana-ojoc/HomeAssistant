using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.Room;
using API.Models;
using API.Queries.Room;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("users/{user_id}/houses/{house_id}/rooms")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RoomController
    {
        private readonly IMediator _mediator;

        public RoomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> Get(int user_id, int house_id)
        {
            IEnumerable<Room> rooms = await _mediator.Send(new RoomsQuery(user_id, house_id));
            if (rooms == null)
            {
                return new NotFoundResult();
            }

            return new ActionResult<IEnumerable<Room>>(rooms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> Get(int user_id, int house_id, int id)
        {
            Room room = await _mediator.Send(new RoomById(user_id, house_id, id));
            if (room == null)
            {
                return new NotFoundResult();
            }

            return room;
        }

        [HttpPost]
        public async Task<ActionResult<Room>> Post(int user_id, int house_id, [FromBody] Room room)
        {
            try
            {
                Room newRoom = await _mediator.Send(new AddRoom(user_id, house_id, room));
                if (newRoom == null)
                {
                    return new NotFoundResult();
                }

                return newRoom;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int user_id, int house_id, int id)
        {
            try
            {
                Room room = await _mediator.Send(new DeleteRoom(user_id, house_id, id));
                if (room == null)
                {
                    return new NotFoundResult();
                }

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