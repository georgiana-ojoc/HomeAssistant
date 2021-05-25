using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Room.Handlers
{
    public class CreateRoomCommandHandler : Handler, IRequestHandler<CreateRoomCommand, Models.Room>
    {
        private readonly IRoomRepository _repository;

        public CreateRoomCommandHandler(Identity identity, IRoomRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Room> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            return await _repository.CreateRoomAsync(Identity.Email, request.HouseId, new Models.Room
            {
                Name = request.Request.Name
            });
        }
    }
}