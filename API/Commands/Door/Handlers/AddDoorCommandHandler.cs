using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Door.Handlers
{
    public class AddDoorCommandHandler : IRequestHandler<AddDoorCommand, Shared.Models.Door>
    {
        private readonly IDoorRepository _repository;

        public AddDoorCommandHandler(IDoorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Door> Handle(AddDoorCommand request, CancellationToken cancellationToken)
        {
            return await _repository.CreateDoorAsync(request.Email, request.HouseId, request.RoomId, request.Door);
        }
    }
}