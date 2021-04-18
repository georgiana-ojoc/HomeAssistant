using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.Room;
using API.Queries.Room;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("houses/{house_id}/rooms")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RoomController
    {
        private readonly Identity _identity;
        private readonly IMediator _mediator;

        public RoomController(Identity identity, IMediator mediator)
        {
            _identity = identity;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAsync(Guid house_id)
        {
            IEnumerable<Room> rooms = await _mediator.Send(new RoomsQuery(_identity.Email, house_id));
            if (rooms == null)
            {
                return new NotFoundResult();
            }

            return new ActionResult<IEnumerable<Room>>(rooms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetAsync(Guid house_id, Guid id)
        {
            Room room = await _mediator.Send(new RoomByIdQuery(_identity.Email, house_id, id));
            if (room == null)
            {
                return new NotFoundResult();
            }

            return room;
        }

        [HttpPost]
        public async Task<ActionResult<Room>> Post(Guid house_id, [FromBody] Room room)
        {
            try
            {
                Room newRoom = await _mediator.Send(new AddRoomCommand(_identity.Email, house_id, room));
                if (newRoom == null)
                {
                    return new NotFoundResult();
                }

                return new CreatedResult($"houses/{house_id}/rooms", newRoom);
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid house_id, Guid id)
        {
            try
            {
                Room room = await _mediator.Send(new DeleteRoomCommand(_identity.Email, house_id, id));
                if (room == null)
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