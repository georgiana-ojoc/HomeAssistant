using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Door.Handlers
{
    public class CreateDoorCommandHandler : Handler, IRequestHandler<CreateDoorCommand, Shared.Models.Door>
    {
        private readonly IDoorRepository _repository;

        public CreateDoorCommandHandler(Identity identity, IDoorRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Door> Handle(CreateDoorCommand request, CancellationToken cancellationToken)
        {
            return await _repository.CreateDoorAsync(Identity.Email, request.HouseId, request.RoomId,
                new Shared.Models.Door
                {
                    Name = request.Request.Name,
                    Locked = request.Request.Locked
                });
        }
    }
}