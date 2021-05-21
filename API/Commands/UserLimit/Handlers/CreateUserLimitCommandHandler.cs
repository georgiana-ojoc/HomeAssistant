using System.Threading;
using System.Threading.Tasks;
using API.Commands.UserLimit;
using API.Interfaces;
using MediatR;

namespace API.Commands.UserLimitCommand.Handlers
{
    public class CreateUserLimitCommandHandler : Handler, IRequestHandler<CreateUserLimitCommand, Shared.Models.UserLimit>
    {
        private readonly IUserLimitRepository _repository;

        public CreateUserLimitCommandHandler(Identity identity, IUserLimitRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.UserLimit> Handle(CreateUserLimitCommand request, CancellationToken cancellationToken)
        {
            return await _repository.CreateUserLimitAsync(Identity.Email, new Shared.Models.UserLimit
            {
                HouseLimit = request.Request.HouseLimit,
                RoomLimit = request.Request.RoomLimit,
                LightBulbLimit = request.Request.LightBulbLimit,
                DoorLimit = request.Request.DoorLimit,
                ThermostatLimit = request.Request.ThermostatLimit,
            });
        }
    }
}