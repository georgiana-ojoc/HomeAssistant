using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeAssistantAPI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IMediator Mediator;

        protected BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}