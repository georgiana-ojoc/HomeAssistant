using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Subscription.Handlers
{
    public class CreateSubscriptionCommandHandler : Handler, IRequestHandler<CreateSubscriptionCommand,
        Models.Subscription>
    {
        private readonly ISubscriptionRepository _repository;

        public CreateSubscriptionCommandHandler(Identity identity, ISubscriptionRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Subscription> Handle(CreateSubscriptionCommand request, CancellationToken
            cancellationToken)
        {
            CheckEmail();

            return await _repository.CreateSubscriptionAsync(new Models.Subscription
            {
                Name = request.Request.Name,
                Description = request.Request.Description,
                Price = request.Request.Price,
                Houses = request.Request.Houses,
                Rooms = request.Request.Rooms,
                LightBulbs = request.Request.LightBulbs,
                Doors = request.Request.Doors,
                Thermostats = request.Request.Thermostats,
                Schedules = request.Request.Schedules,
                LightBulbCommands = request.Request.LightBulbCommands,
                DoorCommands = request.Request.DoorCommands,
                ThermostatCommands = request.Request.ThermostatCommands
            });
        }
    }
}