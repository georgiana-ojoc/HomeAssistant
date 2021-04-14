using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Door.Handlers
{
    public class DeleteDoorCommandHandler : IRequestHandler<DeleteDoorCommand, Shared.Models.Door>
    {
        private readonly IDoorRepository _repository;

        public DeleteDoorCommandHandler(IDoorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Door> Handle(DeleteDoorCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteDoorAsync(request.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}