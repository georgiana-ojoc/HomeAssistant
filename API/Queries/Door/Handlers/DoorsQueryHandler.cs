using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Door.Handlers
{
    public class DoorsHandler : IRequestHandler<DoorsQuery, IEnumerable<Shared.Models.Door>>
    {
        private readonly IDoorRepository _repository;

        public DoorsHandler(IDoorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Shared.Models.Door>> Handle(DoorsQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetDoorsAsync(request.UserId,
                request.HouseId, request.RoomId);
            return result;
        }
    }
}