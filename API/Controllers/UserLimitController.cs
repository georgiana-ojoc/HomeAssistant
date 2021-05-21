using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.UserLimit;
using API.Commands.UserLimitCommand;
using API.Queries.UserLimit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Requests;

namespace API.Controllers
{
    [ApiController]
    [Route("userLimits")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UserLimitController : BaseController
    {
        public UserLimitController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLimit>>> GetAsync()
        {
            try
            {
                return Ok(await Mediator.Send(new GetUserLimitsQuery()));
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserLimit>> GetAsync(Guid id)
        {
            try
            {
                UserLimit userLimit = await Mediator.Send(new GetUserLimitByIdQuery {Id = id});
                if (userLimit == null)
                {
                    return NotFound();
                }

                return Ok(userLimit);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserLimit>> PostAsync([FromBody] UserLimitRequest request)
        {
            try
            {
                UserLimit newUserLimit = await Mediator.Send(new CreateUserLimitCommand {Request = request});
                return Created($"/userLimits/{newUserLimit.Id}", newUserLimit);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (ConstraintException)
            {
                return Forbid();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<UserLimit>> PatchAsync(Guid id, [FromBody] JsonPatchDocument<UserLimitRequest> patch)
        {
            try
            {
                UserLimit userLimit = await Mediator.Send(new PartialUpdateUserLimitCommand {Id = id, Patch = patch});
                if (userLimit == null)
                {
                    return NotFound();
                }

                return Ok(userLimit);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
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
                UserLimit userLimit = await Mediator.Send(new DeleteUserLimitCommand {Id = id});
                if (userLimit == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}