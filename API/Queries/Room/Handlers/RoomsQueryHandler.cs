using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Room.Handlers
{
    public class RoomsQueryHandler : IRequestHandler<RoomsQuery, IEnumerable<Shared.Models.Room>>
    {
        private readonly IRoomRepository _repository;

        public RoomsQueryHandler(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Shared.Models.Room>> Handle(RoomsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetRoomsAsync(request.Email, request.HouseId);
        }
    }
}