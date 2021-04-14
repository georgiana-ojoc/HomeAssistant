using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Room.Handlers
{
    public class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, Shared.Models.Room>
    {
        private readonly IRoomRepository _repository;

        public AddRoomCommandHandler(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Room> Handle(AddRoomCommand request, CancellationToken cancellationToken)
        {
            return await _repository.CreateRoomAsync(request.Email, request.HouseId, request.Room);
        }
    }
}