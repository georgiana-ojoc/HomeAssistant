using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using API.Commands.Schedule;
using API.Queries.Schedule;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Requests;

namespace API.Controllers
{
    [ApiController]
    [Route("schedules")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ScheduleController : BaseController
    {
        public ScheduleController(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetAsync()
        {
            try
            {
                return Ok(await Mediator.Send(new GetSchedulesQuery()));
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
        public async Task<ActionResult<Schedule>> GetAsync(Guid id)
        {
            try
            {
                Schedule schedule = await Mediator.Send(new GetScheduleByIdQuery {Id = id});
                if (schedule == null)
                {
                    return NotFound();
                }

                return Ok(schedule);
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
        public async Task<ActionResult<Schedule>> PostAsync([FromBody] ScheduleRequest request)
        {
            try
            {
                Schedule newSchedule = await Mediator.Send(new CreateScheduleCommand {Request = request});
                return Created($"/schedules/{newSchedule.Id}", newSchedule);
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
        public async Task<ActionResult<Schedule>> PatchAsync(Guid id, [FromBody] JsonPatchDocument<ScheduleRequest> patch)
        {
            try
            {
                Schedule schedule = await Mediator.Send(new PartialUpdateScheduleCommand {Id = id, Patch = patch});
                if (schedule == null)
                {
                    return NotFound();
                }

                return Ok(schedule);
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
                Schedule schedule = await Mediator.Send(new DeleteScheduleCommand {Id = id});
                if (schedule == null)
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