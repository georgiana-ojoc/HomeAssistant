using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.House;
using API.Queries.House;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Requests;

namespace API.Controllers
{
    [ApiController]
    [Route("houses")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class HouseController : BaseController
    {
        public HouseController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<House>>> GetAsync()
        {
            try
            {
                return Ok(await Mediator.Send(new GetHousesQuery()));
            }
            catch (ArgumentNullException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<House>> GetAsync(Guid id)
        {
            try
            {
                House house = await Mediator.Send(new GetHouseByIdQuery {Id = id});
                if (house == null)
                {
                    return NotFound();
                }

                return Ok(house);
            }
            catch (ArgumentNullException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<House>> PostAsync([FromBody] HouseRequest request)
        {
            try
            {
                House newHouse = await Mediator.Send(new CreateHouseCommand {Request = request});
                return Created($"/houses/{newHouse.Id}", newHouse);
            }
            catch (ArgumentNullException exception)
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
        public async Task<ActionResult<House>> PatchAsync(Guid id, [FromBody] JsonPatchDocument<HouseRequest> patch)
        {
            try
            {
                House house = await Mediator.Send(new PartialUpdateHouseCommand {Id = id, Patch = patch});
                if (house == null)
                {
                    return NotFound();
                }

                return Ok(house);
            }
            catch (ArgumentNullException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                House house = await Mediator.Send(new DeleteHouseCommand {Id = id});
                if (house == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ArgumentNullException exception)
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