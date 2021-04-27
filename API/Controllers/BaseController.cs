using MediatR;

namespace API.Controllers
{
    public abstract class BaseController
    {
        protected readonly Identity Identity;
        protected readonly IMediator Mediator;

        protected BaseController(Identity identity, IMediator mediator)
        {
            Identity = identity;
            Mediator = mediator;
        }
    }
}