using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.Door.Handlers
{
    public class GetDoorsHandler : Handler, IRequestHandler<GetDoorsQuery, IEnumerable<Models.Door>>
    {
        private readonly IDoorRepository _repository;

        public GetDoorsHandler(Identity identity, IDoorRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Models.Door>> Handle(GetDoorsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetDoorsAsync(Identity.Email, request.HouseId, request.RoomId);
        }
    }
}