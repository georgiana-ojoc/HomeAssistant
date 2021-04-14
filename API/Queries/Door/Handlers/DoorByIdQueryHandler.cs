using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Door.Handlers
{
    public class DoorByIdQueryHandler : IRequestHandler<DoorByIdQuery, Shared.Models.Door>
    {
        private readonly IDoorRepository _repository;

        public DoorByIdQueryHandler(IDoorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Door> Handle(DoorByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetDoorByIdAsync(request.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}