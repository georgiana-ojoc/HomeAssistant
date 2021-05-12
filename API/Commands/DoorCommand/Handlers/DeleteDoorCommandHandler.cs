using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Door.Handlers
{
    public class DeleteDoorCommandHandler : Handler, IRequestHandler<DeleteDoorCommand, Shared.Models.Door>
    {
        private readonly IDoorRepository _repository;

        public DeleteDoorCommandHandler(Identity identity, IDoorRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Door> Handle(DeleteDoorCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteDoorAsync(Identity.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}