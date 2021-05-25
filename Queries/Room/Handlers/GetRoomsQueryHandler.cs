using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.Room.Handlers
{
    public class GetRoomsQueryHandler : Handler, IRequestHandler<GetRoomsQuery, IEnumerable<Models.Room>>
    {
        private readonly IRoomRepository _repository;

        public GetRoomsQueryHandler(Identity identity, IRoomRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Models.Room>> Handle(GetRoomsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetRoomsAsync(Identity.Email, request.HouseId);
        }
    }
}