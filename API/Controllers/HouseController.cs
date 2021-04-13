using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.House;
using API.Models;
using API.Queries.House;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("users/{user_id}/houses")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class HouseController
    {
        private readonly IMediator _mediator;

        public HouseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<House>>> Get(int user_id)
        {
            var result = await _mediator.Send(new HousesQuery(user_id));
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new ActionResult<IEnumerable<House>>(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<House>> Get(int user_id, int id)
        {
            House house = await _mediator.Send(new HouseById(user_id, id));
            if (house == null)
            {
                return new NotFoundResult();
            }

            return house;
        }

        [HttpPost]
        public async Task<ActionResult<House>> Post(int user_id, [FromBody] House house)
        {
            try
            {
                House newHouse = await _mediator.Send(new AddHouse(user_id, house));
                if (newHouse == null)
                {
                    return new NotFoundResult();
                }

                return newHouse;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int user_id, int id)
        {
            try
            {
                House house = await _mediator.Send(new DeleteHouse(user_id, id));
                if (house == null)
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