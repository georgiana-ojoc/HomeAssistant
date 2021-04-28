using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Room.Handlers
{
    public class PartialUpdateRoomCommandHandler : Handler,
        IRequestHandler<PartialUpdateRoomCommand, Shared.Models.Room>
    {
        private readonly IRoomRepository _repository;

        public PartialUpdateRoomCommandHandler(Identity identity, IRoomRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Room> Handle(PartialUpdateRoomCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateRoomAsync(Identity.Email, request.HouseId, request.Id, request.Patch);
        }
    }
}