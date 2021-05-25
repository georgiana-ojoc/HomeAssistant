using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Door.Handlers
{
    public class GetDoorByIdQueryHandler : Handler, IRequestHandler<GetDoorByIdQuery, Models.Door>
    {
        private readonly IDoorRepository _repository;

        public GetDoorByIdQueryHandler(Identity identity, IDoorRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Door> Handle(GetDoorByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetDoorByIdAsync(Identity.Email, request.HouseId, request.RoomId, request.Id);
        }
    }
}