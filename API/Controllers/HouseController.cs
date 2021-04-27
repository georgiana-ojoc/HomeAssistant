using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.House;
using API.Queries.House;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.Patch;

namespace API.Controllers
{
    [ApiController]
    [Route("houses")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class HouseController : BaseController
    {
        public HouseController(Identity identity, IMediator mediator) : base(identity, mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<House>>> GetAsync()
        {
            IEnumerable<House> houses = await Mediator.Send(new HousesQuery(Identity.Email));
            if (houses == null)
            {
                return new NotFoundResult();
            }

            return new ActionResult<IEnumerable<House>>(houses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<House>> GetAsync(Guid id)
        {
            House house = await Mediator.Send(new HouseByIdQuery(Identity.Email, id));
            if (house == null)
            {
                return new NotFoundResult();
            }

            return house;
        }

        [HttpPost]
        public async Task<ActionResult<House>> PostAsync([FromBody] House house)
        {
            try
            {
                House newHouse = await Mediator.Send(new AddHouseCommand(Identity.Email, house));
                if (newHouse == null)
                {
                    return new NotFoundResult();
                }

                return new CreatedResult("houses", newHouse);
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<House>> PatchAsync(Guid id, [FromBody] JsonPatchDocument<HousePatch> patch)
        {
            try
            {
                House house = await Mediator.Send(new UpdateHouseCommand(Identity.Email, id, patch));
                if (house == null)
                {
                    return new NotFoundResult();
                }

                return house;
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                House house = await Mediator.Send(new DeleteHouseCommand(Identity.Email, id));
                if (house == null)
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