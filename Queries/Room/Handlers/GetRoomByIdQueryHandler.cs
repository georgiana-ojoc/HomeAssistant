using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.Room.Handlers
{
    public class GetRoomByIdQueryHandler : Handler, IRequestHandler<GetRoomByIdQuery, Models.Room>
    {
        private readonly IRoomRepository _repository;

        public GetRoomByIdQueryHandler(Identity identity, IRoomRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Room> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetRoomByIdAsync(Identity.Email, request.HouseId, request.Id);
        }
    }
}