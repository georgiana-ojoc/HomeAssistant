using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Door.Handlers
{
    public class GetDoorsHandler : Handler, IRequestHandler<GetDoorsQuery, IEnumerable<Shared.Models.Door>>
    {
        private readonly IDoorRepository _repository;

        public GetDoorsHandler(Identity identity, IDoorRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Shared.Models.Door>> Handle(GetDoorsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetDoorsAsync(Identity.Email, request.HouseId, request.RoomId);
        }
    }
}