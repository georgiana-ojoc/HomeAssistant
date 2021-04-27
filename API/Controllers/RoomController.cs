﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.Room;
using API.Queries.Room;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Patch;

namespace API.Controllers
{
    [ApiController]
    [Route("houses/{house_id}/rooms")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RoomController : BaseController
    {
        public RoomController(Identity identity, IMediator mediator) : base(identity, mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAsync(Guid house_id)
        {
            IEnumerable<Room> rooms = await Mediator.Send(new RoomsQuery(Identity.Email, house_id));
            if (rooms == null)
            {
                return new NotFoundResult();
            }

            return new ActionResult<IEnumerable<Room>>(rooms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetAsync(Guid house_id, Guid id)
        {
            Room room = await Mediator.Send(new RoomByIdQuery(Identity.Email, house_id, id));
            if (room == null)
            {
                return new NotFoundResult();
            }

            return room;
        }

        [HttpPost]
        public async Task<ActionResult<Room>> PostAsync(Guid house_id, [FromBody] Room room)
        {
            try
            {
                Room newRoom = await Mediator.Send(new AddRoomCommand(Identity.Email, house_id, room));
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

        [HttpPatch("{id}")]
        public async Task<ActionResult<Room>> PatchAsync(Guid house_id, Guid id,
            [FromBody] JsonPatchDocument<RoomPatch> patch)
        {
            try
            {
                Room room = await Mediator.Send(new UpdateRoomCommand(Identity.Email, house_id, id, patch));
                if (room == null)
                {
                    return new NotFoundResult();
                }

                return room;
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid house_id, Guid id)
        {
            try
            {
                Room room = await Mediator.Send(new DeleteRoomCommand(Identity.Email, house_id, id));
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